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
using System.IO;
using System.Drawing;

namespace WorkFlowManager
{
    public partial class DetailReport : System.Web.UI.Page
    {
        SqlDBHelper dbcls = new SqlDBHelper();
        ReportDocument CustomerReport = new ReportDocument();
        DataTable dsCustomers;
        protected void Page_Load(object sender, EventArgs e)
        {
         //try
         //   {
         //       if (IsPostBack)
         //       {
         //           ReportDocument doc = (ReportDocument)Session["IpDetailReportDocument"];
         //           CrystalReportViewer1.ReportSource = doc;
         //       }
         //   }
         //   catch (Exception ex)
         //   {
         //       Trace.Write("CrystalReportViewer1_DataBinding Exception " + ex.StackTrace);
         //  }
        }
        protected void Page_InIt(object sender, EventArgs e)
        {
            //if (Session["IpDetailReportDocument"] != null)
            //{
            //    ReportDocument doc = (ReportDocument)Session["IpDetailReportDocument"];
            //    CrystalReportViewer1.ReportSource = doc;
            //}
        }
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    if (datepickerfrom.Text != "" && datepickerTo.Text != "")
        //    {
        //        PrintReport();
        //    }
        //}
        //protected void CrystalReportViewer1_DataBinding(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (IsPostBack)
        //        {
        //            ReportDocument doc = (ReportDocument)Session["IpDetailReportDocument"];
        //            CrystalReportViewer1.ReportSource = doc;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trace.Write("CrystalReportViewer1_DataBinding Exception " + ex.StackTrace);
        //    }
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
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a date');", true);
            }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a valid date in MM/dd/yyyy format');", true);
                //throw ex;
                CrystalReportViewer1.ReportSource = null;
            
            }
        }
        private void PrintReport()
        {
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);
            string User = Session["username"].ToString();

            if (Session["access"].ToString() == "4")
            {
                //dsCustomers = GetData("SELECT convert(varchar, FI_ReceiptDate, 105) as FI_ReceiptDate,  IND_IP_Processed_By as ProcessedBy, CAST(Log_NewValue AS VARCHAR(40)) as InvoiceNumber,CAST(DateDiff(MI, Invoice_processing_start_time, process_end_time)/60 AS varchar)+':'+Cast(DateDiff(MI, Invoice_processing_start_time, process_end_time)%60 AS varchar)+':'+cast(DateDiff(s, Invoice_processing_start_time, process_end_time)%60 AS varchar) as TotalTime " +
                //                "FROM (SELECT  * FROM (SELECT FI_ReceiptDate, IND_IP_Processed_By,Log_ModifiedOn as 'Invoice_processing_start_time',Log_NewValue , Log_OldValue,IND_ID FROM  (SELECT FI_ReceiptDate, IND_IP_Processed_By, IND_ID FROM (SELECT FI_ReceiptDate,FI_ID FROM dbo.EMSDB_FileInfo " +
                //                "where  cast(FI_ReceiptDate as date ) >='" + From + "'   and cast(FI_ReceiptDate as date ) <='" + To + "')a Left Join EMSDB_InvDetails ON IND_FI=FI_ID where  IND_IP_Processed_By='" + User + "'  )b left join dbo.EMSDB_Log ON Log_FileId=IND_ID WHERE Log_Field='InvoiceNo')c LEFT JOIN (SELECT   Log_FileId, max(Log_ModifiedOn) AS 'process_end_time' FROM dbo.EMSDB_Log " +
                //                "WHERE Log_NewValue = 'QC_Idle' GROUP BY Log_FileId ) d ON IND_ID=Log_FileId)aaa ");
                dsCustomers = GetData(" SELECT convert(varchar, FI_ReceiptDate, 105) as FI_ReceiptDate, FI_ClientCode, IND_IP_Processed_By, Log_NewValue, CAST(DateDiff(MI, Invoice_processing_start_time, process_end_time)/60 AS varchar)  +':'+Cast(DateDiff(MI, Invoice_processing_start_time, process_end_time)%60 AS varchar) +':'+cast(DateDiff(s, Invoice_processing_start_time, process_end_time)%60 AS varchar)as TotalTime FROM (SELECT  * FROM (SELECT FI_ReceiptDate, IND_IP_Processed_By,Log_ModifiedOn as 'Invoice_processing_start_time',Log_NewValue , Log_OldValue,IND_ID,IND_FI,FI_ClientCode FROM  (SELECT FI_ReceiptDate, IND_IP_Processed_By, IND_ID,IND_FI,FI_ClientCode FROM (SELECT   FI_ReceiptDate,FI_ID,FI_ClientCode FROM dbo.EMSDB_FileInfo where (FI_ReceiptDate >= '" + From + "' and FI_ReceiptDate <= '" + To + "' ) )a Left Join EMSDB_InvDetails ON IND_FI=FI_ID  )b left join dbo.EMSDB_Log ON Log_FileId=IND_FI Where Log_ModifiedOn is not null and IND_IP_Processed_By ='" + User + "'" +
                                      "and Log_Field='InvoiceNo')c LEFT JOIN  (SELECT   Log_FileId, max(Log_ModifiedOn) AS 'process_end_time' FROM dbo.EMSDB_Log WHERE (Log_NewValue = 'QC_Idle' or Log_NewValue='IP_Issue') GROUP BY Log_FileId ) d ON IND_FI=Log_FileId)aaa ");
            }
            else
            {
                dsCustomers = GetData(" SELECT convert(varchar, FI_ReceiptDate, 105) as FI_ReceiptDate, FI_ClientCode, IND_IP_Processed_By, Log_NewValue, CAST(DateDiff(MI, Invoice_processing_start_time, process_end_time)/60 AS varchar)  +':'+Cast(DateDiff(MI, Invoice_processing_start_time, process_end_time)%60 AS varchar) +':'+cast(DateDiff(s, Invoice_processing_start_time, process_end_time)%60 AS varchar)as TotalTime FROM (SELECT  * FROM (SELECT FI_ReceiptDate, IND_IP_Processed_By,Log_ModifiedOn as 'Invoice_processing_start_time',Log_NewValue , Log_OldValue,IND_ID,IND_FI,FI_ClientCode FROM  (SELECT FI_ReceiptDate, IND_IP_Processed_By, IND_ID,IND_FI,FI_ClientCode FROM (SELECT   FI_ReceiptDate,FI_ID,FI_ClientCode FROM dbo.EMSDB_FileInfo where (FI_ReceiptDate >= '" + From + "' and FI_ReceiptDate <= '" + To + "' ) )a Left Join EMSDB_InvDetails ON IND_FI=FI_ID  )b left join dbo.EMSDB_Log ON Log_FileId=IND_FI Where Log_ModifiedOn is not null " +
                                            "and Log_Field='InvoiceNo')c LEFT JOIN  (SELECT   Log_FileId, max(Log_ModifiedOn) AS 'process_end_time' FROM dbo.EMSDB_Log WHERE (Log_NewValue = 'QC_Idle' or Log_NewValue='IP_Issue') GROUP BY Log_FileId ) d ON IND_FI=Log_FileId)aaa ");
  
            }

            if (dsCustomers.Rows.Count < 1)
            {

                lblmsg.Visible = true;
                return;
            }
            lblmsg.Visible = false;
            // string strScript;
            Session["IpDetailReportDocument"] = dsCustomers;
            if (Session["access"].ToString() == "4")
            {

                CustomerReport.Load(Server.MapPath(Request.ApplicationPath + "/CR_DetailedUserReport.rpt"));
                CustomerReport.SetDatabaseLogon(dbcls.DatabaseUserID(), dbcls.DatabasePwd(), dbcls.DataSource(), dbcls.DatabaseName());
                CustomerReport.SetDataSource(dsCustomers);
                CustomerReport.SetParameterValue("From", From);
                CustomerReport.SetParameterValue("To", To);
                CustomerReport.SetParameterValue("User", User);
            }
            else
            {

                CustomerReport.Load(Server.MapPath(Request.ApplicationPath + "/Test.rpt"));
                CustomerReport.SetDatabaseLogon(dbcls.DatabaseUserID(), dbcls.DatabasePwd(), dbcls.DataSource(), dbcls.DatabaseName());
                CustomerReport.SetDataSource(dsCustomers);
                
                CustomerReport.SetParameterValue("From", From);
                CustomerReport.SetParameterValue("To", To);
            }
            //CrystalReportViewer1.ParameterFieldInfo.Clear();
            CrystalReportViewer1.ReportSource = CustomerReport;
    
            //CrystalReportViewer1.RefreshReport();


        }

        private DataTable Data()
        {
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);
            DataTable dt = new DataTable();

            string User = Session["username"].ToString();

            if (Session["access"].ToString() == "4")
            {
                dt = GetData(" SELECT FI_ReceiptDate, FI_ClientCode, IND_IP_Processed_By, Log_NewValue, CAST(DateDiff(MI, Invoice_processing_start_time, process_end_time)/60 AS varchar)  +':'+Cast(DateDiff(MI, Invoice_processing_start_time, process_end_time)%60 AS varchar) +':'+cast(DateDiff(s, Invoice_processing_start_time, process_end_time)%60 AS varchar)as TotalTime FROM (SELECT  * FROM (SELECT FI_ReceiptDate, IND_IP_Processed_By,Log_ModifiedOn as 'Invoice_processing_start_time',Log_NewValue , Log_OldValue,IND_ID,IND_FI,FI_ClientCode FROM  (SELECT FI_ReceiptDate, IND_IP_Processed_By, IND_ID,IND_FI,FI_ClientCode FROM (SELECT   FI_ReceiptDate,FI_ID,FI_ClientCode FROM dbo.EMSDB_FileInfo where (FI_ReceiptDate >= '" + From + "' and FI_ReceiptDate <= '" + To + "' ) )a Left Join EMSDB_InvDetails ON IND_FI=FI_ID  )b left join dbo.EMSDB_Log ON Log_FileId=IND_FI Where Log_ModifiedOn is not null and IND_IP_Processed_By ='" + User + "' " +
                                       " and Log_Field='InvoiceNo')c LEFT JOIN  (SELECT   Log_FileId, max(Log_ModifiedOn) AS 'process_end_time' FROM dbo.EMSDB_Log WHERE (Log_NewValue = 'QC_Idle' or Log_NewValue='IP_Issue') GROUP BY Log_FileId ) d ON IND_FI=Log_FileId)aaa ");
            }
            else
            {
                dt = GetData(" SELECT FI_ReceiptDate, FI_ClientCode, IND_IP_Processed_By, Log_NewValue, CAST(DateDiff(MI, Invoice_processing_start_time, process_end_time)/60 AS varchar)  +':'+Cast(DateDiff(MI, Invoice_processing_start_time, process_end_time)%60 AS varchar) +':'+cast(DateDiff(s, Invoice_processing_start_time, process_end_time)%60 AS varchar)as TotalTime FROM (SELECT  * FROM (SELECT FI_ReceiptDate, IND_IP_Processed_By,Log_ModifiedOn as 'Invoice_processing_start_time',Log_NewValue , Log_OldValue,IND_ID,IND_FI,FI_ClientCode FROM  (SELECT FI_ReceiptDate, IND_IP_Processed_By, IND_ID,IND_FI,FI_ClientCode FROM (SELECT   FI_ReceiptDate,FI_ID,FI_ClientCode FROM dbo.EMSDB_FileInfo where (FI_ReceiptDate >= '" + From + "' and FI_ReceiptDate <= '" + To + "' ) )a Left Join EMSDB_InvDetails ON IND_FI=FI_ID  )b left join dbo.EMSDB_Log ON Log_FileId=IND_FI Where Log_ModifiedOn is not null " +
                                     " and Log_Field='InvoiceNo')c LEFT JOIN  (SELECT   Log_FileId, max(Log_ModifiedOn) AS 'process_end_time' FROM dbo.EMSDB_Log WHERE (Log_NewValue = 'QC_Idle' or Log_NewValue='IP_Issue') GROUP BY Log_FileId ) d ON IND_FI=Log_FileId)aaa ");
            }
         

            return dt;

        }
        private void Export()
        {

            DataTable dt = Data();
          
            string FileName = "EMSDB_DetailReport.xls";
            Response.Clear();
            Response.Buffer = true;
            // Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                GrdDetails.AllowPaging = false;
                if (dt.Rows.Count < 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('No records found');", true);
                    return;
                }
                else
                {
                    GrdDetails.DataSource = dt;
                    GrdDetails.DataBind();
                }

                GrdDetails.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GrdDetails.HeaderRow.Cells)
                {
                    cell.BackColor = GrdDetails.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GrdDetails.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GrdDetails.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GrdDetails.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                GrdDetails.RenderControl(hw);

                //style to format numbers to string
               // string style = @"<style> .textmode { } </style>";
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
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
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a date');", true);
                return;
            }

            }
             catch (Exception ex)
             {
                 ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a valid date in MM/dd/yyyy format');", true);
                 //throw ex;
             }
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