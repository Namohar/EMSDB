using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

public partial class DashBoard : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    SqlDataAdapter da;
    DataTable dt = new DataTable();
    DataSet ds = new DataSet();
    string TeamName;
    DateTime lastLogin;
    DateTime lastLogout;
    DataTable dtResult = new DataTable();
    DateTime CurrentLogin;
    DateTime endOfLastWeek;
    DateTime startOfLatWeek;

    DateTime logoutTime;
    protected void Page_Load(object sender, EventArgs e)
    {
        string acc = Session["access"].ToString();
        if (acc == "1")
        {
            Response.Redirect("~/AdminDashBoard.aspx");
        }
        endOfLastWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
        startOfLatWeek = endOfLastWeek.AddDays(-6);
        try
        {
            if (Session["team"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!IsPostBack)
            {
                ds = GetTeamName(Convert.ToInt32(Session["team"]));

                if (ds.Tables["Team"].Rows.Count > 0)
                {
                    TeamName = ds.Tables["Team"].Rows[0]["T_TeamName"].ToString();
                }

                lblDate.Text = DateTime.Now.ToString("d-MMM-yyyy");
                LastLogin1();
                LastLogout1();
                //Console.WriteLine(Session["access"].ToString());
                //System.Diagnostics.Debug.WriteLine("-------");
                //System.Diagnostics.Trace.WriteLine("You click me ..................");

                BindPieChart1();
                if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
                {
                    BindRUPieChart();
                }
                else
                {
                    BindPieChart2();
                }

                BindPieChart3();

                BindSummaryData();

                BindBarChart4();

                BindLineChartData();
                TotalWorkHours();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private DataSet GetTeamName(int id)
    {
        da = new SqlDataAdapter("select T_TeamName from RTM_Team_List where T_ID= '" + id + "'", con);
        da.Fill(ds, "Team");
        return ds;
    }

    private DataTable LoadClient()
    {
        con.Open();
        SqlCommand cmd = new SqlCommand("SELECT TOP 5 C.CL_ClientName, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records B, RTM_Client_List C where  B.R_Client = C.CL_ID and R_TeamId='" + Convert.ToInt32(Session["team"]) + "'  and R_Duration != 'HH:MM:SS' and R_Start_Date_Time BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "'  AND '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' GROUP BY C.CL_ClientName ORDER BY sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 DESC", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public Title CreateTitle(string name)
    {
        Title title = new Title();
        title.Text = name;
        title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
        title.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold);
        title.ShadowOffset = 3;
        title.ForeColor = Color.FromArgb(26, 59, 105);
        return title;
    }


    protected void Chart1_Click(object sender, ImageMapEventArgs e)
    {
        Response.Redirect("~/ClentUsageTeam.aspx?type=TeamClientsUsage");
    }

    private DataSet LoadUserCleints()
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT TOP 5 C.CL_ClientName, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records B, RTM_Client_List C where  B.R_Client = C.CL_ID and R_User_Name='" + Session["username"].ToString() + "'  and R_Duration != 'HH:MM:SS' and R_Start_Date_Time BETWEEN '" + lastLogin + "' and '" + lastLogout + "' GROUP BY C.CL_ClientName ORDER BY sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 DESC", con);
        da.Fill(ds, "user");
        return ds;
    }

    private void BindPieChart1()
    {
        try
        {
            dt = LoadClient();

            string[] x = new string[dt.Rows.Count];
            double[] y = new double[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = dt.Rows[i][0].ToString();
                //y[i] = Convert.ToDouble(dt.Rows[i][1] + "." + dt.Rows[i][2]);
                y[i] = Math.Round(Convert.ToDouble(dt.Rows[i][1]) + (Convert.ToDouble(dt.Rows[i][2]) / 60) + (Convert.ToDouble(dt.Rows[i][3]) / 3600), 2, MidpointRounding.AwayFromZero);
            }

            Chart1.Series[0].Points.DataBindXY(x, y);
            //this.Chart1.Series[0]["PieLabelStyle"] = "Outside";
            this.Chart1.Series[0].BorderWidth = 1;
            this.Chart1.Series[0].Label = "#VALY";
            this.Chart1.Series[0].LegendText = "#VALX (#VALY)"; // "#VALX (#PERCENT)";

            //Chart1.Series[0].ChartType = SeriesChartType.Column;
            //Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            //Chart1.Legends[0].Enabled = true;
            // Chart1.Series[0].Points[0].CustomProperties = "Exploded=true";
            Chart1.Titles.Add(CreateTitle("Client Usage - " + TeamName + " (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + ") - (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + ")"));
        }
        catch (Exception)
        {

        }


    }

    private void BindPieChart2()
    {
        try
        {
            ds = LoadUserCleints();

            string[] x = new string[ds.Tables["user"].Rows.Count];
            double[] y = new double[ds.Tables["user"].Rows.Count];

            for (int i = 0; i < ds.Tables["user"].Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = ds.Tables["user"].Rows[i][0].ToString();
                //y[i] = Convert.ToDouble(ds.Tables["user"].Rows[i][1] + "." + ds.Tables["user"].Rows[i][2]);
                y[i] = Math.Round(Convert.ToDouble(ds.Tables["user"].Rows[i][1]) + (Convert.ToDouble(ds.Tables["user"].Rows[i][2]) / 60) + (Convert.ToDouble(ds.Tables["user"].Rows[i][3]) / 3600), 2, MidpointRounding.AwayFromZero);
            }

            Chart2.Series[0].Points.DataBindXY(x, y);
            //this.Chart1.Series[0]["PieLabelStyle"] = "Outside";
            this.Chart2.Series[0].BorderWidth = 1;
            this.Chart2.Series[0].Label = "#VALY";
            //this.Chart1.Series[0].LegendText = "#VALX (#VALY - #PERCENT)"; // "#VALX (#PERCENT)";

            //Chart1.Series[0].ChartType = SeriesChartType.Column;
            //Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            //Chart1.Legends[0].Enabled = true;
            //Chart2.Series[0].Points[0].CustomProperties = "Exploded=true";
            Chart2.Titles.Add(CreateTitle("Your Client Usage Data (" + lastLogin.ToShortDateString() + ")"));
        }
        catch (Exception)
        {

        }
    }

    private DataSet LoadUserTasks()
    {

        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT TOP 5 C.TL_Task, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records B, RTM_Task_List C where  B.R_Task = C.TL_ID and R_TeamId='" + Convert.ToInt32(Session["team"]) + "'  and R_Duration != 'HH:MM:SS' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' GROUP BY C.TL_Task  ORDER BY sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 DESC", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT TOP 5 C.TL_Task, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records B, RTM_Task_List C where  B.R_Task = C.TL_ID and R_User_Name='" + Session["username"].ToString() + "'  and R_Duration != 'HH:MM:SS' and R_Start_Date_Time BETWEEN '" + lastLogin + "' and '" + lastLogout + "' GROUP BY C.TL_Task ORDER BY sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 DESC", con);
        }

        da.Fill(ds, "Tasks");
        return ds;
    }

    private void BindPieChart3()
    {
        try
        {
            ds = LoadUserTasks();

            string[] x = new string[ds.Tables["Tasks"].Rows.Count];
            double[] y = new double[ds.Tables["Tasks"].Rows.Count];

            for (int i = 0; i < ds.Tables["Tasks"].Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = ds.Tables["Tasks"].Rows[i][0].ToString();
                //y[i] = Convert.ToDouble(ds.Tables["Tasks"].Rows[i][1] + "." + ds.Tables["Tasks"].Rows[i][2]);
                y[i] = Math.Round(Convert.ToDouble(ds.Tables["Tasks"].Rows[i][1]) + (Convert.ToDouble(ds.Tables["Tasks"].Rows[i][2]) / 60) + (Convert.ToDouble(ds.Tables["Tasks"].Rows[i][3]) / 3600), 2, MidpointRounding.AwayFromZero);
            }

            Chart3.Series[0].Points.DataBindXY(x, y);

            this.Chart3.Series[0].BorderWidth = 1;
            this.Chart3.Series[0].Label = "#VALY";
            //this.Chart1.Series[0].LegendText = "#VALX (#VALY - #PERCENT)"; // "#VALX (#PERCENT)";

            //Chart3.Series[0].Points[0].CustomProperties = "Exploded=true";
            if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
            {
                Chart3.Titles.Add(CreateTitle("   Prod-Task Summary (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + ") - (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + ")"));
            }
            else
            {
                Chart3.Titles.Add(CreateTitle("Your Prod-Task Summary (" + lastLogin.ToShortDateString() + ")"));
            }
        }
        catch (Exception)
        {


        }


    }

    private void LastLogin1()
    {
        try
        {
            da = new SqlDataAdapter("SELECT TOP 1 LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + Session["username"].ToString() + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) <= '" + DateTime.Now.AddDays(-1) + "' and LA_Log_Action = 'Actual Login' ORDER BY LA_ID DESC", con);
            da.Fill(ds, "login");
            if (ds.Tables["login"].Rows.Count > 0)
            {
                lastLogin = Convert.ToDateTime(ds.Tables["login"].Rows[0]["LA_Start_Date_Time"]);
            }
        }
        catch (Exception)
        {

        }

    }

    private void LastLogout1()
    {
        try
        {
            da = new SqlDataAdapter("SELECT TOP 1 LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + Session["username"].ToString() + "' and LA_Start_Date_Time >= '" + lastLogin + "' and LA_Log_Action = 'Actual Logout' ORDER BY LA_ID", con);
            da.Fill(ds, "logout");
            if (ds.Tables["logout"].Rows.Count > 0)
            {
                lastLogout = Convert.ToDateTime(ds.Tables["logout"].Rows[0]["LA_Start_Date_Time"]);
            }
        }
        catch (Exception)
        {

        }
    }

    //Bar Chart Start
    private void buildSummaryTable()
    {
        DataColumn dc;
        try
        {

            dc = new DataColumn("Employee");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Log");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Hours");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Percent");
            dtResult.Columns.Add(dc);

        }
        catch (Exception)
        {


        }
    }

    private DataSet TotalTask(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("Task"))
        {
            ds.Tables.Remove(ds.Tables["Task"]);
        }
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_TeamId='" + Convert.ToInt32(Session["team"]) + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and R_Duration != 'HH:MM:SS'", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and R_Duration != 'HH:MM:SS'", con);
        }

        da.Fill(ds, "Task");
        return ds;
    }

    private DataSet TotalBreak(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("Break"))
        {
            ds.Tables.Remove(ds.Tables["Break"]);
        }
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Break'", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Break'", con);
        }

        da.Fill(ds, "Break");
        return ds;
    }

    private DataSet TotalMeeting(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("Meeting"))
        {
            ds.Tables.Remove(ds.Tables["Meeting"]);
        }
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and (LA_Reason = 'Meetings' or LA_Reason = 'Meeting')", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and (LA_Reason = 'Meetings' or LA_Reason = 'Meeting')", con);
        }

        da.Fill(ds, "Meeting");
        return ds;
    }

    private DataSet TotalConfCall(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("Call"))
        {
            ds.Tables.Remove(ds.Tables["Call"]);
        }
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Conference-Call'", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Conference-Call'", con);
        }

        da.Fill(ds, "Call");
        return ds;
    }

    private DataSet TotalPeerSupport(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("PeerSup"))
        {
            ds.Tables.Remove(ds.Tables["PeerSup"]);
        }
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Peer Support'", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Peer Support'", con);
        }

        da.Fill(ds, "PeerSup");
        return ds;
    }

    private DataSet TotalNonTask1(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("nonTask1"))
        {
            ds.Tables.Remove(ds.Tables["nonTask1"]);
        }
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and (LA_Reason = 'Others' or LA_Reason = 'Non-Task')", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and (LA_Reason = 'Others' or LA_Reason = 'Non-Task')", con);
        }

        da.Fill(ds, "nonTask1");
        return ds;
    }

    private DataSet TotalNonTask2(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("nonTask2"))
        {
            ds.Tables.Remove(ds.Tables["nonTask2"]);
        }
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records, RTM_SubTask_List where R_SubTask = STL_ID and R_TeamId='" + Convert.ToInt32(Session["team"]) + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and R_Duration != 'HH:MM:SS' and STL_SubTask = 'Non Task'", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records, RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and R_Duration != 'HH:MM:SS' and STL_SubTask = 'Non Task'", con);
        }

        da.Fill(ds, "nonTask2");
        return ds;
    }

    private void BindSummaryData()
    {
        try
        {
            dtResult.Clear();
            DataRow dr;
            buildSummaryTable();
            double totalTask = 0;
            double totalBreak = 0;
            double totalMeet = 0;
            double totalCall = 0;
            double totalNonTask1 = 0;
            //double totalNonTask2=0;
            double totalNonTask = 0;
            double totalPeerSup = 0;

            ds = TotalTask(Session["username"].ToString(), lastLogin, lastLogout);

            if (ds.Tables["Task"].Rows.Count > 0 && ds.Tables["Task"].Rows[0]["hour"].ToString().Length > 0)
            {
                // totalTask = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
                totalTask = Math.Round(Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"]) + (Convert.ToDouble(ds.Tables["Task"].Rows[0]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["Task"].Rows[0]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
                dr = dtResult.NewRow();
                dr["Employee"] = Session["username"].ToString();
                dr["Log"] = "Task";
                dr["Hours"] = totalTask;
                dr["Percent"] = "";

                dtResult.Rows.Add(dr);
            }

            ds = TotalMeeting(Session["username"].ToString(), lastLogin, lastLogout);

            if (ds.Tables["Meeting"].Rows.Count > 0 && ds.Tables["Meeting"].Rows[0]["hour"].ToString().Length > 0)
            {
                // totalMeet = Convert.ToDouble(ds.Tables["Meeting"].Rows[0]["hour"] + "." + ds.Tables["Meeting"].Rows[0]["minute"]);
                totalMeet = Math.Round(Convert.ToDouble(ds.Tables["Meeting"].Rows[0]["hour"]) + (Convert.ToDouble(ds.Tables["Meeting"].Rows[0]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["Meeting"].Rows[0]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
                dr = dtResult.NewRow();
                dr["Employee"] = Session["username"].ToString();
                dr["Log"] = "Meeting";
                dr["Hours"] = totalMeet;
                dr["Percent"] = "";

                dtResult.Rows.Add(dr);
            }

            ds = TotalConfCall(Session["username"].ToString(), lastLogin, lastLogout);

            if (ds.Tables["Call"].Rows.Count > 0 && ds.Tables["Call"].Rows[0]["hour"].ToString().Length > 0)
            {
                //totalCall = Convert.ToDouble(ds.Tables["Call"].Rows[0]["hour"] + "." + ds.Tables["Call"].Rows[0]["minute"]);
                totalCall = Math.Round(Convert.ToDouble(ds.Tables["Call"].Rows[0]["hour"]) + (Convert.ToDouble(ds.Tables["Call"].Rows[0]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["Call"].Rows[0]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
                dr = dtResult.NewRow();
                dr["Employee"] = Session["username"].ToString();
                dr["Log"] = "Conference-Call";
                dr["Hours"] = totalCall;
                dr["Percent"] = "";

                dtResult.Rows.Add(dr);
            }

            ds = TotalPeerSupport(Session["username"].ToString(), lastLogin, lastLogout);

            if (ds.Tables["PeerSup"].Rows.Count > 0 && ds.Tables["PeerSup"].Rows[0]["hour"].ToString().Length > 0)
            {
                //totalPeerSup = Convert.ToDouble(ds.Tables["PeerSup"].Rows[0]["hour"] + "." + ds.Tables["PeerSup"].Rows[0]["minute"]);
                totalPeerSup = Math.Round(Convert.ToDouble(ds.Tables["PeerSup"].Rows[0]["hour"]) + (Convert.ToDouble(ds.Tables["PeerSup"].Rows[0]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["PeerSup"].Rows[0]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
                dr = dtResult.NewRow();
                dr["Employee"] = Session["username"].ToString();
                dr["Log"] = "Peer Support";
                dr["Hours"] = totalPeerSup;
                dr["Percent"] = "";

                dtResult.Rows.Add(dr);
            }

            ds = TotalBreak(Session["username"].ToString(), lastLogin, lastLogout);

            if (ds.Tables["Break"].Rows.Count > 0 && ds.Tables["Break"].Rows[0]["hour"].ToString().Length > 0)
            {
                //totalBreak = Convert.ToDouble(ds.Tables["Break"].Rows[0]["hour"] + "." + ds.Tables["Break"].Rows[0]["minute"]);
                totalBreak = Math.Round(Convert.ToDouble(ds.Tables["Break"].Rows[0]["hour"]) + (Convert.ToDouble(ds.Tables["Break"].Rows[0]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["Break"].Rows[0]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
                dr = dtResult.NewRow();
                dr["Employee"] = Session["username"].ToString();
                dr["Log"] = "Break";
                dr["Hours"] = totalBreak;
                dr["Percent"] = "";

                dtResult.Rows.Add(dr);
            }

            ds = TotalNonTask1(Session["username"].ToString(), lastLogin, lastLogout);

            if (ds.Tables["nonTask1"].Rows.Count > 0 && ds.Tables["nonTask1"].Rows[0]["hour"].ToString().Length > 0)
            {
                //totalNonTask1 = Convert.ToDouble(ds.Tables["nonTask1"].Rows[0]["hour"] + "." + ds.Tables["nonTask1"].Rows[0]["minute"]);
                totalNonTask1 = Math.Round(Convert.ToDouble(ds.Tables["nonTask1"].Rows[0]["hour"]) + (Convert.ToDouble(ds.Tables["nonTask1"].Rows[0]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["nonTask1"].Rows[0]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
                totalNonTask = totalNonTask1;
                dr = dtResult.NewRow();
                dr["Employee"] = Session["username"].ToString();
                dr["Log"] = "Non Task";
                dr["Hours"] = totalNonTask;
                dr["Percent"] = "";

                dtResult.Rows.Add(dr);
            }


            //ds = TotalNonTask2(Session["username"].ToString(), lastLogin, lastLogout);

            //if (ds.Tables["nonTask2"].Rows.Count > 0 && ds.Tables["nonTask2"].Rows[0]["hour"].ToString().Length > 0)
            //{
            //    totalNonTask2 = Convert.ToDouble(ds.Tables["nonTask2"].Rows[0]["hour"] + "." + ds.Tables["nonTask2"].Rows[0]["minute"]);
            //}

        }
        catch (Exception)
        {


        }



    }

    private void BindBarChart4()
    {
        try
        {
            string[] x = new string[dtResult.Rows.Count];
            double[] y = new double[dtResult.Rows.Count];

            for (int i = 0; i < dtResult.Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = dtResult.Rows[i]["Log"].ToString();
                y[i] = Convert.ToDouble(dtResult.Rows[i]["Hours"]);
            }

            Chart4.Series[0].Points.DataBindXY(x, y);
            //this.Chart1.Series[0]["PieLabelStyle"] = "Outside";
            this.Chart4.Series[0].BorderWidth = 1;
            this.Chart4.Series[0].Label = "#VALY";
            //this.Chart1.Series[0].LegendText = "#VALX (#VALY)"; // "#VALX (#PERCENT)";

            //Chart1.Series[0].ChartType = SeriesChartType.Column;
            //Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            //Chart1.Legends[0].Enabled = true;
            // Chart1.Series[0].Points[0].CustomProperties = "Exploded=true";
            if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
            {
                Chart4.Titles.Add(CreateTitle("Team Action Summary (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + ") - (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + ")"));
            }
            else
            {
                Chart4.Titles.Add(CreateTitle("Action Summary - Daily (" + lastLogin.ToShortDateString() + ")"));
            }

            Chart4.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart4.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        }
        catch (Exception)
        {


        }

        //string[] x = new string[dt.Rows.Count];
        //int[] y = new int[dt.Rows.Count];

        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]);
        //    if (span1.TotalMinutes != 0)
        //    {
        //        x[i] = dt.Rows[i][0].ToString();
        //        y[i] = Convert.ToInt32(span1.TotalMinutes);
        //    }
        //}
        //Chart1.Series[0].Points.DataBindXY(x, y);
        ////Chart1.ChartAreas[0].AxisY.Minimum = Chart1.Series[0].Points.FindMinByValue("Y1", 0).YValues[0] - 5;
        ////Chart1.ChartAreas[0].AxisY.Maximum = Chart1.Series[0].Points.FindMaxByValue("Y1", 0).YValues[0] + 5;
        ////Chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 90;
        ////Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 100;
        //Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        //Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;



    }

    //Line Chart Starts

    private DataSet TotalLineTask(string user, DateTime start, DateTime end)
    {
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))) as day, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_TeamId='" + Convert.ToInt32(Session["team"]) + "' and R_Start_Date_Time BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and R_Duration != 'HH:MM:SS' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))) as day, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and R_Duration != 'HH:MM:SS' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))", con);
        }

        da.Fill(ds, "LineTask");
        return ds;
    }

    private DataSet TotalLineBreak(string user, DateTime start, DateTime end)
    {
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day,sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and LA_Start_Date_Time BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Break' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day,sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Break' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }

        da.Fill(ds, "LineBreak");
        return ds;
    }

    private DataSet TotalLineMeeting(string user, DateTime start, DateTime end)
    {
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day,sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and LA_Start_Date_Time BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and (LA_Reason = 'Meetings' or LA_Reason = 'Meeting') GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day,sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and (LA_Reason = 'Meetings' or LA_Reason = 'Meeting') GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }

        da.Fill(ds, "LineMeeting");
        return ds;
    }

    private DataSet TotalLineConfCall(string user, DateTime start, DateTime end)
    {
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day,sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and LA_Start_Date_Time BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Conference-Call' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day,sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Conference-Call' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }

        da.Fill(ds, "LineCall");
        return ds;
    }

    private DataSet TotalLineNonTask1(string user, DateTime start, DateTime end)
    {
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day, sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and LA_Start_Date_Time BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and (LA_Reason = 'Others' or LA_Reason = 'Non-Task') GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day, sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and (LA_Reason = 'Others' or LA_Reason = 'Non-Task') GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }

        da.Fill(ds, "LinenonTask1");
        return ds;
    }

    private DataSet TotalLineNonTask2(string user, DateTime start, DateTime end)
    {
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))) as day, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records, RTM_SubTask_List where R_SubTask = STL_ID and R_TeamId='" + Convert.ToInt32(Session["team"]) + "' and R_Start_Date_Time BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and R_Duration != 'HH:MM:SS' and STL_SubTask = 'Non Task' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))) as day, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records, RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and R_Duration != 'HH:MM:SS' and STL_SubTask = 'Non Task' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))", con);
        }

        da.Fill(ds, "LinenonTask2");
        return ds;
    }

    private DataSet TotalLinePeerSupport(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("LinePeerSup"))
        {
            ds.Tables.Remove(ds.Tables["LinePeerSup"]);
        }
        if (Session["access"].ToString() == "3" || Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day, sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_TeamId='" + Convert.ToInt32(Session["team"]) + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Peer Support' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))) as day, sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Peer Support' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        }

        da.Fill(ds, "LinePeerSup");
        return ds;
    }

    private void BuildLineChartTable()
    {
        DataColumn dc;
        try
        {
            dc = new DataColumn("Day");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Log");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Hours");
            dtResult.Columns.Add(dc);

        }
        catch (Exception)
        {


        }
    }

    private void BindLineChartData()
    {

        // DataRow dr;
        Chart5.Titles.Add(CreateTitle("Action Summary - Weekly (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + ") - (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + ")"));
        // Chart5.ChartAreas["ChartArea1"].AxisY.Interval = 1;
        Chart5.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        Chart5.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        ds = TotalLineTask(Session["username"].ToString(), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString()), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString()));

        if (ds.Tables["LineTask"].Rows.Count > 0)
        {
            string[] x = new string[ds.Tables["LineTask"].Rows.Count];
            double[] y = new double[ds.Tables["LineTask"].Rows.Count];

            for (int i = 0; i < ds.Tables["LineTask"].Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = ds.Tables["LineTask"].Rows[i]["day"].ToString();
                //y[i] = Convert.ToDouble(ds.Tables["LineTask"].Rows[i]["Hour"] + "." + ds.Tables["LineTask"].Rows[i]["minute"]);
                y[i] = Math.Round(Convert.ToDouble(ds.Tables["LineTask"].Rows[i]["Hour"]) + (Convert.ToDouble(ds.Tables["LineTask"].Rows[i]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["LineTask"].Rows[i]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
            }
            // totalTask = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
            Chart5.Series[0].Points.DataBindXY(x, y);
            this.Chart5.Series[0].BorderWidth = 1;
            this.Chart5.Series[0].Label = "#VALY";


        }

        ds = TotalLineMeeting(Session["username"].ToString(), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString()), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString()));

        if (ds.Tables["LineMeeting"].Rows.Count > 0 && ds.Tables["LineMeeting"].Rows[0]["hour"].ToString().Length > 0)
        {
            // totalMeet = Convert.ToDouble(ds.Tables["Meeting"].Rows[0]["hour"] + "." + ds.Tables["Meeting"].Rows[0]["minute"]);
            string[] x = new string[ds.Tables["LineMeeting"].Rows.Count];
            double[] y = new double[ds.Tables["LineMeeting"].Rows.Count];

            for (int i = 0; i < ds.Tables["LineMeeting"].Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = ds.Tables["LineMeeting"].Rows[i]["day"].ToString();
                //y[i] = Convert.ToDouble(ds.Tables["LineMeeting"].Rows[i]["Hour"] + "." + ds.Tables["LineMeeting"].Rows[i]["minute"]);
                y[i] = Math.Round(Convert.ToDouble(ds.Tables["LineMeeting"].Rows[i]["Hour"]) + (Convert.ToDouble(ds.Tables["LineMeeting"].Rows[i]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["LineMeeting"].Rows[i]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
            }
            // totalTask = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
            Chart5.Series[1].Points.DataBindXY(x, y);
            this.Chart5.Series[1].BorderWidth = 1;
            this.Chart5.Series[1].Label = "#VALY";
            //Chart5.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            //Chart5.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

        }

        ds = TotalLineConfCall(Session["username"].ToString(), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString()), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString()));

        if (ds.Tables["LineCall"].Rows.Count > 0 && ds.Tables["LineCall"].Rows[0]["hour"].ToString().Length > 0)
        {
            //totalCall = Convert.ToDouble(ds.Tables["Call"].Rows[0]["hour"] + "." + ds.Tables["Call"].Rows[0]["minute"]);

            string[] x = new string[ds.Tables["LineCall"].Rows.Count];
            double[] y = new double[ds.Tables["LineCall"].Rows.Count];

            for (int i = 0; i < ds.Tables["LineCall"].Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = ds.Tables["LineCall"].Rows[i]["day"].ToString();
                //y[i] = Convert.ToDouble(ds.Tables["LineCall"].Rows[i]["Hour"] + "." + ds.Tables["LineCall"].Rows[i]["minute"]);
                y[i] = Math.Round(Convert.ToDouble(ds.Tables["LineCall"].Rows[i]["Hour"]) + (Convert.ToDouble(ds.Tables["LineCall"].Rows[i]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["LineCall"].Rows[i]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
            }
            // totalTask = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
            Chart5.Series[2].Points.DataBindXY(x, y);
            this.Chart5.Series[2].BorderWidth = 1;
            this.Chart5.Series[2].Label = "#VALY";
            //Chart5.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            //Chart5.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

        }

        ds = TotalLineBreak(Session["username"].ToString(), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString()), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString()));

        if (ds.Tables["LineBreak"].Rows.Count > 0 && ds.Tables["LineBreak"].Rows[0]["hour"].ToString().Length > 0)
        {
            // totalBreak = Convert.ToDouble(ds.Tables["Break"].Rows[0]["hour"] + "." + ds.Tables["Break"].Rows[0]["minute"]);
            string[] x = new string[ds.Tables["LineBreak"].Rows.Count];
            double[] y = new double[ds.Tables["LineBreak"].Rows.Count];

            for (int i = 0; i < ds.Tables["LineBreak"].Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = ds.Tables["LineBreak"].Rows[i]["day"].ToString();
                //y[i] = Convert.ToDouble(ds.Tables["LineBreak"].Rows[i]["Hour"] + "." + ds.Tables["LineBreak"].Rows[i]["minute"]);
                y[i] = Math.Round(Convert.ToDouble(ds.Tables["LineBreak"].Rows[i]["Hour"]) + (Convert.ToDouble(ds.Tables["LineBreak"].Rows[i]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["LineBreak"].Rows[i]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
            }
            // totalTask = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
            Chart5.Series[3].Points.DataBindXY(x, y);
            this.Chart5.Series[3].BorderWidth = 1;
            this.Chart5.Series[3].Label = "#VALY";
            //Chart5.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            //Chart5.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

        }

        ds = TotalLineNonTask1(Session["username"].ToString(), lastLogin, lastLogout);

        if (ds.Tables["LinenonTask1"].Rows.Count > 0 && ds.Tables["LinenonTask1"].Rows[0]["hour"].ToString().Length > 0)
        {
            // totalNonTask1 = Convert.ToDouble(ds.Tables["nonTask1"].Rows[0]["hour"] + "." + ds.Tables["nonTask1"].Rows[0]["minute"]);
            string[] x = new string[ds.Tables["LinenonTask1"].Rows.Count];
            double[] y = new double[ds.Tables["LinenonTask1"].Rows.Count];

            for (int i = 0; i < ds.Tables["LinenonTask1"].Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = ds.Tables["LinenonTask1"].Rows[i]["day"].ToString();
                //y[i] = Convert.ToDouble(ds.Tables["LinenonTask1"].Rows[i]["Hour"] + "." + ds.Tables["LinenonTask1"].Rows[i]["minute"]);
                y[i] = Math.Round(Convert.ToDouble(ds.Tables["LinenonTask1"].Rows[i]["Hour"]) + (Convert.ToDouble(ds.Tables["LinenonTask1"].Rows[i]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["LinenonTask1"].Rows[i]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
            }
            // totalTask = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
            Chart5.Series[4].Points.DataBindXY(x, y);
            this.Chart5.Series[4].BorderWidth = 1;
            this.Chart5.Series[4].Label = "#VALY";
        }

        //ds = TotalLineNonTask2(Session["username"].ToString(), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString()), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString()));

        //if (ds.Tables["LinenonTask2"].Rows.Count > 0 && ds.Tables["LinenonTask2"].Rows[0]["hour"].ToString().Length > 0)
        //{
        //    //totalNonTask2 = Convert.ToDouble(ds.Tables["nonTask2"].Rows[0]["hour"] + "." + ds.Tables["nonTask2"].Rows[0]["minute"]);
        //    string[] x = new string[ds.Tables["LinenonTask2"].Rows.Count];
        //    double[] y = new double[ds.Tables["LinenonTask2"].Rows.Count];

        //    for (int i = 0; i < ds.Tables["LinenonTask2"].Rows.Count; i++)
        //    {
        //        //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
        //        x[i] = ds.Tables["LinenonTask2"].Rows[i]["day"].ToString();
        //        y[i] = Convert.ToDouble(ds.Tables["LinenonTask2"].Rows[i]["Hour"] + "." + ds.Tables["LinenonTask2"].Rows[i]["minute"]);
        //    }
        //    // totalTask = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
        //    Chart5.Series[4].Points.DataBindXY(x, y);
        //    this.Chart5.Series[4].BorderWidth = 1;
        //    this.Chart5.Series[4].Label = "#VALY";
        //    //Chart5.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        //    //Chart5.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        //}

        // totalNonTask = totalNonTask1 + totalNonTask2;

        ds = TotalLinePeerSupport(Session["username"].ToString(), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString()), Convert.ToDateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString()));

        if (ds.Tables["LinePeerSup"].Rows.Count > 0 && ds.Tables["LinePeerSup"].Rows[0]["hour"].ToString().Length > 0)
        {
            string[] x = new string[ds.Tables["LinePeerSup"].Rows.Count];
            double[] y = new double[ds.Tables["LinePeerSup"].Rows.Count];

            for (int i = 0; i < ds.Tables["LinePeerSup"].Rows.Count; i++)
            {
                //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
                x[i] = ds.Tables["LinePeerSup"].Rows[i]["day"].ToString();
                //y[i] = Convert.ToDouble(ds.Tables["LinePeerSup"].Rows[i]["Hour"] + "." + ds.Tables["LinePeerSup"].Rows[i]["minute"]);
                y[i] = Math.Round(Convert.ToDouble(ds.Tables["LinePeerSup"].Rows[i]["Hour"]) + (Convert.ToDouble(ds.Tables["LinePeerSup"].Rows[i]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["LinePeerSup"].Rows[i]["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
            }
            // totalTask = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
            Chart5.Series[5].Points.DataBindXY(x, y);
            this.Chart5.Series[5].BorderWidth = 1;
            this.Chart5.Series[5].Label = "#VALY";
        }

    }

    private void TotalWorkHours()
    {
        try
        {
            TodaysLogin1();
            Label1.Text = "Total Working hours (" + CurrentLogin.ToShortDateString() + "):";

            TimeSpan span1 = TimeSpan.Parse("00:00:00");
            TimeSpan span2 = TimeSpan.Parse("00:00:00");

            ds = TotalTaskHours(Session["username"].ToString(), CurrentLogin);

            if (ds.Tables["TotalTaskHours"].Rows.Count > 0 && ds.Tables["TotalTaskHours"].Rows[0]["Hour"].ToString().Length != 0)
            {
                span1 = TimeSpan.Parse(ds.Tables["TotalTaskHours"].Rows[0]["Hour"] + ":" + ds.Tables["TotalTaskHours"].Rows[0]["minute"] + ":" + ds.Tables["TotalTaskHours"].Rows[0]["seconds"]);
            }

            ds = TotalLogHours(Session["username"].ToString(), CurrentLogin);
            if (ds.Tables["TotalLogHours"].Rows.Count > 0 && ds.Tables["TotalLogHours"].Rows[0]["Hour"].ToString().Length != 0)
            {
                span2 = TimeSpan.Parse(ds.Tables["TotalLogHours"].Rows[0]["Hour"] + ":" + ds.Tables["TotalLogHours"].Rows[0]["minute"] + ":" + ds.Tables["TotalLogHours"].Rows[0]["seconds"]);
            }

            lblHours.Text = Convert.ToString(span1.Add(span2));
        }
        catch (Exception)
        {


        }

    }

    private DataSet TotalTaskHours(string user, DateTime start)
    {
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time >= '" + start + "' and R_Duration != 'HH:MM:SS' and STL_SubTask != 'Non Task'", con);
        da.Fill(ds, "TotalTaskHours");
        return ds;
    }

    private void TodaysLogin1()
    {
        da = new SqlDataAdapter("SELECT TOP 1 LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + Session["username"].ToString() + "' and LA_Log_Action = 'Actual Login' ORDER BY LA_ID DESC", con);
        da.Fill(ds, "TodaysLogin");
        if (ds.Tables["TodaysLogin"].Rows.Count > 0)
        {
            CurrentLogin = Convert.ToDateTime(ds.Tables["TodaysLogin"].Rows[0]["LA_Start_Date_Time"]);
        }
    }

    private DataSet TotalLogHours(string user, DateTime start)
    {
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time >= '" + start + "'  and LA_Duration != 'HH:MM:SS' and LA_Reason != 'Break' and LA_Reason != 'Others'", con);
        da.Fill(ds, "TotalLogHours");
        return ds;
    }


    //Resource utilization report start...............................

    private DataSet GetEmployees(int teamId)
    {
        da = new SqlDataAdapter("SELECT UL_User_Name FROM RTM_User_List WHERE UL_Team_Id = '" + teamId + "'", con);
        da.Fill(ds, "users");
        return ds;
    }

    private DataSet GetLoginTime(string user)
    {
        da = new SqlDataAdapter("SELECT LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Log_Action = 'Actual Login'", con);
        da.Fill(ds, "Login");
        return ds;
    }

    private DataSet GetLogoutTime(string user, DateTime date1)
    {
        da = new SqlDataAdapter("SELECT TOP 1 LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + user + "' and LA_Start_Date_Time >= '" + date1 + "' and LA_Log_Action = 'Actual Logout'", con);
        da.Fill(ds, "Logout");
        return ds;
    }

    private DataSet GetTaskHours(string user, DateTime start, DateTime end)
    {
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and R_Duration != 'HH:MM:SS' and STL_SubTask != 'Non Task'", con);
        da.Fill(ds, "Task");
        return ds;
    }

    private DataSet GetLogHours(string user, DateTime start, DateTime end)
    {
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason != 'Break' and (LA_Reason != 'Others' or LA_Reason != 'Non-Task')", con);
        da.Fill(ds, "Log");
        return ds;
    }

    private DataSet GetLeaves(string user)
    {
        da = new SqlDataAdapter("SELECT CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) From RTM_Log_Actions where LA_User_Name = '" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + "' and '" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + "' and LA_Log_Action = 'Actual Login' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        da.Fill(ds, "Leaves");
        return ds;
    }

    private DataSet GetNonTaskRecords(string user, DateTime start, DateTime end)
    {
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records, RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and R_Duration != 'HH:MM:SS' and STL_SubTask = 'Non Task'", con);
        da.Fill(ds, "NonTaskRecords");
        return ds;
    }

    private DataSet GetNonTaskLogs(string user, DateTime start, DateTime end)
    {
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Others'", con);
        da.Fill(ds, "NonTaskLog");
        return ds;
    }

    private void BuildRUTable()
    {
        DataColumn dc;
        try
        {
            dc = new DataColumn("Employee Name");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Team Name");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Total Hours", typeof(double));
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Absent Hours");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("RU%");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("NonTask");
            dtResult.Columns.Add(dc);

        }
        catch (Exception)
        {


        }
    }

    private void BuildRUChart()
    {


        string[] x = new string[dtResult.Rows.Count];
        double[] y = new double[dtResult.Rows.Count];

        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
            x[i] = dtResult.Rows[i]["Employee Name"].ToString();
            //val1 = Convert.ToDecimal(dtResult.Rows[i]["Percent"]);
            //y[i] = (int)val1;
            y[i] = Convert.ToDouble(dtResult.Rows[i]["Total Hours"]);
            if (i == 4)
            {
                break;
            }
        }

        Chart2.Series[0].Points.DataBindXY(x, y);
        //this.Chart1.Series[0].BorderWidth = 1;
        //Chart2.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        //Chart2.ChartAreas["ChartArea1"].AxisY.Interval = 5;
        //Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Employee Names";
        //Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Percentage";
        //Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        //Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        //Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
        //this.Chart2.Series[0].Label = "#VALY %";
        this.Chart2.Series[0].Label = "#PERCENT{P2}";
        Chart2.Titles.Add(CreateTitle("Resource Utilization - " + TeamName + " (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + ") - (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + ")"));
        //gvReport.DataSource = dtResult;
        //gvReport.DataBind();

    }

    private void BindURChart()
    {
        DataRow dr;

        double totalTaskHours = 0;
        double totalLogHours = 0;
        double totalWorkHours;
        double RUPercent = 0;
        int leaveHours = 0;
        double nonTaskRecord = 0;
        double nonTaskLog = 0;
        double totalNonTaskHours = 0;

        BuildRUTable();

        //ds = GetTeamName(Convert.ToInt32(Session["team"]));

        //if (ds.Tables["Team"].Rows.Count > 0)
        //{
        //    TeamName = ds.Tables["Team"].Rows[0]["T_TeamName"].ToString();
        //}

        //TeamName = ddlEmp.SelectedItem.Text;

        ds = GetEmployees(Convert.ToInt32(Session["team"]));

        if (ds.Tables["users"].Rows.Count > 0)
        {
            foreach (DataRow dr1 in ds.Tables["users"].Rows)
            {
                if (ds.Tables.Contains("Login"))
                {
                    ds.Tables.Remove(ds.Tables["Login"]);
                }
                if (ds.Tables.Contains("Logout"))
                {
                    ds.Tables.Remove(ds.Tables["Logout"]);
                }
                if (ds.Tables.Contains("Task"))
                {
                    ds.Tables.Remove(ds.Tables["Task"]);
                }
                if (ds.Tables.Contains("Log"))
                {
                    ds.Tables.Remove(ds.Tables["Log"]);
                }
                if (ds.Tables.Contains("Leaves"))
                {
                    ds.Tables.Remove(ds.Tables["Leaves"]);
                }
                if (ds.Tables.Contains("NonTaskRecords"))
                {
                    ds.Tables.Remove(ds.Tables["NonTaskRecords"]);
                }
                if (ds.Tables.Contains("NonTaskLog"))
                {
                    ds.Tables.Remove(ds.Tables["NonTaskLog"]);
                }

                totalTaskHours = 0;
                totalLogHours = 0;
                totalWorkHours = 0;
                RUPercent = 0;
                leaveHours = 0;
                nonTaskRecord = 0;
                nonTaskLog = 0;
                totalNonTaskHours = 0;
                string username = dr1["UL_User_Name"].ToString();


                ds = GetLoginTime(username);

                if (ds.Tables["Login"].Rows.Count > 0)
                {
                    DateTime loginTime = Convert.ToDateTime(ds.Tables["Login"].Rows[0]["LA_Start_Date_Time"]);

                    DataRow lastRow = ds.Tables["Login"].Rows[ds.Tables["Login"].Rows.Count - 1];


                    ds = GetLogoutTime(username, Convert.ToDateTime(lastRow["LA_Start_Date_Time"]));
                    if (ds.Tables["Logout"].Rows.Count > 0)
                    {
                        logoutTime = Convert.ToDateTime(ds.Tables["Logout"].Rows[0]["LA_Start_Date_Time"]);

                        ds = GetTaskHours(username, loginTime, logoutTime);

                        if (ds.Tables["Task"].Rows.Count > 0 && ds.Tables["Task"].Rows[0]["hour"].ToString().Length > 0)
                        {
                            //totalTaskHours = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"] + "." + ds.Tables["Task"].Rows[0]["minute"]);
                            totalTaskHours = Math.Round(Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"]) + Convert.ToDouble(ds.Tables["Task"].Rows[0]["minute"]) / 60, 2, MidpointRounding.AwayFromZero);
                        }


                        ds = GetLogHours(username, loginTime, logoutTime);

                        if (ds.Tables["Log"].Rows.Count > 0 && ds.Tables["Log"].Rows[0]["hour"].ToString().Length > 0)
                        {
                            //totalLogHours = Convert.ToDouble(ds.Tables["Log"].Rows[0]["hour"] + "." + ds.Tables["Log"].Rows[0]["minute"]);
                            totalLogHours = Math.Round(Convert.ToDouble(ds.Tables["Log"].Rows[0]["hour"]) + Convert.ToDouble(ds.Tables["Log"].Rows[0]["minute"]) / 60, 2, MidpointRounding.AwayFromZero);
                        }

                        totalWorkHours = totalTaskHours + totalLogHours;

                        ds = GetLeaves(username);
                        if (ds.Tables["Leaves"].Rows.Count <= 5)
                        {
                            leaveHours = (5 - Convert.ToInt32(ds.Tables["Leaves"].Rows.Count)) * 8;
                        }

                        if (totalWorkHours != 0)
                        {
                            RUPercent = (totalWorkHours / (40 - leaveHours)) * 100;
                            RUPercent = Math.Round(RUPercent, 2, MidpointRounding.AwayFromZero);
                        }

                        ds = GetNonTaskRecords(username, loginTime, logoutTime);

                        if (ds.Tables["NonTaskRecords"].Rows.Count > 0 && ds.Tables["NonTaskRecords"].Rows[0]["hour"].ToString().Length > 0)
                        {
                            //nonTaskRecord = Convert.ToDouble(ds.Tables["NonTaskRecords"].Rows[0]["hour"] + "." + ds.Tables["NonTaskRecords"].Rows[0]["minute"]);
                            nonTaskRecord = Math.Round(Convert.ToDouble(ds.Tables["NonTaskRecords"].Rows[0]["hour"]) + Convert.ToDouble(ds.Tables["NonTaskRecords"].Rows[0]["minute"]) / 60, 2, MidpointRounding.AwayFromZero);
                        }

                        ds = GetNonTaskLogs(username, loginTime, logoutTime);

                        if (ds.Tables["NonTaskLog"].Rows.Count > 0 && ds.Tables["NonTaskLog"].Rows[0]["hour"].ToString().Length > 0)
                        {
                            // nonTaskLog = Convert.ToDouble(ds.Tables["NonTaskLog"].Rows[0]["hour"] + "." + ds.Tables["NonTaskLog"].Rows[0]["minute"]);
                            nonTaskLog = Math.Round(Convert.ToDouble(ds.Tables["NonTaskLog"].Rows[0]["hour"]) + Convert.ToDouble(ds.Tables["NonTaskLog"].Rows[0]["minute"]) / 60, 2, MidpointRounding.AwayFromZero);
                        }

                        totalNonTaskHours = nonTaskRecord + nonTaskLog;

                        dr = dtResult.NewRow();

                        dr["Employee Name"] = username;
                        dr["Team Name"] = TeamName;
                        dr["Total Hours"] = totalWorkHours;
                        dr["Absent Hours"] = leaveHours;
                        dr["RU%"] = RUPercent;
                        dr["NonTask"] = totalNonTaskHours;

                        dtResult.Rows.Add(dr);


                    }
                }
            }
            DataView view = dtResult.DefaultView;
            view.Sort = "Total Hours DESC";
            dtResult = view.ToTable();

            BuildRUChart();
        }
    }

    private DataSet LoadRUDetails()
    {
        if (ds.Tables.Contains("ru"))
        {
            ds.Tables.Remove(ds.Tables["ru"]);
        }
        da = new SqlDataAdapter("select TOP 5 RU_UserName, RU_Percent from RTM_ResourceUtil where RU_TeamId ='" + Convert.ToInt32(Session["team"]) + "' order by RU_Percent desc", con);
        da.Fill(ds, "ru");
        return ds;
    }

    private void BindRUPieChart()
    {
        ds = LoadRUDetails();

        string[] x = new string[ds.Tables["ru"].Rows.Count];
        double[] y = new double[ds.Tables["ru"].Rows.Count];

        for (int i = 0; i < ds.Tables["ru"].Rows.Count; i++)
        {
            x[i] = ds.Tables["ru"].Rows[i]["RU_UserName"].ToString();
            y[i] = Convert.ToDouble(ds.Tables["ru"].Rows[i]["RU_Percent"]);
        }

        Chart2.Series[0].Points.DataBindXY(x, y);

        this.Chart2.Series[0].BorderWidth = 1;
        this.Chart2.Series[0].Label = "#VALY %";
        this.Chart2.Series[0].LegendText = "#VALX (#VALY)";

        Chart2.Titles.Add(CreateTitle("Resource Utilization - " + TeamName + " (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6).ToShortDateString() + ") - (" + DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).ToShortDateString() + ")"));
    }

    protected void Chart2_Click(object sender, ImageMapEventArgs e)
    {
        Response.Redirect("~/RUChart.aspx");
    }

    protected void Chart3_Click(object sender, ImageMapEventArgs e)
    {
        Response.Redirect("~/ClentUsageTeam.aspx?type=Tasks");
    }
}