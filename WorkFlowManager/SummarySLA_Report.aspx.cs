using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using ClosedXML.Excel;

namespace WorkFlowManager
{
    public partial class SummarySLA_Report : System.Web.UI.Page
    {
        string conString;
        string SqlQuery;
        DataSet ds = new DataSet();
        DataTable data;
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExport.Enabled = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
          //  Response.Redirect("DashBoard.aspx");
            Response.Redirect("SummarySLA_Report.aspx");
        }

        protected void btnGenerate_Click(object sender, EventArgs e)    
        {
            try
            {
                if (datepickerfrom.Text != "")
                {
                    bool dates = CheckFutureDates(Convert.ToDateTime(datepickerfrom.Text));
                    if (dates != false)
                    {
                       
                       Generate();
                       btnExport.Enabled = true;
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

        private void Generate()
        {
            int number;
            int flag = 0;
            DateTime Recievedate = Convert.ToDateTime(datepickerfrom.Text);
            if (ddlselectDay.SelectedItem.Text == "6+")
            {
                number = 6;
                flag = 1;
            }
            else
            {
                number = Convert.ToInt16(ddlselectDay.SelectedItem.Text);
                flag = 0;
            }
            DateTime Day;
         
            if (Recievedate.DayOfWeek == DayOfWeek.Friday)
            {
                Day = Recievedate.AddDays(number);
            }
            else
            {
                Day = Recievedate.AddDays(number);
            }

         
            //generate table
            if (flag == 0)
            {
                data = GetData("select convert(varchar, FI_CreatedOn , 105) as ReceivedDate ,convert(varchar, IND_IP_ModifiedOn , 105) as ProcessDate,IND_FI, FI_OriginalName,FI_Source,FI_ClientCode,IND_Status,IND_InvoiceNo,IND_IP_Assigned_By,IND_IP_Processed_By from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID  where cast(FI_CreatedOn as date )='" + Recievedate + "' and  cast(IND_IP_ModifiedOn as date)>='" + Day + "'  and cast(IND_IP_ModifiedOn as date)  <='" + Day + "' and FI_Source !='EDI'");
            }
            else
            {
                data = GetData("select convert(varchar, FI_CreatedOn , 105) as ReceivedDate ,convert(varchar, IND_IP_ModifiedOn , 105) as ProcessDate,IND_FI, FI_OriginalName,FI_Source,FI_ClientCode,IND_Status,IND_InvoiceNo,IND_IP_Assigned_By,IND_IP_Processed_By from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID  where cast(FI_CreatedOn as date )='" + Recievedate + "' and  cast(IND_IP_ModifiedOn as date)>='" + Day + "'  and FI_Source !='EDI'");
            }
            if (data.Rows.Count < 1)
            {
                grdSummarySLAReport.DataSource = null;
                grdSummarySLAReport.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('No data found.');", true);
                return;
            }
            grdSummarySLAReport.DataSource = data;
            grdSummarySLAReport.DataBind();
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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {

                if (datepickerfrom.Text != "")
                {
                    bool dates = CheckFutureDates(Convert.ToDateTime(datepickerfrom.Text));
                    if (dates != false)
                    {

                       //
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

                int number;
                int flag = 0;
                DateTime Recievedate = Convert.ToDateTime(datepickerfrom.Text);
                if (ddlselectDay.SelectedItem.Text == "6+")
                {
                    number = 6;
                    flag = 1;
                }
                else
                {
                    number = Convert.ToInt16(ddlselectDay.SelectedItem.Text);
                    flag = 0;
                }
                DateTime Day;

                if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                {
                    Day = Recievedate.AddDays(number);
                }
                else
                {
                    Day = Recievedate.AddDays(number);
                }


                //generate table
                if (flag == 0)
                {
                    data = GetData("select convert(varchar, FI_CreatedOn , 105) as ReceivedDate ,convert(varchar, IND_IP_ModifiedOn , 105) as ProcessDate,IND_FI, FI_OriginalName,FI_Source,FI_ClientCode,IND_Status,IND_InvoiceNo,IND_IP_Assigned_By,IND_IP_Processed_By from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID  where cast(FI_CreatedOn as date )='" + Recievedate + "' and  cast(IND_IP_ModifiedOn as date)>='" + Day + "'  and cast(IND_IP_ModifiedOn as date)  <='" + Day + "' and FI_Source !='EDI'");
                }
                else
                {
                    data = GetData("select convert(varchar, FI_CreatedOn , 105) as ReceivedDate ,convert(varchar, IND_IP_ModifiedOn , 105) as ProcessDate,IND_FI, FI_OriginalName,FI_Source,FI_ClientCode,IND_Status,IND_InvoiceNo,IND_IP_Assigned_By,IND_IP_Processed_By from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID  where cast(FI_CreatedOn as date )='" + Recievedate + "' and  cast(IND_IP_ModifiedOn as date)>='" + Day + "'  and FI_Source !='EDI'");
                }
                if (data.Rows.Count < 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('No data found.');", true);
                    return;
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(data);
                ExportExcel(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExportExcel(DataSet data)
        {

            //Set Name of DataTables.
            data.Tables[0].TableName = "Summary SLA Report";
         
            using (XLWorkbook wb = new XLWorkbook())
            {
                //foreach (DataTable dt in ds.Tables)
                //{
                //    //Add DataTable as Worksheet.
                //    wb.Worksheets.Add(dt);
                //}
                DataTable dt = data.Tables[0];
                wb.Worksheets.Add(dt);
                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Summary SLA Report.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }


        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdSummarySLAReport.PageIndex = e.NewPageIndex;
            //grdSummarySLAReport.DataBind();
            Generate();
        }
    }
}