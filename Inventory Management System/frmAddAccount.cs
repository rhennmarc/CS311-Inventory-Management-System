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
    public partial class frmAddAccount : Form
    {
        private string username;
        public frmAddAccount(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        Class1 newaccount = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private int errorcount;
        private void btnadd_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                errorProvider1.SetError(txtusername, "Username is empty.");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                errorProvider1.SetError(txtpassword, "Password is empty.");
                errorcount++;
            }
            if (cmbusertype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbusertype, "Select usertype.");
                errorcount++;
            }
            try
            {
                DataTable dt = newaccount.GetData("SELECT * FROM tblaccounts WHERE username = '" + txtusername.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtusername, "Username is already in use.");
                    errorcount++;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on checking if username existing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to add this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        newaccount.executeSQL("INSERT INTO tblaccounts (username, password, usertype, status, createdby, datecreated) VALUES ('" + txtusername.Text
                            + "' , '" + txtpassword.Text + "' , '" + cmbusertype.Text.ToUpper() + "' , 'ACTIVE' , '"
                            + username + "' , '" + DateTime.Now.ToShortDateString() + "')");
                        if (newaccount.rowAffected > 0)
                        {
                            MessageBox.Show("New account added.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on adding new account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtusername.Clear();
            txtpassword.Clear();
            cmbusertype.SelectedIndex = -1;
        }
    }
}
