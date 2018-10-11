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
using System.Text.RegularExpressions;

public partial class Estimate : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    SqlCommand cmd;
    SqlDataAdapter da;
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    int select = 0;
    int checkedCount = 0;
    int seconds;
    int hours1;
    int minutes1;
    int secs1;
    int remainingSecs;
    TimeSpan totalTime;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblid.Text = Request.QueryString["TID"];
            if (Session["access"].ToString() == "3")
            {
                //ddlClient.Visible = false;
                //ddlTask.Visible = false;
                //ddlSubTask.Visible = false;
                //txtDate.Visible = false;
                //txtTime.Visible = false;
                //lblClient.Visible = false;
                //lblTask.Visible = false;
                //lblSubTask.Visible = false;
                //lblDate.Visible = false;
                //lblTime.Visible = false;
                LoadEmp();
                LoadClient();
                LoadTask();
                DisplayRecords();

            }
            else
            {
                LoadEmp();
                LoadClient();
                LoadTask();
                DisplayRecords();
            }
            if (Session["team"].ToString() == "9" || Session["team"].ToString() == "11")
            {
                ddlClient.Visible = true;
                ddlTask.Visible = true;
                ddlSubTask.Visible = true;
                txtDate.Visible = true;
                txtTime.Visible = true;
                lblClient.Visible = true;
                lblTask.Visible = true;
                lblSubTask.Visible = true;
                lblDate.Visible = true;
                lblTime.Visible = true;

                LoadEmp();
                LoadClient();
                LoadTask();
                DisplayRecords();
            }
            LoadMultiClient();
            PreviousEstimates();
        }
    }

    private void LoadClient()
    {

        da = new SqlDataAdapter("select * from RTM_Client_List where CL_TeamId= '" + lblid.Text + "' and CL_Status=1 order by CL_ClientName", con);


        da.Fill(ds, "client");

        ddlClient.DataSource = ds.Tables["client"];
        ddlClient.DataTextField = "CL_ClientName";
        ddlClient.DataValueField = "CL_ID";
        ddlClient.DataBind();
        ddlClient.Items.Insert(0, "-Select Client-");
        ddlClient.SelectedIndex = 0;
    }

    private void LoadMultiClient()
    {

        da = new SqlDataAdapter("select * from RTM_Client_List where CL_TeamId= '" + lblid.Text + "' and CL_Status=1 and CL_ClientName <> 'Other' order by CL_ClientName", con);


        da.Fill(ds, "multiclient");

        if (ds.Tables["multiclient"].Rows.Count > 0)
        {
            ddlMultiple.DataSource = ds.Tables["multiclient"];
            ddlMultiple.DataTextField = "CL_ClientName";
            ddlMultiple.DataValueField = "CL_ID";
            ddlMultiple.DataBind();
        }
    }
    private void LoadTask()
    {

        da = new SqlDataAdapter("select * from RTM_Task_List where TL_TeamId = '" + lblid.Text + "' and TL_Status =1 order by TL_Task", con);

        da.Fill(ds, "task");

        ddlTask.DataSource = ds.Tables["task"];
        ddlTask.DataTextField = "TL_Task";
        ddlTask.DataValueField = "TL_ID";
        ddlTask.DataBind();
        ddlTask.Items.Insert(0, "-Select Task-");
        ddlTask.SelectedIndex = 0;


    }

    private void LoadSubTask()
    {
        da = new SqlDataAdapter("select * from RTM_SubTask_List where STL_Task_Id= '" + Convert.ToInt32(ddlTask.SelectedValue) + "' and STL_ViewStatus =1 order by STL_SubTask", con);

        da.Fill(ds, "subtask");

        ddlSubTask.DataSource = ds.Tables["subtask"];
        ddlSubTask.DataTextField = "STL_SubTask";
        ddlSubTask.DataValueField = "STL_ID";
        ddlSubTask.DataBind();
        ddlSubTask.Items.Insert(0, "-Select SubTask-");
        ddlSubTask.SelectedIndex = 0;
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
            da = new SqlDataAdapter("select * from RTM_User_List where UL_Team_Id ='" + lblid.Text + "' and UL_User_Status =1 ORDER BY UL_User_Name ", con);
        }
        else if (Session["access"].ToString() == "3")
        {
            //da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_Team_Id ='" + lblid.Text + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_System_User_Name='" + Session["user"].ToString() + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        }
        else if (Session["access"].ToString() == "4")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_System_User_Name='" + Session["user"].ToString() + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        }

        da.Fill(dt);

        ddlEmp.DataSource = dt;
        ddlEmp.DataValueField = "UL_ID";
        ddlEmp.DataTextField = "UL_User_Name";
        ddlEmp.DataBind();
        if (Session["team"].ToString() == "11")
        {

        }
        else if (Session["team"].ToString() == "9")
        {

        }
        else
        {
            if (Session["access"].ToString() != "4" && Session["access"].ToString() != "3")
            {
                ddlEmp.Items.Insert(0, "All");
                ddlEmp.SelectedIndex = 0;
            }
        }


    }

    private DataTable GetServiceCode(int taskId, string taskName)
    {
        dt = new DataTable();
        da = new SqlDataAdapter("select STL_ServiceCode from RTM_SubTask_List where STL_SubTask ='" + taskName + "' and STL_Id ='" + taskId + "'", con);
        da.Fill(dt);
        return dt;
    }

    public bool checkRecords()
    {
        dt = new DataTable();
        da = new SqlDataAdapter("select * from RTM_Estimation where EST_ClientId=" + Convert.ToInt32(ddlClient.SelectedValue) + " and EST_TaskId=" + Convert.ToInt32(ddlTask.SelectedValue) + " and EST_SubTaskId=" + Convert.ToInt32(ddlSubTask.Text) + " and EST_TeamId='" + lblid.Text + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EST_Date)))='" + txtDate.Text + "' and EST_UserName='" + ddlEmp.SelectedItem.Text + "'", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }


    protected void ddlEmp_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMultiple.Visible = false;
        ddlMultiple.Visible = false;
        if (ddlTask.SelectedIndex != 0)
        {
            LoadSubTask();
        }
    }
    protected void ddlSubTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Session["access"].ToString() == "3" || Session["access"].ToString() == "1")
        //{
        dt = new DataTable();
        if (ddlClient.SelectedItem.Text == "Other")
        {
            dt = GetServiceCode(Convert.ToInt32(ddlSubTask.SelectedValue), ddlSubTask.SelectedItem.Text);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["STL_ServiceCode"].ToString().Contains("General/Internal") == false)
                {
                    ddlMultiple.Visible = true;
                    lblMultiple.Visible = true;
                }
                else
                {
                    ddlMultiple.Visible = false;
                    lblMultiple.Visible = false;
                }
            }
        }

        //}
    }

    private void PreviousEstimates()
    {
        dt = new DataTable();
        da = new SqlDataAdapter("select Distinct CL_ClientName,EST_ClientId, TL_Task ,EST_TaskId,  STL_SubTask, EST_SubTaskId from dbo.RTM_Estimation, dbo.RTM_Client_List, dbo.RTM_Task_List,dbo.RTM_SubTask_List  where EST_ClientId = CL_ID and EST_TaskId = TL_ID and EST_SubTaskId = STL_ID and EST_UserName = '" + Session["username"].ToString() + "' and EST_CreatedOn >= '" + DateTime.Now.AddDays(-14) + "'", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            gvPreviousEstimates.DataSource = dt;
            gvPreviousEstimates.DataBind();
        }
        else
        {
            gvPreviousEstimates.DataSource = null;
            gvPreviousEstimates.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        SaveRecord();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            int flag = 0;
            foreach (GridViewRow row in gvPreviousEstimates.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Regex regex = new Regex("^(?:(?:([01]?\\d|2[0-3]):)?([0-5]?\\d):)?([0-5]?\\d)$");

                        string clientid = (row.Cells[1].FindControl("lblClientId") as Label).Text;
                        string taskid = (row.Cells[2].FindControl("lblTaskId") as Label).Text;
                        string subtaskid = (row.Cells[3].FindControl("lblSubTaskId") as Label).Text;
                        string duration = (row.Cells[4].FindControl("txtNewDuration") as TextBox).Text;
                        string comments = (row.Cells[5].FindControl("txtNewComments") as TextBox).Text;

                        if (duration.Length > 0 && regex.IsMatch(duration))
                        {
                            con.Open();
                            cmd = new SqlCommand("insert into RTM_Estimation (EST_TeamId,EST_UserId, EST_UserName, EST_ClientId, EST_TaskId, EST_SubTaskId, EST_Duration, EST_Date, EST_CreatedOn, EST_Comments) values(" + Convert.ToInt32(lblid.Text) + ",'" + Session["UID"].ToString() + "', '" + Session["username"].ToString() + "', " + Convert.ToInt32(clientid) + ", " + Convert.ToInt32(taskid) + ", " + Convert.ToInt32(subtaskid) + ", '" + duration + "', '" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now + "', '" + comments + "')", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        if (regex.IsMatch(duration) == false)
                        {
                            flag = 1;
                        }

                    }
                }
            }
            if (flag == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Inserted Record", "alert('Few records are not added due to invalid duration.')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Inserted Record", "alert('Estimate records added Successfully.')", true);
            }

            PreviousEstimates();
            DisplayRecords();
        }
        catch (Exception ex)
        {

            lblError.Text = ex.Message;
            lblError.ForeColor = Color.Red;
        }
    }

    private void SaveRecord()
    {
        try
        {
            Regex regex = new Regex("^(?:(?:([01]?\\d|2[0-3]):)?([0-5]?\\d):)?([0-5]?\\d)$");

            if (ddlMultiple.Visible == false)
            {
                bool check = checkRecords();
                if (check == true)
                {
                    lblError.Text = "Client + Task + Sub-Task combination is already present. Please check the data.";
                    return;
                }
            }
            if (ddlClient.SelectedIndex == 0)
            {
                lblError.Text = "Please select client";
            }
            else if (ddlTask.SelectedIndex == 0)
            {
                lblError.Text = "Please select task";
            }
            else if (ddlSubTask.SelectedIndex == 0)
            {
                lblError.Text = "Please select subtask";
            }
            else if (txtDate.Text.Length <= 0)
            {
                lblError.Text = "Please select Date";
            }
            else if (txtTime.Text.Length < 8)
            {
                lblError.Text = "Please enter time in HH:MM:SS format";
            }
            else if (regex.IsMatch(txtTime.Text) == false)
            {
                lblError.Text = "Please enter valid time in HH:MM:SS format";
            }

            else
            {
                if (ddlMultiple.Visible == false)
                {
                    lblError.Text = "";
                    int userid;
                    con.Open();
                    if (ddlEmp.SelectedIndex == 0)
                    {
                        userid = Convert.ToInt32(ddlEmp.SelectedValue);
                    }
                    else
                    {
                        userid = Convert.ToInt32(ddlEmp.SelectedValue);
                    }
                    cmd = new SqlCommand("insert into RTM_Estimation (EST_TeamId,EST_UserId, EST_UserName, EST_ClientId, EST_TaskId, EST_SubTaskId, EST_Duration, EST_Date, EST_CreatedOn, EST_Comments) values(" + Convert.ToInt32(lblid.Text) + "," + userid + ", '" + ddlEmp.SelectedItem.Text + "', " + Convert.ToInt32(ddlClient.SelectedValue) + ", " + Convert.ToInt32(ddlTask.SelectedValue) + ", " + Convert.ToInt32(ddlSubTask.SelectedValue) + ", '" + txtTime.Text + "', '" + txtDate.Text + "', '" + DateTime.Now + "', '" + txtComments.Text + "')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    clearAll();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Inserted Record", "alert('Saved Successfully')", true);
                    DisplayRecords();
                }
                else
                {
                    //lblEmp.Text = "";
                    int userid = Convert.ToInt32(ddlEmp.SelectedValue);
                    foreach (System.Web.UI.WebControls.ListItem item in ddlMultiple.Items)
                    {
                        if (item.Selected)
                        {
                            checkedCount = checkedCount + 1;
                        }
                    }

                    if (checkedCount == 0)
                    {
                        lblError.Text = "Please select the clients";
                        lblError.ForeColor = Color.Red;
                        ddlMultiple.Focus();
                        return;
                    }
                    else
                    {
                        seconds = Convert.ToInt32(TimeSpan.Parse(txtTime.Text).TotalSeconds);
                        seconds = seconds / checkedCount;

                        var hms = TimeSpan.FromSeconds(seconds);
                        var h = hms.Hours.ToString("00");
                        var m = hms.Minutes.ToString("00");
                        var s = hms.Seconds.ToString("00");
                        //string totalTime = h + ":" + m + ":" + s;
                        totalTime = TimeSpan.Parse(h + ":" + m + ":" + s);
                        //hours1 = seconds / 3600;
                        //remainingSecs = seconds - (hours1 * 3600);
                        //minutes1 = remainingSecs / 60;
                        //secs1 = seconds - (hours1 * 3600) - (minutes1 * 60);
                        //totalTime = TimeSpan.Parse(hours1 + ":" + minutes1 + ":" + secs1);
                    }

                    foreach (System.Web.UI.WebControls.ListItem item in ddlMultiple.Items)
                    {
                        if (item.Selected)
                        {
                            select = 1;
                            con.Open();
                            cmd = new SqlCommand("insert into RTM_Estimation (EST_TeamId,EST_UserId, EST_UserName, EST_ClientId, EST_TaskId, EST_SubTaskId, EST_Duration, EST_Date, EST_CreatedOn, EST_Comments) values(" + Convert.ToInt32(lblid.Text) + "," + userid + ", '" + ddlEmp.SelectedItem.Text + "', " + Convert.ToInt32(item.Value) + ", " + Convert.ToInt32(ddlTask.SelectedValue) + ", " + Convert.ToInt32(ddlSubTask.SelectedValue) + ", '" + totalTime.ToString() + "', '" + txtDate.Text + "', '" + DateTime.Now + "', '" + txtComments.Text + "')", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                    clearAll();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Inserted Record", "alert('Saved Successfully')", true);
                    DisplayRecords();

                }

            }

        }
        catch (Exception)
        {


        }
    }

    private void clearAll()
    {
        ddlEmp.SelectedIndex = 0;
        ddlClient.SelectedIndex = 0;
        ddlTask.SelectedIndex = 0;
        ddlSubTask.DataSource = null;
        ddlSubTask.DataBind();
        ddlMultiple.ClearSelection();
        ddlMultiple.Visible = false;
        lblMultiple.Visible = false;
        txtDate.Text = "";
        txtTime.Text = "";
    }

    private void DisplayRecords()
    {
        dt = new DataTable();
        //if (Session["access"].ToString() == "3")
        //{
        //    if (ddlEmp.SelectedIndex == 0)
        //    {
        //        da = new SqlDataAdapter("select EST_ID, EST_UserName, CL_ClientName, TL_Task, STL_SubTask, EST_Duration,EST_Comments, CONVERT(VARCHAR(10),EST_Date,101) as EST_Date from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID " +
        //     "left join rtm_subtask_list on EST_SubTaskId = STL_ID where EST_Date >= '" + DateTime.Now.ToShortDateString() + "' and EST_TeamId='"+ Convert.ToInt32(lblid.Text) +"' ", con);
        //    }
        //    else
        //    {
        //        da = new SqlDataAdapter("select EST_ID, EST_UserName, CL_ClientName, TL_Task, STL_SubTask, EST_Duration,EST_Comments, CONVERT(VARCHAR(10),EST_Date,101) as EST_Date from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID " +
        //     "left join rtm_subtask_list on EST_SubTaskId = STL_ID where EST_Date >= '" + DateTime.Now.ToShortDateString() + "' and EST_UserName ='" + ddlEmp.SelectedItem.Text + "' and EST_TeamId='" + Convert.ToInt32(lblid.Text) + "' ", con);
        //    }
        //}
        if (Session["access"].ToString() == "1")
        {
            da = new SqlDataAdapter("select EST_ID, EST_UserName, CL_ClientName, TL_Task, STL_SubTask, EST_Duration,EST_Comments, CONVERT(VARCHAR(10),EST_Date,101) as EST_Date from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID " +
            "left join rtm_subtask_list on EST_SubTaskId = STL_ID where EST_Date >= '" + DateTime.Now.ToShortDateString() + "' and EST_TeamId='" + Convert.ToInt32(lblid.Text) + "' ", con);
        }
        else
        {
            da = new SqlDataAdapter("select EST_ID, EST_UserName, CL_ClientName, TL_Task, STL_SubTask, EST_Duration,EST_Comments, CONVERT(VARCHAR(10),EST_Date,101) as EST_Date from RTM_Estimation left join rtm_client_list on EST_ClientId = CL_ID left join rtm_task_list on EST_TaskId = TL_ID " +
             "left join rtm_subtask_list on EST_SubTaskId = STL_ID where EST_Date >= '" + DateTime.Now.ToShortDateString() + "' and EST_UserName ='" + ddlEmp.SelectedItem.Text + "' and EST_TeamId='" + Convert.ToInt32(lblid.Text) + "' ", con);
        }

        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            gvEstimateNew.DataSource = dt;
            gvEstimateNew.DataBind();
        }
        else
        {
            gvEstimateNew.DataSource = null;
            gvEstimateNew.DataBind();
        }
    }

    protected void gvEstimateNew_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvEstimateNew.EditIndex = e.NewEditIndex;
        DisplayRecords();
    }

    protected void gvEstimateNew_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvEstimateNew.EditIndex = -1;
        DisplayRecords();
    }

    protected void gvEstimateNew_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int rowid = Convert.ToInt32(gvEstimateNew.DataKeys[e.RowIndex].Value.ToString());
            DropDownList ddlGvClient = (DropDownList)gvEstimateNew.Rows[e.RowIndex].FindControl("ddlGvClient");
            DropDownList ddlGvTask = (DropDownList)gvEstimateNew.Rows[e.RowIndex].FindControl("ddlGvTask");
            DropDownList ddlGvSubTask = (DropDownList)gvEstimateNew.Rows[e.RowIndex].FindControl("ddlGvSubTask");
            TextBox txtDur = (TextBox)gvEstimateNew.Rows[e.RowIndex].FindControl("txtDuration");
            TextBox txtEditComment = (TextBox)gvEstimateNew.Rows[e.RowIndex].FindControl("txtEditComm");

            Regex regex = new Regex("^(?:(?:([01]?\\d|2[0-3]):)?([0-5]?\\d):)?([0-5]?\\d)$");


            if (ddlGvTask.SelectedIndex == 0)
            {
                lblError.Text = "Please Select Task";
                return;
            }
            if (ddlGvSubTask.SelectedIndex == 0)
            {
                lblError.Text = "Please Select SubTask";
                return;
            }
            if (ddlGvClient.SelectedIndex == 0)
            {
                lblError.Text = "Please Select Client";
                return;
            }
            if (txtDur.Text.Length < 8)
            {
                lblError.Text = "Please enter time in HH:MM:SS format";
                return;
            }
            if (regex.IsMatch(txtDur.Text) == false)
            {
                lblError.Text = "Please enter valid time in HH:MM:SS format";
                return;
            }

            cmd = new SqlCommand("UPDATE RTM_Estimation SET EST_ClientId='" + ddlGvClient.SelectedValue + "', EST_TaskId='" + ddlGvTask.SelectedValue + "', EST_SubTaskId='" + ddlGvSubTask.SelectedValue + "', EST_Duration='" + txtDur.Text + "', EST_Comments='" + txtEditComment.Text + "' where EST_ID='" + rowid + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            gvEstimateNew.EditIndex = -1;
            DisplayRecords();
        }
        catch (Exception)
        {

        }
    }

    protected void gvEstimateNew_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gvEstimateNew.EditIndex == e.Row.RowIndex)
        {
            dt = new DataTable();

            DropDownList ddlGvTask = (DropDownList)e.Row.FindControl("ddlGvTask");
            Label lblTask = (Label)e.Row.FindControl("lblTask");

            da = new SqlDataAdapter("select * from RTM_Task_List where TL_TeamId = '" + lblid.Text + "' and TL_Status =1", con);
            da.Fill(dt);
            ddlGvTask.DataSource = dt;
            ddlGvTask.DataTextField = "TL_Task";
            ddlGvTask.DataValueField = "TL_ID";
            ddlGvTask.DataBind();
            ddlGvTask.Items.Insert(0, "--Select Task--");
            ddlGvTask.SelectedIndex = 0;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (lblid.Text == "10")
            {
                if (DateTime.Now.Hour < 11 || DateTime.Now.Hour >= 16)
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    Button1.Enabled = false;
                }
                else
                {
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                    Button1.Enabled = true;
                }
            }
            else
            {
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = true;
                Button1.Enabled = true;
            }
            if (lblid.Text == "13")
            {
                if (DateTime.Now.Hour < 9 || DateTime.Now.Hour >= 14)
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    Button1.Enabled = false;
                }
                else
                {
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                    Button1.Enabled = true;
                }
            }
            else
            {
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = true;
                Button1.Enabled = true;
            }
            if (lblid.Text == "22")
            {
                if ((DateTime.Now.Hour < 7) || (DateTime.Now.Hour >= 11 && DateTime.Now.Minute >= 30))
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                    Button1.Enabled = false;
                }
                else
                {
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                    Button1.Enabled = true;
                }
            }
            else
            {
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = true;
                Button1.Enabled = true;
            }
            if (lblid.Text == "23")
            {
                if ((DateTime.Now.Hour < 7) || (DateTime.Now.Hour >= 11 && DateTime.Now.Minute >= 30))
                {
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    Button1.Enabled = false;
                }
                else
                {
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                    Button1.Enabled = true;
                }
            }
            else
            {
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = true;
                Button1.Enabled = true;
            }

            if (lblid.Text == "8")
            {
                if ((DateTime.Now.Hour < 7) || (DateTime.Now.Hour >= 14))
                {
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    Button1.Enabled = false;
                }
                else
                {
                    e.Row.Cells[7].Visible = true;
                    e.Row.Cells[8].Visible = true;
                    Button1.Enabled = true;
                }
            }
            else
            {
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = true;
                Button1.Enabled = true;
            }

        }
    }

    protected void ddlGvTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlSubTask = (DropDownList)sender;
        GridViewRow Grow = (GridViewRow)ddlSubTask.NamingContainer;
        DropDownList ddlGvTask = (DropDownList)Grow.FindControl("ddlGvTask");
        DropDownList ddlGvSubTask = (DropDownList)Grow.FindControl("ddlGvSubTask");
        int taskid = Convert.ToInt32(ddlGvTask.SelectedValue);
        string taskname = ddlGvTask.SelectedItem.Text;
        if (ddlGvTask.SelectedIndex != 0)
        {
            dt = new DataTable();
            da = new SqlDataAdapter("select * from RTM_SubTask_List where STL_Task_Id= '" + taskid + "'", con);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddlGvSubTask.DataSource = dt;
                ddlGvSubTask.DataTextField = "STL_SubTask";
                ddlGvSubTask.DataValueField = "STL_ID";
                ddlGvSubTask.DataBind();
                ddlGvSubTask.Items.Insert(0, "-Select SubTask-");
                ddlGvSubTask.SelectedIndex = 0;
            }
        }

    }

    protected void ddlGvSubTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        dt = new DataTable();
        DropDownList ddlClient = (DropDownList)sender;
        GridViewRow Grow = (GridViewRow)ddlClient.NamingContainer;
        DropDownList ddlGvClient = (DropDownList)Grow.FindControl("ddlGvClient");
        DropDownList ddlGvSubTask = (DropDownList)Grow.FindControl("ddlGvSubTask");

        int subTaskId = Convert.ToInt32(ddlGvSubTask.SelectedValue);

        if (ddlGvSubTask.SelectedIndex != 0)
        {
            dt = GetServiceCode(Convert.ToInt32(ddlGvSubTask.SelectedValue), ddlGvSubTask.SelectedItem.Text);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["STL_ServiceCode"].ToString().Contains("General/Internal") == false)
                {
                    dt = new DataTable();
                    da = new SqlDataAdapter("select * from RTM_Client_List where CL_TeamId= '" + lblid.Text + "' and CL_Status=1 and CL_ClientName <> 'Other' order by CL_ClientName", con);
                    da.Fill(dt);
                    ddlGvClient.DataSource = dt;
                    ddlGvClient.DataTextField = "CL_ClientName";
                    ddlGvClient.DataValueField = "CL_ID";
                    ddlGvClient.DataBind();
                    ddlGvClient.Items.Insert(0, "-Select Client-");
                    ddlGvClient.SelectedIndex = 0;
                }
                else
                {
                    dt = new DataTable();
                    da = new SqlDataAdapter("select * from RTM_Client_List where CL_TeamId= '" + lblid.Text + "' and CL_Status=1 and CL_ClientName = 'Other'", con);
                    da.Fill(dt);
                    ddlGvClient.DataSource = dt;
                    ddlGvClient.DataTextField = "CL_ClientName";
                    ddlGvClient.DataValueField = "CL_ID";
                    ddlGvClient.DataBind();
                    ddlGvClient.Items.Insert(0, "-Select Client-");
                    ddlGvClient.SelectedIndex = 0;
                }
            }
        }


    }

    protected void DeleteRecord(object sender, EventArgs e)
    {
        LinkButton lnkRemove = (LinkButton)sender;
        string recId = lnkRemove.CommandArgument;
        cmd = new SqlCommand("Delete from RTM_Estimation where EST_ID='" + recId + "'", con);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
        DisplayRecords();
    }
}