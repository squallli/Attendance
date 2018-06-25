using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LDAPUpdater
{
    public partial class Form_Editor : Form
    {
        public int ObjectIndex;
        public string ObjectName;
        public string IsOff;
        public string LDAPObjectString;
        public string CompId;
        public string DeptId;

        public Form_Editor()
        {
            InitializeComponent();
        }

        private void Form_Editor_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = this.ObjectName;
            this.txtLDAPObjectString.Text = this.LDAPObjectString;
            this.txtCompId.Text = this.CompId;
            this.txtDeptId.Text = this.DeptId;

            cbxIsOff.SelectedIndex = IsOff == "Y" ? 0 : 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtLDAPObjectString.Text.Length == 0)
            {
                MessageBox.Show("LDAP Object String cannot be null.");
                return;
            }

            this.ObjectName = textBox1.Text;
            this.IsOff = cbxIsOff.Text;
            this.LDAPObjectString = txtLDAPObjectString.Text;
            this.CompId = txtCompId.Text;
            this.DeptId = txtDeptId.Text;

            DialogResult = DialogResult.Yes;
        }
    }
}