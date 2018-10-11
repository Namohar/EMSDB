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

public partial class SheduledHours : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    SqlDataAdapter da;
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTeamList();
        }
    }

    private void BindGrid()
    {
        if (ds.Tables.Contains("emp"))
        {
            ds.Tables.Remove(ds.Tables["emp"]);
        }
        da = new SqlDataAdapter("SELECT UL_Employee_Id, UL_User_Name, CONVERT(VARCHAR(8),UL_SCH_Login,108) AS UL_SCH_Login, CONVERT(VARCHAR(8),UL_SCH_Logout,108) AS UL_SCH_Logout FROM RTM_User_List WHERE UL_Team_Id = '" + Convert.ToInt32(ddlTeam.SelectedValue) + "' and UL_User_Status =1 order by UL_User_Name", con);
        da.Fill(ds, "emp");
        gvEmployee.DataSource = ds.Tables["emp"];
        gvEmployee.DataBind();
    }

    private void BindTeamList()
    {
        if (ds.Tables.Contains("Team"))
        {
            ds.Tables.Remove(ds.Tables["Team"]);
        }
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

    protected void gvEmployee_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblError.Text = "";
        gvEmployee.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    protected void gvEmployee_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            string empid = ((Label)gvEmployee.Rows[e.RowIndex].FindControl("lblEmpId")).Text;

            DateTime login = Convert.ToDateTime(((TextBox)gvEmployee.Rows[e.RowIndex].FindControl("txtSCHLogin")).Text);

            DateTime logout = Convert.ToDateTime(((TextBox)gvEmployee.Rows[e.RowIndex].FindControl("txtSCHLogout")).Text);

            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE RTM_User_List SET UL_SCH_Login='" + login + "', UL_SCH_Logout='" + logout + "' where UL_Employee_Id = '" + empid + "'", con);
            cmd.ExecuteNonQuery();
            lblError.Text = "Updated Succesfully";
            lblError.ForeColor = System.Drawing.Color.Blue;
            con.Close();
            gvEmployee.EditIndex = -1;
            BindGrid();
        }
        catch (Exception)
        {
            lblError.Text = "Something went wrong..Please try again";
            lblError.ForeColor = System.Drawing.Color.Red;
        }

    }

    protected void gvEmployee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvEmployee.EditIndex = -1;
        BindGrid();
    }

    protected void ddlTeam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTeam.SelectedIndex != 0)
        {
            BindGrid();
        }
    }
}