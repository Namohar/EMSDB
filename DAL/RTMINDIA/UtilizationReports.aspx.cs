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
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using HtmlPivot;
using System.Web.UI.HtmlControls;
using BAL;

public partial class UtilizationReports : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    clsRealTimeReports objReal = new clsRealTimeReports();
    SqlDataAdapter da;
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    DataSet ds = new DataSet();
    DataTable dtResult = new DataTable();
    string TeamName;
    decimal total;
    TimeSpan span1;
    TimeSpan span2;
    decimal val1;
    decimal val2;

    double totalEMS;
    double totalCMP;
    double totalIPV;
    double totalOthers;
    double totalHCL;
    string[] x;
    double[] y;
    DateTime logoutTime;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["team"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {
            LoadEmp();

            if (Session["access"].ToString() == "4")
            {
                rbServiceCode.Visible = false;
                rbJobCode.Visible = false;
                rbProductCode.Visible = false;
                rbTaskPer.Visible = false;
                rbRU.Visible = false;
                RadioButton1.Visible = false;
            }
        }
    }

    private void LoadEmp()
    {
        
        //if (Session["access"].ToString() == "1")
        //{
        //    da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_User_Status =1 ORDER BY UL_User_Name", con);
        //}
        //else if (Session["access"].ToString() == "2")
        //{
        //    da = new SqlDataAdapter("select * from RTM_User_List where UL_Team_Id IN (Select T_ID from RTM_Team_List where T_Manager = '" + Session["username"].ToString() + "') and UL_User_Status =1 ", con);
        //}
        //else if (Session["access"].ToString() == "3")
        //{
        //    da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_Team_Id ='" + Convert.ToInt32(Session["team"]) + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        //}
        //else if (Session["access"].ToString() == "4")
        //{
        //    da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_System_User_Name='" + Session["user"].ToString() + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        //}
        ////da = new SqlDataAdapter("SELECT * FROM RTM_User_List ORDER BY UL_User_Name", con);
        //da.Fill(dt);
        dt = new DataTable();
        dt = objReal.LoadEmp(Session["access"].ToString(), Session["Location"].ToString(), Session["username"].ToString(), Session["team"].ToString(), Session["user"].ToString());

        ddlEmp.DataSource = dt;
        ddlEmp.DataValueField = "UL_ID";
        ddlEmp.DataTextField = "UL_User_Name";
        ddlEmp.DataBind();

        ddlEmp.Items.Insert(0, "--Select Employee--");
        ddlEmp.SelectedIndex = 0;

        lblEmp.Text = "Select Employee:";
        //ddlEmp.Items.Insert(0, "All"); 
        //ddlEmp.SelectedIndex = 0;
    }

    private void LoadTeams()
    {
        //if (Session["access"].ToString() == "1")
        //{
        //    da = new SqlDataAdapter("SELECT * From RTM_Team_List where T_Active = 1 order by T_TeamName", con);
        //}
        //else if (Session["access"].ToString() == "2")
        //{
        //    da = new SqlDataAdapter("SELECT * From RTM_Team_List where T_Manager ='" + Session["username"].ToString() + "' and T_Active = 1 order by T_TeamName", con);
        //}
        //else if (Session["access"].ToString() == "3")
        //{
        //    da = new SqlDataAdapter("SELECT * From RTM_Team_List where T_ID = '" + Convert.ToInt32(Session["team"]) + "' and T_Active = 1", con);
        //}


        //da.Fill(dt);
        dt = new DataTable();
        dt = objReal.LoadTeams(Session["access"].ToString(), Session["Location"].ToString(), Session["username"].ToString(), Session["team"].ToString());

        ddlEmp.DataSource = dt;
        ddlEmp.DataValueField = "T_ID";
        ddlEmp.DataTextField = "T_TeamName";
        ddlEmp.DataBind();

        ddlEmp.Items.Insert(0, "--Select Team--");
        ddlEmp.SelectedIndex = 0;

        lblEmp.Text = "Select Team:";
    }

    private void FetchTasks()
    {
        dt = new DataTable();
        da = new SqlDataAdapter("Select * from RTM_Task_List where TL_TeamId = '" + ddlEmp.SelectedValue + "' order by TL_Task", con);
        da.Fill(dt);
        ddlTask.DataSource = dt;
        ddlTask.DataTextField = "TL_Task";
        ddlTask.DataValueField = "TL_ID";
        ddlTask.DataBind();
    }

    protected void rbClient_CheckedChanged(object sender, EventArgs e)
    {
        if (rbClient.Checked == true)
        {
            if (rbUser.Checked == true)
            {
                LoadEmp();
            }
            else if (rbTeam.Checked == true)
            {
                LoadTeams();
            }

            datepickerTo.Visible = true;
            lblTo.Visible = true;
            lblEmp.Visible = true;
            ddlEmp.Visible = true;
            Chart1.Visible = false;
            btnExport.Visible = false;
            btnPDF.Visible = false;
            gvPeerSupport.Visible = false;
            div2.Visible = false;
            lblHeading.Visible = false;
            lblTask.Visible = false;
            ddlTask.Visible = false;

            rbUser.Visible = true;
            rbTeam.Visible = true;

            btnDetail.Visible = false;
        }
    }
    protected void rbTask_CheckedChanged(object sender, EventArgs e)
    {
        if (rbTask.Checked == true)
        {
            if (rbUser.Checked == true)
            {
                LoadEmp();
            }
            else if (rbTeam.Checked == true)
            {
                LoadTeams();
            }
            datepickerTo.Visible = true;
            lblTo.Visible = true;
            lblEmp.Visible = true;
            ddlEmp.Visible = true;
            Chart1.Visible = false;
            btnExport.Visible = false;
            btnPDF.Visible = false;
            gvPeerSupport.Visible = false;

            div2.Visible = false;
            lblHeading.Visible = false;
            lblTask.Visible = false;
            ddlTask.Visible = false;

            rbUser.Visible = true;
            rbTeam.Visible = true;

            btnDetail.Visible = false;
        }
    }
    protected void rbSubTask_CheckedChanged(object sender, EventArgs e)
    {
        if (rbSubTask.Checked == true)
        {
            LoadTeams();
            datepickerTo.Visible = true;
            lblTo.Visible = true;
            lblEmp.Visible = true;
            ddlEmp.Visible = true;
            Chart1.Visible = false;
            btnExport.Visible = false;
            btnPDF.Visible = false;
            gvPeerSupport.Visible = false;

            rbUser.Visible = false;
            rbTeam.Visible = false;

            btnDetail.Visible = false;
        }
    }
    protected void rbServiceCode_CheckedChanged(object sender, EventArgs e)
    {
        if (rbServiceCode.Checked == true)
        {
            LoadTeams();
            datepickerTo.Visible = true;
            lblTo.Visible = true;
            Chart1.Visible = false;
            btnExport.Visible = false;
            btnPDF.Visible = false;
            gvPeerSupport.Visible = false;
            div2.Visible = false;
            lblHeading.Visible = false;
            lblTask.Visible = false;
            ddlTask.Visible = false;

            rbUser.Visible = false;
            rbTeam.Visible = false;

            btnDetail.Visible = false;
        }
    }
    protected void rbTaskPer_CheckedChanged(object sender, EventArgs e)
    {
        if (rbTaskPer.Checked == true)
        {
            LoadTeams();
            datepickerTo.Visible = true;
            lblTo.Visible = true;

            Chart1.Visible = false;
            btnExport.Visible = false;
            btnPDF.Visible = false;
            gvPeerSupport.Visible = false;

            div2.Visible = false;
            lblHeading.Visible = false;
            lblTask.Visible = false;
            ddlTask.Visible = false;

            rbUser.Visible = false;
            rbTeam.Visible = false;

            btnDetail.Visible = false;
        }
    }
    protected void rbJobCode_CheckedChanged(object sender, EventArgs e)
    {
        if (rbJobCode.Checked == true)
        {
            LoadTeams();
            datepickerTo.Visible = true;
            lblTo.Visible = true;

            Chart1.Visible = false;
            btnExport.Visible = false;
            btnPDF.Visible = false;
            gvPeerSupport.Visible = false;
            div2.Visible = false;
            lblHeading.Visible = false;
            lblTask.Visible = false;
            ddlTask.Visible = false;

            rbUser.Visible = false;
            rbTeam.Visible = false;

            btnDetail.Visible = false;
        }
    }
    protected void rbProductCode_CheckedChanged(object sender, EventArgs e)
    {
        if (rbProductCode.Checked == true)
        {
            LoadTeams();
            datepickerTo.Visible = true;
            lblTo.Visible = true;

            Chart1.Visible = false;
            btnExport.Visible = false;
            btnPDF.Visible = false;
            gvPeerSupport.Visible = false;
            div2.Visible = false;
            lblHeading.Visible = false;
            lblTask.Visible = false;
            ddlTask.Visible = false;

            rbUser.Visible = false;
            rbTeam.Visible = false;

            btnDetail.Visible = false;
        }
    }
    protected void rbRU_CheckedChanged(object sender, EventArgs e)
    {
        if (rbRU.Checked == true)
        {
            LoadTeams();
            datepickerTo.Visible = true;
            lblTo.Visible = true;

            Chart1.Visible = false;
            btnExport.Visible = false;
            btnPDF.Visible = false;
            gvPeerSupport.Visible = false;
            div2.Visible = false;
            lblHeading.Visible = false;
            lblTask.Visible = false;
            ddlTask.Visible = false;

            rbUser.Visible = false;
            rbTeam.Visible = false;

            btnDetail.Visible = false;
        }
    }
    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        LoadTeams();
        datepickerTo.Visible = true;
        lblTo.Visible = true;
        Chart1.Visible = false;
        btnExport.Visible = false;
        btnPDF.Visible = false;
        gvPeerSupport.Visible = false;
        rbUser.Visible = false;
        rbTeam.Visible = false;
    }
    protected void rbUser_CheckedChanged(object sender, EventArgs e)
    {
        LoadEmp();

        datepickerTo.Visible = true;
        lblTo.Visible = true;
        lblEmp.Visible = true;
        ddlEmp.Visible = true;
        Chart1.Visible = false;
        btnExport.Visible = false;
        btnPDF.Visible = false;
        gvPeerSupport.Visible = false;
        div2.Visible = false;
        lblHeading.Visible = false;
        lblTask.Visible = false;
        ddlTask.Visible = false;
    }
    protected void rbTeam_CheckedChanged(object sender, EventArgs e)
    {
        LoadTeams();

        datepickerTo.Visible = true;
        lblTo.Visible = true;
        lblEmp.Visible = true;
        ddlEmp.Visible = true;
        Chart1.Visible = false;
        btnExport.Visible = false;
        btnPDF.Visible = false;
        gvPeerSupport.Visible = false;
        div2.Visible = false;
        lblHeading.Visible = false;
        lblTask.Visible = false;
        ddlTask.Visible = false;
    }
    protected void ddlEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbSubTask.Checked == true)
        {
            FetchTasks();
            lblTask.Visible = true;
            ddlTask.Visible = true;
        }
        else
        {
            lblTask.Visible = false;
            ddlTask.Visible = false;
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddlEmp.SelectedIndex == 0)
            {
                if (lblEmp.Text == "Select Employee")
                {
                    lblError.Text = "Please select User Name";
                }
                else
                {
                    lblError.Text = "Please select Team Name";
                }

                lblError.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (datepicker.Visible == true)
            {
                if (datepicker.Value.Length > 0)
                {
                    lblError.Text = "";
                }
                else
                {
                    lblError.Text = "Please Select Date";
                    lblError.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
            if (rbClient.Checked == true)
            {
                PrintByClientReport();

                //string strScript;
                //Session["rptSrc"] = dtResult;

                //strScript = "<script language=javascript>window.open('ByClientReport.aspx')</script>";
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "strScript", strScript);

            }
            else if (rbTask.Checked == true)
            {
                PrintByTaskReport();

                //string strScript;
                //Session["rptSrc"] = dtResult;

                //strScript = "<script language=javascript>window.open('ByTaskReport.aspx')</script>";
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "strScript", strScript);
            }
            else if (rbSubTask.Checked == true)
            {
                if (Convert.ToDateTime(datepicker.Value).DayOfWeek != DayOfWeek.Monday)
                {
                    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
                else if (Convert.ToDateTime(datepickerTo.Value) != Convert.ToDateTime(datepicker.Value).AddDays(6))
                {
                    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    PrintBySubTaskReport();
                    lblHeading.Text = "Hours Utilized on Sub-Tasks (" + datepicker.Value + " - " + datepickerTo.Value + ")";
                    div2.Visible = true;
                    lblHeading.Visible = true;
                }
            }
            else if (rbServiceCode.Checked == true)
            {
                //if (Convert.ToDateTime(datepicker.Value).DayOfWeek != DayOfWeek.Monday)
                //{
                //    lblError.Text = "Please Select the date from Monday to its subsequent Sunday";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //}
                //else if (Convert.ToDateTime(datepickerTo.Value) != Convert.ToDateTime(datepicker.Value).AddDays(6))
                //{
                //    lblError.Text = "Please Select the date from Monday to its subsequent Sunday";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //}
                //else
                //{
                //    BindGrid();
                //}

                BindGrid();

            }
            else if (rbJobCode.Checked == true)
            {
                //if (Convert.ToDateTime(datepicker.Value).DayOfWeek != DayOfWeek.Monday)
                //{
                //    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //}
                //else if (Convert.ToDateTime(datepickerTo.Value) != Convert.ToDateTime(datepicker.Value).AddDays(6))
                //{
                //    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //}
                //else
                //{
                //    BindJobCodeChart();
                //}
                BindJobCodeChart();
            }
            else if (rbProductCode.Checked == true)
            {
                //if (Convert.ToDateTime(datepicker.Value).DayOfWeek != DayOfWeek.Monday)
                //{
                //    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //}
                //else if (Convert.ToDateTime(datepickerTo.Value) != Convert.ToDateTime(datepicker.Value).AddDays(6))
                //{
                //    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //}
                //else
                //{
                //    BindProductCodeChart();
                //}

                BindProductCodeChart();
            }

            else if (rbRU.Checked == true)
            {
                if (Convert.ToDateTime(datepicker.Value).DayOfWeek != DayOfWeek.Monday)
                {
                    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
                else if (Convert.ToDateTime(datepickerTo.Value) != Convert.ToDateTime(datepicker.Value).AddDays(6))
                {
                    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    BindURChart();
                }

            }
            else if (rbTaskPer.Checked == true)
            {
                //if (Convert.ToDateTime(datepicker.Value).DayOfWeek != DayOfWeek.Monday)
                //{
                //    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //}
                //else if (Convert.ToDateTime(datepickerTo.Value) != Convert.ToDateTime(datepicker.Value).AddDays(6))
                //{
                //    lblError.Text = "Please Select the date from Monday to Sunday of same week";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //}
                //else
                //{

                //}
                BindTaskChart();
            }
            else if (RadioButton1.Checked == true)
            {
                BindPeerSupport();
            }
        }
        catch (Exception)
        {


        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlEmp.SelectedIndex = 0;
        datepicker.Value = string.Empty;
        datepickerTo.Value = string.Empty;
        gvPeerSupport.Visible = false;
    }

    private void buildByTaskTable()
    {
        DataColumn dc;
        try
        {

            dc = new DataColumn("EmpName");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Client");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Task");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Date", typeof(DateTime));
            dtResult.Columns.Add(dc);

            dc = new DataColumn("ToDate", typeof(DateTime));
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Duration");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("SubTask");
            dtResult.Columns.Add(dc);

        }
        catch (Exception)
        {


        }
    }

    private DataTable LoadByTaskReport(DateTime login, DateTime logout)
    {
        da = new SqlDataAdapter("SELECT A.TL_Task, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Task_List A, RTM_Records B where A.TL_ID  = B.R_Task and R_User_Name='" + ddlEmp.SelectedItem.Text + "' and R_Start_Date_Time BETWEEN '" + login + "' and '" + logout + "' and R_Duration != 'HH:MM:SS' GROUP BY A.TL_Task", con);
        da.Fill(dt);
        return dt;
    }

    private DataTable LoadByTaskReportNew(string login, string logout)
    {
        if (rbUser.Checked)
        {
            da = new SqlDataAdapter("SELECT A.TL_Task, C.STL_SubTask, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Task_List A, RTM_Records B, RTM_SubTask_List C where A.TL_ID  = B.R_Task and C.STL_ID =B.R_SubTask and R_User_Name='" + ddlEmp.SelectedItem.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + login + "' and '" + logout + "' and R_Duration != 'HH:MM:SS' GROUP BY A.TL_Task, C.STL_SubTask", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT A.TL_Task, C.STL_SubTask, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Task_List A, RTM_Records B, RTM_SubTask_List C where A.TL_ID  = B.R_Task and C.STL_ID =B.R_SubTask and R_TeamId='" + ddlEmp.SelectedValue + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + login + "' and '" + logout + "' and R_Duration != 'HH:MM:SS' GROUP BY A.TL_Task, C.STL_SubTask", con);
        }

        da.Fill(dt);
        return dt;
    }

    private DataSet LoadMeetings(string login, string logout)
    {
        if (rbUser.Checked)
        {
            da = new SqlDataAdapter("SELECT " +
          "ISNULL(sum(datediff(second,'00:00:00',LA_Duration))/3600,00) as hour," +
          "ISNULL((sum(datediff(second,'00:00:00',LA_Duration))/60)%60,00) as minute," +
          "ISNULL((sum(datediff(second,'00:00:00',LA_Duration))-(((sum(datediff(second,'00:00:00',LA_Duration))/3600)*3600)-60*((sum(datediff(second,'00:00:00',LA_Duration))/60)%60)))%60,00) as seconds " +
          "from RTM_Log_Actions " +
          "where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + login + "' AND '" + logout + "' and (LA_Reason ='Meetings' or LA_Reason ='Meeting') and LA_User_Name = '" + ddlEmp.SelectedItem.Text + "' and LA_Duration != 'HH:MM:SS'", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT " +
          "ISNULL(sum(datediff(second,'00:00:00',LA_Duration))/3600,00) as hour," +
          "ISNULL((sum(datediff(second,'00:00:00',LA_Duration))/60)%60,00) as minute," +
          "ISNULL((sum(datediff(second,'00:00:00',LA_Duration))-(((sum(datediff(second,'00:00:00',LA_Duration))/3600)*3600)-60*((sum(datediff(second,'00:00:00',LA_Duration))/60)%60)))%60,00) as seconds " +
          "from RTM_Log_Actions " +
          "where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + login + "' AND '" + logout + "' and (LA_Reason ='Meetings' or LA_Reason ='Meeting') and LA_TeamId = '" + ddlEmp.SelectedValue + "' and LA_Duration != 'HH:MM:SS'", con);
        }

        da.Fill(ds, "Meeting");
        return ds;
    }

    private void PrintByTaskReport()
    {
        DataRow dr;

        buildByTaskTable();
        //ds = GetLogin();
        //if (ds.Tables["login"].Rows.Count > 0)
        //{
        //    DateTime loginTime = Convert.ToDateTime(ds.Tables["login"].Rows[0]["LA_Start_Date_Time"]);

        //    ds = GetLogoutNew(loginTime);
        //    if (ds.Tables["logout"].Rows.Count > 0)
        //    {
        //        DateTime logoutTime = Convert.ToDateTime(ds.Tables["logout"].Rows[0]["LA_Start_Date_Time"]);

        //        dt = LoadByTaskReportNew(loginTime, logoutTime);

        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr1 in dt.Rows)
        //            {
        //                dr = dtResult.NewRow();

        //                dr["EmpName"] = ddlEmp.SelectedItem.Text;
        //                dr["Client"] = "";
        //                dr["Task"] = dr1["TL_Task"];
        //                dr["SubTask"] = dr1["STL_SubTask"];
        //                dr["Date"] = Convert.ToDateTime(datepicker.Value);
        //                dr["ToDate"] = Convert.ToDateTime(datepickerTo.Value);
        //                dr["Duration"] = Convert.ToInt32(dr1["hour"]).ToString("00") + ":" + Convert.ToInt32(dr1["minute"]).ToString("00") + ":" + Convert.ToInt32(dr1["seconds"]).ToString("00"); ;

        //                dtResult.Rows.Add(dr);
        //            }
        //        }

        //        ds = LoadMeetings(loginTime, logoutTime);

        //        if (ds.Tables["Meeting"].Rows.Count > 0)
        //        {
        //            dr = dtResult.NewRow();
        //            dr["EmpName"] = ddlEmp.SelectedItem.Text;
        //            dr["Client"] = "";
        //            dr["Task"] = "Meetings";
        //            dr["Date"] = Convert.ToDateTime(datepicker.Value);
        //            dr["SubTask"] = "Meetings";
        //            dr["ToDate"] = Convert.ToDateTime(datepickerTo.Value);
        //            dr["Duration"] = Convert.ToInt32(ds.Tables["Meeting"].Rows[0]["hour"]).ToString("00") + ":" + Convert.ToInt32(ds.Tables["Meeting"].Rows[0]["minute"]).ToString("00") + ":" + Convert.ToInt32(ds.Tables["Meeting"].Rows[0]["seconds"]).ToString("00");
        //            dtResult.Rows.Add(dr);
        //        }
        //    }

        //}

        dt = LoadByTaskReportNew(datepicker.Value, datepickerTo.Value);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr1 in dt.Rows)
            {
                dr = dtResult.NewRow();

                dr["EmpName"] = ddlEmp.SelectedItem.Text;
                dr["Client"] = "";
                dr["Task"] = dr1["TL_Task"];
                dr["SubTask"] = dr1["STL_SubTask"];
                dr["Date"] = Convert.ToDateTime(datepicker.Value);
                dr["ToDate"] = Convert.ToDateTime(datepickerTo.Value);
                dr["Duration"] = Convert.ToInt32(dr1["hour"]).ToString("00") + ":" + Convert.ToInt32(dr1["minute"]).ToString("00") + ":" + Convert.ToInt32(dr1["seconds"]).ToString("00"); ;

                dtResult.Rows.Add(dr);
            }
        }

        ds = LoadMeetings(datepicker.Value, datepickerTo.Value);

        if (ds.Tables["Meeting"].Rows.Count > 0)
        {
            dr = dtResult.NewRow();
            dr["EmpName"] = ddlEmp.SelectedItem.Text;
            dr["Client"] = "";
            dr["Task"] = "Meetings";
            dr["Date"] = Convert.ToDateTime(datepicker.Value);
            dr["SubTask"] = "Meetings";
            dr["ToDate"] = Convert.ToDateTime(datepickerTo.Value);
            dr["Duration"] = Convert.ToInt32(ds.Tables["Meeting"].Rows[0]["hour"]).ToString("00") + ":" + Convert.ToInt32(ds.Tables["Meeting"].Rows[0]["minute"]).ToString("00") + ":" + Convert.ToInt32(ds.Tables["Meeting"].Rows[0]["seconds"]).ToString("00");
            dtResult.Rows.Add(dr);
        }

    }

    //By Task Report end1

    //By Client start1
    private DataSet GetLogin()
    {
        da = new SqlDataAdapter("SELECT LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + ddlEmp.SelectedItem.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) >= '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and LA_Log_Action = 'Actual Login'", con);
        da.Fill(ds, "login");
        return ds;
    }

    private DataSet GetLogout(DateTime login)
    {
        da = new SqlDataAdapter("SELECT TOP 1 LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + ddlEmp.SelectedItem.Text + "' and LA_Start_Date_Time >= '" + login + "' and LA_Log_Action = 'Actual Logout' ORDER BY LA_ID", con);
        da.Fill(ds, "logout");
        return ds;
    }

    private DataSet GetLogoutNew(DateTime login)
    {
        da = new SqlDataAdapter("SELECT TOP 1 LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + ddlEmp.SelectedItem.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) >= '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and LA_Log_Action = 'Actual Logout' ORDER BY LA_ID", con);
        da.Fill(ds, "logout");
        return ds;
    }

    private DataTable LoadByClientReport(string login, string logout)
    {
        if (rbUser.Checked)
        {
            da = new SqlDataAdapter("SELECT C.CL_ClientName, A.TL_Task, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Task_List A, RTM_Records B, RTM_Client_List C where A.TL_ID  = B.R_Task and B.R_Client = C.CL_ID and R_User_Name='" + ddlEmp.SelectedItem.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + login + "' and '" + logout + "' and R_Duration != 'HH:MM:SS' GROUP BY A.TL_Task, C.CL_ClientName", con);
        }
        else
        {
            da = new SqlDataAdapter("SELECT C.CL_ClientName, A.TL_Task, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Task_List A, RTM_Records B, RTM_Client_List C where A.TL_ID  = B.R_Task and B.R_Client = C.CL_ID and R_TeamId='" + ddlEmp.SelectedValue + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + login + "' and '" + logout + "' and R_Duration != 'HH:MM:SS' GROUP BY A.TL_Task, C.CL_ClientName", con);

        }

        da.Fill(dt);
        return dt;
    }


    private void PrintByClientReport()
    {
        DataRow dr;

        buildByTaskTable();
        //ds = GetLogin();
        //if (ds.Tables["login"].Rows.Count > 0)
        //{
        //    DateTime loginTime = Convert.ToDateTime(ds.Tables["login"].Rows[0]["LA_Start_Date_Time"]);

        //    ds = GetLogoutNew(loginTime);
        //    if (ds.Tables["logout"].Rows.Count > 0)
        //    {
        //        DateTime logoutTime = Convert.ToDateTime(ds.Tables["logout"].Rows[0]["LA_Start_Date_Time"]);

        //        dt = LoadByClientReport(loginTime, logoutTime);

        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr1 in dt.Rows)
        //            {
        //                dr = dtResult.NewRow();

        //                dr["EmpName"] = ddlEmp.SelectedItem.Text;
        //                dr["Client"] = dr1["CL_ClientName"];
        //                dr["Task"] = dr1["TL_Task"];
        //                dr["Date"] = Convert.ToDateTime(datepicker.Value);
        //                dr["ToDate"] = Convert.ToDateTime(datepickerTo.Value);
        //                dr["Duration"] = Convert.ToInt32(dr1["hour"]).ToString("00") + ":" + Convert.ToInt32(dr1["minute"]).ToString("00") + ":" + Convert.ToInt32(dr1["seconds"]).ToString("00");
        //                dr["SubTask"] = "";
        //                dtResult.Rows.Add(dr);
        //            }
        //        }
        //    }

        //}

        dt = LoadByClientReport(datepicker.Value, datepickerTo.Value);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr1 in dt.Rows)
            {
                dr = dtResult.NewRow();

                dr["EmpName"] = ddlEmp.SelectedItem.Text;
                dr["Client"] = dr1["CL_ClientName"];
                dr["Task"] = dr1["TL_Task"];
                dr["Date"] = Convert.ToDateTime(datepicker.Value);
                dr["ToDate"] = Convert.ToDateTime(datepickerTo.Value);
                dr["Duration"] = Convert.ToInt32(dr1["hour"]).ToString("00") + ":" + Convert.ToInt32(dr1["minute"]).ToString("00") + ":" + Convert.ToInt32(dr1["seconds"]).ToString("00");
                dr["SubTask"] = "";
                dtResult.Rows.Add(dr);
            }
        }

    }

    // Service Code % Report Start

    private void BuildServiceCodeTable()
    {
        DataColumn dc;
        try
        {

            //dc = new DataColumn("Team Name");
            //dtResult.Columns.Add(dc);

            dc = new DataColumn("Service Code");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Total Hours", typeof(decimal));
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Percent");
            dtResult.Columns.Add(dc);



        }
        catch (Exception)
        {


        }
    }

    private DataSet GetTeamName(int id)
    {
        da = new SqlDataAdapter("select T_TeamName from RTM_Team_List where T_ID= '" + id + "'", con);
        da.Fill(ds, "Team");
        return ds;
    }
    private DataSet GetServiceCodeData(DateTime start, DateTime end, int TeamId)
    {
        da = new SqlDataAdapter("select STL_ServiceCode, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records, RTM_SubTask_List where R_SubTask = STL_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + start.ToShortDateString() + "' and '" + end.ToShortDateString() + "' and R_TeamId = '" + TeamId + "' and R_Duration != 'HH:MM:SS' GROUP BY STL_ServiceCode Order By sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 Desc, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 Desc", con);
        da.Fill(ds, "servicecode");
        return ds;
    }

    private DataSet GetTotal(DateTime start, DateTime end, int TeamId)
    {
        da = new SqlDataAdapter("select sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + start.ToShortDateString() + "' and '" + end.ToShortDateString() + "' and R_TeamId = '" + TeamId + "' and R_Duration != 'HH:MM:SS'", con);
        da.Fill(ds, "Total");
        return ds;
    }



    private void BindGrid()
    {
        DataRow dr;
        BuildServiceCodeTable();
        //ds = GetTeamName(Convert.ToInt32(ddlEmp.SelectedValue));

        //if (ds.Tables["Team"].Rows.Count > 0)
        //{
        //    TeamName = ds.Tables["Team"].Rows[0]["T_TeamName"].ToString();
        //}

        TeamName = ddlEmp.SelectedItem.Text;

        ds = GetTotal(Convert.ToDateTime(datepicker.Value), Convert.ToDateTime(datepickerTo.Value), Convert.ToInt32(ddlEmp.SelectedValue));

        if (ds.Tables["Total"].Rows.Count > 0)
        {
            string total1 = ds.Tables["Total"].Rows[0]["hour"] + "." + ds.Tables["Total"].Rows[0]["minute"];
            total = Convert.ToDecimal(total1);

            ds = GetServiceCodeData(Convert.ToDateTime(datepicker.Value), Convert.ToDateTime(datepickerTo.Value), Convert.ToInt32(ddlEmp.SelectedValue));

            if (ds.Tables["servicecode"].Rows.Count > 0)
            {
                foreach (DataRow dr1 in ds.Tables["servicecode"].Rows)
                {
                    dr = dtResult.NewRow();

                    //dr["Team Name"] = TeamName ;
                    if (dr1["STL_ServiceCode"].ToString().Contains("|") == false)
                    {
                        dr["Service Code"] = dr1["STL_ServiceCode"].ToString();
                    }
                    else
                    {
                        //string[] service = Convert.ToString(dr1["STL_ServiceCode"]).Split('|');
                        val1 = 0;
                        // dr["Service Code"] = service[1].ToString().Trim();
                        val1 = decimal.Round((Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600)), 2, MidpointRounding.AwayFromZero);

                        val2 = val2 + val1;
                        continue;
                    }

                    decimal percent = ((Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600)) / total) * 100;
                    percent = decimal.Round(percent, 2, MidpointRounding.AwayFromZero);
                    dr["Percent"] = percent;
                    dr["Total Hours"] = decimal.Round((Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600)), 2, MidpointRounding.AwayFromZero);
                    dtResult.Rows.Add(dr);
                }

                if (val2 != 0)
                {
                    dr = dtResult.NewRow();
                    //dr["Team Name"] = TeamName;
                    dr["Service Code"] = "General/Internal";

                    decimal percent2 = (val2 / total) * 100;
                    percent2 = decimal.Round(percent2, 2, MidpointRounding.AwayFromZero);
                    dr["Percent"] = percent2;
                    dr["Total Hours"] = val2;
                    dtResult.Rows.Add(dr);
                }
            }
        }
        DataView dv = dtResult.DefaultView;
        dv.Sort = "[Total Hours] desc";
        dtResult = dv.ToTable();

        BindChartforServiceCode();
    }

    private void BindChartforServiceCode()
    {
        Chart1.Visible = true;
        if (dtResult.Rows.Count > 20)
        {
            x = new string[20];
            y = new double[20];
        }
        else
        {
            x = new string[dtResult.Rows.Count];
            y = new double[dtResult.Rows.Count];
        }


        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            if (i >= 20)
            {
                break;
            }
            //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
            x[i] = dtResult.Rows[i]["Service Code"].ToString();
            //val1 = Convert.ToDecimal(dtResult.Rows[i]["Percent"]);
            //y[i] = (int)val1;
            y[i] = Convert.ToDouble(dtResult.Rows[i]["Percent"]);
        }

        Chart1.Series[0].Points.DataBindXY(x, y);
        //this.Chart1.Series[0].BorderWidth = 1;
        Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 5;
        Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
        this.Chart1.Series[0].Label = "#VALY %";
        if (rbJobCode.Checked == true)
        {
            Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Job Codes";
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Percentage";
            Chart1.Titles.Add(CreateTitle("Job Code % - " + TeamName + " (" + datepicker.Value + ") - (" + datepickerTo.Value + ")"));
        }
        else if (rbServiceCode.Checked == true)
        {
            Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Service Codes";
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Percentage";
            Chart1.Titles.Add(CreateTitle("Service Code % - " + TeamName + " (" + datepicker.Value + ") - (" + datepickerTo.Value + ")"));
        }

        //gvReport.DataSource = dtResult;
        //gvReport.DataBind();
        btnExport.Visible = true;
        btnPDF.Visible = true;
    }

    public Title CreateTitle(string name)
    {
        Title title = new Title();
        title.Text = name;
        title.ShadowColor = System.Drawing.Color.FromArgb(32, 0, 0, 0);
        title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
        title.ShadowOffset = 3;
        title.ForeColor = System.Drawing.Color.FromArgb(26, 59, 105);
        return title;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (rbServiceCode.Checked == true)
        {
            BindGrid();
        }
        else if (rbJobCode.Checked == true)
        {
            BindJobCodeChart();
        }
        else if (rbProductCode.Checked == true)
        {
            BindProductCodeChart();
        }
        else if (rbRU.Checked == true)
        {
            BindURChart();
        }
        else if (rbTaskPer.Checked == true)
        {
            BindTaskChart();
        }
        else if (RadioButton1.Checked == true)
        {
            BindPeerSupport();
        }

        Response.Clear();
        Response.Buffer = true;
        if (rbJobCode.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;filename=Job Code %-" + TeamName + ".csv");
        }
        else if (rbServiceCode.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;filename=Service Code %-" + TeamName + ".csv");
        }
        else if (rbProductCode.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;filename=Product Code %-" + TeamName + ".csv");
        }
        else if (rbRU.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;filename=Resource Utilization%-" + TeamName + ".csv");
        }
        else if (rbTaskPer.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;filename=Task Percentage-" + TeamName + ".csv");
        }
        else if (RadioButton1.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;filename=Peer Support-" + TeamName + ".csv");
        }

        Response.Charset = "";
        Response.ContentType = "application/text";
        StringBuilder sb = new StringBuilder();
        for (int k = 0; k < dtResult.Columns.Count; k++)
        {
            //add separator
            sb.Append(dtResult.Columns[k].ColumnName + ',');
        }
        sb.Append("\r\n");

        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            for (int k = 0; k < dtResult.Columns.Count; k++)
            {
                //add separator
                sb.Append(dtResult.Rows[i][k].ToString().Replace(",", ";") + ',');
            }
            //append new line
            sb.Append("\r\n");
        }

        Response.Output.Write(sb.ToString());
        Response.Flush();
        Response.End();
    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        if (rbServiceCode.Checked == true)
        {
            BindGrid();
        }
        else if (rbJobCode.Checked == true)
        {
            BindJobCodeChart();
        }
        else if (rbProductCode.Checked == true)
        {
            BindProductCodeChart();
        }
        else if (rbRU.Checked == true)
        {
            BindURChart();
        }
        else if (rbTaskPer.Checked == true)
        {
            BindTaskChart();
        }
        Document pdfDoc = new Document(PageSize.A3, 10f, 10f, 10f, 0f);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();

        using (MemoryStream stream = new MemoryStream())
        {
            Chart1.SaveImage(stream, ChartImageFormat.Png);
            iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
            chartImage.ScalePercent(75f);
            pdfDoc.Add(chartImage);
        }

        pdfDoc.Close();
        Response.ContentType = "application/pdf";
        if (rbServiceCode.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;" + "filename=Service Code Graph-" + TeamName + ".pdf");
        }
        else if (rbJobCode.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;" + "filename=Job Code Graph-" + TeamName + ".pdf");
        }
        else if (rbProductCode.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;" + "filename=Product Code Graph-" + TeamName + ".pdf");
        }
        else if (rbRU.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;" + "filename=Resource Utilization Graph-" + TeamName + ".pdf");
        }
        else if (rbTaskPer.Checked == true)
        {
            Response.AddHeader("content-disposition", "attachment;" + "filename=Task Percentage Graph-" + TeamName + ".pdf");
        }

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(pdfDoc);
        Response.End();
    }


    //Job Code% Report

    private DataSet GetTotalJobCode(int TeamId)
    {
        da = new SqlDataAdapter("select sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and R_TeamId = '" + TeamId + "' and R_Duration != 'HH:MM:SS'", con);
        da.Fill(ds, "Total");
        return ds;
    }

    private DataSet GetJobCodeData(int TeamId)
    {
        da = new SqlDataAdapter("select CL_ClientName, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records, RTM_Client_List where R_Client = CL_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and R_TeamId = '" + TeamId + "' and R_Duration != 'HH:MM:SS' GROUP BY CL_ClientName Order By sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 Desc, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 Desc", con);
        da.Fill(ds, "JobCode");
        return ds;
    }

    private void BindJobCodeChart()
    {
        DataRow dr;
        BuildJobCodeTable();

        //ds = GetTeamName(Convert.ToInt32(Session["team"]));

        //if (ds.Tables["Team"].Rows.Count > 0)
        //{
        //    TeamName = ds.Tables["Team"].Rows[0]["T_TeamName"].ToString();
        //}

        TeamName = ddlEmp.SelectedItem.Text;

        ds = GetTotalJobCode(Convert.ToInt32(ddlEmp.SelectedValue));

        if (ds.Tables["Total"].Rows.Count > 0)
        {
            //string total1 = Convert.ToDecimal(ds.Tables["Total"].Rows[0]["hour"]) + Convert.ToDecimal(ds.Tables["Total"].Rows[0]["minute"])/60;
            total = Convert.ToDecimal(ds.Tables["Total"].Rows[0]["hour"]) + (Convert.ToDecimal(ds.Tables["Total"].Rows[0]["minute"]) / 60) + (Convert.ToDecimal(ds.Tables["Total"].Rows[0]["seconds"]) / 3600); // Convert.ToDecimal(total1);

            ds = GetJobCodeData(Convert.ToInt32(ddlEmp.SelectedValue));

            if (ds.Tables["JobCode"].Rows.Count > 0)
            {
                foreach (DataRow dr1 in ds.Tables["JobCode"].Rows)
                {
                    dr = dtResult.NewRow();

                    // dr["Team Name"] = TeamName ;
                    dr["Job Code"] = dr1["CL_ClientName"].ToString();
                    dr["Total Hours"] = decimal.Round((Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600)), 2, MidpointRounding.AwayFromZero);
                    decimal percent = ((Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600)) / total) * 100;
                    percent = decimal.Round(percent, 2, MidpointRounding.AwayFromZero);
                    dr["Percent"] = percent;

                    dtResult.Rows.Add(dr);
                }
            }
        }
        BindChartForJobCodes();
    }

    private void BuildJobCodeTable()
    {
        DataColumn dc;
        try
        {

            //dc = new DataColumn("Team Name");
            //dtResult.Columns.Add(dc);

            dc = new DataColumn("Job Code");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Total Hours");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Percent");
            dtResult.Columns.Add(dc);



        }
        catch (Exception)
        {


        }
    }

    private void BindChartForJobCodes()
    {
        Chart1.Visible = true;
        if (dtResult.Rows.Count > 20)
        {
            x = new string[20];
            y = new double[20];
        }
        else
        {
            x = new string[dtResult.Rows.Count];
            y = new double[dtResult.Rows.Count];
        }


        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            if (i >= 20)
            {
                break;
            }
            //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
            x[i] = dtResult.Rows[i]["Job Code"].ToString();
            //val1 = Convert.ToDecimal(dtResult.Rows[i]["Percent"]);
            //y[i] = (int)val1;
            y[i] = Convert.ToDouble(dtResult.Rows[i]["Percent"]);

        }

        Chart1.Series[0].Points.DataBindXY(x, y);
        //this.Chart1.Series[0].BorderWidth = 1;
        Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 5;
        Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Job Codes (Showing Top 20)";
        Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Percentage";
        Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
        this.Chart1.Series[0].Label = "#VALY %";
        Chart1.Titles.Add(CreateTitle("Job Codes % - " + TeamName + " (" + datepicker.Value + ") - (" + datepickerTo.Value + ")"));
        //gvReport.DataSource = dtResult;
        //gvReport.DataBind();
        btnExport.Visible = true;
        btnPDF.Visible = true;
        btnExport.ToolTip = "Export all Clients";

    }

    //Product Code wise report Start

    private DataSet GetTotalEMS(int teamId)
    {
        da = new SqlDataAdapter("select sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records, RTM_Client_List where R_Client = CL_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and R_TeamId = '" + teamId + "' and R_Duration != 'HH:MM:SS' and (CL_TSheetClient LIKE '%Alliance Partner Projects IPV/EMS/MTM/CMP/Bill Tamer%' or CL_TSheetClient LIKE '%Fixed Symphony Projects%')", con);
        da.Fill(ds, "EMS");
        return ds;
    }

    private DataSet GetTotalCMP(int teamId)
    {
        da = new SqlDataAdapter("select sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records, RTM_Client_List where R_Client = CL_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and R_TeamId = '" + teamId + "' and R_Duration != 'HH:MM:SS' and (CL_TSheetClient LIKE '%Fixed CMP Projects%' or CL_TSheetClient LIKE '%Fixed Insight CMP Projects%')", con);
        da.Fill(ds, "CMP");
        return ds;
    }

    private DataSet GetTotalIPV(int teamId)
    {
        da = new SqlDataAdapter("select sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records, RTM_Client_List where R_Client = CL_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and R_TeamId = '" + teamId + "' and R_Duration != 'HH:MM:SS' and CL_TSheetClient LIKE '%Fixed IPV Projects%'", con);
        da.Fill(ds, "IPV");
        return ds;
    }

    private DataSet GetTotalOthers(int teamId)
    {
        da = new SqlDataAdapter("select sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records, RTM_Client_List where R_Client = CL_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and R_TeamId = '" + teamId + "' and R_Duration != 'HH:MM:SS' and (CL_TSheetClient LIKE '%Wireless Projects%' or CL_TSheetClient LIKE '%Internal%')", con);
        da.Fill(ds, "Others");
        return ds;
    }

    private DataSet GetTotalHCL(int teamId)
    {
        da = new SqlDataAdapter("select sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from  RTM_Records, RTM_Client_List where R_Client = CL_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and R_TeamId = '" + teamId + "' and R_Duration != 'HH:MM:SS' and CL_TSheetClient LIKE '%Fixed HCL Projects%'", con);
        da.Fill(ds, "HCL");
        return ds;
    }

    private void BuildProductCodeTable()
    {
        DataColumn dc;
        try
        {

            dc = new DataColumn("Team Name");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Product Code");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Percent");
            dtResult.Columns.Add(dc);

        }
        catch (Exception)
        {


        }
    }

    private void BindProductCodeChart()
    {
        DataRow dr;

        BuildProductCodeTable();

        //ds = GetTeamName(Convert.ToInt32(Session["team"]));

        //if (ds.Tables["Team"].Rows.Count > 0)
        //{
        //    TeamName = ds.Tables["Team"].Rows[0]["T_TeamName"].ToString();
        //}

        TeamName = ddlEmp.SelectedItem.Text;

        ds = GetTotalEMS(Convert.ToInt32(ddlEmp.SelectedValue));
        if (ds.Tables["EMS"].Rows.Count > 0)
        {
            if (ds.Tables["EMS"].Rows[0]["hour"].ToString().Length > 0)
            {
                totalEMS = Convert.ToDouble(ds.Tables["EMS"].Rows[0]["hour"] + "." + ds.Tables["EMS"].Rows[0]["minute"]);
            }

        }

        ds = GetTotalCMP(Convert.ToInt32(ddlEmp.SelectedValue));
        if (ds.Tables["CMP"].Rows.Count > 0)
        {
            if (ds.Tables["CMP"].Rows[0]["hour"].ToString().Length > 0)
            {
                totalCMP = Convert.ToDouble(ds.Tables["CMP"].Rows[0]["hour"] + "." + ds.Tables["CMP"].Rows[0]["minute"]);
            }

        }

        ds = GetTotalIPV(Convert.ToInt32(ddlEmp.SelectedValue));
        if (ds.Tables["IPV"].Rows.Count > 0)
        {
            if (ds.Tables["IPV"].Rows[0]["hour"].ToString().Length > 0)
            {
                totalIPV = Convert.ToDouble(ds.Tables["IPV"].Rows[0]["hour"] + "." + ds.Tables["IPV"].Rows[0]["minute"]);
            }

        }

        ds = GetTotalOthers(Convert.ToInt32(ddlEmp.SelectedValue));
        if (ds.Tables["Others"].Rows.Count > 0)
        {
            if (ds.Tables["Others"].Rows[0]["hour"].ToString().Length > 0)
            {
                totalOthers = Convert.ToDouble(ds.Tables["Others"].Rows[0]["hour"] + "." + ds.Tables["Others"].Rows[0]["minute"]);
            }

        }

        ds = GetTotalHCL(Convert.ToInt32(ddlEmp.SelectedValue));

        if (ds.Tables["HCL"].Rows.Count > 0 && ds.Tables["HCL"].Rows[0]["hour"].ToString().Length > 0)
        {
            totalHCL = Convert.ToDouble(ds.Tables["HCL"].Rows[0]["hour"] + "." + ds.Tables["HCL"].Rows[0]["minute"]);
        }

        if (totalEMS != 0)
        {
            dr = dtResult.NewRow();

            dr["Team Name"] = TeamName;
            dr["Product Code"] = "EMS";
            double percent1 = (totalEMS / (totalEMS + totalCMP + totalIPV + totalOthers + totalHCL)) * 100;
            percent1 = Math.Round(percent1, 2, MidpointRounding.AwayFromZero);
            dr["Percent"] = percent1;

            dtResult.Rows.Add(dr);
        }

        if (totalCMP != 0)
        {
            dr = dtResult.NewRow();

            dr["Team Name"] = TeamName;
            dr["Product Code"] = "CMP";
            double percent1 = (totalCMP / (totalEMS + totalCMP + totalIPV + totalOthers + totalHCL)) * 100;
            percent1 = Math.Round(percent1, 2, MidpointRounding.AwayFromZero);
            dr["Percent"] = percent1;

            dtResult.Rows.Add(dr);
        }

        if (totalIPV != 0)
        {
            dr = dtResult.NewRow();

            dr["Team Name"] = TeamName;
            dr["Product Code"] = "IPV";
            double percent1 = (totalIPV / (totalEMS + totalCMP + totalIPV + totalOthers + totalHCL)) * 100;
            percent1 = Math.Round(percent1, 2, MidpointRounding.AwayFromZero);
            dr["Percent"] = percent1;

            dtResult.Rows.Add(dr);
        }

        if (totalHCL != 0)
        {
            dr = dtResult.NewRow();

            dr["Team Name"] = TeamName;
            dr["Product Code"] = "HCL";
            double percent1 = (totalHCL / (totalEMS + totalCMP + totalIPV + totalOthers + totalHCL)) * 100;
            percent1 = Math.Round(percent1, 2, MidpointRounding.AwayFromZero);
            dr["Percent"] = percent1;

            dtResult.Rows.Add(dr);
        }

        if (totalOthers != 0)
        {
            dr = dtResult.NewRow();

            dr["Team Name"] = TeamName;
            dr["Product Code"] = "Others";
            double percent1 = (totalOthers / (totalEMS + totalCMP + totalIPV + totalOthers + totalHCL)) * 100;
            percent1 = Math.Round(percent1, 2, MidpointRounding.AwayFromZero);
            dr["Percent"] = percent1;

            dtResult.Rows.Add(dr);
        }

        BindChartForProductCode();

    }

    private void BindChartForProductCode()
    {
        Chart1.Visible = true;
        x = new string[dtResult.Rows.Count];
        y = new double[dtResult.Rows.Count];

        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
            x[i] = dtResult.Rows[i]["Product Code"].ToString();
            //val1 = Convert.ToDecimal(dtResult.Rows[i]["Percent"]);
            //y[i] = (int)val1;
            y[i] = Convert.ToDouble(dtResult.Rows[i]["Percent"]);

        }

        Chart1.Series[0].Points.DataBindXY(x, y);
        //this.Chart1.Series[0].BorderWidth = 1;
        Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 5;
        Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Product Codes";
        Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Percentage";
        Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
        this.Chart1.Series[0].Label = "#VALY %";
        Chart1.Titles.Add(CreateTitle("Product Codes % - " + TeamName + " (" + datepicker.Value + ") - (" + datepickerTo.Value + ")"));
        //gvReport.DataSource = dtResult;
        //gvReport.DataBind();
        btnExport.Visible = true;
        btnPDF.Visible = true;
    }

    //Resource utilization report start...............................

    private DataSet GetEmployees(int teamId)
    {
        if (ds.Tables.Contains("users"))
        {
            ds.Tables.Remove(ds.Tables["users"]);
        }
        da = new SqlDataAdapter("SELECT UL_User_Name FROM RTM_User_List WHERE UL_Team_Id = '" + teamId + "'", con);
        da.Fill(ds, "users");
        return ds;
    }

    private DataSet GetLoginTime(string user)
    {
        if (ds.Tables.Contains("Login"))
        {
            ds.Tables.Remove(ds.Tables["Login"]);
        }
        da = new SqlDataAdapter("SELECT LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and LA_Log_Action = 'Actual Login' order by LA_Start_Date_Time", con);
        da.Fill(ds, "Login");
        return ds;
    }

    private DataSet GetLogoutTime(string user, DateTime date1)
    {
        if (ds.Tables.Contains("Logout"))
        {
            ds.Tables.Remove(ds.Tables["Logout"]);
        }
        da = new SqlDataAdapter("SELECT TOP 1 LA_Start_Date_Time From RTM_Log_Actions where LA_User_Name = '" + user + "' and LA_Start_Date_Time >= '" + date1 + "' and LA_Log_Action = 'Actual Logout' order by LA_Start_Date_Time", con);
        da.Fill(ds, "Logout");
        return ds;
    }

    private DataSet GetTaskHours(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("Task"))
        {
            ds.Tables.Remove(ds.Tables["Task"]);
        }
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and R_Duration != 'HH:MM:SS' and STL_SubTask != 'NON-TASK'", con);
        da.Fill(ds, "Task");
        return ds;
    }

    private DataSet GetLogHours(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("Log"))
        {
            ds.Tables.Remove(ds.Tables["Log"]);
        }
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason != 'Break' and LA_Reason != 'Non-Task' and  LA_Reason !='Idle Time'", con);
        da.Fill(ds, "Log");
        return ds;
    }

    private DataSet GetLeaves(string user)
    {
        if (ds.Tables.Contains("Leaves"))
        {
            ds.Tables.Remove(ds.Tables["Leaves"]);
        }
        da = new SqlDataAdapter("SELECT CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) From RTM_Log_Actions where LA_User_Name = '" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) BETWEEN '" + Convert.ToDateTime(datepicker.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(datepickerTo.Value).ToShortDateString() + "' and LA_Log_Action = 'Actual Login' GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time)))", con);
        da.Fill(ds, "Leaves");
        return ds;
    }

    private DataSet GetNonTaskRecords(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("NonTaskRecords"))
        {
            ds.Tables.Remove(ds.Tables["NonTaskRecords"]);
        }
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records, RTM_SubTask_List where R_SubTask = STL_ID and R_User_Name='" + user + "' and R_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and R_Duration != 'HH:MM:SS' and STL_SubTask = 'NON-TASK'", con);
        da.Fill(ds, "NonTaskRecords");
        return ds;
    }

    private DataSet GetNonTaskLogs(string user, DateTime start, DateTime end)
    {
        if (ds.Tables.Contains("NonTaskLog"))
        {
            ds.Tables.Remove(ds.Tables["NonTaskLog"]);
        }
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(LA_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Log_Actions where LA_User_Name='" + user + "' and LA_Start_Date_Time BETWEEN '" + start + "' and '" + end + "' and LA_Duration != 'HH:MM:SS' and LA_Reason = 'Non-Task'", con);
        da.Fill(ds, "NonTaskLog");
        return ds;
    }

    private void BuildRUTable()
    {
        dtResult = new DataTable();
        DataColumn dc;
        try
        {
            dc = new DataColumn("Employee Name");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Team Name");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Total Hours");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Absent Hours");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("RU%", typeof(double));
            dtResult.Columns.Add(dc);

            dc = new DataColumn("NonTask");
            dtResult.Columns.Add(dc);

        }
        catch (Exception)
        {


        }
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

        TeamName = ddlEmp.SelectedItem.Text;

        ds = GetEmployees(Convert.ToInt32(ddlEmp.SelectedValue));

        if (ds.Tables["users"].Rows.Count > 0)
        {
            foreach (DataRow dr1 in ds.Tables["users"].Rows)
            {
                //if (ds.Tables.Contains("Login"))
                //{
                //    ds.Tables.Remove(ds.Tables["Login"]);
                //}
                //if (ds.Tables.Contains("Logout"))
                //{
                //    ds.Tables.Remove(ds.Tables["Logout"]);
                //}
                //if (ds.Tables.Contains("Task"))
                //{
                //    ds.Tables.Remove(ds.Tables["Task"]);
                //}
                //if (ds.Tables.Contains("Log"))
                //{
                //    ds.Tables.Remove(ds.Tables["Log"]);
                //}
                //if (ds.Tables.Contains("Leaves"))
                //{
                //    ds.Tables.Remove(ds.Tables["Leaves"]);
                //}
                //if (ds.Tables.Contains("NonTaskRecords"))
                //{
                //    ds.Tables.Remove(ds.Tables["NonTaskRecords"]);
                //}
                //if (ds.Tables.Contains("NonTaskLog"))
                //{
                //    ds.Tables.Remove(ds.Tables["NonTaskLog"]);
                //}

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
                            totalTaskHours = Convert.ToDouble(ds.Tables["Task"].Rows[0]["hour"]) + (Convert.ToDouble(ds.Tables["Task"].Rows[0]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["Task"].Rows[0]["seconds"]) / 3600);
                        }


                        ds = GetLogHours(username, loginTime, logoutTime);

                        if (ds.Tables["Log"].Rows.Count > 0 && ds.Tables["Log"].Rows[0]["hour"].ToString().Length > 0)
                        {
                            totalLogHours = Convert.ToDouble(ds.Tables["Log"].Rows[0]["hour"]) + (Convert.ToDouble(ds.Tables["Log"].Rows[0]["minute"]) / 60) + (Convert.ToDouble(ds.Tables["Log"].Rows[0]["seconds"]) / 3600);
                        }


                        totalWorkHours = totalTaskHours + totalLogHours;
                        totalWorkHours = Math.Round(totalWorkHours, 2, MidpointRounding.AwayFromZero);

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
                            nonTaskRecord = Convert.ToDouble(ds.Tables["NonTaskRecords"].Rows[0]["hour"] + "." + ds.Tables["NonTaskRecords"].Rows[0]["minute"]);
                        }

                        ds = GetNonTaskLogs(username, loginTime, logoutTime);

                        if (ds.Tables["NonTaskLog"].Rows.Count > 0 && ds.Tables["NonTaskLog"].Rows[0]["hour"].ToString().Length > 0)
                        {
                            nonTaskLog = Convert.ToDouble(ds.Tables["NonTaskLog"].Rows[0]["hour"] + "." + ds.Tables["NonTaskLog"].Rows[0]["minute"]);
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

                        totalTaskHours = 0;
                        totalLogHours = 0;
                        totalWorkHours = 0;
                        RUPercent = 0;
                        leaveHours = 0;
                        nonTaskRecord = 0;
                        nonTaskLog = 0;
                        totalNonTaskHours = 0;
                    }
                }
            }
            DataView dv = dtResult.DefaultView;
            dv.Sort = "[RU%] Desc";
            dtResult = dv.ToTable();

            BuildRUChart();
        }
    }

    private void BuildRUChart()
    {
        Chart1.Visible = true;
        x = new string[dtResult.Rows.Count];
        y = new double[dtResult.Rows.Count];

        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
            x[i] = dtResult.Rows[i]["Employee Name"].ToString();
            //val1 = Convert.ToDecimal(dtResult.Rows[i]["Percent"]);
            //y[i] = (int)val1;
            y[i] = Convert.ToDouble(dtResult.Rows[i]["RU%"]);

        }

        Chart1.Series[0].Points.DataBindXY(x, y);
        //this.Chart1.Series[0].BorderWidth = 1;
        Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 5;
        Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Employee Names";
        Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Percentage";
        Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
        this.Chart1.Series[0].Label = "#VALY %";
        Chart1.Titles.Add(CreateTitle("Resource Utilization % - " + TeamName + " (" + datepicker.Value + ") - (" + datepickerTo.Value + ")"));
        //gvReport.DataSource = dtResult;
        //gvReport.DataBind();
        btnExport.Visible = true;
        btnPDF.Visible = true;
    }

    //Task % report starts

    private DataSet GetTaskData(DateTime start, DateTime end, int TeamId)
    {
        da = new SqlDataAdapter("SELECT A.TL_Task, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Task_List A, RTM_Records B where A.TL_ID  = B.R_Task and R_TeamId = '" + TeamId + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + start.ToShortDateString() + "' and '" + end.ToShortDateString() + "' and R_Duration != 'HH:MM:SS' GROUP BY A.TL_Task ORDER BY sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 DESC, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 DESC", con);
        //da = new SqlDataAdapter("SELECT A.TL_Task, sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Task_List A, RTM_Records B where A.TL_ID  = B.R_Task  and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + start.ToShortDateString() + "' and '" + end.ToShortDateString() + "' and R_Duration != 'HH:MM:SS' GROUP BY A.TL_Task", con);
        da.Fill(ds, "Task");
        return ds;
    }

    private DataSet GetTotalTaskHours(DateTime start, DateTime end, int TeamId)
    {
        da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records where R_TeamId = '" + TeamId + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + start.ToShortDateString() + "' and '" + end.ToShortDateString() + "' and R_Duration != 'HH:MM:SS'", con);
        // da = new SqlDataAdapter("SELECT sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records where  CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + start.ToShortDateString() + "' and '" + end.ToShortDateString() + "' and R_Duration != 'HH:MM:SS'", con);
        da.Fill(ds, "Total");
        return ds;
    }

    private void BuildTaskPerTable()
    {
        DataColumn dc;
        try
        {

            //dc = new DataColumn("Team Name");
            //dtResult.Columns.Add(dc);

            dc = new DataColumn("Task");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Total Hours");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("Percent");
            dtResult.Columns.Add(dc);

        }
        catch (Exception)
        {


        }
    }

    private void BindTaskChart()
    {
        DataRow dr;
        BuildTaskPerTable();
        //ds = GetTeamName(Convert.ToInt32(ddlEmp.SelectedValue));

        //if (ds.Tables["Team"].Rows.Count > 0)
        //{
        //    TeamName = ds.Tables["Team"].Rows[0]["T_TeamName"].ToString();
        //}

        TeamName = ddlEmp.SelectedItem.Text;

        ds = GetTotalTaskHours(Convert.ToDateTime(datepicker.Value), Convert.ToDateTime(datepickerTo.Value), Convert.ToInt32(ddlEmp.SelectedValue));

        if (ds.Tables["Total"].Rows.Count > 0)
        {
            //string total1 = ds.Tables["Total"].Rows[0]["hour"] + "." + ds.Tables["Total"].Rows[0]["minute"];
            total = Convert.ToDecimal(ds.Tables["Total"].Rows[0]["hour"]) + (Convert.ToDecimal(ds.Tables["Total"].Rows[0]["minute"]) / 60) + (Convert.ToDecimal(ds.Tables["Total"].Rows[0]["seconds"]) / 3600); //Convert.ToDecimal(total1);

            ds = GetTaskData(Convert.ToDateTime(datepicker.Value), Convert.ToDateTime(datepickerTo.Value), Convert.ToInt32(ddlEmp.SelectedValue));

            if (ds.Tables["Task"].Rows.Count > 0)
            {
                foreach (DataRow dr1 in ds.Tables["Task"].Rows)
                {
                    dr = dtResult.NewRow();

                    //dr["Team Name"] = TeamName;

                    dr["Task"] = dr1["TL_Task"].ToString();
                    dr["Total Hours"] = decimal.Round(Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);

                    decimal percent = ((Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600)) / total) * 100;
                    percent = decimal.Round(percent, 2, MidpointRounding.AwayFromZero);
                    dr["Percent"] = percent;

                    dtResult.Rows.Add(dr);
                }
            }
        }

        BindChartForTask();
    }

    private void BindChartForTask()
    {
        Chart1.Visible = true;
        x = new string[dtResult.Rows.Count];
        y = new double[dtResult.Rows.Count];

        for (int i = 0; i < dtResult.Rows.Count; i++)
        {
            //TimeSpan span1 = TimeSpan.Parse(dt.Rows[i][1] + ":" + dt.Rows[i][2] + ":" + dt.Rows[i][3]); 
            x[i] = dtResult.Rows[i]["Task"].ToString();
            //val1 = Convert.ToDecimal(dtResult.Rows[i]["Percent"]);
            //y[i] = (int)val1;
            y[i] = Convert.ToDouble(dtResult.Rows[i]["Percent"]);
        }

        Chart1.Series[0].Points.DataBindXY(x, y);
        //this.Chart1.Series[0].BorderWidth = 1;
        Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
        Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 5;
        Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
        Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
        Chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
        this.Chart1.Series[0].Label = "#VALY %";

        Chart1.ChartAreas["ChartArea1"].AxisX.Title = "Tasks";
        Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Percentage";
        Chart1.Titles.Add(CreateTitle("Task % - " + TeamName + " (" + datepicker.Value + ") - (" + datepickerTo.Value + ")"));


        //gvReport.DataSource = dtResult;
        //gvReport.DataBind();
        btnExport.Visible = true;
        btnPDF.Visible = true;
    }

    private void BindPeerSupport()
    {
        dtResult = new DataTable();
        da = new SqlDataAdapter("SELECT R_User_Name as [User Name], R_Start_Date_Time as [Date], R_Duration as [Duration], CL_ClientName as [Clients],  LEFT(R_Comments, case when  CHARINDEX('-', R_Comments ) = 0 then LEN(R_Comments) else CHARINDEX('-', R_Comments) - 1 end) as Comments, SUBSTRING( R_Comments , LEN(R_Comments) -  CHARINDEX('-',REVERSE(R_Comments)) + 2  , LEN(R_Comments)) as [Supported Members] FROM RTM_Records, RTM_Client_List , RTM_SubTask_List where R_Client = CL_ID and R_SubTask = STL_ID and STL_SubTask Like 'Peer Support%' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) Between '" + datepicker.Value + "' and '" + datepickerTo.Value + "' and R_TeamId='" + ddlEmp.SelectedValue + "' order by R_User_Name, R_Start_Date_Time", con);
        da.Fill(dtResult);
        if (dtResult.Rows.Count > 0)
        {
            gvPeerSupport.DataSource = dtResult;
            gvPeerSupport.DataBind();
            gvPeerSupport.Visible = true;
            btnExport.Visible = true;
            TeamName = ddlEmp.SelectedItem.Text;
        }
    }

    

    private void BuildSubTaskTable()
    {
        dtResult = new DataTable();
        DataColumn dc;

        dc = new DataColumn("Day");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("SubTask");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("Hours");
        dtResult.Columns.Add(dc);
    }

    private DataSet FetchSubTasks()
    {
        if (ds.Tables.Contains("subtask"))
        {
            ds.Tables.Remove(ds.Tables["subtask"]);
        }

        da = new SqlDataAdapter("SELECT datename(dw,CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time)))) as [day], DATEPART(dw,R_Start_Date_Time) as daynum, STL_SubTask as [Sub Task]," +
                "sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds " +
                "from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_TeamId='" + ddlEmp.SelectedValue + "' and R_Task = '" + ddlTask.SelectedValue + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + datepicker.Value + "' and '" + datepickerTo.Value + "' and R_Duration != 'HH:MM:SS'  GROUP BY CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))), DATEPART(dw,R_Start_Date_Time), STL_SubTask Order By DATEPART(dw,R_Start_Date_Time), STL_SubTask", con);
        da.Fill(ds, "subtask");
        return ds;
    }

    private void PrintBySubTaskReport()
    {
        try
        {
            DataRow dr;

            BuildSubTaskTable();

            ds = FetchSubTasks();
            if (ds.Tables["subtask"].Rows.Count > 0)
            {
                foreach (DataRow dr1 in ds.Tables["subtask"].Rows)
                {
                    dr = dtResult.NewRow();

                    dr["Day"] = dr1["day"];
                    dr["SubTask"] = dr1["Sub Task"];
                    dr["Hours"] = Math.Round(Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);
                    dtResult.Rows.Add(dr);
                }
            }

            if (dtResult.Rows.Count > 0)
            {

                Pivot advPivot = new Pivot(dtResult);
                advPivot.CssTopHeading = "Heading";
                advPivot.CssLeftColumn = "LeftColumn";
                HtmlTable advancedPivot = advPivot.PivotTable("Day", "SubTask", new string[] { "Hours" });
                advancedPivot.Align = "center";
                //advancedPivot.Width = "80%";
                div2.Controls.Add(advancedPivot);

                btnDetail.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void BuildDetailViewTable()
    {
        DataColumn dc;
        dtResult = new DataTable();

        dc = new DataColumn("Task");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("SubTask");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("User");
        dtResult.Columns.Add(dc);

        dc = new DataColumn("FromDt", typeof(DateTime));
        dtResult.Columns.Add(dc);

        dc = new DataColumn("ToDt", typeof(DateTime));
        dtResult.Columns.Add(dc);

        dc = new DataColumn("Duration", typeof(decimal));
        dtResult.Columns.Add(dc);
    }

    private DataSet GetSubTaskDetails()
    {
        if (ds.Tables.Contains("details"))
        {
            ds.Tables.Remove(ds.Tables["details"]);
        }
        da = new SqlDataAdapter("SELECT R_User_Name, STL_SubTask as [Sub Task],sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600 as hour, (sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60 as minute,(sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(R_Duration,'-', '')))/60)%60)))%60 as seconds from RTM_Records , RTM_SubTask_List where R_SubTask = STL_ID and R_TeamId='" + ddlEmp.SelectedValue + "' and R_Task = '" + ddlTask.SelectedValue + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) BETWEEN '" + datepicker.Value + "' and '" + datepickerTo.Value + "' and R_Duration != 'HH:MM:SS'  GROUP BY R_User_Name, STL_SubTask Order By STL_SubTask, R_User_Name", con);
        da.Fill(ds, "details");
        return ds;
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {

        PrintBySubTaskReport();
        DataRow dr;

        BuildDetailViewTable();

        ds = GetSubTaskDetails();
        if (ds.Tables["details"].Rows.Count > 0)
        {
            foreach (DataRow dr1 in ds.Tables["details"].Rows)
            {
                dr = dtResult.NewRow();

                dr["Task"] = ddlTask.SelectedItem.Text;
                dr["SubTask"] = dr1["Sub Task"];
                dr["User"] = dr1["R_User_Name"];
                dr["FromDt"] = Convert.ToDateTime(datepicker.Value);
                dr["ToDt"] = Convert.ToDateTime(datepickerTo.Value);
                dr["Duration"] = Math.Round(Convert.ToDecimal(dr1["hour"]) + (Convert.ToDecimal(dr1["minute"]) / 60) + (Convert.ToDecimal(dr1["seconds"]) / 3600), 2, MidpointRounding.AwayFromZero);

                dtResult.Rows.Add(dr);
            }

            string strScript;
            Session["rptSrc"] = dtResult;

            strScript = "<script language=javascript>window.open('BySubtaskReport.aspx')</script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "strScript", strScript);
        }
    }
}