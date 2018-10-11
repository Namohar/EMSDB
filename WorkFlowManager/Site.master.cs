using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls.HtmlGenericControl;

namespace WorkFlowManager
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
                    HttpCookie newSessionIdCookie = Request.Cookies["ASP.NET_SessionId"];
                    if (newSessionIdCookie != null)
                    {
                        string newSessionIdCookieValue = newSessionIdCookie.Value;
                        if (newSessionIdCookieValue != string.Empty)
                        {
                            Response.Redirect("Default.aspx");
                        }
                    }
                }
            }

            if (!IsPostBack)
            {
                string usr = Session["username"].ToString();
                lblUser.Text = Session["username"].ToString().Replace('.', ' ');

                if (Session["EMSTeam"].ToString() == "IP")
                {
                    EMSDB.Visible = true;
                    if (Session["access"].ToString() == "4")
                    {
                        EMSIP.Visible = true;
                        EMSAdmin.Visible = false;
                        EMSReports.Visible = true;
                       // EMSDBSummaryReport.Visible = false;
                        //IP_UserReport.Visible = true;
                        IP_UserReport.Visible = true;
                        IP_DetailReport.Visible = false;
                         EMSDBSummaryReport.Visible = false;
                    }
                    else
                    {
                        EMSIP.Visible = true;
                        EMSAdmin.Visible = true;
                        EMSReports.Visible = true;
                        EMSDBSummaryReport.Visible = true;
                        IP_UserReport.Visible = true;
                        IP_DetailReport.Visible = false;
                        EDI.Visible = true;
                        Li2.Visible = true;
                    }

                }
                if (Session["EMSTeam"].ToString() == "QC")
                {
                    EMSDB.Visible = true;
                    if (Session["access"].ToString() == "4")
                    {
                        EMSQC.Visible = true;
                        EMSAdmin.Visible = false;
                        EMSReports.Visible = false;                
                        EMSDBSummaryReport.Visible = false;
                        IP_UserReport.Visible = false;
                        IP_DetailReport.Visible = false;
                    }
                    else
                    {
                        EMSQC.Visible = true;
                        EMSAdmin.Visible = true;
                        EMSReports.Visible = true;
                        QC_SummaryReport.Visible = true;
                        QCAdminUserReport.Visible = true;
                        IP_DetailReport.Visible = false;

                    }

                }
            }
        }
    }
}