using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceSyncApi.Models
{
    public class retObject
    {
        public string retCode { set; get; }
        public object retObj { set; get; }
        public string errMsg { set; get; }
        public retObject()
        {
        }
    }
}