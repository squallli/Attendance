using AttendanceSyncApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AttendanceSyncApi.Controllers
{
    public class gvEmployeeAllController : ApiController
    {
        private Attendance db = null;

        public gvEmployeeAllController()
        {
            db = new Attendance();
        }

        public retObject getEmployeeDataByCompany(string company)
        {
            retObject retObj = new retObject();
            try
            {
                List<gvEmployeeAll> empdata = db.gvEmployeeAll.Where(e => e.Status == "1").Where(e => e.CardNo != null).ToList();
                if (empdata.Count == 0)
                {
                    retObj.retCode = "-1";
                    retObj.retObj = null;
                    retObj.errMsg = "查無資料";
                }
                else
                {
                    retObj.retCode = "0";
                    retObj.retObj = empdata;
                    retObj.errMsg = "";
                }                
            }
            catch (Exception ex)
            {
                retObj.retCode = "-1";
                retObj.retObj = null;
                retObj.errMsg = ex.Message;
            }
            finally
            {
                Dispose(true);
            }
            return retObj;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}