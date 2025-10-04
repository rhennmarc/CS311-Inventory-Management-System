using inventory_management;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmUpdateAccount : Form
    {
        private string editusername, editpassword, editusertype, editstatus, username;

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to edit this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    editaccount.executeSQL("UPDATE tblaccounts SET password = '" + txtpassword.Text + "' ,usertype = '" + cmbusertype.Text.ToUpper() +
                        "' ,status = '" + cmbstatus.Text.ToUpper() + "' WHERE username = '" + txtusername.Text + "'");
                    if (editaccount.rowAffected > 0)
                    {
                        MessageBox.Show("Account updated.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        editaccount.executeSQL("INSERT tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy")
                            + "' , '" + DateTime.Now.ToShortTimeString() + "' , 'UPDATE', 'ACCOUNTS MANAGEMENT', '" + txtusername.Text + "', '" + username + "')");
                        this.Close();
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnupdate_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public frmUpdateAccount(string editusername, string editpassword, string editusertype, string editstatus, string username)
        {
            InitializeComponent();
            this.editusername = editusername;
            this.editpassword = editpassword;
            this.editusertype = editusertype;
            this.editstatus = editstatus;
            this.username = username;
        }
        Class1 editaccount = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");
        private void cbshowpassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbshowpassword.Checked)
            {
                txtpassword.PasswordChar = '\0';
            }
            else
            {
                txtpassword.PasswordChar = '*';
            }
        }

        private void frmUpdateAccount_Load(object sender, EventArgs e)
        {
            txtusername.Text = editusername;
            txtpassword.Text = editpassword;
            if (editusertype == "ADMINISTRATOR")
            {
                cmbusertype.SelectedIndex = 0;
            }
            else if (editusertype == "PHARMACIST")
            {
                cmbusertype.SelectedIndex = 1;
            }
            else
            {
                cmbusertype.SelectedIndex = -1;
            }
            if (editstatus == "ACTIVE")
            {
                cmbstatus.SelectedIndex = 0;
            }
            else if (editstatus == "INACTIVE")
            {
                cmbstatus.SelectedIndex = 1;
            }
            else
            {
                cmbstatus.SelectedIndex = -1;
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
