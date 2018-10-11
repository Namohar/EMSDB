using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class DelayInstances : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    DataTable dt = new DataTable();
    SqlDataAdapter da = new SqlDataAdapter();
    clsUserLogMonitor objUserLog = new clsUserLogMonitor();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadTeams();
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        PrintReport();
    }

    private void LoadTeams()
    {
        int access = Convert.ToInt32(Session["access"]);
        dt = objUserLog.fetchTeams(access, Session["username"].ToString(), Convert.ToInt32(Session["team"]));

        ddlTeam.DataSource = dt;
        ddlTeam.DataValueField = "T_ID";
        ddlTeam.DataTextField = "T_TeamName";
        ddlTeam.DataBind();
        if (access != 3)
        {
            ddlTeam.Items.Insert(0, "--Select Team--");
            ddlTeam.SelectedIndex = 0;
        }

    }

    private void BuildTable()
    {
        try
        {
            dt = new DataTable();
            DataColumn dc;

            dc = new DataColumn("User Name");
            dt.Columns.Add(dc);

            dc = new DataColumn("# of Delayed Login");
            dt.Columns.Add(dc);

            dc = new DataColumn("# of Delayed Logoff");
            dt.Columns.Add(dc);

            dc = new DataColumn("Total Hours (Delayed Login)");
            dt.Columns.Add(dc);

            dc = new DataColumn("Total Hours (Delayed Logoff)");
            dt.Columns.Add(dc);
        }
        catch (Exception)
        {

        }
    }

    private void PrintReport()
    {
        try
        {
            DataRow dr;
            BuildTable();
            ds = FetchUsers();
            if (ds.Tables["users"].Rows.Count > 0)
            {
                foreach (DataRow drUser in ds.Tables["users"].Rows)
                {
                    dr = dt.NewRow();

                    dr["User Name"] = drUser["UL_User_Name"];
                    ds = FetchDelayCount(drUser["UL_User_Name"].ToString(), "In");
                    if (ds.Tables["in"].Rows.Count > 0)
                    {
                        dr["# of Delayed Login"] = ds.Tables["in"].Rows[0]["InCount"];
                        dr["Total Hours (Delayed Login)"] = ds.Tables["in"].Rows[0]["Duration"];
                    }

                    ds = FetchDelayCount(drUser["UL_User_Name"].ToString(), "Off");
                    if (ds.Tables["in"].Rows.Count > 0)
                    {
                        dr["# of Delayed Logoff"] = ds.Tables["in"].Rows[0]["InCount"];
                        dr["Total Hours (Delayed Logoff)"] = ds.Tables["in"].Rows[0]["Duration"];
                    }

                    dt.Rows.Add(dr);
                }
            }

            grdInstances.DataSource = dt;
            grdInstances.DataBind();
        }
        catch (Exception)
        {

        }
    }

    public DataSet FetchUsers()
    {
        if (ds.Tables.Contains("users"))
        {
            ds.Tables.Remove(ds.Tables["users"]);
        }
        da = new SqlDataAdapter("select * from RTM_User_List where UL_Team_Id ='" + ddlTeam.SelectedValue + "' and UL_User_Status = 1", con);
        da.Fill(ds, "users");
        return ds;
    }

    public DataSet FetchDelayCount(string user, string type)
    {
        if (ds.Tables.Contains("in"))
        {
            ds.Tables.Remove(ds.Tables["in"]);
        }
        da = new SqlDataAdapter("select Count(D_Type) as InCount, Convert(Varchar(2),sum(datediff(second,'00:00:00',REPLACE(D_Duration,'-', '')))/3600) +':'+ Convert(Varchar(2),(sum(datediff(second,'00:00:00',REPLACE(D_Duration,'-', '')))/60)%60) +':'+ Convert(Varchar(2),(sum(datediff(second,'00:00:00',REPLACE(D_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(D_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(D_Duration,'-', '')))/60)%60)))%60) as Duration  from RTM_DelayedLogInOff where D_UserName ='" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, D_Date))) between '" + txtFrom.Text + "' and '" + txtTo.Text + "' and D_Type = '" + type + "'", con);
        da.Fill(ds, "in");
        return ds;
    }
}