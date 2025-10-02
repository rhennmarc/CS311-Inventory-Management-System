using inventory_management;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace Inventory_Management_System
{
    public partial class frmViewRefunds : Form
    {
        private string username;
        private Class1 db;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;

        // Print-related variables for refund receipt
        private PrintDocument refundPrintDocument;
        private string printOrderID;
        private decimal printFinalTotal;
        private decimal printPayment;
        private decimal printChange;
        private DataTable printOrderData;
        private bool printDiscounted;
        private string printDateCreated;
        private string printTimeCreated;
        private string printCreatedBy;
        private DataTable printRefundData;
        private decimal printTotalRefund;

        public frmViewRefunds(string username)
        {
            InitializeComponent();
            this.username = username;
            db = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

            // Initialize print document for refund receipts
            InitializeRefundPrintDocument();

            InitializeForm();
        }

        private void InitializeRefundPrintDocument()
        {
            refundPrintDocument = new PrintDocument();
            refundPrintDocument.PrintPage += RefundPrintDocument_PrintPage;
            refundPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 280, 600);
            refundPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);
        }

        private void RefundPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Font titleFont = new Font("Arial", 12, FontStyle.Bold);
                Font headerFont = new Font("Arial", 9, FontStyle.Bold);
                Font regularFont = new Font("Arial", 8);
                Font smallFont = new Font("Arial", 7);

                Brush brush = Brushes.Black;

                float yPos = 20;
                float leftMargin = 10;
                float centerPos = e.PageBounds.Width / 2;

                // Header - EXACT same as sales report
                string headerText = "AMGC PHARMACY";
                SizeF headerSize = g.MeasureString(headerText, titleFont);
                g.DrawString(headerText, titleFont, brush, centerPos - (headerSize.Width / 2), yPos);
                yPos += 20;

                string taglineText = "Your Health, Our Priority";
                SizeF taglineSize = g.MeasureString(taglineText, regularFont);
                g.DrawString(taglineText, regularFont, brush, centerPos - (taglineSize.Width / 2), yPos);
                yPos += 15;

                // Address below tagline - EXACT same as sales report
                string addressText = "1340 G Tuazon St Sampaloc Manila";
                SizeF addressSize = g.MeasureString(addressText, smallFont);
                g.DrawString(addressText, smallFont, brush, centerPos - (addressSize.Width / 2), yPos);
                yPos += 20;

                // Line separator - EXACT same as sales report
                g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                yPos += 15;

                // Order details - EXACT same as sales report
                g.DrawString($"Order ID: {printOrderID}", regularFont, brush, leftMargin, yPos);
                yPos += 15;
                g.DrawString($"Date: {printDateCreated} {printTimeCreated}", regularFont, brush, leftMargin, yPos);
                yPos += 15;
                g.DrawString($"Cashier: {printCreatedBy}", regularFont, brush, leftMargin, yPos);
                yPos += 15;

                // TIN below cashier - EXACT same as sales report
                string tinText = "Tin:";
                g.DrawString(tinText, smallFont, brush, leftMargin, yPos);
                yPos += 20;

                // Line separator - EXACT same as sales report
                g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                yPos += 10;

                // Column headers - EXACT same as sales report
                g.DrawString("Product", headerFont, brush, leftMargin, yPos);
                g.DrawString("Qty", headerFont, brush, 140, yPos);
                g.DrawString("Amount", headerFont, brush, 180, yPos);
                yPos += 15;

                // UPDATED PRODUCTS - Show remaining products after refund
                // Only show if there are remaining products
                if (printOrderData.Rows.Count > 0)
                {
                    foreach (DataRow row in printOrderData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                        decimal itemTotal = quantity * unitPrice;

                        // Apply discount if applicable
                        decimal displayUnitPrice = printDiscounted ? unitPrice * 0.8m : unitPrice;
                        decimal displayItemTotal = printDiscounted ? itemTotal * 0.8m : itemTotal;

                        // Truncate product name if too long - EXACT same as sales report
                        if (productName.Length > 18)
                            productName = productName.Substring(0, 15) + "...";

                        g.DrawString(productName, regularFont, brush, leftMargin, yPos);
                        g.DrawString(quantity.ToString(), regularFont, brush, 145, yPos);
                        g.DrawString(displayItemTotal.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                        yPos += 12;

                        // Unit price for multiple quantities - EXACT same as sales report
                        if (quantity > 1)
                        {
                            g.DrawString($"  @ {displayUnitPrice:₱#,##0.00} each", smallFont, Brushes.Gray, leftMargin + 5, yPos);
                            yPos += 10;
                        }
                    }

                    yPos += 10;
                }
                else
                {
                    // If no remaining products, add some spacing
                    yPos += 20;
                }

                // REFUNDED ITEMS SECTION - EXACT same as sales report but with total refund
                if (printRefundData.Rows.Count > 0)
                {
                    // Separator line before refunds - EXACT same as sales report
                    g.DrawLine(Pens.Red, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                    yPos += 10;

                    // Refund header - EXACT same as sales report
                    string refundHeaderText = "REFUNDED ITEMS";
                    SizeF refundHeaderSize = g.MeasureString(refundHeaderText, headerFont);
                    g.DrawString(refundHeaderText, headerFont, Brushes.Red, centerPos - (refundHeaderSize.Width / 2), yPos);
                    yPos += 15;

                    // Refunded items - EXACT layout as sales report
                    foreach (DataRow row in printRefundData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                        decimal itemTotal = quantity * unitPrice;
                        string refundDate = row["daterefunded"].ToString();
                        string refundReason = row["reason"].ToString();

                        // Truncate product name if too long - EXACT same as sales report
                        if (productName.Length > 18)
                            productName = productName.Substring(0, 15) + "...";

                        g.DrawString(productName, regularFont, brush, leftMargin, yPos);
                        g.DrawString($"-{quantity}", regularFont, Brushes.Red, 145, yPos);
                        g.DrawString($"-{itemTotal:₱#,##0.00}", regularFont, Brushes.Red, 180, yPos);
                        yPos += 12;

                        // Unit price for multiple quantities - EXACT same as sales report
                        if (quantity > 1)
                        {
                            g.DrawString($"  @ {unitPrice:₱#,##0.00} each", smallFont, Brushes.Gray, leftMargin + 5, yPos);
                            yPos += 10;
                        }

                        // Refund details - Refunded on and reason - EXACT same as sales report
                        g.DrawString($"Refunded on {refundDate} - Reason: {refundReason}", smallFont, Brushes.DarkRed, leftMargin, yPos);
                        yPos += 10;
                    }

                    yPos += 5;

                    // TOTAL REFUND section - Added to match sales report consistency
                    g.DrawLine(Pens.Red, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                    yPos += 10;

                    g.DrawString("TOTAL REFUND:", headerFont, Brushes.Red, leftMargin, yPos);
                    g.DrawString(printTotalRefund.ToString("₱#,##0.00"), headerFont, Brushes.Red, 180, yPos);
                    yPos += 20;
                }

                // Line separator - EXACT same as sales report
                g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                yPos += 15;

                // Totals section - EXACT same as sales report
                // Only show discount if there are remaining products and the order was discounted
                if (printDiscounted && printOrderData.Rows.Count > 0)
                {
                    decimal originalTotal = 0;
                    foreach (DataRow row in printOrderData.Rows)
                    {
                        originalTotal += Convert.ToDecimal(row["unitprice"]) * Convert.ToInt32(row["quantity"]);
                    }

                    g.DrawString("Subtotal:", regularFont, brush, leftMargin, yPos);
                    g.DrawString(originalTotal.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                    yPos += 12;

                    g.DrawString("Senior/PWD Discount (20%):", regularFont, brush, leftMargin, yPos);
                    g.DrawString((originalTotal * 0.2m).ToString("-₱#,##0.00"), regularFont, Brushes.Red, 180, yPos);
                    yPos += 15;

                    // Line before total
                    g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                    yPos += 10;
                }

                // Final total - EXACT same as sales report
                g.DrawString("TOTAL AMOUNT:", headerFont, brush, leftMargin, yPos);
                g.DrawString(printFinalTotal.ToString("₱#,##0.00"), headerFont, brush, 180, yPos);
                yPos += 20;

                // Payment details - EXACT same as sales report
                g.DrawString("Cash Payment:", regularFont, brush, leftMargin, yPos);
                g.DrawString(printPayment.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                yPos += 12;

                g.DrawString("Change:", regularFont, brush, leftMargin, yPos);
                g.DrawString(printChange.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                yPos += 25;

                // Footer - EXACT same as sales report
                g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                yPos += 15;

                string thankYou = "THANK YOU!";
                SizeF thankYouSize = g.MeasureString(thankYou, headerFont);
                g.DrawString(thankYou, headerFont, brush, centerPos - (thankYouSize.Width / 2), yPos);
                yPos += 20;

                string greatDay = "Have a great day ahead!";
                SizeF greatDaySize = g.MeasureString(greatDay, regularFont);
                g.DrawString(greatDay, regularFont, brush, centerPos - (greatDaySize.Width / 2), yPos);
                yPos += 20;

                // Contact info - EXACT same as sales report
                string contact = "For concerns, please contact us at:";
                SizeF contactSize = g.MeasureString(contact, smallFont);
                g.DrawString(contact, smallFont, brush, centerPos - (contactSize.Width / 2), yPos);
                yPos += 10;

                string phone = "Phone: (02) 123-4567";
                SizeF phoneSize = g.MeasureString(phone, smallFont);
                g.DrawString(phone, smallFont, brush, centerPos - (phoneSize.Width / 2), yPos);
                yPos += 8;

                string email = "Email: info@amgcpharmacy.com";
                SizeF emailSize = g.MeasureString(email, smallFont);
                g.DrawString(email, smallFont, brush, centerPos - (emailSize.Width / 2), yPos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during refund receipt printing: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeForm()
        {
            // Initialize duration combobox
            cmbDuration.Items.AddRange(new string[] { "Daily", "Weekly", "Monthly", "Yearly", "All Time" });
            cmbDuration.SelectedIndex = 0;

            // DatePicker setup
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "MM/dd/yyyy";
            dtpDate.Value = DateTime.Today;

            LoadRefunds();
        }

        private void LoadRefunds(string search = "")
        {
            try
            {
                // Get all refund data first
                string query = "SELECT orderid, products, quantity, unitprice, reason, daterefunded, timerefunded, refundedby FROM tblrefunds";
                DataTable allRefundsData = db.GetData(query);

                if (allRefundsData.Rows.Count == 0)
                {
                    // Show empty state
                    DataTable emptyTable = new DataTable();
                    emptyTable.Columns.Add("orderid", typeof(string));
                    emptyTable.Columns.Add("totalamount", typeof(decimal));
                    emptyTable.Columns.Add("daterefunded", typeof(string));
                    emptyTable.Columns.Add("timerefunded", typeof(string));
                    emptyTable.Columns.Add("itemcount", typeof(int));

                    ApplyPagination(emptyTable);
                    CalculateTotalRefunds(emptyTable);
                    return;
                }

                // Apply duration filter in C#
                string duration = cmbDuration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dtpDate.Value;
                DataTable filteredData = ApplyDurationFilter(allRefundsData, duration, selectedDate);

                // Apply search filter in C#
                if (!string.IsNullOrEmpty(search))
                {
                    filteredData = ApplySearchFilter(filteredData, search);
                }

                // Group by orderid in C#
                DataTable groupedData = GroupRefundsByOrderID(filteredData);

                // Sort by date and time (newest first)
                DataTable sortedData = SortRefundsByDateTime(groupedData);

                ApplyPagination(sortedData);
                CalculateTotalRefunds(filteredData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading refunds: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable ApplyDurationFilter(DataTable data, string duration, DateTime selectedDate)
        {
            if (duration == "All Time")
                return data;

            DataTable filteredData = data.Clone();

            foreach (DataRow row in data.Rows)
            {
                try
                {
                    string dateString = row["daterefunded"]?.ToString();
                    if (string.IsNullOrEmpty(dateString))
                        continue;

                    // Parse the date in MM/dd/yyyy format
                    string[] dateParts = dateString.Split('/');
                    if (dateParts.Length == 3)
                    {
                        if (int.TryParse(dateParts[0], out int month) &&
                            int.TryParse(dateParts[1], out int day) &&
                            int.TryParse(dateParts[2], out int year))
                        {
                            DateTime refundDate = new DateTime(year, month, day);
                            bool include = false;

                            switch (duration)
                            {
                                case "Daily":
                                    include = refundDate.Date == selectedDate.Date;
                                    break;

                                case "Weekly":
                                    DateTime startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek);
                                    DateTime endOfWeek = startOfWeek.AddDays(6);
                                    include = refundDate.Date >= startOfWeek.Date && refundDate.Date <= endOfWeek.Date;
                                    break;

                                case "Monthly":
                                    include = refundDate.Year == selectedDate.Year && refundDate.Month == selectedDate.Month;
                                    break;

                                case "Yearly":
                                    include = refundDate.Year == selectedDate.Year;
                                    break;
                            }

                            if (include)
                            {
                                filteredData.ImportRow(row);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Skip rows with invalid dates
                    Console.WriteLine($"Error parsing date: {ex.Message}");
                    continue;
                }
            }

            return filteredData;
        }

        private DataTable ApplySearchFilter(DataTable data, string search)
        {
            if (string.IsNullOrEmpty(search)) return data;

            DataTable filteredData = data.Clone();
            string searchLower = search.ToLower();

            foreach (DataRow row in data.Rows)
            {
                string orderid = row["orderid"]?.ToString() ?? "";
                string products = row["products"]?.ToString() ?? "";
                string reason = row["reason"]?.ToString() ?? "";

                if (orderid.ToLower().Contains(searchLower) ||
                    products.ToLower().Contains(searchLower) ||
                    reason.ToLower().Contains(searchLower))
                {
                    filteredData.ImportRow(row);
                }
            }

            return filteredData;
        }

        private DataTable GroupRefundsByOrderID(DataTable data)
        {
            DataTable groupedTable = new DataTable();
            groupedTable.Columns.Add("orderid", typeof(string));
            groupedTable.Columns.Add("totalamount", typeof(decimal));
            groupedTable.Columns.Add("daterefunded", typeof(string));
            groupedTable.Columns.Add("timerefunded", typeof(string));
            groupedTable.Columns.Add("itemcount", typeof(int));

            // Group by orderid
            var orderGroups = data.AsEnumerable()
                .GroupBy(row => row.Field<string>("orderid"))
                .ToList();

            foreach (var group in orderGroups)
            {
                decimal totalAmount = 0;
                DateTime latestDateTime = DateTime.MinValue;
                string latestDate = "";
                string latestTime = "";

                foreach (DataRow row in group)
                {
                    // Calculate total amount
                    string quantityStr = row["quantity"]?.ToString() ?? "0";
                    string unitPriceStr = row["unitprice"]?.ToString() ?? "0";

                    if (int.TryParse(quantityStr, out int quantity) &&
                        decimal.TryParse(unitPriceStr, out decimal unitPrice))
                    {
                        totalAmount += quantity * unitPrice;
                    }

                    // Determine latest date and time
                    string currentDate = row["daterefunded"]?.ToString() ?? "";
                    string currentTime = row["timerefunded"]?.ToString() ?? "";

                    try
                    {
                        // Parse date in MM/dd/yyyy format
                        string[] dateParts = currentDate.Split('/');
                        if (dateParts.Length == 3 &&
                            int.TryParse(dateParts[0], out int month) &&
                            int.TryParse(dateParts[1], out int day) &&
                            int.TryParse(dateParts[2], out int year))
                        {
                            // Parse time in HH:mm:ss format
                            string[] timeParts = currentTime.Split(':');
                            if (timeParts.Length >= 2 &&
                                int.TryParse(timeParts[0], out int hours) &&
                                int.TryParse(timeParts[1], out int minutes))
                            {
                                DateTime currentDateTime = new DateTime(year, month, day, hours, minutes, 0);
                                if (currentDateTime > latestDateTime)
                                {
                                    latestDateTime = currentDateTime;
                                    latestDate = currentDate;
                                    latestTime = currentTime;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // If parsing fails, use string comparison as fallback
                        if (string.Compare(currentDate + currentTime, latestDate + latestTime) > 0)
                        {
                            latestDate = currentDate;
                            latestTime = currentTime;
                        }
                    }
                }

                // If no valid date/time found, use the first one
                if (string.IsNullOrEmpty(latestDate) && group.Any())
                {
                    var firstRow = group.First();
                    latestDate = firstRow["daterefunded"]?.ToString() ?? "";
                    latestTime = firstRow["timerefunded"]?.ToString() ?? "";
                }

                groupedTable.Rows.Add(
                    group.Key,
                    totalAmount,
                    latestDate,
                    latestTime,
                    group.Count()
                );
            }

            return groupedTable;
        }

        private DataTable SortRefundsByDateTime(DataTable data)
        {
            DataTable sortedTable = data.Clone();

            var sortedRows = data.AsEnumerable()
                .OrderByDescending(row =>
                {
                    string dateStr = row.Field<string>("daterefunded") ?? "";
                    string timeStr = row.Field<string>("timerefunded") ?? "";

                    try
                    {
                        // Parse date in MM/dd/yyyy format
                        string[] dateParts = dateStr.Split('/');
                        if (dateParts.Length == 3 &&
                            int.TryParse(dateParts[0], out int month) &&
                            int.TryParse(dateParts[1], out int day) &&
                            int.TryParse(dateParts[2], out int year))
                        {
                            // Parse time in HH:mm:ss format
                            string[] timeParts = timeStr.Split(':');
                            int hours = 0, minutes = 0, seconds = 0;

                            if (timeParts.Length >= 1) int.TryParse(timeParts[0], out hours);
                            if (timeParts.Length >= 2) int.TryParse(timeParts[1], out minutes);
                            if (timeParts.Length >= 3) int.TryParse(timeParts[2], out seconds);

                            return new DateTime(year, month, day, hours, minutes, seconds);
                        }
                    }
                    catch
                    {
                        // If parsing fails, use string comparison
                    }

                    return DateTime.MinValue;
                })
                .ThenByDescending(row => row.Field<string>("timerefunded") ?? "")
                .ThenByDescending(row => row.Field<string>("orderid") ?? "");

            foreach (var row in sortedRows)
            {
                sortedTable.ImportRow(row);
            }

            return sortedTable;
        }

        private void ApplyPagination(DataTable data)
        {
            totalRecords = data.Rows.Count;
            totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;

            DataTable dtPage = data.Clone();
            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, totalRecords);

            for (int i = startIndex; i < endIndex; i++)
            {
                dtPage.ImportRow(data.Rows[i]);
            }

            dgvRefunds.DataSource = dtPage;
            StyleDataGridView();

            lblPageInfo.Text = totalRecords == 0 ? "No refunds found" : $"Page {currentPage} of {totalPages} ({totalRecords} orders)";
        }

        private void StyleDataGridView()
        {
            dgvRefunds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvRefunds.RowTemplate.Height = 28;
            dgvRefunds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRefunds.ReadOnly = true;

            if (dgvRefunds.Columns.Count > 0)
            {
                dgvRefunds.Columns["orderid"].HeaderText = "Order ID";
                dgvRefunds.Columns["orderid"].Width = 150;

                dgvRefunds.Columns["totalamount"].HeaderText = "Total Amount";
                dgvRefunds.Columns["totalamount"].Width = 120;
                dgvRefunds.Columns["totalamount"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvRefunds.Columns["totalamount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvRefunds.Columns["daterefunded"].HeaderText = "Date";
                dgvRefunds.Columns["daterefunded"].Width = 100;

                dgvRefunds.Columns["timerefunded"].HeaderText = "Time";
                dgvRefunds.Columns["timerefunded"].Width = 80;
                dgvRefunds.Columns["timerefunded"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvRefunds.Columns["itemcount"].HeaderText = "Items";
                dgvRefunds.Columns["itemcount"].Width = 60;
                dgvRefunds.Columns["itemcount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        private void CalculateTotalRefunds(DataTable refundData)
        {
            decimal totalRefundAmount = 0;
            foreach (DataRow row in refundData.Rows)
            {
                string quantityStr = row["quantity"]?.ToString() ?? "0";
                string unitPriceStr = row["unitprice"]?.ToString() ?? "0";

                if (int.TryParse(quantityStr, out int quantity) &&
                    decimal.TryParse(unitPriceStr, out decimal unitPrice))
                {
                    totalRefundAmount += quantity * unitPrice;
                }
            }

            txtTotalRefunds.Text = totalRefundAmount.ToString("₱#,##0.00");
            lblTotalRecords.Text = $"{refundData.Rows.Count} refund record(s)";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadRefunds(txtSearch.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            txtSearch.Text = "";
            dtpDate.Value = DateTime.Today;
            cmbDuration.SelectedIndex = 0;
            LoadRefunds();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRefunds.CurrentRow == null)
                {
                    MessageBox.Show("Please select a refund order to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dgvRefunds.CurrentRow;
                string orderID = selectedRow.Cells["orderid"].Value?.ToString();
                int itemCount = Convert.ToInt32(selectedRow.Cells["itemcount"].Value);

                DialogResult result = MessageBox.Show($"Delete ALL refund records for Order: {orderID}?\nThis will delete {itemCount} item(s).",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string deleteQuery = $"DELETE FROM tblrefunds WHERE orderid = '{orderID.Replace("'", "''")}'";
                    db.executeSQL(deleteQuery);

                    if (db.rowAffected > 0)
                    {
                        MessageBox.Show("Refund records deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log the action
                        db.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                            "', 'DELETE', 'REFUNDS', 'Order: " + orderID.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");

                        LoadRefunds(txtSearch.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting refund records: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                string duration = cmbDuration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dtpDate.Value;
                string filterText = $"{duration} refunds for {selectedDate:MM/dd/yyyy}";

                DialogResult result = MessageBox.Show($"Delete ALL {filterText}? This action cannot be undone.", "Confirm Delete All", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Get all refund data first
                    string query = "SELECT * FROM tblrefunds";
                    DataTable allData = db.GetData(query);
                    DataTable filteredData = ApplyDurationFilter(allData, duration, selectedDate);

                    // Delete each record
                    int deletedCount = 0;
                    foreach (DataRow row in filteredData.Rows)
                    {
                        string orderID = row["orderid"].ToString();
                        string product = row["products"].ToString();
                        string dateRefunded = row["daterefunded"].ToString();

                        string deleteQuery = $"DELETE FROM tblrefunds WHERE orderid = '{orderID.Replace("'", "''")}' " +
                                            $"AND products = '{product.Replace("'", "''")}' " +
                                            $"AND daterefunded = '{dateRefunded.Replace("'", "''")}'";
                        db.executeSQL(deleteQuery);
                        deletedCount++;
                    }

                    MessageBox.Show($"{deletedCount} refund record(s) deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Log the action
                    db.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                        "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                        "', 'DELETE ALL', 'REFUNDS', '" + filterText.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");

                    LoadRefunds();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting all refunds: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string duration = cmbDuration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dtpDate.Value;
                string searchTerm = txtSearch.Text.Trim();

                // Get all refund data
                string query = "SELECT orderid, products, quantity, unitprice, reason, daterefunded, timerefunded, refundedby FROM tblrefunds";
                DataTable allData = db.GetData(query);

                // Apply filters
                DataTable filteredData = ApplyDurationFilter(allData, duration, selectedDate);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    filteredData = ApplySearchFilter(filteredData, searchTerm);
                }

                if (filteredData.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files|*.csv";
                saveDialog.Title = "Export Refunds Data";
                string durationText = duration.ToLower();
                string dateText = selectedDate.ToString("yyyy-MM-dd");
                saveDialog.FileName = $"Refunds_{durationText}_{dateText}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCSV(filteredData, saveDialog.FileName, duration, selectedDate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(DataTable data, string fileName, string duration, DateTime selectedDate)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Add header information
                csvContent.AppendLine("AMGC PHARMACY - REFUNDS REPORT");
                csvContent.AppendLine($"Duration: {duration}, Date: {selectedDate:MM/dd/yyyy}, Generated: {DateTime.Now:MM/dd/yyyy HH:mm:ss}");
                csvContent.AppendLine();

                // Add column headers
                csvContent.AppendLine("Order ID,Product,Quantity,Unit Price,Refund Amount,Reason,Date Refunded,Time Refunded,Refunded By");

                decimal totalRefundAmount = 0;

                foreach (DataRow row in data.Rows)
                {
                    string orderid = EscapeCSVField(row["orderid"]?.ToString() ?? "");
                    string products = EscapeCSVField(row["products"]?.ToString() ?? "");
                    string quantity = row["quantity"]?.ToString() ?? "0";
                    string unitPrice = row["unitprice"]?.ToString() ?? "0";
                    string reason = EscapeCSVField(row["reason"]?.ToString() ?? "");
                    string dateRefunded = EscapeCSVField(row["daterefunded"]?.ToString() ?? "");
                    string timeRefunded = EscapeCSVField(row["timerefunded"]?.ToString() ?? "");
                    string refundedBy = EscapeCSVField(row["refundedby"]?.ToString() ?? "");

                    // Calculate refund amount
                    if (int.TryParse(quantity, out int qty) && decimal.TryParse(unitPrice, out decimal price))
                    {
                        decimal refundAmount = qty * price;
                        totalRefundAmount += refundAmount;
                        csvContent.AppendLine($"{orderid},{products},{quantity},{price:F2},{refundAmount:F2},{reason},{dateRefunded},{timeRefunded},{refundedBy}");
                    }
                    else
                    {
                        csvContent.AppendLine($"{orderid},{products},{quantity},{unitPrice},0.00,{reason},{dateRefunded},{timeRefunded},{refundedBy}");
                    }
                }

                // Add totals
                csvContent.AppendLine();
                csvContent.AppendLine($",,,,TOTAL REFUNDS:,{totalRefundAmount:F2},,,");

                File.WriteAllText(fileName, csvContent.ToString(), Encoding.UTF8);

                MessageBox.Show($"Refunds report exported successfully!\nLocation: {fileName}\nTotal Records: {data.Rows.Count}\nTotal Refund Amount: ₱{totalRefundAmount:N2}",
                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Log the export action
                db.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                    "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                    "', 'EXPORT', 'REFUNDS', 'CSV Export: " + duration + " - " + selectedDate.ToString("MM/dd/yyyy") + "', '" + username.Replace("'", "''") + "')");
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadRefunds(txtSearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadRefunds(txtSearch.Text.Trim());
            }
        }

        private void cmbDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadRefunds(txtSearch.Text.Trim());
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadRefunds(txtSearch.Text.Trim());
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter key
            {
                currentPage = 1;
                LoadRefunds(txtSearch.Text.Trim());
            }
        }

        private void btnreceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRefunds.CurrentRow == null)
                {
                    MessageBox.Show("Please select a refund order to generate receipt.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dgvRefunds.CurrentRow;
                string orderID = selectedRow.Cells["orderid"].Value?.ToString();

                // Get refund details for this order
                string refundQuery = $"SELECT products, quantity, unitprice, reason, daterefunded, timerefunded, refundedby FROM tblrefunds WHERE orderid = '{orderID.Replace("'", "''")}'";
                DataTable refundDetails = db.GetData(refundQuery);

                if (refundDetails.Rows.Count == 0)
                {
                    MessageBox.Show("No refund details found for this order.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Get original order details - but don't show error if not found
                string orderQuery = $"SELECT * FROM tblsales WHERE orderid = '{orderID.Replace("'", "''")}' LIMIT 1";
                DataTable orderData = db.GetData(orderQuery);

                // Check if we have original order data or if it's all refunded
                bool hasOriginalOrder = orderData.Rows.Count > 0;

                if (hasOriginalOrder)
                {
                    // Process with original order details (your existing logic)
                    ProcessReceiptWithOriginalOrder(orderID, refundDetails, orderData);
                }
                else
                {
                    // Process as refund-only receipt
                    ProcessRefundOnlyReceipt(orderID, refundDetails);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating refund receipt: " + ex.Message, "Receipt Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessReceiptWithOriginalOrder(string orderID, DataTable refundDetails, DataTable orderData)
        {
            // Your existing logic for when original order data is available
            DataRow firstRow = orderData.Rows[0];
            string originalDate = firstRow["datecreated"].ToString();
            string originalTime = firstRow["timecreated"].ToString();
            string originalCashier = firstRow["createdby"].ToString();
            bool discounted = firstRow["discounted"].ToString().ToUpper() == "YES";
            decimal originalPayment = Convert.ToDecimal(firstRow["payment"]);
            decimal originalChange = Convert.ToDecimal(firstRow["paymentchange"]);

            // Get current sales items (remaining products after refund)
            string currentSalesQuery = $"SELECT products, quantity, totalcost FROM tblsales WHERE orderid = '{orderID.Replace("'", "''")}'";
            DataTable currentSalesData = db.GetData(currentSalesQuery);

            // Create DataTable for UPDATED products (remaining after refund)
            DataTable updatedProductsData = new DataTable();
            updatedProductsData.Columns.Add("products", typeof(string));
            updatedProductsData.Columns.Add("quantity", typeof(int));
            updatedProductsData.Columns.Add("unitprice", typeof(decimal));

            foreach (DataRow row in currentSalesData.Rows)
            {
                string product = row["products"].ToString();
                int quantity = Convert.ToInt32(row["quantity"]);
                decimal totalCost = Convert.ToDecimal(row["totalcost"]);
                decimal unitPrice = totalCost / quantity;

                DataRow updatedRow = updatedProductsData.NewRow();
                updatedRow["products"] = product;
                updatedRow["quantity"] = quantity;
                updatedRow["unitprice"] = unitPrice;
                updatedProductsData.Rows.Add(updatedRow);
            }

            // Create DataTable for REFUNDED products
            DataTable refundReceiptData = new DataTable();
            refundReceiptData.Columns.Add("products", typeof(string));
            refundReceiptData.Columns.Add("quantity", typeof(int));
            refundReceiptData.Columns.Add("unitprice", typeof(decimal));
            refundReceiptData.Columns.Add("daterefunded", typeof(string));
            refundReceiptData.Columns.Add("reason", typeof(string));

            decimal totalRefundAmount = 0;

            foreach (DataRow row in refundDetails.Rows)
            {
                string product = row["products"].ToString();
                int quantity = Convert.ToInt32(row["quantity"]);
                decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                string refundDate = row["daterefunded"].ToString();
                string refundReason = row["reason"].ToString();

                DataRow refundRow = refundReceiptData.NewRow();
                refundRow["products"] = product;
                refundRow["quantity"] = quantity;
                refundRow["unitprice"] = unitPrice;
                refundRow["daterefunded"] = refundDate;
                refundRow["reason"] = refundReason;
                refundReceiptData.Rows.Add(refundRow);

                totalRefundAmount += quantity * unitPrice;
            }

            // Calculate updated order total
            decimal updatedOrderTotal = 0;
            foreach (DataRow row in updatedProductsData.Rows)
            {
                updatedOrderTotal += Convert.ToDecimal(row["unitprice"]) * Convert.ToInt32(row["quantity"]);
            }

            // Apply discount if original order was discounted
            if (discounted)
            {
                updatedOrderTotal *= 0.8m;
            }

            // Store print data for receipt
            printOrderID = orderID;
            printFinalTotal = updatedOrderTotal; // Updated order total after refund
            printPayment = originalPayment; // Original cash payment
            printChange = originalChange; // Original change
            printOrderData = updatedProductsData; // Updated products (remaining after refund)
            printRefundData = refundReceiptData; // Refunded items with details
            printDiscounted = discounted; // Keep original discount status
            printDateCreated = originalDate; // Original order date
            printTimeCreated = originalTime; // Original order time
            printCreatedBy = originalCashier; // Original cashier
            printTotalRefund = totalRefundAmount; // Total refund amount

            // Show refund receipt
            ShowRefundReceipt();
        }

        private void ProcessRefundOnlyReceipt(string orderID, DataTable refundDetails)
        {
            // Get the first refund record to use for date/time/cashier info
            DataRow firstRefund = refundDetails.Rows[0];
            string refundDate = firstRefund["daterefunded"].ToString();
            string refundTime = firstRefund["timerefunded"].ToString();
            string refundedBy = firstRefund["refundedby"].ToString();

            // Create DataTable for REFUNDED products
            DataTable refundReceiptData = new DataTable();
            refundReceiptData.Columns.Add("products", typeof(string));
            refundReceiptData.Columns.Add("quantity", typeof(int));
            refundReceiptData.Columns.Add("unitprice", typeof(decimal));
            refundReceiptData.Columns.Add("daterefunded", typeof(string));
            refundReceiptData.Columns.Add("reason", typeof(string));

            decimal totalRefundAmount = 0;

            foreach (DataRow row in refundDetails.Rows)
            {
                string product = row["products"].ToString();
                int quantity = Convert.ToInt32(row["quantity"]);
                decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                string refundDateItem = row["daterefunded"].ToString();
                string refundReason = row["reason"].ToString();

                DataRow refundRow = refundReceiptData.NewRow();
                refundRow["products"] = product;
                refundRow["quantity"] = quantity;
                refundRow["unitprice"] = unitPrice;
                refundRow["daterefunded"] = refundDateItem;
                refundRow["reason"] = refundReason;
                refundReceiptData.Rows.Add(refundRow);

                totalRefundAmount += quantity * unitPrice;
            }

            // Create empty DataTable for updated products (since it's all refunded)
            DataTable updatedProductsData = new DataTable();
            updatedProductsData.Columns.Add("products", typeof(string));
            updatedProductsData.Columns.Add("quantity", typeof(int));
            updatedProductsData.Columns.Add("unitprice", typeof(decimal));

            // Store print data for receipt - REFUND ONLY
            printOrderID = orderID;
            printFinalTotal = 0; // No remaining balance
            printPayment = totalRefundAmount; // Show refund as payment
            printChange = 0; // No change
            printOrderData = updatedProductsData; // Empty - no remaining products
            printRefundData = refundReceiptData; // All refunded items
            printDiscounted = false; // Not applicable for refund-only
            printDateCreated = refundDate; // Use refund date
            printTimeCreated = refundTime; // Use refund time
            printCreatedBy = refundedBy; // Use refund processor
            printTotalRefund = totalRefundAmount;

            // Show refund receipt
            ShowRefundReceipt();
        }

        private void ShowRefundReceipt()
        {
            try
            {
                Form receiptForm = new Form();
                receiptForm.Text = "AMGC Pharmacy - Refund Receipt";
                receiptForm.Size = new Size(420, 650);
                receiptForm.StartPosition = FormStartPosition.CenterParent;
                receiptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                receiptForm.MaximizeBox = false;
                receiptForm.MinimizeBox = false;
                receiptForm.BackColor = Color.White;

                // Calculate the required height based on items
                int baseHeight = 400;
                int itemHeight = 20;
                int extraHeightForMultipleQty = 0;

                // Calculate extra height for updated products
                foreach (DataRow row in printOrderData.Rows)
                {
                    int quantity = Convert.ToInt32(row["quantity"]);
                    if (quantity > 1)
                        extraHeightForMultipleQty += 15;
                }

                // Calculate extra height for refund items (including refund details)
                int refundSectionHeight = 0;
                if (printRefundData.Rows.Count > 0)
                {
                    refundSectionHeight = 60; // Header + separator + total refund
                    foreach (DataRow row in printRefundData.Rows)
                    {
                        refundSectionHeight += 32; // Product line + refund details line
                        int quantity = Convert.ToInt32(row["quantity"]);
                        if (quantity > 1)
                            refundSectionHeight += 12; // Unit price line
                    }
                }

                int calculatedHeight = baseHeight +
                    (printOrderData.Rows.Count * itemHeight) +
                    refundSectionHeight +
                    extraHeightForMultipleQty;
                int panelHeight = Math.Max(550, calculatedHeight);

                // Create a scrollable panel for the receipt content
                Panel scrollablePanel = new Panel();
                scrollablePanel.Size = new Size(400, 500);
                scrollablePanel.Location = new Point(10, 10);
                scrollablePanel.BackColor = Color.White;
                scrollablePanel.BorderStyle = BorderStyle.FixedSingle;
                scrollablePanel.AutoScroll = true;

                // Create the actual receipt content panel
                Panel receiptPanel = new Panel();
                receiptPanel.Size = new Size(380, panelHeight);
                receiptPanel.Location = new Point(0, 0);
                receiptPanel.BackColor = Color.White;

                int yPos = 10;

                // Header - EXACT same as sales report
                Label headerLabel = CreateReceiptLabel("AMGC PHARMACY", new Font("Arial", 16, FontStyle.Bold), 380);
                headerLabel.Location = new Point(0, yPos);
                headerLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(headerLabel);
                yPos += 25;

                Label taglineLabel = CreateReceiptLabel("Your Health, Our Priority", new Font("Arial", 10, FontStyle.Italic), 380);
                taglineLabel.Location = new Point(0, yPos);
                taglineLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(taglineLabel);
                yPos += 20;

                // Address below tagline - EXACT same as sales report
                Label addressLabel = CreateReceiptLabel("1340 G Tuazon St Sampaloc Manila", new Font("Arial", 8), 380);
                addressLabel.Location = new Point(0, yPos);
                addressLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(addressLabel);
                yPos += 20;

                // Separator line - EXACT same as sales report
                Panel line1 = new Panel();
                line1.Size = new Size(360, 1);
                line1.Location = new Point(10, yPos);
                line1.BackColor = Color.Black;
                receiptPanel.Controls.Add(line1);
                yPos += 10;

                // Order info - EXACT same layout as sales report
                Label orderInfoLabel = CreateReceiptLabel($"Order ID: {printOrderID}", new Font("Arial", 9), 380);
                orderInfoLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(orderInfoLabel);
                yPos += 18;

                Label dateTimeLabel = CreateReceiptLabel($"Date: {printDateCreated} {printTimeCreated}", new Font("Arial", 9), 380);
                dateTimeLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(dateTimeLabel);
                yPos += 18;

                Label cashierLabel = CreateReceiptLabel($"Cashier: {printCreatedBy}", new Font("Arial", 9), 380);
                cashierLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(cashierLabel);
                yPos += 18;

                // TIN below cashier - EXACT same as sales report
                Label tinLabel = CreateReceiptLabel("Tin:", new Font("Arial", 8), 380);
                tinLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(tinLabel);
                yPos += 20;

                // Product header - EXACT same layout as sales report
                Panel line2 = new Panel();
                line2.Size = new Size(360, 1);
                line2.Location = new Point(10, yPos);
                line2.BackColor = Color.Black;
                receiptPanel.Controls.Add(line2);
                yPos += 10;

                // Product header with better alignment - EXACT same as sales report
                Label productHeaderLabel = new Label();
                productHeaderLabel.Text = "Product";
                productHeaderLabel.Font = new Font("Arial", 9, FontStyle.Bold);
                productHeaderLabel.Size = new Size(180, 18);
                productHeaderLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(productHeaderLabel);

                Label qtyHeaderLabel = new Label();
                qtyHeaderLabel.Text = "Qty";
                qtyHeaderLabel.Font = new Font("Arial", 9, FontStyle.Bold);
                qtyHeaderLabel.Size = new Size(50, 18);
                qtyHeaderLabel.Location = new Point(190, yPos);
                qtyHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(qtyHeaderLabel);

                Label amountHeaderLabel = new Label();
                amountHeaderLabel.Text = "Amount";
                amountHeaderLabel.Font = new Font("Arial", 9, FontStyle.Bold);
                amountHeaderLabel.Size = new Size(100, 18);
                amountHeaderLabel.Location = new Point(270, yPos);
                amountHeaderLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(amountHeaderLabel);
                yPos += 20;

                // UPDATED PRODUCTS - Show remaining products after refund
                // Only show if there are remaining products
                if (printOrderData.Rows.Count > 0)
                {
                    foreach (DataRow row in printOrderData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                        decimal itemTotal = quantity * unitPrice;

                        // Apply discount if applicable
                        decimal displayUnitPrice = printDiscounted ? unitPrice * 0.8m : unitPrice;
                        decimal displayItemTotal = printDiscounted ? itemTotal * 0.8m : itemTotal;

                        // Truncate product name if too long - EXACT same as sales report
                        if (productName.Length > 20)
                            productName = productName.Substring(0, 17) + "...";

                        // Product line - EXACT same layout as sales report
                        Label productLabel = new Label();
                        productLabel.Text = productName;
                        productLabel.Font = new Font("Arial", 9);
                        productLabel.Size = new Size(180, 18);
                        productLabel.Location = new Point(10, yPos);
                        receiptPanel.Controls.Add(productLabel);

                        Label qtyLabel = new Label();
                        qtyLabel.Text = quantity.ToString();
                        qtyLabel.Font = new Font("Arial", 9);
                        qtyLabel.Size = new Size(50, 18);
                        qtyLabel.Location = new Point(190, yPos);
                        qtyLabel.TextAlign = ContentAlignment.MiddleCenter;
                        receiptPanel.Controls.Add(qtyLabel);

                        Label amountLabel = new Label();
                        amountLabel.Text = displayItemTotal.ToString("₱#,##0.00");
                        amountLabel.Font = new Font("Arial", 9);
                        amountLabel.Size = new Size(100, 18);
                        amountLabel.Location = new Point(270, yPos);
                        amountLabel.TextAlign = ContentAlignment.MiddleRight;
                        receiptPanel.Controls.Add(amountLabel);

                        yPos += 18;

                        // Unit price for multiple quantities - EXACT same as sales report
                        if (quantity > 1)
                        {
                            Label unitPriceLabel = CreateReceiptLabel($"  @ {displayUnitPrice.ToString("₱#,##0.00")} each", new Font("Arial", 8, FontStyle.Italic), 380);
                            unitPriceLabel.Location = new Point(20, yPos);
                            unitPriceLabel.ForeColor = Color.Gray;
                            receiptPanel.Controls.Add(unitPriceLabel);
                            yPos += 15;
                        }
                    }

                    yPos += 10;
                }
                else
                {
                    // If no remaining products, add some spacing
                    yPos += 20;
                }

                // REFUNDED ITEMS SECTION - EXACT same as sales report but with total refund
                if (printRefundData.Rows.Count > 0)
                {
                    // Separator line before refunds - EXACT same as sales report
                    Panel refundSeparator = new Panel();
                    refundSeparator.Size = new Size(360, 2);
                    refundSeparator.Location = new Point(10, yPos);
                    refundSeparator.BackColor = Color.Red;
                    receiptPanel.Controls.Add(refundSeparator);
                    yPos += 10;

                    // Refund header - EXACT same as sales report
                    Label refundHeaderLabel = CreateReceiptLabel("REFUNDED ITEMS", new Font("Arial", 10, FontStyle.Bold), 380);
                    refundHeaderLabel.Location = new Point(0, yPos);
                    refundHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
                    refundHeaderLabel.ForeColor = Color.Red;
                    receiptPanel.Controls.Add(refundHeaderLabel);
                    yPos += 20;

                    // Refunded items - EXACT same layout as sales report
                    foreach (DataRow row in printRefundData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                        decimal itemTotal = quantity * unitPrice;
                        string refundDate = row["daterefunded"].ToString();
                        string refundReason = row["reason"].ToString();

                        // Truncate product name if too long - EXACT same as sales report
                        if (productName.Length > 20)
                            productName = productName.Substring(0, 17) + "...";

                        // Product line - EXACT same layout as sales report but with negative values
                        Label productLabel = new Label();
                        productLabel.Text = productName;
                        productLabel.Font = new Font("Arial", 9);
                        productLabel.Size = new Size(180, 18);
                        productLabel.Location = new Point(10, yPos);
                        receiptPanel.Controls.Add(productLabel);

                        Label qtyLabel = new Label();
                        qtyLabel.Text = $"-{quantity}";
                        qtyLabel.Font = new Font("Arial", 9);
                        qtyLabel.Size = new Size(50, 18);
                        qtyLabel.Location = new Point(190, yPos);
                        qtyLabel.TextAlign = ContentAlignment.MiddleCenter;
                        qtyLabel.ForeColor = Color.Red;
                        receiptPanel.Controls.Add(qtyLabel);

                        Label amountLabel = new Label();
                        amountLabel.Text = $"-{itemTotal.ToString("₱#,##0.00")}";
                        amountLabel.Font = new Font("Arial", 9);
                        amountLabel.Size = new Size(100, 18);
                        amountLabel.Location = new Point(270, yPos);
                        amountLabel.TextAlign = ContentAlignment.MiddleRight;
                        amountLabel.ForeColor = Color.Red;
                        receiptPanel.Controls.Add(amountLabel);

                        yPos += 18;

                        // Unit price for multiple quantities - EXACT same as sales report
                        if (quantity > 1)
                        {
                            Label unitPriceLabel = CreateReceiptLabel($"  @ {unitPrice.ToString("₱#,##0.00")} each", new Font("Arial", 8, FontStyle.Italic), 380);
                            unitPriceLabel.Location = new Point(20, yPos);
                            unitPriceLabel.ForeColor = Color.Gray;
                            receiptPanel.Controls.Add(unitPriceLabel);
                            yPos += 12;
                        }

                        // Refund details - Refunded on and reason - EXACT same as sales report
                        Label refundDetailsLabel = CreateReceiptLabel($"Refunded on {refundDate} - Reason: {refundReason}", new Font("Arial", 7, FontStyle.Italic), 380);
                        refundDetailsLabel.Location = new Point(15, yPos);
                        refundDetailsLabel.ForeColor = Color.DarkRed;
                        receiptPanel.Controls.Add(refundDetailsLabel);
                        yPos += 14;
                    }

                    yPos += 10;

                    // TOTAL REFUND section - Added to match sales report consistency
                    Panel totalRefundSeparator = new Panel();
                    totalRefundSeparator.Size = new Size(360, 1);
                    totalRefundSeparator.Location = new Point(10, yPos);
                    totalRefundSeparator.BackColor = Color.Red;
                    receiptPanel.Controls.Add(totalRefundSeparator);
                    yPos += 10;

                    Label totalRefundLabel = new Label();
                    totalRefundLabel.Text = "TOTAL REFUND:";
                    totalRefundLabel.Font = new Font("Arial", 9, FontStyle.Bold);
                    totalRefundLabel.Size = new Size(200, 18);
                    totalRefundLabel.Location = new Point(10, yPos);
                    totalRefundLabel.ForeColor = Color.Red;
                    receiptPanel.Controls.Add(totalRefundLabel);

                    Label totalRefundAmountLabel = new Label();
                    totalRefundAmountLabel.Text = printTotalRefund.ToString("₱#,##0.00");
                    totalRefundAmountLabel.Font = new Font("Arial", 9, FontStyle.Bold);
                    totalRefundAmountLabel.Size = new Size(150, 18);
                    totalRefundAmountLabel.Location = new Point(220, yPos);
                    totalRefundAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                    totalRefundAmountLabel.ForeColor = Color.Red;
                    receiptPanel.Controls.Add(totalRefundAmountLabel);
                    yPos += 25;
                }

                // Separator line - EXACT same as sales report
                Panel line3 = new Panel();
                line3.Size = new Size(360, 1);
                line3.Location = new Point(10, yPos);
                line3.BackColor = Color.Black;
                receiptPanel.Controls.Add(line3);
                yPos += 15;

                // Totals section - EXACT same as sales report
                // Only show discount if there are remaining products and the order was discounted
                if (printDiscounted && printOrderData.Rows.Count > 0)
                {
                    decimal originalTotal = 0;
                    foreach (DataRow row in printOrderData.Rows)
                    {
                        originalTotal += Convert.ToDecimal(row["unitprice"]) * Convert.ToInt32(row["quantity"]);
                    }

                    // Subtotal
                    Label subtotalLabel = new Label();
                    subtotalLabel.Text = "Subtotal:";
                    subtotalLabel.Font = new Font("Arial", 9);
                    subtotalLabel.Size = new Size(200, 18);
                    subtotalLabel.Location = new Point(10, yPos);
                    receiptPanel.Controls.Add(subtotalLabel);

                    Label subtotalAmountLabel = new Label();
                    subtotalAmountLabel.Text = originalTotal.ToString("₱#,##0.00");
                    subtotalAmountLabel.Font = new Font("Arial", 9);
                    subtotalAmountLabel.Size = new Size(150, 18);
                    subtotalAmountLabel.Location = new Point(220, yPos);
                    subtotalAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                    receiptPanel.Controls.Add(subtotalAmountLabel);
                    yPos += 18;

                    // Discount
                    Label discountLabel = new Label();
                    discountLabel.Text = "Senior/PWD Discount (20%):";
                    discountLabel.Font = new Font("Arial", 9);
                    discountLabel.Size = new Size(200, 18);
                    discountLabel.Location = new Point(10, yPos);
                    receiptPanel.Controls.Add(discountLabel);

                    Label discountAmountLabel = new Label();
                    discountAmountLabel.Text = (originalTotal * 0.2m).ToString("-₱#,##0.00");
                    discountAmountLabel.Font = new Font("Arial", 9);
                    discountAmountLabel.Size = new Size(150, 18);
                    discountAmountLabel.Location = new Point(220, yPos);
                    discountAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                    discountAmountLabel.ForeColor = Color.Red;
                    receiptPanel.Controls.Add(discountAmountLabel);
                    yPos += 18;

                    // Line before total
                    Panel line4 = new Panel();
                    line4.Size = new Size(360, 1);
                    line4.Location = new Point(10, yPos);
                    line4.BackColor = Color.Black;
                    receiptPanel.Controls.Add(line4);
                    yPos += 10;
                }

                // Total
                Label totalLabel = new Label();
                totalLabel.Text = "TOTAL AMOUNT:";
                totalLabel.Font = new Font("Arial", 11, FontStyle.Bold);
                totalLabel.Size = new Size(200, 20);
                totalLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(totalLabel);

                Label totalAmountLabel = new Label();
                totalAmountLabel.Text = printFinalTotal.ToString("₱#,##0.00");
                totalAmountLabel.Font = new Font("Arial", 11, FontStyle.Bold);
                totalAmountLabel.Size = new Size(150, 20);
                totalAmountLabel.Location = new Point(220, yPos);
                totalAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(totalAmountLabel);
                yPos += 25;

                // Payment
                Label paymentLabel = new Label();
                paymentLabel.Text = "Cash Payment:";
                paymentLabel.Font = new Font("Arial", 9);
                paymentLabel.Size = new Size(200, 18);
                paymentLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(paymentLabel);

                Label paymentAmountLabel = new Label();
                paymentAmountLabel.Text = printPayment.ToString("₱#,##0.00");
                paymentAmountLabel.Font = new Font("Arial", 9);
                paymentAmountLabel.Size = new Size(150, 18);
                paymentAmountLabel.Location = new Point(220, yPos);
                paymentAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(paymentAmountLabel);
                yPos += 18;

                // Change
                Label changeLabel = new Label();
                changeLabel.Text = "Change:";
                changeLabel.Font = new Font("Arial", 9);
                changeLabel.Size = new Size(200, 18);
                changeLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(changeLabel);

                Label changeAmountLabel = new Label();
                changeAmountLabel.Text = printChange.ToString("₱#,##0.00");
                changeAmountLabel.Font = new Font("Arial", 9);
                changeAmountLabel.Size = new Size(150, 18);
                changeAmountLabel.Location = new Point(220, yPos);
                changeAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(changeAmountLabel);
                yPos += 30;

                // Footer separator - EXACT same as sales report
                Panel line5 = new Panel();
                line5.Size = new Size(360, 2);
                line5.Location = new Point(10, yPos);
                line5.BackColor = Color.Black;
                receiptPanel.Controls.Add(line5);
                yPos += 15;

                // Thank you message - EXACT same as sales report
                Label thankYouLabel = CreateReceiptLabel("THANK YOU!", new Font("Arial", 14, FontStyle.Bold), 380);
                thankYouLabel.Location = new Point(0, yPos);
                thankYouLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(thankYouLabel);
                yPos += 25;

                Label greatDayLabel = CreateReceiptLabel("Have a great day ahead!", new Font("Arial", 10), 380);
                greatDayLabel.Location = new Point(0, yPos);
                greatDayLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(greatDayLabel);
                yPos += 25;

                // Contact info - EXACT same as sales report
                Label contactLabel = CreateReceiptLabel("For concerns, please contact us at:", new Font("Arial", 8), 380);
                contactLabel.Location = new Point(0, yPos);
                contactLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(contactLabel);
                yPos += 15;

                Label phoneLabel = CreateReceiptLabel("Phone: (02) 123-4567", new Font("Arial", 8), 380);
                phoneLabel.Location = new Point(0, yPos);
                phoneLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(phoneLabel);
                yPos += 12;

                Label emailLabel = CreateReceiptLabel("Email: info@amgcpharmacy.com", new Font("Arial", 8), 380);
                emailLabel.Location = new Point(0, yPos);
                emailLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(emailLabel);

                // Add the receipt panel to the scrollable panel
                scrollablePanel.Controls.Add(receiptPanel);

                // Add the scrollable panel to the form
                receiptForm.Controls.Add(scrollablePanel);

                // Buttons - EXACT same as sales report
                Button printButton = new Button();
                printButton.Text = "Print";
                printButton.Size = new Size(80, 30);
                printButton.Location = new Point(80, 520);
                printButton.Click += (s, ev) => PrintRefundReceipt();
                receiptForm.Controls.Add(printButton);

                Button saveButton = new Button();
                saveButton.Text = "Save as Image";
                saveButton.Size = new Size(100, 30);
                saveButton.Location = new Point(170, 520);
                saveButton.Click += (s, ev) => SaveRefundReceiptAsImage(receiptPanel);
                receiptForm.Controls.Add(saveButton);

                Button closeButton = new Button();
                closeButton.Text = "Close";
                closeButton.Size = new Size(80, 30);
                closeButton.Location = new Point(280, 520);
                closeButton.DialogResult = DialogResult.OK;
                receiptForm.Controls.Add(closeButton);

                receiptForm.AcceptButton = closeButton;
                receiptForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating refund receipt: " + ex.Message, "Receipt Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintRefundReceipt()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = refundPrintDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    refundPrintDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing refund receipt: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveRefundReceiptAsImage(Panel receiptPanel)
        {
            try
            {
                Bitmap bitmap = new Bitmap(receiptPanel.Width, receiptPanel.Height);
                receiptPanel.DrawToBitmap(bitmap, new Rectangle(0, 0, receiptPanel.Width, receiptPanel.Height));

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                saveDialog.Title = "Save Refund Receipt";
                saveDialog.FileName = $"Refund_Receipt_{printOrderID.Replace("/", "-").Replace(":", "-")}.png";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show($"Refund receipt saved successfully!\nLocation: {saveDialog.FileName}",
                        "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving refund receipt: " + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Label CreateReceiptLabel(string text, Font font, int width)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = font;
            label.Size = new Size(width, (int)(font.Size * 1.5));
            label.AutoSize = false;
            return label;
        }
    }
}