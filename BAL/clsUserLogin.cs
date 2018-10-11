using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using System.Data.SqlClient;

namespace BAL
{
    public class clsUserLogin
    {
        SqlDBHelper objDB = new SqlDBHelper();
        public DataTable getUser(string username)
        {
            DataTable dt = new DataTable();
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@userName", username)
               
           };
            string sQuery = "select UL_ID, UL_Team_Id, UL_User_Name, AL_AccessLevel, T_Location, T_Preference, UL_EMS_Team from RTM_User_List, RTM_Access_Level, dbo.RTM_Team_List where UL_Employee_Id = AL_EmployeeId and UL_Team_Id = T_ID and UL_System_User_Name =@userName";
            dt = objDB.ExecuteParamerizedSelectCommand(sQuery, CommandType.Text, parameters);
            //dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }

        public DataTable getLocations()
        {
            DataTable dt = new DataTable();
            string sQuery = "select Distinct T_Location from RTM_Team_List order by T_Location";
            dt = objDB.DBExecDataTable(sQuery);
            return dt;
        }
    }
}
