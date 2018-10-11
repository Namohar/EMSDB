using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class EditRecords : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    SqlCommand cmd;
    SqlDataAdapter da;
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblid.Text = Request.QueryString["TID"].ToString();

            LoadEmp();
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
            da = new SqlDataAdapter("select * from RTM_User_List where UL_Team_Id ='" + lblid.Text + "' and UL_User_Status =1 ORDER BY UL_User_Name ", con);
        }
        else if (Session["access"].ToString() == "3")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_Team_Id ='" + lblid.Text + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
            //da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_System_User_Name='" + Session["user"].ToString() + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
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

    }

    private void DisplayRecords()
    {
        dt = new DataTable();

        da = new SqlDataAdapter("select R_ID, R_User_Name, CL_ClientName, TL_Task, STL_SubTask, R_Duration, R_Start_Date_Time, R_Comments from RTM_Records, RTM_Client_List, RTM_Task_List, RTM_SubTask_List " +
                          "where R_Client = CL_ID and R_Task = TL_ID and R_SubTask = STL_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, R_Start_Date_Time))) = '" + txtDate.Text + "' and R_User_Name='" + ddlEmp.SelectedItem.Text + "' order By R_ID, R_Start_Date_Time", con);
        da.Fill(dt);

        gvRecords.DataSource = dt;
        gvRecords.DataBind();
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        if (txtDate.Text.Length <= 0)
        {
            lblError.Text = "Please select Date";
            lblError.ForeColor = Color.Red;
            return;
        }
        DisplayRecords();
    }

    protected void gvRecords_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvRecords.EditIndex = -1;
        DisplayRecords();
    }

    protected void gvRecords_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && gvRecords.EditIndex == e.Row.RowIndex)
        {
            dt = new DataTable();
            DropDownList ddlGvClient = (DropDownList)e.Row.FindControl("ddlGvClient");
            Label lblClient = (Label)e.Row.FindControl("lblEditClient");

            da = new SqlDataAdapter("select * from RTM_Client_List where CL_TeamId= '" + lblid.Text + "' and CL_Status = 1 order By CL_ClientName", con);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddlGvClient.DataSource = dt;
                ddlGvClient.DataTextField = "CL_ClientName";
                ddlGvClient.DataValueField = "CL_ID";
                ddlGvClient.DataBind();

                ddlGvClient.Items.FindByText(lblClient.Text).Selected = true;
            }
            dt = new DataTable();
            DropDownList ddlGvTask = (DropDownList)e.Row.FindControl("ddlGvTask");
            Label lblTask = (Label)e.Row.FindControl("lblEditTask");

            da = new SqlDataAdapter("select * from RTM_Task_List where TL_TeamId = '" + lblid.Text + "' and TL_Status =1 order By TL_Task", con);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddlGvTask.DataSource = dt;
                ddlGvTask.DataTextField = "TL_Task";
                ddlGvTask.DataValueField = "TL_ID";
                ddlGvTask.DataBind();

                ddlGvTask.Items.FindByText(lblTask.Text).Selected = true;
            }
            dt = new DataTable();

            DropDownList ddlGvSubTask = (DropDownList)e.Row.FindControl("ddlGvSubTask");
            Label lblEditSubtask = (Label)e.Row.FindControl("lblEditSubtask");

            da = new SqlDataAdapter("select STL_ID, STL_SubTask from RTM_SubTask_List  where STL_Task_Id ='" + ddlGvTask.SelectedValue + "'", con);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ddlGvSubTask.DataSource = dt;
                ddlGvSubTask.DataTextField = "STL_SubTask";
                ddlGvSubTask.DataValueField = "STL_ID";
                ddlGvSubTask.DataBind();

                ddlGvSubTask.Items.FindByText(lblEditSubtask.Text).Selected = true;
            }
        }
    }

    protected void gvRecords_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvRecords.EditIndex = e.NewEditIndex;
        DisplayRecords();
    }

    protected void gvRecords_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            int rowid = Convert.ToInt32(gvRecords.DataKeys[e.RowIndex].Value.ToString());
            DropDownList ddlGvClient = (DropDownList)gvRecords.Rows[e.RowIndex].FindControl("ddlGvClient");
            DropDownList ddlGvTask = (DropDownList)gvRecords.Rows[e.RowIndex].FindControl("ddlGvTask");
            DropDownList ddlGvSubTask = (DropDownList)gvRecords.Rows[e.RowIndex].FindControl("ddlGvSubTask");
            TextBox txtComments = (TextBox)gvRecords.Rows[e.RowIndex].FindControl("txtComments");

            cmd = new SqlCommand("UPDATE RTM_Records SET R_Client ='" + ddlGvClient.SelectedValue + "', R_Task='" + ddlGvTask.SelectedValue + "', R_SubTask = '" + ddlGvSubTask.SelectedValue + "', R_Comments='" + txtComments.Text + "' where R_ID=" + rowid + "", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            gvRecords.EditIndex = -1;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Update Record", "alert('Updated Successfully')", true);
            DisplayRecords();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.ForeColor = Color.Red;
            gvRecords.EditIndex = -1;
            DisplayRecords();
        }
    }

    protected void ddlGvTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlSubTask = (DropDownList)sender;
        GridViewRow Grow = (GridViewRow)ddlSubTask.NamingContainer;
        DropDownList ddlGvTask = (DropDownList)Grow.FindControl("ddlGvTask");
        DropDownList ddlGvSubTask = (DropDownList)Grow.FindControl("ddlGvSubTask");

        dt = new DataTable();
        da = new SqlDataAdapter("select * from RTM_SubTask_List where STL_Task_Id= '" + ddlGvTask.SelectedValue + "'", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlGvSubTask.DataSource = dt;
            ddlGvSubTask.DataTextField = "STL_SubTask";
            ddlGvSubTask.DataValueField = "STL_ID";
            ddlGvSubTask.DataBind();

        }
    }

    protected void lnkInsert_Click(object sender, EventArgs e)
    {
        lblProcess.Text = "Split";
        LinkButton lnk = (LinkButton)sender;
        GridViewRow currentRow = (GridViewRow)lnk.NamingContainer;
        int rowid = Convert.ToInt32(gvRecords.DataKeys[currentRow.RowIndex].Value.ToString());
        dt = new DataTable();
        da = new SqlDataAdapter("select * from RTM_Records where R_ID=" + rowid, con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            lblTeamId.Text = dt.Rows[0]["R_TeamId"].ToString();
            lblEmpID.Text = dt.Rows[0]["R_Employee_Id"].ToString();
            lblUserName.Text = dt.Rows[0]["R_User_Name"].ToString();
            lblStartTime.Text = dt.Rows[0]["R_Start_Date_Time"].ToString();
            lblEndTime.Text = dt.Rows[0]["R_CreatedOn"].ToString();
            lblMainDuration.Text = "Duration should be less than " + dt.Rows[0]["R_Duration"].ToString();
            lblOldDuration.Text = dt.Rows[0]["R_Duration"].ToString();
            lblOldRId.Text = rowid.ToString();
            LoadClients(Convert.ToInt32(lblTeamId.Text));
            LoadTasks(Convert.ToInt32(lblTeamId.Text));
            mpePopUp.Show();
        }

    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        txtDuration.Text = string.Empty;
        txtComments.Text = string.Empty;
        lblPopError.Text = string.Empty;
        lblOldDuration.Text = string.Empty;
        mpePopUp.Hide();
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            Regex regex = new Regex("^(?:(?:([01]?\\d|2[0-3]):)?([0-5]?\\d):)?([0-5]?\\d)$");

            if (ddlNewClient.SelectedIndex == 0)
            {
                lblPopError.Text = "Please Select Client";
                lblPopError.ForeColor = Color.Red;
                mpePopUp.Show();
            }
            else if (ddlNewTask.SelectedIndex == 0)
            {
                lblPopError.Text = "Please Select Task";
                lblPopError.ForeColor = Color.Red;
                mpePopUp.Show();
            }
            else if (ddlNewSubTask.SelectedIndex == 0)
            {
                lblPopError.Text = "Please Select Sub Task";
                lblPopError.ForeColor = Color.Red;
                mpePopUp.Show();
            }
            else if (txtDuration.Text.Length < 8)
            {
                lblPopError.Text = "Please enter time in HH:MM:SS format";
                lblPopError.ForeColor = Color.Red;
                mpePopUp.Show();
            }
            else if (regex.IsMatch(txtDuration.Text) == false)
            {
                lblPopError.Text = "Please enter valid time in HH:MM:SS format";
                lblPopError.ForeColor = Color.Red;
                mpePopUp.Show();
            }
            else if (TimeSpan.Parse(txtDuration.Text) >= TimeSpan.Parse(lblOldDuration.Text))
            {
                lblPopError.Text = "Duration is incorrect";
                lblPopError.ForeColor = Color.Red;
                mpePopUp.Show();
            }
            else
            {
                if (lblProcess.Text == "Split")
                {
                    TimeSpan dur = TimeSpan.Parse(lblOldDuration.Text).Subtract(TimeSpan.Parse(txtDuration.Text));

                    using (cmd = new SqlCommand())
                    {
                        string comm = txtComments.Text + "-Split";
                        string sQuery = "insert into RTM_Records (R_TeamId, R_Employee_Id, R_User_Name, R_Client, R_Task, R_SubTask, R_Comments, R_Duration, R_Start_Date_Time, R_CreatedOn) values ('" + lblTeamId.Text + "', '" + lblEmpID.Text + "', '" + lblUserName.Text + "', '" + ddlNewClient.SelectedValue + "', '" + ddlNewTask.SelectedValue + "', '" + ddlNewSubTask.SelectedValue + "', '" + comm + "', '" + txtDuration.Text + "', '" + lblStartTime.Text + "', '" + lblEndTime.Text + "')";
                        cmd.CommandText = sQuery;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    using (cmd = new SqlCommand())
                    {
                        string sQuery = "Update RTM_Records SET R_Duration='" + dur + "' where R_ID='" + lblOldRId.Text + "'";
                        cmd.CommandText = sQuery;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    txtDuration.Text = string.Empty;
                    txtComments.Text = string.Empty;
                    lblPopError.Text = string.Empty;
                    lblOldDuration.Text = string.Empty;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Inserted Record", "alert('Saved Successfully')", true);
                    DisplayRecords();
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.ForeColor = Color.Red;
        }
    }

    protected void ddlNewTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNewTask.SelectedIndex != 0)
        {
            LoadSubTasks();
            mpePopUp.Show();
        }
    }

    private void LoadClients(int TID)
    {
        dt = new DataTable();
        da = new SqlDataAdapter("select * from RTM_Client_List where CL_TeamId= " + TID + " and CL_Status = 1 order By CL_ClientName", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlNewClient.DataSource = dt;
            ddlNewClient.DataTextField = "CL_ClientName";
            ddlNewClient.DataValueField = "CL_ID";
            ddlNewClient.DataBind();
            ddlNewClient.Items.Insert(0, "--Select--");
            ddlNewClient.SelectedIndex = 0;
        }
    }

    private void LoadTasks(int TID)
    {
        dt = new DataTable();
        da = new SqlDataAdapter("select * from RTM_Task_List where TL_TeamId = " + TID + " and TL_Status =1 order By TL_Task", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlNewTask.DataSource = dt;
            ddlNewTask.DataTextField = "TL_Task";
            ddlNewTask.DataValueField = "TL_ID";
            ddlNewTask.DataBind();
            ddlNewTask.Items.Insert(0, "--Select--");
            ddlNewTask.SelectedIndex = 0;
        }
    }

    private void LoadSubTasks()
    {
        dt = new DataTable();
        da = new SqlDataAdapter("select * from RTM_SubTask_List where STL_Task_Id= '" + ddlNewTask.SelectedValue + "'", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlNewSubTask.DataSource = dt;
            ddlNewSubTask.DataTextField = "STL_SubTask";
            ddlNewSubTask.DataValueField = "STL_ID";
            ddlNewSubTask.DataBind();
            ddlNewSubTask.Items.Insert(0, "--Select--");
            ddlNewSubTask.SelectedIndex = 0;
        }
    }
}