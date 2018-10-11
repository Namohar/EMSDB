using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace WorkFlowManager
{
    public partial class PendingInvoices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime From;
            DateTime To;
            From = Convert.ToDateTime(DateTime.Today.AddDays(-14));
            To = Convert.ToDateTime(DateTime.Now);
            DataTable PendingReport = GetData("select * from (select FI_ClientCode as Client,convert(varchar, ReceivedDate, 120) as 'Date Added',Issue+Invoicesassigned+InvoiceInProgress+invoiceUnassigned as Incomplete from (select ReceivedDate,FI_ClientCode, TotalInvoicesReceived,Invoicesassigned,InvoiceInProgress,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite+Statement)) as invoiceUnassigned,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,Duplicate,Issue,EDI,DNP,Expedite  from (select cast(FI_CreatedOn as date )as ReceivedDate,FI_ClientCode,count (FI_OriginalName) as TotalInvoicesReceived, sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp]) as InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged, sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted, sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP,  SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_OriginalName, FI_CreatedOn ,FI_ClientCode, IND_Status,IND_FI  from dbo.EMSDB_FileInfo   left join  dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status  in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP], [Expedite],[Statement]) ) piv) aaa   where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' group by cast(FI_CreatedOn as date ),FI_ClientCode)a)b)c where Incomplete!=0 ");
            grdPendingReport.DataSource = PendingReport;
            grdPendingReport.DataBind();
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