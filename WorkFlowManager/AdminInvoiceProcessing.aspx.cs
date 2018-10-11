using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Text;

namespace WorkFlowManager
{
    public partial class AdminInvoiceProcessing : System.Web.UI.Page
    {
        //get EMSDB Sql query's from ClsEMSDB class.
        clsEMSDB objEMS = new clsEMSDB();
        DataTable dt = new DataTable();
        int counter = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            if (!IsPostBack)
            {
                
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvInvoices.ClientID + "', 500, 900 , 40 ,true); </script>", false);
                //get clients in EMSDB_Clients table.
                FetchClients();
                //get user's based on IP or QA team for binding into invoice's assign dropdown. 
                BindUser();
                //get total invoice's recieved 
                TotalInvoicesRecieved();
                //get total invoices assigned to Users
                TotalInvoicesAssigned();
                if (ddlClient.SelectedIndex==0)
                {
                    BindGrid();
                }
              
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvInvoices.ClientID + "', 500, 900 , 40 ,true); </script>", false);
        }
        //get clients in EMSDB_Clients table.
        private void FetchClients()
        {
            dt = objEMS.getClients(Session["EMSTeam"].ToString());
            if (dt.Rows.Count > 0)
            {
                ddlClient.DataSource = dt;
                ddlClient.DataTextField = "C_Name";
                ddlClient.DataValueField = "C_Code";
                ddlClient.DataBind();
                ddlClient.Items.Insert(0, "--Select Client--");
                ddlClient.SelectedIndex = 0;
            }
        }
        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = String.Empty;
            if (ddlClient.SelectedIndex == 0)
            {
                gvInvoices.DataSource = null;
                gvInvoices.DataBind();

                BindGrid();
                //get total invoice's recieved 
                TotalInvoicesRecieved();
                //get total invoices assigned to Users
                TotalInvoicesAssigned();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvInvoices.ClientID + "', 500, 900 , 40 ,true); </script>", false);
            }
            else
            {
                //bind data to grid 
                BindGrid();
                TotalInvoicesRecieved();
                TotalInvoicesAssigned();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>MakeStaticHeader('" + gvInvoices.ClientID + "', 500, 900 , 40 ,true); </script>", false);
            }
        }
        //get total invoice's recieved 
        private void TotalInvoicesRecieved()
        {
            dt = new DataTable();
            if (ddlClient.SelectedIndex == 0)
            {
            
                dt = objEMS.GetTotalInvoicesRecieved("all");
            }
            else
            {
                dt = objEMS.GetTotalInvoicesRecieved(ddlClient.SelectedValue);
            }
            if (dt.Rows.Count >0 )
            {
                txtTotal.Text = dt.Rows[0]["total"].ToString();
                if (Convert.ToInt32(txtTotal.Text) < 1)
                {
                    txtTotal.Text = dt.Rows[0]["total"].ToString();
                    lblError.Text = "No Invoices received today";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
           
        }
        //get total invoices assigned to Users
        private void TotalInvoicesAssigned()
        {
            dt = new DataTable();
            if (ddlClient.SelectedIndex == 0)
            {
                dt = objEMS.GetTotalInvoicesAssigned("all");
            }
            else
            {
                dt = objEMS.GetTotalInvoicesAssigned(ddlClient.SelectedValue);
            }
            if (dt.Rows.Count > 0)
            {
               txtAssigned.Text = dt.Rows[0]["total"].ToString();
               //int i=counter;
               //txtAssigned.Text = i.ToString();
            }
        }

        //bind invoce details data to grid.
        private void BindGrid()
        {
         
            dt = new DataTable();
            if (ddlClient.SelectedIndex == 0)
            {
                dt = objEMS.getInvoices("all", Session["EMSTeam"].ToString());
            }
            else
            {

                dt = objEMS.getInvoices(ddlClient.SelectedValue, Session["EMSTeam"].ToString());
            }

            if (dt.Rows.Count > 0)
            {
                
                gvInvoices.DataSource = dt;
                gvInvoices.DataBind();
            }
            else
            {
                gvInvoices.DataSource = null;
                gvInvoices.DataBind();
        
            }
        }
        //get user's based on IP or QA team for binding into invoice's assign dropdown. 
        private void BindUser()
        {
            dt = new DataTable();
            dt = objEMS.getUser(Session["EMSTeam"].ToString());
            if (dt.Rows.Count > 0)
            {
                ddlUser.DataSource = dt;
                ddlUser.DataTextField = "UL_User_Name";
                ddlUser.DataValueField = "UL_ID";
                ddlUser.DataBind();
                ddlUser.Items.Insert(0, "--Select User--");
                ddlUser.SelectedIndex = 0;
            }
        }
        //by clicking in the invoice file we can view the invoice
        protected void lnkViewFile_Click(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)(sender);
            string id = lnk.CommandArgument;
            string url = "ViewFile.aspx?id=" + id;
            string s = "window.open('" + url + "', 'popup_window', 'width=1000,height =600, top=100,left=100,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

        }
        //after viewing the invoice file we can download that particular invoice.
        private void download(DataTable dt)
        {
            Byte[] bytes = (Byte[])dt.Rows[0]["FI_Data"];
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = dt.Rows[0]["FI_ContentType"].ToString();
            Response.AddHeader("content-disposition", "attachment;filename="
            + dt.Rows[0]["FI_FileName"].ToString());
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        //btnSubmit button is invoice's assign to IP user's
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Assign();
        }
        //selecting the invoice and assign to the user's
        private void Assign()
        {
            try
            {
                //if (ddlClient.SelectedIndex == 0)
                //{
                //    lblError.Text = "Please Select the client";
                //    lblError.ForeColor = System.Drawing.Color.Red;
                //    return;
                //}
                if (ddlUser.SelectedIndex == 0)
                {
                    lblUserError.Visible = true;
                    lblUserError.Text = "Please Select the User";
                 //   lblError.Text = "Please Select the User";
                    lblUserError.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                lblUserError.Visible = false;
                
                int flag = 0;
                foreach (GridViewRow row in gvInvoices.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[1].FindControl("chkSelect") as CheckBox);
                        if (chkRow.Checked)
                        {
                            string _fileId = (row.Cells[0].FindControl("lblFIID") as Label).Text;

                            bool result;
                            if (Session["EMSTeam"].ToString() == "IP")
                            {
                                result = objEMS.IPAssign(_fileId, "IP_Asg", Session["username"].ToString(), ddlUser.SelectedItem.Text, DateTime.Now.ToString());
                            }
                            else if (Session["EMSTeam"].ToString() == "QC")
                            {
                                result = objEMS.QCAssign(_fileId, "QC_Asg", Session["username"].ToString(), ddlUser.SelectedItem.Text, DateTime.Now.ToString());
                            }
                            //if flag =1 then data is available in data table and invoices are assighned to user's
                            flag = 1;

                            counter++;
                        }
                    }

                }
                if (flag == 1)
                {
                   // lblUserError.Text = "Selected invoices successfully assigned to " + ddlUser.SelectedItem.Text;
                    lblUserError.Text = "Invoices assigned successfully";
                    lblUserError.ForeColor = System.Drawing.Color.Blue;
                    lblUserError.Visible = true;
                }
                else
                {
                    lblError.Text = "No Invoices received today";
                    lblError.ForeColor = System.Drawing.Color.Red;

                    lblUserError.Visible = true;
                    lblUserError.Text = "Please select Invoices";
                    //   lblError.Text = "Please Select the User";
                    lblUserError.ForeColor = System.Drawing.Color.Red;
                }


                ddlUser.SelectedIndex = 0;
                //once invoice's assigned to user after grid will refresh and display TotalInvoicesRecieved and TotalInvoicesAssigned count. 
                BindGrid();
                TotalInvoicesRecieved();
                TotalInvoicesAssigned();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            try
            {

                int flag = 0;
                foreach (GridViewRow row in gvInvoices.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[1].FindControl("chkSelect") as CheckBox);
                        if (chkRow.Checked)
                        {
                            string _fileId = (row.Cells[0].FindControl("lblFIID") as Label).Text;

                            bool result;
                            if (Session["EMSTeam"].ToString() == "IP")
                            {
                                result = objEMS.DeleteRecord(_fileId);
                                if (result == true)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delete option", "alert('Data Successfully deleted')", true);
                                    BindGrid();
                                    TotalInvoicesRecieved();
                                    TotalInvoicesAssigned();
                                }
                            }
                           flag = 1;

                            counter++;
                        }
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