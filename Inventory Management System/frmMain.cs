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
    public partial class frmMain : Form
    {
        private string username;
        private string usertype; // ✅ store usertype

        public frmMain(string username, string usertype)
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "Username: " + username;
            toolStripStatusLabel2.Text = "Usertype: " + usertype;
            this.username = username;
            this.usertype = usertype;

            // ✅ Control visibility based on usertype
            if (usertype.ToUpper() == "PHARMACIST")
            {
                accountToolStripMenuItem.Visible = false;   // hide Accounts
                viewLogsToolStripMenuItem.Visible = false;  // hide Logs
            }
            else if (usertype.ToUpper() == "ADMINISTRATOR")
            {
                accountToolStripMenuItem.Visible = true;
                viewLogsToolStripMenuItem.Visible = true;
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                frmLogin login = new frmLogin();
                login.Show();
                this.Close();
            }
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccounts accountsForm = new frmAccounts(username);
            accountsForm.MdiParent = this;
            accountsForm.Show();
        }

        private void posToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: POS form
        }

        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSuppliers suppliersForm = new frmSuppliers(username);
            suppliersForm.MdiParent = this;
            suppliersForm.Show();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Sales form
        }

        private void viewLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogs LogsForm = new frmLogs(username);
            LogsForm.MdiParent = this;
            LogsForm.Show();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProducts productsForm = new frmProducts(username);
            productsForm.MdiParent = this;
            productsForm.Show();
        }

        private void adjustmentstoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // TODO: Adjustments form
        }
    }
}
