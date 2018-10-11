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
using DAL;

namespace WorkFlowManager
{
    public partial class EDISummaryReport : System.Web.UI.Page
    {
        SqlDBHelper dbcls = new SqlDBHelper();

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
        public void Bind()
        {
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);
            DataTable dt = GetData("select Client,convert(varchar, ReceivedDate, 105) as ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite+Statement)) as InvoiceUnassigned,InvoiceInProgress,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) - (QCAssigned+QCInprogress+QCCompleted) as QCUnassigned,QCAssigned,QCInprogress,QCCompleted,Duplicate,Issue from (select  FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source,count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp])InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status   in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],  [Statement]) ) piv) aaa where cast(FI_CreatedOn as date ) >='" + From + "'  and cast(FI_CreatedOn as date ) <= '" + To + "' and FI_Source ='EDI' group by FI_ClientCode, cast(FI_CreatedOn as date ) ,FI_Source )a order by Client");
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {

            Response.Redirect("DashBoard.aspx");
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
            //DataTable dt = GetData("select Client,convert(varchar, ReceivedDate, 103) as ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite+Statement)) as InvoiceUnassigned,InvoiceInProgress,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) - (QCAssigned+QCInprogress+QCCompleted) as QCUnassigned,QCAssigned,QCInprogress,QCCompleted,Duplicate,Issue from (select  FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source,count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp])InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status   in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],  [Statement]) ) piv) aaa where cast(FI_CreatedOn as date ) >='"+From+"'  and cast(FI_CreatedOn as date ) <= '" + To + "' and FI_Source ='EDI' group by FI_ClientCode, cast(FI_CreatedOn as date ) ,FI_Source )a order by Client");
            DataTable dt = GetData("select Client, ReceivedDate, Source,TotalInvoicesReceived,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite+Statement)) as InvoiceUnassigned,InvoiceInProgress,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) - (QCAssigned+QCInprogress+QCCompleted) as QCUnassigned,QCAssigned,QCInprogress,QCCompleted,Duplicate,Issue from (select  FI_ClientCode as Client, cast(FI_CreatedOn as date )as ReceivedDate,FI_Source as Source,count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp])InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_ClientCode,FI_Source,FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status   in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],  [Statement]) ) piv) aaa where cast(FI_CreatedOn as date ) >='" + From + "'  and cast(FI_CreatedOn as date ) <= '" + To + "' and FI_Source ='EDI' group by FI_ClientCode, cast(FI_CreatedOn as date ) ,FI_Source )a order by Client");

            if (dt.Rows.Count < 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('No records found');", true);
                return;
            }

            string attachment = "attachment; filename=EDI_SummaryReport.xls";
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
    }
}