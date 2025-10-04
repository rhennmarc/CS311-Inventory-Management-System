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
        private int row = -1; // Initialize to -1 to indicate no selection

        public frmAccounts(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        Class1 accounts = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private void ShowOrActivateForm<T>(Func<T> createForm) where T : Form
        {
            // Check if a form of type T is already open
            var existingForm = Application.OpenForms.OfType<T>().FirstOrDefault();
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
                newForm.FormClosed += (s, args) => {
                    // Refresh the accounts data when the child form is closed
                    frmAccounts_Load(this, EventArgs.Empty);
                };
                newForm.Show();
            }
        }

        private void frmAccounts_Load(object sender, EventArgs e)
        {
            try
            {
                // Include password in the SELECT query
                DataTable dt = accounts.GetData("SELECT username, password, usertype, status, createdby, datecreated FROM tblaccounts ORDER BY username");
                dataGridView1.DataSource = dt;

                // === Table Styling ===
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.RowTemplate.Height = 28;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;

                // === Rename headers ===
                if (dataGridView1.Columns.Contains("username"))
                    dataGridView1.Columns["username"].HeaderText = "Username";
                if (dataGridView1.Columns.Contains("password"))
                    dataGridView1.Columns["password"].HeaderText = "Password";
                if (dataGridView1.Columns.Contains("usertype"))
                    dataGridView1.Columns["usertype"].HeaderText = "User Type";
                if (dataGridView1.Columns.Contains("status"))
                    dataGridView1.Columns["status"].HeaderText = "Status";
                if (dataGridView1.Columns.Contains("createdby"))
                    dataGridView1.Columns["createdby"].HeaderText = "Created By";
                if (dataGridView1.Columns.Contains("datecreated"))
                    dataGridView1.Columns["datecreated"].HeaderText = "Date Created";

                // Hide password column for security
                if (dataGridView1.Columns.Contains("password"))
                    dataGridView1.Columns["password"].Visible = false;

                // Optional: minimum widths for cleaner spacing
                dataGridView1.Columns["username"].MinimumWidth = 150;
                dataGridView1.Columns["usertype"].MinimumWidth = 120;
                dataGridView1.Columns["status"].MinimumWidth = 100;
                dataGridView1.Columns["createdby"].MinimumWidth = 150;
                dataGridView1.Columns["datecreated"].MinimumWidth = 150;

                // Clear any selection that might have been made automatically
                dataGridView1.ClearSelection();
                row = -1; // Reset row selection
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on frmAccounts_Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            // Enable/disable buttons based on whether a row is selected
            bool hasSelection = row >= 0 && row < dataGridView1.Rows.Count;

            btnedit.Enabled = hasSelection;
            btndelete.Enabled = hasSelection;
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmAccounts_Load(sender, e);
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmAddAccount(username));
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT username, password, usertype, status, createdby, datecreated FROM tblaccounts WHERE username LIKE '%" + txtsearch.Text + "%' OR usertype LIKE '%" + txtsearch.Text + "%' ORDER BY username");
                dataGridView1.DataSource = dt;

                // Hide password column for security
                if (dataGridView1.Columns.Contains("password"))
                    dataGridView1.Columns["password"].Visible = false;

                // Clear selection after search
                dataGridView1.ClearSelection();
                row = -1;
                UpdateButtonStates();
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

        private void btnedit_Click(object sender, EventArgs e)
        {
            if (row < 0 || row >= dataGridView1.Rows.Count)
            {
                MessageBox.Show("Please select an account to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use column names instead of indices to avoid confusion
            string editusername = dataGridView1.Rows[row].Cells["username"].Value?.ToString() ?? "";
            string editpassword = dataGridView1.Rows[row].Cells["password"].Value?.ToString() ?? "";
            string editusertype = dataGridView1.Rows[row].Cells["usertype"].Value?.ToString() ?? "";
            string editstatus = dataGridView1.Rows[row].Cells["status"].Value?.ToString() ?? "";

            // Debug: Show what values we're passing
            Console.WriteLine($"Passing to update form - Username: {editusername}, UserType: {editusertype}, Status: {editstatus}");

            ShowOrActivateForm(() => new frmUpdateAccount(editusername, editpassword, editusertype, editstatus, username));
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select an account to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string usernameToDelete = dataGridView1.Rows[row].Cells["username"].Value?.ToString() ?? "";

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    accounts.executeSQL("DELETE FROM tblaccounts WHERE username = '" + usernameToDelete + "'");
                    if (accounts.rowAffected > 0)
                    {
                        MessageBox.Show("Account deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        accounts.executeSQL("INSERT tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy")
                            + "' , '" + DateTime.Now.ToShortTimeString() + "' , 'DELETE', 'ACCOUNTS MANAGEMENT', '" + usernameToDelete + "', '" + username + "')");
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
                if (e.RowIndex >= 0) // Make sure it's a valid row
                {
                    row = e.RowIndex;
                    UpdateButtonStates();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on dataGridView1_CellClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}