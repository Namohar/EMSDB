using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;
using System.Drawing;
using ClosedXML.Excel;
namespace WorkFlowManager
{
    public partial class EDI_WeeklyReport : System.Web.UI.Page
    { //  SqlDBHelper dbcls = new SqlDBHelper();
        string conString;
        string SqlQuery;
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnExport.Enabled = false;
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
                        LblPending.Visible = true;
                        lblSLA.Visible = true;
                        subgrid.Visible = true;
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
                        //generate table
                        DataTable data = GetData("SELECT RECEIVEDATE as DateAdded,Day1Count as Day1,Day2Count as Day2,Day3Count as Day3,Day4Count as Day4,Day5Count as Day5,Day6Count as Day6,TotalInvoice,Convert(varchar(10),day1Percentage)+'%' AS 'Day 1 ',Convert(varchar(10),day2Percentage)+'%' AS 'Day 2',Convert(varchar(10),day3Percentage)+'%' AS 'Day3 ',Convert(varchar(10),day4Percentage)+'%' AS 'Day 4 ',Convert(varchar(10),day5Percentage)+'%' AS 'Day 5 ', Convert(varchar(10),day6Percentage)+'%' AS 'Day 6 ' FROM REPORT");
                        //grdReport.DataSource = data;
                        //grdReport.DataBind();


                        //generate SLA table
                        DataTable SLA = GetData("select RECEIVEDATE,TotalInvoice,ProcessedInvoices,Convert(varchar(10),(CAST(round(Percentage,2) as decimal(10,2))))+'%' as Percentage from (select RECEIVEDATE,TotalInvoice,sum(Day1Count+Day2Count+Day3Count+Day4Count+Day5Count) as ProcessedInvoices,case when sum(Day1Count+Day2Count+Day3Count+Day4Count+Day5Count)<>0 then (CAST(sum(Day1Count+Day2Count+Day3Count+Day4Count+Day5Count) as float)/CAST(TotalInvoice as float)*100.0) else 100 END as Percentage from dbo.REPORT  group by RECEIVEDATE,TotalInvoice ) as a order by RECEIVEDATE");
                        //grdSLA.DataSource = SLA;
                        //grdSLA.DataBind();

                        DataTable pendingReport = GetData("select RECEIVEDATE,Pending from REPORT");
                        //grdPending.DataSource = pendingReport;
                        //grdPending.DataBind();

                        ds.Tables.Add(data);
                        ds.Tables.Add(SLA);
                        ds.Tables.Add(pendingReport);
                        ExportExcel(ds);
                        //DataTable data = GetData("select * from REPORT");
                        //grdReport.DataSource = data;
                        //grdReport.DataBind();
                        //ExportGridToExcel();
       
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
            int Issue = 0;
            //delete from table.
            SqlQuery = "delete from REPORT";
            conString = ConfigurationManager.AppSettings["conString"];
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(SqlQuery, con);
                cmd.ExecuteNonQuery();
            }
            DateTime From = Convert.ToDateTime(datepickerfrom.Text);
            DateTime To = Convert.ToDateTime(datepickerTo.Text);


            // DataTable pending = GetData("select ReceivedDate, Issue+Invoicesassigned+InvoiceInProgress+invoiceUnassigned as PendingTotal, Invoicesassigned+InvoiceInProgress+invoiceUnassigned as pending,Issue as Issue  from(select ReceivedDate,TotalInvoicesReceived,Invoicesassigned,InvoiceInProgress,(TotalInvoicesReceived - (Invoicesassigned+InvoiceInProgress+QCUnassinged+QCAssigned+QCInprogress+QCCompleted+Duplicate+Issue+EDI+DNP+Expedite+Statement)) as invoiceUnassigned,(QCUnassinged+QCAssigned+QCInprogress+QCCompleted) as InvoiceCompleted,Duplicate,Issue,EDI,DNP,Expedite from (select cast(FI_CreatedOn as date )as ReceivedDate,count (FI_OriginalName) as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp]) as InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI  from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID ) src   pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]) ) piv) aaa   where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and    cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' group by cast(FI_CreatedOn as date ))a)b ");
               // DataTable pending = GetData("select ReceivedDate,Issue+Invoicesassigned+InvoiceInProgress as PendingTotal, Invoicesassigned+InvoiceInProgress as pending,Issue as Issue from (select cast(FI_CreatedOn as date )as ReceivedDate,sum(cast(IND_InvoiceNo as int))  as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp]) as InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI,IND_InvoiceNo  from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID where FI_Source in ('EDI')) src pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]) ) piv) aaa  where cast(FI_CreatedOn as date ) >='" + To.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "'  group by cast(FI_CreatedOn as date ))B");
            DataTable pending = GetData("select ReceivedDate,Issue+Invoicesassigned+InvoiceInProgress as PendingTotal, Invoicesassigned+InvoiceInProgress as pending,Issue as Issue from (select cast(FI_CreatedOn as date )as ReceivedDate,sum(cast(IND_InvoiceNo as int))  as TotalInvoicesReceived,sum([IP_Asg]) as Invoicesassigned,sum([IP_Inp]) as InvoiceInProgress,sum ([QC_Idle]) as QCUnassinged,sum([QC_Asg])as QCAssigned,sum([QC_Inp])as QCInprogress,sum([QC_Comp])as QCCompleted,sum([Duplicate])as Duplicate,sum([IP_Issue])as Issue, SUM([EDI]) as EDI, SUM([DNP]) as DNP, SUM([Expedite]) as Expedite,SUM([Statement]) as Statement from (select * from (select FI_OriginalName,FI_CreatedOn ,IND_Status,IND_FI,IND_InvoiceNo  from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID where FI_Source in ('EDI')) src pivot (count(IND_Status)  for IND_Status in ([IP_Asg],[QC_Inp],[QC_Idle],[IP_Inp],[QC_Comp],[QC_Asg],[Duplicate],[IP_Issue],[EDI],[DNP],[Expedite],[Statement]) ) piv) aaa  where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "'  group by cast(FI_CreatedOn as date ))B");
            DataTable dt = new DataTable();


            //dt = GetData(";with CTE_Input1 as ( select RecieveDate,cast(IND_IP_ModifiedOn as date) as ProcessDate, count(FI_OriginalName) as InvoiceCount from  (select cast(FI_CreatedOn as date ) as RecieveDate, IND_IP_ModifiedOn,FI_OriginalName from dbo.EMSDB_FileInfo  left join dbo.EMSDB_InvDetails on IND_FI=FI_ID   where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' and IND_IP_Processed_By !='null' and IND_Status not in ('IP_Asg','IP_Inp','IP_Issue')) A group by cast(IND_IP_ModifiedOn as date),RecieveDate),CTE_Input2 as (select cast(FI_CreatedOn as date )as RecieveDate, count(FI_OriginalName) as TotalInvoices from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID  where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' group by cast(FI_CreatedOn as date )),CTE_Input3 as(SELECT RecieveDate,ProcessDate,TotalInvoices,InvoiceCount,Cast(round(Accuracy,2) as decimal(10,2)) as Day1 FROM (select CTE_Input1.RecieveDate as RecieveDate,CTE_Input1.ProcessDate as ProcessDate,CTE_Input1.InvoiceCount as InvoiceCount,CTE_Input2.TotalInvoices as TotalInvoices, case when CTE_Input1.InvoiceCount<>0 then (CAST(CTE_Input1.InvoiceCount as float)/CAST(CTE_Input2.TotalInvoices as float)*100.0) else 100 END as Accuracy from  CTE_Input1 join CTE_Input2 on CTE_Input1.RecieveDate=CTE_Input2.RecieveDate )B ) " +
            //            "select * from CTE_Input3");



            dt = GetData(";with CTE_Input1 as (select cast(FI_CreatedOn as date ) as RecieveDate,cast(IND_IP_ModifiedOn as date) as ProcessDate,IND_InvoiceNo  from dbo.EMSDB_FileInfo  left join dbo.EMSDB_InvDetails on IND_FI=FI_ID where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' and IND_Status not in ('IP_Asg','IP_Inp','IP_Issue')and IND_IP_Processed_By !='null' and FI_Source in ('EDI') ),CTE_Input2 as (select cast(FI_CreatedOn as date ) as RecieveDate, sum(cast(IND_InvoiceNo as int)) as TotalInvoices  from dbo.EMSDB_FileInfo  left join dbo.EMSDB_InvDetails on IND_FI=FI_ID where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' and IND_IP_Processed_By !='null' and FI_Source in ('EDI') group by cast(FI_CreatedOn as date )),CTE_Input3 as(select CTE_Input1.RecieveDate as RecieveDate,CTE_Input1.ProcessDate as ProcessDate,CTE_Input2.TotalInvoices as TotalInvoices,sum(cast(CTE_Input1.IND_InvoiceNo as int)) as InvoiceCount  from CTE_Input1 join CTE_Input2 on CTE_Input1.RecieveDate=CTE_Input2.RecieveDate  group by CTE_Input1.RecieveDate,CTE_Input1.ProcessDate,TotalInvoices),CTE_Input4 as(select RecieveDate,ProcessDate,TotalInvoices,InvoiceCount,case when InvoiceCount<>0 then (CAST(InvoiceCount as float)/CAST(TotalInvoices as float)*100.0) else 100 END as Accuracy from CTE_Input3 ), CTE_Input5 as (select RecieveDate,sum(InvoiceCount)as Pending from CTE_Input4 group by RecieveDate),CTE_Input6 as( select  CTE_Input4.RecieveDate as RecieveDate,ProcessDate,TotalInvoices,InvoiceCount, Cast(round(Accuracy,2) as decimal(10,2)) as Day1,CTE_Input5.Pending as Pending from CTE_Input5 join CTE_Input4 on CTE_Input4.RecieveDate=CTE_Input5.RecieveDate) select  RecieveDate,ProcessDate,TotalInvoices,InvoiceCount,Day1,case when TotalInvoices<>0 then (CAST(TotalInvoices as int)-CAST(Pending as int)) else 0 END as Pending  from CTE_Input6");
            if (dt.Rows.Count < 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "YourUniqueScriptKey", "alert('No records found');", true);
                grdPending.DataSource = null;
                grdPending.DataBind();
                grdReport.DataSource = null;
                grdReport.DataBind();
                grdSLA.DataSource = null;
                grdSLA.DataBind();
                subgrid.Visible = false;
                return;
            }
            //select distinct date.
            var distinctValues = dt.AsEnumerable()
                            .Select(row => new
                            {
                                attribute1_name = row.Field<DateTime>("RecieveDate"),
                            })
                            .Distinct();
            //loop through recieve date
            foreach (var RecieveDate in distinctValues)
            {
                DataTable GroupData = null;

                var query = from t in dt.AsEnumerable()
                            where t.Field<DateTime>("RecieveDate") == RecieveDate.attribute1_name
                            select t;
                if (query != null && query.Count() > 0)
                {
                    int Day6Invoice = 0;
                    decimal Day6Percenatage = 0;
                    int Day5Invoice = 0;
                    decimal Day5Percenatage = 0;
                    int Day4Invoice = 0;
                    decimal Day4Percenatage = 0;
                    int Day3Invoice = 0;
                    decimal Day3Percenatage = 0;
                    int Day2Invoice = 0;
                    decimal Day2Percenatage = 0;
                    int Pending = 0;
                    decimal Day1Percenatage = 0;
                    int Day1Invoice = 0;
                    DateTime Processdate = DateTime.Now;
                    GroupData = query.CopyToDataTable();
                    DateTime Recievedate = Convert.ToDateTime(GroupData.Rows[0]["RecieveDate"]);
                    int TotalInvoices = Convert.ToInt32(GroupData.Rows[0]["TotalInvoices"]);



                    for (int i = 0; i < pending.Rows.Count; i++)
                    {
                        DateTime PendingReceiveDate = Convert.ToDateTime(pending.Rows[i]["ReceivedDate"]);
                        //DateTime PendingReceiveDate = Convert.ToDateTime(dt.Rows[i]["RecieveDate"]);


                        if (PendingReceiveDate == Recievedate)
                        {
                            Pending = Convert.ToInt32(pending.Rows[i]["pending"]);
                            Issue = Convert.ToInt32(pending.Rows[i]["Issue"]);
                        }
                    }

             

                    //DateTime Day2;
                    //DateTime Day3;
                    //DateTime Day4;
                    //DateTime Day5;
                    //DateTime Day6;
                    DateTime extraDay = DateTime.Now;
                    int flag = 0;
                    DateTime Day2;
                    DateTime Day3;
                    DateTime Day4;
                    DateTime Day5;
                    DateTime Day6;
                    //if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    //{
                    //    Day2 = Recievedate.AddDays(3);
                    //}
                    //else
                    //{
                    //    Day2 = Recievedate.AddDays(1);
                    //}

                    //if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    //{
                    //    Day3 = Recievedate.AddDays(3);
                    //}
                    //else
                    //{
                    //    Day3 = Recievedate.AddDays(2);
                    //}
                    //if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    //{
                    //    Day4 = Recievedate.AddDays(3);
                    //}
                    //else
                    //{
                    //    Day4 = Recievedate.AddDays(3);
                    //}
                    //if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    //{
                    //    Day5 = Recievedate.AddDays(3);
                    //}
                    //else
                    //{
                    //    Day5 = Recievedate.AddDays(4);
                    //}
                    //if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    //{
                    //    Day6 = Recievedate.AddDays(3);
                    //}
                    //else
                    //{
                    //    Day6 = Recievedate.AddDays(5);
                    //}

                    if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    {
                        flag = 1;
                        Day2 = Recievedate.AddDays(3);
                    }
                    else
                    {
                        Day2 = Recievedate.AddDays(1);
                        flag = 0;
                        if (Day2.DayOfWeek == DayOfWeek.Saturday)
                        {
                            Day2 = Day2.AddDays(2);
                        }
                        if (Day2.DayOfWeek == DayOfWeek.Sunday)
                        {
                            Day2 = Day2.AddDays(1);
                        }
                    }

                    if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    {
                        if (flag == 1)
                        {
                            Day3 = Recievedate.AddDays(4);
                            flag = 1;
                        }
                        else
                        {
                            Day3 = Recievedate.AddDays(3);
                            flag = 0;
                        }
                    }
                    else
                    {
                        if (Day2 == Recievedate.AddDays(3))
                        {
                            Day3 = Recievedate.AddDays(4);
                        }
                        else
                        {
                            Day3 = Recievedate.AddDays(2);
                        }

                        if (Day3.DayOfWeek == DayOfWeek.Saturday)
                        {
                            Day3 = Day3.AddDays(2);
                        }
                        if (Day3.DayOfWeek == DayOfWeek.Sunday)
                        {
                            if (Day2 != Day3.AddDays(1))
                            {
                                Day3 = Day3.AddDays(1);
                            }
                        }
                    }
                    if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    {
                        if (flag == 1)
                        {
                            Day4 = Recievedate.AddDays(5);
                            flag = 1;
                        }
                        else
                        {
                            Day4 = Recievedate.AddDays(3);
                            flag = 0;
                        }
                    }
                    else
                    {
                        if (Day2 == Recievedate.AddDays(3) || Day3 == Recievedate.AddDays(3))
                        {
                            Day4 = Recievedate.AddDays(5);
                        }
                        else
                        {
                            Day4 = Recievedate.AddDays(3);
                        }

                        if (Day4.DayOfWeek == DayOfWeek.Saturday)
                        {
                            Day4 = Day4.AddDays(2);
                        }
                        if (Day4.DayOfWeek == DayOfWeek.Sunday)
                        {
                            if (Day3 != Day4.AddDays(1))
                            {
                                Day4 = Day4.AddDays(1);
                            }
                        }
                    }
                    if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    {
                        if (flag == 1)
                        {
                            Day5 = Recievedate.AddDays(6);
                            flag = 1;
                        }
                        else
                        {
                            Day5 = Recievedate.AddDays(3);
                            flag = 0;
                        }

                    }
                    else
                    {
                        if (Day2 == Recievedate.AddDays(4) || Day3 == Recievedate.AddDays(4) || Day4 == Recievedate.AddDays(4))
                        {
                            Day5 = Recievedate.AddDays(6);
                        }
                        else
                        {
                            Day5 = Recievedate.AddDays(4);
                        }
                        if (Day5.DayOfWeek == DayOfWeek.Saturday)
                        {
                            Day5 = Day5.AddDays(2);
                        }
                        if (Day5.DayOfWeek == DayOfWeek.Sunday)
                        {
                            if (Day4 != Day5.AddDays(1))
                            {
                                Day5 = Day5.AddDays(1);
                            }
                        }


                    }
                    if (Recievedate.DayOfWeek == DayOfWeek.Friday)
                    {
                        if (flag == 1)
                        {
                            Day6 = Recievedate.AddDays(7);
                            flag = 1;
                        }
                        else
                        {
                            Day6 = Recievedate.AddDays(3);
                            flag = 0;
                        }

                    }
                    else
                    {
                        Day6 = Recievedate.AddDays(5);
                    }

                    extraDay = Day6;

                    //loop 5 times for day1,day2,day3,day4,day5
                    for (int k = 0; k < GroupData.Rows.Count; k++)
                    {
                        Processdate = Convert.ToDateTime(GroupData.Rows[k]["ProcessDate"]);
                        //if (k == 0)
                        //{
                        if (Processdate == Recievedate)
                        {
                            Day1Invoice = Convert.ToInt32(GroupData.Rows[k]["InvoiceCount"]);
                            Day1Percenatage = Convert.ToDecimal(GroupData.Rows[k]["Day1"]);
                        }
                        //}
                        //if (k == 1)
                        //{
                        //if (GroupData.Rows.Count > 1)
                        if (Processdate == Day2)
                        {
                            Day2Invoice = Convert.ToInt32(GroupData.Rows[k]["InvoiceCount"]);
                            Day2Percenatage = Convert.ToDecimal(GroupData.Rows[k]["Day1"]);
                        }
                        //}
                        //if (k == 2)
                        //{
                        if (Processdate == Day3)
                        {
                            Day3Invoice = Convert.ToInt32(GroupData.Rows[k]["InvoiceCount"]);
                            Day3Percenatage = Convert.ToDecimal(GroupData.Rows[k]["Day1"]);

                        }
                        //}
                        //if (k == 3)
                        //{
                        if (Processdate == Day4)
                        {
                            Day4Invoice = Convert.ToInt32(GroupData.Rows[k]["InvoiceCount"]);
                            Day4Percenatage = Convert.ToDecimal(GroupData.Rows[k]["Day1"]);

                        }
                        //}
                        //if (k == 4)
                        //{
                        if (Processdate == Day5)
                        {
                            Day5Invoice = Convert.ToInt32(GroupData.Rows[k]["InvoiceCount"]);
                            Day5Percenatage = Convert.ToDecimal(GroupData.Rows[k]["Day1"]);

                        }
                        //}

                    }



                    SqlQuery = "INSERT INTO REPORT VALUES('" + Recievedate + "'," + TotalInvoices + "," + Pending + ","+Issue+"," + Day1Invoice + "," + Day1Percenatage + "," + Day2Invoice + "," + Day2Percenatage + "," + Day3Invoice + "," + Day3Percenatage + "," + Day4Invoice + "," + Day4Percenatage + "," + Day5Invoice + "," + Day5Percenatage + "," + Day6Invoice + "," + Day6Percenatage + ")";
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand(SqlQuery, con);
                        cmd.ExecuteNonQuery();
                    }

                }
            }


            //get 6plus day report
           // DataTable PlusDay_dt = GetData(";with CTE_Input1 as ( select RecieveDate,cast(IND_IP_ModifiedOn as date) as ProcessDate, count(FI_OriginalName) as InvoiceCount from (select cast(FI_CreatedOn as date ) as RecieveDate, IND_IP_ModifiedOn,FI_OriginalName from dbo.EMSDB_FileInfo  left join dbo.EMSDB_InvDetails on IND_FI=FI_ID   where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' and IND_IP_Processed_By !='null' and IND_Status not in ('IP_Asg','IP_Issue')) A group by cast(IND_IP_ModifiedOn as date),RecieveDate),CTE_Input2 as (select cast(FI_CreatedOn as date )as RecieveDate,count(FI_OriginalName) as TotalInvoices from dbo.EMSDB_FileInfo   left join dbo.EMSDB_InvDetails on IND_FI=FI_ID  where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' group by cast(FI_CreatedOn as date )),CTE_Input3 as (SELECT RecieveDate,ProcessDate,TotalInvoices,InvoiceCount,Cast(round(Accuracy,2) as decimal(10,2)) as Day1 FROM (select CTE_Input1.RecieveDate as RecieveDate,CTE_Input1.ProcessDate as ProcessDate,CTE_Input1.InvoiceCount as InvoiceCount,CTE_Input2.TotalInvoices as TotalInvoices, case when CTE_Input1.InvoiceCount<>0 then (CAST(CTE_Input1.InvoiceCount as float)/CAST(CTE_Input2.TotalInvoices as float)*100.0) else 100 END as Accuracy from CTE_Input1 join CTE_Input2 on CTE_Input1.RecieveDate=CTE_Input2.RecieveDate )B ) select  RecieveDate,TotalInvoices,PlusDay as '6PlusDay',Cast(round(Accuracy,2) as decimal(10,2)) as '6PlusDay%' from (select RecieveDate,TotalInvoices,PlusDay,case when PlusDay<>0 then (CAST(PlusDay as float)/CAST(TotalInvoices as float)*100.0) else 100 END as Accuracy  from (select RecieveDate,TotalInvoices,sum(InvoiceCount) as PlusDay from CTE_Input3 WHERE ProcessDate >= DATEADD(day,5, RecieveDate)group by RecieveDate,TotalInvoices)C)D order by RecieveDate");
            DataTable PlusDay_dt = GetData(";with CTE_Input1 as (select cast(FI_CreatedOn as date ) as RecieveDate,cast(IND_IP_ModifiedOn as date) as ProcessDate,IND_InvoiceNo  from dbo.EMSDB_FileInfo  left join dbo.EMSDB_InvDetails on IND_FI=FI_ID where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' and IND_Status not in ('IP_Asg','IP_Inp','IP_Issue')and IND_IP_Processed_By !='null' and FI_Source in ('EDI') ),CTE_Input2 as (select cast(FI_CreatedOn as date ) as RecieveDate, sum(cast(IND_InvoiceNo as int)) as TotalInvoices  from dbo.EMSDB_FileInfo  left join dbo.EMSDB_InvDetails on IND_FI=FI_ID where cast(FI_CreatedOn as date ) >='" + From.ToString("yyyy-MM-dd") + "'  and cast(FI_CreatedOn as date ) <='" + To.ToString("yyyy-MM-dd") + "' and IND_IP_Processed_By !='null' and FI_Source in ('EDI') group by cast(FI_CreatedOn as date )),CTE_Input3 as(select CTE_Input1.RecieveDate as RecieveDate,CTE_Input1.ProcessDate as ProcessDate,CTE_Input2.TotalInvoices as TotalInvoices,sum(cast(CTE_Input1.IND_InvoiceNo as int)) as InvoiceCount  from CTE_Input1 join CTE_Input2 on CTE_Input1.RecieveDate=CTE_Input2.RecieveDate  group by CTE_Input1.RecieveDate,CTE_Input1.ProcessDate,TotalInvoices),CTE_Input4 as(select RecieveDate,ProcessDate,TotalInvoices,InvoiceCount,case when InvoiceCount<>0 then (CAST(InvoiceCount as float)/CAST(TotalInvoices as float)*100.0) else 100 END as Accuracy from CTE_Input3 ), CTE_Input5 as (select RecieveDate,sum(InvoiceCount)as Pending from CTE_Input4 group by RecieveDate),CTE_Input6 as( select  CTE_Input4.RecieveDate as RecieveDate,ProcessDate,TotalInvoices,InvoiceCount, Cast(round(Accuracy,2) as decimal(10,2)) as Day1,CTE_Input5.Pending as Pending from CTE_Input5 join CTE_Input4 on CTE_Input4.RecieveDate=CTE_Input5.RecieveDate) select  RecieveDate,TotalInvoices,InvoiceCount as '6PlusDay',Day1 as '6PlusDay%' from CTE_Input6 WHERE ProcessDate >= DATEADD(day,5, RecieveDate) order by RecieveDate");

            for (int z = 0; z < PlusDay_dt.Rows.Count; z++)
            {
                int plusdayCount = Convert.ToInt32(PlusDay_dt.Rows[z]["6PlusDay"]);
                decimal plusdayPercentage = Convert.ToInt32(PlusDay_dt.Rows[z]["6PlusDay%"]);
                DateTime Recievedate = Convert.ToDateTime(PlusDay_dt.Rows[z]["RecieveDate"]);

                //update 6plus day data.
                SqlQuery = " update Report set Day6Count=" + plusdayCount + ", Day6Percentage='" + plusdayPercentage + "' where RECEIVEDATE='" + Recievedate + "'";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(SqlQuery, con);
                    cmd.ExecuteNonQuery();
                }
            }
            //generate table
            DataTable data = GetData("SELECT RECEIVEDATE,TotalInvoice,Pending,Day1Count,Convert(varchar(10),day1Percentage)+'%' AS day1Percentage,Day2Count,Convert(varchar(10),day2Percentage)+'%' AS Day2Percentage,Day3Count,Convert(varchar(10),day3Percentage)+'%' AS Day3Percentage,Day4Count,Convert(varchar(10),day4Percentage)+'%' AS Day4Percentage,Day5Count,Convert(varchar(10),day5Percentage)+'%' AS Day5Percentage,Day6Count,Convert(varchar(10),day6Percentage)+'%' AS Day6Percentage FROM REPORT");
            grdReport.DataSource = data;
            grdReport.DataBind();


            //generate SLA table
            DataTable SLA = GetData("select RECEIVEDATE,TotalInvoice,ProcessedInvoices,Convert(varchar(10),(CAST(round(Percentage,2) as decimal(10,2))))+'%' as Percentage from (select RECEIVEDATE,TotalInvoice,sum(Day1Count+Day2Count+Day3Count+Day4Count+Day5Count) as ProcessedInvoices,case when sum(Day1Count+Day2Count+Day3Count+Day4Count+Day5Count)<>0 then (CAST(sum(Day1Count+Day2Count+Day3Count+Day4Count+Day5Count) as float)/CAST(TotalInvoice as float)*100.0) else 100 END as Percentage from dbo.REPORT  group by RECEIVEDATE,TotalInvoice ) as a order by RECEIVEDATE");
            grdSLA.DataSource = SLA;
            grdSLA.DataBind();

            DataTable pendingReport = GetData("select RECEIVEDATE,Pending+Issue as TotalPending,Pending,Issue from REPORT where Pending<>0 or Issue <>0");
            grdPending.DataSource = pendingReport;
            grdPending.DataBind();

            ds.Tables.Add(data);
            ds.Tables.Add(SLA);
            ds.Tables.Add(pendingReport);

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

        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=EMSDBWeeklyReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                grdReport.AllowPaging = false;
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdReport.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in grdReport.HeaderRow.Cells)
                {
                    cell.BackColor = grdReport.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grdReport.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grdReport.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grdReport.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
                grdReport.RenderControl(hw);
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
                Response.AddHeader("content-disposition", "attachment;filename=EMSDB_Weekly Report.xlsx");
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


    }
}