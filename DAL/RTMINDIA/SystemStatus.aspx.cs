using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BAL;
using System.Configuration;
using System.Text;

public partial class SystemStatus : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    DataTable dt = new DataTable();
    SqlDataAdapter da = new SqlDataAdapter();
    clsUserLogMonitor objUserLog = new clsUserLogMonitor();
    DataSet ds = new DataSet();
    DateTime dt1;
    DateTime dt2;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadTeams();
            // imgloading.Visible = false;
        }
    }

    private void LoadTeams()
    {
        int access = Convert.ToInt32(Session["access"]);
        dt = objUserLog.fetchTeams(access, Session["username"].ToString(), Convert.ToInt32(Session["team"]));

        ddlTeam.DataSource = dt;
        ddlTeam.DataValueField = "T_ID";
        ddlTeam.DataTextField = "T_TeamName";
        ddlTeam.DataBind();
        ddlTeam.Items.Insert(0, "--Select Team--");
        ddlTeam.SelectedIndex = 0;
    }

    public DataSet GetUsers(int teamId)
    {
        if (ds.Tables.Contains("users"))
        {
            ds.Tables.Remove(ds.Tables["users"]);
        }
        da = new SqlDataAdapter("select * from RTM_User_List where UL_Team_Id=" + teamId + " and UL_User_Status=1 order by UL_User_Name", con);
        da.Fill(ds, "users");
        return ds;
    }

    private void BuildTable()
    {
        dt = new DataTable();
        DataColumn dc;

        dc = new DataColumn("User Name");
        dt.Columns.Add(dc);

        dc = new DataColumn("Client");
        dt.Columns.Add(dc);
        dc = new DataColumn("Task");
        dt.Columns.Add(dc);
        dc = new DataColumn("SubTask");
        dt.Columns.Add(dc);
        dc = new DataColumn("Log");
        dt.Columns.Add(dc);
        dc = new DataColumn("Start Time");
        dt.Columns.Add(dc);
        dc = new DataColumn("Duration");
        dt.Columns.Add(dc);
        dc = new DataColumn("Status");
        dt.Columns.Add(dc);
        dc = new DataColumn("Comments");
        dt.Columns.Add(dc);
    }

    private DataSet LoadUserLastTaskRecord(string user)
    {
        if (ds.Tables.Contains("record"))
        {
            ds.Tables.Remove(ds.Tables["record"]);
        }
        da = new SqlDataAdapter("select TOP 1 R_ID, R_User_Name, CL_ClientName, TL_Task, STL_SubTask, R_Start_Date_Time, R_Duration, R_Status, R_Comments, R_CreatedOn from RTM_Records left join RTM_Client_List on R_Client = CL_ID left join RTM_Task_List on R_Task = TL_ID left join RTM_SubTask_List on R_SubTask = STL_ID where R_User_Name= '" + user + "' and STL_SubTask Not Like 'Peer Support%' and R_Duration != 'HH:MM:SS' ORDER BY R_CreatedOn DESC", con);
        da.Fill(ds, "record");
        return ds;
    }

    private DataSet LoadUserLastLogRecord(string user)
    {
        if (ds.Tables.Contains("log"))
        {
            ds.Tables.Remove(ds.Tables["log"]);
        }
        da = new SqlDataAdapter("select TOP 1 LA_ID, LA_User_Name, LA_Log_Action, LA_Start_Date_Time, LA_Duration, LA_Status, LA_Comments, LA_Reason, LA_CreatedOn from RTM_Log_Actions where LA_User_Name= '" + user + "' and LA_Duration != 'HH:MM:SS' ORDER BY LA_CreatedOn DESC", con);
        da.Fill(ds, "log");
        return ds;
    }

    private void PrintReport()
    {
        BuildTable();
        DataRow dr;
        ds = GetUsers(Convert.ToInt32(ddlTeam.SelectedValue));

        if (ds.Tables["users"].Rows.Count > 0)
        {
            foreach (DataRow dr5 in ds.Tables["users"].Rows)
            {
                ds = LoadUserLastTaskRecord(dr5["UL_User_Name"].ToString());
                if (ds.Tables["record"].Rows.Count > 0)
                {
                    dr = dt.NewRow();

                    dr["User Name"] = dr5["UL_User_Name"];
                    dr["Client"] = ds.Tables["record"].Rows[0]["CL_ClientName"];
                    dr["Task"] = ds.Tables["record"].Rows[0]["TL_Task"];
                    dr["SubTask"] = ds.Tables["record"].Rows[0]["STL_SubTask"];
                    dr["Log"] = "-";
                    dr["Start Time"] = ds.Tables["record"].Rows[0]["R_Start_Date_Time"];
                    dr["Duration"] = ds.Tables["record"].Rows[0]["R_Duration"];
                    dr["Status"] = ds.Tables["record"].Rows[0]["R_Status"];
                    dr["Comments"] = ds.Tables["record"].Rows[0]["R_Comments"];
                    dt.Rows.Add(dr);

                }

                ds = LoadUserLastLogRecord(dr5["UL_User_Name"].ToString());
                if (ds.Tables["log"].Rows.Count > 0)
                {
                    dr = dt.NewRow();

                    dr["User Name"] = dr5["UL_User_Name"];
                    dr["Client"] = "-";
                    dr["Task"] = "-";
                    dr["SubTask"] = "-";
                    dr["Log"] = ds.Tables["log"].Rows[0]["LA_Log_Action"];
                    dr["Start Time"] = ds.Tables["log"].Rows[0]["LA_Start_Date_Time"];
                    dr["Duration"] = ds.Tables["log"].Rows[0]["LA_Duration"];
                    dr["Status"] = ds.Tables["log"].Rows[0]["LA_Status"];
                    dr["Comments"] = ds.Tables["log"].Rows[0]["LA_Comments"];
                    dt.Rows.Add(dr);
                }
            }


        }

    }

    private void PrintReportNew()
    {
        try
        {
            //imgloading.Visible = true;
            System.Threading.Thread.Sleep(20);
            BuildTable();
            DataRow dr;

            ds = GetUsers(Convert.ToInt32(ddlTeam.SelectedValue));

            if (ds.Tables["users"].Rows.Count > 0)
            {
                foreach (DataRow dr5 in ds.Tables["users"].Rows)
                {
                    DateTime? dt1 = null;
                    DateTime? dt2 = null;
                    ds = LoadUserLastLogRecord(dr5["UL_User_Name"].ToString());
                    if (ds.Tables["log"].Rows.Count > 0)
                    {
                        dt1 = Convert.ToDateTime(ds.Tables["log"].Rows[0]["LA_CreatedOn"].ToString());
                    }

                    ds = LoadUserLastTaskRecord(dr5["UL_User_Name"].ToString());
                    if (ds.Tables["record"].Rows.Count > 0)
                    {
                        dt2 = Convert.ToDateTime(ds.Tables["record"].Rows[0]["R_CreatedOn"].ToString());
                    }

                    if (ds.Tables["record"].Rows.Count > 0 && ds.Tables["record"].Rows[0]["R_Status"].ToString() == "Running")
                    {
                        dr = dt.NewRow();

                        dr["User Name"] = dr5["UL_User_Name"];
                        dr["Client"] = ds.Tables["record"].Rows[0]["CL_ClientName"];
                        dr["Task"] = ds.Tables["record"].Rows[0]["TL_Task"];
                        dr["SubTask"] = ds.Tables["record"].Rows[0]["STL_SubTask"];
                        dr["Log"] = "-";
                        dr["Start Time"] = ds.Tables["record"].Rows[0]["R_Start_Date_Time"];
                        dr["Duration"] = ds.Tables["record"].Rows[0]["R_Duration"];
                        dr["Status"] = ds.Tables["record"].Rows[0]["R_Status"];
                        dr["Comments"] = ds.Tables["record"].Rows[0]["R_Comments"];
                        dt.Rows.Add(dr);
                    }
                    else if (ds.Tables["log"].Rows.Count > 0 && ds.Tables["log"].Rows[0]["LA_Status"].ToString() != "Unlocked")
                    {
                        dr = dt.NewRow();

                        dr["User Name"] = dr5["UL_User_Name"];
                        dr["Client"] = "-";
                        dr["Task"] = "-";
                        dr["SubTask"] = "-";
                        dr["Log"] = ds.Tables["log"].Rows[0]["LA_Log_Action"];
                        dr["Start Time"] = ds.Tables["log"].Rows[0]["LA_Start_Date_Time"];
                        dr["Duration"] = ds.Tables["log"].Rows[0]["LA_Duration"];
                        dr["Status"] = ds.Tables["log"].Rows[0]["LA_Status"];
                        dr["Comments"] = ds.Tables["log"].Rows[0]["LA_Comments"];
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        if (dt2 >= dt1)
                        {
                            dr = dt.NewRow();

                            dr["User Name"] = dr5["UL_User_Name"];
                            dr["Client"] = ds.Tables["record"].Rows[0]["CL_ClientName"];
                            dr["Task"] = ds.Tables["record"].Rows[0]["TL_Task"];
                            dr["SubTask"] = ds.Tables["record"].Rows[0]["STL_SubTask"];
                            dr["Log"] = "-";
                            dr["Start Time"] = ds.Tables["record"].Rows[0]["R_Start_Date_Time"];
                            dr["Duration"] = ds.Tables["record"].Rows[0]["R_Duration"];
                            dr["Status"] = ds.Tables["record"].Rows[0]["R_Status"];
                            dr["Comments"] = ds.Tables["record"].Rows[0]["R_Comments"];
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            dr = dt.NewRow();

                            dr["User Name"] = dr5["UL_User_Name"];
                            dr["Client"] = "-";
                            dr["Task"] = "-";
                            dr["SubTask"] = "-";
                            dr["Log"] = ds.Tables["log"].Rows[0]["LA_Log_Action"];
                            dr["Start Time"] = ds.Tables["log"].Rows[0]["LA_Start_Date_Time"];
                            dr["Duration"] = ds.Tables["log"].Rows[0]["LA_Duration"];
                            dr["Status"] = ds.Tables["log"].Rows[0]["LA_Status"];
                            dr["Comments"] = ds.Tables["log"].Rows[0]["LA_Comments"];
                            dt.Rows.Add(dr);
                        }
                    }

                }
            }
            //imgloading.Visible = false;
        }
        catch (Exception)
        {

        }
        finally
        {
            // imgloading.Visible = false;
        }

    }

    protected void ddlTeam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTeam.SelectedIndex != 0)
        {

            PrintReportNew();
            if (dt.Rows.Count > 0)
            {
                //DataView dv = dt.DefaultView;
                //dv.Sort = "User Name asc, Start Time asc";
                //dt = dv.ToTable();
                gvSystemStatus.DataSource = dt;
                gvSystemStatus.DataBind();
            }
            else
            {
                gvSystemStatus.DataSource = null;
                gvSystemStatus.DataBind();
            }
        }
        else
        {
            gvSystemStatus.DataSource = null;
            gvSystemStatus.DataBind();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            PrintReport();

            StringBuilder sb = new StringBuilder();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=System Status-" + ddlTeam.SelectedItem.Text + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

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
        catch (Exception)
        {

        }


    }
}