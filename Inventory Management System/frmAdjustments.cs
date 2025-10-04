using inventory_management;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

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

            // Initialize category combobox
            cmbcategory.Items.AddRange(new string[] { "All", "Quantity", "Unit Price" });
            cmbcategory.SelectedIndex = 0;

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

            cmbcategory.SelectedIndexChanged += (s, ev) =>
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
                string category = cmbcategory.SelectedItem?.ToString() ?? "All";

                string query = @"SELECT products, quantity, unitprice, reason, createdby, dateadjusted, timeadjusted 
                                 FROM tbladjustment 
                                 WHERE " + GetDurationWhereClause(duration, selectedDate);

                // Add category filter at database level to exclude records without values
                if (category == "Quantity")
                {
                    query += " AND quantity IS NOT NULL AND quantity != 0";
                }
                else if (category == "Unit Price")
                {
                    query += " AND unitprice IS NOT NULL AND unitprice != 0";
                }

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

                // Apply category filter for column visibility
                ApplyCategoryFilter();

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

        private void ApplyCategoryFilter()
        {
            if (dataGridView1.Columns.Contains("quantity") && dataGridView1.Columns.Contains("unitprice"))
            {
                string category = cmbcategory.SelectedItem?.ToString() ?? "All";

                switch (category)
                {
                    case "All":
                        dataGridView1.Columns["quantity"].Visible = true;
                        dataGridView1.Columns["unitprice"].Visible = true;
                        break;
                    case "Quantity":
                        dataGridView1.Columns["quantity"].Visible = true;
                        dataGridView1.Columns["unitprice"].Visible = false;
                        break;
                    case "Unit Price":
                        dataGridView1.Columns["quantity"].Visible = false;
                        dataGridView1.Columns["unitprice"].Visible = true;
                        break;
                }

                // Update column widths when some columns are hidden
                UpdateColumnWidths();
            }
        }

        private void UpdateColumnWidths()
        {
            // Adjust column widths based on which columns are visible
            int visibleColumnCount = 0;

            if (dataGridView1.Columns.Contains("products") && dataGridView1.Columns["products"].Visible)
                visibleColumnCount++;
            if (dataGridView1.Columns.Contains("quantity") && dataGridView1.Columns["quantity"].Visible)
                visibleColumnCount++;
            if (dataGridView1.Columns.Contains("unitprice") && dataGridView1.Columns["unitprice"].Visible)
                visibleColumnCount++;
            if (dataGridView1.Columns.Contains("reason") && dataGridView1.Columns["reason"].Visible)
                visibleColumnCount++;
            if (dataGridView1.Columns.Contains("createdby") && dataGridView1.Columns["createdby"].Visible)
                visibleColumnCount++;
            if (dataGridView1.Columns.Contains("dateadjusted") && dataGridView1.Columns["dateadjusted"].Visible)
                visibleColumnCount++;

            if (visibleColumnCount > 0)
            {
                // Adjust widths based on number of visible columns
                int baseWidth = dataGridView1.Width - 50; // Leave some margin
                int columnWidth = baseWidth / visibleColumnCount;

                if (dataGridView1.Columns.Contains("products"))
                    dataGridView1.Columns["products"].Width = Math.Max(180, columnWidth);
                if (dataGridView1.Columns.Contains("quantity") && dataGridView1.Columns["quantity"].Visible)
                    dataGridView1.Columns["quantity"].Width = Math.Max(100, columnWidth);
                if (dataGridView1.Columns.Contains("unitprice") && dataGridView1.Columns["unitprice"].Visible)
                    dataGridView1.Columns["unitprice"].Width = Math.Max(120, columnWidth);
                if (dataGridView1.Columns.Contains("reason"))
                    dataGridView1.Columns["reason"].Width = Math.Max(250, columnWidth);
                if (dataGridView1.Columns.Contains("createdby"))
                    dataGridView1.Columns["createdby"].Width = Math.Max(120, columnWidth);
                if (dataGridView1.Columns.Contains("dateadjusted"))
                    dataGridView1.Columns["dateadjusted"].Width = Math.Max(120, columnWidth);
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
            cmbcategory.SelectedIndex = 0;
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

        private void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                // Get all adjustments data for export
                string duration = cmbduration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dateTimePicker1.Value;
                string category = cmbcategory.SelectedItem?.ToString() ?? "All";

                string query = @"SELECT products, quantity, unitprice, reason, createdby, dateadjusted, timeadjusted 
                                 FROM tbladjustment 
                                 WHERE " + GetDurationWhereClause(duration, selectedDate);

                // Add category filter at database level to exclude records without values
                if (category == "Quantity")
                {
                    query += " AND quantity IS NOT NULL AND quantity != 0";
                }
                else if (category == "Unit Price")
                {
                    query += " AND unitprice IS NOT NULL AND unitprice != 0";
                }

                query += " ORDER BY dateadjusted DESC, timeadjusted DESC";

                DataTable exportData = adjustments.GetData(query);

                if (exportData.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export for the selected criteria.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files|*.csv|Excel Files|*.xlsx";
                saveDialog.Title = "Export Adjustments Report";
                string durationText = duration.ToLower().Replace(" ", "");
                string dateText = selectedDate.ToString("yyyy-MM-dd");
                saveDialog.FileName = $"AdjustmentsReport_{durationText}_{dateText}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();
                    if (extension == ".csv")
                    {
                        ExportToCSV(exportData, saveDialog.FileName, duration, selectedDate, category);
                    }
                    else
                    {
                        string csvFileName = Path.ChangeExtension(saveDialog.FileName, ".csv");
                        ExportToCSV(exportData, csvFileName, duration, selectedDate, category);
                        MessageBox.Show($"Data exported as CSV: {csvFileName}\n\nYou can open this file in Excel and save it as .xlsx format if needed.",
                            "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(DataTable data, string fileName, string duration, DateTime selectedDate, string category)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Add title and header information
                csvContent.AppendLine("AMGC PHARMACY - ADJUSTMENTS REPORT");
                csvContent.AppendLine($"Duration: {duration}, Date: {selectedDate:MM/dd/yyyy}, Category: {category}, Generated: {DateTime.Now:MM/dd/yyyy hh:mm:ss tt}");
                csvContent.AppendLine(); // Empty line

                // Add column headers based on category
                if (category == "All")
                {
                    csvContent.AppendLine("Product,Quantity,Unit Price,Reason,Created By,Date Adjusted,Time Adjusted");
                }
                else if (category == "Quantity")
                {
                    csvContent.AppendLine("Product,Quantity,Reason,Created By,Date Adjusted,Time Adjusted");
                }
                else if (category == "Unit Price")
                {
                    csvContent.AppendLine("Product,Unit Price,Reason,Created By,Date Adjusted,Time Adjusted");
                }

                // Add data rows
                foreach (DataRow row in data.Rows)
                {
                    string product = EscapeCSVField(row["products"].ToString());
                    string quantity = row["quantity"].ToString();
                    string unitPrice = row["unitprice"].ToString();
                    string reason = EscapeCSVField(row["reason"].ToString());
                    string createdBy = EscapeCSVField(row["createdby"].ToString());
                    string dateAdjusted = EscapeCSVField(row["dateadjusted"].ToString());
                    string timeAdjusted = EscapeCSVField(row["timeadjusted"].ToString());

                    if (category == "All")
                    {
                        // Format quantity with + sign for positive values
                        if (int.TryParse(quantity, out int qty) && qty != 0)
                        {
                            quantity = qty > 0 ? $"+{qty}" : qty.ToString();
                        }

                        // Format unit price as currency
                        if (decimal.TryParse(unitPrice, out decimal price) && price != 0)
                        {
                            unitPrice = price.ToString("F2");
                        }

                        csvContent.AppendLine($"{product},{quantity},{unitPrice},{reason},{createdBy},{dateAdjusted},{timeAdjusted}");
                    }
                    else if (category == "Quantity")
                    {
                        // Format quantity with + sign for positive values
                        if (int.TryParse(quantity, out int qty) && qty != 0)
                        {
                            quantity = qty > 0 ? $"+{qty}" : qty.ToString();
                            csvContent.AppendLine($"{product},{quantity},{reason},{createdBy},{dateAdjusted},{timeAdjusted}");
                        }
                    }
                    else if (category == "Unit Price")
                    {
                        // Format unit price as currency
                        if (decimal.TryParse(unitPrice, out decimal price) && price != 0)
                        {
                            unitPrice = price.ToString("F2");
                            csvContent.AppendLine($"{product},{unitPrice},{reason},{createdBy},{dateAdjusted},{timeAdjusted}");
                        }
                    }
                }

                // Write to file
                File.WriteAllText(fileName, csvContent.ToString(), Encoding.UTF8);

                MessageBox.Show($"Adjustments report exported successfully!\nLocation: {fileName}\n" +
                    $"Records: {data.Rows.Count}\nDuration: {duration}\nCategory: {category}",
                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Log the export action
                adjustments.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                    "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                    "', 'EXPORT', 'ADJUSTMENT MANAGEMENT', 'CSV Export: " + duration + " - " + selectedDate.ToString("MM/dd/yyyy") + "', '" + username.Replace("'", "''") + "')");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating CSV file: " + ex.Message, "CSV Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string EscapeCSVField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            if (field.Contains(",") || field.Contains("\n") || field.Contains("\r") || field.Contains("\""))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }
            return field;
        }
    }
}