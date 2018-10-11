using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using DAL;
using System.Globalization;
namespace WorkFlowManager
{
    public partial class EMSDB_Reports1 : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        clsEMSDB objEMS = new clsEMSDB();
        protected void Page_Load(object sender, EventArgs e)
        {
            dt = objEMS.GetCompletedRecords(1);
            grdReports.DataSource = dt;
            grdReports.DataBind();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + grdReports.ClientID + "', 450, 950 , 40 ,true); </script>", false);
        }
    }
}