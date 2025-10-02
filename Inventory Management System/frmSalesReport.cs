using inventory_management;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
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

        // Print-related variables for receipts
        private PrintDocument receiptPrintDocument;
        private string printOrderID;
        private decimal printFinalTotal;
        private decimal printPayment;
        private decimal printChange;
        private System.Data.DataTable printOrderData;
        private bool printDiscounted;
        private string printDateCreated;
        private string printTimeCreated;
        private string printCreatedBy;
        private DataTable printRefundData;
        private decimal printTotalRefund;

        Class1 sales = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmSalesReport(string username, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.usertype = usertype;

            // Initialize duration combobox
            cmbduration.Items.AddRange(new string[] { "Daily", "Weekly", "Monthly", "Yearly" });
            cmbduration.SelectedIndex = 0;

            // DatePicker setup: default to today, format MM/dd/yyyy
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy";
            dateTimePicker1.Value = DateTime.Today;

            // Initialize print document for receipts
            InitializeReceiptPrintDocument();

            // Event handlers
            dateTimePicker1.ValueChanged += (s, e) =>
            {
                currentPage = 1;
                LoadSalesReport();
            };

            cmbduration.SelectedIndexChanged += (s, e) =>
            {
                currentPage = 1;
                LoadSalesReport();
            };

            txtsearch.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)13) // Enter
                {
                    currentPage = 1;
                    LoadSalesReport(txtsearch.Text.Trim());
                }
            };

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void InitializeReceiptPrintDocument()
        {
            receiptPrintDocument = new PrintDocument();
            receiptPrintDocument.PrintPage += ReceiptPrintDocument_PrintPage;
            receiptPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 280, 600); // 280 points = ~3.9 inches width
            receiptPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);
        }

        private void ReceiptPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
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

                // Header
                string headerText = "AMGC PHARMACY";
                SizeF headerSize = g.MeasureString(headerText, titleFont);
                g.DrawString(headerText, titleFont, brush, centerPos - (headerSize.Width / 2), yPos);
                yPos += 20;

                string taglineText = "Your Health, Our Priority";
                SizeF taglineSize = g.MeasureString(taglineText, regularFont);
                g.DrawString(taglineText, regularFont, brush, centerPos - (taglineSize.Width / 2), yPos);
                yPos += 15;

                // ADD ADDRESS BELOW TAGLINE
                string addressText = "1340 G Tuazon St Sampaloc Manila";
                SizeF addressSize = g.MeasureString(addressText, smallFont);
                g.DrawString(addressText, smallFont, brush, centerPos - (addressSize.Width / 2), yPos);
                yPos += 20;

                // Line separator
                g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                yPos += 15;

                // Order details
                g.DrawString($"Order ID: {printOrderID}", regularFont, brush, leftMargin, yPos);
                yPos += 15;
                g.DrawString($"Date: {printDateCreated} {printTimeCreated}", regularFont, brush, leftMargin, yPos);
                yPos += 15;
                g.DrawString($"Cashier: {printCreatedBy}", regularFont, brush, leftMargin, yPos);
                yPos += 15;

                // ADD TIN BELOW CASHIER
                string tinText = "Tin:";
                g.DrawString(tinText, smallFont, brush, leftMargin, yPos);
                yPos += 20;

                // Line separator
                g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                yPos += 10;

                // Column headers
                g.DrawString("Product", headerFont, brush, leftMargin, yPos);
                g.DrawString("Qty", headerFont, brush, 140, yPos);
                g.DrawString("Amount", headerFont, brush, 180, yPos);
                yPos += 15;

                // Products
                foreach (DataRow row in printOrderData.Rows)
                {
                    string productName = row["products"].ToString();
                    int quantity = Convert.ToInt32(row["quantity"]);
                    decimal unitPrice = Convert.ToDecimal(row["totalcost"]) / quantity;
                    decimal itemTotal = Convert.ToDecimal(row["totalcost"]);

                    // Apply discount if applicable
                    decimal displayUnitPrice = printDiscounted ? unitPrice * 0.8m : unitPrice;
                    decimal displayItemTotal = printDiscounted ? itemTotal * 0.8m : itemTotal;

                    // Truncate product name if too long for receipt
                    if (productName.Length > 18)
                        productName = productName.Substring(0, 15) + "...";

                    g.DrawString(productName, regularFont, brush, leftMargin, yPos);
                    g.DrawString(quantity.ToString(), regularFont, brush, 145, yPos);
                    g.DrawString(displayItemTotal.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                    yPos += 12;

                    // Unit price for multiple quantities
                    if (quantity > 1)
                    {
                        g.DrawString($"  @ {displayUnitPrice:₱#,##0.00} each", smallFont, Brushes.Gray, leftMargin + 5, yPos);
                        yPos += 10;
                    }
                }

                yPos += 10;

                // REFUND SECTION - Only show if there are refunds
                if (printRefundData != null && printRefundData.Rows.Count > 0)
                {
                    // Line separator before refunds
                    g.DrawLine(Pens.Red, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                    yPos += 10;

                    // Refund header
                    string refundHeader = "REFUNDED ITEMS";
                    SizeF refundHeaderSize = g.MeasureString(refundHeader, headerFont);
                    g.DrawString(refundHeader, headerFont, Brushes.Red, centerPos - (refundHeaderSize.Width / 2), yPos);
                    yPos += 15;

                    // Refunded items
                    foreach (DataRow row in printRefundData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                        decimal itemTotal = quantity * unitPrice;
                        string refundDate = row["daterefunded"].ToString();
                        string reason = row["reason"].ToString();

                        // Truncate product name if too long for receipt
                        if (productName.Length > 18)
                            productName = productName.Substring(0, 15) + "...";

                        g.DrawString(productName, regularFont, brush, leftMargin, yPos);
                        g.DrawString($"-{quantity}", regularFont, Brushes.Red, 145, yPos);
                        g.DrawString($"-{itemTotal:₱#,##0.00}", regularFont, Brushes.Red, 180, yPos);
                        yPos += 12;

                        // Unit price for multiple quantities
                        if (quantity > 1)
                        {
                            g.DrawString($"  @ {unitPrice:₱#,##0.00} each", smallFont, Brushes.Gray, leftMargin + 5, yPos);
                            yPos += 10;
                        }

                        // Refund details
                        g.DrawString($"Refunded on {refundDate} - Reason: {reason}", smallFont, Brushes.DarkRed, leftMargin, yPos);
                        yPos += 10;
                    }

                    yPos += 5;

                    // TOTAL REFUND line
                    g.DrawLine(Pens.Red, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                    yPos += 10;

                    g.DrawString("TOTAL REFUND:", headerFont, Brushes.Red, leftMargin, yPos);
                    g.DrawString(printTotalRefund.ToString("-₱#,##0.00"), headerFont, Brushes.Red, 180, yPos);
                    yPos += 20;
                }

                // Line separator
                g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                yPos += 15;

                // Totals section
                if (printDiscounted)
                {
                    decimal originalTotal = 0;
                    foreach (DataRow row in printOrderData.Rows)
                    {
                        originalTotal += Convert.ToDecimal(row["totalcost"]);
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

                // Final total
                g.DrawString("TOTAL AMOUNT:", headerFont, brush, leftMargin, yPos);
                g.DrawString(printFinalTotal.ToString("₱#,##0.00"), headerFont, brush, 180, yPos);
                yPos += 20;

                // Payment details
                g.DrawString("Cash Payment:", regularFont, brush, leftMargin, yPos);
                g.DrawString(printPayment.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                yPos += 12;

                g.DrawString("Change:", regularFont, brush, leftMargin, yPos);
                g.DrawString(printChange.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                yPos += 25;

                // Footer
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

                // Contact info
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
                MessageBox.Show("Error during printing: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmSalesReport_Load(object sender, EventArgs e)
        {
            LoadSalesReport();
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = row >= 0 && row < dataGridView1.Rows.Count;
            btndelete.Enabled = hasSelection && usertype != null && usertype.ToUpperInvariant() == "ADMINISTRATOR";
            btndeleteall.Enabled = usertype != null && usertype.ToUpperInvariant() == "ADMINISTRATOR";
            btnview.Enabled = hasSelection;
        }

        private void LoadSalesReport(string search = "")
        {
            try
            {
                string duration = cmbduration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dateTimePicker1.Value;

                // Get all sales data for the selected duration
                System.Data.DataTable allSalesData = GetAllSalesData(duration, selectedDate);

                // Group by orderid to get unique orders
                System.Data.DataTable groupedData = GroupSalesByOrderID(allSalesData);

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(search))
                {
                    groupedData = ApplySearchFilter(groupedData, search);
                }

                // Apply pagination
                ApplyPagination(groupedData);

                // Calculate total sales for the duration
                CalculateTotalSales(allSalesData);

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on LoadSalesReport", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private System.Data.DataTable GetAllSalesData(string duration, DateTime selectedDate)
        {
            string query = "SELECT orderid, products, quantity, payment, paymentchange, totalcost, discounted, datecreated, timecreated, createdby FROM tblsales WHERE 1=1";

            switch (duration)
            {
                case "Daily":
                    string dailyDate = selectedDate.ToString("MM/dd/yyyy");
                    query += " AND datecreated = '" + dailyDate.Replace("'", "''") + "'";
                    break;

                case "Weekly":
                    DateTime startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(6);
                    query += " AND STR_TO_DATE(datecreated, '%m/%d/%Y') BETWEEN '" + startOfWeek.ToString("yyyy-MM-dd") + "' AND '" + endOfWeek.ToString("yyyy-MM-dd") + "'";
                    break;

                case "Monthly":
                    DateTime startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                    query += " AND STR_TO_DATE(datecreated, '%m/%d/%Y') BETWEEN '" + startOfMonth.ToString("yyyy-MM-dd") + "' AND '" + endOfMonth.ToString("yyyy-MM-dd") + "'";
                    break;

                case "Yearly":
                    DateTime startOfYear = new DateTime(selectedDate.Year, 1, 1);
                    DateTime endOfYear = new DateTime(selectedDate.Year, 12, 31);
                    query += " AND STR_TO_DATE(datecreated, '%m/%d/%Y') BETWEEN '" + startOfYear.ToString("yyyy-MM-dd") + "' AND '" + endOfYear.ToString("yyyy-MM-dd") + "'";
                    break;
            }

            query += " ORDER BY STR_TO_DATE(datecreated, '%m/%d/%Y') DESC, STR_TO_DATE(timecreated, '%H:%i:%s') DESC";

            return sales.GetData(query);
        }

        private System.Data.DataTable GroupSalesByOrderID(System.Data.DataTable salesData)
        {
            System.Data.DataTable groupedTable = new System.Data.DataTable();
            groupedTable.Columns.Add("orderid", typeof(string));
            groupedTable.Columns.Add("total", typeof(decimal));
            groupedTable.Columns.Add("datecreated", typeof(string));
            groupedTable.Columns.Add("timecreated", typeof(string));

            var orderGroups = salesData.AsEnumerable()
                .GroupBy(row => row.Field<string>("orderid"));

            foreach (var group in orderGroups)
            {
                decimal orderTotal = group.Sum(row =>
                {
                    string totalCostStr = row.Field<string>("totalcost") ?? "0";
                    string cleanTotal = Regex.Replace(totalCostStr, @"[^\d\.\-]", "");
                    return decimal.TryParse(cleanTotal, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal total) ? total : 0;
                });

                DataRow firstRow = group.First();
                groupedTable.Rows.Add(
                    group.Key,
                    orderTotal,
                    firstRow.Field<string>("datecreated"),
                    firstRow.Field<string>("timecreated")
                );
            }

            return groupedTable;
        }

        private System.Data.DataTable ApplySearchFilter(System.Data.DataTable data, string search)
        {
            if (string.IsNullOrEmpty(search)) return data;

            System.Data.DataTable filteredData = data.Clone();
            string searchLower = search.ToLower();

            foreach (DataRow row in data.Rows)
            {
                if (row["orderid"].ToString().ToLower().Contains(searchLower) ||
                    row["total"].ToString().ToLower().Contains(searchLower) ||
                    row["datecreated"].ToString().ToLower().Contains(searchLower) ||
                    row["timecreated"].ToString().ToLower().Contains(searchLower))
                {
                    filteredData.ImportRow(row);
                }
            }

            return filteredData;
        }

        private void ApplyPagination(System.Data.DataTable data)
        {
            totalRecords = data.Rows.Count;
            totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (currentPage > totalPages) currentPage = totalPages;

            System.Data.DataTable dtPage = data.Clone();
            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, totalRecords);

            for (int i = startIndex; i < endIndex; i++)
            {
                dtPage.ImportRow(data.Rows[i]);
            }

            dataGridView1.DataSource = dtPage;
            StyleDataGridView();

            // Clear selection and reset row index
            dataGridView1.ClearSelection();
            row = -1;

            // Page info
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

        private void StyleDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView1.RowTemplate.Height = 28;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DefaultCellStyle.Padding = new Padding(4);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            if (dataGridView1.Columns.Contains("orderid"))
            {
                dataGridView1.Columns["orderid"].HeaderText = "Order ID";
                dataGridView1.Columns["orderid"].Width = 180;
            }
            if (dataGridView1.Columns.Contains("total"))
            {
                dataGridView1.Columns["total"].HeaderText = "Total Amount";
                dataGridView1.Columns["total"].Width = 120;
                dataGridView1.Columns["total"].DefaultCellStyle.Format = "₱#,##0.00";
                dataGridView1.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
        }

        private void CalculateTotalSales(System.Data.DataTable salesData)
        {
            try
            {
                decimal grandTotal = 0m;
                foreach (DataRow dr in salesData.Rows)
                {
                    if (dr["totalcost"] != null && dr["totalcost"] != DBNull.Value)
                    {
                        string raw = dr["totalcost"].ToString();
                        string cleaned = Regex.Replace(raw, @"[^\d\.\-]", "");
                        if (!string.IsNullOrWhiteSpace(cleaned))
                        {
                            if (decimal.TryParse(cleaned, NumberStyles.Number | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal value))
                            {
                                grandTotal += value;
                            }
                        }
                    }
                }
                txttotal.Text = "₱" + grandTotal.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on CalculateTotalSales", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txttotal.Text = "₱0.00";
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadSalesReport(txtsearch.Text.Trim());
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            txtsearch.Text = "";
            dateTimePicker1.Value = DateTime.Today;
            cmbduration.SelectedIndex = 0;
            LoadSalesReport();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadSalesReport(txtsearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadSalesReport(txtsearch.Text.Trim());
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
                    MessageBox.Show("Please select an order first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow sel = dataGridView1.Rows[row];
                string orderid = sel.Cells["orderid"].Value?.ToString() ?? "";

                DialogResult dr = MessageBox.Show($"Delete all records for order {orderid}? This cannot be undone.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string del = "DELETE FROM tblsales WHERE orderid = '" + orderid.Replace("'", "''") + "'";
                    sales.executeSQL(del);

                    if (sales.rowAffected > 0)
                    {
                        MessageBox.Show($"Order {orderid} deleted successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sales.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                            "', 'DELETE', 'SALES REPORT', 'ORDER ID: " + orderid.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");
                    }

                    LoadSalesReport(txtsearch.Text.Trim());
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

                string duration = cmbduration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dateTimePicker1.Value;
                string durationText = $"{duration} sales for {selectedDate:MM/dd/yyyy}";

                DialogResult dr = MessageBox.Show($"Are you sure you want to delete ALL {durationText}? This cannot be undone.", "Delete All Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    string whereClause = GetDurationWhereClause(duration, selectedDate);
                    string delAll = "DELETE FROM tblsales WHERE " + whereClause;
                    sales.executeSQL(delAll);

                    if (sales.rowAffected > 0)
                    {
                        MessageBox.Show($"{sales.rowAffected} sale(s) deleted for {durationText}.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sales.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                            "', 'DELETE ALL', 'SALES REPORT', '" + durationText.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");
                    }
                    else
                    {
                        MessageBox.Show("No records to delete for the selected duration.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadSalesReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on btndeleteall_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetDurationWhereClause(string duration, DateTime selectedDate)
        {
            switch (duration)
            {
                case "Daily":
                    string dailyDate = selectedDate.ToString("MM/dd/yyyy");
                    return "datecreated = '" + dailyDate.Replace("'", "''") + "'";

                case "Weekly":
                    DateTime startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(6);
                    return "STR_TO_DATE(datecreated, '%m/%d/%Y') BETWEEN '" + startOfWeek.ToString("yyyy-MM-dd") + "' AND '" + endOfWeek.ToString("yyyy-MM-dd") + "'";

                case "Monthly":
                    DateTime startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                    return "STR_TO_DATE(datecreated, '%m/%d/%Y') BETWEEN '" + startOfMonth.ToString("yyyy-MM-dd") + "' AND '" + endOfMonth.ToString("yyyy-MM-dd") + "'";

                case "Yearly":
                    DateTime startOfYear = new DateTime(selectedDate.Year, 1, 1);
                    DateTime endOfYear = new DateTime(selectedDate.Year, 12, 31);
                    return "STR_TO_DATE(datecreated, '%m/%d/%Y') BETWEEN '" + startOfYear.ToString("yyyy-MM-dd") + "' AND '" + endOfYear.ToString("yyyy-MM-dd") + "'";

                default:
                    return "1=1";
            }
        }

        private void btnview_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select an order first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow sel = dataGridView1.Rows[row];
                string orderid = sel.Cells["orderid"].Value?.ToString() ?? "";

                // Get all items for this order
                string query = "SELECT * FROM tblsales WHERE orderid = '" + orderid.Replace("'", "''") + "' ORDER BY products";
                System.Data.DataTable orderData = sales.GetData(query);

                if (orderData.Rows.Count == 0)
                {
                    MessageBox.Show("No order details found for the selected order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get the first row to get common order information
                DataRow firstRow = orderData.Rows[0];
                decimal payment = Convert.ToDecimal(firstRow["payment"]);
                decimal change = Convert.ToDecimal(firstRow["paymentchange"]);
                string dateCreated = firstRow["datecreated"].ToString();
                string timeCreated = firstRow["timecreated"].ToString();
                string createdBy = firstRow["createdby"].ToString();
                bool discounted = firstRow["discounted"].ToString().ToUpper() == "YES";

                // Calculate total from individual items
                decimal total = 0;
                foreach (DataRow row in orderData.Rows)
                {
                    string totalCostStr = row["totalcost"].ToString();
                    string cleanTotal = Regex.Replace(totalCostStr, @"[^\d\.\-]", "");
                    if (decimal.TryParse(cleanTotal, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal itemTotal))
                    {
                        total += itemTotal;
                    }
                }

                // Apply discount if applicable
                decimal finalTotal = discounted ? total * 0.8m : total;

                // Get refund information for this order
                DataTable refundData = GetRefundData(orderid);

                // Calculate total refund amount
                decimal totalRefund = 0;
                if (refundData.Rows.Count > 0)
                {
                    foreach (DataRow row in refundData.Rows)
                    {
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                        totalRefund += quantity * unitPrice;
                    }
                }

                // Show receipt
                ShowReceipt(orderid, finalTotal, payment, change, orderData, discounted, dateCreated, timeCreated, createdBy, refundData, totalRefund);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetRefundData(string orderID)
        {
            try
            {
                string query = "SELECT products, quantity, unitprice, reason, daterefunded, timerefunded, refundedby " +
                              "FROM tblrefunds WHERE orderid = '" + orderID.Replace("'", "''") + "' " +
                              "ORDER BY daterefunded DESC, timerefunded DESC";
                return sales.GetData(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading refund data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        private void ShowReceipt(string orderID, decimal finalTotal, decimal payment, decimal change, System.Data.DataTable orderData, bool discounted, string dateCreated, string timeCreated, string createdBy, DataTable refundData, decimal totalRefund)
        {
            try
            {
                Form receiptForm = new Form();
                receiptForm.Text = "AMGC Pharmacy - Receipt";
                receiptForm.Size = new Size(420, 650);
                receiptForm.StartPosition = FormStartPosition.CenterParent;
                receiptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                receiptForm.MaximizeBox = false;
                receiptForm.MinimizeBox = false;
                receiptForm.BackColor = Color.White;

                // Calculate the required height based on order items and refunds
                int baseHeight = 400; // Base height for header, footer, totals
                int itemHeight = 20; // Height per order item
                int refundItemHeight = 32; // Height per refund item (including details)
                int extraHeightForMultipleQty = 0;

                // Calculate extra height for items with quantity > 1 (they show unit price)
                foreach (DataRow row in orderData.Rows)
                {
                    int quantity = Convert.ToInt32(row["quantity"]);
                    if (quantity > 1)
                        extraHeightForMultipleQty += 15; // Extra height for unit price line
                }

                // Add height for refund section if there are refunds
                int refundSectionHeight = 0;
                if (refundData.Rows.Count > 0)
                {
                    refundSectionHeight = 80 + (refundData.Rows.Count * refundItemHeight); // Header + items + total refund
                    // Add extra height for refund items with multiple quantities
                    foreach (DataRow row in refundData.Rows)
                    {
                        int quantity = Convert.ToInt32(row["quantity"]);
                        if (quantity > 1)
                            refundSectionHeight += 12; // Extra height for unit price line
                    }
                }

                int calculatedHeight = baseHeight + (orderData.Rows.Count * itemHeight) + extraHeightForMultipleQty + refundSectionHeight;
                int panelHeight = Math.Max(550, calculatedHeight); // Minimum 550, or calculated height

                // Create a scrollable panel for the receipt content
                Panel scrollablePanel = new Panel();
                scrollablePanel.Size = new Size(400, 500); // Fixed viewport size
                scrollablePanel.Location = new Point(10, 10);
                scrollablePanel.BackColor = Color.White;
                scrollablePanel.BorderStyle = BorderStyle.FixedSingle;
                scrollablePanel.AutoScroll = true; // Enable scrolling

                // Create the actual receipt content panel
                Panel receiptPanel = new Panel();
                receiptPanel.Size = new Size(380, panelHeight);
                receiptPanel.Location = new Point(0, 0);
                receiptPanel.BackColor = Color.White;

                int yPos = 10;

                // Header
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

                // ADD ADDRESS BELOW TAGLINE
                Label addressLabel = CreateReceiptLabel("1340 G Tuazon St Sampaloc Manila", new Font("Arial", 8), 380);
                addressLabel.Location = new Point(0, yPos);
                addressLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(addressLabel);
                yPos += 20;

                // Separator line
                Panel line1 = new Panel();
                line1.Size = new Size(360, 1);
                line1.Location = new Point(10, yPos);
                line1.BackColor = Color.Black;
                receiptPanel.Controls.Add(line1);
                yPos += 10;

                // Order info
                Label orderInfoLabel = CreateReceiptLabel($"Order ID: {orderID}", new Font("Arial", 9), 380);
                orderInfoLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(orderInfoLabel);
                yPos += 18;

                Label dateTimeLabel = CreateReceiptLabel($"Date: {dateCreated} {timeCreated}", new Font("Arial", 9), 380);
                dateTimeLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(dateTimeLabel);
                yPos += 18;

                Label cashierLabel = CreateReceiptLabel($"Cashier: {createdBy}", new Font("Arial", 9), 380);
                cashierLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(cashierLabel);
                yPos += 18;

                // ADD TIN BELOW CASHIER
                Label tinLabel = CreateReceiptLabel("Tin:", new Font("Arial", 8), 380);
                tinLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(tinLabel);
                yPos += 20;

                // Product header
                Panel line2 = new Panel();
                line2.Size = new Size(360, 1);
                line2.Location = new Point(10, yPos);
                line2.BackColor = Color.Black;
                receiptPanel.Controls.Add(line2);
                yPos += 10;

                // Product header with better alignment
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

                // Products
                foreach (DataRow row in orderData.Rows)
                {
                    string productName = row["products"].ToString();
                    int quantity = Convert.ToInt32(row["quantity"]);
                    decimal unitPrice = Convert.ToDecimal(row["totalcost"]) / quantity; // Calculate unit price from total
                    decimal itemTotal = Convert.ToDecimal(row["totalcost"]);

                    // Apply discount if applicable
                    decimal displayUnitPrice = discounted ? unitPrice * 0.8m : unitPrice;
                    decimal displayItemTotal = discounted ? itemTotal * 0.8m : itemTotal;

                    // Truncate product name if too long
                    if (productName.Length > 20)
                        productName = productName.Substring(0, 17) + "...";

                    // Product line
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

                    // Unit price for multiple quantities
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

                // REFUND SECTION - Only show if there are refunds
                if (refundData.Rows.Count > 0)
                {
                    // Separator line before refunds
                    Panel refundSeparator = new Panel();
                    refundSeparator.Size = new Size(360, 2);
                    refundSeparator.Location = new Point(10, yPos);
                    refundSeparator.BackColor = Color.Red;
                    receiptPanel.Controls.Add(refundSeparator);
                    yPos += 10;

                    // Refund header
                    Label refundHeaderLabel = CreateReceiptLabel("REFUNDED ITEMS", new Font("Arial", 10, FontStyle.Bold), 380);
                    refundHeaderLabel.Location = new Point(0, yPos);
                    refundHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
                    refundHeaderLabel.ForeColor = Color.Red;
                    receiptPanel.Controls.Add(refundHeaderLabel);
                    yPos += 20;

                    // Refund items - FIXED ALIGNMENT to match main products
                    foreach (DataRow row in refundData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                        decimal refundAmount = quantity * unitPrice;
                        string reason = row["reason"].ToString();
                        string refundDate = row["daterefunded"].ToString();
                        string refundTime = row["timerefunded"].ToString();

                        // Truncate product name if too long - SAME as main products
                        if (productName.Length > 20)
                            productName = productName.Substring(0, 17) + "...";

                        // Product line - EXACT SAME ALIGNMENT as main products
                        Label refundProductLabel = new Label();
                        refundProductLabel.Text = productName;
                        refundProductLabel.Font = new Font("Arial", 9);
                        refundProductLabel.Size = new Size(180, 18);
                        refundProductLabel.Location = new Point(10, yPos);
                        receiptPanel.Controls.Add(refundProductLabel);

                        Label refundQtyLabel = new Label();
                        refundQtyLabel.Text = $"-{quantity}";
                        refundQtyLabel.Font = new Font("Arial", 9);
                        refundQtyLabel.Size = new Size(50, 18);
                        refundQtyLabel.Location = new Point(190, yPos);
                        refundQtyLabel.TextAlign = ContentAlignment.MiddleCenter;
                        refundQtyLabel.ForeColor = Color.Red;
                        receiptPanel.Controls.Add(refundQtyLabel);

                        Label refundAmountLabel = new Label();
                        refundAmountLabel.Text = $"-{refundAmount.ToString("₱#,##0.00")}";
                        refundAmountLabel.Font = new Font("Arial", 9);
                        refundAmountLabel.Size = new Size(100, 18);
                        refundAmountLabel.Location = new Point(270, yPos);
                        refundAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                        refundAmountLabel.ForeColor = Color.Red;
                        receiptPanel.Controls.Add(refundAmountLabel);

                        yPos += 18;

                        // Unit price for multiple quantities - SAME as main products
                        if (quantity > 1)
                        {
                            Label unitPriceLabel = CreateReceiptLabel($"  @ {unitPrice.ToString("₱#,##0.00")} each", new Font("Arial", 8, FontStyle.Italic), 380);
                            unitPriceLabel.Location = new Point(20, yPos);
                            unitPriceLabel.ForeColor = Color.Gray;
                            receiptPanel.Controls.Add(unitPriceLabel);
                            yPos += 12;
                        }

                        // Refund details
                        Label refundDetailsLabel = CreateReceiptLabel($"Refunded on {refundDate} - Reason: {reason}", new Font("Arial", 7, FontStyle.Italic), 380);
                        refundDetailsLabel.Location = new Point(15, yPos);
                        refundDetailsLabel.ForeColor = Color.DarkRed;
                        receiptPanel.Controls.Add(refundDetailsLabel);
                        yPos += 14;
                    }

                    yPos += 10;

                    // TOTAL REFUND section
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
                    totalRefundAmountLabel.Text = totalRefund.ToString("-₱#,##0.00");
                    totalRefundAmountLabel.Font = new Font("Arial", 9, FontStyle.Bold);
                    totalRefundAmountLabel.Size = new Size(150, 18);
                    totalRefundAmountLabel.Location = new Point(220, yPos);
                    totalRefundAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                    totalRefundAmountLabel.ForeColor = Color.Red;
                    receiptPanel.Controls.Add(totalRefundAmountLabel);
                    yPos += 25;
                }

                // Separator line
                Panel line3 = new Panel();
                line3.Size = new Size(360, 1);
                line3.Location = new Point(10, yPos);
                line3.BackColor = Color.Black;
                receiptPanel.Controls.Add(line3);
                yPos += 15;

                // Totals section
                if (discounted)
                {
                    decimal originalTotal = 0;
                    foreach (DataRow row in orderData.Rows)
                    {
                        originalTotal += Convert.ToDecimal(row["totalcost"]);
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
                totalAmountLabel.Text = finalTotal.ToString("₱#,##0.00");
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
                paymentAmountLabel.Text = payment.ToString("₱#,##0.00");
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
                changeAmountLabel.Text = change.ToString("₱#,##0.00");
                changeAmountLabel.Font = new Font("Arial", 9);
                changeAmountLabel.Size = new Size(150, 18);
                changeAmountLabel.Location = new Point(220, yPos);
                changeAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(changeAmountLabel);
                yPos += 30;

                // Footer separator
                Panel line5 = new Panel();
                line5.Size = new Size(360, 2);
                line5.Location = new Point(10, yPos);
                line5.BackColor = Color.Black;
                receiptPanel.Controls.Add(line5);
                yPos += 15;

                // Thank you message
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

                // Contact info
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

                // Store print data for printing functionality
                printOrderID = orderID;
                printFinalTotal = finalTotal;
                printPayment = payment;
                printChange = change;
                printOrderData = orderData;
                printDiscounted = discounted;
                printDateCreated = dateCreated;
                printTimeCreated = timeCreated;
                printCreatedBy = createdBy;
                printRefundData = refundData;
                printTotalRefund = totalRefund;

                // Buttons
                Button printButton = new Button();
                printButton.Text = "Print";
                printButton.Size = new Size(80, 30);
                printButton.Location = new Point(80, 520);
                printButton.Click += (s, ev) => PrintReceipt();
                receiptForm.Controls.Add(printButton);

                Button saveButton = new Button();
                saveButton.Text = "Save as Image";
                saveButton.Size = new Size(100, 30);
                saveButton.Location = new Point(170, 520);
                saveButton.Click += (s, ev) => SaveReceiptAsImage(receiptPanel, orderID);
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
                MessageBox.Show("Error generating receipt: " + ex.Message, "Receipt Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReceipt()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = receiptPrintDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    receiptPrintDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing receipt: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void SaveReceiptAsImage(Panel receiptPanel, string orderID)
        {
            try
            {
                Bitmap bitmap = new Bitmap(receiptPanel.Width, receiptPanel.Height);
                receiptPanel.DrawToBitmap(bitmap, new Rectangle(0, 0, receiptPanel.Width, receiptPanel.Height));

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg";
                saveDialog.Title = "Save Receipt";
                saveDialog.FileName = $"Receipt_{orderID.Replace("/", "-").Replace(":", "-")}.png";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(saveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show($"Receipt saved successfully!\nLocation: {saveDialog.FileName}",
                        "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving receipt: " + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnexport_Click(object sender, EventArgs e)
        {
            try
            {
                // Get all sales data for export (based on current filter settings)
                string duration = cmbduration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dateTimePicker1.Value;
                System.Data.DataTable exportData = GetAllSalesData(duration, selectedDate);

                if (exportData.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export for the selected criteria.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Show save dialog - offer both CSV and Excel options
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files|*.csv|Excel Files|*.xlsx";
                saveDialog.Title = "Export Sales Report";
                string durationText = duration.ToLower();
                string dateText = selectedDate.ToString("yyyy-MM-dd");
                saveDialog.FileName = $"SalesReport_{durationText}_{dateText}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();
                    if (extension == ".csv")
                    {
                        ExportToCSV(exportData, saveDialog.FileName, duration, selectedDate);
                    }
                    else
                    {
                        // For .xlsx, we'll export as CSV but suggest they can open it in Excel
                        string csvFileName = Path.ChangeExtension(saveDialog.FileName, ".csv");
                        ExportToCSV(exportData, csvFileName, duration, selectedDate);
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

        private void ExportToCSV(System.Data.DataTable data, string fileName, string duration, DateTime selectedDate)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Add title and header information
                csvContent.AppendLine("AMGC PHARMACY - SALES REPORT");
                csvContent.AppendLine($"Duration: {duration}, Date: {selectedDate:MM/dd/yyyy}, Generated: {DateTime.Now:MM/dd/yyyy hh:mm:ss tt}");
                csvContent.AppendLine(); // Empty line

                // Add column headers
                csvContent.AppendLine("Order ID,Product,Quantity,Payment,Payment Change,Total Cost,Discounted,Date Created,Time Created,Created By");

                // Sort data by date and time (latest to oldest)
                DataView dv = data.DefaultView;
                dv.Sort = "datecreated DESC, timecreated DESC";
                System.Data.DataTable sortedData = dv.ToTable();

                // Add data rows
                decimal totalSales = 0;
                foreach (DataRow row in sortedData.Rows)
                {
                    // Escape commas and quotes in text fields
                    string orderid = EscapeCSVField(row["orderid"].ToString());
                    string products = EscapeCSVField(row["products"].ToString());
                    string quantity = row["quantity"].ToString();

                    decimal payment = Convert.ToDecimal(row["payment"]);
                    decimal paymentChange = Convert.ToDecimal(row["paymentchange"]);

                    // Parse total cost
                    string totalCostStr = row["totalcost"].ToString();
                    string cleanTotal = Regex.Replace(totalCostStr, @"[^\d\.\-]", "");
                    decimal totalCost = decimal.TryParse(cleanTotal, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal cost) ? cost : 0;
                    totalSales += totalCost;

                    string discounted = EscapeCSVField(row["discounted"].ToString());
                    string dateCreated = EscapeCSVField(row["datecreated"].ToString());
                    string timeCreated = EscapeCSVField(row["timecreated"].ToString());
                    string createdBy = EscapeCSVField(row["createdby"].ToString());

                    csvContent.AppendLine($"{orderid},{products},{quantity},{payment:F2},{paymentChange:F2},{totalCost:F2},{discounted},{dateCreated},{timeCreated},{createdBy}");
                }

                // Add totals row
                csvContent.AppendLine(); // Empty line
                csvContent.AppendLine($",,,,TOTAL SALES:,{totalSales:F2},,,,");

                // Write to file
                File.WriteAllText(fileName, csvContent.ToString(), Encoding.UTF8);

                MessageBox.Show($"Sales report exported successfully!\nLocation: {fileName}\nTotal Records: {sortedData.Rows.Count}\nTotal Sales: ₱{totalSales:N2}",
                    "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Log the export action
                sales.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                    "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                    "', 'EXPORT', 'SALES REPORT', 'CSV Export: " + duration + " - " + selectedDate.ToString("MM/dd/yyyy") + "', '" + username.Replace("'", "''") + "')");
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

        private void btnrefund_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedOrderID = null;

                if (row >= 0 && row < dataGridView1.Rows.Count)
                {
                    DataGridViewRow sel = dataGridView1.Rows[row];
                    selectedOrderID = sel.Cells["orderid"].Value?.ToString() ?? "";
                }

                using (frmRefund refundForm = new frmRefund(username, selectedOrderID))
                {
                    if (refundForm.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Refund processed successfully.", "Refund Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSalesReport(); // Refresh the sales report
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening refund form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnviewrefunds_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmViewRefunds viewRefundsForm = new frmViewRefunds(username))
                {
                    viewRefundsForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening refunds view: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}