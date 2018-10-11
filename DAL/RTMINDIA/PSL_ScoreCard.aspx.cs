using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

public partial class PSL_ScoreCard : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());

    SqlDataAdapter da;
    DataSet ds = new DataSet();
    int w;
    DataTable dtResult = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindLineChart();
            CumulativeChart();
            BindTodaysBarChart();
            BindTodayEffectiveRateChart();
            BindPreviousDayCount();
            BindPrevdayEffectiveRateChart();
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        BindLineChart();
        CumulativeChart();
        BindTodaysBarChart();
        BindTodayEffectiveRateChart();
        BindPreviousDayCount();
        BindPrevdayEffectiveRateChart();
    }

    private static DataTable GetData(string query)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.AppSettings["conString"].ToString();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                }
            }
            return dt;
        }
    }

    private void BindLineChart()
    {
        //int j;
        string query;
        string name;
        if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
        {
            query = "select convert(char(5), dateadd(minute, 60 + (datediff(minute, 0, EndTime) / 60) * 60, 0), 108) as time1 , count(SKU_Id) as ipvcount " +
                        "from RTM_IPVDetails where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.AddDays(-3).ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') " +
                        "group by dateadd(minute, 60 + (datediff(minute, 0, EndTime) / 60) * 60, 0)";
            name = DateTime.Now.AddDays(-3).ToShortDateString();
        }
        else
        {
            query = "select convert(char(5), dateadd(minute, 60 + (datediff(minute, 0, EndTime) / 60) * 60, 0), 108) as time1 , count(SKU_Id) as ipvcount " +
                        "from RTM_IPVDetails where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.AddDays(-1).ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') " +
                        "group by dateadd(minute, 60 + (datediff(minute, 0, EndTime) / 60) * 60, 0)";
            name = DateTime.Now.AddDays(-1).ToShortDateString();
        }


        DataTable dt = GetData(query);

        //string[] x = new string[dt.Rows.Count];
        string[] x = { "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "00:00" };

        decimal[] y = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };// new decimal[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int index1 = Array.IndexOf(x, dt.Rows[i][0].ToString());
            if (index1 >= 0)
            {
                x[index1] = dt.Rows[i][0].ToString();
                y[index1] = Convert.ToInt32(dt.Rows[i][1]);
            }

        }

        LineChart1.Series.Add(new AjaxControlToolkit.LineChartSeries { Name = name, Data = y, LineColor = "#FAA30C" });
        LineChart1.CategoriesAxis = string.Join(",", x);

        query = "select convert(char(5), dateadd(minute, 60 + (datediff(minute, 0, EndTime) / 60) * 60, 0), 108) as time1 , count(SKU_Id) as ipvcount " +
                        "from RTM_IPVDetails where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') " +
                        "group by dateadd(minute, 60 + (datediff(minute, 0, EndTime) / 60) * 60, 0)";

        dt = GetData(query);
        if (dt.Rows.Count > 0)
        {
            DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
            string value = lastRow["time1"].ToString();
            int index2 = Array.IndexOf(x, value);
            //Array.Clear(x, index2 + 1, (x.Length) - (index2 + 1));
            //List<string> list = new List<string>(x);
            //list.RemoveRange(index2 + 1, (x.Length) - (index2 + 1));
            //x = list.ToArray();
            // y = new decimal[index2 + 1];
            y = new decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int index1 = Array.IndexOf(x, dt.Rows[i][0].ToString());
                if (index1 >= 0)
                {
                    x[index1] = dt.Rows[i][0].ToString();
                    y[index1] = Convert.ToInt32(dt.Rows[i][1]);
                }

            }

            LineChart1.Series.Add(new AjaxControlToolkit.LineChartSeries { Name = DateTime.Now.ToShortDateString(), Data = y, LineColor = "#0C77FA" });





        }
        // LineChart1.ChartTitle = "CMP Strike Rate (Invoices/Hour)";
        LineChart1.Visible = true;
    }

    private void CumulativeChart()
    {
        string name = "";
        DataTable dt3 = new DataTable();
        con.Open();
        SqlCommand cmd = new SqlCommand("sp_PSL_Cumulative", con);
        cmd.CommandType = CommandType.StoredProcedure;
        if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
        {
            cmd.Parameters.Add("@date1", SqlDbType.VarChar).Value = DateTime.Now.AddDays(-3).ToShortDateString();
            name = DateTime.Now.AddDays(-3).ToShortDateString();
        }
        else
        {
            cmd.Parameters.Add("@date1", SqlDbType.VarChar).Value = DateTime.Now.AddDays(-1).ToShortDateString();
            name = DateTime.Now.AddDays(-1).ToShortDateString();
        }

        da = new SqlDataAdapter(cmd);
        da.Fill(dt3);
        con.Close();
        string[] x = { "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00", "00:00" };
        if (dt3.Rows.Count > 0)
        {


            decimal[] y = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };// new decimal[dt.Rows.Count];
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                int index1 = Array.IndexOf(x, dt3.Rows[i][0].ToString());
                if (index1 >= 0)
                {
                    x[index1] = dt3.Rows[i][0].ToString();
                    y[index1] = Convert.ToInt32(dt3.Rows[i][1]);
                }

            }

            LineChart2.Series.Add(new AjaxControlToolkit.LineChartSeries { Name = name, Data = y, LineColor = "#FAA30C" });
            LineChart2.CategoriesAxis = string.Join(",", x);


        }

        dt3 = new DataTable();
        con.Open();
        cmd = new SqlCommand("sp_PSL_Cumulative", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@date1", SqlDbType.VarChar).Value = DateTime.Now.ToShortDateString();
        da = new SqlDataAdapter(cmd);
        da.Fill(dt3);
        con.Close();

        if (dt3.Rows.Count > 0)
        {

            decimal[] y = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };// new decimal[dt.Rows.Count];
            for (int i = 0; i < dt3.Rows.Count; i++)
            {
                int index1 = Array.IndexOf(x, dt3.Rows[i][0].ToString());
                if (index1 >= 0)
                {
                    x[index1] = dt3.Rows[i][0].ToString();
                    y[index1] = Convert.ToInt32(dt3.Rows[i][1]);
                }

            }

            LineChart2.Series.Add(new AjaxControlToolkit.LineChartSeries { Name = DateTime.Now.ToShortDateString(), Data = y, LineColor = "#0C77FA" });
            LineChart2.CategoriesAxis = string.Join(",", x);
        }

        //LineChart2.ChartTitle = "PSL Strike Rate (Cumulative)";
        LineChart2.Visible = true;
    }

    private void BindPreviousDayCount()
    {
        string query;
        string name;
        if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
        {
            query = "select  UserName, count(Distinct SKU_ID) as TotalCount from RTM_IPVDetails where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.AddDays(-3).ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') group by UserName order by count(Distinct SKU_ID) Desc";
            name = DateTime.Now.AddDays(-3).ToShortDateString();
        }
        else
        {
            query = "select  UserName, count(Distinct SKU_ID) as TotalCount from RTM_IPVDetails where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.AddDays(-1).ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') group by UserName order by count(Distinct SKU_ID) Desc";
            name = DateTime.Now.AddDays(-1).ToShortDateString();
        }


        DataTable dt = GetData(query);
        if (dt.Rows.Count > 0)
        {
            string[] x = new string[dt.Rows.Count];
            decimal[] y = new decimal[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][0].ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][1]);
            }

            BarChart1.Series[0].Points.DataBindXY(x, y);
            this.BarChart1.Series[0].BorderWidth = 1;
            this.BarChart1.Series[0].Label = "#VALY";
            BarChart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            BarChart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            BarChart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Angle = 45;
            BarChart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            //BarChart1.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y, BarColor = "#090DEB" });
            //BarChart1.CategoriesAxis = string.Join(",", x);
            //BarChart1.ChartTitle = name +"-Effective Rate/Hour";
            //BarChart1.Titles.Add(CreateTitle(name + "-Total Invoices Processed"));
            lblPrevTotal.Text = name + "-Total Invoices Processed";
            BarChart1.Visible = true;
        }

    }

    private void BindTodaysBarChart()
    {


        string query = "select  UserName, count(Distinct SKU_ID) as TotalCount from RTM_IPVDetails where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') group by UserName order by count(Distinct SKU_ID) Desc";

        DataTable dt = GetData(query);
        if (dt.Rows.Count > 0)
        {
            string[] x = new string[dt.Rows.Count];
            decimal[] y = new decimal[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][0].ToString();
                y[i] = Convert.ToInt32(dt.Rows[i][1]);
            }

            BarChart2.Series[0].Points.DataBindXY(x, y);
            this.BarChart2.Series[0].BorderWidth = 1;
            this.BarChart2.Series[0].Label = "#VALY";
            BarChart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            BarChart2.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            BarChart2.ChartAreas["ChartArea1"].AxisY.LabelStyle.Angle = 45;
            BarChart2.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            //BarChart2.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y, BarColor = "#090DEB" });
            //BarChart2.CategoriesAxis = string.Join(",", x);
            //BarChart2.ChartTitle = DateTime.Now.ToShortDateString() +"-Effective Rate/Hour";
            //BarChart2.Titles.Add(CreateTitle(DateTime.Now.ToShortDateString() + "-Total Invoices Processed"));
            lblTotalInvoiceHeader.Text = DateTime.Now.ToShortDateString() + "-Total Invoices Processed";
            // BarChart2.ChartAreas["ChartArea1"].AxisY.Interval = 5;
            BarChart2.Visible = true;
        }
    }

    private void BuildTodayEffectiveRateTable()
    {
        DataColumn dc;
        //dtResult = new DataTable();
        try
        {
            dc = new DataColumn("Employee");
            dtResult.Columns.Add(dc);

            dc = new DataColumn("EffectiveRate", typeof(Decimal));
            dtResult.Columns.Add(dc);
        }
        catch (Exception)
        {

        }
    }

    private DataSet TodaysEffectiveRateResult()
    {
        da = new SqlDataAdapter("select  UserName, count(Distinct SKU_ID) as TotalCount, " +
                 "sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/3600 as hour, " +
                 "(sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/60)%60 as minute," +
                 "(sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/60)%60)))%60 as seconds " +
                 "from RTM_IPVDetails " +
                "where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') " +
                "group by UserName order by count(Distinct SKU_ID) Desc", con);
        da.Fill(ds, "TodayRate");
        return ds;
    }

    private void BindTodayEffectiveRateChart()
    {
        try
        {
            DataRow dr;

            BuildTodayEffectiveRateTable();

            ds = TodaysEffectiveRateResult();

            if (ds.Tables["TodayRate"].Rows.Count > 0)
            {
                foreach (DataRow dr1 in ds.Tables["TodayRate"].Rows)
                {
                    dr = dtResult.NewRow();

                    dr["Employee"] = dr1["UserName"];

                    decimal value = Convert.ToDecimal(dr1["hour"]) + Convert.ToDecimal(dr1["minute"]) / 60;
                    if (value != 0)
                    {
                        dr["EffectiveRate"] = Convert.ToInt32(dr1["TotalCount"]) / (Convert.ToDecimal(dr1["hour"]) + Convert.ToDecimal(dr1["minute"]) / 60);
                    }
                    else
                    {
                        continue;
                    }
                    dr["EffectiveRate"] = decimal.Round(Convert.ToDecimal(dr["EffectiveRate"]), 2, MidpointRounding.AwayFromZero);

                    dtResult.Rows.Add(dr);
                }
            }

            if (dtResult.Rows.Count > 0)
            {
                dtResult.DefaultView.Sort = "EffectiveRate Desc";
                dtResult = dtResult.DefaultView.ToTable();

                string[] x = new string[dtResult.Rows.Count];
                decimal[] y = new decimal[dtResult.Rows.Count];
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    x[i] = dtResult.Rows[i]["Employee"].ToString();
                    y[i] = Convert.ToDecimal(dtResult.Rows[i]["EffectiveRate"]);
                }

                ChartTodayEffRate.Series[0].Points.DataBindXY(x, y);
                this.ChartTodayEffRate.Series[0].BorderWidth = 1;
                this.ChartTodayEffRate.Series[0].Label = "#VALY";
                ChartTodayEffRate.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                ChartTodayEffRate.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                ChartTodayEffRate.ChartAreas["ChartArea1"].AxisY.LabelStyle.Angle = 45;
                ChartTodayEffRate.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                //BarChart2.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y, BarColor = "#090DEB" });
                //BarChart2.CategoriesAxis = string.Join(",", x);
                //BarChart2.ChartTitle = DateTime.Now.ToShortDateString() +"-Effective Rate/Hour";
                //ChartTodayEffRate.Titles.Add(CreateTitle(DateTime.Now.ToShortDateString() + "-Effective Rate/Hour"));
                lblEfectiveRateHeader.Text = DateTime.Now.ToShortDateString() + "-Effective Rate/Hour";
            }

        }
        catch (Exception)
        {

        }
    }

    private DataSet PreviousDaysEffReateResult()
    {

        if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
        {
            da = new SqlDataAdapter("select  UserName, count(Distinct SKU_ID) as TotalCount, " +
                "sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/3600 as hour, " +
                "(sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/60)%60 as minute," +
                "(sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/60)%60)))%60 as seconds " +
                "from RTM_IPVDetails " +
               "where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.AddDays(-3).ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') " +
               "group by UserName order by count(Distinct SKU_ID) Desc", con);
        }
        else
        {
            da = new SqlDataAdapter("select  UserName, count(Distinct SKU_ID) as TotalCount, " +
                "sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/3600 as hour, " +
                "(sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/60)%60 as minute," +
                "(sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))-(((sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/3600)*3600)-60*((sum(datediff(second,'00:00:00',REPLACE(Duration,'-', '')))/60)%60)))%60 as seconds " +
                "from RTM_IPVDetails " +
               "where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, EndTime))) = '" + DateTime.Now.AddDays(-1).ToShortDateString() + "' and (SubTask_Id='131' or SubTask_Id='143') " +
               "group by UserName order by count(Distinct SKU_ID) Desc", con);
        }

        da.Fill(ds, "PrevDayRate");
        return ds;
    }

    private void BindPrevdayEffectiveRateChart()
    {
        try
        {
            dtResult.Clear();
            dtResult.Clone();
            DataRow dr;

            BuildTodayEffectiveRateTable();

            ds = PreviousDaysEffReateResult();

            if (ds.Tables["PrevDayRate"].Rows.Count > 0)
            {
                foreach (DataRow dr1 in ds.Tables["PrevDayRate"].Rows)
                {
                    dr = dtResult.NewRow();

                    dr["Employee"] = dr1["UserName"];

                    decimal value = Convert.ToDecimal(dr1["hour"]) + Convert.ToDecimal(dr1["minute"]) / 60;
                    if (value != 0)
                    {
                        dr["EffectiveRate"] = Convert.ToInt32(dr1["TotalCount"]) / (Convert.ToDecimal(dr1["hour"]) + Convert.ToDecimal(dr1["minute"]) / 60);
                    }
                    else
                    {
                        continue;
                    }
                    dr["EffectiveRate"] = decimal.Round(Convert.ToDecimal(dr["EffectiveRate"]), 2, MidpointRounding.AwayFromZero);

                    dtResult.Rows.Add(dr);
                }
            }

            if (dtResult.Rows.Count > 0)
            {
                dtResult.DefaultView.Sort = "EffectiveRate Desc";
                dtResult = dtResult.DefaultView.ToTable();

                string[] x = new string[dtResult.Rows.Count];
                decimal[] y = new decimal[dtResult.Rows.Count];
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    x[i] = dtResult.Rows[i]["Employee"].ToString();
                    y[i] = Convert.ToDecimal(dtResult.Rows[i]["EffectiveRate"]);
                }

                ChartPrevDayEffRate.Series[0].Points.DataBindXY(x, y);
                this.ChartPrevDayEffRate.Series[0].BorderWidth = 1;
                this.ChartPrevDayEffRate.Series[0].Label = "#VALY";
                ChartPrevDayEffRate.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                ChartPrevDayEffRate.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                ChartPrevDayEffRate.ChartAreas["ChartArea1"].AxisY.LabelStyle.Angle = 45;
                ChartPrevDayEffRate.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                //BarChart2.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = y, BarColor = "#090DEB" });
                //BarChart2.CategoriesAxis = string.Join(",", x);
                //BarChart2.ChartTitle = DateTime.Now.ToShortDateString() +"-Effective Rate/Hour";
                if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
                {
                    // ChartPrevDayEffRate.Titles.Add(CreateTitle(DateTime.Now.AddDays(-3).ToShortDateString() + "-Effective Rate/Hour"));
                    lblPrevEffectiveRate.Text = DateTime.Now.AddDays(-3).ToShortDateString() + "-Effective Rate/Hour";
                }
                else
                {
                    // ChartPrevDayEffRate.Titles.Add(CreateTitle(DateTime.Now.AddDays(-1).ToShortDateString() + "-Effective Rate/Hour"));
                    lblPrevEffectiveRate.Text = DateTime.Now.AddDays(-1).ToShortDateString() + "-Effective Rate/Hour";
                }

            }
        }
        catch (Exception)
        {


        }
    }
}