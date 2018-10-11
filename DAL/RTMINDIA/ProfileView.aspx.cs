using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;

public partial class ProfileView : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    clsProfile objProfile = new clsProfile();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetUserDetails();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        UpdateDetails();
    }

    private void GetUserDetails()
    {
        dt = objProfile.getUserDetails(Session["user"].ToString());
        if (dt.Rows.Count > 0)
        {
            lblName.Text = dt.Rows[0]["UL_User_Name"].ToString();
            lblTeam.Text = dt.Rows[0]["T_TeamName"].ToString();
            lblEmpId.Text = dt.Rows[0]["UL_Employee_Id"].ToString();
            lblUserName.Text = dt.Rows[0]["UL_System_User_Name"].ToString();
            lblEmail.Text = dt.Rows[0]["UL_EmailId"].ToString();

            txtName.Text = dt.Rows[0]["UL_User_Name"].ToString();
            txtTeam.Text = dt.Rows[0]["T_TeamName"].ToString();
            txtEmployeeId.Text = dt.Rows[0]["UL_Employee_Id"].ToString();
            txtUserName.Text = dt.Rows[0]["UL_System_User_Name"].ToString();
            txtEmail.Text = dt.Rows[0]["UL_EmailId"].ToString();
            if (dt.Rows[0]["UL_Gender"].ToString() == "M")
            {
                lblGender.Text = "Male";
                rbMale.Checked = true;
            }
            else
            {
                lblGender.Text = "Female";
                rbFemale.Checked = true;
            }
        }
    }

    private void UpdateDetails()
    {
        bool result;
        string gender;

        if (rbMale.Checked == true) { gender = "M"; }
        else { gender = "F"; }

        result = objProfile.UpdateProfile(gender, Session["user"].ToString(), txtEmail.Text);

        if (result == true)
        {
            lblMessage.Text = "Updated Successfully";
            lblMessage.ForeColor = System.Drawing.Color.Blue;
        }
        else
        {
            lblMessage.Text = "Failed.. Please try again";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
}