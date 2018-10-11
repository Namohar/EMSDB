using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class SqlDBHelper
    {
        DataSet ds = new DataSet();
        // This function will be used to execute R(CRUD) operation of parameterless commands
        public  static DataTable ExecuteSelectCommand(string CommandName, CommandType cmdType)
        {
            DataTable table = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString()))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            table = new DataTable();
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }                   
                }
            }

            return table;
        }

        // This function will be used to execute R(CRUD) operation of parameterized commands
        public DataTable ExecuteParamerizedSelectCommand(string CommandName, CommandType cmdType, SqlParameter[] param)
        {
            DataTable table = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString()))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }                    
                }
            }

            return table;
        }

        public DataSet DSExecuteParamerizedSelectCommand(string CommandName, CommandType cmdType, SqlParameter[] param, string dtName)
        {
            
            if (ds.Tables.Contains(dtName))
            {
                ds.Tables.Remove(ds.Tables[dtName]);
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString()))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds, dtName);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return ds;
        }

        // This function will be used to execute CUD(CRUD) operation of parameterized commands
        public bool ExecuteNonQuery(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString()))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);                    

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        result = cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }                   
                }
            }

            return (result > 0);        
        }

        public DataTable DBExecDataTable(string ssql)
        {
            DataTable table = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString()))
            {
                da = new SqlDataAdapter(ssql, con);
                da.Fill(table);
            }
            return table;
        }

        public DataSet DBExecDataSet(string ssql, string dtName)
        {
            if (ds.Tables.Contains(dtName))
            {
                ds.Tables.Remove(ds.Tables[dtName]);
            }
            DataTable table = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString()))
            {
                da = new SqlDataAdapter(ssql, con);
                da.Fill(ds, dtName);
            }
            return ds;
        }

        public static SqlConnection OpenConnection(string sNameSpace)
        {
            SqlConnection ObjCon = new SqlConnection();
            string sConString = null;
            try
            {
                sConString = sNameSpace;
                // ObjDBSec.GetKeyValues(sNameSpace, "ConnectionString")
                ObjCon.ConnectionString = sConString;
                ObjCon.Open();
                //ObjCon.ReleaseObjectPool()
                System.Data.OleDb.OleDbConnection.ReleaseObjectPool();
                return ObjCon;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool DBExecuteNoNQuery(string MySql)
        {
            bool functionReturnValue = false;
            SqlConnection pSqlcon = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
            SqlCommand pSqlCmd = new SqlCommand();
            SqlTransaction  MyTrans = null;
            try
            {
                pSqlcon.Open();// = OpenConnection(sNameSpace);
                MyTrans = pSqlcon.BeginTransaction();
                var _with1 = pSqlCmd;
                _with1.Connection = pSqlcon;
                _with1.Transaction = MyTrans;
                _with1.CommandText = MySql;
                _with1.ExecuteNonQuery();
                MyTrans.Commit();
                _with1.Dispose();
                functionReturnValue = true;

            }
            catch (Exception ex)
            {
                MyTrans.Rollback();
                throw;
            }
            finally
            {
                if (pSqlcon.State == ConnectionState.Open)
                {
                    pSqlcon.Close();
                    SqlConnection.ClearAllPools();
                    pSqlcon.Dispose();
                }
            }
            return functionReturnValue;
        }

        public SqlDataReader DBDataReader(string pstrSQL)
        {
            SqlConnection pSqlcon = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
            SqlDataReader pdr = null;
            pSqlcon.Open(); // = OpenConnection(sNameSpace);
            try
            {
                SqlCommand psqlCmd = new SqlCommand(pstrSQL, pSqlcon);
                pdr = psqlCmd.ExecuteReader();
                return pdr;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SqlDataReader ParameterizedSelectReader(string commandName, CommandType cmdType, SqlParameter[] param)
        {
            SqlConnection pSqlcon = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString());
            SqlDataReader pdr = null;
            pSqlcon.Open(); // = OpenConnection(sNameSpace);
            try
            {
                SqlCommand psqlCmd = new SqlCommand();
                psqlCmd.CommandText = commandName;
                psqlCmd.CommandType = cmdType;
                psqlCmd.Connection = pSqlcon;
                psqlCmd.Parameters.AddRange(param);
                if (pSqlcon.State != ConnectionState.Open)
                {
                    pSqlcon.Open();
                }
                pdr = psqlCmd.ExecuteReader();
                return pdr;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int AddTicket(int id, string empId, string tktType, string summary, string details, DateTime issueDate, string assignedTo, string etaDate, string priority, string status, int modifiedBy)
        {
            int ticketId=0;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["conString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("sp_AddTicket", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@T_Employee_Id", empId);
                cmd.Parameters.AddWithValue("@T_Ticket_Type", tktType);
                cmd.Parameters.AddWithValue("@T_Summary", summary);
                cmd.Parameters.AddWithValue("@T_Request_Details", details);
                cmd.Parameters.AddWithValue("@T_Issued_Date_Time", issueDate);
                cmd.Parameters.AddWithValue("@T_Assigned_To", assignedTo);
                cmd.Parameters.AddWithValue("@T_ETA_Date_Time", etaDate);
                cmd.Parameters.AddWithValue("@T_Priority", priority);
                cmd.Parameters.AddWithValue("@T_Status", status);
                cmd.Parameters.AddWithValue("@T_ModifiedBy", modifiedBy);
                cmd.Parameters.AddWithValue("@id", id);

                SqlParameter outPutParameter = new SqlParameter();
                outPutParameter.ParameterName = "@result";
                outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                outPutParameter.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outPutParameter);

                con.Open();
                cmd.ExecuteNonQuery();

                string result = outPutParameter.Value.ToString();

                if (!string.IsNullOrEmpty(result))
                {
                    ticketId = Convert.ToInt32(result);
                }
                else
                {
                    ticketId = 0;
                }
            }


            return ticketId;
        }

        public string DataSource()
        {
            string DataSource = ConfigurationManager.AppSettings["DataSource"];
            return DataSource;
        }

        public string DatabaseName()
        {
            string DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
            return DatabaseName;
        }

        public string DatabaseUserID()
        {
            string DatabaseUserID = ConfigurationManager.AppSettings["DatabaseUserID"];
            return DatabaseUserID;
        }

        public string DatabasePwd()
        {
            string DatabasePwd = ConfigurationManager.AppSettings["DatabasePwd"];
            return DatabasePwd;
        }
    }
}
