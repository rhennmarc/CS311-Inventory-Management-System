using inventory_management;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmSuppliers : Form
    {
        private string username;
        private int row = -1; // Initialize to -1 to indicate no selection
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;

        Class1 suppliers = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmSuppliers(string username)
        {
            InitializeComponent();
            this.username = username;
        }

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
                newForm.FormClosed += (s, args) => { LoadSuppliers(); };
                newForm.Show();
            }
        }

        private void frmSuppliers_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            UpdateButtonStates(); // Update button states on form load
        }

        private void UpdateButtonStates()
        {
            // Enable/disable buttons based on whether a row is selected
            bool hasSelection = row >= 0 && row < dataGridView1.Rows.Count;

            // Assuming your View button is named btnView
            btnView.Enabled = hasSelection;
            btnupdate.Enabled = hasSelection;
            btndelete.Enabled = hasSelection;
            btnAddPO.Enabled = hasSelection;
        }

        private void LoadSuppliers(string search = "")
        {
            try
            {
                string query = "SELECT supplier, description, contactinfo, createdby, datecreated FROM tblsupplier ";
                if (!string.IsNullOrEmpty(search))
                {
                    query += "WHERE supplier LIKE '%" + search + "%' OR description LIKE '%" + search + "%' OR contactinfo LIKE '%" + search + "%' ";
                }
                query += "ORDER BY supplier";

                DataTable dtAll = suppliers.GetData(query);

                totalRecords = dtAll.Rows.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Apply paging
                DataTable dtPage = dtAll.Clone();
                int startIndex = (currentPage - 1) * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalRecords);

                for (int i = startIndex; i < endIndex; i++)
                {
                    dtPage.ImportRow(dtAll.Rows[i]);
                }

                dataGridView1.DataSource = dtPage;

                // Reset row selection when data is reloaded
                row = -1;
                UpdateButtonStates();

                // === Table Formatting ===
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView1.RowTemplate.Height = 28;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;

                dataGridView1.DefaultCellStyle.Padding = new Padding(5);
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // === Column headers + widths (fit inside 796px) ===
                if (dataGridView1.Columns.Contains("supplier"))
                {
                    dataGridView1.Columns["supplier"].HeaderText = "Supplier Name";
                    dataGridView1.Columns["supplier"].Width = 180;
                }
                if (dataGridView1.Columns.Contains("description"))
                {
                    dataGridView1.Columns["description"].HeaderText = "Description";
                    dataGridView1.Columns["description"].Width = 200;
                }
                if (dataGridView1.Columns.Contains("contactinfo"))
                {
                    dataGridView1.Columns["contactinfo"].HeaderText = "Contact Info";
                    dataGridView1.Columns["contactinfo"].Width = 150;
                }
                if (dataGridView1.Columns.Contains("createdby"))
                {
                    dataGridView1.Columns["createdby"].HeaderText = "Created By";
                    dataGridView1.Columns["createdby"].Width = 130;
                }
                if (dataGridView1.Columns.Contains("datecreated"))
                {
                    dataGridView1.Columns["datecreated"].HeaderText = "Date Created";
                    dataGridView1.Columns["datecreated"].Width = 130;
                }

                // === Page info ===
                if (totalRecords == 0)
                {
                    lblPageInfo.Text = "No records found";
                    currentPage = 1;
                }
                else
                {
                    lblPageInfo.Text = $"Page {currentPage} of {totalPages}";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on LoadSuppliers", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadSuppliers();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmAddSupplier(username));
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a supplier to update.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string suppliername = dataGridView1.Rows[row].Cells["supplier"].Value.ToString();
                string description = dataGridView1.Rows[row].Cells["description"].Value.ToString();
                string contactinfo = dataGridView1.Rows[row].Cells["contactinfo"].Value.ToString();

                ShowOrActivateForm(() => new frmUpdateSupplier(suppliername, description, contactinfo, username));
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnupdate_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a valid supplier record first.",
                        "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string suppliername = dataGridView1.Rows[row].Cells["supplier"].Value.ToString();

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    suppliers.executeSQL("DELETE FROM tblsupplier WHERE supplier = '" + suppliername.Replace("'", "''") + "'");

                    if (suppliers.rowAffected > 0)
                    {
                        MessageBox.Show("Supplier deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log deletion
                        suppliers.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE', 'SUPPLIER MANAGEMENT', '" + suppliername + "', '" + username + "')");
                    }

                    LoadSuppliers();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndelete_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddPO_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a supplier first.", "Add Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string suppliername = dataGridView1.Rows[row].Cells["supplier"].Value.ToString();

                ShowOrActivateForm(() => new frmAddPurchaseOrder(username, suppliername));
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnAddPO_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a supplier first.", "View Purchase Orders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string suppliername = dataGridView1.Rows[row].Cells["supplier"].Value.ToString();

                ShowOrActivateForm(() => new frmPurchaseOrders(username, suppliername));
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnView_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Pagination
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadSuppliers(txtsearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadSuppliers(txtsearch.Text.Trim());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Valid row clicked
                {
                    row = e.RowIndex;
                    UpdateButtonStates(); // Update button states when a row is selected
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on dataGridView1_CellClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadSuppliers(txtsearch.Text.Trim());
        }

        private void txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnsearch_Click(sender, e);
            }
        }
    }
}