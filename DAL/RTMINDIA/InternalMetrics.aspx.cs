using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using BAL;

public partial class InternalMetrics : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    SqlDataAdapter da;
    DataTable dt = new DataTable();
    clsInternalMetrics objMetrics = new clsInternalMetrics();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void btnGet_Click(object sender, EventArgs e)
    {
        if (txtFrom.Text.Length <= 0)
        {
            lblError.Text = "Please select From date";
            return;
        }
        if (txtTo.Text.Length <= 0)
        {
            lblError.Text = "Please select To date";
            return;
        }

        if (Convert.ToDateTime(txtFrom.Text) > Convert.ToDateTime(txtTo.Text))
        {
            lblError.Text = "From date should be lesser than To date";
            return;
        }
        FetchallTeams();
        FetchallClients();
        FetchallUsers();
        FetchallTasks();
        dropDiv.Visible = true;
        lblSelect.Text = "0";
    }

    private void FetchallTeams()
    {
        dt = new DataTable();
        dt = objMetrics.fetchAllTeams(txtFrom.Text, txtTo.Text);
        //da = new SqlDataAdapter("select T_ID, T_TeamName from RTM_Records left join RTM_Team_List on R_TeamId = T_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '"+ txtFrom.Text +"' and '"+ txtTo.Text +"' Group By T_ID, T_TeamName order by T_TeamName", con);
        //da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            ddlTeam.DataSource = dt;
            ddlTeam.DataValueField = "T_ID";
            ddlTeam.DataTextField = "T_TeamName";
            ddlTeam.DataBind();
            ddlTeam.Items.Insert(0, "--Select Team--");
            ddlTeam.SelectedIndex = 0;
        }
    }

    private void FetchallClients()
    {
        dt = new DataTable();
        dt = objMetrics.fetchAllClients(txtFrom.Text, txtTo.Text);
        //da = new SqlDataAdapter("select Distinct CL_ClientName from RTM_Records left join RTM_Client_List on R_Client = CL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '"+ txtFrom.Text +"' and '"+ txtTo.Text +"' order by CL_ClientName", con);
        //da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            ddlClient.DataSource = dt;
            ddlClient.DataValueField = "CL_ClientName";
            ddlClient.DataTextField = "CL_ClientName";
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, "--Select Client--");
            ddlClient.SelectedIndex = 0;
        }
    }

    private void FetchallUsers()
    {
        dt = new DataTable();
        dt = objMetrics.fetchAllUsers(txtFrom.Text, txtTo.Text);
        //da = new SqlDataAdapter("select Distinct R_User_Name from RTM_Records where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '"+ txtFrom.Text +"' and '"+ txtTo.Text +"' order by R_User_Name", con);
        //da.Fill(dt);

        if (dt.Rows.Count > 0)
        {

            ddlUsers.DataSource = dt;
            ddlUsers.DataValueField = "R_User_Name";
            ddlUsers.DataTextField = "R_User_Name";
            ddlUsers.DataBind();
            ddlUsers.Items.Insert(0, "--Select User--");
            ddlUsers.SelectedIndex = 0;
        }
    }

    private void FetchallTasks()
    {
        dt = new DataTable();
        dt = objMetrics.fetchAllTasks(txtFrom.Text, txtTo.Text);
        //da = new SqlDataAdapter("select Distinct STL_ServiceCode from RTM_Records left join RTM_SubTask_List on R_SubTask = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '"+ txtFrom.Text +"' and '"+ txtTo.Text +"' order by STL_ServiceCode", con);
        //da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            ddlTask.DataSource = dt;
            ddlTask.DataValueField = "STL_ServiceCode";
            ddlTask.DataTextField = "STL_ServiceCode";
            ddlTask.DataBind();
            ddlTask.Items.Insert(0, "--Select Task--");
            ddlTask.SelectedIndex = 0;
        }
    }



    protected void ddlTeam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblSelect.Text == "0")
        {
            lblSelect.Text = "Team";
        }

        if (lblSelect.Text == "Team")
        {
            ddlClient.SelectedIndex = 0;
            ddlUsers.SelectedIndex = 0;
            ddlTask.SelectedIndex = 0;
            // ddlTeam.SelectedIndex = 0;
        }

        if (ddlClient.SelectedIndex == 0)
        {
            FetchClients();
        }
        if (ddlUsers.SelectedIndex == 0)
        {
            FetchUsers();
        }
        if (ddlTask.SelectedIndex == 0)
        {
            FetchTasks();
        }
    }

    protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblSelect.Text == "0")
        {
            lblSelect.Text = "Client";
        }

        if (lblSelect.Text == "Client")
        {
            //ddlClient.SelectedIndex = 0;
            ddlUsers.SelectedIndex = 0;
            ddlTask.SelectedIndex = 0;
            ddlTeam.SelectedIndex = 0;
        }

        if (ddlTeam.SelectedIndex == 0)
        {
            FetchTeams();
        }
        if (ddlUsers.SelectedIndex == 0)
        {
            FetchUsers();
        }
        if (ddlTask.SelectedIndex == 0)
        {
            FetchTasks();
        }


    }

    protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblSelect.Text == "0")
        {
            lblSelect.Text = "User";
        }

        if (lblSelect.Text == "User")
        {
            ddlClient.SelectedIndex = 0;
            // ddlUsers.SelectedIndex = 0;
            ddlTask.SelectedIndex = 0;
            ddlTeam.SelectedIndex = 0;
        }

        if (ddlClient.SelectedIndex == 0)
        {
            FetchClients();
        }
        if (ddlTeam.SelectedIndex == 0)
        {
            FetchTeams();
        }
        if (ddlTask.SelectedIndex == 0)
        {
            FetchTasks();
        }


    }

    protected void ddlTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lblSelect.Text == "0")
        {
            lblSelect.Text = "Task";
        }

        if (lblSelect.Text == "Task")
        {
            ddlClient.SelectedIndex = 0;
            ddlUsers.SelectedIndex = 0;
            // ddlTask.SelectedIndex = 0;
            ddlTeam.SelectedIndex = 0;
        }

        if (ddlClient.SelectedIndex == 0)
        {
            FetchClients();
        }
        if (ddlUsers.SelectedIndex == 0)
        {
            FetchUsers();
        }
        if (ddlTeam.SelectedIndex == 0)
        {
            FetchTeams();
        }


    }

    private void FetchTeams()
    {

        StringBuilder sb = new StringBuilder();
        if (ddlClient.SelectedIndex != 0)
        {
            sb.Append(" and CL_ClientName='" + ddlClient.SelectedItem.Text + "'");
        }
        if (ddlUsers.SelectedIndex != 0)
        {
            sb.Append(" and R_User_Name='" + ddlUsers.SelectedItem.Text + "'");
        }
        if (ddlTask.SelectedIndex != 0)
        {
            sb.Append(" and STL_ServiceCode='" + ddlTask.SelectedItem.Text + "'");
        }
        dt = new DataTable();
        //da = new SqlDataAdapter("select T_ID, T_TeamName from RTM_Records left join RTM_Team_List on R_TeamId = T_ID left join RTM_Client_List on R_Client = CL_ID left join RTM_SubTask_List on R_SubTask = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '"+ txtFrom.Text +"' and '"+ txtTo.Text +"' "+ sb.ToString() +" Group By T_ID, T_TeamName order by T_TeamName", con);
        //da.Fill(dt);

        dt = objMetrics.fetchTeams(txtFrom.Text, txtTo.Text, sb);

        if (dt.Rows.Count > 0)
        {
            ddlTeam.DataSource = dt;
            ddlTeam.DataValueField = "T_ID";
            ddlTeam.DataTextField = "T_TeamName";
            ddlTeam.DataBind();
            ddlTeam.Items.Insert(0, "--Select Team--");
            ddlTeam.SelectedIndex = 0;
        }
    }

    private void FetchClients()
    {

        StringBuilder sb = new StringBuilder();
        if (ddlTeam.SelectedIndex != 0)
        {
            sb.Append(" and T_TeamName='" + ddlTeam.SelectedItem.Text + "'");
        }
        if (ddlUsers.SelectedIndex != 0)
        {
            sb.Append(" and R_User_Name='" + ddlUsers.SelectedItem.Text + "'");
        }
        if (ddlTask.SelectedIndex != 0)
        {
            sb.Append(" and STL_ServiceCode='" + ddlTask.SelectedItem.Text + "'");
        }

        dt = new DataTable();
        //da = new SqlDataAdapter("select Distinct CL_ClientName from RTM_Records left join RTM_Client_List on R_Client = CL_ID left join RTM_Team_List on R_TeamId = T_ID left join RTM_SubTask_List on R_SubTask = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + txtFrom.Text + "' and '" + txtTo.Text + "' "+ sb.ToString() +" order by CL_ClientName", con);
        //da.Fill(dt);

        dt = objMetrics.fetchClients(txtFrom.Text, txtTo.Text, sb);

        if (dt.Rows.Count > 0)
        {
            ddlClient.DataSource = dt;
            ddlClient.DataValueField = "CL_ClientName";
            ddlClient.DataTextField = "CL_ClientName";
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, "--Select Client--");
            ddlClient.SelectedIndex = 0;
        }
    }

    private void FetchUsers()
    {

        StringBuilder sb = new StringBuilder();
        if (ddlClient.SelectedIndex != 0)
        {
            sb.Append(" and CL_ClientName='" + ddlClient.SelectedItem.Text + "'");
        }
        if (ddlTeam.SelectedIndex != 0)
        {
            sb.Append(" and T_TeamName='" + ddlTeam.SelectedItem.Text + "'");
        }
        if (ddlTask.SelectedIndex != 0)
        {
            sb.Append(" and STL_ServiceCode='" + ddlTask.SelectedItem.Text + "'");
        }

        dt = new DataTable();
        //da = new SqlDataAdapter("select Distinct R_User_Name from RTM_Records left join RTM_Team_List on R_TeamId = T_ID left join RTM_Client_List on R_Client = CL_ID left join RTM_SubTask_List on R_SubTask = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + txtFrom.Text + "' and '" + txtTo.Text + "' "+ sb.ToString() +" order by R_User_Name", con);
        //da.Fill(dt);

        dt = objMetrics.fetchUsers(txtFrom.Text, txtTo.Text, sb);

        if (dt.Rows.Count > 0)
        {
            ddlUsers.DataSource = dt;
            ddlUsers.DataValueField = "R_User_Name";
            ddlUsers.DataTextField = "R_User_Name";
            ddlUsers.DataBind();
            ddlUsers.Items.Insert(0, "--Select User--");
            ddlUsers.SelectedIndex = 0;
        }
    }

    private void FetchTasks()
    {

        StringBuilder sb = new StringBuilder();
        if (ddlClient.SelectedIndex != 0)
        {
            sb.Append(" and CL_ClientName='" + ddlClient.SelectedItem.Text + "'");
        }
        if (ddlUsers.SelectedIndex != 0)
        {
            sb.Append(" and R_User_Name='" + ddlUsers.SelectedItem.Text + "'");
        }
        if (ddlTeam.SelectedIndex != 0)
        {
            sb.Append(" and T_TeamName='" + ddlTeam.SelectedItem.Text + "'");
        }

        dt = new DataTable();
        //da = new SqlDataAdapter("select Distinct STL_ServiceCode from RTM_Records left join RTM_SubTask_List on R_SubTask = STL_ID left join RTM_Team_List on R_TeamId = T_ID left join RTM_Client_List on R_Client = CL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + txtFrom.Text + "' and '" + txtTo.Text + "' "+ sb.ToString() +" order by STL_ServiceCode", con);
        //da.Fill(dt);

        dt = objMetrics.fetchTasks(txtFrom.Text, txtTo.Text, sb);

        if (dt.Rows.Count > 0)
        {
            ddlTask.DataSource = dt;
            ddlTask.DataValueField = "STL_ServiceCode";
            ddlTask.DataTextField = "STL_ServiceCode";
            ddlTask.DataBind();
            ddlTask.Items.Insert(0, "--Select Task--");
            ddlTask.SelectedIndex = 0;
        }
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        Display();
    }

    private void Display()
    {
        try
        {
            dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            if (ddlTeam.SelectedIndex != 0)
            {
                sb.Append(" and T_TeamName='" + ddlTeam.SelectedItem.Text + "'");
            }
            if (ddlClient.SelectedIndex != 0)
            {
                sb.Append(" and CL_ClientName='" + ddlClient.SelectedItem.Text + "'");
            }
            if (ddlUsers.SelectedIndex != 0)
            {
                sb.Append(" and R_User_Name='" + ddlUsers.SelectedItem.Text + "'");
            }
            if (ddlTask.SelectedIndex != 0)
            {
                sb.Append(" and STL_ServiceCode='" + ddlTask.SelectedItem.Text + "'");
            }

            //da = new SqlDataAdapter("select T_TeamName, CL_ClientName,R_User_Name, STL_ServiceCode, CONVERT(varchar(10), sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600) +':'+ CONVERT(varchar(10),(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60) +':'+ CONVERT(varchar(10),(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60) as [duration] from RTM_Records left join RTM_Team_List on R_TeamId = T_ID left join RTM_Client_List on R_Client = CL_ID left join RTM_SubTask_List on R_SubTask = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + txtFrom.Text + "' and '" + txtTo.Text + "' and R_Duration != 'HH:MM:SS' " + sb.ToString() + " Group By T_TeamName, CL_ClientName,R_User_Name, STL_ServiceCode Order By T_TeamName, R_User_Name", con);
            //da.Fill(dt);

            dt = objMetrics.DisplayData(txtFrom.Text, txtTo.Text, sb);

            if (dt.Rows.Count > 0)
            {
                gvDisplay.DataSource = dt;
                gvDisplay.DataBind();
            }
            else
            {
                gvDisplay.DataSource = null;
                gvDisplay.DataBind();
            }
            dt = new DataTable();
            //da = new SqlDataAdapter("select CONVERT(varchar(10), sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600) +':'+ CONVERT(varchar(10),(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60) +':'+ CONVERT(varchar(10),(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60) as [duration] from RTM_Records left join RTM_Team_List on R_TeamId = T_ID left join RTM_Client_List on R_Client = CL_ID left join RTM_SubTask_List on R_SubTask = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + txtFrom.Text + "' and '" + txtTo.Text + "' and R_Duration != 'HH:MM:SS' " + sb.ToString() + " ", con);
            //da.Fill(dt);

            dt = objMetrics.GetTotalDuration(txtFrom.Text, txtTo.Text, sb);

            if (dt.Rows.Count > 0)
            {
                lblTotal.Text = dt.Rows[0]["duration"].ToString();
                lblTotalDur.Visible = true;
                lblTotal.Visible = true;
            }
        }
        catch (Exception)
        {

        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            if (ddlTeam.SelectedIndex != 0)
            {
                sb.Append(" and T_TeamName='" + ddlTeam.SelectedItem.Text + "'");
            }
            if (ddlClient.SelectedIndex != 0)
            {
                sb.Append(" and CL_ClientName='" + ddlClient.SelectedItem.Text + "'");
            }
            if (ddlUsers.SelectedIndex != 0)
            {
                sb.Append(" and R_User_Name='" + ddlUsers.SelectedItem.Text + "'");
            }
            if (ddlTask.SelectedIndex != 0)
            {
                sb.Append(" and STL_ServiceCode='" + ddlTask.SelectedItem.Text + "'");
            }

            da = new SqlDataAdapter("select T_TeamName as [Team], CL_ClientName as Client,R_User_Name as [User Name], STL_ServiceCode as [Service Code], CONVERT(varchar(10), sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600) +':'+ CONVERT(varchar(10),(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60) +':'+ CONVERT(varchar(10),(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60) as [duration] from RTM_Records left join RTM_Team_List on R_TeamId = T_ID left join RTM_Client_List on R_Client = CL_ID left join RTM_SubTask_List on R_SubTask = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + txtFrom.Text + "' and '" + txtTo.Text + "' and R_Duration != 'HH:MM:SS' " + sb.ToString() + " Group By T_TeamName, CL_ClientName,R_User_Name, STL_ServiceCode Order By T_TeamName", con);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                sb = new StringBuilder();
                if (ddlTeam.SelectedIndex != 0)
                {
                    sb.Append(ddlTeam.SelectedItem.Text + "-");
                }
                if (ddlClient.SelectedIndex != 0)
                {
                    sb.Append(ddlClient.SelectedItem.Text + "-");
                }
                if (ddlUsers.SelectedIndex != 0)
                {
                    sb.Append(ddlUsers.SelectedItem.Text + "-");
                }
                if (ddlTask.SelectedIndex != 0)
                {
                    sb.Append(ddlTask.SelectedItem.Text + "-");
                }
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Internal Metrics- " + sb.ToString() + "" + txtFrom.Text + " to " + txtTo.Text + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                sb = new StringBuilder();
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(dt.Columns[k].ColumnName + ',');
                }
                sb.Append("\r\n");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        //add separator
                        sb.Append(dt.Rows[i][k].ToString().Replace(",", ";") + ',');
                    }
                    //append new line
                    sb.Append("\r\n");
                }

                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception)
        {


        }
    }

    private void Export()
    {

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        if (txtFrom.Text.Length <= 0)
        {
            lblError.Text = "Please select From date";
            return;
        }
        if (txtTo.Text.Length <= 0)
        {
            lblError.Text = "Please select To date";
            return;
        }

        if (Convert.ToDateTime(txtFrom.Text) > Convert.ToDateTime(txtTo.Text))
        {
            lblError.Text = "From date should be lesser than To date";
            return;
        }
        FetchallTeams();
        FetchallClients();
        FetchallUsers();
        FetchallTasks();
        //dropDiv.Visible = true;
        lblSelect.Text = "0";
        lblTotal.Text = string.Empty;
        gvDisplay.DataSource = null;
        gvDisplay.DataBind();
    }
}