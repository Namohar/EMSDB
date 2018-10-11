using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.Drawing;

public partial class EstimateReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());

    SqlDataAdapter da;
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblid.Text = Request.QueryString["TID"];
            LoadEmp();
            lblTotal.Text = "00:00:00";
        }
    }
    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            if (txtDate.Text.Length <= 0)
            {
                lblError.Text = "Please select Date";
                return;
            }
            DisplayRecords();
        }
        catch (Exception)
        {


        }
    }

    private void LoadEmp()
    {
        dt = new DataTable();
        if (Session["access"].ToString() == "1")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_Team_Id ='" + lblid.Text + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        }
        else if (Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("select * from RTM_User_List where UL_Team_Id ='" + lblid.Text + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        }
        else if (Session["access"].ToString() == "3")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_Team_Id ='" + lblid.Text + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        }
        else if (Session["access"].ToString() == "4")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_System_User_Name='" + Session["user"].ToString() + "' ORDER BY UL_User_Name", con);
        }

        da.Fill(dt);

        ddlEmp.DataSource = dt;
        ddlEmp.DataValueField = "UL_ID";
        ddlEmp.DataTextField = "UL_User_Name";
        ddlEmp.DataBind();
        if (Session["access"].ToString() != "4")
        {
            ddlEmp.Items.Insert(0, "All");
            ddlEmp.SelectedIndex = 0;
        }

    }

    private void DisplayRecords()
    {
        dt = new DataTable();

        if (ddlEmp.SelectedItem.Text == "All")
        {
            da = new SqlDataAdapter("select EST_UserName, CL_ClientName, TL_Task, STL_SubTask, EST_Duration, CONVERT(VARCHAR(10),EST_Date,101) as EST_Date from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID " +
         "left join rtm_subtask_list on EST_SubTaskId = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) = '" + txtDate.Text + "' and EST_TeamId='" + lblid.Text + "' Order By EST_UserName", con);
        }
        else
        {
            da = new SqlDataAdapter("select EST_UserName, CL_ClientName, TL_Task, STL_SubTask, EST_Duration, CONVERT(VARCHAR(10),EST_Date,101) as EST_Date from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID " +
         "left join rtm_subtask_list on EST_SubTaskId = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) = '" + txtDate.Text + "' and EST_UserName ='" + ddlEmp.SelectedItem.Text + "' and EST_TeamId='" + lblid.Text + "' Order By EST_UserName", con);
        }

        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            gvEstimate.DataSource = dt;
            gvEstimate.DataBind();
            GetTotal();
        }
        else
        {
            gvEstimate.DataSource = null;
            gvEstimate.DataBind();
            lblTotal.Text = "00:00:00";
        }
    }

    private void GetTotal()
    {
        dt = new DataTable();
        if (ddlEmp.SelectedItem.Text  == "All")
        {
            da = new SqlDataAdapter("select Convert(varchar(4), sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/3600) +':'+ Convert(varchar(4),(sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/60)%60) +':'+ Convert(varchar(4),(sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/60)%60)))%60) as Duration from RTM_Estimation where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) = '" + txtDate.Text + "' and EST_TeamId='" + lblid.Text + "'", con);
        }
        else
        {
            da = new SqlDataAdapter("select Convert(varchar(4), sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/3600) +':'+ Convert(varchar(4),(sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/60)%60) +':'+ Convert(varchar(4),(sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/60)%60)))%60) as Duration from RTM_Estimation where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) = '" + txtDate.Text + "' and EST_UserName ='" + ddlEmp.SelectedItem.Text + "' and EST_TeamId='" + lblid.Text + "'", con);
        }
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            lblTotal.Text = dt.Rows[0]["Duration"].ToString();
        }
    }
}