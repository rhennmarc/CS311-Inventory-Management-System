using inventory_management;
using System;
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
    public partial class frmPurchaseOrders : Form
    {
        private string username;
        private string selectedSupplier;
        private int row = -1;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;
        Class1 purchaseOrders = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmPurchaseOrders(string username, string supplierName = "")
        {
            InitializeComponent();
            this.username = username;
            this.selectedSupplier = supplierName;

            // Initialize status filter combobox
            InitializeStatusFilter();

            // Set the title label to show selected supplier
            if (!string.IsNullOrEmpty(selectedSupplier))
            {
                lblTitle.Text = $"Purchase Orders - {selectedSupplier}";
            }
            else
            {
                lblTitle.Text = "Purchase Orders - All Suppliers";
            }
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
                newForm.FormClosed += (s, args) => { LoadPurchaseOrders(); };
                newForm.Show();
            }
        }

        private void InitializeStatusFilter()
        {
            // Initialize status filter combobox
            if (cmbstatus != null)
            {
                cmbstatus.Items.Clear();
                cmbstatus.Items.AddRange(new string[] { "All", "Pending", "Received" });
                cmbstatus.SelectedIndex = 0; // Default to "All"

                // Add event handler for status filter changes
                cmbstatus.SelectedIndexChanged += (s, e) =>
                {
                    currentPage = 1;
                    LoadPurchaseOrders(txtsearch.Text.Trim());
                };
            }
        }

        private void UpdateButtonStates()
        {
            // Enable/disable buttons based on whether a row is selected
            bool hasSelection = row >= 0 && row < dataGridView1.Rows.Count;
            btnupdate.Enabled = hasSelection;
            btndelete.Enabled = hasSelection;
            btnreceive.Enabled = hasSelection;

            // Enable receive button only for pending orders
            if (hasSelection && dataGridView1.Rows[row].Cells["status"].Value != null)
            {
                string status = dataGridView1.Rows[row].Cells["status"].Value.ToString();
                btnreceive.Enabled = status.ToUpper() != "RECEIVED";
            }
        }

        private void frmPurchaseOrders_Load(object sender, EventArgs e)
        {
            // Subscribe to selection changed to keep `row` in sync
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            LoadPurchaseOrders();
            UpdateButtonStates();
        }

        private void LoadPurchaseOrders(string search = "")
        {
            try
            {
                string query = "SELECT products, quantity, unitcost, totalcost, status, createdby, datecreated, datereceived, supplier FROM tblpurchase_order ";

                // Build WHERE clause
                string whereClause = "";

                // Supplier filter
                if (!string.IsNullOrEmpty(selectedSupplier))
                {
                    whereClause = "WHERE supplier = '" + selectedSupplier.Replace("'", "''") + "' ";
                }

                // Status filter
                string statusFilter = cmbstatus?.SelectedItem?.ToString() ?? "All";
                if (statusFilter != "All")
                {
                    if (string.IsNullOrEmpty(whereClause))
                    {
                        whereClause = "WHERE ";
                    }
                    else
                    {
                        whereClause += "AND ";
                    }
                    whereClause += "status = '" + statusFilter + "' ";
                }

                // Search filter
                if (!string.IsNullOrEmpty(search))
                {
                    if (string.IsNullOrEmpty(whereClause))
                    {
                        whereClause = "WHERE ";
                    }
                    else
                    {
                        whereClause += "AND ";
                    }
                    whereClause += "(products LIKE '%" + search.Replace("'", "''") + "%' OR status LIKE '%" + search.Replace("'", "''") + "%' OR createdby LIKE '%" + search.Replace("'", "''") + "%') ";
                }

                query += whereClause + "ORDER BY datecreated DESC";

                // Get all matching rows (this dtAll WILL be filtered by search and status)
                DataTable dtAll = purchaseOrders.GetData(query);
                totalRecords = dtAll.Rows.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                if (totalPages == 0) totalPages = 1;

                // Apply paging to display only a page
                DataTable dtPage = dtAll.Clone();
                int startIndex = (currentPage - 1) * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalRecords);
                for (int i = startIndex; i < endIndex; i++)
                {
                    dtPage.ImportRow(dtAll.Rows[i]);
                }
                dataGridView1.DataSource = dtPage;

                // Ensure no leftover selection and reset row
                dataGridView1.ClearSelection();
                row = -1;
                UpdateButtonStates();

                // === Table Formatting ===
                StyleDataGridView();

                // Apply row coloring based on status
                ApplyRowColoring();

                // Calculate and display grand total (based on current filters)
                CalculateGrandTotal(statusFilter);

                // === Page info ===
                if (totalRecords == 0)
                {
                    lblPageInfo.Text = "No records found";
                    currentPage = 1;
                }
                else
                {
                    lblPageInfo.Text = $"Page {currentPage} of {totalPages} ({totalRecords} orders)";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on LoadPurchaseOrders", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.RowTemplate.Height = 28;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DefaultCellStyle.Padding = new Padding(5);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // === Column headers + widths ===
            if (dataGridView1.Columns.Contains("products"))
            {
                dataGridView1.Columns["products"].HeaderText = "Products";
                dataGridView1.Columns["products"].Width = 150;
            }
            if (dataGridView1.Columns.Contains("quantity"))
            {
                dataGridView1.Columns["quantity"].HeaderText = "Quantity";
                dataGridView1.Columns["quantity"].Width = 80;
                dataGridView1.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dataGridView1.Columns.Contains("unitcost"))
            {
                dataGridView1.Columns["unitcost"].HeaderText = "Unit Cost";
                dataGridView1.Columns["unitcost"].Width = 100;
                dataGridView1.Columns["unitcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["unitcost"].DefaultCellStyle.Format = "C2";
            }
            if (dataGridView1.Columns.Contains("totalcost"))
            {
                dataGridView1.Columns["totalcost"].HeaderText = "Total Cost";
                dataGridView1.Columns["totalcost"].Width = 100;
                dataGridView1.Columns["totalcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["totalcost"].DefaultCellStyle.Format = "C2";
            }
            if (dataGridView1.Columns.Contains("status"))
            {
                dataGridView1.Columns["status"].HeaderText = "Status";
                dataGridView1.Columns["status"].Width = 100;
            }
            if (dataGridView1.Columns.Contains("createdby"))
            {
                dataGridView1.Columns["createdby"].HeaderText = "Created By";
                dataGridView1.Columns["createdby"].Width = 120;
            }
            if (dataGridView1.Columns.Contains("datecreated"))
            {
                dataGridView1.Columns["datecreated"].HeaderText = "Date Created";
                dataGridView1.Columns["datecreated"].Width = 130;
            }
            if (dataGridView1.Columns.Contains("datereceived"))
            {
                dataGridView1.Columns["datereceived"].HeaderText = "Date Received";
                dataGridView1.Columns["datereceived"].Width = 130;
            }
            if (dataGridView1.Columns.Contains("supplier"))
            {
                dataGridView1.Columns["supplier"].HeaderText = "Supplier";
                dataGridView1.Columns["supplier"].Width = 120;
                // Hide supplier column if we're viewing orders for a specific supplier
                if (!string.IsNullOrEmpty(selectedSupplier))
                {
                    dataGridView1.Columns["supplier"].Visible = false;
                }
            }
        }

        private void ApplyRowColoring()
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["status"].Value != null)
                    {
                        string status = row.Cells["status"].Value.ToString().ToUpper();
                        switch (status)
                        {
                            case "PENDING":
                                row.DefaultCellStyle.BackColor = Color.LightYellow;
                                row.DefaultCellStyle.ForeColor = Color.Black;
                                break;
                            case "RECEIVED":
                                row.DefaultCellStyle.BackColor = Color.LightGreen;
                                row.DefaultCellStyle.ForeColor = Color.Black;
                                break;
                            default:
                                row.DefaultCellStyle.BackColor = Color.White;
                                row.DefaultCellStyle.ForeColor = Color.Black;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Don't show error for coloring issues, just log or ignore
                System.Diagnostics.Debug.WriteLine("Row coloring error: " + ex.Message);
            }
        }

        /// <summary>
        /// Calculates the grand total (sum of totalcost) for the currently selected supplier and status filter
        /// </summary>
        private void CalculateGrandTotal(string statusFilter = "All")
        {
            try
            {
                decimal grandTotal = 0m;
                // Build a query that fetches all totalcost values for the current filters
                string totalQuery = "SELECT totalcost FROM tblpurchase_order ";
                string whereClause = "";

                // Supplier filter
                if (!string.IsNullOrEmpty(selectedSupplier))
                {
                    whereClause = "WHERE supplier = '" + selectedSupplier.Replace("'", "''") + "' ";
                }

                // Status filter
                if (statusFilter != "All")
                {
                    if (string.IsNullOrEmpty(whereClause))
                    {
                        whereClause = "WHERE ";
                    }
                    else
                    {
                        whereClause += "AND ";
                    }
                    whereClause += "status = '" + statusFilter + "' ";
                }

                totalQuery += whereClause;

                DataTable dtTotals = purchaseOrders.GetData(totalQuery);
                foreach (DataRow dr in dtTotals.Rows)
                {
                    if (dr["totalcost"] != null && dr["totalcost"] != DBNull.Value)
                    {
                        string raw = dr["totalcost"].ToString();
                        // Clean the string: remove currency symbol, commas, spaces, etc.
                        string cleaned = Regex.Replace(raw, @"[^\d\.\-]", "");
                        if (!string.IsNullOrWhiteSpace(cleaned))
                        {
                            decimal value = 0m;
                            if (decimal.TryParse(cleaned, NumberStyles.Number | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value))
                            {
                                grandTotal += value;
                            }
                        }
                    }
                }
                // Format using your local display (prefix ₱ as you used previously)
                txttotal.Text = "₱" + grandTotal.ToString("N2");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on CalculateGrandTotal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txttotal.Text = "₱0.00";
            }
        }

        private void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files|*.csv|Excel Files|*.xlsx";
                saveDialog.Title = "Export Purchase Orders";

                string supplierText = string.IsNullOrEmpty(selectedSupplier) ? "AllSuppliers" : selectedSupplier.Replace(" ", "_");
                string statusText = cmbstatus?.SelectedItem?.ToString() ?? "All";
                string dateText = DateTime.Now.ToString("yyyy-MM-dd");
                saveDialog.FileName = $"PurchaseOrders_{supplierText}_{statusText}_{dateText}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();
                    if (extension == ".csv")
                    {
                        ExportToCSV(saveDialog.FileName);
                    }
                    else
                    {
                        // For .xlsx, we'll export as CSV but suggest they can open it in Excel
                        string csvFileName = Path.ChangeExtension(saveDialog.FileName, ".csv");
                        ExportToCSV(csvFileName);
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

        private void ExportToCSV(string fileName)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Add title and header information
                csvContent.AppendLine("AMGC PHARMACY - PURCHASE ORDERS REPORT");
                string supplierInfo = string.IsNullOrEmpty(selectedSupplier) ? "All Suppliers" : selectedSupplier;
                string statusInfo = cmbstatus?.SelectedItem?.ToString() ?? "All";
                csvContent.AppendLine($"Supplier: {supplierInfo}, Status: {statusInfo}, Generated: {DateTime.Now:MM/dd/yyyy hh:mm:ss tt}");
                csvContent.AppendLine(); // Empty line

                // Add column headers based on visible columns
                StringBuilder headerRow = new StringBuilder();
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    if (column.Visible)
                    {
                        if (headerRow.Length > 0) headerRow.Append(",");
                        headerRow.Append(EscapeCSVField(column.HeaderText));
                    }
                }
                csvContent.AppendLine(headerRow.ToString());

                // Add data rows from the current DataGridView (what user sees)
                decimal totalValue = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    StringBuilder dataRow = new StringBuilder();
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (column.Visible)
                        {
                            if (dataRow.Length > 0) dataRow.Append(",");

                            object cellValue = row.Cells[column.Index].Value;
                            string cellText = cellValue?.ToString() ?? "";

                            // Handle currency formatting for cost columns
                            if (column.Name == "unitcost" || column.Name == "totalcost")
                            {
                                if (decimal.TryParse(cellText.Replace("₱", "").Replace(",", ""), out decimal value))
                                {
                                    cellText = value.ToString("F2");
                                    if (column.Name == "totalcost")
                                    {
                                        totalValue += value;
                                    }
                                }
                            }

                            dataRow.Append(EscapeCSVField(cellText));
                        }
                    }
                    csvContent.AppendLine(dataRow.ToString());
                }

                // Add totals row
                csvContent.AppendLine(); // Empty line
                csvContent.AppendLine($",,,,TOTAL VALUE:,{totalValue:F2},,,,");

                // Write to file
                File.WriteAllText(fileName, csvContent.ToString(), Encoding.UTF8);

                MessageBox.Show($"Purchase Orders exported successfully!\nLocation: {fileName}\nTotal Records: {dataGridView1.Rows.Count - 1}\nTotal Value: ₱{totalValue:N2}",
                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Log the export action
                string statusFilter = cmbstatus?.SelectedItem?.ToString() ?? "All";
                string supplierFilter = string.IsNullOrEmpty(selectedSupplier) ? "All Suppliers" : selectedSupplier;
                purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                    "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                    "', 'EXPORT', 'PURCHASE ORDER MANAGEMENT', 'CSV Export: " + supplierFilter + " - " + statusFilter + "', '" + username.Replace("'", "''") + "')");
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

            // If field contains comma, newline, or quote, wrap in quotes and escape internal quotes
            if (field.Contains(",") || field.Contains("\n") || field.Contains("\r") || field.Contains("\""))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }
            return field;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmAddPurchaseOrder(username, selectedSupplier));
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a purchase order to update.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string products = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string quantity = dataGridView1.Rows[row].Cells["quantity"].Value.ToString();
                string unitcost = dataGridView1.Rows[row].Cells["unitcost"].Value.ToString();

                ShowOrActivateForm(() => new frmUpdatePurchaseOrder(products, quantity, unitcost, username));
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
                    MessageBox.Show("Please select a valid purchase order record first.",
                        "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string products = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string supplier = dataGridView1.Columns.Contains("supplier") && dataGridView1.Columns["supplier"].Visible
                    ? dataGridView1.Rows[row].Cells["supplier"].Value.ToString()
                    : selectedSupplier;
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this purchase order?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string deleteQuery = "DELETE FROM tblpurchase_order WHERE products = '" + products.Replace("'", "''") + "'";
                    if (!string.IsNullOrEmpty(supplier))
                    {
                        deleteQuery += " AND supplier = '" + supplier.Replace("'", "''") + "'";
                    }
                    purchaseOrders.executeSQL(deleteQuery);
                    if (purchaseOrders.rowAffected > 0)
                    {
                        MessageBox.Show("Purchase order deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Log deletion
                        purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE', 'PURCHASE ORDER MANAGEMENT', '" + products + "', '" + username + "')");
                    }
                    LoadPurchaseOrders();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndelete_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndeleteall_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ALL purchase orders? This action cannot be undone.",
                    "Delete All Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    string deleteQuery = "DELETE FROM tblpurchase_order";
                    if (!string.IsNullOrEmpty(selectedSupplier))
                    {
                        deleteQuery += " WHERE supplier = '" + selectedSupplier.Replace("'", "''") + "'";
                    }
                    purchaseOrders.executeSQL(deleteQuery);
                    if (purchaseOrders.rowAffected > 0)
                    {
                        MessageBox.Show($"{purchaseOrders.rowAffected} purchase order(s) deleted.", "Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Log deletion
                        purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE ALL', 'PURCHASE ORDER MANAGEMENT', 'ALL RECORDS', '" + username + "')");
                    }
                    else
                    {
                        MessageBox.Show("No records to delete.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    LoadPurchaseOrders();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndeleteall_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            txtsearch.Text = "";
            if (cmbstatus != null) cmbstatus.SelectedIndex = 0; // Reset to "All"
            LoadPurchaseOrders();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Protect against header or out-of-range clicks
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                {
                    row = e.RowIndex;
                    // Make sure the clicked row is selected visually and set current cell
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[row].Selected = true;
                    if (dataGridView1.Columns.Count > 0 && dataGridView1.Rows[row].Cells.Count > 0)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[0];
                    }
                }
                else
                {
                    row = -1;
                }
                UpdateButtonStates();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on dataGridView1_CellClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Keep row in sync if selection changes (keyboard, mouse, etc.)
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell != null)
                {
                    int idx = dataGridView1.CurrentCell.RowIndex;
                    if (idx >= 0 && idx < dataGridView1.Rows.Count)
                    {
                        row = idx;
                    }
                    else
                    {
                        row = -1;
                    }
                }
                else
                {
                    row = -1;
                }
                UpdateButtonStates();
            }
            catch
            {
                // swallow to avoid selection change exceptions
                row = -1;
                UpdateButtonStates();
            }
        }

        private void txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) // Enter key
            {
                currentPage = 1;
                LoadPurchaseOrders(txtsearch.Text.Trim());
            }
        }

        // Pagination methods
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPurchaseOrders(txtsearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPurchaseOrders(txtsearch.Text.Trim());
            }
        }

        private void btnrecieveall_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Mark ALL purchase orders as received?", "Receive All Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string pendingQuery = "SELECT products, quantity, unitcost, supplier FROM tblpurchase_order WHERE status != 'Received'";
                    if (!string.IsNullOrEmpty(selectedSupplier))
                        pendingQuery += " AND supplier = '" + selectedSupplier.Replace("'", "''") + "'";
                    DataTable dtPending = purchaseOrders.GetData(pendingQuery);
                    string updateQuery = "UPDATE tblpurchase_order SET status = 'Received', datereceived = '" + DateTime.Now.ToString("MM/dd/yyyy") + "' WHERE status != 'Received'";
                    if (!string.IsNullOrEmpty(selectedSupplier))
                        updateQuery += " AND supplier = '" + selectedSupplier.Replace("'", "''") + "'";
                    purchaseOrders.executeSQL(updateQuery);
                    if (purchaseOrders.rowAffected > 0)
                    {
                        foreach (DataRow drRow in dtPending.Rows)
                        {
                            string products = drRow["products"].ToString();
                            string quantity = drRow["quantity"].ToString();
                            string unitcost = drRow["unitcost"].ToString();
                            int qtyToAdd = 0;
                            int.TryParse(quantity, out qtyToAdd);
                            string selectProductQuery = "SELECT currentstock FROM tblproducts WHERE LOWER(products) = LOWER('" + products.Replace("'", "''") + "')";
                            DataTable dtProd = purchaseOrders.GetData(selectProductQuery);
                            if (dtProd.Rows.Count > 0)
                            {
                                int currentStock = 0;
                                int.TryParse(dtProd.Rows[0]["currentstock"].ToString(), out currentStock);
                                int newStock = currentStock + qtyToAdd;
                                string updateProd = "UPDATE tblproducts SET currentstock = '" + newStock + "' WHERE LOWER(products) = LOWER('" + products.Replace("'", "''") + "')";
                                purchaseOrders.executeSQL(updateProd);
                            }
                            else
                            {
                                string insertProd = "INSERT INTO tblproducts (products, description, unitprice, currentstock, createdby, datecreated) " +
                                    "VALUES ('" + products.Replace("'", "''") + "', '', '" + unitcost.Replace("'", "''") + "', '" + quantity.Replace("'", "''") + "', '" + username.Replace("'", "''") + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";
                                purchaseOrders.executeSQL(insertProd);
                            }
                        }
                        MessageBox.Show($"{purchaseOrders.rowAffected} purchase order(s) marked as received.", "Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'RECEIVE ALL', 'PURCHASE ORDER MANAGEMENT', 'ALL PENDING ORDERS', '" + username + "')");
                    }
                    LoadPurchaseOrders();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnrecieveall_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnreceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a purchase order to receive.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string products = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string currentStatus = dataGridView1.Rows[row].Cells["status"].Value.ToString();
                if (currentStatus.ToUpper() == "RECEIVED")
                {
                    MessageBox.Show("This purchase order has already been received.", "Already Received",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DialogResult dr = MessageBox.Show($"Mark purchase order for '{products}' as received?", "Receive Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string updateQuery = "UPDATE tblpurchase_order SET status = 'Received', datereceived = '" + DateTime.Now.ToString("MM/dd/yyyy") + "' WHERE products = '" +
                        products.Replace("'", "''") + "'";
                    if (!string.IsNullOrEmpty(selectedSupplier))
                        updateQuery += " AND supplier = '" + selectedSupplier.Replace("'", "''") + "'";
                    purchaseOrders.executeSQL(updateQuery);
                    if (purchaseOrders.rowAffected > 0)
                    {
                        string quantity = dataGridView1.Rows[row].Cells["quantity"].Value.ToString();
                        string unitcost = dataGridView1.Rows[row].Cells["unitcost"].Value.ToString();
                        int qtyToAdd = 0;
                        int.TryParse(quantity, out qtyToAdd);
                        string selectProductQuery = "SELECT currentstock FROM tblproducts WHERE LOWER(products) = LOWER('" + products.Replace("'", "''") + "')";
                        DataTable dtProd = purchaseOrders.GetData(selectProductQuery);
                        if (dtProd.Rows.Count > 0)
                        {
                            int currentStock = 0;
                            int.TryParse(dtProd.Rows[0]["currentstock"].ToString(), out currentStock);
                            int newStock = currentStock + qtyToAdd;
                            string updateProd = "UPDATE tblproducts SET currentstock = '" + newStock + "' WHERE LOWER(products) = LOWER('" + products.Replace("'", "''") + "')";
                            purchaseOrders.executeSQL(updateProd);
                        }
                        else
                        {
                            string insertProd = "INSERT INTO tblproducts (products, description, unitprice, currentstock, createdby, datecreated) " +
                                "VALUES ('" + products.Replace("'", "''") + "', '', '" + unitcost.Replace("'", "''") + "', '" + quantity.Replace("'", "''") + "', '" + username.Replace("'", "''") + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";
                            purchaseOrders.executeSQL(insertProd);
                        }
                        MessageBox.Show("Purchase order marked as received.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'RECEIVE', 'PURCHASE ORDER MANAGEMENT', '" + products + "', '" + username + "')");
                    }
                    LoadPurchaseOrders();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnreceive_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadPurchaseOrders(txtsearch.Text.Trim());
        }
    }
}