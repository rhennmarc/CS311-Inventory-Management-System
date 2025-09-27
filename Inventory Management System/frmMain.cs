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
        private string usertype;

        public frmMain(string username, string usertype)
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "Username: " + username;
            toolStripStatusLabel2.Text = "Usertype: " + usertype;
            this.username = username;
            this.usertype = usertype;

            if (usertype.ToUpper() == "PHARMACIST")
            {
                accountToolStripMenuItem.Visible = false;
                viewLogsToolStripMenuItem.Visible = false;
            }
            else if (usertype.ToUpper() == "ADMINISTRATOR")
            {
                accountToolStripMenuItem.Visible = true;
                viewLogsToolStripMenuItem.Visible = true;
            }
        }

        private void ShowOrActivateForm<T>(Func<T> createForm) where T : Form
        {
            // Check if a form of type T is already open
            var existingForm = this.MdiChildren.FirstOrDefault(f => f is T);
            if (existingForm != null)
            {
                // Form is open, bring it to the front
                existingForm.BringToFront();
                existingForm.WindowState = FormWindowState.Normal; // Ensure it's not minimized
                existingForm.Focus();
            }
            else
            {
                // Create new instance if not open
                var newForm = createForm();
                newForm.MdiParent = this;
                newForm.Show();
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
            ShowOrActivateForm(() => new frmAccounts(username));
        }

        private void posToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmPOS(username));
        }

        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmSuppliers(username));
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmSalesReport(username, usertype));
        }

        private void viewLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmLogs(username));
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmProducts(username));
        }

        private void adjustmentstoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmAdjustments(username));
        }
    }
}