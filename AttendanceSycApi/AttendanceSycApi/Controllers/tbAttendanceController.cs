using AttendanceSyncApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AttendanceSyncApi.Controllers
{
    public class tbAttendanceController : ApiController
    {
        private Attendance db = null;

        public tbAttendanceController()
        {
            db = new Attendance();
        }

        public retObject InserttbAttendanceData([FromBody] tbAttendance attendanceData)
        {
            bool isSuccess = false;
            string err = "";
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.tbAttendance.Add(attendanceData);
                    db.tbAttendance.AddRange(new tbAttendance[] { attendanceData });
                    db.SaveChanges();

                    dbContextTransaction.Commit();
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    err = ex.InnerException.InnerException.Message;
                    dbContextTransaction.Rollback();
                }
            }

            retObject retObj = new retObject();

            if (isSuccess)
            {
                retObj.retCode = "0";
                retObj.retObj = null;
                retObj.errMsg = "";
            }
            else
            {
                retObj.retCode = "-1";
                retObj.retObj = null;
                retObj.errMsg = err;
            }

            return retObj;
        }
    }
}
