using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Regalscan.Data.Common;

namespace LDAPUpdater
{
    public partial class Form_Main : Form
    {
        ArrayList alLDAPObjects = new ArrayList();
        DataTable dtOrganizationalUnits;
        List<LDAPUser> LdapUsers = new List<LDAPUser>();

        public Form_Main()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            try
            {
                string[] args = new string[] { "/update" };

                if (args.Length > 0)
                {
                    foreach (string s in args)
                    {
                        if (s == "/update")
                        {
                            UpdateLDAPObjects();
                            Application.Exit();
                        }
                    }

                }

                dtOrganizationalUnits = OrganizationalUnit.CreateOrganizationalUnitDataTable();
                dtOrganizationalUnits.ReadXml(Application.StartupPath + "\\LDAPOU.xml");
                if (dtOrganizationalUnits.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtOrganizationalUnits.Rows)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = dr["Name"].ToString();
                        item.SubItems.Add(dr["IsOff"].ToString());
                        item.SubItems.Add(dr["CompId"].ToString());
                        item.SubItems.Add(dr["DeptId"].ToString());
                        item.SubItems.Add(dr["LDAPObjectString"].ToString());
                        listView1.Items.Add(item);
                    }
                }


                //GetOrganizationalUnitObjects("OU=Suzhou,OU=帝商科技,DC=regalscan,DC=com");
                //GetOrganizationalUnitObjects("OU=系統部,OU=帝商科技,DC=regalscan,DC=com");
                //GetOrganizationalUnitObjects("OU=業務部,OU=帝商科技,DC=regalscan,DC=com");
                //GetOrganizationalUnitObjects("OU=管理部,OU=帝商科技,DC=regalscan,DC=com");
                //GetOrganizationalUnitObjects("OU=財務部,OU=帝商科技,DC=regalscan,DC=com");            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetOrganizationalUnitObjects(string OuDn, string IsOff, string CompId, string DeptId)
        {
            int idx = OuDn.IndexOf("DC=", 0) + 3;
            string dc = OuDn.Substring(idx, OuDn.IndexOf(",DC=", idx) - idx);
            //dc = dc.Replace("DC=", "");
            //dc = dc.Replace(",", ".");
            //int idx = OuDn.IndexOf("DC");
            //string dc = OuDn.Substring(idx, OuDn.Length - idx);
            //dc = dc.Replace("DC=", "");
            //dc = dc.Replace(",", ".");
            ArrayList alObjects = new ArrayList();

            try
            {
                DirectoryEntry directoryObject = new DirectoryEntry("LDAP://" + OuDn);

                if (directoryObject.SchemaClassName.ToLower() == "user")
                {
                    LDAPUser user = new LDAPUser();
                    user.sAMAccountName = Convert.ToString(directoryObject.Properties["sAMAccountName"].Value);
                    user.sAMAccountType = Convert.ToString(directoryObject.Properties["sAMAccountType"].Value);
                    user.ObjectClass = Convert.ToString(directoryObject.Properties["ObjectClass"].Value);
                    user.objectGUID = directoryObject.Guid;
                    user.givenName = Convert.ToString(directoryObject.Properties["givenName"].Value);
                    
                    //if (directoryObject.Properties["sn"].Value != null)
                        user.sn = Convert.ToString(directoryObject.Properties["sn"].Value);
                    
                    user.displayName = Convert.ToString(directoryObject.Properties["displayName"].Value);
                    user.description = Convert.ToString(directoryObject.Properties["description"].Value);
                    user.mail = Convert.ToString(directoryObject.Properties["mail"].Value);
                    user.userAccountControl = Convert.ToString(directoryObject.Properties["userAccountControl"].Value);
                    user.userPrincipleName = Convert.ToString(directoryObject.Properties["userPrincipleName"].Value);
                    user.DN = Convert.ToString(directoryObject.Properties["DN"].Value);
                    user.CN = Convert.ToString(directoryObject.Properties["CN"].Value);
                    //user.m = directoryObject.Properties["description"].Value.ToString();  
                    user.Path = Convert.ToString(directoryObject.Path);
                    user.Domain = dc;
                    user.DomainAccount = dc + @"\" + user.sAMAccountName;
                    user.IsOff = IsOff;
                    user.CompId = CompId;
                    user.DeptId = DeptId;
                    user.EmployeeId = Convert.ToString(directoryObject.Properties["description"].Value);
                    LdapUsers.Add(user);

                }
                else
                {
                    string s = "";
                }
                    //alLDAPObjects.Add(directoryObject);
                    //listBox1.Items.Add(directoryObject.Path);
                    //Console.WriteLine(directoryObject.Path);

                foreach (DirectoryEntry child in directoryObject.Children)
                {
                    string childPath = child.Path.ToString();
                    alObjects.Add(childPath.Remove(0, 7));
                    //remove the LDAP prefix from the path


                    child.Close();
                    child.Dispose();
                }

                directoryObject.Close();
                directoryObject.Dispose();

                if (alObjects.Count > 0)
                    for (int i = 0; i < alObjects.Count; i++)
                    {
                        GetOrganizationalUnitObjects(alObjects[i].ToString(), IsOff, CompId, DeptId);
                    }
            }
            catch (DirectoryServicesCOMException e)
            {
                Console.WriteLine("An Error Occurred: " + e.Message.ToString());
            }

        }

        private void UpdateLDAPObjects()
        {
            dtOrganizationalUnits = OrganizationalUnit.CreateOrganizationalUnitDataTable();
            dtOrganizationalUnits.ReadXml(Application.StartupPath + "\\LDAPOU.xml");
            if (dtOrganizationalUnits.Rows.Count > 0)
            {
                foreach (DataRow dr in dtOrganizationalUnits.Rows)
                {
                    string xIsOff = dr["IsOff"].ToString() == "Y" ? "Y" : "N";
                    string xCompId = dr["CompId"].ToString();
                    string xDeptId = dr["DeptId"].ToString();
                    GetOrganizationalUnitObjects(dr["LDAPObjectString"].ToString(), xIsOff, xCompId, xDeptId);
                }
            }

            if (LdapUsers.Count > 0)
            {
                RGDBManager.ConnectionString = Properties.Settings.Default.RegalAttendance;
#if DEBUG
                RGDBManager.ConnectionString = Properties.Settings.Default.RegalAttendanceTest;
#endif
                RGDBManager.ProviderName = "System.Data.SqlClient";
                if (LdapUsers.Count > 0)
                {
                    try
                    {
                        RGDBManager.BeginTranscation();

                        foreach (LDAPUser user in LdapUsers)
                        {
                            if (int.Parse(RGDBManager.ExecuteScalar("SELECT COUNT(*) FROM LDAPUsers WHERE objectGUID=@objectGUID",
                                new DbParameter[] { RGDBManager.CreateParameter("@objectGUID", DbType.Guid, user.objectGUID) }).ToString()) > 0)
                            {
                                RGDBManager.ExecuteNonQuery("UPDATE LDAPUsers SET Domain=@Domain,sAMAccountName=@sAMAccountName,sAMAccountType=@sAMAccountType,ObjectClass=@ObjectClass,givenName=@givenName,sn=@sn,displayName=@displayName,description=@description,mail=@mail,userAccountControl=@userAccountControl,userPrincipleName=@userPrincipleName,Path=@Path,DN=@DN,CN=@CN,IsOff=@IsOff,CompId=@CompId,DeptId=@DeptId,ERP_UID=@ERP_UID WHERE objectGUID=@objectGUID",
                                    new DbParameter[] { 
                                            RGDBManager.CreateParameter("@DomainAccount", DbType.String, user.DomainAccount),
                                            RGDBManager.CreateParameter("@Domain", DbType.String, user.Domain),
                                            RGDBManager.CreateParameter("@objectGUID", DbType.Guid, user.objectGUID),
                                            RGDBManager.CreateParameter("@sAMAccountName", DbType.String, user.sAMAccountName),
                                            RGDBManager.CreateParameter("@sAMAccountType", DbType.String, user.sAMAccountType),
                                            RGDBManager.CreateParameter("@ObjectClass", DbType.String, user.ObjectClass),
                                            RGDBManager.CreateParameter("@givenName", DbType.String, user.givenName),
                                            RGDBManager.CreateParameter("@sn", DbType.String, user.sn),
                                            RGDBManager.CreateParameter("@displayName", DbType.String, user.displayName),
                                            RGDBManager.CreateParameter("@description", DbType.String, user.description),
                                            RGDBManager.CreateParameter("@mail", DbType.String, user.mail),
                                            RGDBManager.CreateParameter("@userAccountControl", DbType.String, user.userAccountControl),
                                            RGDBManager.CreateParameter("@userPrincipleName", DbType.String, user.userPrincipleName),
                                            RGDBManager.CreateParameter("@Path", DbType.String, user.Path),
                                            RGDBManager.CreateParameter("@DN", DbType.String, user.DN),
                                            RGDBManager.CreateParameter("@CN", DbType.String, user.CN),
                                            RGDBManager.CreateParameter("@IsOff", DbType.String, user.IsOff),
                                            RGDBManager.CreateParameter("@CompId", DbType.String, user.CompId),
                                            RGDBManager.CreateParameter("@DeptId", DbType.String, user.DeptId),
                                            RGDBManager.CreateParameter("@ERP_UID", DbType.String, user.EmployeeId)
                                        });
                            }
                            else
                            {
                                RGDBManager.ExecuteNonQuery("INSERT INTO LDAPUsers(DomainAccount,Domain,objectGUID,sAMAccountName,sAMAccountType,ObjectClass,givenName,sn,displayName,description,mail,userAccountControl,userPrincipleName,Path,DN,CN,IsOff,CompId,DeptId,ERP_UID)VALUES(@DomainAccount,@Domain,@objectGUID,@sAMAccountName,@sAMAccountType,@ObjectClass,@givenName,@sn,@displayName,@description,@mail,@userAccountControl,@userPrincipleName,@Path,@DN,@CN,@IsOff,@CompId,@DeptId,@ERP_UID)",
                                    new DbParameter[] { 
                                            RGDBManager.CreateParameter("@DomainAccount", DbType.String, user.DomainAccount),
                                            RGDBManager.CreateParameter("@Domain", DbType.String, user.Domain),
                                            RGDBManager.CreateParameter("@objectGUID", DbType.Guid, user.objectGUID),
                                            RGDBManager.CreateParameter("@sAMAccountName", DbType.String, user.sAMAccountName),
                                            RGDBManager.CreateParameter("@sAMAccountType", DbType.String, user.sAMAccountType),
                                            RGDBManager.CreateParameter("@ObjectClass", DbType.String, user.ObjectClass),
                                            RGDBManager.CreateParameter("@givenName", DbType.String, user.givenName),
                                            RGDBManager.CreateParameter("@sn", DbType.String, user.sn),
                                            RGDBManager.CreateParameter("@displayName", DbType.String, user.displayName),
                                            RGDBManager.CreateParameter("@description", DbType.String, user.description),
                                            RGDBManager.CreateParameter("@mail", DbType.String, user.mail),
                                            RGDBManager.CreateParameter("@userAccountControl", DbType.String, user.userAccountControl),
                                            RGDBManager.CreateParameter("@userPrincipleName", DbType.String, user.userPrincipleName),
                                            RGDBManager.CreateParameter("@Path", DbType.String, user.Path),
                                            RGDBManager.CreateParameter("@DN", DbType.String, user.DN),
                                            RGDBManager.CreateParameter("@CN", DbType.String, user.CN),
                                            RGDBManager.CreateParameter("@IsOff", DbType.String, user.IsOff),
                                            RGDBManager.CreateParameter("@CompId", DbType.String, user.CompId),
                                            RGDBManager.CreateParameter("@DeptId", DbType.String, user.DeptId),
                                            RGDBManager.CreateParameter("@ERP_UID", DbType.String, user.EmployeeId)
                                        });
                            }

                        }

                        RGDBManager.CommitTranscation();
                    }
                    catch
                    {
                        RGDBManager.RollBackTranscation();
                        throw;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0 && listView1.FocusedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this item?", "Question", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dtOrganizationalUnits.Rows.RemoveAt(listView1.FocusedItem.Index);
                    listView1.Items.Remove(listView1.FocusedItem);
                    dtOrganizationalUnits.WriteXml("LDAPOU.xml");
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form_Editor fmEditor = new Form_Editor();
            if (fmEditor.ShowDialog() == DialogResult.Yes)
            {
                dtOrganizationalUnits.Rows.Add(new object[] { fmEditor.ObjectName, fmEditor.IsOff, fmEditor.LDAPObjectString, fmEditor.CompId, fmEditor.DeptId });
                dtOrganizationalUnits.WriteXml("LDAPOU.xml");

                ListViewItem item = new ListViewItem();
                item.Text = fmEditor.ObjectName;
                item.SubItems.Add(fmEditor.IsOff);
                item.SubItems.Add(fmEditor.CompId);
                item.SubItems.Add(fmEditor.DeptId);
                item.SubItems.Add(fmEditor.LDAPObjectString);
                listView1.Items.Add(item);
            }
            fmEditor.Dispose();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                Form_Editor fmEditor = new Form_Editor();
                fmEditor.ObjectIndex = listView1.FocusedItem.Index;
                fmEditor.ObjectName = listView1.FocusedItem.Text;
                fmEditor.IsOff = listView1.FocusedItem.SubItems[1].Text;
                fmEditor.CompId = listView1.FocusedItem.SubItems[2].Text;
                fmEditor.DeptId = listView1.FocusedItem.SubItems[3].Text;
                fmEditor.LDAPObjectString = listView1.FocusedItem.SubItems[4].Text;
                if (fmEditor.ShowDialog() == DialogResult.Yes)
                {
                    dtOrganizationalUnits.Rows[fmEditor.ObjectIndex]["Name"] = fmEditor.ObjectName;
                    dtOrganizationalUnits.Rows[fmEditor.ObjectIndex]["IsOff"] = fmEditor.IsOff;
                    dtOrganizationalUnits.Rows[fmEditor.ObjectIndex]["LDAPObjectString"] = fmEditor.LDAPObjectString;
                    dtOrganizationalUnits.Rows[fmEditor.ObjectIndex]["CompId"] = fmEditor.CompId;
                    dtOrganizationalUnits.Rows[fmEditor.ObjectIndex]["DeptId"] = fmEditor.DeptId;
                    dtOrganizationalUnits.WriteXml("LDAPOU.xml");

                    listView1.FocusedItem.Text = fmEditor.ObjectName;
                    listView1.FocusedItem.SubItems[1].Text = fmEditor.IsOff;
                    listView1.FocusedItem.SubItems[2].Text = fmEditor.CompId;
                    listView1.FocusedItem.SubItems[3].Text = fmEditor.DeptId;
                    listView1.FocusedItem.SubItems[4].Text = fmEditor.LDAPObjectString;
                }
                fmEditor.Dispose();
            }
        }
    }
}