using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using System.Drawing;
using ClosedXML.Excel;
using System.Configuration;
namespace WorkFlowManager
{
    public partial class ErrorReport : System.Web.UI.Page
    {
        string conString;
        string SqlQuery;
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("DashBoard.aspx");
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
                        Bind();
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

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a valid date');", true);
                }
            }
            catch (Exception ex)
            {
               // ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Please select a valid date in MM/dd/yyyy format');", true);
                throw ex;
            }
        }
        private void Generate()
        {


        }
        public void Bind()
        {
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);
            string ReportModule = ddlReport.SelectedValue;
                 DataTable dt;
            if (ReportModule == "1")
            {
                dt = GetData("select IND_FI, FI_OriginalName,FI_Source,FI_ClientCode,IND_InvoiceNo,COM_Comments as Notes,IND_IP_Assigned_By,IND_IP_Processed_By, IND_QC_Assigned_By,IND_QC_Processed_By,IND_Error,FI_ReceiptDate from dbo.EMSDB_InvDetails join  dbo.EMSDB_FileInfo on FI_ID=IND_FI left join dbo.EMSDB_Comments on COM_FI=IND_FI where FI_Source !='EDI' and IND_Error is not null and  cast(FI_CreatedOn as date ) >='" + From.ToString() + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString() + "'");

            }
            else
            {
                dt = GetData("select IND_FI, FI_OriginalName,FI_Source,FI_ClientCode,IND_InvoiceNo,COM_Comments as Notes,IND_IP_Assigned_By,IND_IP_Processed_By, IND_QC_Assigned_By,IND_QC_Processed_By,IND_Error,FI_ReceiptDate from dbo.EMSDB_InvDetails join  dbo.EMSDB_FileInfo on FI_ID=IND_FI left join dbo.EMSDB_Comments on COM_FI=IND_FI where FI_Source ='EDI' and IND_Error is not null and  cast(FI_CreatedOn as date ) >='" + From.ToString() + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString() + "'");
            }
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('Data not found.');", true);
                GridView1.DataSource = null;
                GridView1.DataBind();
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
        private void Export()
        {
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);
            string ReportModule = ddlReport.SelectedValue;
            DataTable dt;
            if (ReportModule == "1")
            {
                dt = GetData("select IND_FI, FI_OriginalName,FI_Source,FI_ClientCode,IND_InvoiceNo,COM_Comments as Notes,IND_IP_Assigned_By,IND_IP_Processed_By, IND_QC_Assigned_By,IND_QC_Processed_By,IND_Error,FI_ReceiptDate from dbo.EMSDB_InvDetails join  dbo.EMSDB_FileInfo on FI_ID=IND_FI left join dbo.EMSDB_Comments on COM_FI=IND_FI where FI_Source !='EDI' and IND_Error is not null and  cast(FI_CreatedOn as date ) >='" + From.ToString() + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString() + "'");

            }
            else
            {
                dt = GetData("select IND_FI, FI_OriginalName,FI_Source,FI_ClientCode,IND_InvoiceNo,COM_Comments as Notes,IND_IP_Assigned_By,IND_IP_Processed_By, IND_QC_Assigned_By,IND_QC_Processed_By,IND_Error,FI_ReceiptDate from dbo.EMSDB_InvDetails join  dbo.EMSDB_FileInfo on FI_ID=IND_FI left join dbo.EMSDB_Comments on COM_FI=IND_FI where FI_Source ='EDI' and IND_Error is not null and  cast(FI_CreatedOn as date ) >='" + From.ToString() + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString() + "'");
            }

            if (dt.Rows.Count < 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('No records found');", true);
                return;
            }


            dt.TableName = "Invoice Processing";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable as Worksheet.
                wb.Worksheets.Add(dt);

                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ErrorReport.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

         /*   string attachment = "attachment; filename=ErrorReport.xls";
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
                    if (i == 11)
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
          */
          
        }
        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ErrorReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                GridView1.AllowPaging = false;
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView1.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GridView1.HeaderRow.Cells)
                {
                    cell.BackColor = GridView1.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GridView1.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GridView1.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                GridView1.RenderControl(hw);
                //style to format numbers to string
                //string style = @"<style> .textmode { } </style>";
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }

        private void ExportExcel(DataSet data)
        {

            //Set Name of DataTables.
            data.Tables[0].TableName = "Weekly Report";
            data.Tables[1].TableName = "Met GS SLA";
            data.Tables[2].TableName = "Pending";
            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    //Add DataTable as Worksheet.
                    wb.Worksheets.Add(dt);
                }

                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Error Report.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //       server control at run time. */
        //}
        public bool CheckFutureDates(DateTime ThisDate)
        {
            string Date= DateTime.Now.ToString("MMM/dd/yyyy");
            DateTime CurrentDate = Convert.ToDateTime(Date);
            //int result = DateTime.Compare(ThisDate, DateTime.Now);
            int result = DateTime.Compare(ThisDate, CurrentDate);
            if (result <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}