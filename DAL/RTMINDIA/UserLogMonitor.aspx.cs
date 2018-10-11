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
using System.Drawing;
using System.Text;
using System.Net.Mail;

public partial class UserLogMonitor : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
    DataTable dt = new DataTable();
    SqlDataAdapter da = new SqlDataAdapter();
    clsUserLogMonitor objUserLog = new clsUserLogMonitor();
    DataSet ds = new DataSet();
    StringBuilder myBuilder = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            LoadTeams();
            btnExport.Enabled = false;
            btnSend.Enabled = false;
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

    private DataSet fecthScheduledTimes(int TID)
    {
        if (ds.Tables.Contains("times"))
        {
            ds.Tables.Remove(ds.Tables["times"]);
        }
        da = new SqlDataAdapter("select distinct Convert(VARCHAR, UL_SCH_Login,108) as [Scheduled] from RTM_User_List " +
                            "WHERE UL_Team_Id =" + TID + " and UL_User_status =1 order by Convert(VARCHAR, UL_SCH_Login,108)", con);

        da.Fill(ds, "times");
        return ds;
    }

    private DataSet fetchUsers(int TID, string time)
    {
        if (ds.Tables.Contains("users"))
        {
            ds.Tables.Remove(ds.Tables["users"]);
        }
        da = new SqlDataAdapter("select UL_User_Name, UL_Employee_Id, Convert(VARCHAR, UL_SCH_Login,108) as [Sch Login], Convert(VARCHAR, UL_SCH_Logout,108) as [Sch Logoff] from RTM_User_List where Convert(VARCHAR, UL_SCH_Login,108) = '" + time + "' and UL_Team_Id =" + TID + " and UL_User_Status=1", con);
        da.Fill(ds, "users");
        return ds;
    }

    private DataSet fetchActualLogin(string user)
    {
        if (ds.Tables.Contains("actual"))
        {
            ds.Tables.Remove(ds.Tables["actual"]);
        }
        da = new SqlDataAdapter("select LA_Start_Date_Time, Convert(VARCHAR, LA_Start_Date_Time,108) as [Actual] from RTM_Log_Actions where LA_User_Name='" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) = '" + txtDate.Text + "' and la_log_action ='Actual Login'", con);
        da.Fill(ds, "actual");
        return ds;
    }

    private DataSet fetchActualLogOff(string user)
    {
        if (ds.Tables.Contains("actual"))
        {
            ds.Tables.Remove(ds.Tables["actual"]);
        }
        da = new SqlDataAdapter("select LA_Start_Date_Time, Convert(VARCHAR, LA_Start_Date_Time,108) as [Actual] from RTM_Log_Actions where LA_User_Name='" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, LA_Start_Date_Time))) >= '" + txtDate.Text + "' and la_log_action ='Actual Logout' order by LA_Start_Date_Time", con);
        da.Fill(ds, "actual");
        return ds;
    }

    private DataSet fetchDelayReason(string user, string type)
    {
        if (ds.Tables.Contains("reason"))
        {
            ds.Tables.Remove(ds.Tables["reason"]);
        }
        if (type == "In")
        {
            da = new SqlDataAdapter("select * from RTM_DelayedLogInOff where D_UserName = '" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, D_Date))) = '" + txtDate.Text + "' and D_Type = 'In'", con);
        }
        else
        {
            da = new SqlDataAdapter("select * from RTM_DelayedLogInOff where D_UserName = '" + user + "' and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, D_Date))) = '" + txtDate.Text + "' and D_Type = 'Off'", con);
        }

        da.Fill(ds, "reason");
        return ds;
    }

    private void BuildLogTable()
    {
        dt = new DataTable();

        DataColumn dc;

        dc = new DataColumn("User Name");
        dt.Columns.Add(dc);

        dc = new DataColumn("Employee Id");
        dt.Columns.Add(dc);

        dc = new DataColumn("Date");
        dt.Columns.Add(dc);

        dc = new DataColumn("Sceduled Login");
        dt.Columns.Add(dc);

        dc = new DataColumn("Actual Login");
        dt.Columns.Add(dc);

        dc = new DataColumn("Delayed Login");
        dt.Columns.Add(dc);

        dc = new DataColumn("Delay Login Reason");
        dt.Columns.Add(dc);

        dc = new DataColumn("Scheduled Logoff");
        dt.Columns.Add(dc);

        dc = new DataColumn("Actual Logoff");
        dt.Columns.Add(dc);

        dc = new DataColumn("Delayed Logoff");
        dt.Columns.Add(dc);

        dc = new DataColumn("Delay Logoff Reason");
        dt.Columns.Add(dc);
    }

    private void DisplayData()
    {
        int flagActual = 0;
        DataRow dr;
        ds = fecthScheduledTimes(Convert.ToInt32(ddlTeam.SelectedValue));
        for (int i = 0; i <= ds.Tables["times"].Rows.Count - 1; i++)
        {
            BuildLogTable();
            flagActual = 0;
            string time = ds.Tables["times"].Rows[i]["Scheduled"].ToString();
            ds = fetchUsers(Convert.ToInt32(ddlTeam.SelectedValue), time);
            foreach (DataRow dr1 in ds.Tables["users"].Rows)
            {
                dr = dt.NewRow();

                dr["User Name"] = dr1["UL_User_Name"];
                dr["Employee Id"] = dr1["UL_Employee_Id"];
                dr["Date"] = txtDate.Text;
                dr["Sceduled Login"] = dr1["Sch Login"];
                dr["Scheduled Logoff"] = dr1["Sch Logoff"];
                ds = fetchActualLogin(dr1["UL_User_Name"].ToString());
                if (ds.Tables["actual"].Rows.Count > 0)
                {
                    flagActual = 1;
                    dr["Actual Login"] = ds.Tables["actual"].Rows[0]["Actual"].ToString();
                }
                ds = fetchDelayReason(dr1["UL_User_Name"].ToString(), "In");
                if (ds.Tables["reason"].Rows.Count > 0)
                {
                    TimeSpan delay = TimeSpan.Parse(dr["Actual Login"].ToString()).Subtract(TimeSpan.Parse(dr["Sceduled Login"].ToString()));
                    dr["Delayed Login"] = delay;
                    dr["Delay Login Reason"] = ds.Tables["reason"].Rows[0]["D_Reason"].ToString();
                }
                ds = fetchActualLogOff(dr1["UL_User_Name"].ToString());
                if (ds.Tables["actual"].Rows.Count > 0)
                {
                    dr["Actual Logoff"] = ds.Tables["actual"].Rows[0]["Actual"].ToString();
                }
                ds = fetchDelayReason(dr1["UL_User_Name"].ToString(), "Off");
                if (ds.Tables["reason"].Rows.Count > 0)
                {
                    TimeSpan delay = TimeSpan.Parse(dr["Actual Logoff"].ToString()).Subtract(TimeSpan.Parse(dr["Scheduled Logoff"].ToString()));
                    dr["Delayed Logoff"] = delay;
                    dr["Delay Logoff Reason"] = ds.Tables["reason"].Rows[0]["D_Reason"].ToString();
                }

                dt.Rows.Add(dr);
            }
            if (flagActual == 1)
            {
                GridView grd = new GridView();
                grd.ID = "GridView" + i.ToString();
                grd.DataSource = dt;
                grd.DataBind();
                grd.HorizontalAlign = HorizontalAlign.Center;
                grd.AlternatingRowStyle.BackColor = Color.White;
                grd.AlternatingRowStyle.ForeColor = ColorTranslator.FromHtml("#284775");
                grd.HeaderStyle.BackColor = ColorTranslator.FromHtml("#5D7B9D");
                grd.HeaderStyle.Font.Bold = true;
                grd.HeaderStyle.ForeColor = Color.White;
                grd.Width = 900;
                pnlResult.Controls.Add(grd);
                pnlResult.Controls.Add(new LiteralControl("<br />"));
            }

        }
    }

    protected void ddlTeam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlTeam.SelectedIndex > 0)
            {
                int access = Convert.ToInt32(Session["access"]);
                DisplayData();
                btnExport.Enabled = true;
                if (access == 1)
                {
                    btnSend.Enabled = true;
                }

            }
            else
            {
                btnExport.Enabled = false;
                btnSend.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = Color.Red;
        }

    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        ddlTeam.SelectedIndex = 0;
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        DisplayData();
        txtDetails.Value = string.Empty;
        mpePopUp.Show();
        GetTimes();
        GetLeads();
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        int checkedCount = 0;
        int checkedCount2 = 0;
        foreach (System.Web.UI.WebControls.ListItem item in ddlMultipleTime.Items)
        {
            if (item.Selected)
            {
                checkedCount = checkedCount + 1;
            }
        }
        foreach (System.Web.UI.WebControls.ListItem item in ddlEmails.Items)
        {
            if (item.Selected)
            {
                checkedCount2 = checkedCount2 + 1;
            }
        }
        if (checkedCount == 0)
        {
            lblMessage.Text = "Please select Times to send mail";
            lblMessage.ForeColor = Color.Red;
            mpePopUp.Show();
            DisplayData();
        }
        else if (checkedCount2 == 0)
        {
            lblMessage.Text = "Please select users to send mail";
            lblMessage.ForeColor = Color.Red;
            mpePopUp.Show();
            DisplayData();
        }
        else
        {
            fetchDataToSend();
            mpePopUp.Show();
            DisplayData();
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        txtDetails.Value = string.Empty;
        mpePopUp.Hide();
        DisplayData();
    }

    private void GetLeads()
    {
        dt = new DataTable();
        da = new SqlDataAdapter("select UL_User_Name, UL_EmailId from RTM_User_List, dbo.RTM_Access_Level where UL_Employee_Id = AL_EmployeeId and (AL_AccessLevel =3 or AL_AccessLevel=2) and UL_Team_Id= " + ddlTeam.SelectedValue + " order by UL_User_Name", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            ddlEmails.DataSource = dt;
            ddlEmails.DataTextField = "UL_User_Name";
            ddlEmails.DataValueField = "UL_EmailId";
            ddlEmails.DataBind();
        }
    }

    private void GetTimes()
    {
        ds = fecthScheduledTimes(Convert.ToInt32(ddlTeam.SelectedValue));
        if (ds.Tables["times"].Rows.Count > 0)
        {
            ddlMultipleTime.DataSource = ds.Tables["times"];
            ddlMultipleTime.DataTextField = "Scheduled";
            ddlMultipleTime.DataValueField = "Scheduled";
            ddlMultipleTime.DataBind();
        }
    }

    private void fetchDataToSend()
    {
        try
        {
            DataRow dr;
            BuildLogTable();
            ds = fecthScheduledTimes(Convert.ToInt32(ddlTeam.SelectedValue));
            foreach (System.Web.UI.WebControls.ListItem item in ddlMultipleTime.Items)
            {
                if (item.Selected)
                {
                    string time = item.Value;
                    ds = fetchUsers(Convert.ToInt32(ddlTeam.SelectedValue), time);
                    foreach (DataRow dr1 in ds.Tables["users"].Rows)
                    {
                        dr = dt.NewRow();

                        dr["User Name"] = dr1["UL_User_Name"];
                        dr["Employee Id"] = dr1["UL_Employee_Id"];
                        dr["Date"] = txtDate.Text;
                        dr["Sceduled Login"] = dr1["Sch Login"];
                        dr["Scheduled Logoff"] = dr1["Sch Logoff"];
                        ds = fetchActualLogin(dr1["UL_User_Name"].ToString());
                        if (ds.Tables["actual"].Rows.Count > 0)
                        {

                            dr["Actual Login"] = ds.Tables["actual"].Rows[0]["Actual"].ToString();
                        }
                        ds = fetchDelayReason(dr1["UL_User_Name"].ToString(), "In");
                        if (ds.Tables["reason"].Rows.Count > 0)
                        {
                            TimeSpan delay = TimeSpan.Parse(dr["Actual Login"].ToString()).Subtract(TimeSpan.Parse(dr["Sceduled Login"].ToString()));
                            dr["Delayed Login"] = delay;
                            dr["Delay Login Reason"] = ds.Tables["reason"].Rows[0]["D_Reason"].ToString();
                        }

                        ds = fetchActualLogOff(dr1["UL_User_Name"].ToString());
                        if (ds.Tables["actual"].Rows.Count > 0)
                        {
                            dr["Actual Logoff"] = ds.Tables["actual"].Rows[0]["Actual"].ToString();
                        }
                        ds = fetchDelayReason(dr1["UL_User_Name"].ToString(), "Off");
                        if (ds.Tables["reason"].Rows.Count > 0)
                        {
                            TimeSpan delay = TimeSpan.Parse(dr["Actual Logoff"].ToString()).Subtract(TimeSpan.Parse(dr["Scheduled Logoff"].ToString()));
                            dr["Delayed Logoff"] = delay;
                            dr["Delay Logoff Reason"] = ds.Tables["reason"].Rows[0]["D_Reason"].ToString();
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }

            getDelayHTML(dt);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine(txtDetails.Value);
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine(myBuilder.ToString());
            sb.AppendLine("Please remove BLR-RTM-Server@tangoe.com while replying to this mail. Please send mail to RTM-Support@tangoe.com if you have any issues.");
            sb.AppendLine("");

            MailMessage message1 = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message1.From = new MailAddress("BLR-RTM-Server@tangoe.com");
            foreach (System.Web.UI.WebControls.ListItem item in ddlEmails.Items)
            {
                if (item.Selected)
                {
                    message1.To.Add(new MailAddress(item.Value));
                }
            }
            message1.CC.Add(new MailAddress("Rashmi.Ahuja@tangoe.com"));
            // message1.CC.Add(new MailAddress("RTM-Support@tangoe.com"));

            message1.Subject = "Login & Logoff Status - " + ddlTeam.SelectedItem.Text + " - " + txtDate.Text.Replace("/", "-") + "";
            message1.Body = sb.ToString();

            message1.IsBodyHtml = true;
            smtp.Port = 25;
            smtp.Host = "outlook-south.tangoe.com";
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = false;

            smtp.Send(message1);

            lblMessage.Text = "Mail Sent";
            lblMessage.ForeColor = Color.Blue;

        }
        catch (Exception)
        {
            lblMessage.Text = "Failed to send a mail. Please try again";
            lblMessage.ForeColor = Color.Red;
        }

    }

    private string getDelayHTML(DataTable dt)
    {
        myBuilder = new StringBuilder();

        myBuilder.Append("<table border='1' cellpadding='5' cellspacing='0' ");
        myBuilder.Append("style='border: solid 1px Silver; font-size: x-small;'>");

        myBuilder.Append("<tr align='left' valign='top'>");
        foreach (DataColumn myColumn in dt.Columns)
        {
            myBuilder.Append("<td align='left' valign='top' bgcolor='#FFFF00'>");
            myBuilder.Append("<B />" + myColumn.ColumnName);
            myBuilder.Append("</td>");
        }
        myBuilder.Append("</tr>");

        foreach (DataRow myRow in dt.Rows)
        {
            myBuilder.Append("<tr align='left' valign='top'>");
            foreach (DataColumn myColumn in dt.Columns)
            {
                myBuilder.Append("<td align='left' valign='top'>");
                myBuilder.Append(myRow[myColumn.ColumnName].ToString());
                myBuilder.Append("</td>");
            }
            myBuilder.Append("</tr>");

        }
        myBuilder.Append("</table>");

        return myBuilder.ToString();
    }

    private void ExportData()
    {
        try
        {
            DataRow dr;
            BuildLogTable();
            ds = fecthScheduledTimes(Convert.ToInt32(ddlTeam.SelectedValue));
            for (int i = 0; i <= ds.Tables["times"].Rows.Count - 1; i++)
            {
                string time = ds.Tables["times"].Rows[i]["Scheduled"].ToString();
                ds = fetchUsers(Convert.ToInt32(ddlTeam.SelectedValue), time);
                foreach (DataRow dr1 in ds.Tables["users"].Rows)
                {
                    dr = dt.NewRow();

                    dr["User Name"] = dr1["UL_User_Name"];
                    dr["Employee Id"] = dr1["UL_Employee_Id"];
                    dr["Date"] = txtDate.Text;
                    dr["Sceduled Login"] = dr1["Sch Login"];
                    dr["Scheduled Logoff"] = dr1["Sch Logoff"];
                    ds = fetchActualLogin(dr1["UL_User_Name"].ToString());
                    if (ds.Tables["actual"].Rows.Count > 0)
                    {

                        dr["Actual Login"] = ds.Tables["actual"].Rows[0]["Actual"].ToString();
                    }
                    ds = fetchDelayReason(dr1["UL_User_Name"].ToString(), "In");
                    if (ds.Tables["reason"].Rows.Count > 0)
                    {
                        TimeSpan delay = TimeSpan.Parse(dr["Actual Login"].ToString()).Subtract(TimeSpan.Parse(dr["Sceduled Login"].ToString()));
                        dr["Delayed Login"] = delay;
                        dr["Delay Login Reason"] = ds.Tables["reason"].Rows[0]["D_Reason"].ToString();
                    }

                    ds = fetchActualLogOff(dr1["UL_User_Name"].ToString());
                    if (ds.Tables["actual"].Rows.Count > 0)
                    {
                        dr["Actual Logoff"] = ds.Tables["actual"].Rows[0]["Actual"].ToString();
                    }
                    ds = fetchDelayReason(dr1["UL_User_Name"].ToString(), "Off");
                    if (ds.Tables["reason"].Rows.Count > 0)
                    {
                        TimeSpan delay = TimeSpan.Parse(dr["Actual Logoff"].ToString()).Subtract(TimeSpan.Parse(dr["Scheduled Logoff"].ToString()));
                        dr["Delayed Logoff"] = delay;
                        dr["Delay Logoff Reason"] = ds.Tables["reason"].Rows[0]["D_Reason"].ToString();
                    }
                    dt.Rows.Add(dr);
                }
            }
            StringBuilder sb = new StringBuilder();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=User Login Summary-" + ddlTeam.SelectedItem.Text + ".csv");
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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExportData();
        DisplayData();
    }
}