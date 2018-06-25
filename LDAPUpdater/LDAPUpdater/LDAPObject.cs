using System;
using System.Collections.Generic;
using System.Text;

namespace LDAPUpdater
{
    public class LDAPUser
    {
        private Guid _objectGUID;
        public Guid objectGUID
        {
            get { return _objectGUID; }
            set { _objectGUID = value; }
        }

        private string _domainAccount;
        public string DomainAccount
        {
            get { return _domainAccount; }
            set { _domainAccount = value; }
        }

        private string _domain;
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        private string _sAMAccountName;
        public string sAMAccountName
        {
            get { return _sAMAccountName; }
            set { _sAMAccountName = value; }
        }

        private string _sAMAccountType;
        public string sAMAccountType
        {
            get { return _sAMAccountType; }
            set { _sAMAccountType = value; }
        }

        private string _ObjectClass;
        public string ObjectClass
        {
            get { return _ObjectClass; }
            set { _ObjectClass = value; }
        }

        private string _givenName;
        public string givenName
        {
            get { return _givenName; }
            set { _givenName = value; }
        }

        private string _sn;
        public string sn
        {
            get { return _sn; }
            set { _sn = value; }
        }

        private string _displayName;
        public string displayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        private string _description;
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _mail;
        public string mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        private string _userAccountControl;
        public string userAccountControl
        {
            get { return _userAccountControl; }
            set { _userAccountControl = value; }
        }

        private string _userPrincipleName;
        public string userPrincipleName
        {
            get { return _userPrincipleName; }
            set { _userPrincipleName = value; }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private string _dN;
        public string DN
        {
            get { return _dN; }
            set { _dN = value; }
        }

        private string _cN;
        public string CN
        {
            get { return _cN; }
            set { _cN = value; }
        }

        private string _isOff;
        public string IsOff
        {
            get { return _isOff; }
            set { _isOff = value; }
        }

        private string _compId;
        public string CompId
        {
            get { return _compId; }
            set { _compId = value; }
        }

        private string _deptId;
        public string DeptId
        {
            get { return _deptId; }
            set { _deptId = value; }
        }

        private string _EmployeeId;
        public string EmployeeId
        {
            get { return _EmployeeId; }
            set { _EmployeeId = value; }
        }
    }
}
