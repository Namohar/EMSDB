using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using System.Data.SqlClient;

namespace BAL
{
    public class clsEMSDB
    {
        SqlDBHelper objDB = new SqlDBHelper();

        public DataTable getClients(string _team)
        {
            DataTable dt = new DataTable();
            string sQuery;
          
            if (_team == "IP")
            {
                sQuery = "select * from EMSDB_Clients";
            }
            else if (_team == "QC")
            {
                //sQuery = "select C_Code, C_Name from EMSDB_Clients, EMSDB_FileInfo, EMSDB_InvDetails where C_Code = FI_ClientCode and FI_ID = IND_FI and IND_Status = 'QC_Idle' and IND_QC_Assigned_By is null Group By C_Code, C_Name Order By C_Name";
                sQuery = "select * from EMSDB_Clients";
            }
            else
            {
                sQuery = "select * from EMSDB_Clients";
            }
            dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }

        public DataTable getInvoices(string _clientCode, string _team)
        {
            DataTable dt = new DataTable();
            string sQuery = "";
           SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@clientCode", _clientCode),
               new SqlParameter("@createdOn", DateTime.Now.ToShortDateString())
           };
            if (_team == "IP")
            {
                if (_clientCode == "all")
                {
                    //sQuery = "select FI_ID, CONVERT(VARCHAR(10), FI_ReceiptDate, 111) as  FI_ReceiptDate, FI_Source, FI_ClientCode, FI_FileName, FI_OriginalName, CONVERT(VARCHAR(10), FI_CreatedOn, 111) as FI_CreatedOn, 'Idle' as IND_Status, IND_InvoiceNo, IND_LineItem, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where  CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, FI_CreatedOn))) = @createdOn and IND_Status is null order by FI_CreatedOn Desc";
                    sQuery = "select FI_ID, CONVERT(VARCHAR(10), FI_ReceiptDate, 111) as  FI_ReceiptDate, FI_Source, FI_ClientCode, FI_FileName, FI_OriginalName, CONVERT(VARCHAR(10), FI_CreatedOn, 111) as FI_CreatedOn, 'Idle' as IND_Status, IND_InvoiceNo, IND_LineItem, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where   IND_Status is null  and FI_ClientCode not in ('FRB','VER') order by FI_CreatedOn Desc";
                }
                else
                {
                    sQuery = "select FI_ID, CONVERT(VARCHAR(10), FI_ReceiptDate, 111) as  FI_ReceiptDate, FI_Source, FI_ClientCode, FI_FileName, FI_OriginalName, CONVERT(VARCHAR(10), FI_CreatedOn, 111) as FI_CreatedOn, 'Idle' as IND_Status, IND_InvoiceNo, IND_LineItem, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_ClientCode=@clientCode  and IND_Status is null order by FI_CreatedOn Desc";
                    //sQuery = "select FI_ID, CONVERT(VARCHAR(10), FI_ReceiptDate, 111) as  FI_ReceiptDate, FI_Source, FI_ClientCode, FI_FileName, FI_OriginalName, CONVERT(VARCHAR(10), FI_CreatedOn, 111) as FI_CreatedOn, 'Idle' as IND_Status, IND_InvoiceNo, IND_LineItem, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_ClientCode=@clientCode and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, FI_CreatedOn))) = @createdOn and IND_Status is null order by FI_CreatedOn Desc";
                  
                }
            }
            else if (_team == "QC")
            {
                if (_clientCode == "all")
                {
                    sQuery = "select FI_ID, CONVERT(VARCHAR(10), FI_ReceiptDate, 111) as  FI_ReceiptDate, FI_Source, FI_ClientCode, FI_FileName, FI_OriginalName, CONVERT(VARCHAR(10), FI_CreatedOn, 111) as FI_CreatedOn, IND_Status, IND_InvoiceNo, IND_LineItem, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_Status = 'QC_Idle' and IND_QC_Assigned_By is null order by FI_CreatedOn Desc";
                }
                else
                {
                    sQuery = "select FI_ID, CONVERT(VARCHAR(10), FI_ReceiptDate, 111) as  FI_ReceiptDate, FI_Source, FI_ClientCode, FI_FileName, FI_OriginalName, CONVERT(VARCHAR(10), FI_CreatedOn, 111) as FI_CreatedOn, IND_Status, IND_InvoiceNo, IND_LineItem, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_ClientCode=@clientCode and  IND_Status = 'QC_Idle' and IND_QC_Assigned_By is null order by FI_CreatedOn Desc";
                }
            }
            else if (_team == "both")
            {
                sQuery = "select FI_ID, CONVERT(VARCHAR(10), FI_ReceiptDate, 111) as  FI_ReceiptDate, FI_Source, FI_ClientCode, FI_FileName, FI_OriginalName, CONVERT(VARCHAR(10), FI_CreatedOn, 111) as FI_CreatedOn, CASE IND_Status WHEN 'QC_Idle' THEN 'QC_Idle' ELSE 'Idle' END as IND_Status, IND_InvoiceNo, IND_LineItem, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_ClientCode=@clientCode and (IND_Status is null or IND_Status = 'QC_Idle') order by FI_CreatedOn Desc";
            }
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            //dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }

       

        public DataTable getQCInvoices(string _clientCode)
        {
            DataTable dt = new DataTable();
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@clientCode", _clientCode)
           };
            string sQuery = "select FI_ID, CONVERT(VARCHAR(10), FI_ReceiptDate, 111) as  FI_ReceiptDate, FI_Source, FI_ClientCode, FI_FileName, FI_OriginalName, CONVERT(VARCHAR(10), FI_CreatedOn, 111) as FI_CreatedOn, IND_Status, IND_InvoiceNo, IND_LineItem, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_ClientCode=@clientCode and IND_Status = 'QC_Idle' and IND_QC_Assigned_By is null order by FI_CreatedOn Desc";
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            //dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }

        public DataTable getAttachment(string _id)
        {
            DataTable dt = new DataTable();
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@id", _id)
           };
            string sQuery = "select FI_ID, FI_FileName,FI_ContentType,FI_Data from dbo.EMSDB_FileInfo where FI_ID=@id";
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            //dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }

        public DataTable getUser(string team)
        {
            DataTable dt = new DataTable();
            string sQuery ="";
            if(team ==("IP"))
            {
                sQuery = "select * from RTM_User_List where UL_EMS_Team = 'IP' and UL_Status =1";
            }
            else if(team == ("QC"))
            {
                sQuery = "select * from RTM_User_List where UL_EMS_Team = 'QC' and UL_Status =1";
            }
            
            dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }

        public DataTable GetTotalInvoicesRecieved(string _clientCode)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@clientCode", _clientCode),
               new SqlParameter("@createdOn", DateTime.Now.ToShortDateString())
           };
            if (_clientCode == "all")
            {
                sQuery = "select count(*) as total from EMSDB_FileInfo where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, FI_CreatedOn))) = @createdOn";
            }
            else
            {
                sQuery = "select count(*) as total from EMSDB_FileInfo where CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, FI_CreatedOn))) = @createdOn and FI_ClientCode=@clientCode";
            }
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            //dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }

        public DataTable GetTotalInvoicesAssigned(string _clientCode)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@clientCode", _clientCode),
               new SqlParameter("@createdOn", DateTime.Now.ToShortDateString())
           };
            if (_clientCode == "all")
            {
                sQuery = "select count(*) as total from EMSDB_InvDetails, EMSDB_FileInfo  where IND_FI= FI_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, FI_CreatedOn))) = @createdOn";
            }
            else
            {
                sQuery = "select count(*) as total from EMSDB_InvDetails, EMSDB_FileInfo  where IND_FI= FI_ID and CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, FI_CreatedOn))) = @createdOn and FI_ClientCode=@clientCode ";
            }
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            //dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }

        public bool IPAssign(string _fileId, string _status, string _assignedBy, string _assignedTo, string createdOn)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@fileId", _fileId),
               new SqlParameter("@status", _status),
               new SqlParameter("@assignedBy", _assignedBy),
               new SqlParameter("@assignedTo", _assignedTo),
               new SqlParameter("@createdOn", createdOn)
           };
            string sQuery = "Insert into EMSDB_InvDetails (IND_FI, IND_Status, IND_IP_Assigned_By, IND_IP_Processed_By, IND_IP_CreatedOn) values(@fileId, @status, @assignedBy, @assignedTo, @createdOn)";

            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            //result = objDB.DBExecuteNoNQuery(sQuery);

            return result;
        }

        public bool UpdateIPUser(string _fileId, string _status, string _assignedBy, string _assignedTo)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@fileId", _fileId),
               new SqlParameter("@status", _status),
               new SqlParameter("@assignedBy", _assignedBy),
               new SqlParameter("@assignedTo", _assignedTo),
               new SqlParameter("@createdOn", DateTime.Now)
           };
            //string sQuery = "Insert into EMSDB_InvDetails (IND_FI, IND_Status, IND_IP_Assigned_By, IND_IP_Processed_By, IND_IP_CreatedOn) values('" + _fileId + "', '" + _status + "', '" + _assignedBy + "', '" + _assignedTo + "', '" + DateTime.Now + "')";
            string sQuery = "Update EMSDB_InvDetails set IND_Status=@status, IND_IP_Assigned_By=@assignedBy, IND_IP_Processed_By=@assignedTo, IND_IP_CreatedOn=@createdOn where IND_FI=@fileId";
            //result = objDB.DBExecuteNoNQuery(sQuery);
            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            return result;
        }
        //Update process column status
        public bool UpdateIPProcess(string _fileId, string _status)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@fileId", _fileId),
               new SqlParameter("@status", _status),
            
               new SqlParameter("@modifiedOn", DateTime.Now)
           };

            string sQuery = "Update EMSDB_InvDetails set IND_Status=@status, IND_IP_ModifiedOn=@modifiedOn where IND_FI=@fileId";
          
            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            return result;
        }

       

        public bool QCAssign(string _fileId, string _status, string _assignedBy, string _assignedTo, string createdOn)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@fileId", _fileId),
               new SqlParameter("@status", _status),
               new SqlParameter("@assignedBy", _assignedBy),
               new SqlParameter("@assignedTo", _assignedTo),
               new SqlParameter("@createdOn", DateTime.Now)
           };
            string sQuery = "Update EMSDB_InvDetails SET IND_Status= @status, IND_QC_Assigned_By= @assignedBy, IND_QC_Processed_By=@assignedTo, IND_QC_CreatedOn = @createdOn where IND_FI =@fileId";

            //result = objDB.DBExecuteNoNQuery(sQuery);
            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);

            return result;
        }

        public bool UpdateStatus(string _status, string _lineItem, string _fileId)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@fileId", _fileId),
               new SqlParameter("@status", _status),
               new SqlParameter("@lineItem", _lineItem),               
               new SqlParameter("@createdOn", DateTime.Now)
           };
            string sQuery = "Update EMSDB_InvDetails set IND_Status=@status, IND_LineItem =@lineItem, IND_IP_ModifiedOn =@createdOn where IND_FI=@fileId";

            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            return result;
        }

        public bool UpdateLineItemCount(string _lineItem, string _fileId)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@fileId", _fileId),               
               new SqlParameter("@lineItem", _lineItem),               
               new SqlParameter("@createdOn", DateTime.Now)
           };
            string sQuery = "Update EMSDB_InvDetails set IND_LineItem =@lineItem, IND_IP_ModifiedOn =@createdOn where IND_FI=@fileId";

            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            return result;
        }


        public bool UpdateInvoiceNo(string _invoiceNo, string _fileId)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@fileId", _fileId),               
               new SqlParameter("@invoiceNo", _invoiceNo),    
               new SqlParameter("@createdOn", DateTime.Now)
           };
            string sQuery = "Update EMSDB_InvDetails set IND_InvoiceNo=@invoiceNo, IND_IP_ModifiedOn =@createdOn where IND_FI=@fileId";

            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            return result;
        }

        public bool UpdateErrorNo(string _errorNo, string _fileId)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@fileId", _fileId),               
               new SqlParameter("@errorNo", _errorNo),    
               new SqlParameter("@createdOn", DateTime.Now)
           };
            string sQuery = "Update EMSDB_InvDetails set IND_Error=@errorNo, IND_IP_ModifiedOn =@createdOn where IND_FI=@fileId";

            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            return result;
        }

        public DataTable GetAssignedIPInvoices(string access, string username)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                //sQuery = "select FI_ID,FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_IP_Processed_By =@username and IND_Status != 'QC_Comp' and IND_Status != 'Duplicate' Order By FI_CreatedOn";
                sQuery = "select FI_ID,FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_Source not in ('EDI') and  IND_IP_Processed_By =@username and IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework')  Order By FI_CreatedOn";
            }
            else
            {
                //sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_Status Is Not Null and IND_Status != 'QC_Comp' and IND_Status != 'Duplicate'  Order By FI_CreatedOn ";
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_Source not in ('EDI') and  IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework') Order By FI_CreatedOn";
            }
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
    
            return dt;
        }

        public DataTable GetAssignedQCInvoices(string access, string username)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_QC_Processed_By =@username  and IND_Status != 'QC_Comp' and IND_Status != 'Duplicate' Order By FI_CreatedOn";
            }
            else
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_QC_Processed_By is Not Null and IND_Status != 'QC_Comp' and IND_Status != 'Duplicate' Order By FI_CreatedOn ";
            }

            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }


        public DataTable GetAssignedEDIInvoices(string access, string username)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                //sQuery = "select FI_ID,FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_IP_Processed_By =@username and IND_Status != 'QC_Comp' and IND_Status != 'Duplicate' Order By FI_CreatedOn";
                sQuery = "select FI_ID,FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_IP_Processed_By =@username and IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework') and FI_Source in ('EDI') Order By FI_CreatedOn";
            }
            else
            {
                //sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_Status Is Not Null and IND_Status != 'QC_Comp' and IND_Status != 'Duplicate'  Order By FI_CreatedOn ";
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework') and FI_Source in ('EDI') Order By FI_CreatedOn";
            }
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);

            return dt;
        }

        public DataTable SearchDataQC(string access, string username, string Query)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_QC_Processed_By =@username  and IND_Status != 'QC_Comp' and " + Query + "%' and IND_Status Is Not Null Order By FI_CreatedOn";
            }
            else
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where " + Query + "%' and IND_Status Is Not Null Order By FI_CreatedOn ";
            }

            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }

        //IP invoice processing
        public DataTable SearchData(string access, string username,string Query)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_IP_Processed_By =@username  and " + Query + "%' and IND_Status Is Not Null and FI_Source not in ('EDI') Order By FI_CreatedOn";
            }
            else
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where " + Query + "%' and IND_Status Is Not Null and FI_Source not in ('EDI') Order By FI_CreatedOn ";
            }

            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }

        public DataTable SearchDataByReceivedDate(string access, string username, DateTime ReceivedDate)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_IP_Processed_By =@username and cast(FI_ReceiptDate as date ) = '" + ReceivedDate + "' and IND_Status Is Not Null and FI_Source not in ('EDI') Order By FI_CreatedOn";
            }
            else
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where cast(FI_ReceiptDate as date ) = '" + ReceivedDate + "'and IND_Status Is Not Null and FI_Source not in ('EDI') Order By FI_CreatedOn ";
            }

            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }

        //EDI invoice processing
        public DataTable SearchData_EDI(string access, string username, string Query)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_IP_Processed_By =@username  and " + Query + "%' and IND_Status Is Not Null and FI_Source in ('EDI') Order By FI_CreatedOn";
            }
            else
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where " + Query + "%' and IND_Status Is Not Null and FI_Source in ('EDI') Order By FI_CreatedOn ";
            }

            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }

        public DataTable SearchDataByReceivedDate_EDI(string access, string username, DateTime ReceivedDate)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By,IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_IP_Processed_By =@username and cast(FI_ReceiptDate as date ) = '" + ReceivedDate + "' and IND_Status Is Not Null and FI_Source  in ('EDI') Order By FI_CreatedOn";
            }
            else
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, IND_QC_Assigned_By, IND_QC_Processed_By, IND_Error,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate  from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where cast(FI_ReceiptDate as date ) = '" + ReceivedDate + "'and IND_Status Is Not Null and FI_Source in ('EDI') Order By FI_CreatedOn ";
            }

            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }

        public DataTable GetComments(int _fileId)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@fileId", _fileId)
            };
            sQuery = "select COM_TeamCode as [Team], COM_CommentsBy as [Commented By], COM_Comments as Comments,  COM_CreatedOn as [Commented On] from EMSDB_Comments where COM_FI =@fileId order By COM_CreatedOn Desc";
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }

        public bool UpdateComments(string _comments, string _team, string _fileId, string _commentedBy)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@fileId", _fileId),
                new SqlParameter("@comments", _comments),
                new SqlParameter("@team", _team),
                new SqlParameter("@commentedBy", _commentedBy),
                new SqlParameter("@commentedOn", DateTime.Now)
            };
            //string sQuery = "update EMSDB_Comments set COM_Comments='" + _comments + "', COM_CommentsBy='" + _commentedBy + "', COM_CreatedOn='" + DateTime.Now + "', COM_TeamCode= '"+ _team +"'";
            string sQuery = "Insert into EMSDB_Comments (COM_FI, COM_TeamCode, COM_Comments, COM_CommentsBy, COM_CreatedOn) values (@fileId, @team, @comments, @commentedBy, @commentedOn)";
            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            return result;
        }

        public DataTable getLineItem(int id)
        {
            DataTable dt = new DataTable();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            string sQuery = "select C_DataSource,C_DataBase , C_UserId, C_Password from dbo.EMSDB_Clients, dbo.EMSDB_FileInfo where C_Code = FI_ClientCode and FI_ID = @id";
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }

        public bool RecordLog(string _fileId, string _team, string _column, string _oldValue, string _newValue, string _modifiedBy)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@fileId", _fileId),
                new SqlParameter("@column", _column),
                new SqlParameter("@team", _team),
                new SqlParameter("@oldValue", _oldValue),
                new SqlParameter("@newValue", _newValue),
                new SqlParameter("@modifiedBy", _modifiedBy),
                new SqlParameter("@modifiedOn", DateTime.Now)
            };
            string sQuery = "insert into dbo.EMSDB_Log (Log_FileId, Log_Team, Log_Field, Log_OldValue, Log_NewValue, Log_ModifiedBy, Log_ModifiedOn) values (@fileId, @team, @column, @oldValue, @newValue, @modifiedBy, @modifiedOn)";

            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);

            return result;
        }


       public bool UpdateRecordLog(string _fileId,  string _newValue, string _modifiedBy)
        {
            bool result;
          
            string sQuery = " update EMSDB_Log set Log_NewValue='" + _newValue + "',Log_ModifiedBy='" + _modifiedBy + "',Log_ModifiedOn='" + DateTime.Now + "' where Log_FileId=" + _fileId + " and Log_NewValue='" + _newValue + "'";
            result=objDB.DBExecuteNoNQuery(sQuery);
            return result;
        }
       
        public DataTable CheckingInvoiceNumber(string _fileId, string _team, string _newValue)
        {
         

            string sQuery = "select * from dbo.EMSDB_Log where Log_FileId=" + _fileId + " and Log_Team='" + _team + "' and Log_NewValue='" + _newValue + "'";

            DataTable dt = objDB.DBExecDataTable(sQuery);


            return dt;
        }



        //*************************************REPORTS****************************

        public DataTable GetCompletedRecords(int id)
        {
            DataTable dt = new DataTable();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            string sQuery = " select FI_CreatedOn, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By,IND_Error from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where IND_IP_Processed_By ='R Ganesh' and IND_Status != 'QC_Comp' and IND_Status != 'Duplicate' and FI_CreatedOn between '2017-03-03' and '2017-03-14'  Order By FI_CreatedOn";
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            return dt;
        }

        public bool DeleteRecord(string Fi_Id)
        {
            bool result;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@id", Fi_Id)
            };
            string sQuery = "delete from dbo.EMSDB_FileInfo where FI_ID=@id";
            result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
            return result;
        }

        public DataTable Export(string access, string username)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {              
                sQuery = "select FI_ID,FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_Source not in ('EDI') and  IND_IP_Processed_By =@username and IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework')  Order By FI_CreatedOn";
            }
            else
            {                
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_Source not in ('EDI') and  IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework') Order By FI_CreatedOn";
            }
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);

            return dt;
        }

        public DataTable Export_RecieveDate(string access, string username, DateTime ReceivedDate)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                sQuery = "select FI_ID,FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_Source not in ('EDI') and  IND_IP_Processed_By =@username and IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework') and cast(FI_ReceiptDate as date ) = '" + ReceivedDate + "'  Order By FI_CreatedOn";
            }
            else
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_Source not in ('EDI') and  IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework') and cast(FI_ReceiptDate as date ) = '" + ReceivedDate + "' Order By FI_CreatedOn";
            }
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);

            return dt;
        }
        public DataTable Export_SearchBy(string access, string username, string Query)
        {
            DataTable dt = new DataTable();
            string sQuery;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username)
            };
            if (access == "4")
            {
                sQuery = "select FI_ID,FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By, CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_Source not in ('EDI') and  IND_IP_Processed_By =@username and IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework') and " + Query + "%'  Order By FI_CreatedOn";
            }
            else
            {
                sQuery = "select FI_ID, FI_Source, FI_FileName, IND_Status, IND_InvoiceNo, IND_IP_Assigned_By, IND_IP_Processed_By,CONVERT(VARCHAR(12),FI_ReceiptDate, 107) AS ReceiptDate from dbo.EMSDB_FileInfo left join EMSDB_InvDetails on FI_ID = IND_FI where FI_Source not in ('EDI') and  IND_Status Is Not Null and IND_Status in ('IP_Asg','IP_Inp','IP_Issue','Rework') and " + Query + "%' Order By FI_CreatedOn";
            }
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);

            return dt;
        }
    }
}
