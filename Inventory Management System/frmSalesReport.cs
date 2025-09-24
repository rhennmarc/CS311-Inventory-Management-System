using inventory_management;
using System;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmSalesReport : Form
    {
        private string username;
        private string usertype;
        private int row = -1;
        private int currentPage = 1;
        private int pageSize = 10;   // rows per page
        private int totalRecords = 0;
        private int totalPages = 0;

        Class1 sales = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmSalesReport(string username, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.usertype = usertype;

            // DatePicker setup: default to today, format MM/dd/yyyy
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";
            dateTimePicker1.Value = DateTime.Today;

            // Respond immediately when date changes (no button click)
            dateTimePicker1.ValueChanged += (s, e) =>
            {
                currentPage = 1;
                txtsearch.Text = ""; // reset search if you want; comment out if you prefer to keep search text
                LoadSales();
            };

            // Enter key on search box triggers search (within the selected date)
            txtsearch.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)13) // Enter
                {
                    currentPage = 1;
                    LoadSales(txtsearch.Text.Trim());
                }
            };

            // Keep selection in sync (handles keyboard/mouse selection changes)
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void frmSalesReport_Load(object sender, EventArgs e)
        {
            // Initial load for today's date
            LoadSales();
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = row >= 0 && row < dataGridView1.Rows.Count;
            btndelete.Enabled = hasSelection && usertype != null && usertype.ToUpperInvariant() == "ADMINISTRATOR";
            btndeleteall.Enabled = usertype != null && usertype.ToUpperInvariant() == "ADMINISTRATOR";
        }

        /// <summary>
        /// Loads sales for the currently selected date (MM/dd/yyyy). If a search string is provided,
        /// it filters only within that date. Paging is applied to the filtered (displayed) set.
        /// The grand total textbox is computed for the whole date (ignoring search).
        /// </summary>
        private void LoadSales(string search = "")
        {
            try
            {
                string selectedDate = dateTimePicker1.Value.ToString("MM/dd/yyyy");

                // Query for display (date + optional search) — include discounted column
                string where = "WHERE datecreated = '" + selectedDate.Replace("'", "''") + "' ";
                if (!string.IsNullOrEmpty(search))
                {
                    string s = search.Replace("'", "''");
                    where += "AND (products LIKE '%" + s + "%' OR createdby LIKE '%" + s + "%' OR discounted LIKE '%" + s + "%') ";
                }
                string query = "SELECT products, quantity, totalcost, discounted, datecreated, timecreated, createdby FROM tblsales " +
                               where + " ORDER BY timecreated DESC";

                DataTable dtAll = sales.GetData(query);

                // Paging
                totalRecords = dtAll.Rows.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                if (totalPages == 0) totalPages = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                DataTable dtPage = dtAll.Clone();
                int startIndex = (currentPage - 1) * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalRecords);
                for (int i = startIndex; i < endIndex; i++)
                {
                    dtPage.ImportRow(dtAll.Rows[i]);
                }

                dataGridView1.DataSource = dtPage;

                // Styling & columns
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView1.RowTemplate.Height = 28;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.DefaultCellStyle.Padding = new Padding(4);
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                if (dataGridView1.Columns.Contains("products"))
                {
                    dataGridView1.Columns["products"].HeaderText = "Product";
                    dataGridView1.Columns["products"].Width = 180;
                }
                if (dataGridView1.Columns.Contains("quantity"))
                {
                    dataGridView1.Columns["quantity"].HeaderText = "Qty";
                    dataGridView1.Columns["quantity"].Width = 80;
                    dataGridView1.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dataGridView1.Columns.Contains("totalcost"))
                {
                    dataGridView1.Columns["totalcost"].HeaderText = "Total";
                    dataGridView1.Columns["totalcost"].Width = 110;
                    dataGridView1.Columns["totalcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                // NEW: discounted column
                if (dataGridView1.Columns.Contains("discounted"))
                {
                    dataGridView1.Columns["discounted"].HeaderText = "Discounted";
                    dataGridView1.Columns["discounted"].Width = 90;
                    dataGridView1.Columns["discounted"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dataGridView1.Columns.Contains("datecreated"))
                {
                    dataGridView1.Columns["datecreated"].HeaderText = "Date";
                    dataGridView1.Columns["datecreated"].Width = 100;
                }
                if (dataGridView1.Columns.Contains("timecreated"))
                {
                    dataGridView1.Columns["timecreated"].HeaderText = "Time";
                    dataGridView1.Columns["timecreated"].Width = 90;
                    dataGridView1.Columns["timecreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dataGridView1.Columns.Contains("createdby"))
                {
                    dataGridView1.Columns["createdby"].HeaderText = "Created By";
                    dataGridView1.Columns["createdby"].Width = 140;
                }

                // Clear selection and reset row index
                dataGridView1.ClearSelection();
                row = -1;
                UpdateButtonStates();

                // Calculate and display grand total for the selected date (ignores search)
                CalculateTotalForDate(selectedDate);

                // Page info
                if (totalRecords == 0)
                {
                    lblPageInfo.Text = "No records found";
                    currentPage = 1;
                }
                else
                {
                    lblPageInfo.Text = $"Page {currentPage} of {totalPages} ({totalRecords} records)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on LoadSales", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Calculates the sum of totalcost for all sales on the given date (ignores search).
        /// Handles totalcost stored as varchar (with or without currency symbols).
        /// </summary>
        private void CalculateTotalForDate(string dateMMDDYYYY)
        {
            try
            {
                decimal grandTotal = 0m;
                string totalQuery = "SELECT totalcost FROM tblsales WHERE datecreated = '" + dateMMDDYYYY.Replace("'", "''") + "'";
                DataTable dtTotals = sales.GetData(totalQuery);
                foreach (DataRow dr in dtTotals.Rows)
                {
                    if (dr["totalcost"] != null && dr["totalcost"] != DBNull.Value)
                    {
                        string raw = dr["totalcost"].ToString();
                        string cleaned = Regex.Replace(raw, @"[^\d\.\-]", ""); // remove currency symbols, commas, spaces
                        if (!string.IsNullOrWhiteSpace(cleaned))
                        {
                            decimal value;
                            // Try parse with invariant culture (dot decimal). If fails, try current culture.
                            if (decimal.TryParse(cleaned, NumberStyles.Number | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value) ||
                                decimal.TryParse(cleaned, NumberStyles.Number | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out value))
                            {
                                grandTotal += value;
                            }
                        }
                    }
                }
                // Format with currency symbol (₱)
                txttotal.Text = "₱" + grandTotal.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on CalculateTotalForDate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txttotal.Text = "₱0.00";
            }
        }

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

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadSales(txtsearch.Text.Trim());
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            txtsearch.Text = "";
            dateTimePicker1.Value = DateTime.Today; // reset to today's date
            LoadSales();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadSales(txtsearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadSales(txtsearch.Text.Trim());
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (usertype == null || usertype.ToUpperInvariant() != "ADMINISTRATOR")
                {
                    MessageBox.Show("Only ADMINISTRATOR can delete records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a sale first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow sel = dataGridView1.Rows[row];
                string products = sel.Cells["products"].Value?.ToString() ?? "";
                string datecreated = sel.Cells["datecreated"].Value?.ToString() ?? "";
                string timecreated = sel.Cells["timecreated"].Value?.ToString() ?? "";
                string createdby = sel.Cells["createdby"].Value?.ToString() ?? "";

                DialogResult dr = MessageBox.Show($"Delete selected sale?\n{products} - {datecreated} {timecreated}", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    // Use multiple fields to uniquely identify the row (avoid accidental deletion)
                    string del = "DELETE FROM tblsales WHERE products = '" + products.Replace("'", "''") +
                                 "' AND datecreated = '" + datecreated.Replace("'", "''") +
                                 "' AND timecreated = '" + timecreated.Replace("'", "''") +
                                 "' AND createdby = '" + createdby.Replace("'", "''") + "' LIMIT 1";
                    sales.executeSQL(del);
                    if (sales.rowAffected > 0)
                    {
                        MessageBox.Show("Sale deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Optionally log deletion similar to your other forms
                        sales.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE', 'SALES REPORT', '" + products.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");
                    }

                    // Reload for current date and search
                    LoadSales(txtsearch.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on btndelete_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndeleteall_Click(object sender, EventArgs e)
        {
            try
            {
                if (usertype == null || usertype.ToUpperInvariant() != "ADMINISTRATOR")
                {
                    MessageBox.Show("Only ADMINISTRATOR can delete records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedDate = dateTimePicker1.Value.ToString("MM/dd/yyyy");
                DialogResult dr = MessageBox.Show($"Are you sure you want to delete ALL sales for {selectedDate}? This cannot be undone.", "Delete All Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    string delAll = "DELETE FROM tblsales WHERE datecreated = '" + selectedDate.Replace("'", "''") + "'";
                    sales.executeSQL(delAll);
                    if (sales.rowAffected > 0)
                    {
                        MessageBox.Show($"{sales.rowAffected} sale(s) deleted for {selectedDate}.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sales.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE ALL', 'SALES REPORT', 'ALL SALES ON " + selectedDate.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");
                    }
                    else
                    {
                        MessageBox.Show("No records to delete for that date.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadSales();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on btndeleteall_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
