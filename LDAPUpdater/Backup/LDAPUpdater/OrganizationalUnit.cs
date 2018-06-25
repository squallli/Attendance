using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LDAPUpdater
{
    public class OrganizationalUnit
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _ldapObjectString;
        public string LDAPObjectString
        {
            get { return _ldapObjectString; }
            set { _ldapObjectString = value; }
        }

        public static DataTable CreateOrganizationalUnitDataTable()
        {
            DataTable dtOrganizationalUnits = new DataTable("OrganizationalUnits");
            dtOrganizationalUnits.Columns.Add(new DataColumn("Name"));
            dtOrganizationalUnits.Columns.Add(new DataColumn("IsOff"));
            dtOrganizationalUnits.Columns.Add(new DataColumn("LDAPObjectString"));
            dtOrganizationalUnits.Columns.Add(new DataColumn("CompId"));
            dtOrganizationalUnits.Columns.Add(new DataColumn("DeptId"));

            return dtOrganizationalUnits;
        }
    }    
}
