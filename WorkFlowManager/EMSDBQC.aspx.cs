using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Data.SqlClient;
using DAL;
using System.Globalization;

namespace WorkFlowManager
{
    public partial class EMSDBQC : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        //get emsdb sql queries from clsEMSDB class.
        clsEMSDB objEMS = new clsEMSDB();
        //data base connection class.
        SqlDBHelper objDB = new SqlDBHelper();
        string SQlQuery;
        bool result;
        string oldInvoice;
        string newInvoice;
        string Query;
        private const int _firstEditCellIndex = 2;
        int Flag = 0;
        int commentsFlag = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            //user id is storing in session, if session in null or time out it will redirect to the Default.aspx.
            if (Context.Session != null)
            {
                if (Session.IsNewSession)
                {
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
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
            if (!IsPostBack)
            {
                //page load.  get invoice's which are not proccessed by QC team, once process completed data will not appear in the page  and diplayed in the grid  
                GetAssignedInvoices();

                EmployeesUpdatePanel.Update();
                //UpdatePanel2.Update();
                CommentsUpdatePanel.Update();
            }

            if (this.gvIPInvoices.SelectedIndex > -1)
            {
                // Call UpdateRow on every postback
                this.gvIPInvoices.UpdateRow(this.gvIPInvoices.SelectedIndex, false);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
            }
        }
        // get invoice's which are not proccessed by QC team, once process completed data will not appear in the page  and diplayed in the grid 
        private void GetAssignedInvoices()
        {

            dt = new DataTable();
            dt = objEMS.GetAssignedQCInvoices(Session["access"].ToString(), Session["username"].ToString());
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
                    CheckBox chkReturn = (CheckBox)_gridView.Rows[_rowIndex].Cells[15].FindControl("chkReturn");
                    Label lblInvoiceNo = (Label)_gridView.Rows[_rowIndex].Cells[6].FindControl("lblInvoiceNo");
                    Label lblStatus = (Label)_gridView.Rows[_rowIndex].Cells[5].FindControl("lblStatus");
                    Label Notes = (Label)_gridView.Rows[_rowIndex].Cells[7].FindControl("Notes");

                    int _columnIndex = int.Parse(Request.Form["__EVENTARGUMENT"]);
                    _gridView.SelectedIndex = _rowIndex;
                    if (_columnIndex == 3)
                    {

                        if (chkSelect.Checked == true)
                        {
                            string url = "ViewFile.aspx?id=" + rowid;
                            string s = "window.open('" + url + "', 'popup_window', 'width=1000,height =600, top=100,left=100,resizable=yes');";
                         //   ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", s, true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice", "alert('Please select invoice')", true);
                            return;
                        }
                        return;
                    }


                    if (lblStatus.Text == "IP_Asg" || lblStatus.Text == "IP_Inp" || lblStatus.Text == "IP_Issue" || lblStatus.Text == "Rework")
                    {


                        chkSelect.Checked = true;
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QCInvoices", "alert('Invoice Sent to IP. You cannot modify')", true);
                        GetAssignedInvoices();
                        //  return;
                    }

                    if (_columnIndex == 2)
                    {
                        result = objEMS.UpdateStatus("QC_Inp", "", Convert.ToString(rowid));
                        if (result == true)
                        {
                            //RecordLog
                            result = objEMS.RecordLog(rowid.ToString(), "QC", "Status", ((Label)lblStatus).Text, "QC_Inp", Session["username"].ToString());
                            GetAssignedInvoices();
                            result = false;
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error", "alert('Error occured.. Please try again')", true);
                        }
                        return;
                    }

                    if (_columnIndex == 4 || _columnIndex == 5 || _columnIndex == 10 || _columnIndex == 11 || _columnIndex == 13)
                    {
                        return;
                    }

                    if (_columnIndex == 14)
                    {
                        if (Session["access"].ToString() == "4")
                        {
                            return;
                        }
                    }
                    if (_columnIndex == 7)
                    {
                        if (chkSelect.Checked == true)
                        {
                            lblPopTeam.Text = "QC";
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
                            mpePopUp.Show();

                        }

                        return;
                    }
                    if (_columnIndex == 8)
                    {
                        if (chkCompleted.Checked == true || chkReturn.Checked==true)
                        {
                            chkIssue.Checked = false;                          
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Hello", "alert('Invoice status can not modify');", true);
                            return;
                        }
                        if (chkSelect.Checked == true)
                        {

                            result = objEMS.UpdateStatus("QC_Issue", "", Convert.ToString(rowid));
                            if (result == true)
                            {
                                //RecordLog
                                result = objEMS.RecordLog(rowid.ToString(), "QC", "Issue", ((CheckBox)chkIssue).Text, "QC_Issue", Session["username"].ToString());
                                GetAssignedInvoices();
                                result = false;
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
                    if (_columnIndex == 15)
                    {

                        if (chkSelect.Checked == true)
                        {
                            if (chkCompleted.Checked == true)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Hello", "alert('Invoice status can not modify');", true);
                                chkReturn.Checked = false;
                                return;
                            }
                            result = objEMS.UpdateStatus("Rework", "", Convert.ToString(rowid));
                            if (result == true)
                            {
                                //RecordLog
                                result = objEMS.RecordLog(rowid.ToString(), "QC", "Status", ((Label)lblStatus).Text, "Rework", Session["username"].ToString());
                                GetAssignedInvoices();
                                result = false;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Rework", "alert('Error occured.. Please try again')", true);
                                chkReturn.Checked = false;
                            }
                        }
                        else
                        {
                            chkReturn.Checked = false;
                        }
                        return;
                    }
                    if (_columnIndex == 9)
                    {
                        if (chkReturn.Checked == true)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Hello", "alert('Invoice status can not modify');", true);
                          
                            return;
                        }

                        if (chkSelect.Checked == true)
                        {
                            //if (chkCompleted.Checked == true)
                            //{
                            //    chkCompleted.Checked = true;
                            //    return;
                            //}
                            dt = new DataTable();
                            dt = objEMS.getLineItem(rowid);
                            if (dt.Rows.Count > 0)
                            {
                                SqlConnection con = new SqlConnection("Data Source=" + dt.Rows[0]["C_DataSource"].ToString() + ";Initial Catalog=" + dt.Rows[0]["C_DataBase"].ToString() + ";User ID=" + dt.Rows[0]["C_UserId"].ToString() + ";Password=" + dt.Rows[0]["C_Password"].ToString() + ";");

                                DateTime Invoicedate;
                                SqlDataAdapter da;
                                string IEG_invoice;
                                string IEG = dt.Rows[0]["C_DataBase"].ToString();
                                if (IEG == "EMS_INTEGRYS")
                                {
                                    string Invoice = lblInvoiceNo.Text;
                                    IEG_invoice = Invoice.Remove(Invoice.Length - 8);
                                    string dateString = Invoice.Substring(Invoice.Length - 8);
                                    CultureInfo provider = CultureInfo.InvariantCulture;
                                    //invoice number formate with date
                                    string format = "MMddyyyy";
                                    Invoicedate = DateTime.ParseExact(dateString, format, provider);

                                    da = new SqlDataAdapter("Select [Status] from IM_Invoice where Invoice_Num ='" + IEG_invoice + "' and Invoice_Date='" + Invoicedate.ToString("yyyy-MM-dd") + "'", con);
                                }
                                else
                                {
                                    da = new SqlDataAdapter("Select [Status] from IM_Invoice where Invoice_Num = '" + lblInvoiceNo.Text + "'", con);


                                }
                                da.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    lblPop2FileId.Text = rowid.ToString();
                                    lblInvoiceStatus.Text = "Invoice Status is " + dt.Rows[0]["Status"].ToString() + ". Do you wish to continue?";
                                    //lblInvoiceStatus.Text = "Invoice Status is " +  ". Do you wish to continue?";
                                    // mpeStatus.Show();
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "LineItem Error", "alert('OOPS! You missed to attach the invoice to EMS')", true);
                                    return;
                                }
                            }
                            else
                            {

                            }
                            result = objEMS.UpdateStatus("QC_Comp", "", Convert.ToString(rowid));
                            if (result == true)
                            {
                                result = objEMS.RecordLog(rowid.ToString(), "QC", "Status", ((Label)lblStatus).Text, "QC_Comp", Session["username"].ToString());
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Status", "alert('Invoice successfully Completed')", true);
                                GetAssignedInvoices();
                                result = false;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error3", "alert('Error occured.. Please try again')", true);
                                chkCompleted.Checked = false;
                            }
                        }
                        else
                        {
                            chkCompleted.Checked = false;
                        }
                        return;
                    }
                    if (chkSelect.Checked == true)
                    {
                        if (chkCompleted.Checked == true)
                        {
                            chkIssue.Checked = false;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Hello", "alert('Invoice number can not modify');", true);
                            return;
                        }
                      //  GetAssignedInvoices();

                        //Namohar Changed code for Grid refresh on 10/02/2017
                        if (search_textbox.Text == "")
                        {
                            GetAssignedInvoices();

                        }
                        else
                        {
                            if (ddlList.SelectedItem.Text == "ReceiveDate")
                            {
                                Flag = 1;
                            }
                            else
                            {
                                Flag = 0;
                            }
                            SerachData();
                        }
                      
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
                        }

                    }
                    break;
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
                CheckBox chkReturn = (CheckBox)e.Row.FindControl("chkReturn");
                CheckBox chkCompleted = (CheckBox)e.Row.FindControl("chkCompleted");
                DropDownList ddlGvUser = (DropDownList)e.Row.FindControl("ddlGvUser");

                dt = new DataTable();
                dt = objEMS.getUser("QC");
                if (dt.Rows.Count > 0)
                {
                    ddlGvUser.DataSource = dt;
                    ddlGvUser.DataTextField = "UL_User_Name";
                    ddlGvUser.DataValueField = "UL_ID";
                    ddlGvUser.DataBind();
                }
                if (lblStatus.Text == "QC_Asg")
                {
                    chkSelect.Checked = false;
                }
                else if (lblStatus.Text == "QC_Idle")
                {
                    chkSelect.Checked = false;
                }
                else
                {
                    chkSelect.Checked = true;
                }
                if (lblStatus.Text == "QC_Issue")
                {
                    chkSelect.Checked = true;
                    chkIssue.Checked = true;
                }
                if (lblStatus.Text == "Rework")
                {
                    chkSelect.Checked = true;
                    chkReturn.Checked = true;
                }

                if (lblStatus.Text == "QC_Comp")
                {
                    chkSelect.Checked = true;
                    chkCompleted.Checked = true;
                }
                //if (lblStatus.Text == "QC_Idle" || lblStatus.Text == "QC_Asg" || lblStatus.Text == "QC_Inp" || lblStatus.Text == "QC_Issue" || lblStatus.Text == "QC_Comp")
                //{
                //    chkSelect.Checked = true;
                //    chkComp.Checked = true;
                //}
            
            }
            EmployeesUpdatePanel.Update();
            //UpdatePanel2.Update();
            CommentsUpdatePanel.Update();
        }
        //grid rows updation.
        protected void gvIPInvoices_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView _gridView = (GridView)sender;
            string key = "";
            string value = "";
            //string selectedText = "";
            Label lblStatus = (_gridView.Rows[e.RowIndex].FindControl("lblStatus") as Label);

            //if (lblStatus.Text == "QC_Comp" || lblStatus.Text == "IP_Inp" || lblStatus.Text == "IP_Issue" || lblStatus.Text == "Rework")
            //{
            //    return;
            //}
            string[] _columnKeys = new string[] { "select", "File", "Source", "Status", " Invoice#", "Notes", "Issue", "Completed", "IP Asg By", "IP Com By", "ErrorNo", "QC Asg By", "QC Com By", "Back To IP" };
            
            if (lblStatus.Text == "IP_Asg" || lblStatus.Text == "Rework")
            {
                return;
            }
            if (e.RowIndex > -1)
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
                    if (i == 4 || i == 5 || i == 10 || i == 11 || i == 13 || i == 15)
                    {
                        continue;
                    }
                    if (i == 14)
                    {
                        if (Session["access"].ToString() == "4")
                        {
                            continue;
                        }
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


                    int rowid = Convert.ToInt32(_gridView.DataKeys[e.RowIndex].Value.ToString());
                    Control _displayControl = _gridView.Rows[e.RowIndex].Cells[i].Controls[1];
                    Control _editControl = _gridView.Rows[e.RowIndex].Cells[i].Controls[3];
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
                            if (i == 6)
                            {
                                oldInvoice = ((Label)_displayControl).Text;
                                newInvoice = ((TextBox)_editControl).Text;

                                if (value.Length <= 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Enter Invoice No", "alert('Please enter valid invoice#')", true);

                                    return;
                                }
                                lblgvRowId.Text = rowid.ToString();
                                lblgvInvoiceNo.Text = value;

                                string script = "$(document).ready(function () { $('[id*=btnHidden]').click(); });";
                                ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);

                                //  dt = new DataTable();
                                //  dt = objEMS.getLineItem(rowid);
                                //  if (dt.Rows.Count > 0)
                                //  {
                                //      SqlConnection con = new SqlConnection("Data Source=" + dt.Rows[0]["C_DataSource"].ToString() + ";Initial Catalog=" + dt.Rows[0]["C_DataBase"].ToString() + ";User ID=" + dt.Rows[0]["C_UserId"].ToString() + ";Password=" + dt.Rows[0]["C_Password"].ToString() + ";");

                                //      SqlDataAdapter da = new SqlDataAdapter("SELECT INVOICE_NUM, ATTACHMENT_NAME FROM IM_INVOICE IM WITH (NOLOCK) LEFT JOIN IM_INVOICE_ATTACHMENT IMA WITH (NOLOCK) ON IM.INVOICE_RID = IMA.INVOICE_FK where Invoice_Num = '1189798'", con);

                                //      dt = new DataTable();

                                //      da.Fill(dt);

                                //      if (dt.Rows.Count > 0)
                                //      {
                                //          da = new SqlDataAdapter("SELECT IM.INVOICE_NUM, COUNT(INVOICE_LINE_ITEM_RID) AS LINE_ITEM_COUNT FROM IM_INVOICE IM WITH (NOLOCK) " +
                                //                                               "JOIN IM_INVOICE_INVENTORY INV WITH (NOLOCK) ON IM.INVOICE_RID = INV.INVOICE_FK " +
                                //                                              "JOIN IM_INVOICE_LINE_ITEM LIN WITH (NOLOCK) ON LIN.INVOICE_INV_FK = INV.INVOICE_INVENTORY_RID " +
                                //                                               "WHERE Invoice_Num = '1189798' " +
                                //                                         "GROUP BY IM.INVOICE_NUM", con);
                                //          DataTable dt3 = new DataTable();
                                //          da.Fill(dt3);

                                //          if (dt3.Rows.Count > 0)
                                //          {
                                //              result = objEMS.UpdateInvoiceNo(value, rowid.ToString());
                                //              //result = objEMS.UpdateStatus("QC_Idle", dt3.Rows[0]["LINE_ITEM_COUNT"].ToString(), Convert.ToString(rowid));
                                //              if (result == true)
                                //              {
                                //                  result = false;
                                //                  result = objEMS.UpdateLineItemCount(dt3.Rows[0]["LINE_ITEM_COUNT"].ToString(), rowid.ToString());
                                //                  if (result == true)
                                //                  {
                                //                      ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Invoice No Success", "alert('Invoice# updated Successfully')", true);
                                //                  }
                                //                  else
                                //                  {
                                //                      ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Invoice No Failure", "alert('Error occured while updating line item count.. Please try again')", true);
                                //                  }
                                //                  //GetAssignedInvoices();
                                //                  //result = false;
                                //              }
                                //              else
                                //              {
                                //                  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Invoice No", "alert('Error occured while updating invoice#.. Please try again')", true);
                                //              }
                                //          }
                                //      }
                                //      else
                                //      {
                                //          ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "LineItem Error", "alert('OOPS! You missed to attach the invoice to EMS')", true);
                                //      }
                                //}

                            }
                            else if (i == 12)
                            {
                                bool result;
                                result = objEMS.UpdateErrorNo(value, rowid.ToString());
                                if (result == true)
                                {
                                    //RecordLog
                                    result = objEMS.RecordLog(rowid.ToString(), "QC", "Error No", ((Label)_displayControl).Text, value, Session["username"].ToString());
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Error No", "alert('Error occured.. Please try again')", true);
                                }
                            }
                            else if (i == 14)
                            {
                                bool result;
                                if (value == "--Select--")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select User", "alert('Please select user')", true);
                                }
                                else
                                {
                                    result = objEMS.QCAssign(rowid.ToString(), "QC_Asg", Session["username"].ToString(), value, DateTime.Now.ToString());
                                    if (result == true)
                                    {
                                        //RecordLog
                                        result = objEMS.RecordLog(rowid.ToString(), "QC", "QC_AsgTo", ((Label)_displayControl).Text, value, Session["username"].ToString());
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
                        }
                        e.NewValues.Add(key, value);
                        break;
                    }
                }
                _gridView.SelectedIndex = -1;
                //GetAssignedInvoices();
                //Namohar Changed code for Grid refresh on 10/02/2017
                if (search_textbox.Text == "")
                {
                    GetAssignedInvoices();
                }
                else
                {
                    if (ddlList.SelectedItem.Text == "ReceiveDate")
                    {
                        Flag = 1;
                    }
                    else
                    {
                        Flag = 0;
                    }
                    SerachData();
                }
                EmployeesUpdatePanel.Update();
                //UpdatePanel2.Update();
               // CommentsUpdatePanel.Update();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // The client events for GridView1 were created in GridView1_RowDataBound
            foreach (GridViewRow r in gvIPInvoices.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    for (int columnIndex = _firstEditCellIndex; columnIndex < r.Cells.Count; columnIndex++)
                    {
                        Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$ctl00", columnIndex.ToString());
                    }
                }
            }

            base.Render(writer);
            //   ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 900 , 40 ,true); </script>", false); 
        }
        //get invoice details from actuall EMSDB server and update the invoice status in our local emsdb database.
        private void CheckandUpdateLineItem(string rowid)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@invoiceNo",  lblgvInvoiceNo.Text ),
           };
                bool result;
                dt = new DataTable();
                //get the EMSDB server credentials.
                dt = objEMS.getLineItem(Convert.ToInt32(rowid));
                if (dt.Rows.Count > 0)
                {
                    SqlConnection con = new SqlConnection("Data Source=" + dt.Rows[0]["C_DataSource"].ToString() + ";Initial Catalog=" + dt.Rows[0]["C_DataBase"].ToString() + ";User ID=" + dt.Rows[0]["C_UserId"].ToString() + ";Password=" + dt.Rows[0]["C_Password"].ToString() + ";");
                    SqlCommand cmd;
                    SqlDataAdapter da;
                    //SqlDataAdapter da = new SqlDataAdapter("SELECT INVOICE_NUM, ATTACHMENT_NAME FROM IM_INVOICE IM WITH (NOLOCK) LEFT JOIN IM_INVOICE_ATTACHMENT IMA WITH (NOLOCK) ON IM.INVOICE_RID = IMA.INVOICE_FK where Invoice_Num = '" + lblgvInvoiceNo.Text + "'", con);
                    dt = new DataTable();
                    //da.Fill(dt);
                    SQlQuery = "SELECT INVOICE_NUM, ATTACHMENT_NAME FROM IM_INVOICE IM WITH (NOLOCK) LEFT JOIN IM_INVOICE_ATTACHMENT IMA WITH (NOLOCK) ON IM.INVOICE_RID = IMA.INVOICE_FK where Invoice_Num =@invoiceNo";
                    cmd = new SqlCommand();
                    cmd.CommandText = SQlQuery;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddRange(parameters);
                    con.Open();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    con.Close();
                    //dt = objDB.ExecuteParamerizedSelectCommand(SQlQuery, CommandType.Text, parameters);
                    if (dt.Rows.Count > 0)
                    {
                        SqlParameter[] parameters1 = new SqlParameter[]
                   {
                       new SqlParameter("@invoiceNo",  lblgvInvoiceNo.Text ),
                   };
                        //da = new SqlDataAdapter("SELECT IM.INVOICE_NUM, COUNT(INVOICE_LINE_ITEM_RID) AS LINE_ITEM_COUNT FROM IM_INVOICE IM WITH (NOLOCK) " +
                        //                                     "JOIN IM_INVOICE_INVENTORY INV WITH (NOLOCK) ON IM.INVOICE_RID = INV.INVOICE_FK " +
                        //                                    "JOIN IM_INVOICE_LINE_ITEM LIN WITH (NOLOCK) ON LIN.INVOICE_INV_FK = INV.INVOICE_INVENTORY_RID " +
                        //                                     "WHERE Invoice_Num = '" + lblgvInvoiceNo.Text + "' " +
                        //                               "GROUP BY IM.INVOICE_NUM", con);
                        DataTable dt3 = new DataTable();
                        //da.Fill(dt3);
                        SQlQuery = "SELECT IM.INVOICE_NUM, COUNT(INVOICE_LINE_ITEM_RID) AS LINE_ITEM_COUNT FROM IM_INVOICE IM WITH (NOLOCK) " +
                                    "JOIN IM_INVOICE_INVENTORY INV WITH (NOLOCK) ON IM.INVOICE_RID = INV.INVOICE_FK " +
                                    "JOIN IM_INVOICE_LINE_ITEM LIN WITH (NOLOCK) ON LIN.INVOICE_INV_FK = INV.INVOICE_INVENTORY_RID " +
                                    "WHERE Invoice_Num =@invoiceNo " +
                                    "GROUP BY IM.INVOICE_NUM";

                        cmd = new SqlCommand();
                        cmd.CommandText = SQlQuery;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        cmd.Parameters.AddRange(parameters1);
                        con.Open();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt3);
                        con.Close();

                        //dt3 = objDB.ExecuteParamerizedSelectCommand(SQlQuery, CommandType.Text, parameters);

                        if (dt3.Rows.Count > 0)
                        {
                            result = objEMS.UpdateInvoiceNo(lblgvInvoiceNo.Text, rowid.ToString());
                            //RecordLog
                            result = objEMS.RecordLog(rowid.ToString(), "QC", "InvoiceNo", oldInvoice, newInvoice, Session["username"].ToString());
                            //result = objEMS.UpdateStatus("QC_Idle", dt3.Rows[0]["LINE_ITEM_COUNT"].ToString(), Convert.ToString(rowid));
                            if (result == true)
                            {
                                result = false;
                                result = objEMS.UpdateLineItemCount(dt3.Rows[0]["LINE_ITEM_COUNT"].ToString(), rowid.ToString());
                                if (result == true)
                                {

                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Invoice No Success", "alert('Invoice# updated Successfully')", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Invoice No Failure", "alert('Error occured while updating line item count.. Please try again')", true);
                                }
                                //GetAssignedInvoices();
                                //result = false;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Invoice No", "alert('Error occured while updating invoice#.. Please try again')", true);
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
                GetAssignedInvoices();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Exception", "alert('" + ex.Message + "')", true);
                GetAssignedInvoices();
            }


        }
        //update the comments and store in database 
        protected void btnYes_Click(object sender, EventArgs e)
        {
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
                int id = Convert.ToInt32(lblPopFileId.Text);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Comments", "alert('Comments updated')", true);
                txtComments.Text = string.Empty;
                lblPopFileId.Text = string.Empty;
                lblPopTeam.Text = string.Empty;
                dt = new DataTable();
             
                dt = objEMS.GetComments(id);
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
                CommentsUpdatePanel.Update();
                mpePopUp.Show();
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
                CommentsUpdatePanel.Update();
                mpePopUp.Show();
            }
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            txtComments.Text = "";
        }
        //update the invoice status and bind it to grid view.
        protected void btnContinue_Click(object sender, EventArgs e)
        {
            bool result;

            result = objEMS.UpdateStatus("QC_Comp", "", lblPop2FileId.Text);
            if (result == true)
            {
                //RecordLog
                GetAssignedInvoices();
                result = false;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select Invoice Error3", "alert('Error occured.. Please try again')", true);
                //chkCompleted.Checked = false;
            }
        }
        // get invoice's which are not proccessed by QC team and diplayed in the grid 
        protected void btnBack_Click(object sender, EventArgs e)
        {
            GetAssignedInvoices();
        }
        //get invoice details from actuall EMSDB server and update the invoice status in our local emsdb database.
        protected void btnHidden_Click(object sender, EventArgs e)
        {
            if (lblgvRowId.Text.Length <= 0)
            {
                return;
            }

            // System.Threading.Thread.Sleep(5000);

            CheckandUpdateLineItem(lblgvRowId.Text);

            lblgvRowId.Text = string.Empty;
            lblgvInvoiceNo.Text = string.Empty;

        }

        protected void gvIPInvoices_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // Search data based invoice, IPAsgTo,QcAsgTo and Status
        DateTime ReceiveDate;
        protected void btnSearch_Click(object sender, EventArgs e)
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
                            if (txt == "Rework")
                            {
                                Query = "IND_Status like '%" + txt;
                            }
                            else if (txt.Substring(0, 2) != "QC")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('No Records were found for QC status criteria')", true);
                                return;
                            }
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
                                Session["ReceiveDate"] = ReceiveDate;
                            }
                            else
                            {
                                GetAssignedInvoices();
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('Date format should be  MM/DD/YYYY')", true);
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

        //search data from data base
        public void SerachData()
        {
            dt = new DataTable();
            if (Flag == 1)
            {
                dt = objEMS.SearchDataByReceivedDate(Session["access"].ToString(), Session["username"].ToString(), Convert.ToDateTime(Session["ReceiveDate"]));
            }
            else
            {
                dt = objEMS.SearchData(Session["access"].ToString(), Session["username"].ToString(), Session["Query"].ToString());
            }
            if (dt.Rows.Count > 0)
            {

                gvIPInvoices.DataSource = dt;
                gvIPInvoices.DataBind();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Select field", "alert('No Records were found for this criteria')", true);
                GetAssignedInvoices();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvIPInvoices.ClientID + "', 500, 1450 , 40 ,true); </script>", false);
            }
        }

        protected void btnReset_Click1(object sender, EventArgs e)
        {
            search_textbox.Text = string.Empty;
            ddlList.SelectedIndex = 0;
            GetAssignedInvoices();
        }
        }

        
    }
