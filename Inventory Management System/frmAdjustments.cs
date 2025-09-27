using inventory_management;
using System;
using System.Data;
using System.Linq;
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
                newForm.FormClosed += (s, args) => { LoadAdjustments(); };
                newForm.Show();
            }
        }

        private void frmAdjustments_Load(object sender, EventArgs e)
        {
            // Initialize duration combobox
            cmbduration.Items.AddRange(new string[] { "Daily", "Weekly", "Monthly", "Yearly", "All Records" });
            cmbduration.SelectedIndex = 0;

            // DatePicker setup: default to today, format MM/dd/yyyy
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";
            dateTimePicker1.Value = DateTime.Today;

            // Event handlers for filtering
            dateTimePicker1.ValueChanged += (s, ev) =>
            {
                currentPage = 1;
                LoadAdjustments();
            };

            cmbduration.SelectedIndexChanged += (s, ev) =>
            {
                currentPage = 1;
                LoadAdjustments();
            };

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
                string duration = cmbduration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dateTimePicker1.Value;

                string query = @"SELECT products, quantity, unitprice, reason, createdby, dateadjusted, timeadjusted 
                                 FROM tbladjustment 
                                 WHERE " + GetDurationWhereClause(duration, selectedDate);

                if (!string.IsNullOrEmpty(search))
                {
                    query += " AND (products LIKE '%" + search.Replace("'", "''") + "%' OR reason LIKE '%" + search.Replace("'", "''") + "%') ";
                }
                query += " ORDER BY dateadjusted DESC, timeadjusted DESC";

                DataTable dtAll = adjustments.GetData(query);

                totalRecords = dtAll.Rows.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                if (totalPages == 0) totalPages = 1;
                if (currentPage > totalPages) currentPage = totalPages;

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

                    // Format quantity to show + sign for positive values
                    dataGridView1.Columns["quantity"].DefaultCellStyle.Format = "+0;-#";
                }
                if (dataGridView1.Columns.Contains("unitprice"))
                {
                    dataGridView1.Columns["unitprice"].HeaderText = "Unit Price";
                    dataGridView1.Columns["unitprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["unitprice"].DefaultCellStyle.Format = "₱#,##0.00";
                    dataGridView1.Columns["unitprice"].Width = 120;
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
                if (dataGridView1.Columns.Contains("timeadjusted"))
                {
                    dataGridView1.Columns["timeadjusted"].Visible = false; // Hide time column
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
                    lblPageInfo.Text = $"Page {currentPage} of {totalPages} ({totalRecords} adjustments)";
                }

                UpdateButtonStates();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on LoadAdjustments", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetDurationWhereClause(string duration, DateTime selectedDate)
        {
            switch (duration)
            {
                case "Daily":
                    string dailyDate = selectedDate.ToString("MM/dd/yyyy");
                    return "dateadjusted = '" + dailyDate.Replace("'", "''") + "'";

                case "Weekly":
                    DateTime startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(6);
                    return "STR_TO_DATE(dateadjusted, '%m/%d/%Y') BETWEEN '" + startOfWeek.ToString("yyyy-MM-dd") + "' AND '" + endOfWeek.ToString("yyyy-MM-dd") + "'";

                case "Monthly":
                    DateTime startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                    return "STR_TO_DATE(dateadjusted, '%m/%d/%Y') BETWEEN '" + startOfMonth.ToString("yyyy-MM-dd") + "' AND '" + endOfMonth.ToString("yyyy-MM-dd") + "'";

                case "Yearly":
                    DateTime startOfYear = new DateTime(selectedDate.Year, 1, 1);
                    DateTime endOfYear = new DateTime(selectedDate.Year, 12, 31);
                    return "STR_TO_DATE(dateadjusted, '%m/%d/%Y') BETWEEN '" + startOfYear.ToString("yyyy-MM-dd") + "' AND '" + endOfYear.ToString("yyyy-MM-dd") + "'";

                case "All Records":
                    return "1=1"; // Return all records

                default:
                    return "1=1";
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            txtsearch.Text = "";
            dateTimePicker1.Value = DateTime.Today;
            cmbduration.SelectedIndex = 0;
            LoadAdjustments();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmAddAdjustment(username));
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
                string quantity = dataGridView1.Rows[row].Cells["quantity"].Value?.ToString() ?? "";
                string unitprice = dataGridView1.Rows[row].Cells["unitprice"].Value?.ToString() ?? "";
                string reason = dataGridView1.Rows[row].Cells["reason"].Value.ToString();
                string createdby = dataGridView1.Rows[row].Cells["createdby"].Value.ToString();
                string dateadjusted = dataGridView1.Rows[row].Cells["dateadjusted"].Value.ToString();
                string timeadjusted = dataGridView1.Rows[row].Cells["timeadjusted"].Value.ToString();

                ShowOrActivateForm(() => new frmUpdateAdjustment(
                    product, quantity, unitprice, reason, createdby, username, dateadjusted, timeadjusted));
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
                string quantity = dataGridView1.Rows[row].Cells["quantity"].Value?.ToString() ?? "";
                string unitprice = dataGridView1.Rows[row].Cells["unitprice"].Value?.ToString() ?? "";
                string dateadjusted = dataGridView1.Rows[row].Cells["dateadjusted"].Value.ToString();
                string timeadjusted = dataGridView1.Rows[row].Cells["timeadjusted"].Value.ToString();

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this adjustment?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    // Use dateadjusted and timeadjusted to uniquely identify the record
                    string deleteQuery = "DELETE FROM tbladjustment WHERE products = '" + product.Replace("'", "''") +
                                         "' AND dateadjusted = '" + dateadjusted.Replace("'", "''") +
                                         "' AND timeadjusted = '" + timeadjusted.Replace("'", "''") + "'";

                    adjustments.executeSQL(deleteQuery);

                    if (adjustments.rowAffected > 0)
                    {
                        MessageBox.Show("Adjustment deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log deletion
                        adjustments.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE', 'ADJUSTMENT MANAGEMENT', 'Product: " + product + "', '" + username + "')");
                    }
                    else
                    {
                        MessageBox.Show("No adjustment record was deleted. The record may have been modified or deleted by another user.",
                            "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (e.KeyChar == 13) // Enter key
            {
                btnsearch_Click(sender, e);
            }
        }

        // Handle cell formatting to color code positive/negative quantities
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "quantity" && e.Value != null)
            {
                try
                {
                    // Parse the quantity value
                    if (int.TryParse(e.Value.ToString(), out int quantity))
                    {
                        if (quantity > 0)
                        {
                            e.CellStyle.ForeColor = System.Drawing.Color.Green;
                            e.CellStyle.Font = new System.Drawing.Font(dataGridView1.Font, System.Drawing.FontStyle.Bold);
                        }
                        else if (quantity < 0)
                        {
                            e.CellStyle.ForeColor = System.Drawing.Color.Red;
                            e.CellStyle.Font = new System.Drawing.Font(dataGridView1.Font, System.Drawing.FontStyle.Bold);
                        }
                    }
                }
                catch
                {
                    // Ignore formatting errors
                }
            }
        }

        // New method to handle selection change in data grid
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell != null)
                {
                    int idx = dataGridView1.CurrentCell.RowIndex;
                    if (idx >= 0 && idx < dataGridView1.Rows.Count)
                        row = idx;
                    else
                        row = -1;
                }
                else
                {
                    row = -1;
                }
                UpdateButtonStates();
            }
            catch
            {
                row = -1;
                UpdateButtonStates();
            }
        }
    }
}