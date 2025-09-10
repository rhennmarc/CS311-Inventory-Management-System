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
    public partial class frmAccounts : Form
    {
        private string username;
        public frmAccounts(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        Class1 accounts = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");
        private void frmAccounts_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT * FROM tblaccounts ORDER BY username");
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 1)
                {
                    dataGridView1.Columns[1].Visible = false;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on frmAccounts_Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmAccounts_Load(sender, e);
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddAccount addAccountForm = new frmAddAccount(username);
            addAccountForm.FormClosed += (s, args) => {
                frmAccounts_Load(sender, e);
            };
            addAccountForm.Show();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT * FROM tblaccounts WHERE username LIKE '%" + txtsearch.Text + "%' OR usertype LIKE '%" + txtsearch.Text + "%' ORDER BY username");
                dataGridView1.DataSource = dt;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on txtsearch_TextChanged", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnsearch_Click(sender, e);
            }
        }
        private int row;
        

        private void btnedit_Click(object sender, EventArgs e)
        {
            string editusername = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editpassword = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string editusertype = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editstatus = dataGridView1.Rows[row].Cells[3].Value.ToString();
            frmUpdateAccount updateaccountForm = new frmUpdateAccount(editusername, editpassword, editusertype, editstatus, username);

            updateaccountForm.FormClosed += (s, args) => {
                frmAccounts_Load(sender, e);
            };

            updateaccountForm.Show();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    accounts.executeSQL("DELETE FROM tblaccounts WHERE username = '" + dataGridView1.Rows[row].Cells[0].Value.ToString() + "'");
                    if (accounts.rowAffected > 0)
                    {
                        MessageBox.Show("Account deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        accounts.executeSQL("INSERT tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" + DateTime.Now.ToString("dd/MM/yyyy")
                            + "' , '" + DateTime.Now.ToShortTimeString() + "' , 'DELETE', 'ACCOUNTS MANAGEMENT', '" + dataGridView1.Rows[row].Cells[0].Value.ToString() + "', '" + username + "')");
                    }
                    frmAccounts_Load(sender, e);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndelete_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnsearch_Click(sender, e);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = (int)e.RowIndex;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on dataGridView1_CellClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
