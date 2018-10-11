using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL
{
   public class clsProfile
    {
       public DataTable getUserDetails(string username)
       {
           DataTable dt = new DataTable();
           SqlDBHelper objDB = new SqlDBHelper();
           SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@UL_User_Name", username)  
           };
           string sQuery = "select * from dbo.RTM_User_List, dbo.RTM_Team_List where UL_Team_Id = T_ID and UL_System_User_Name =@UL_User_Name";
           dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
           //dt = objDB.DBExecDataTable(sQuery);
           return dt;
       }

       public bool UpdateProfile(string gender, string UserName, string emailId)
       {
           SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@UL_Gender", gender),
               new SqlParameter("@UL_User_Name", UserName),
               new SqlParameter("@UL_EmailId", emailId)
           };

           
           bool result;
           SqlDBHelper objDB = new SqlDBHelper();
           string sQuery = "UPDATE RTM_User_List SET UL_EmailId=@UL_EmailId, UL_Gender=@UL_Gender WHERE UL_System_User_Name = @UL_User_Name";
           result = objDB.ExecuteNonQuery(sQuery, CommandType.Text, parameters);
          // string sQuery = "UPDATE RTM_User_List SET UL_EmailId='" + emailId + "', UL_Gender='" + gender + "' WHERE UL_System_User_Name = '"+ UserName +"'";

           //result = objDB.DBExecuteNoNQuery(sQuery);

           return result;
       }
    }
}
