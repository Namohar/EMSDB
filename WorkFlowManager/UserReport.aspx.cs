using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using DAL;
using System.Data;
using System.Configuration;
using CrystalDecisions.Shared;
using CrystalDecisions.Reporting.WebControls;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Reporting;
using System.Globalization;
namespace WorkFlowManager
{
    public partial class UserReport : System.Web.UI.Page
    {
        SqlDBHelper dbcls = new SqlDBHelper();
        ReportDocument CustomerReport = new ReportDocument();
        DataTable dsCustomers;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Default.aspx");
            }
        }
        protected void Page_InIt(object sender, EventArgs e)
        {
            //  PrintReport();
        }
        protected void CrystalReportViewer1_DataBinding(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    ReportDocument doc = (ReportDocument)Session["IpUserReportDocument"];
                    CrystalReportViewer1.ReportSource = doc;
                }
            }
            catch (Exception ex)
            {
                Trace.Write("CrystalReportViewer1_DataBinding Exception " + ex.StackTrace);
            }
        }
        public bool CheckFutureDates(DateTime ThisDate)
        {
            int result = DateTime.Compare(ThisDate, DateTime.Now);

            if (result <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (datepickerfrom.Text != "" && datepickerTo.Text != "")
                {
                    bool TodateCompare = Convert.ToDateTime(datepickerTo.Text) >= Convert.ToDateTime(datepickerfrom.Text);
                    if (TodateCompare == false)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('TO date should be greater than FROM date');", true);
                        return;
                    }
                    bool dates = CheckFutureDates(Convert.ToDateTime(datepickerTo.Text));
                    if (dates != false)
                    {
                        PrintReport();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Date Entered Exceeds Todays Date.Please select a valid date.');", true);

                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a valid date');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a valid date in MM/dd/yyyy format');", true);
                //throw ex;
            }
        }
        private void PrintReport()
        {
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);
            string User = Session["username"].ToString();

            if (Session["access"].ToString() == "4")
            {
                //dsCustomers = GetData("SELECT FI_ClientCode as Client,convert(varchar, ReceivedDate, 105) as ReceivedDate,UserName,count (FI_OriginalName) as TotalInvoicesProcessed,count(IND_Error) as Error,SUM([Duplicate]) as Duplicate, SUM([IP_Issue]) as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite " +
                //        "FROM (select FI_ClientCode,IND_IP_Processed_By as UserName,cast(FI_CreatedOn as date ) as ReceivedDate,FI_OriginalName,IND_Error,IND_Status from dbo.EMSDB_FileInfo left join dbo.EMSDB_InvDetails " +
                //        "on IND_FI=FI_ID   where cast(FI_CreatedOn as date ) >='" + From + "'   and cast(FI_CreatedOn as date ) <='" + To + "'  and IND_IP_Processed_By !='null'and IND_IP_Processed_By ='" + User + "') as s PIVOT (count(IND_Status)  for IND_Status in ([Duplicate],[IP_Issue],[EDI],[DNP],[Expedite]) )AS pvt " +
                //        "group by ReceivedDate,FI_ClientCode,UserName");


                dsCustomers = GetData("SELECT Client,RecieveDate,ProcessedDate,UserName,AssignedInvoices,ProcessedInvoice,CompletedInvoice,Duplicate,Issue,EDI,DNP,Expedite,Statement,Error,(AssignedInvoices-CompletedInvoice) as pending,case when Error<>0 then (CAST(ProcessedInvoice as float)/CAST(Error as float)*100.0) else 100 END as Accuracy FROM(SELECT FI_ClientCode as Client,convert(varchar, RecieveDate, 105) as RecieveDate,convert(varchar, ProcessedDate, 105) as ProcessedDate,UserName, count (IND_FI) as AssignedInvoices,sum([QC_Inp]+[QC_Idle]+[QC_Comp]+[QC_Asg]+[Duplicate]+[IP_Issue]+[EDI]+[DNP]+[Expedite])CompletedInvoice,SUM([QC_Inp]+[QC_Idle]+[QC_Comp]+[QC_Asg]) as ProcessedInvoice, SUM([Duplicate]) as Duplicate, SUM([IP_Issue]) as Issue, SUM([EDI]) as EDI, " +
                                 " SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,sum([Statement]) as Statement, count(IND_Error) as Error   FROM ( select IND_FI, FI_ClientCode,IND_IP_Processed_By as UserName,cast(FI_CreatedOn as date ) as RecieveDate,  cast(FI_CreatedOn as date ) as ProcessedDate,  FI_OriginalName,IND_Status,IND_Error   from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID   where cast(FI_CreatedOn as date ) >='" + From + "'  and cast(FI_CreatedOn as date ) <='" + To + "' and FI_Source not in ('EDI') and IND_IP_Processed_By !='null' and " +
                                     " IND_IP_Processed_By ='" + User + "') as s PIVOT (count(IND_Status)  for IND_Status in ([IP_Inp],[QC_Inp],[QC_Idle],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]))AS pvt   group by ProcessedDate,RecieveDate,FI_ClientCode,UserName)a");


            }
            else
            {
                dsCustomers = GetData("SELECT Client,RecieveDate,ProcessedDate,UserName,AssignedInvoices,ProcessedInvoice,CompletedInvoice,Duplicate,Issue,EDI,DNP,Expedite,Statement,Error,(AssignedInvoices-CompletedInvoice) as pending,case when Error<>0 then (CAST(ProcessedInvoice as float)/CAST(Error as float)*100.0) else 100 END as Accuracy FROM(SELECT FI_ClientCode as Client,convert(varchar, RecieveDate, 105) as RecieveDate,convert(varchar, ProcessedDate, 105) as ProcessedDate,UserName, count (IND_FI) as AssignedInvoices,sum([QC_Inp]+[QC_Idle]+[QC_Comp]+[QC_Asg]+[Duplicate]+[IP_Issue]+[EDI]+[DNP]+[Expedite])CompletedInvoice,SUM([QC_Inp]+[QC_Idle]+[QC_Comp]+[QC_Asg]) as ProcessedInvoice, SUM([Duplicate]) as Duplicate, SUM([IP_Issue]) as Issue, SUM([EDI]) as EDI, " +
                  " SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,sum([Statement]) as Statement, count(IND_Error) as Error   FROM ( select IND_FI, FI_ClientCode,IND_IP_Processed_By as UserName,cast(FI_CreatedOn as date ) as RecieveDate,  cast(FI_CreatedOn as date ) as ProcessedDate,  FI_OriginalName,IND_Status,IND_Error   from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID   where cast(FI_CreatedOn as date ) >='" + From + "'  and cast(FI_CreatedOn as date ) <='" + To + "' and FI_Source not in ('EDI') and IND_IP_Processed_By !='null' " +
                      ") as s PIVOT (count(IND_Status)  for IND_Status in ([IP_Inp],[QC_Inp],[QC_Idle],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]))AS pvt   group by ProcessedDate,RecieveDate,FI_ClientCode,UserName)a");
            }

            if (dsCustomers.Rows.Count < 1)
            {

                lblmsg.Visible = true;
                return;
            }
            lblmsg.Visible = false;
            // string strScript;
            Session["IpUserReportDocument"] = dsCustomers;


            //CustomerReport.SetParameterValue("From", From);
            //CustomerReport.SetParameterValue("To", To);

            if (Session["access"].ToString() == "4")
            {

                CustomerReport.Load(Server.MapPath(Request.ApplicationPath + "/CR_InvoiceUserReport.rpt"));

                CustomerReport.SetDatabaseLogon(dbcls.DatabaseUserID(), dbcls.DatabasePwd(), dbcls.DataSource(), dbcls.DatabaseName());
                CustomerReport.SetDataSource(dsCustomers);
                CustomerReport.SetParameterValue("From", From.ToString("yyyy-MM-dd"));
                CustomerReport.SetParameterValue("To", To.ToString("yyyy-MM-dd"));
                CustomerReport.SetParameterValue("User", User);
            }
            else
            {

                CustomerReport.Load(Server.MapPath(Request.ApplicationPath + "/CR_InvoiceAdminReport.rpt"));
                CustomerReport.SetDatabaseLogon(dbcls.DatabaseUserID(), dbcls.DatabasePwd(), dbcls.DataSource(), dbcls.DatabaseName());
                CustomerReport.SetDataSource(dsCustomers);
                CustomerReport.SetParameterValue("From", From.ToString("yyyy-MM-dd"));
                CustomerReport.SetParameterValue("To", To.ToString("yyyy-MM-dd"));
            }

            CrystalReportViewer1.ReportSource = CustomerReport;
            //CrystalReportViewer1.DataBind();
            CrystalReportViewer1.RefreshReport();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

            Response.Redirect("DashBoard.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (datepickerfrom.Text != "" && datepickerTo.Text != "")
                {
                    bool TodateCompare = Convert.ToDateTime(datepickerTo.Text) >= Convert.ToDateTime(datepickerfrom.Text);
                    if (TodateCompare == false)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('TO date should be greater than FROM date');", true);
                        return;
                    }
                    bool dates = CheckFutureDates(Convert.ToDateTime(datepickerTo.Text));
                    if (dates != false)
                    {
                        Export();
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Date Entered Exceeds Todays Date.Please select a valid date.');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a valid date');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a valid date in MM/dd/yyyy format');", true);
                //throw ex;
            }

        }
            
        private void Export()
        {
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);
            DataTable dt = new DataTable();

            string User = Session["username"].ToString();

            if (Session["access"].ToString() == "4")
            {
                dt = GetData("SELECT Client,RecieveDate,ProcessedDate,UserName,AssignedInvoices,ProcessedInvoice,CompletedInvoice,Duplicate,Issue,EDI,DNP,Expedite,Statement,(AssignedInvoices-CompletedInvoice) as pending,Error,case when Error<>0 then (CAST(ProcessedInvoice as float)/CAST(Error as float)*100.0) else 100 END as Accuracy FROM(SELECT FI_ClientCode as Client, RecieveDate as RecieveDate,ProcessedDate as ProcessedDate,UserName, count (IND_FI) as AssignedInvoices,sum([QC_Inp]+[QC_Idle]+[QC_Comp]+[QC_Asg]+[Duplicate]+[IP_Issue]+[EDI]+[DNP]+[Expedite]+[Statement])CompletedInvoice,SUM([QC_Inp]+[QC_Idle]+[QC_Comp]+[QC_Asg]) as ProcessedInvoice, SUM([Duplicate]) as Duplicate, SUM([IP_Issue]) as Issue, SUM([EDI]) as EDI, " +
                                " SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,sum([Statement]) as Statement, count(IND_Error) as Error   FROM ( select IND_FI,FI_ClientCode,IND_IP_Processed_By as UserName,cast(FI_CreatedOn as date ) as RecieveDate,  cast(FI_CreatedOn as date ) as ProcessedDate,  FI_OriginalName,IND_Status,IND_Error   from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID   where cast(FI_CreatedOn as date ) >='" + From + "'  and cast(FI_CreatedOn as date ) <='" + To + "' and FI_Source not in ('EDI') and IND_IP_Processed_By !='null' and " +
                                    " IND_IP_Processed_By ='" + User + "') as s PIVOT (count(IND_Status)  for IND_Status in ([IP_Inp],[QC_Inp],[QC_Idle],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]))AS pvt   group by ProcessedDate,RecieveDate,FI_ClientCode,UserName)a");

            }
            else
            {
                dt = GetData("SELECT Client, RecieveDate,ProcessedDate,UserName,AssignedInvoices,ProcessedInvoice,CompletedInvoice,Duplicate,Issue,EDI,DNP,Expedite,Statement,(AssignedInvoices-CompletedInvoice) as pending,Error,case when Error<>0 then (CAST(ProcessedInvoice as float)/CAST(Error as float)*100.0) else 100 END as Accuracy FROM(SELECT FI_ClientCode as Client,RecieveDate as RecieveDate,ProcessedDate as ProcessedDate,UserName, count (IND_FI) as AssignedInvoices,sum([QC_Inp]+[QC_Idle]+[QC_Comp]+[QC_Asg]+[Duplicate]+[IP_Issue]+[EDI]+[DNP]+[Expedite]+[Statement])CompletedInvoice,SUM([QC_Inp]+[QC_Idle]+[QC_Comp]+[QC_Asg]) as ProcessedInvoice, SUM([Duplicate]) as Duplicate, SUM([IP_Issue]) as Issue, SUM([EDI]) as EDI, " +
                               " SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,sum([Statement]) as Statement, count(IND_Error) as Error   FROM ( select IND_FI, FI_ClientCode,IND_IP_Processed_By as UserName,cast(FI_CreatedOn as date ) as RecieveDate,  cast(FI_CreatedOn as date ) as ProcessedDate,  FI_OriginalName,IND_Status,IND_Error   from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID   where cast(FI_CreatedOn as date ) >='" + From + "'  and cast(FI_CreatedOn as date ) <='" + To + "' and FI_Source not in ('EDI') and IND_IP_Processed_By !='null' " +
                                   " ) as s PIVOT (count(IND_Status)  for IND_Status in ([IP_Inp],[QC_Inp],[QC_Idle],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]))AS pvt   group by ProcessedDate,RecieveDate,FI_ClientCode,UserName)a");
            }
            if (dt.Rows.Count < 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('No records found');", true);
                return;
            }

            string attachment = "attachment; filename=EMSDB_UserReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == 1 || i == 2)
                    {
                        DateTime Receivedate = Convert.ToDateTime(dr[i]);
                        string date = Receivedate.ToString("MM/dd/yyyy");
                        Response.Write(tab + date);
                    }
                    else if (i == 15)
                    {
                        Response.Write(tab + dr[i].ToString() + "%");
                    }
                    else
                    {
                        Response.Write(tab + dr[i].ToString());
                        // tab = "\t";
                    }
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }
        private DataTable GetData(string query)
        {
            // string conString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string conString = ConfigurationManager.AppSettings["conString"];

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }
}