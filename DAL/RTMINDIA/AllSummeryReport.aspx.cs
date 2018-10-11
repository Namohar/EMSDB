using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class AllSummeryReport : System.Web.UI.Page
{
    ReportDocument rptDoc = new ReportDocument();

    protected void Page_Init(object sender, EventArgs e)
    {
        printReport();
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void printReport()
    {
        rptDoc.Load(Server.MapPath("crAllSummery.rpt"));
        rptDoc.SetDataSource(Session["rptSrc"]);
        CrystalReportViewer1.ReportSource = rptDoc;
        CrystalReportViewer1.RefreshReport();
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        if (rptDoc != null)
        {
            rptDoc.Close();
            rptDoc.Dispose();
        }
    }
}