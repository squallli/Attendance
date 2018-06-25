using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Globalization;

namespace EMPSYNC
{
    class Program
    {
        static void Main(string[] args)
        {
           
            ArrayList sqlArr = new ArrayList();
            
            try
            {
                sqlArr.AddRange(getTPEEmployeeData());

                sqlArr.AddRange(getTXGEmployeeData());

                sqlArr.AddRange(getTNNEmployeeData());

                sqlArr.AddRange(getSUEmployeeData());


                DBAccess.configuration.連結方式 = DBAccess.configuration.ConnectionType.SQLClient;
                DBAccess.configuration.連結字串 = Settings1.Default.RegalConnection;

                DBAccess.CommonDB.ExecuteModifyQueries(sqlArr);

            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        static private ArrayList getTPEEmployeeData()
        {
            DBAccess.configuration.連結方式 = DBAccess.configuration.ConnectionType.SQLClient;
            DBAccess.configuration.連結字串 = Settings1.Default.RegalConnection;

            string sql = "select MV001,MV002,MV004,MV007,MV008,MV021,MV022,MV047 from CMSMV";
            ArrayList sqlArr = new ArrayList();

            try
            {
                DataTable allEmptb = DBAccess.CommonDB.ExecuteSelectQuery("select EmployeeNo from tbEmployee where  Company='TPE' and Status <> '2'");

                DBAccess.configuration.連結字串 = Settings1.Default.DSCTPEConnection;

                DataTable tb = DBAccess.CommonDB.ExecuteSelectQuery(sql);

                if (tb.Rows.Count > 0) sqlArr.Add("delete from tbEmployee where Company='TPE'");
                string Status = "";

                foreach (DataRow row in tb.Rows)
                {
                    if (row["MV008"].ToString().Trim() == "") continue;

                    if (row["MV004"].ToString().Trim().IndexOf("SA") >= 0)
                        row["MV004"] = "SA";

                    if (allEmptb.Select("EmployeeNo='" + row["MV001"].ToString().Trim() + "'").Length == 0) //如果原本的員工資料沒有,代表是新進人員，再還沒轉正職前狀態為2
                        Status = "2";
                    else
                    {
                        
                        if (row["MV022"].ToString().Trim() != "")
                        {
                            if (DateTime.Now.ToString("yyyyMMdd").CompareTo(row["MV022"].ToString().Trim()) > 0)
                            {
                                Status = "0";
                            }
                            else
                            {
                                Status = "1";
                            }
                        }
                        else
                        {
                            Status = "1";
                        }

                    }
                        
                    sqlArr.Add("insert into tbEmployee(EmployeeNo,EmployeeName,EmployeeEName,DepartMentNo,Company,Sex,Birthday,dayofduty,offDutyDate,[Status]) values(" +
                               "'" + row["MV001"].ToString().Trim() + "'," +
                               "'" + row["MV002"].ToString().Trim() + "'," +
                               "'" + row["MV047"].ToString().Trim() + "'," +
                               "'" + row["MV004"].ToString().Trim() + "'," +
                               "'TPE'," +
                               "'" + row["MV007"].ToString().Trim() + "'," +
                               "'" + row["MV008"].ToString().Trim() + "'," +
                               "'" + row["MV021"].ToString().Trim() + "'," +
                               "'" + row["MV022"].ToString().Trim() + "'," +
                               "'" + Status + "')");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return sqlArr;
            
        }

        static private ArrayList getTXGEmployeeData()
        {
            DBAccess.configuration.連結方式 = DBAccess.configuration.ConnectionType.SQLClient;
            DBAccess.configuration.連結字串 = Settings1.Default.RegalConnection;

            string sql = "select MV001,MV002,MV004,MV007,MV008,MV021,MV022,MV047 from CMSMV";
            ArrayList sqlArr = new ArrayList();

            try
            {
                DataTable allEmptb = DBAccess.CommonDB.ExecuteSelectQuery("select EmployeeNo from tbEmployee where  Company='TXG'  and Status <> '2'");

                DBAccess.configuration.連結字串 = Settings1.Default.DSCTXGConnection;

                DataTable tb = DBAccess.CommonDB.ExecuteSelectQuery(sql);

                if (tb.Rows.Count > 0) sqlArr.Add("delete from tbEmployee where Company='TXG'");
                string Status = "";

                foreach (DataRow row in tb.Rows)
                {
                    if (row["MV008"].ToString().Trim() == "") continue;

                    if (row["MV004"].ToString().Trim().IndexOf("SA") >= 0)
                        row["MV004"] = "SA";

                    if (allEmptb.Select("EmployeeNo='" + row["MV001"].ToString().Trim() + "'").Length == 0) //如果原本的員工資料沒有代表是新進人員，再還沒轉正職前狀態為2
                        Status = "2";
                    else
                    {
                        if (row["MV022"].ToString().Trim() != "")
                        {
                            if (DateTime.Now.ToString("yyyyMMdd").CompareTo(row["MV022"].ToString().Trim()) > 0)
                            {
                                Status = "0";
                            }
                            else
                            {
                                Status = "1";
                            }
                        }
                        else
                        {
                            Status = "1";
                        }
                    }
                       // Status = row["MV022"].ToString().Trim() == "" ? "1" : "0";  //檢查是否有離職日，若有則狀態為 0 

                    sqlArr.Add("insert into tbEmployee(EmployeeNo,EmployeeName,EmployeeEName,DepartMentNo,Company,Sex,Birthday,dayofduty,offDutyDate,[Status]) values(" +
                               "'" + row["MV001"].ToString().Trim() + "'," +
                               "'" + row["MV002"].ToString().Trim() + "'," +
                               "'" + row["MV047"].ToString().Trim() + "'," +
                               "'" + row["MV004"].ToString().Trim() + "'," +
                               "'TXG'," +
                               "'" + row["MV007"].ToString().Trim() + "'," +
                               "'" + row["MV008"].ToString().Trim() + "'," +
                               "'" + row["MV021"].ToString().Trim() + "'," +
                               "'" + row["MV022"].ToString().Trim() + "'," +
                               "'" + Status + "')");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return sqlArr;

        }

        static private ArrayList getTNNEmployeeData()
        {
            DBAccess.configuration.連結方式 = DBAccess.configuration.ConnectionType.SQLClient;
            DBAccess.configuration.連結字串 = Settings1.Default.RegalConnection;

            string sql = "select MV001,MV002,MV004,MV007,MV008,MV021,MV022,MV047 from CMSMV";
            ArrayList sqlArr = new ArrayList();

            try
            {
                DataTable allEmptb = DBAccess.CommonDB.ExecuteSelectQuery("select EmployeeNo from tbEmployee where Company='TNN'  and Status <> '2'");

                DBAccess.configuration.連結字串 = Settings1.Default.DSCTNNConnection;

                DataTable tb = DBAccess.CommonDB.ExecuteSelectQuery(sql);

                if (tb.Rows.Count > 0) sqlArr.Add("delete from tbEmployee where Company='TNN'");
                string Status = "";

                foreach (DataRow row in tb.Rows)
                {
                    if (row["MV008"].ToString().Trim() == "") continue;

                    if (row["MV004"].ToString().Trim().IndexOf("SA") >= 0)
                        row["MV004"] = "SA";

                    if (allEmptb.Select("EmployeeNo='" + row["MV001"].ToString().Trim() + "'").Length == 0) //如果原本的員工資料沒有代表是新進人員，再還沒轉正職前狀態為2
                        Status = "2";
                    else
                    {

                        if (row["MV022"].ToString().Trim() != "")
                        {
                            if (DateTime.Now.ToString("yyyyMMdd").CompareTo(row["MV022"].ToString().Trim()) > 0)
                            {
                                Status = "0";
                            }
                            else
                            {
                                Status = "1";
                            }
                        }
                        else
                        {
                            Status = "1";
                        }

                    }

                    sqlArr.Add("insert into tbEmployee(EmployeeNo,EmployeeName,EmployeeEName,DepartMentNo,Company,Sex,Birthday,dayofduty,offDutyDate,[Status]) values(" +
                               "'" + row["MV001"].ToString().Trim() + "'," +
                               "'" + row["MV002"].ToString().Trim() + "'," +
                               "'" + row["MV047"].ToString().Trim() + "'," +
                               "'" + row["MV004"].ToString().Trim().Replace("6","") + "'," +
                               "'TNN'," +
                               "'" + row["MV007"].ToString().Trim() + "'," +
                               "'" + row["MV008"].ToString().Trim() + "'," +
                               "'" + row["MV021"].ToString().Trim() + "'," +
                               "'" + row["MV022"].ToString().Trim() + "'," +
                               "'" + Status + "')");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return sqlArr;

        }


        static private ArrayList getSUEmployeeData()
        {
            DBAccess.configuration.連結方式 = DBAccess.configuration.ConnectionType.SQLClient;
            DBAccess.configuration.連結字串 = Settings1.Default.RegalConnection;

            string sql = "select MV001,MV002,MV004,MV007,MV008,MV021,MV022,MV047 from CMSMV";
            ArrayList sqlArr = new ArrayList();

            try
            {
                DataTable allEmptb = DBAccess.CommonDB.ExecuteSelectQuery("select EmployeeNo from tbEmployee where  Company='SU'  and Status <> '2'");

                DBAccess.configuration.連結字串 = Settings1.Default.DSCSUConnection;

                DataTable tb = DBAccess.CommonDB.ExecuteSelectQuery(sql);

                if (tb.Rows.Count > 0) sqlArr.Add("delete from tbEmployee where Company='SU'");
                string Status = "";

                foreach (DataRow row in tb.Rows)
                {
                    //if (row["MV008"].ToString().Trim() == "") continue;

                    if (row["MV004"].ToString().Trim().IndexOf("SA") >= 0)
                        row["MV004"] = "SA";

                    if (allEmptb.Select("EmployeeNo='" + row["MV001"].ToString().Trim() + "'").Length == 0) //如果原本的員工資料沒有代表是新進人員，再還沒轉正職前狀態為2
                        Status = "2";
                    else
                    {

                        if (row["MV022"].ToString().Trim() != "")
                        {
                            if (DateTime.Now.ToString("yyyyMMdd").CompareTo(row["MV022"].ToString().Trim()) > 0)
                            {
                                Status = "0";
                            }
                            else
                            {
                                Status = "1";
                            }
                        }
                        else
                        {
                            Status = "1";
                        }
                    }

                    sqlArr.Add("insert into tbEmployee(EmployeeNo,EmployeeName,EmployeeEName,DepartMentNo,Company,Sex,Birthday,dayofduty,offDutyDate,[Status]) values(" +
                               "'" + row["MV001"].ToString().Trim() + "'," +
                               "'" + row["MV002"].ToString().Trim() + "'," +
                               "'" + row["MV047"].ToString().Trim() + "'," +
                               "'" + row["MV004"].ToString().Trim() + "'," +
                               "'SU'," +
                               "'" + row["MV007"].ToString().Trim() + "'," +
                               "'" + row["MV008"].ToString().Trim() + "'," +
                               "'" + row["MV021"].ToString().Trim() + "'," +
                               "'" + row["MV022"].ToString().Trim() + "'," +
                               "'" + Status + "')");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return sqlArr;

        }


        static private void Log(string errMsg)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings1.Default.RegalConnection))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into SystemLog(LogDate,LogTime,LogMsg) values('" + System.DateTime.Now.ToString("yyyyMMdd") + "'," +
                                          "'" + System.DateTime.Now.ToString("HHmmdd") + "','" + errMsg.Replace("'","''") + "')" ;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
            }
            
        }
    }
}
