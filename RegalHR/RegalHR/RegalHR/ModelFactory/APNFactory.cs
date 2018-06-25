using REGAL.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace RegalHR.ModelFactory
{
    public class APNFactory
    {
        public DataAccess DbAccess = new DataAccess();
        public APNFactory()
        {
            //資料庫連線配置
            DbAccess.ConnectionString = RegalLib.DbConnStr;
            DbAccess.ProviderName = RegalLib.ProviderName;
        }

        public bool InsertUpdateDeviceToken(string EmployeeNo,string deviceToken)
        {
            DbTransaction objTrans = DbAccess.CreateDbTransaction();
            if (DbAccess.ExecuteDataTable("SELECT * FROM EmpApnMapping Where EmployeeNo = @EmployeeNo ",
               new DbParameter[] {
                    DataAccess.CreateParameter("EmployeeNo", DbType.String, EmployeeNo)
               }
                ).Rows.Count > 0)
            {
                
                try
                {
                    DbAccess.ExecuteNonQuery("update EmpApnMapping set deviceToken=@deviceToken where EmployeeNo=@EmployeeNO", objTrans, new DbParameter[] {

                        DataAccess.CreateParameter("deviceToken", DbType.String,  deviceToken),
                         DataAccess.CreateParameter("EmployeeNo", DbType.String, EmployeeNo)
                    });
                    objTrans.Commit();
                }
                catch
                {
                    objTrans.Rollback();
                    return false;
                }
                finally
                {
                    if (objTrans != null)
                    {
                        objTrans.Dispose();
                    }
                }
            }
            else
            {
                try
                {
                    DbAccess.ExecuteNonQuery("insert into EmpApnMapping(EmployeeNo,deviceToken) values(@EmployeeNo,@deviceToken)", objTrans, new DbParameter[] {
                           DataAccess.CreateParameter("EmployeeNo", DbType.String,  EmployeeNo),
                           DataAccess.CreateParameter("deviceToken", DbType.String,  deviceToken)
                    });
                    objTrans.Commit();
                }
                catch
                {
                    objTrans.Rollback();
                    return false;
                }
                finally
                {
                    if (objTrans != null)
                    {
                        objTrans.Dispose();
                    }
                }
            }

           
            return true;
        }
    }
}
