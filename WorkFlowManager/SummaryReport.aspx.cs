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
    public partial class SummaryReport : System.Web.UI.Page
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
            if (Session["access"].ToString() == "4")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('" + Session["username"].ToString() + "  does not have access to summary report.');", true);
                return;
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
                  
                    ReportDocument doc = (ReportDocument)Session["ReportDocument"];
                    CrystalReportViewer1.ReportSource = doc;
                }
            }
            catch (Exception ex)
            {
                Trace.Write("CrystalReportViewer1_DataBinding Exception " + ex.StackTrace);
            }
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    //DataTable dsCustomers = GetData("select Client,Month, Source,TotalInvoicesReceived, (TotalInvoicesReceived - (Invoicesassigned + QCInprogress +InvoiceInProgress+QCCompleted+QCAssigned+Duplicate+Issue)) as UnAssigned,Invoicesassigned  ,QCInprogress,InvoiceInProgress, QCCompleted,QCAssigned,Duplicate,Issue from (select FI_ClientCode as Client, month(FI_CreatedOn )as Month,FI_Source as Source,count (FI_OriginalName) as TotalInvoicesReceived, sum([IP_Asg]) as Invoicesassigned,sum([QC_Inp])as QCInprogress,sum([IP_Inp])InvoiceInProgress, sum([QC_Comp])as QCCompleted,sum([QC_Asg])as QCAssigned,sum([Duplicate])as Duplicate, sum([IP_Issue])as Issue from (select * from ( select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI from dbo.EMSDB_FileInfo left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src pivot (  count(IND_Status) for IND_Status in ([IP_Asg],[QC_Inp],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue])) piv) aaa group by FI_ClientCode, MONTH(FI_CreatedOn),FI_Source)a order by Client");

        //    DataTable dsCustomers = GetData("select Client,convert(varchar, ReceivedDate, 105) as ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Duplicate+Issue+EDI+DNP+QCUnassinged+QCAssigned+QCInprogress+QCCompleted)) as invoiceUnassigned,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) - (QCAssigned+QCInprogress+QCCompleted) as QCUnassigned,QCAssigned,QCInprogress,QCCompleted,Duplicate,Issue,EDI,DNP,Expedite from (select  FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source, " +
        //                "count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp])InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo "+
        //                 " left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite]) ) piv) aaa where cast(FI_CreatedOn as date ) >='05/24/2017'  and cast(FI_CreatedOn as date ) <='05/24/2017'group by FI_ClientCode, cast(FI_CreatedOn as date ) ,FI_Source )a order by Client");
        //    // string strScript;
        //    // Session["rptSrc"] = dsCustomers;
        //    // strScript = "<script language=javascript>window.open('SampleReport.aspx')</script>";
        //    //ClientScript.RegisterClientScriptBlock(this.GetType(), "strScript", strScript);

        //    // CustomerReport.Load(Server.MapPath(Request.ApplicationPath + "/CR_SummaryReport.rpt"));
        //    CustomerReport.Load(Server.MapPath("CR_SummaryReport.rpt"));
        //    CustomerReport.SetDataSource(dsCustomers);
        //    CrystalReportViewer1.ReportSource = CustomerReport;
        //    //CrystalReportViewer1.DataBind();
        //    CrystalReportViewer1.RefreshReport();
        //}
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
            Session["Count"] = "1";
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);
          //  dsCustomers = GetData("select Client,convert(varchar, ReceivedDate, 105) as ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Invoicesassigned + QCInprogress +InvoiceInProgress+QCCompleted+QCAssigned+Duplicate+Issue+EDI+DNP)) as UnAssigned,Invoicesassigned  ,QCUnassinged, QCInprogress,InvoiceInProgress, QCCompleted,QCAssigned,Duplicate,Issue,EDI,DNP,Expedite from (select FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source,count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum ([QC_Idle]) as QCUnassinged, sum([QC_Inp])as QCInprogress,sum([IP_Inp])InvoiceInProgress,sum([QC_Comp])as QCCompleted,sum([QC_Asg])as QCAssigned,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI from dbo.EMSDB_FileInfo left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite]) ) piv) aaa where cast(FI_CreatedOn as date ) >='" + From + "'   and cast(FI_CreatedOn as date ) <='" + To + "' group by FI_ClientCode, cast(FI_CreatedOn as date ),FI_Source )a order by Client");
            //dsCustomers = GetData("select Client,convert(varchar, ReceivedDate, 105) as ReceivedDate, Source,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite)) as invoiceUnassigned,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) - (QCAssigned+QCInprogress+QCCompleted) as QCUnassigned,QCAssigned,QCInprogress,QCCompleted,Duplicate,Issue,EDI,DNP,Expedite from (select  FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source, " +
            //           "count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp])InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo " +
            //            " left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite]) ) piv) aaa where cast(FI_CreatedOn as date ) >='"+From+"'  and cast(FI_CreatedOn as date ) <='"+To+"'group by FI_ClientCode, cast(FI_CreatedOn as date ) ,FI_Source )a order by Client");

            dsCustomers = GetData("select Client,convert(varchar, ReceivedDate, 105) as ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite+Statement)) as invoiceUnassigned,InvoiceInProgress,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) - (QCAssigned+QCInprogress+QCCompleted) as QCUnassigned,QCAssigned,QCInprogress,QCCompleted,Duplicate,Issue,EDI,DNP,Expedite,Statement from (select  FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source,count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp])InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]) ) piv) aaa where cast(FI_CreatedOn as date ) >='" + From + "'  and cast(FI_CreatedOn as date ) <='" + To + "' and FI_Source !='EDI' group by FI_ClientCode, cast(FI_CreatedOn as date ) ,FI_Source )a order by Client");

        
            if (dsCustomers.Rows.Count < 1)
            {
                //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('No Records Found.')", true);
                lblmsg.Visible = true;
                return;
            }
            lblmsg.Visible = false;
            // string strScript;
            Session["ReportDocument"] = dsCustomers;
            // strScript = "<script language=javascript>window.open('SampleReport.aspx')</script>";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "strScript", strScript);

            CustomerReport.Load(Server.MapPath(Request.ApplicationPath + "/CR_SummaryReport.rpt"));
            CustomerReport.SetDatabaseLogon(dbcls.DatabaseUserID(), dbcls.DatabasePwd(), dbcls.DataSource(), dbcls.DatabaseName());
            CustomerReport.SetDataSource(dsCustomers);
            CustomerReport.SetParameterValue("From", From);
            CustomerReport.SetParameterValue("To", To);
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

                        ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Date Entered Exceeds Todays Date.Please select a Valid date.');", true);
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
           // DataTable dt = GetData("select Client,ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Invoicesassigned + QCInprogress +InvoiceInProgress+QCCompleted+QCAssigned+Duplicate+Issue+EDI+DNP)) as UnAssigned,Invoicesassigned  ,QCUnassinged, QCInprogress,InvoiceInProgress, QCCompleted,QCAssigned,Duplicate,Issue,EDI,DNP,Expedite from (select FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source,count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum ([QC_Idle]) as QCUnassinged, sum([QC_Inp])as QCInprogress,sum([IP_Inp])InvoiceInProgress,sum([QC_Comp])as QCCompleted,sum([QC_Asg])as QCAssigned,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI from dbo.EMSDB_FileInfo left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite]) ) piv) aaa where cast(FI_CreatedOn as date ) >='" + From + "'   and cast(FI_CreatedOn as date ) <='" + To + "' group by FI_ClientCode, cast(FI_CreatedOn as date ),FI_Source )a order by Client");
            //DataTable dt = GetData("select Client, ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite)) as invoiceUnassigned,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) - (QCAssigned+QCInprogress+QCCompleted) as QCUnassigned,QCAssigned,QCInprogress,QCCompleted,Duplicate,Issue,EDI,DNP,Expedite from (select  FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source, " +
            //             "count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp])InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo " +
            //              " left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite]) ) piv) aaa where cast(FI_CreatedOn as date ) >='"+From+"'  and cast(FI_CreatedOn as date ) <='"+To+"'group by FI_ClientCode, cast(FI_CreatedOn as date ) ,FI_Source )a order by Client");
            DataTable dt = GetData("select Client,ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite+Statement)) as invoiceUnassigned,Invoicesassigned,InvoiceInProgress,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) - (QCAssigned+QCInprogress+QCCompleted) as QCUnassigned,QCAssigned,QCInprogress,QCCompleted,Duplicate,Issue,EDI,DNP,Expedite,Statement from (select  FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source,count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp])InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]) ) piv) aaa where cast(FI_CreatedOn as date ) >='" + From + "'  and cast(FI_CreatedOn as date ) <='" + To + "' and FI_Source !='EDI' group by FI_ClientCode, cast(FI_CreatedOn as date ) ,FI_Source )a order by Client");
        
            if (dt.Rows.Count < 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('No records found');", true);
                return;
            }

            string attachment = "attachment; filename=EMSDB_SummaryReport.xls";
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
                    if (i == 1)
                    {
                        DateTime Receivedate = Convert.ToDateTime(dr[i]);
                        string date = Receivedate.ToString("MM/dd/yyyy");
                        Response.Write(tab + date);
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
        //protected void Page_Unload(object sender, EventArgs e)
        //{
        //    if (CustomerReport != null)
        //    {
        //        CustomerReport.Close();
        //        CustomerReport.Dispose();
        //    }
        //}


    }
}