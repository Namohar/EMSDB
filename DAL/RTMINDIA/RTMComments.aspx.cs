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

public partial class RTMComments : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());

    SqlDataAdapter da;
    DataSet ds = new DataSet();
    SqlCommand cmd;
    SqlDataReader dr;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["access"].ToString() == "3")
            {
                divReminder.Visible = false;
                BindTeamList();
            }
            else if (Session["access"].ToString() == "1")
            {
                BindTeamList();
            }
            else if (Session["access"].ToString() == "4")
            {
                divComments.Visible = false;

            }

            LoadEmp();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int checkedValue = 0;
            string check = "";
            if (ddlTeam.SelectedIndex == 0)
            {
                lblError.Text = "Please select Team";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (rbNo.Checked == true)
                {
                    checkedValue = 0;
                    check = "No";
                }
                else if (rbYes.Checked == true)
                {
                    checkedValue = 1;
                    check = "Yes";
                }
                con.Open();
                cmd = new SqlCommand("UPDATE RTM_Team_List SET T_Status = " + checkedValue + " WHERE T_ID = '" + Convert.ToInt32(ddlTeam.SelectedValue) + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RTM Comments Updated", "alert('RTM Comments for " + ddlTeam.SelectedItem.Text + " is set to " + check + "')", true);
                //lblError.Text = "Updated Successfully";
                //lblError.ForeColor = System.Drawing.Color.Blue;
            }

        }
        catch (Exception)
        {
            lblError.Text = "Failed.. Please try again";
            lblError.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void BindTeamList()
    {
        if (Session["access"].ToString() == "1")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_Team_List where T_Active = 1 order by T_TeamName", con);
        }
        else if (Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_Team_List where T_Manager = '" + Session["username"].ToString() + "' and T_Active = 1 order by T_TeamName", con);
        }
        else if (Session["access"].ToString() == "3")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_Team_List where T_ID = '" + Convert.ToInt32(Session["team"]) + "' and T_Active = 1 order by T_TeamName", con);
        }
        da.Fill(ds, "Team");

        ddlTeam.DataSource = ds.Tables["Team"];
        ddlTeam.DataTextField = "T_TeamName";
        ddlTeam.DataValueField = "T_ID";
        ddlTeam.DataBind();
        ddlTeam.Items.Insert(0, "--Select Team--");
        ddlTeam.SelectedIndex = 0;
    }

    private void CheckLastInput()
    {
        DataTable dt = new DataTable();
        da = new SqlDataAdapter("SELECT T_Status FROM RTM_Team_List WHERE T_ID = '" + Convert.ToInt32(ddlTeam.SelectedValue) + "'", con);
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["T_Status"].ToString() == "0")
            {
                rbNo.Checked = true;
                rbYes.Checked = false;
            }
            else
            {
                rbNo.Checked = false;
                rbYes.Checked = true;
            }
        }
        else
        {
            rbNo.Checked = true;
            rbYes.Checked = false;
        }
    }

    protected void ddlTeam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTeam.SelectedIndex != 0)
        {
            CheckLastInput();
        }
    }

    private void LoadEmp()
    {
        DataTable dt = new DataTable();
        if (Session["access"].ToString() == "1")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_User_Status =1 ORDER BY UL_User_Name", con);
        }
        else if (Session["access"].ToString() == "2")
        {
            da = new SqlDataAdapter("select * from RTM_User_List where UL_Team_Id IN (Select T_ID from RTM_Team_List where T_Manager = '" + Session["username"].ToString() + "') and UL_User_Status =1 ", con);
        }
        else if (Session["access"].ToString() == "3")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_Team_Id ='" + Convert.ToInt32(Session["team"]) + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        }
        else if (Session["access"].ToString() == "4")
        {
            da = new SqlDataAdapter("SELECT * FROM RTM_User_List where UL_System_User_Name='" + Session["user"].ToString() + "' and UL_User_Status =1 ORDER BY UL_User_Name", con);
        }
        //da = new SqlDataAdapter("SELECT * FROM RTM_User_List ORDER BY UL_User_Name", con);
        da.Fill(dt);
        ddlEmp.DataSource = dt;
        ddlEmp.DataValueField = "UL_ID";
        ddlEmp.DataTextField = "UL_User_Name";
        ddlEmp.DataBind();
        ddlEmp.Items.Insert(0, "--Select Employee--");
        ddlEmp.SelectedIndex = 0;

        ddlCustEmp.DataSource = dt;
        ddlCustEmp.DataValueField = "UL_ID";
        ddlCustEmp.DataTextField = "UL_User_Name";
        ddlCustEmp.DataBind();
        ddlCustEmp.Items.Insert(0, "--Select Employee--");
        ddlCustEmp.SelectedIndex = 0;

    }

    protected void ddlEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEmp.SelectedIndex != 0)
        {
            con.Open();
            cmd = new SqlCommand("SELECT UL_Status FROM RTM_User_List where UL_User_Name='" + ddlEmp.SelectedItem.Text + "' ORDER BY UL_User_Name", con);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                if (dr["UL_Status"].ToString() == "1")
                {
                    rbRemNo.Checked = false;
                    rbRemYes.Checked = true;
                }
                else
                {
                    rbRemNo.Checked = true;
                    rbRemYes.Checked = false;
                }
            }
            con.Close();
        }
    }

    protected void btnReminder_Click(object sender, EventArgs e)
    {
        try
        {
            int remCheck = 0;
            string check = "";
            if (ddlEmp.SelectedIndex == 0)
            {
                lblError.Text = "Please select Employee";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                if (rbRemYes.Checked == true)
                {
                    remCheck = 1;
                    check = "Yes";
                }
                else if (rbRemNo.Checked == true)
                {
                    remCheck = 0;
                    check = "No";
                }
                cmd = new SqlCommand("UPDATE RTM_User_List SET UL_Status =" + remCheck + " WHERE UL_User_Name ='" + ddlEmp.SelectedItem.Text + "'", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RTM Task Duration Reminder Updated", "alert('RTM Task Duration Reminder for user " + ddlEmp.SelectedItem.Text + " is set to " + check + "')", true);
                //lblError.Text = "Updated Successfully";
                //lblError.ForeColor = System.Drawing.Color.Blue;
            }
        }
        catch (Exception)
        {
            lblError.Text = "Failed.. Please try again";
            lblError.ForeColor = System.Drawing.Color.Red;
        }

    }

    protected void btnCustomize_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustEmp.SelectedIndex == 0)
            {
                lblError.Text = "Please Select Employee";
                lblError.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (ddlDuration.SelectedIndex == 0)
            {
                lblError.Text = "Please Select Duration";
                lblError.ForeColor = System.Drawing.Color.Red;
                return;
            }

            cmd = new SqlCommand("UPDATE RTM_User_List SET UL_Reminder_Duration =" + ddlDuration.SelectedItem.Text + " WHERE UL_User_Name ='" + ddlCustEmp.SelectedItem.Text + "'", con);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "RTM Task Duration Reminder Updated", "alert('RTM Task Duration Reminder for user " + ddlCustEmp.SelectedItem.Text + " is set to " + ddlDuration.SelectedItem.Text + " Mins')", true);

        }
        catch (Exception)
        {

        }
    }
}