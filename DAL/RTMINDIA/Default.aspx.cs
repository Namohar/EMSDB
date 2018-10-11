using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Principal;
using BAL;

public partial class _Default : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    SqlCommand cmd;
    SqlDataAdapter da;
    DataTable dt = new DataTable();
    clsUserLogin objLogin = new clsUserLogin();
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Security.Principal.WindowsPrincipal user;
        user = new WindowsPrincipal(this.Request.LogonUserIdentity);

        lblUserName.Text = user.Identity.Name;
        if (!IsPostBack)
        {
            getLocations();
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        System.Security.Principal.WindowsPrincipal user;
        user = new WindowsPrincipal(this.Request.LogonUserIdentity);
        dt = new DataTable();
        dt = objLogin.getUser(user.Identity.Name);
       
        if (dt.Rows.Count > 0)
        {
            string team = dt.Rows[0]["UL_Team_Id"].ToString();
            string accessLevel = dt.Rows[0]["AL_AccessLevel"].ToString();
            Session["UID"] = dt.Rows[0]["UL_ID"].ToString();
            Session["team"] = team;
            Session["access"] = accessLevel;
            Session["user"] = lblUserName.Text;
            Session["username"] = dt.Rows[0]["UL_User_Name"].ToString();
            Session["Location"] = dt.Rows[0]["T_Location"].ToString();
            Session["preference"] = dt.Rows[0]["T_Preference"].ToString();
            if (accessLevel == "1")
            {
                mpePopUp.Show();
                
            }
            else
            {
                Response.Redirect("~/DashBoard.aspx");
            }
        }
        else
        {
            lblError.Text = "You are not authorized to access reports";
            lblError.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void getLocations()
    {
        dt = new DataTable();
        dt = objLogin.getLocations();

        if (dt.Rows.Count > 0)
        {
            ddlLocation.DataSource = dt;
            ddlLocation.DataTextField = "T_Location";
            ddlLocation.DataValueField = "T_Location";
            ddlLocation.DataBind();
        }
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        Session["Location"] = ddlLocation.SelectedItem.Text;
        Response.Redirect("~/DashBoard_Admin.aspx");
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {

    }

    //private DataTable getUser(string user)
    //{
    //    da = new SqlDataAdapter("select UL_ID, UL_Team_Id, UL_User_Name, AL_AccessLevel, T_Location, T_Preference from RTM_User_List, RTM_Access_Level, dbo.RTM_Team_List where UL_Employee_Id = AL_EmployeeId and UL_Team_Id = T_ID and UL_System_User_Name ='" + user + "'", con);
    //    da.Fill(dt);
    //    return dt;
    //}
}