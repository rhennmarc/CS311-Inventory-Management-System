using inventory_management;
using System;
using System.Data;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmAdjustments : Form
    {
        private string username;
        private int row = -1; // Initialize to -1 to indicate no selection
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;

        Class1 adjustments = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmAdjustments(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void frmAdjustments_Load(object sender, EventArgs e)
        {
            LoadAdjustments();
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = row >= 0 && row < dataGridView1.Rows.Count;

            btnupdate.Enabled = hasSelection;
            btndelete.Enabled = hasSelection;
        }

        private void LoadAdjustments(string search = "")
        {
            try
            {
                string query = @"SELECT products, quantity, reason, createdby, dateadjusted 
                                 FROM tbladjustment ";
                if (!string.IsNullOrEmpty(search))
                {
                    query += "WHERE products LIKE '%" + search + "%' OR reason LIKE '%" + search + "%' ";
                }
                query += "ORDER BY dateadjusted DESC";

                DataTable dtAll = adjustments.GetData(query);

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
                if (dataGridView1.Columns.Contains("quantity"))
                {
                    dataGridView1.Columns["quantity"].HeaderText = "Quantity";
                    dataGridView1.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["quantity"].Width = 100;
                }
                if (dataGridView1.Columns.Contains("reason"))
                {
                    dataGridView1.Columns["reason"].HeaderText = "Reason";
                    dataGridView1.Columns["reason"].Width = 250;
                }
                if (dataGridView1.Columns.Contains("createdby"))
                {
                    dataGridView1.Columns["createdby"].HeaderText = "Created By";
                    dataGridView1.Columns["createdby"].Width = 120;
                }
                if (dataGridView1.Columns.Contains("dateadjusted"))
                {
                    dataGridView1.Columns["dateadjusted"].HeaderText = "Date Adjusted";
                    dataGridView1.Columns["dateadjusted"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dataGridView1.Columns["dateadjusted"].Width = 120;
                }

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

                UpdateButtonStates();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on LoadAdjustments", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadAdjustments();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddAdjustment addForm = new frmAddAdjustment(username);
            addForm.FormClosed += (s, args) => { LoadAdjustments(); };
            addForm.Show();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select an adjustment to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string product = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string quantity = dataGridView1.Rows[row].Cells["quantity"].Value.ToString();
                string reason = dataGridView1.Rows[row].Cells["reason"].Value.ToString();
                string createdby = dataGridView1.Rows[row].Cells["createdby"].Value.ToString();

                frmUpdateAdjustment updateForm = new frmUpdateAdjustment(product, quantity, reason, createdby, username);
                updateForm.FormClosed += (s, args) => { LoadAdjustments(); };
                updateForm.Show();
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
                    MessageBox.Show("Please select a valid adjustment record first.",
                        "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string product = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string reason = dataGridView1.Rows[row].Cells["reason"].Value.ToString();

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this adjustment?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    adjustments.executeSQL("DELETE FROM tbladjustment WHERE products = '" + product + "' AND reason = '" + reason + "'");

                    if (adjustments.rowAffected > 0)
                    {
                        MessageBox.Show("Adjustment deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log deletion
                        adjustments.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE', 'ADJUSTMENT MANAGEMENT', '" + product + "', '" + username + "')");
                    }

                    LoadAdjustments();
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
                LoadAdjustments(txtsearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadAdjustments(txtsearch.Text.Trim());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
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
            LoadAdjustments(txtsearch.Text.Trim());
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
