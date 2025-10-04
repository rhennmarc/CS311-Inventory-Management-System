using inventory_management;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmProductHistory : Form
    {
        private string productName;
        private string username;
        private Class1 database;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;
        private int selectedRow = -1;

        public frmProductHistory(string productName, string username)
        {
            InitializeComponent();
            this.productName = productName;
            this.username = username;
            this.database = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");
        }

        private void frmProductHistory_Load(object sender, EventArgs e)
        {
            try
            {
                // Set the product name in the label
                lblProductName.Text = $"History for: {productName}";

                // Initialize DataGridView before loading data
                InitializeDataGridView();

                // Load the product history
                LoadProductHistory();

                // Disable delete buttons initially
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product history: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDataGridView()
        {
            // Clear any existing data and columns
            dataGridViewHistory.DataSource = null;
            dataGridViewHistory.Columns.Clear();

            // Create columns manually to ensure they display correctly
            dataGridViewHistory.Columns.Add("UnitCost", "UNIT COST");
            dataGridViewHistory.Columns.Add("DatePeriod", "DATE PERIOD");
            dataGridViewHistory.Columns.Add("CreatedBy", "CREATED BY");
            dataGridViewHistory.Columns.Add("Occurrences", "OCCURRENCES");

            // Basic grid settings
            dataGridViewHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridViewHistory.RowTemplate.Height = 35;
            dataGridViewHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewHistory.MultiSelect = false;
            dataGridViewHistory.ReadOnly = true;
            dataGridViewHistory.AllowUserToAddRows = false;
            dataGridViewHistory.AllowUserToResizeRows = false;
            dataGridViewHistory.RowHeadersVisible = false;
            dataGridViewHistory.BackgroundColor = Color.White;
            dataGridViewHistory.BorderStyle = BorderStyle.None;
            dataGridViewHistory.GridColor = Color.FromArgb(240, 240, 240);

            // Cell styling
            dataGridViewHistory.DefaultCellStyle.Padding = new Padding(8);
            dataGridViewHistory.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dataGridViewHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            dataGridViewHistory.DefaultCellStyle.SelectionForeColor = Color.White;

            // Header styling
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewHistory.ColumnHeadersDefaultCellStyle.Padding = new Padding(8);
            dataGridViewHistory.ColumnHeadersHeight = 40;
            dataGridViewHistory.EnableHeadersVisualStyles = false;

            // Set column widths and alignment
            dataGridViewHistory.Columns["UnitCost"].Width = 130;
            dataGridViewHistory.Columns["UnitCost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewHistory.Columns["UnitCost"].DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            dataGridViewHistory.Columns["DatePeriod"].Width = 300;
            dataGridViewHistory.Columns["DatePeriod"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridViewHistory.Columns["CreatedBy"].Width = 150;
            dataGridViewHistory.Columns["CreatedBy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridViewHistory.Columns["Occurrences"].Width = 120;
            dataGridViewHistory.Columns["Occurrences"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Auto-size the last column to fill remaining space
            dataGridViewHistory.Columns[dataGridViewHistory.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void LoadProductHistory(string search = "")
        {
            try
            {
                // Clear existing rows
                dataGridViewHistory.Rows.Clear();

                // Get all history records for this product, ordered by date and time with PM on top
                string query = $@"SELECT products, unitcost, datecreated, createdby 
                                FROM tblhistory 
                                WHERE products = '{productName.Replace("'", "''")}' 
                                ORDER BY STR_TO_DATE(SUBSTRING_INDEX(datecreated, ' ', 1), '%m/%d/%Y') DESC,
                                         CASE 
                                             WHEN SUBSTRING_INDEX(datecreated, ' ', -1) = 'PM' THEN 1
                                             WHEN SUBSTRING_INDEX(datecreated, ' ', -1) = 'AM' THEN 2
                                             ELSE 3
                                         END,
                                         STR_TO_DATE(SUBSTRING_INDEX(SUBSTRING_INDEX(datecreated, ' ', 2), ' ', -1), '%h:%i:%s') DESC";

                DataTable rawData = database.GetData(query);
                totalRecords = rawData.Rows.Count;

                if (totalRecords == 0)
                {
                    UpdatePageInfo();
                    UpdateButtonStates();
                    return;
                }

                // Process the data to group consecutive same unit costs
                List<HistoryRecord> processedRecords = ProcessHistoryData(rawData);

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(search))
                {
                    processedRecords = processedRecords.Where(r =>
                        r.UnitCost.ToLower().Contains(search.ToLower()) ||
                        r.DateCreated.ToLower().Contains(search.ToLower()) ||
                        r.CreatedBy.ToLower().Contains(search.ToLower())
                    ).ToList();
                }

                totalRecords = processedRecords.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                if (totalPages == 0) totalPages = 1;

                // Apply paging
                var pagedRecords = processedRecords
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Add data to DataGridView manually
                foreach (var record in pagedRecords)
                {
                    int rowIndex = dataGridViewHistory.Rows.Add(
                        record.UnitCost,
                        record.DateCreated,
                        record.CreatedBy,
                        record.Occurrences
                    );

                    // Apply alternating row colors
                    if (rowIndex % 2 == 0)
                    {
                        dataGridViewHistory.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                    else
                    {
                        dataGridViewHistory.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
                    }
                    dataGridViewHistory.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
                }

                // Update page info
                UpdatePageInfo();

                // Clear selection
                dataGridViewHistory.ClearSelection();
                selectedRow = -1;
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product history: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<HistoryRecord> ProcessHistoryData(DataTable rawData)
        {
            var processedRecords = new List<HistoryRecord>();

            if (rawData.Rows.Count == 0)
                return processedRecords;

            // Group by consecutive same unit costs
            string currentUnitCost = "";
            string startDate = "";
            string endDate = "";
            string createdBy = "";
            int consecutiveCount = 0;

            for (int i = 0; i < rawData.Rows.Count; i++)
            {
                string rowUnitCost = rawData.Rows[i]["unitcost"].ToString();
                string rowDate = rawData.Rows[i]["datecreated"].ToString();
                string rowCreatedBy = rawData.Rows[i]["createdby"].ToString();

                if (i == 0)
                {
                    // First record
                    currentUnitCost = rowUnitCost;
                    startDate = rowDate;
                    endDate = rowDate;
                    createdBy = rowCreatedBy;
                    consecutiveCount = 1;
                }
                else if (rowUnitCost == currentUnitCost)
                {
                    // Same unit cost as previous - update end date and count
                    endDate = rowDate;
                    consecutiveCount++;
                }
                else
                {
                    // Unit cost changed - add the previous group to processed records
                    processedRecords.Add(new HistoryRecord
                    {
                        ProductName = productName,
                        UnitCost = FormatCurrency(currentUnitCost),
                        DateCreated = consecutiveCount > 1 ? $"{startDate} to {endDate}" : startDate,
                        CreatedBy = createdBy,
                        Occurrences = consecutiveCount > 1 ? $"{consecutiveCount} times" : "1 time"
                    });

                    // Start new group
                    currentUnitCost = rowUnitCost;
                    startDate = rowDate;
                    endDate = rowDate;
                    createdBy = rowCreatedBy;
                    consecutiveCount = 1;
                }
            }

            // Add the last group
            if (consecutiveCount > 0)
            {
                processedRecords.Add(new HistoryRecord
                {
                    ProductName = productName,
                    UnitCost = FormatCurrency(currentUnitCost),
                    DateCreated = consecutiveCount > 1 ? $"{startDate} to {endDate}" : startDate,
                    CreatedBy = createdBy,
                    Occurrences = consecutiveCount > 1 ? $"{consecutiveCount} times" : "1 time"
                });
            }

            return processedRecords;
        }

        private string FormatCurrency(string amount)
        {
            if (decimal.TryParse(amount.Replace("₱", "").Replace(",", "").Trim(), out decimal value))
            {
                return "₱" + value.ToString("N2");
            }
            return amount;
        }

        private void UpdatePageInfo()
        {
            if (totalRecords == 0)
            {
                lblPageInfo.Text = "No history records found";
            }
            else
            {
                lblPageInfo.Text = $"Page {currentPage} of {totalPages} ({totalRecords} records)";
            }
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = selectedRow >= 0 && selectedRow < dataGridViewHistory.Rows.Count;
            btnDelete.Enabled = hasSelection;
            btnDeleteAll.Enabled = totalRecords > 0;

            // Update pagination buttons
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;

            // Visual feedback for disabled buttons
            btnDelete.BackColor = hasSelection ? Color.FromArgb(192, 57, 43) : Color.FromArgb(200, 200, 200);
            btnDeleteAll.BackColor = totalRecords > 0 ? Color.FromArgb(231, 76, 60) : Color.FromArgb(200, 200, 200);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewHistory.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "No Data",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files|*.csv";
                saveDialog.Title = "Export Product History";
                saveDialog.FileName = $"ProductHistory_{productName.Replace(" ", "_")}_{DateTime.Now:yyyy-MM-dd}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(saveDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data: " + ex.Message, "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(string fileName)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Add title and header information
                csvContent.AppendLine("AMGC PHARMACY - PRODUCT UNIT COST HISTORY");
                csvContent.AppendLine($"Product: {productName}, Generated: {DateTime.Now:MM/dd/yyyy hh:mm:ss tt}");
                csvContent.AppendLine(); // Empty line

                // Add column headers
                csvContent.AppendLine("Unit Cost,Date Period,Created By,Occurrences");

                // Add data rows
                foreach (DataGridViewRow row in dataGridViewHistory.Rows)
                {
                    if (row.IsNewRow) continue;

                    StringBuilder dataRow = new StringBuilder();
                    foreach (DataGridViewColumn column in dataGridViewHistory.Columns)
                    {
                        if (dataRow.Length > 0) dataRow.Append(",");

                        object cellValue = row.Cells[column.Index].Value;
                        string cellText = cellValue?.ToString() ?? "";
                        dataRow.Append(EscapeCSVField(cellText));
                    }
                    csvContent.AppendLine(dataRow.ToString());
                }

                // Write to file
                File.WriteAllText(fileName, csvContent.ToString(), Encoding.UTF8);

                MessageBox.Show($"Product history exported successfully!\nLocation: {fileName}",
                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Log the export action
                database.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                    "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("hh:mm:ss tt") +
                    "', 'EXPORT', 'PRODUCT HISTORY', '" + productName.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating CSV file: " + ex.Message, "CSV Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedRow < 0 || selectedRow >= dataGridViewHistory.Rows.Count)
                {
                    MessageBox.Show("Please select a history record to delete.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the selected record details
                string unitCost = dataGridViewHistory.Rows[selectedRow].Cells["UnitCost"].Value?.ToString() ?? "";
                string datePeriod = dataGridViewHistory.Rows[selectedRow].Cells["DatePeriod"].Value?.ToString() ?? "";

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete this unit cost history?\n\nUnit Cost: {unitCost}\nPeriod: {datePeriod}",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Extract the actual unit cost value without currency formatting
                    string cleanUnitCost = unitCost.Replace("₱", "").Replace(",", "").Trim();

                    // Delete from database - this will delete all records for this product with this unit cost
                    string deleteQuery = $@"DELETE FROM tblhistory 
                                          WHERE products = '{productName.Replace("'", "''")}' 
                                          AND unitcost = '{cleanUnitCost.Replace("'", "''")}'";

                    database.executeSQL(deleteQuery);

                    if (database.rowAffected > 0)
                    {
                        MessageBox.Show("Unit cost history deleted successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log the action
                        database.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("hh:mm:ss tt") +
                            "', 'DELETE', 'PRODUCT HISTORY', '" + productName.Replace("'", "''") + " - " + unitCost.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");

                        // Reload the history
                        LoadProductHistory();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting history record: " + ex.Message, "Deletion Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete ALL unit cost history for '{productName}'? This action cannot be undone.",
                    "Confirm Delete All",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string deleteQuery = $@"DELETE FROM tblhistory 
                                          WHERE products = '{productName.Replace("'", "''")}'";

                    database.executeSQL(deleteQuery);

                    if (database.rowAffected > 0)
                    {
                        MessageBox.Show($"All unit cost history for '{productName}' has been deleted.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log the action
                        database.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("hh:mm:ss tt") +
                            "', 'DELETE ALL', 'PRODUCT HISTORY', '" + productName.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");

                        // Reload the history
                        LoadProductHistory();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting all history records: " + ex.Message, "Deletion Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadProductHistory();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadProductHistory();
            }
        }

        // Fixed selection event handlers
        private void dataGridViewHistory_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewHistory.SelectedRows.Count > 0)
            {
                selectedRow = dataGridViewHistory.SelectedRows[0].Index;
            }
            else
            {
                selectedRow = -1;
            }
            UpdateButtonStates();
        }

        private void dataGridViewHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewHistory.Rows.Count)
            {
                // Clear previous selection
                dataGridViewHistory.ClearSelection();

                // Select the clicked row
                dataGridViewHistory.Rows[e.RowIndex].Selected = true;
                selectedRow = e.RowIndex;

                // Ensure the row is visible
                dataGridViewHistory.CurrentCell = dataGridViewHistory.Rows[e.RowIndex].Cells[0];
            }
            else
            {
                selectedRow = -1;
            }
            UpdateButtonStates();
        }

        // Add these methods for better user experience
        private void dataGridViewHistory_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewHistory.Rows.Count)
            {
                dataGridViewHistory.Cursor = Cursors.Hand;
            }
        }

        private void dataGridViewHistory_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewHistory.Cursor = Cursors.Default;
        }
    }

    // Helper class to represent processed history records
    public class HistoryRecord
    {
        public string ProductName { get; set; }
        public string UnitCost { get; set; }
        public string DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string Occurrences { get; set; }
    }
}