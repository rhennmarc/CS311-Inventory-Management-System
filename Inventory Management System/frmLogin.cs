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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        //declare an object that will be a copy of the class1 file
        Class1 login = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                // First check if username exists
                DataTable dtUser = login.GetData("SELECT * FROM tblaccounts WHERE username = '" + txtusername.Text + "'");

                if (dtUser.Rows.Count == 0)
                {
                    MessageBox.Show("Username is wrong.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // If username exists, check password and status
                DataTable dtLogin = login.GetData("SELECT * FROM tblaccounts WHERE username = '" + txtusername.Text + "' AND password = '" + txtpassword.Text + "'");

                if (dtLogin.Rows.Count == 0)
                {
                    MessageBox.Show("Password is wrong.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // If username and password are correct, check if account is active
                DataTable dtActive = login.GetData("SELECT * FROM tblaccounts WHERE username = '" + txtusername.Text + "' AND password = '" + txtpassword.Text + "' AND status = 'ACTIVE'");

                if (dtActive.Rows.Count == 0)
                {
                    MessageBox.Show("Account is inactive.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // If all validations pass, login successful
                frmMain mainForm = new frmMain(txtusername.Text, dtActive.Rows[0].Field<string>("usertype"));
                mainForm.Show();
                this.Hide();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnlogin.Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtusername.Clear();
            txtpassword.Clear();
            txtusername.Focus();
        }

        private void txtpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnlogin_Click(sender, e);
            }
        }

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
    }
}