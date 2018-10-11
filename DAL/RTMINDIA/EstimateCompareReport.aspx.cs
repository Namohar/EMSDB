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

public partial class EstimateCompareReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());

    SqlDataAdapter da;
    DataTable dt = new DataTable();
    DataTable dtResult = new DataTable();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblid.Text = Request.QueryString["TID"];
            LoadEmp();
        }
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        try
        {
            DisplayData();
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
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_Team_Id ='" + lblid.Text + "' ORDER BY UL_User_Name", con);
        }
        else if (Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("select * from RTM_User_List where UL_Team_Id ='" + lblid.Text + "' ORDER BY UL_User_Name", con);
        }
        else if (Session["access"].ToString() == "3")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_Team_Id ='" + lblid.Text + "' ORDER BY UL_User_Name", con);
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
        ddlEmp.Items.Insert(0, "All");
        ddlEmp.SelectedIndex = 0;
    }

    private void BuidTable()
    {
        DataColumn dc;

        dc = new DataColumn("Employee Name");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("Date");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("Client");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("Task");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("Subtask");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("User Estimate");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("RTM Time");
        dtResult.Columns.Add(dc);
    }

    private DataSet GetRTMRecords()
    {

        if (ds.Tables.Contains("Records"))
        {
            ds.Tables.Remove(ds.Tables["Records"]);
        }

        if (ddlEmp.SelectedIndex == 0)
        {
            da = new SqlDataAdapter("select R_User_Name, R_Client, CL_ClientName, R_Task, TL_Task, R_SubTask, STL_SubTask, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds " +
                         " from RTM_Records left join rtm_client_list on R_Client = CL_ID left join rtm_task_list on R_Task = TL_ID " +
                        " left join rtm_subtask_list on R_SubTask = STL_ID " +
                         " where R_TeamId = '" + lblid.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) = '" + txtDate.Text + "' " +
                          " Group by R_User_Name, R_Client, CL_ClientName, R_Task, TL_Task, R_SubTask, STL_SubTask", con);

            //da = new SqlDataAdapter("select R_User_Name, R_Client, CL_ClientName, R_Task, TL_Task, R_SubTask, STL_SubTask, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds " +
            //            " from RTM_Records left join rtm_client_list on R_Client = CL_ID left join rtm_task_list on R_Task = TL_ID " +
            //           " left join rtm_subtask_list on R_SubTask = STL_ID " +
            //            " where R_TeamId = '" + lblid.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '11/1/2015' and '11/30/2015' " +
            //             " Group by R_User_Name, R_Client, CL_ClientName, R_Task, TL_Task, R_SubTask, STL_SubTask", con);
        }
        else
        {
            da = new SqlDataAdapter("select R_User_Name, R_Client, CL_ClientName, R_Task, TL_Task, R_SubTask, STL_SubTask, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds " +
                         " from RTM_Records left join rtm_client_list on R_Client = CL_ID left join rtm_task_list on R_Task = TL_ID " +
                        " left join rtm_subtask_list on R_SubTask = STL_ID " +
                         " where R_TeamId = '" + lblid.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) = '" + txtDate.Text + "' and R_User_Name='" + ddlEmp.SelectedItem.Text + "' " +
                          " Group by R_User_Name, R_Client, CL_ClientName, R_Task, TL_Task, R_SubTask, STL_SubTask", con);
        }

        da.Fill(ds, "Records");
        return ds;
    }

    private DataSet GetEstimateRecords(string user, int client, int task, int subtask)
    {
        if (ds.Tables.Contains("estRec"))
        {
            ds.Tables.Remove(ds.Tables["estRec"]);
        }
        //dt = new DataTable();
        da = new SqlDataAdapter("select  sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/60)%60)))%60 as seconds " +
            " from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID" +
            " left join rtm_subtask_list on EST_SubTaskId = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) = '" + txtDate.Text + "' and  EST_ClientId ='" + client + "' and EST_TaskId = '" + task + "' and EST_SubTaskId = '" + subtask + "' and EST_UserName ='" + user + "' ", con);

        //da = new SqlDataAdapter("select  sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(EST_Duration,'-', '')))/60)%60)))%60 as seconds " +
        //    " from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID" +
        //    " left join rtm_subtask_list on EST_SubTaskId = STL_ID where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) BETWEEN '11/1/2015' and '11/30/2015' and  EST_ClientId ='" + client + "' and EST_TaskId = '" + task + "' and EST_SubTaskId = '" + subtask + "' and EST_UserName ='" + user + "' ", con);
        da.Fill(ds, "estRec");
        return ds;
    }

    private DataSet GetEstNotInRec(string user)
    {
        if (ds.Tables.Contains("onlyest"))
        {
            ds.Tables.Remove(ds.Tables["onlyest"]);
        }

        if (ddlEmp.SelectedIndex == 0)
        {
            da = new SqlDataAdapter("select EST_UserName, EST_ClientId,CL_ClientName, EST_TaskId,TL_Task, EST_SubTaskId, STL_SubTask, EST_Duration from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID left join rtm_subtask_list on EST_SubTaskId = STL_ID where NOT EXISTS (select * from RTM_Records where EST_ClientId = R_Client and EST_TaskId= R_Task and EST_SubTaskId=R_SubTask  and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) ='" + txtDate.Text + "' ) and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) ='" + txtDate.Text + "' and EST_TeamId = '" + lblid.Text + "'", con);

            // da = new SqlDataAdapter("select EST_UserName, EST_ClientId,CL_ClientName, EST_TaskId,TL_Task, EST_SubTaskId, STL_SubTask, EST_Duration from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID left join rtm_subtask_list on EST_SubTaskId = STL_ID where NOT EXISTS (select * from RTM_Records where EST_ClientId = R_Client and EST_TaskId= R_Task and EST_SubTaskId=R_SubTask  and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '11/1/2015' and '11/30/2015' ) and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) BETWEEN '11/1/2015' and '11/30/2015' and EST_TeamId = '" + lblid.Text + "'", con);
        }
        else
        {
            da = new SqlDataAdapter("select EST_UserName, EST_ClientId,CL_ClientName, EST_TaskId,TL_Task, EST_SubTaskId, STL_SubTask, EST_Duration from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID left join rtm_subtask_list on EST_SubTaskId = STL_ID where NOT EXISTS (select * from RTM_Records where EST_ClientId = R_Client and EST_TaskId= R_Task and EST_SubTaskId=R_SubTask and R_User_Name ='" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) ='" + txtDate.Text + "' ) and EST_UserName ='" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date))) ='" + txtDate.Text + "' and EST_TeamId = '" + lblid.Text + "'", con);
        }
        da.Fill(ds, "onlyest");
        return ds;
    }

    private DataSet GetLogs(string user)
    {
        if (ds.Tables.Contains("logs"))
        {
            ds.Tables.Remove(ds.Tables["logs"]);
        }
        //dt = new DataTable();
        if (ddlEmp.SelectedIndex == 0)
        {
            da = new SqlDataAdapter("select LA_User_Name, LA_Reason, sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds " +
            "from RTM_Log_Actions where LA_TeamId = '" + lblid.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) ='" + txtDate.Text + "' and (LA_Reason = 'Meetings' or LA_Reason='Meeting'  or LA_Reason = 'Conference-Call')  group by LA_User_Name, LA_Reason", con);

            //da = new SqlDataAdapter("select LA_User_Name, LA_Reason, sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds " +
            //"from RTM_Log_Actions where LA_TeamId = '" + lblid.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '11/1/2015' and '11/30/2015' and (LA_Reason = 'Meetings' or LA_Reason='Meeting'  or LA_Reason = 'Conference-Call')  group by LA_User_Name, LA_Reason", con);
        }
        else
        {
            da = new SqlDataAdapter("select LA_User_Name, LA_Reason, sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds " +
            "from RTM_Log_Actions where LA_TeamId = '" + lblid.Text + "' and  CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) ='" + txtDate.Text + "' and LA_User_Name='" + ddlEmp.SelectedItem.Text + "' and (LA_Reason = 'Meetings' or LA_Reason='Meeting'  or LA_Reason = 'Conference-Call' )  group by LA_User_Name, LA_Reason", con);
        }

        da.Fill(ds, "logs");
        return ds;
    }

    private void DisplayData()
    {
        DataRow dr;

        BuidTable();

        ds = GetRTMRecords();
        if (ds.Tables["Records"].Rows.Count > 0)
        {
            foreach (DataRow dr1 in ds.Tables["Records"].Rows)
            {
                dr = dtResult.NewRow();

                dr["Employee Name"] = dr1["R_User_Name"];
                dr["Date"] = txtDate.Text;
                dr["Client"] = dr1["CL_ClientName"];
                dr["Task"] = dr1["TL_Task"];
                dr["Subtask"] = dr1["STL_SubTask"];

                ds = GetEstimateRecords(dr1["R_User_Name"].ToString(), Convert.ToInt32(dr1["R_Client"]), Convert.ToInt32(dr1["R_Task"]), Convert.ToInt32(dr1["R_SubTask"]));

                if (ds.Tables["estRec"].Rows.Count > 0 && ds.Tables["estRec"].Rows[0]["hour"].ToString().Length > 0)
                {
                    dr["User Estimate"] = ds.Tables["estRec"].Rows[0]["hour"].ToString() + ":" + ds.Tables["estRec"].Rows[0]["minute"].ToString() + ":" + ds.Tables["estRec"].Rows[0]["seconds"].ToString();
                }

                dr["RTM Time"] = dr1["hour"] + ":" + dr1["minute"] + ":" + dr1["seconds"];

                dtResult.Rows.Add(dr);
            }
        }

        ds = GetEstNotInRec(ddlEmp.SelectedItem.Text);

        if (ds.Tables["onlyest"].Rows.Count > 0)
        {
            foreach (DataRow dr2 in ds.Tables["onlyest"].Rows)
            {
                dr = dtResult.NewRow();

                dr["Employee Name"] = dr2["EST_UserName"];
                dr["Date"] = txtDate.Text;
                dr["Client"] = dr2["CL_ClientName"];
                dr["Task"] = dr2["TL_Task"];
                dr["Subtask"] = dr2["STL_SubTask"];
                dr["User Estimate"] = dr2["EST_Duration"];
                dr["RTM Time"] = "";

                dtResult.Rows.Add(dr);
            }
        }

        ds = GetLogs(ddlEmp.SelectedItem.Text);
        if (ds.Tables["logs"].Rows.Count > 0)
        {
            foreach (DataRow dr2 in ds.Tables["logs"].Rows)
            {
                dr = dtResult.NewRow();

                dr["Employee Name"] = dr2["LA_User_Name"];
                dr["Date"] = txtDate.Text;
                dr["Client"] = "";
                dr["Task"] = dr2["LA_Reason"];
                dr["Subtask"] = "";
                dr["User Estimate"] = "";
                dr["RTM Time"] = dr2["hour"] + ":" + dr2["minute"] + ":" + dr2["seconds"];

                dtResult.Rows.Add(dr);
            }
        }

        DataView dv = dtResult.DefaultView;
        dv.Sort = "[Employee Name] asc";
        dtResult = dv.ToTable();

        gvEstimate.DataSource = dtResult;
        gvEstimate.DataBind();
    }
}