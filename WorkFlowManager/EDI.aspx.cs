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
using System.IO;
using System.Text;
using System.Collections;
using System.Security.Permissions;
namespace WorkFlowManager
{
    public partial class EDI : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        //get emsdb sql queries from clsEMSDB class
        clsEMSDB objEMS = new clsEMSDB();
        //data base connection class.
        SqlDBHelper objDB = new SqlDBHelper();
        string SQlQuery;
        bool result;
        private const int _firstEditCellIndex = 2;
        int Flag = 0;
        int commentsFlag = 0;
        string Query;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
                    //user id storing in session, if session in null or time out it will redirect to the Default.aspx.
                    HttpCookie newSessionIdCookie = Request.Cookies["ASP.NET_SessionId"];
                    if (newSessionIdCookie != null)
                    {
                        string newSessionIdCookieValue = newSessionIdCookie.Value;
                        if (newSessionIdCookieValue != string.Empty)
                        {
                            Response.Redirect("Default.aspx");
                        }
                    }
                }
            }
            if (!IsPostBack)
            {
                //page load.  get invoice's which are not proccessed by IP team, once process completed data will not appear in the page and diplayed in the grid  
                GetAssignedInvoices();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
                EmployeesUpdatePanel.Update();
                //UpdatePanel2.Update();
                //CommentsUpdatePanel.Update();
            }

            if (this.gvIPInvoices.SelectedIndex > -1)
            {
                // Call UpdateRow on every postback
                this.gvIPInvoices.UpdateRow(this.gvIPInvoices.SelectedIndex, false);
                EmployeesUpdatePanel.Update();
                //UpdatePanel2.Update();
                //CommentsUpdatePanel.Update();
            }
        }

        //get invoice details from actuall EMSDB server and update the invoice status in our local emsdb database.
        protected void btnHidden_Click(object sender, EventArgs e)
        {
            if (lblgvRowId.Text.Length <= 0)
            {
                return;
            }
            //  System.Threading.Thread.Sleep(5000);

           // CheckandUpdateLineItem(lblgvRowId.Text);

            lblgvRowId.Text = string.Empty;
            lblgvInvoiceNo.Text = string.Empty;
        }

        //get invoice's which are not proccessed and diplayed in the grid  
        private void GetAssignedInvoices()
        {
            dt = new DataTable();
            dt = objEMS.GetAssignedEDIInvoices(Session["access"].ToString(), Session["username"].ToString());
            if (dt.Rows.Count > 0)
            {
                gvIPInvoices.DataSource = dt;
                gvIPInvoices.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
            }
            else
            {
                gvIPInvoices.DataSource = null;
                gvIPInvoices.DataBind();
            }
        }
        //grid rows and columns binding
        protected void gvIPInvoices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridView _gridView = (GridView)sender;
                bool result;
                switch (e.CommandName)
                {
                    case ("SingleClick"):

                        // Get the row index
                        int _rowIndex = int.Parse(e.CommandArgument.ToString());
                        int rowid = Convert.ToInt32(_gridView.DataKeys[_rowIndex].Value.ToString());
                        CheckBox chkSelect = (CheckBox)_gridView.Rows[_rowIndex].Cells[1].FindControl("chkSelect");
                        CheckBox chkIssue = (CheckBox)_gridView.Rows[_rowIndex].Cells[1].FindControl("chkIssue");
                        CheckBox chkCompleted = (CheckBox)_gridView.Rows[_rowIndex].Cells[1].FindControl("chkCompleted");
                        Label lblInvoiceNo = (Label)_gridView.Rows[_rowIndex].Cells[6].FindControl("lblInvoiceNo");
                        Label lblStatus = (Label)_gridView.Rows[_rowIndex].Cells[5].FindControl("lblStatus");
                        //CheckBox chkduplicates = (CheckBox)_gridView.Rows[_rowIndex].Cells[15].FindControl("chkduplicates");
                        DropDownList ddlprocess = (DropDownList)_gridView.Rows[_rowIndex].Cells[15].FindControl("ddlprocess");
                       // FileUpload fileUpload = _gridView.Rows[_rowIndex].FindControl("FileUpload1") as FileUpload;
                        Label read = (Label)_gridView.Rows[_rowIndex].Cells[17].FindControl("btnRead");
                        Label lblFile = (Label)_gridView.Rows[_rowIndex].Cells[3].FindControl("lblFile"); 
                        int _columnIndex = int.Parse(Request.Form["__EVENTARGUMENT"]);
                        _gridView.SelectedIndex = _rowIndex;
                        if (_columnIndex == 3)
                        {

                            if (chkSelect.Checked == true)
                            {
                                string url = "ViewFile.aspx?id=" + rowid;
                                string s = "window.open('" + url + "', 'popup_window', 'width=1000,height =600, top=100,left=100,resizable=yes');";
                                //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "newWindow", cmd, true);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", s, true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice", "alert('Please select invoice')", true);
                                return;
                            }
                            return;
                        }
                        if (lblStatus.Text == "QC_Idle" || lblStatus.Text == "QC_Asg" || lblStatus.Text == "QC_Inp" || lblStatus.Text == "QC_Issue" || lblStatus.Text == "QC_Comp")
                        // if (lblStatus.Text == "IP_Asg" || lblStatus.Text == "Rework")
                        {
                            if (_columnIndex == 7)
                            {
                                if (chkSelect.Checked == true)
                                {
                                    lblPopTeam.Text = "IP";
                                    lblPopFileId.Text = rowid.ToString();
                                    dt = new DataTable();
                                    dt = objEMS.GetComments(rowid);
                                    if (dt.Rows.Count > 0)
                                    {
                                        gvComments.DataSource = dt;
                                        gvComments.DataBind();
                                    }
                                    else
                                    {
                                        gvComments.DataSource = null;
                                        gvComments.DataBind();
                                    }
                                    if (chkCompleted.Checked == true)
                                    {

                                        commentsFlag = 1;
                                    }
                                    else
                                    {

                                        commentsFlag = 0;
                                    }
                                    mpePopUp.Show();
                                }

                                return;
                            }
                            else
                            {
                                chkCompleted.Checked = true;
                                chkSelect.Checked = true;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QCInvoices", "alert('Invoice Sent for QC. You cannot modify')", true);
                                GetAssignedInvoices();
                                return;
                            }
                        }

                        if (_columnIndex == 2)
                        {
                            if (lblStatus.Text == "IP_Asg" || lblStatus.Text == "Rework")
                            {
                                result = objEMS.UpdateStatus("IP_Inp", "", Convert.ToString(rowid));
                                if (result == true)
                                {

                                    //Namohar Changed code for Grid refresh on 10/02/2017
                                    if (search_textbox.Text == "")
                                    {
                                        GetAssignedInvoices();
                                    }
                                    else
                                    {
                                        SerachData();
                                    }

                                    result = false;
                                    result = objEMS.RecordLog(rowid.ToString(), "IP", "Status", "IP_Asg", "IP_Inp", Session["username"].ToString());
                                    if (result == true)
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error", "alert('Error occured.. Please try again')", true);
                                }
                            }

                            return;
                        }
                        //Checking and updating duplicate invoice's, namohar changed code on 17-02-2017.
                        if (_columnIndex == 15)
                        {
                            DataTable dt1 = new DataTable();
                            DataRow dr;
                            dt1.Columns.Add("Process", typeof(string));
                            dt1.Columns.Add("ID", typeof(int));
                            dr = dt1.NewRow();
                            dr["Process"] = "DNP";
                            dr["ID"] = "1";
                            dr = dt1.NewRow();
                            dr["Process"] = "Duplicates";
                            dr["ID"] = "2";

                            dr = dt1.NewRow();
                            dr["Process"] = "EDI";
                            dr["ID"] = "3";
                            dt1.Rows.Add(dr);
                            if (dt1.Rows.Count > 0)
                            {
                                ddlprocess.DataSource = dt1;
                                ddlprocess.DataTextField = "Process";
                                ddlprocess.DataValueField = "ID";
                                ddlprocess.DataBind();
                                //ddlprocess.Items.Insert(0, "Select");
                                //ddlprocess.SelectedIndex = 0;
                            }


                            //if (lblStatus.Text == "IP_Inp" || lblStatus.Text == "Rework")
                            //{
                            //    result = objEMS.UpdateStatus("Duplicate", "", Convert.ToString(rowid));
                            //    if (result == true)
                            //    {

                            //        //Namohar Changed code for Grid refresh on 10/02/2017
                            //        if (search_textbox.Text == "")
                            //        {
                            //            GetAssignedInvoices();
                            //        }
                            //        else
                            //        {
                            //            SerachData();
                            //        }

                            //        result = false;
                            //        result = objEMS.RecordLog(rowid.ToString(), "IP", "Status", "IP_Inp", "Duplicate", Session["username"].ToString());
                            //        if (result == true)
                            //        {
                            //            result = false;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error", "alert('Error occured.. Please try again')", true);
                            //    }
                            //}


                            // return;
                        }

                        if (_columnIndex == 11)
                        {
                            if (Session["access"].ToString() == "4")
                            {
                                return;
                            }
                            if (lblInvoiceNo.Text != "")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QCInvoices", "alert('Please clear the invoice count and assign.')", true);
                                return;
                            }

                        }

                        if (_columnIndex == 4 || _columnIndex == 5 || _columnIndex == 10 || _columnIndex == 12 || _columnIndex == 13 || _columnIndex == 14)
                        {
                            return;
                        }
                        if (_columnIndex == 7)
                        {
                            if (chkSelect.Checked == true)
                            {
                                lblPopTeam.Text = "IP";
                                lblPopFileId.Text = rowid.ToString();
                                dt = new DataTable();
                                dt = objEMS.GetComments(rowid);
                                if (dt.Rows.Count > 0)
                                {
                                    gvComments.DataSource = dt;
                                    gvComments.DataBind();
                                }
                                else
                                {
                                    gvComments.DataSource = null;
                                    gvComments.DataBind();
                                }
                                if (chkCompleted.Checked == true)
                                {

                                    commentsFlag = 1;
                                }
                                else
                                {

                                    commentsFlag = 0;
                                }
                                //model pop up.
                                mpePopUp.Show();
                            }

                            return;
                        }
                        if (_columnIndex == 8)
                        {

                            if (chkSelect.Checked == true)
                            {

                                if (chkCompleted.Checked == true)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QCInvoices", "alert('Invoice Sent for QC. You cannot modify')", true);
                                    return;
                                }

                                if (lblInvoiceNo.Text == "")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QCInvoices", "alert('Please Enter the invoice count')", true);
                                    return;
                                }

                                result = objEMS.UpdateStatus("IP_Issue", "", Convert.ToString(rowid));
                                if (result == true)
                                {
                                    GetAssignedInvoices();

                                    result = false;
                                    result = objEMS.RecordLog(rowid.ToString(), "IP", "Status", lblStatus.Text, "IP_Issue", Session["username"].ToString());
                                    if (result == true)
                                    {
                                        result = false;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error2", "alert('Error occured.. Please try again')", true);
                                    chkIssue.Checked = false;
                                }
                            }
                            else
                            {
                                chkIssue.Checked = false;
                            }
                            return;
                        }
                        if (_columnIndex == 9)
                        {
                            if (chkSelect.Checked == true)
                            {
                                //if (chkCompleted.Checked == true)
                                //{
                                //    chkCompleted.Checked = true;
                                //    return;
                                //}

                                if (lblInvoiceNo.Text.Length <= 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Enter Invoice No", "alert('Please enter invoice count')", true);
                                    chkCompleted.Checked = false;
                                    return;
                                }
                                lblgvRowId.Text = rowid.ToString();
                                lblgvInvoiceNo.Text = lblInvoiceNo.Text;

                                //string script = "$(document).ready(function () { $('[id*=btnHidden]').click(); });";
                                //ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);

                                if (lblgvRowId.Text.Length <= 0)
                                {
                                    return;
                                }
                                //  System.Threading.Thread.Sleep(5000);

                             //   CheckandUpdateLineItem(lblgvRowId.Text);

                                if (lblgvInvoiceNo.Text != string.Empty)
                                {

                                    result = objEMS.UpdateStatus("QC_Idle", "", Convert.ToString(rowid));
                                    if (result == true)
                                    {
                                        GetAssignedInvoices();
                                        result = false;
                                        result = objEMS.RecordLog(rowid.ToString(), "IP", "Status", "IP_Inp", "QC_Idle", Session["username"].ToString());
                                        if (result == true)
                                        {
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error3", "alert('Error occured.. Please try again')", true);
                                        //chkCompleted.Checked = false;
                                    }
                                }

                                lblgvRowId.Text = string.Empty;
                                lblgvInvoiceNo.Text = string.Empty;


                                //System.Threading.Thread.Sleep(5000);
                                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "load", "ShowProgress();", true);

                                //dt = new DataTable();
                                //dt = objEMS.getLineItem(rowid);
                                //if (dt.Rows.Count > 0)
                                //{
                                //    SqlConnection con = new SqlConnection("Data Source=" + dt.Rows[0]["C_DataSource"].ToString() + ";Initial Catalog=" + dt.Rows[0]["C_DataBase"].ToString() + ";User ID=" + dt.Rows[0]["C_UserId"].ToString() + ";Password=" + dt.Rows[0]["C_Password"].ToString() + ";");

                                //    SqlDataAdapter da = new SqlDataAdapter("SELECT INVOICE_NUM, ATTACHMENT_NAME FROM IM_INVOICE IM WITH (NOLOCK) LEFT JOIN IM_INVOICE_ATTACHMENT IMA WITH (NOLOCK) ON IM.INVOICE_RID = IMA.INVOICE_FK where Invoice_Num = '11897981'", con);

                                //    dt = new DataTable();

                                //    da.Fill(dt);

                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        da = new SqlDataAdapter("SELECT IM.INVOICE_NUM, COUNT(INVOICE_LINE_ITEM_RID) AS LINE_ITEM_COUNT FROM IM_INVOICE IM WITH (NOLOCK) " +
                                //                                             "JOIN IM_INVOICE_INVENTORY INV WITH (NOLOCK) ON IM.INVOICE_RID = INV.INVOICE_FK " +
                                //                                            "JOIN IM_INVOICE_LINE_ITEM LIN WITH (NOLOCK) ON LIN.INVOICE_INV_FK = INV.INVOICE_INVENTORY_RID " +
                                //                                             "WHERE Invoice_Num = '1189798' " +
                                //                                       "GROUP BY IM.INVOICE_NUM", con);
                                //        DataTable dt3 = new DataTable();
                                //        da.Fill(dt3);

                                //        if (dt3.Rows.Count > 0)
                                //        {
                                //            result = objEMS.UpdateStatus("QC_Idle", dt3.Rows[0]["LINE_ITEM_COUNT"].ToString(), Convert.ToString(rowid));
                                //            if (result == true)
                                //            {
                                //                GetAssignedInvoices();
                                //                result = false;
                                //            }
                                //            else
                                //            {
                                //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error3", "alert('Error occured.. Please try again')", true);
                                //                chkCompleted.Checked = false;
                                //            }
                                //        }
                                //        else
                                //        {

                                //        }
                                //    }
                                //    else
                                //    {

                                //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "LineItem Error", "alert('OOPS! You missed to attach the invoice to EMS')", true);
                                //        chkCompleted.Checked = false;
                                //    }

                                //}
                                //else
                                //{

                                //}


                            }
                            else
                            {
                                chkCompleted.Checked = false;
                            }
                            return;
                        }

                        if (_columnIndex == 17)
                        {
                            if (chkSelect.Checked != true)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice", "alert('Please select invoice')", true);
                                return;
                            }
                            int numb=0;
                            string path=string.Empty; 
                              // path = @"D:\test\Namohar\2017\July\17072017\ATT Flowserve_ATT Mobility Premier E-Bill06272017_General.txt".ToString();
                 
                          //  path = @"\\10.80.20.251\InvoicesRepository\Convergys\Invoices\Flowserve Billing 07-31-2017\AT&T Mobility - June_2017\EIFC Output".ToString();
                         //  path= "\\\\10.80.20.251\\EDIRepository\\DHL\\AT&T Mobility\\JUNE 2017\\Namohar\\ATT Flowserve_ATT Mobility Premier E-Bill06272017_General.txt".ToString();
                           path = lblFile.Text;
                           //Upload file reading

                           string PathValid = path.Substring(path.LastIndexOf('_')+1);
                           if (PathValid == "Unknown ChgCats.csv")
                           {
                               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Enter Invoice No", "alert('This is Unknow file. Please enter the invoice count.')", true);
                               return;
                           }

                            //using (var impersonation = new ImpersonatedUser("Namohar.M", "CORP", "105728@tangoe"))
                            using (var impersonation = new ImpersonatedUser("Namohar.M", "CORP", "H@ri.123"))
                            {
                               // string filePath = "\\\\10.1.2.179\\source\\";
                                StreamWriter SW1;
                                FileIOPermission myPerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, path);
                                myPerm.Assert();
                                //SW1 = File.CreateText(path);
                          
                               var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                                {
                                    string line;
                                    int count = 0;
                                    string InvceNo = string.Empty;
                                   
                                    while ((line = streamReader.ReadLine()) != null)
                                    {
                                        if (line != "")
                                        {
                                            count++;
                                            // process the line
                                            string[] lines = line.Split('|');
                                            string InvoiceNumber = lines[2].ToString();

                                            InvceNo += InvoiceNumber + ",";

                                        }
                                    }

                                  
                                    objEMS.UpdateInvoiceNo(Convert.ToString(count), rowid.ToString());
                                    //objEMS.UpdateInvoiceNo(InvceNo.TrimEnd(','), rowid.ToString());
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Enter Invoice No", "alert('File uploaded successfully.')", true);

                                }
                            }

                           
               
                        }
                        if (chkSelect.Checked == true)
                        {

                            //Namohar Changed code for Grid refresh on 10/02/2017
                            if (search_textbox.Text == "")
                            {
                                GetAssignedInvoices();

                            }
                            else
                            {
                                SerachData();
                            }

                            if (_columnIndex != 17)
                            {
                                Control _displayControl = _gridView.Rows[_rowIndex].Cells[_columnIndex].Controls[1];
                                _displayControl.Visible = false;
                                Control _editControl = _gridView.Rows[_rowIndex].Cells[_columnIndex].Controls[3];
                                _editControl.Visible = true;
                                _gridView.Rows[_rowIndex].Cells[_columnIndex].Attributes.Clear();
                                ScriptManager.RegisterStartupScript(this, GetType(), "SetFocus",
                                            "document.getElementById('" + _editControl.ClientID + "').focus();", true);
                                if (_editControl is DropDownList && _displayControl is Label)
                                {
                                    try
                                    {
                                        ((DropDownList)_editControl).ClearSelection();
                                        ((DropDownList)_editControl).Items.FindByText(((Label)_displayControl).Text).Selected = true;
                                    }
                                    catch
                                    {
                                        ((DropDownList)_editControl).SelectedValue = ((Label)_displayControl).Text;
                                    }

                                }
                                if (_editControl is TextBox)
                                {
                                    ((TextBox)_editControl).Attributes.Add("onfocus", "this.select()");
                                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
                                }

                            } //
                        }

                        break;
                }

                EmployeesUpdatePanel.Update();
                //UpdatePanel2.Update();
                //CommentsUpdatePanel.Update();
            }
            catch (Exception ex)
            {
       
             //   WriteToFile(ex.Message);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Enter Invoice No", "alert('Invoice not found in the repository')", true);
              //  WriteToFile("abc");
               // GetAssignedInvoices();
            }
        }

        private void WriteToFile(string text)
        {
            string path = "C:\\EMSDBLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }
        //grid records selecting
        protected void gvIPInvoices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                LinkButton _singleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];

                string _jsSingle = ClientScript.GetPostBackClientHyperlink(_singleClickButton, "");


                if (Page.Validators.Count > 0)
                    _jsSingle = _jsSingle.Insert(11, "if(Page_ClientValidate())");
                checked
                {
                    for (int columnIndex = _firstEditCellIndex; columnIndex < e.Row.Cells.Count; columnIndex++)
                    {
                        string js = _jsSingle.Insert(_jsSingle.Length - 2, columnIndex.ToString());
                        e.Row.Cells[columnIndex].Attributes["onclick"] = js;
                        e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
                    }
                }


                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                CheckBox chkIssue = (CheckBox)e.Row.FindControl("chkIssue");
                CheckBox chkComp = (CheckBox)e.Row.FindControl("chkCompleted");
                DropDownList ddlGvUser = (DropDownList)e.Row.FindControl("ddlGvUser");


                dt = new DataTable();
                dt = objEMS.getUser("IP");
                if (dt.Rows.Count > 0)
                {
                    ddlGvUser.DataSource = dt;
                    ddlGvUser.DataTextField = "UL_User_Name";
                    ddlGvUser.DataValueField = "UL_ID";
                    ddlGvUser.DataBind();
                    ddlGvUser.Items.Insert(0, "--Select--");
                    ddlGvUser.SelectedIndex = 0;



                }
                if (lblStatus.Text == "IP_Asg")
                {
                    chkSelect.Checked = false;
                }
                else if (lblStatus.Text == "Rework")
                {
                    chkSelect.Checked = false;
                }
                else
                {
                    chkSelect.Checked = true;
                }
                if (lblStatus.Text == "IP_Issue")
                {
                    chkSelect.Checked = true;
                    chkIssue.Checked = true;
                }
                if (lblStatus.Text == "QC_Idle" || lblStatus.Text == "QC_Asg" || lblStatus.Text == "QC_Inp" || lblStatus.Text == "QC_Issue" || lblStatus.Text == "QC_Comp")
                {
                    chkSelect.Checked = true;
                    chkComp.Checked = true;
                }
                // check for condition             
                Label lblFile = (Label)e.Row.FindControl("lblFile");
                string filename = lblFile.Text.ToString();
                filename = filename.Substring(0, 3).ToString();
                if (filename == "IEG")
                {
                    e.Row.ToolTip = "please enter IEG invoice number with MMddyyyy format";
                }

                //dt.Columns.Add("Process", typeof(string));
                //DataRow dtrow = dt.NewRow();    // Create New Row
                //dtrow["Process"] = "Duplicates";
                //DataRow dtrow1 = dt.NewRow();
                //dtrow1["Process"] = "DNP"; //Bind Data to Columns

                //ddlprocess.DataSource = dt;
                //ddlprocess.DataTextField = "Process";
                //ddlprocess.DataBind();
                //ddlprocess.Items.Insert(0, "--Select--");
                //ddlprocess.SelectedIndex = 0;
            }
            EmployeesUpdatePanel.Update();
            //UpdatePanel2.Update();
            //CommentsUpdatePanel.Update();
        }
        //grid rows updation.
        protected void gvIPInvoices_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView _gridView = (GridView)sender;
            string key = "";
            string value = "";
            //string selectedText = "";

            string[] _columnKeys = new string[] { "select", "File", "Source", "Status", " Invoice#", "Notes", "Issue", "Completed", "IP Asg By", "IP Com By", "ErrorNo", "QC Asg By", "QC Com By", "Duplicates", "ReceivedDate", "btnRead" };
            Label lblStatus = (_gridView.Rows[e.RowIndex].FindControl("lblStatus") as Label);
            DropDownList ddlprocess = (DropDownList)_gridView.Rows[e.RowIndex].FindControl("ddlprocess");
            string selectedText = ddlprocess.SelectedItem.Text;//get text
            if (lblStatus.Text == "QC_Idle" || lblStatus.Text == "QC_Asg" || lblStatus.Text == "QC_Inp" || lblStatus.Text == "QC_Issue" || lblStatus.Text == "QC_Comp")
            {
                commentsFlag = 1;
                return;

            }
            commentsFlag = 0;
            if (e.RowIndex > -1)
            {
                checked
                {
                    for (int i = _firstEditCellIndex; i < _gridView.Columns.Count; i++)
                    {
                        if (i == 1)
                        {
                            continue;
                        }
                        if (i == 2)
                        {
                            continue;
                        }
                        if (i == 3)
                        {
                            continue;
                        }
                        if (i == 11)
                        {
                            if (Session["access"].ToString() == "4")
                            {
                                continue;
                            }
                        }
                        if (i == 4 || i == 5 || i == 10 || i == 12 || i == 13 || i == 14)
                        {
                            continue;
                        }
                        if (i == 7)
                        {
                            continue;
                        }
                        if (i == 8)
                        {
                            continue;
                        }
                        if (i == 9)
                        {
                            continue;
                        }
                        if (i == 15)
                        {
                            continue;
                        }
                        if (i == 16)
                        {
                            continue;
                        }
                        if (i == 17)
                        {
                            continue;
                        }

                        int rowid = Convert.ToInt32(_gridView.DataKeys[e.RowIndex].Value.ToString());
                        Control _displayControl = _gridView.Rows[e.RowIndex].Cells[i].Controls[1];
                        Control _editControl = _gridView.Rows[e.RowIndex].Cells[i].Controls[3];
                        //update duplicates and Dnp invoices.
                        if (selectedText != string.Empty && lblStatus.Text == "DNP" || lblStatus.Text == "Duplicate" || lblStatus.Text == "EDI" || lblStatus.Text == "Expedite" || lblStatus.Text == "Statement" || selectedText != "Select")
                        {
                            result = objEMS.UpdateIPProcess(rowid.ToString(), selectedText);
                        }
                        key = _columnKeys[i - _firstEditCellIndex];
                        if (_editControl.Visible)
                        {
                            if (_editControl is TextBox)
                            {
                                value = ((TextBox)_editControl).Text;
                            }
                            else if (_editControl is DropDownList)
                            {
                                // value = ((DropDownList)_editControl).SelectedValue;
                                value = ((DropDownList)_editControl).SelectedItem.Text;
                            }
                            if (((Label)_displayControl).Text != value)
                            {
                                bool result;
                                if (i == 6)
                                {
                                    result = objEMS.UpdateInvoiceNo(value, rowid.ToString());
                                    if (result == true)
                                    {
                                        //insert and update invoice number in logfile
                                        DataTable dt = objEMS.CheckingInvoiceNumber(rowid.ToString(), "IP", value);
                                        if (dt.Rows.Count > 0)
                                        {

                                            result = objEMS.UpdateRecordLog(rowid.ToString(), value, Session["username"].ToString());
                                        }
                                        else
                                        {
                                            result = objEMS.RecordLog(rowid.ToString(), "IP", "InvoiceNo", ((Label)_displayControl).Text, value, Session["username"].ToString());
                                        }

                                        //  result = objEMS.RecordLog(rowid.ToString(), "IP", "InvoiceNo", ((Label)_displayControl).Text, value, Session["username"].ToString());
                                        if (result == true)
                                        {
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Invoice No", "alert('Error occured.. Please try again')", true);

                                    }
                                }
                                if (i == 11)
                                {
                                    if (value == "--Select--")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select User", "alert('Please select user')", true);

                                    }
                                    else
                                    {
                                        result = objEMS.UpdateIPUser(rowid.ToString(), "IP_Asg", Session["username"].ToString(), value);
                                        if (result == true)
                                        {
                                            result = objEMS.RecordLog(rowid.ToString(), "IP", "IP_AsgTo", ((Label)_displayControl).Text, value, Session["username"].ToString());
                                            if (result == true)
                                            {
                                                result = false;
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Update User", "alert('Error occured while updating user.. Please try again')", true);
                                        }
                                    }

                                }

                                if (i == 15)
                                {

                                    //result = objEMS.UpdateIPUser(rowid.ToString(), selectedText, Session["username"].ToString(), value);
                                    result = objEMS.UpdateIPProcess(rowid.ToString(), selectedText);
                                    if (result == true)
                                    {
                                        result = objEMS.RecordLog(rowid.ToString(), selectedText, "", ((Label)_displayControl).Text, value, Session["username"].ToString());
                                        if (result == true)
                                        {
                                            result = false;
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Update User", "alert('Error occured while updating user.. Please try again')", true);
                                    }
                                }
                            }
                            e.NewValues.Add(key, value);
                            break;
                        }
                    }
                }

                _gridView.SelectedIndex = -1;


                //Namohar Changed code for Grid refresh on 10/02/2017
                if (search_textbox.Text == "")
                {
                    GetAssignedInvoices();
                }
                else
                {
                    SerachData();
                }
            }
            EmployeesUpdatePanel.Update();
            //UpdatePanel2.Update();
            //CommentsUpdatePanel.Update();
        }
        //render each row and column to grid view.
        protected override void Render(HtmlTextWriter writer)
        {
            // The client events for GridView1 were created in GridView1_RowDataBound
            foreach (GridViewRow r in gvIPInvoices.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    checked
                    {
                        for (int columnIndex = _firstEditCellIndex; columnIndex < r.Cells.Count; columnIndex++)
                        {
                            Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$ctl00", columnIndex.ToString());
                        }
                    }

                }
            }

            base.Render(writer);
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);

        }
        //updating comments.
        protected void btnYes_Click(object sender, EventArgs e)
        {
            if (commentsFlag == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QCInvoices", "alert('Invoice Sent for QC. You cannot modify')", true);
                return;
            }

            bool result;
            if (txtComments.Text.Trim().Length <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EmptyComments", "alert('Please enter comments')", true);
                mpePopUp.Show();
                return;
            }

            result = objEMS.UpdateComments(txtComments.Text.Replace("'", " "), lblPopTeam.Text, lblPopFileId.Text, Session["username"].ToString());
            if (result == true)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Comments", "alert('Comments updated')", true);
                txtComments.Text = string.Empty;
                lblPopFileId.Text = string.Empty;
                lblPopTeam.Text = string.Empty;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Comments Error", "alert('Error occured.. Please try again')", true);
                dt = new DataTable();
                dt = objEMS.GetComments(Convert.ToInt32(lblPopFileId.Text));
                if (dt.Rows.Count > 0)
                {
                    gvComments.DataSource = dt;
                    gvComments.DataBind();
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
                }
                else
                {
                    gvComments.DataSource = null;
                    gvComments.DataBind();
                }

                mpePopUp.Show();
            }
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {

        }
        DateTime Invoicedate;
        string IEG_invoice;
        string IEG;
        //get invoice details from actuall EMSDB server and update the invoice status in our local emsdb database.
        private void CheckandUpdateLineItem(string rowid)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@invoiceNo",lblgvInvoiceNo.Text ),           
            };
                bool result;
                dt = new DataTable();

                dt = objEMS.getLineItem(Convert.ToInt32(rowid));
                if (dt.Rows.Count > 0)
                {

                    SqlConnection con = new SqlConnection("Data Source=" + dt.Rows[0]["C_DataSource"].ToString() + ";Initial Catalog=" + dt.Rows[0]["C_DataBase"].ToString() + ";User ID=" + dt.Rows[0]["C_UserId"].ToString() + ";Password=" + dt.Rows[0]["C_Password"].ToString() + ";");
                    SqlCommand cmd;
                    SqlDataAdapter da;
                    IEG = dt.Rows[0]["C_DataBase"].ToString();
                    if (IEG == "EMS_INTEGRYS")
                    {
                        string Invoice = lblgvInvoiceNo.Text;
                        IEG_invoice = Invoice.Remove(Invoice.Length - 8);
                        string dateString = Invoice.Substring(Invoice.Length - 8);
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        //invoice number formate with date
                        string format = "MMddyyyy";
                        Invoicedate = DateTime.ParseExact(dateString, format, provider);

                        SQlQuery = "SELECT INVOICE_NUM, ATTACHMENT_NAME FROM IM_INVOICE IM WITH (NOLOCK) LEFT JOIN IM_INVOICE_ATTACHMENT IMA WITH (NOLOCK) ON IM.INVOICE_RID = IMA.INVOICE_FK where Invoice_Num ='" + IEG_invoice + "' and Invoice_Date='" + Invoicedate.ToString("yyyy-MM-dd") + "'";


                    }
                    else
                    {
                        SQlQuery = "SELECT INVOICE_NUM, ATTACHMENT_NAME FROM IM_INVOICE IM WITH (NOLOCK) LEFT JOIN IM_INVOICE_ATTACHMENT IMA WITH (NOLOCK) ON IM.INVOICE_RID = IMA.INVOICE_FK where Invoice_Num =@invoiceNo";
                    }
                    cmd = new SqlCommand();
                    cmd.CommandText = SQlQuery;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddRange(parameters);
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    //dt = objDB.ExecuteParamerizedSelectCommand(SQlQuery, CommandType.Text, parameters);
                    da.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {

                        result = objEMS.UpdateStatus("QC_Idle", "", Convert.ToString(rowid));
                        if (result == true)
                        {
                            GetAssignedInvoices();
                            result = false;
                            result = objEMS.RecordLog(rowid.ToString(), "IP", "Status", "IP_Inp", "QC_Idle", Session["username"].ToString());
                            if (result == true)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error3", "alert('Error occured.. Please try again')", true);
                            //chkCompleted.Checked = false;
                        }
                        //da = new SqlDataAdapter("SELECT IM.INVOICE_NUM, COUNT(INVOICE_LINE_ITEM_RID) AS LINE_ITEM_COUNT FROM IM_INVOICE IM WITH (NOLOCK) " +
                        //                                     "JOIN IM_INVOICE_INVENTORY INV WITH (NOLOCK) ON IM.INVOICE_RID = INV.INVOICE_FK " +
                        //                                    "JOIN IM_INVOICE_LINE_ITEM LIN WITH (NOLOCK) ON LIN.INVOICE_INV_FK = INV.INVOICE_INVENTORY_RID " +
                        //                                     "WHERE Invoice_Num = '" + lblgvInvoiceNo.Text + "' " +
                        //                               "GROUP BY IM.INVOICE_NUM", con);


                        SqlParameter[] parameters1 = new SqlParameter[]
                    {
                        new SqlParameter("@invoiceNo",lblgvInvoiceNo.Text ),           
                    };

                        if (IEG == "EMS_INTEGRYS")
                        {
                            SQlQuery = "SELECT IM.INVOICE_NUM, COUNT(INVOICE_LINE_ITEM_RID) AS LINE_ITEM_COUNT FROM IM_INVOICE IM WITH (NOLOCK) " +
                                                            "JOIN IM_INVOICE_INVENTORY INV WITH (NOLOCK) ON IM.INVOICE_RID = INV.INVOICE_FK " +
                                                           "JOIN IM_INVOICE_LINE_ITEM LIN WITH (NOLOCK) ON LIN.INVOICE_INV_FK = INV.INVOICE_INVENTORY_RID " +
                                                            "WHERE  Invoice_Num = '" + IEG_invoice + "' and Invoice_Date='" + Invoicedate.ToString("yyyy-MM-dd") + "' " +
                                                      "GROUP BY IM.INVOICE_NUM";
                        }
                        else
                        {
                            SQlQuery = "SELECT IM.INVOICE_NUM, COUNT(INVOICE_LINE_ITEM_RID) AS LINE_ITEM_COUNT FROM IM_INVOICE IM WITH (NOLOCK) " +
                                                                 "JOIN IM_INVOICE_INVENTORY INV WITH (NOLOCK) ON IM.INVOICE_RID = INV.INVOICE_FK " +
                                                                "JOIN IM_INVOICE_LINE_ITEM LIN WITH (NOLOCK) ON LIN.INVOICE_INV_FK = INV.INVOICE_INVENTORY_RID " +
                                                                 "WHERE Invoice_Num = @invoiceNo " +
                                                           "GROUP BY IM.INVOICE_NUM";
                        }
                        con.Open();
                        cmd = new SqlCommand();
                        cmd.CommandText = SQlQuery;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        cmd.Parameters.AddRange(parameters1);
                        //con.Open();
                        da = new SqlDataAdapter(cmd);

                        DataTable dt3 = new DataTable();
                        //dt3 = objDB.ExecuteParamerizedSelectCommand(SQlQuery, CommandType.Text, parameters);
                        da.Fill(dt3);
                        con.Close();

                        if (dt3.Rows.Count > 0)
                        {
                            result = objEMS.UpdateStatus("QC_Idle", dt3.Rows[0]["LINE_ITEM_COUNT"].ToString(), Convert.ToString(rowid));
                            if (result == true)
                            {
                                GetAssignedInvoices();
                                //result = false;
                                //result = objEMS.RecordLog(rowid.ToString(), "IP", "Status", "IP_Inp", "QC_Idle", Session["username"].ToString());
                                //if (result == true)
                                //{
                                //    result = false;
                                //}
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error3", "alert('Error occured.. Please try again')", true);
                                //chkCompleted.Checked = false;
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "LineItem Error", "alert('OOPS! You missed to attach the invoice to EMS')", true);

                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "InvNo Error", "alert('Error Occured while checking invoice.. Please try again')", true);
                }

                GetAssignedInvoices();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Exception", "alert('" + ex.Message + "')", true);
                GetAssignedInvoices();
            }

        }

        //search data from data base
        public void SerachData()
        {
            
            dt = new DataTable();
            if (Flag == 1)
            {
                dt = objEMS.SearchDataByReceivedDate_EDI(Session["access"].ToString(), Session["username"].ToString(), ReceiveDate);
            }
            else
            {
                dt = objEMS.SearchData_EDI(Session["access"].ToString(), Session["username"].ToString(), Session["Query"].ToString());
            }
            if (dt.Rows.Count > 0)
            {

                gvIPInvoices.DataSource = dt;
                gvIPInvoices.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 700, 1450 , 40 ,true); </script>", false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('No Records were found for this criteria')", true);
                GetAssignedInvoices();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 700, 1450 , 40 ,true); </script>", false);
            }
        }
        DateTime ReceiveDate;
        // Search data based invoice, IPAsgTo,QcAsgTo and Status
        protected void btnSearch_Click1(object sender, EventArgs e)
        {
            try
            {
                string ddltext = ddlList.SelectedItem.Text;
                if (ddltext == "Select field")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('Please select field')", true);
                }
                else
                {
                    string txt = search_textbox.Text;
                    if (txt != "")
                    {
                        switch (ddltext)
                        {
                            case "Invoice":
                                Query = "IND_InvoiceNo like '%" + txt;
                                break;
                            case "IP Asg To":
                                Query = "IND_IP_Processed_By like '%" + txt;
                                break;
                            case "QC Asg To":
                                Query = "IND_QC_Processed_By like '%" + txt;
                                break;
                            case "Status":
                                Query = "IND_Status like '%" + txt;
                                break;
                            case "Client":
                                Query = "FI_FileName like '%" + txt;
                                break;
                            case "Source":
                                Query = "FI_Source like '%" + txt;
                                break;
                            case "ReceiveDate":
                                Flag = 1;
                                DateTime Test;
                                if (DateTime.TryParseExact(txt, "MM/dd/yyyy", null, DateTimeStyles.None, out Test) == true)
                                {
                                    ReceiveDate = Convert.ToDateTime(txt.ToString());
                                }
                                else
                                {
                                    GetAssignedInvoices();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('Date formate should be MM/dd/yyyy')", true);
                                    return;
                                }

                                Query = "FI_ReceiptDate =" + ReceiveDate + "";
                                break;
                        }

                        Session["Query"] = Query;
                        SerachData();

                    }
                    else
                    {
                        GetAssignedInvoices();
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void btnReset_Click1(object sender, EventArgs e)
        {
            search_textbox.Text = string.Empty;
            ddlList.SelectedIndex = 0;
            GetAssignedInvoices();
        }

        protected void search_textbox_TextChanged(object sender, EventArgs e)
        {

            try
            {
                string ddltext = ddlList.SelectedItem.Text;
                if (ddltext == "Select field")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('Please select field')", true);
                }
                else
                {
                    string txt = search_textbox.Text;
                    if (txt != "")
                    {
                        switch (ddltext)
                        {
                            case "Invoice":
                                Query = "IND_InvoiceNo like '%" + txt;
                                break;
                            case "IP Asg To":
                                Query = "IND_IP_Processed_By like '%" + txt;
                                break;
                            case "QC Asg To":
                                Query = "IND_QC_Processed_By like '%" + txt;
                                break;
                            case "Status":
                                Query = "IND_Status like '%" + txt;
                                break;
                            case "Client":
                                Query = "FI_FileName like '%" + txt;
                                break;
                            case "Source":
                                Query = "FI_Source like '%" + txt;
                                break;
                            case "ReceiveDate":
                                Flag = 1;
                                DateTime Test;
                                if (DateTime.TryParseExact(txt, "MM/dd/yyyy", null, DateTimeStyles.None, out Test) == true)
                                {
                                    ReceiveDate = Convert.ToDateTime(txt.ToString());
                                }
                                else
                                {
                                    GetAssignedInvoices();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('Date formate should be MM/dd/yyyy')", true);
                                    return;
                                }

                                Query = "FI_ReceiptDate =" + ReceiveDate + "";
                                break;
                        }

                        Session["Query"] = Query;
                        SerachData();

                    }
                    else
                    {
                        GetAssignedInvoices();
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}