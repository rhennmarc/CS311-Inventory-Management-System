using inventory_management;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmProducts : Form
    {
        private string username;
        private int row = -1; // Initialize to -1 to indicate no selection
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;

        Class1 products = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmProducts(string username)
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
                newForm.FormClosed += (s, args) => { LoadProducts(); };
                newForm.Show();
            }
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            LoadProducts();
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            // Enable/disable buttons based on whether a row is selected
            bool hasSelection = row >= 0 && row < dataGridView1.Rows.Count;

            btnupdate.Enabled = hasSelection;
            btndelete.Enabled = hasSelection;
        }

        private void LoadProducts(string search = "")
        {
            try
            {
                string query = @"SELECT products, description, unitprice, currentstock, supplier, createdby, datecreated 
                         FROM tblproducts ";
                if (!string.IsNullOrEmpty(search))
                {
                    query += "WHERE products LIKE '%" + search + "%' OR description LIKE '%" + search + "%' ";
                }
                query += "ORDER BY products";

                DataTable dtAll = products.GetData(query);

                totalRecords = dtAll.Rows.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Paging
                DataTable dtPage = dtAll.Clone();
                int startIndex = (currentPage - 1) * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalRecords);

                for (int i = startIndex; i < endIndex; i++)
                {
                    dtPage.ImportRow(dtAll.Rows[i]);
                }

                dataGridView1.DataSource = dtPage;

                // === Table Styling ===
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView1.DefaultCellStyle.Padding = new Padding(4);
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.RowTemplate.Height = 28;

                // === Rename & Format Columns ===
                if (dataGridView1.Columns.Contains("products"))
                {
                    dataGridView1.Columns["products"].HeaderText = "Product Name";
                    dataGridView1.Columns["products"].Width = 180;
                }
                if (dataGridView1.Columns.Contains("description"))
                {
                    dataGridView1.Columns["description"].HeaderText = "Description";
                    dataGridView1.Columns["description"].Width = 250;
                }
                if (dataGridView1.Columns.Contains("unitprice"))
                {
                    dataGridView1.Columns["unitprice"].HeaderText = "Unit Price";
                    dataGridView1.Columns["unitprice"].DefaultCellStyle.Format = "C2"; // ₱1,500.00 style
                    dataGridView1.Columns["unitprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["unitprice"].Width = 120;
                }
                if (dataGridView1.Columns.Contains("currentstock"))
                {
                    dataGridView1.Columns["currentstock"].HeaderText = "Stock";
                    dataGridView1.Columns["currentstock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["currentstock"].Width = 100;
                }
                if (dataGridView1.Columns.Contains("supplier"))
                {
                    dataGridView1.Columns["supplier"].HeaderText = "Supplier";
                    dataGridView1.Columns["supplier"].Width = 150;
                }
                if (dataGridView1.Columns.Contains("createdby"))
                {
                    dataGridView1.Columns["createdby"].HeaderText = "Created By";
                    dataGridView1.Columns["createdby"].Width = 120;
                }
                if (dataGridView1.Columns.Contains("datecreated"))
                {
                    dataGridView1.Columns["datecreated"].HeaderText = "Date Created";
                    dataGridView1.Columns["datecreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dataGridView1.Columns["datecreated"].Width = 120;
                }

                // Clear any automatic selection and reset row selection
                dataGridView1.ClearSelection();
                row = -1;

                // === Page Info ===
                if (totalRecords == 0)
                {
                    lblPageInfo.Text = "No records found";
                    currentPage = 1;
                }
                else
                {
                    lblPageInfo.Text = $"Page {currentPage} of {totalPages}";
                }

                // Update button states after clearing selection
                UpdateButtonStates();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on LoadProducts", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadProducts();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmAddProduct(username));
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a product to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string productname = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string description = dataGridView1.Rows[row].Cells["description"].Value.ToString();
                string unitprice = dataGridView1.Rows[row].Cells["unitprice"].Value.ToString();
                string currentstock = dataGridView1.Rows[row].Cells["currentstock"].Value.ToString();
                string supplier = dataGridView1.Rows[row].Cells["supplier"].Value.ToString();

                ShowOrActivateForm(() => new frmUpdateProduct(productname, description, unitprice, currentstock, supplier, username));
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
                    MessageBox.Show("Please select a valid product record first.",
                        "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string productname = dataGridView1.Rows[row].Cells["products"].Value.ToString();

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this product?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    products.executeSQL("DELETE FROM tblproducts WHERE products = '" + productname + "'");

                    if (products.rowAffected > 0)
                    {
                        MessageBox.Show("Product deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log deletion
                        products.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE', 'PRODUCTS MANAGEMENT', '" + productname + "', '" + username + "')");
                    }

                    LoadProducts();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndelete_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Pagination buttons
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadProducts(txtsearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadProducts(txtsearch.Text.Trim());
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

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadProducts(txtsearch.Text.Trim());
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