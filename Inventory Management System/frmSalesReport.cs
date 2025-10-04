using inventory_management;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Text;

namespace Inventory_Management_System
{
    public partial class frmSalesReport : Form
    {
        private string username;
        private string usertype;
        private int salesRow = -1;
        private int refundsRow = -1;
        private int currentSalesPage = 1;
        private int currentRefundsPage = 1;
        private int pageSize = 10;
        private int totalSalesRecords = 0;
        private int totalRefundsRecords = 0;
        private int totalSalesPages = 0;
        private int totalRefundsPages = 0;

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
                currentSalesPage = 1;
                currentRefundsPage = 1;
                LoadSalesReport();
                LoadRefundsReport();
            };

            cmbduration.SelectedIndexChanged += (s, e) =>
            {
                currentSalesPage = 1;
                currentRefundsPage = 1;
                LoadSalesReport();
                LoadRefundsReport();
            };

            txtsearch.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)13) // Enter
                {
                    currentSalesPage = 1;
                    currentRefundsPage = 1;
                    LoadSalesReport(txtsearch.Text.Trim());
                    LoadRefundsReport(txtsearch.Text.Trim());
                }
            };

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
        }

        private void InitializeReceiptPrintDocument()
        {
            receiptPrintDocument = new PrintDocument();
            receiptPrintDocument.PrintPage += ReceiptPrintDocument_PrintPage;
            receiptPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 280, 600);
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

                // REFUND SECTION
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

                        // Refund details - FIXED: Use only daterefunded, not timerefunded
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
            LoadRefundsReport();
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasSalesSelection = salesRow >= 0 && salesRow < dataGridView1.Rows.Count;
            bool hasRefundsSelection = refundsRow >= 0 && refundsRow < dataGridView2.Rows.Count;

            btndelete.Enabled = (hasSalesSelection || hasRefundsSelection) && usertype != null && usertype.ToUpperInvariant() == "ADMINISTRATOR";
            btndeleteall.Enabled = usertype != null && usertype.ToUpperInvariant() == "ADMINISTRATOR";
            btnview.Enabled = hasSalesSelection || hasRefundsSelection;
            btnrefund.Enabled = hasSalesSelection;
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
                ApplySalesPagination(groupedData);

                // Calculate total sales for the duration
                CalculateTotalSales(allSalesData);

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on LoadSalesReport", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRefundsReport(string search = "")
        {
            try
            {
                string duration = cmbduration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dateTimePicker1.Value;

                // Get all refunds data for the selected duration
                System.Data.DataTable refundsData = GetAllRefundsData(duration, selectedDate);

                // Group refunds by orderid to show summary
                System.Data.DataTable groupedRefunds = GroupRefundsByOrderID(refundsData);

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(search))
                {
                    groupedRefunds = ApplyRefundsSearchFilter(groupedRefunds, search);
                }

                // Apply pagination
                ApplyRefundsPagination(groupedRefunds);

                // Calculate total refunds for the duration
                CalculateTotalRefunds(refundsData);

                // Update overall total
                UpdateOverallTotal();

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on LoadRefundsReport", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private System.Data.DataTable GroupRefundsByOrderID(System.Data.DataTable refundsData)
        {
            System.Data.DataTable groupedTable = new System.Data.DataTable();
            groupedTable.Columns.Add("orderid", typeof(string));
            groupedTable.Columns.Add("totalamount", typeof(decimal));
            groupedTable.Columns.Add("daterefunded", typeof(string));
            groupedTable.Columns.Add("timerefunded", typeof(string));
            groupedTable.Columns.Add("itemcount", typeof(int));

            var orderGroups = refundsData.AsEnumerable()
                .GroupBy(row => row.Field<string>("orderid"));

            foreach (var group in orderGroups)
            {
                decimal totalRefund = group.Sum(row =>
                    Convert.ToDecimal(row["unitprice"]) * Convert.ToInt32(row["quantity"]));

                DataRow firstRow = group.First();
                groupedTable.Rows.Add(
                    group.Key,
                    totalRefund,
                    firstRow.Field<string>("daterefunded"),
                    firstRow.Field<string>("timerefunded"),
                    group.Count()
                );
            }

            return groupedTable;
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

            // FIXED: Proper date and time sorting with PM on top
            query += " ORDER BY STR_TO_DATE(datecreated, '%m/%d/%Y') DESC, " +
                     "CASE " +
                     "WHEN timecreated LIKE '%pm%' THEN 1 " +
                     "WHEN timecreated LIKE '%am%' THEN 2 " +
                     "ELSE 3 END, " +
                     "STR_TO_DATE(timecreated, '%h:%i:%s %p') DESC";

            return sales.GetData(query);
        }

        private System.Data.DataTable GetAllRefundsData(string duration, DateTime selectedDate)
        {
            string query = "SELECT orderid, products, quantity, unitprice, reason, daterefunded, timerefunded, refundedby FROM tblrefunds WHERE 1=1";

            switch (duration)
            {
                case "Daily":
                    string dailyDate = selectedDate.ToString("MM/dd/yyyy");
                    query += " AND daterefunded = '" + dailyDate.Replace("'", "''") + "'";
                    break;

                case "Weekly":
                    DateTime startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek);
                    DateTime endOfWeek = startOfWeek.AddDays(6);
                    query += " AND STR_TO_DATE(daterefunded, '%m/%d/%Y') BETWEEN '" + startOfWeek.ToString("yyyy-MM-dd") + "' AND '" + endOfWeek.ToString("yyyy-MM-dd") + "'";
                    break;

                case "Monthly":
                    DateTime startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                    query += " AND STR_TO_DATE(daterefunded, '%m/%d/%Y') BETWEEN '" + startOfMonth.ToString("yyyy-MM-dd") + "' AND '" + endOfMonth.ToString("yyyy-MM-dd") + "'";
                    break;

                case "Yearly":
                    DateTime startOfYear = new DateTime(selectedDate.Year, 1, 1);
                    DateTime endOfYear = new DateTime(selectedDate.Year, 12, 31);
                    query += " AND STR_TO_DATE(daterefunded, '%m/%d/%Y') BETWEEN '" + startOfYear.ToString("yyyy-MM-dd") + "' AND '" + endOfYear.ToString("yyyy-MM-dd") + "'";
                    break;
            }

            // FIXED: Proper date and time sorting with PM on top for refunds too
            query += " ORDER BY STR_TO_DATE(daterefunded, '%m/%d/%Y') DESC, " +
                     "CASE " +
                     "WHEN timerefunded LIKE '%pm%' THEN 1 " +
                     "WHEN timerefunded LIKE '%am%' THEN 2 " +
                     "ELSE 3 END, " +
                     "STR_TO_DATE(timerefunded, '%h:%i:%s %p') DESC";

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

        private System.Data.DataTable ApplyRefundsSearchFilter(System.Data.DataTable data, string search)
        {
            if (string.IsNullOrEmpty(search)) return data;

            System.Data.DataTable filteredData = data.Clone();
            string searchLower = search.ToLower();

            foreach (DataRow row in data.Rows)
            {
                if (row["orderid"].ToString().ToLower().Contains(searchLower) ||
                    row["totalamount"].ToString().ToLower().Contains(searchLower) ||
                    row["daterefunded"].ToString().ToLower().Contains(searchLower) ||
                    row["timerefunded"].ToString().ToLower().Contains(searchLower) ||
                    row["itemcount"].ToString().ToLower().Contains(searchLower))
                {
                    filteredData.ImportRow(row);
                }
            }

            return filteredData;
        }

        private void ApplySalesPagination(System.Data.DataTable data)
        {
            totalSalesRecords = data.Rows.Count;
            totalSalesPages = (int)Math.Ceiling(totalSalesRecords / (double)pageSize);
            if (totalSalesPages == 0) totalSalesPages = 1;
            if (currentSalesPage > totalSalesPages) currentSalesPage = totalSalesPages;

            System.Data.DataTable dtPage = data.Clone();
            int startIndex = (currentSalesPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, totalSalesRecords);

            for (int i = startIndex; i < endIndex; i++)
            {
                dtPage.ImportRow(data.Rows[i]);
            }

            dataGridView1.DataSource = dtPage;
            StyleSalesDataGridView();

            // Clear selection and reset row index
            dataGridView1.ClearSelection();
            salesRow = -1;

            // Page info
            if (totalSalesRecords == 0)
            {
                lblPageInfo.Text = "No records found";
                currentSalesPage = 1;
            }
            else
            {
                lblPageInfo.Text = $"Page {currentSalesPage} of {totalSalesPages} ({totalSalesRecords} orders)";
            }
        }

        private void ApplyRefundsPagination(System.Data.DataTable data)
        {
            totalRefundsRecords = data.Rows.Count;
            totalRefundsPages = (int)Math.Ceiling(totalRefundsRecords / (double)pageSize);
            if (totalRefundsPages == 0) totalRefundsPages = 1;
            if (currentRefundsPage > totalRefundsPages) currentRefundsPage = totalRefundsPages;

            System.Data.DataTable dtPage = data.Clone();
            int startIndex = (currentRefundsPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, totalRefundsRecords);

            for (int i = startIndex; i < endIndex; i++)
            {
                dtPage.ImportRow(data.Rows[i]);
            }

            dataGridView2.DataSource = dtPage;
            StyleRefundsDataGridView();

            // Clear selection and reset row index
            dataGridView2.ClearSelection();
            refundsRow = -1;

            // Page info
            if (totalRefundsRecords == 0)
            {
                label8.Text = "No records found";
                currentRefundsPage = 1;
            }
            else
            {
                label8.Text = $"Page {currentRefundsPage} of {totalRefundsPages} ({totalRefundsRecords} refunds)";
            }
        }

        private void StyleSalesDataGridView()
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

        private void StyleRefundsDataGridView()
        {
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView2.RowTemplate.Height = 28;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.DefaultCellStyle.Padding = new Padding(4);
            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            if (dataGridView2.Columns.Contains("orderid"))
            {
                dataGridView2.Columns["orderid"].HeaderText = "Order ID";
                dataGridView2.Columns["orderid"].Width = 150;
            }
            if (dataGridView2.Columns.Contains("totalamount"))
            {
                dataGridView2.Columns["totalamount"].HeaderText = "Total Refund";
                dataGridView2.Columns["totalamount"].Width = 120;
                dataGridView2.Columns["totalamount"].DefaultCellStyle.Format = "₱#,##0.00";
                dataGridView2.Columns["totalamount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dataGridView2.Columns.Contains("daterefunded"))
            {
                dataGridView2.Columns["daterefunded"].HeaderText = "Date Refunded";
                dataGridView2.Columns["daterefunded"].Width = 100;
            }
            if (dataGridView2.Columns.Contains("timerefunded"))
            {
                dataGridView2.Columns["timerefunded"].HeaderText = "Time Refunded";
                dataGridView2.Columns["timerefunded"].Width = 90;
                dataGridView2.Columns["timerefunded"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dataGridView2.Columns.Contains("itemcount"))
            {
                dataGridView2.Columns["itemcount"].HeaderText = "Items";
                dataGridView2.Columns["itemcount"].Width = 60;
                dataGridView2.Columns["itemcount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        private void CalculateTotalRefunds(System.Data.DataTable refundsData)
        {
            try
            {
                decimal totalRefunds = 0m;
                foreach (DataRow dr in refundsData.Rows)
                {
                    if (dr["unitprice"] != null && dr["unitprice"] != DBNull.Value && dr["quantity"] != null && dr["quantity"] != DBNull.Value)
                    {
                        decimal unitPrice = Convert.ToDecimal(dr["unitprice"]);
                        int quantity = Convert.ToInt32(dr["quantity"]);
                        totalRefunds += unitPrice * quantity;
                    }
                }
                txtrefunds.Text = "₱" + totalRefunds.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on CalculateTotalRefunds", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtrefunds.Text = "₱0.00";
            }
        }

        private void UpdateOverallTotal()
        {
            try
            {
                decimal totalSales = decimal.Parse(txttotal.Text.Replace("₱", "").Replace(",", ""));
                decimal totalRefunds = decimal.Parse(txtrefunds.Text.Replace("₱", "").Replace(",", ""));
                // CHANGED: Add instead of subtract
                decimal overallTotal = totalSales + totalRefunds;
                txtoveralltotal.Text = "₱" + overallTotal.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating overall total: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtoveralltotal.Text = "₱0.00";
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentSalesPage = 1;
            currentRefundsPage = 1;
            string searchText = txtsearch.Text.Trim();
            LoadSalesReport(searchText);
            LoadRefundsReport(searchText);
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            currentSalesPage = 1;
            currentRefundsPage = 1;
            txtsearch.Text = "";
            dateTimePicker1.Value = DateTime.Today;
            cmbduration.SelectedIndex = 0;
            LoadSalesReport();
            LoadRefundsReport();
        }

        private void btnNextSales_Click(object sender, EventArgs e)
        {
            if (currentSalesPage < totalSalesPages)
            {
                currentSalesPage++;
                LoadSalesReport(txtsearch.Text.Trim());
            }
        }

        private void btnPrevSales_Click(object sender, EventArgs e)
        {
            if (currentSalesPage > 1)
            {
                currentSalesPage--;
                LoadSalesReport(txtsearch.Text.Trim());
            }
        }

        private void btnNextRefund_Click(object sender, EventArgs e)
        {
            if (currentRefundsPage < totalRefundsPages)
            {
                currentRefundsPage++;
                LoadRefundsReport(txtsearch.Text.Trim());
            }
        }

        private void btnPrevRefund_Click(object sender, EventArgs e)
        {
            if (currentRefundsPage > 1)
            {
                currentRefundsPage--;
                LoadRefundsReport(txtsearch.Text.Trim());
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

                // Check if sales row is selected
                if (salesRow >= 0 && salesRow < dataGridView1.Rows.Count)
                {
                    // Delete sales order
                    DataGridViewRow sel = dataGridView1.Rows[salesRow];
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
                        LoadRefundsReport(txtsearch.Text.Trim());
                    }
                }
                // Check if refunds row is selected
                else if (refundsRow >= 0 && refundsRow < dataGridView2.Rows.Count)
                {
                    // Delete refund records
                    DataGridViewRow sel = dataGridView2.Rows[refundsRow];
                    string orderid = sel.Cells["orderid"].Value?.ToString() ?? "";

                    DialogResult dr = MessageBox.Show($"Delete all refund records for order {orderid}? This cannot be undone.", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        // First, get the refund items to restore stock
                        string getRefundsQuery = $"SELECT products, quantity FROM tblrefunds WHERE orderid = '{orderid.Replace("'", "''")}'";
                        DataTable refundItems = sales.GetData(getRefundsQuery);

                        // Restore product stock for each refunded item
                        foreach (DataRow refundRow in refundItems.Rows)
                        {
                            string product = refundRow["products"].ToString();
                            int quantity = Convert.ToInt32(refundRow["quantity"]);

                            // Get current stock
                            string getStockQuery = $"SELECT currentstock FROM tblproducts WHERE products = '{product.Replace("'", "''")}'";
                            DataTable stockData = sales.GetData(getStockQuery);

                            if (stockData.Rows.Count > 0)
                            {
                                int currentStock = Convert.ToInt32(stockData.Rows[0]["currentstock"]);
                                int newStock = currentStock - quantity; // Subtract because we're removing the refund

                                // Update stock
                                string updateStock = $"UPDATE tblproducts SET currentstock = '{newStock}' WHERE products = '{product.Replace("'", "''")}'";
                                sales.executeSQL(updateStock);
                            }
                        }

                        // Delete the refund records
                        string del = "DELETE FROM tblrefunds WHERE orderid = '" + orderid.Replace("'", "''") + "'";
                        sales.executeSQL(del);

                        if (sales.rowAffected > 0)
                        {
                            MessageBox.Show($"Refund records for order {orderid} deleted successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            sales.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                                "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("HH:mm:ss") +
                                "', 'DELETE REFUND', 'SALES REPORT', 'ORDER ID: " + orderid.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");
                        }

                        LoadRefundsReport(txtsearch.Text.Trim());
                        LoadSalesReport(txtsearch.Text.Trim());
                    }
                }
                else
                {
                    MessageBox.Show("Please select a sales order or refund record first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    LoadRefundsReport();
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
                // Check if sales row is selected
                if (salesRow >= 0 && salesRow < dataGridView1.Rows.Count)
                {
                    // View sales receipt
                    DataGridViewRow sel = dataGridView1.Rows[salesRow];
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
                // Check if refunds row is selected
                else if (refundsRow >= 0 && refundsRow < dataGridView2.Rows.Count)
                {
                    // View refund receipt
                    DataGridViewRow sel = dataGridView2.Rows[refundsRow];
                    string orderid = sel.Cells["orderid"].Value?.ToString() ?? "";

                    // Get all refund items for this order
                    string query = "SELECT * FROM tblrefunds WHERE orderid = '" + orderid.Replace("'", "''") + "' ORDER BY products";
                    DataTable refundData = sales.GetData(query);

                    if (refundData.Rows.Count == 0)
                    {
                        MessageBox.Show("No refund details found for the selected order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Get original order information - but don't show error if not found
                    string orderQuery = "SELECT * FROM tblsales WHERE orderid = '" + orderid.Replace("'", "''") + "' LIMIT 1";
                    DataTable orderData = sales.GetData(orderQuery);

                    // Check if we have original order data or if it's all refunded
                    bool hasOriginalOrder = orderData.Rows.Count > 0;

                    if (hasOriginalOrder)
                    {
                        // Process with original order details
                        ProcessReceiptWithOriginalOrder(orderid, refundData, orderData);
                    }
                    else
                    {
                        // Process as refund-only receipt
                        ProcessRefundOnlyReceipt(orderid, refundData);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a sales order or refund record first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error viewing receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NEW METHOD: Process receipt when original order data is available
        private void ProcessReceiptWithOriginalOrder(string orderID, DataTable refundData, DataTable orderData)
        {
            try
            {
                DataRow firstRow = orderData.Rows[0];
                decimal payment = Convert.ToDecimal(firstRow["payment"]);
                decimal change = Convert.ToDecimal(firstRow["paymentchange"]);
                string dateCreated = firstRow["datecreated"].ToString();
                string timeCreated = firstRow["timecreated"].ToString();
                string createdBy = firstRow["createdby"].ToString();
                bool discounted = firstRow["discounted"].ToString().ToUpper() == "YES";

                // Get current sales items (remaining products after refund)
                string currentSalesQuery = "SELECT * FROM tblsales WHERE orderid = '" + orderID.Replace("'", "''") + "' ORDER BY products";
                DataTable currentSalesData = sales.GetData(currentSalesQuery);

                // Create DataTable for UPDATED products (remaining after refund)
                DataTable updatedProductsData = new DataTable();
                updatedProductsData.Columns.Add("products", typeof(string));
                updatedProductsData.Columns.Add("quantity", typeof(int));
                updatedProductsData.Columns.Add("totalcost", typeof(decimal));

                foreach (DataRow row in currentSalesData.Rows)
                {
                    string product = row["products"].ToString();
                    int quantity = Convert.ToInt32(row["quantity"]);
                    decimal totalCost = Convert.ToDecimal(row["totalcost"]);

                    DataRow updatedRow = updatedProductsData.NewRow();
                    updatedRow["products"] = product;
                    updatedRow["quantity"] = quantity;
                    updatedRow["totalcost"] = totalCost;
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

                foreach (DataRow row in refundData.Rows)
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
                    updatedOrderTotal += Convert.ToDecimal(row["totalcost"]);
                }

                // Apply discount if original order was discounted
                if (discounted)
                {
                    updatedOrderTotal *= 0.8m;
                }

                // Show receipt with refund information
                ShowReceipt(orderID, updatedOrderTotal, payment, change, updatedProductsData, discounted, dateCreated, timeCreated, createdBy, refundReceiptData, totalRefundAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing receipt with original order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NEW METHOD: Process receipt when only refund data is available (original order not found)
        private void ProcessRefundOnlyReceipt(string orderID, DataTable refundData)
        {
            try
            {
                // Get the first refund record to use for date/time/cashier info
                DataRow firstRefund = refundData.Rows[0];
                string refundDate = firstRefund["daterefunded"].ToString();
                string refundedBy = firstRefund["refundedby"].ToString();

                // Create DataTable for REFUNDED products
                DataTable refundReceiptData = new DataTable();
                refundReceiptData.Columns.Add("products", typeof(string));
                refundReceiptData.Columns.Add("quantity", typeof(int));
                refundReceiptData.Columns.Add("unitprice", typeof(decimal));
                refundReceiptData.Columns.Add("daterefunded", typeof(string));
                refundReceiptData.Columns.Add("reason", typeof(string));

                decimal totalRefundAmount = 0;

                foreach (DataRow row in refundData.Rows)
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
                updatedProductsData.Columns.Add("totalcost", typeof(decimal));

                // Show refund receipt
                ShowReceipt(orderID, 0, totalRefundAmount, 0, updatedProductsData, false, refundDate, "", refundedBy, refundReceiptData, totalRefundAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing refund-only receipt: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetRefundData(string orderID)
        {
            try
            {
                string query = "SELECT products, quantity, unitprice, reason, daterefunded, refundedby " +
                              "FROM tblrefunds WHERE orderid = '" + orderID.Replace("'", "''") + "' " +
                              "ORDER BY daterefunded DESC";
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
                int baseHeight = 400;
                int itemHeight = 20;
                int refundItemHeight = 32;
                int extraHeightForMultipleQty = 0;

                foreach (DataRow row in orderData.Rows)
                {
                    int quantity = Convert.ToInt32(row["quantity"]);
                    if (quantity > 1)
                        extraHeightForMultipleQty += 15;
                }

                int refundSectionHeight = 0;
                if (refundData.Rows.Count > 0)
                {
                    refundSectionHeight = 80 + (refundData.Rows.Count * refundItemHeight);
                    foreach (DataRow row in refundData.Rows)
                    {
                        int quantity = Convert.ToInt32(row["quantity"]);
                        if (quantity > 1)
                            refundSectionHeight += 12;
                    }
                }

                int calculatedHeight = baseHeight + (orderData.Rows.Count * itemHeight) + extraHeightForMultipleQty + refundSectionHeight;
                int panelHeight = Math.Max(550, calculatedHeight);

                Panel scrollablePanel = new Panel();
                scrollablePanel.Size = new Size(400, 500);
                scrollablePanel.Location = new Point(10, 10);
                scrollablePanel.BackColor = Color.White;
                scrollablePanel.BorderStyle = BorderStyle.FixedSingle;
                scrollablePanel.AutoScroll = true;

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
                    decimal unitPrice = Convert.ToDecimal(row["totalcost"]) / quantity;
                    decimal itemTotal = Convert.ToDecimal(row["totalcost"]);

                    decimal displayUnitPrice = discounted ? unitPrice * 0.8m : unitPrice;
                    decimal displayItemTotal = discounted ? itemTotal * 0.8m : itemTotal;

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

                // REFUND SECTION
                if (refundData.Rows.Count > 0)
                {
                    Panel refundSeparator = new Panel();
                    refundSeparator.Size = new Size(360, 2);
                    refundSeparator.Location = new Point(10, yPos);
                    refundSeparator.BackColor = Color.Red;
                    receiptPanel.Controls.Add(refundSeparator);
                    yPos += 10;

                    Label refundHeaderLabel = CreateReceiptLabel("REFUNDED ITEMS", new Font("Arial", 10, FontStyle.Bold), 380);
                    refundHeaderLabel.Location = new Point(0, yPos);
                    refundHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
                    refundHeaderLabel.ForeColor = Color.Red;
                    receiptPanel.Controls.Add(refundHeaderLabel);
                    yPos += 20;

                    foreach (DataRow row in refundData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                        decimal refundAmount = quantity * unitPrice;
                        string reason = row["reason"].ToString();
                        string refundDate = row["daterefunded"].ToString();
                        // FIXED: Removed timerefunded reference

                        if (productName.Length > 20)
                            productName = productName.Substring(0, 17) + "...";

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

                        if (quantity > 1)
                        {
                            Label unitPriceLabel = CreateReceiptLabel($"  @ {unitPrice.ToString("₱#,##0.00")} each", new Font("Arial", 8, FontStyle.Italic), 380);
                            unitPriceLabel.Location = new Point(20, yPos);
                            unitPriceLabel.ForeColor = Color.Gray;
                            receiptPanel.Controls.Add(unitPriceLabel);
                            yPos += 12;
                        }

                        Label refundDetailsLabel = CreateReceiptLabel($"Refunded on {refundDate} - Reason: {reason}", new Font("Arial", 7, FontStyle.Italic), 380);
                        refundDetailsLabel.Location = new Point(15, yPos);
                        refundDetailsLabel.ForeColor = Color.DarkRed;
                        receiptPanel.Controls.Add(refundDetailsLabel);
                        yPos += 14;
                    }

                    yPos += 10;

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

                scrollablePanel.Controls.Add(receiptPanel);
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
                // Get all sales and refunds data for export
                string duration = cmbduration.SelectedItem?.ToString() ?? "Daily";
                DateTime selectedDate = dateTimePicker1.Value;
                System.Data.DataTable salesData = GetAllSalesData(duration, selectedDate);
                System.Data.DataTable refundsData = GetAllRefundsData(duration, selectedDate);

                if (salesData.Rows.Count == 0 && refundsData.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export for the selected criteria.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files|*.csv|Excel Files|*.xlsx";
                saveDialog.Title = "Export Sales and Refunds Report";
                string durationText = duration.ToLower();
                string dateText = selectedDate.ToString("yyyy-MM-dd");
                saveDialog.FileName = $"SalesRefundsReport_{durationText}_{dateText}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();
                    if (extension == ".csv")
                    {
                        ExportToCSV(salesData, refundsData, saveDialog.FileName, duration, selectedDate);
                    }
                    else
                    {
                        string csvFileName = Path.ChangeExtension(saveDialog.FileName, ".csv");
                        ExportToCSV(salesData, refundsData, csvFileName, duration, selectedDate);
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

        private void ExportToCSV(System.Data.DataTable salesData, System.Data.DataTable refundsData, string fileName, string duration, DateTime selectedDate)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Add title and header information
                csvContent.AppendLine("AMGC PHARMACY - SALES AND REFUNDS REPORT");
                csvContent.AppendLine($"Duration: {duration}, Date: {selectedDate:MM/dd/yyyy}, Generated: {DateTime.Now:MM/dd/yyyy hh:mm:ss tt}");
                csvContent.AppendLine(); // Empty line

                // Add SALES DATA section
                csvContent.AppendLine("SALES DATA");
                csvContent.AppendLine("Order ID,Product,Quantity,Payment,Payment Change,Total Cost,Discounted,Date Created,Time Created,Created By");

                // Sort sales data by date and time (latest to oldest)
                DataView dvSales = salesData.DefaultView;
                dvSales.Sort = "datecreated DESC, timecreated DESC";
                System.Data.DataTable sortedSalesData = dvSales.ToTable();

                decimal totalSales = 0;
                foreach (DataRow row in sortedSalesData.Rows)
                {
                    string orderid = EscapeCSVField(row["orderid"].ToString());
                    string products = EscapeCSVField(row["products"].ToString());
                    string quantity = row["quantity"].ToString();

                    decimal payment = Convert.ToDecimal(row["payment"]);
                    decimal paymentChange = Convert.ToDecimal(row["paymentchange"]);

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

                csvContent.AppendLine(); // Empty line

                // Add REFUNDS DATA section
                csvContent.AppendLine("REFUNDS DATA");
                csvContent.AppendLine("Order ID,Product,Quantity,Unit Price,Reason,Date Refunded,Time Refunded,Refunded By");

                // Sort refunds data by date and time (latest to oldest)
                DataView dvRefunds = refundsData.DefaultView;
                dvRefunds.Sort = "daterefunded DESC, timerefunded DESC";
                System.Data.DataTable sortedRefundsData = dvRefunds.ToTable();

                decimal totalRefunds = 0;
                foreach (DataRow row in sortedRefundsData.Rows)
                {
                    string orderid = EscapeCSVField(row["orderid"].ToString());
                    string products = EscapeCSVField(row["products"].ToString());
                    string quantity = row["quantity"].ToString();
                    string unitPrice = row["unitprice"].ToString();
                    string reason = EscapeCSVField(row["reason"].ToString());
                    string dateRefunded = EscapeCSVField(row["daterefunded"].ToString());
                    string timeRefunded = EscapeCSVField(row["timerefunded"].ToString());
                    string refundedBy = EscapeCSVField(row["refundedby"].ToString());

                    csvContent.AppendLine($"{orderid},{products},{quantity},{unitPrice},{reason},{dateRefunded},{timeRefunded},{refundedBy}");

                    totalRefunds += Convert.ToDecimal(unitPrice) * Convert.ToInt32(quantity);
                }

                csvContent.AppendLine(); // Empty line

                // Add TOTALS section
                csvContent.AppendLine("TOTALS SUMMARY");
                csvContent.AppendLine($"Total Sales,{totalSales:F2}");
                csvContent.AppendLine($"Total Refunds,{totalRefunds:F2}");
                // CHANGED: Add instead of subtract
                csvContent.AppendLine($"Overall Total,{totalSales + totalRefunds:F2}");

                // Write to file
                File.WriteAllText(fileName, csvContent.ToString(), Encoding.UTF8);

                MessageBox.Show($"Sales and refunds report exported successfully!\nLocation: {fileName}\n" +
                    $"Sales Records: {sortedSalesData.Rows.Count}\nRefund Records: {sortedRefundsData.Rows.Count}\n" +
                    $"Total Sales: ₱{totalSales:N2}\nTotal Refunds: ₱{totalRefunds:N2}\nOverall Total: ₱{totalSales + totalRefunds:N2}",
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
                    {
                        salesRow = idx;
                        // Clear selection in refunds grid
                        dataGridView2.ClearSelection();
                        refundsRow = -1;
                    }
                    else
                    {
                        salesRow = -1;
                    }
                }
                else
                {
                    salesRow = -1;
                }
                UpdateButtonStates();
            }
            catch
            {
                salesRow = -1;
                UpdateButtonStates();
            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.CurrentCell != null)
                {
                    int idx = dataGridView2.CurrentCell.RowIndex;
                    if (idx >= 0 && idx < dataGridView2.Rows.Count)
                    {
                        refundsRow = idx;
                        // Clear selection in sales grid
                        dataGridView1.ClearSelection();
                        salesRow = -1;
                    }
                    else
                    {
                        refundsRow = -1;
                    }
                }
                else
                {
                    refundsRow = -1;
                }
                UpdateButtonStates();
            }
            catch
            {
                refundsRow = -1;
                UpdateButtonStates();
            }
        }

        private void btnrefund_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedOrderID = null;

                if (salesRow >= 0 && salesRow < dataGridView1.Rows.Count)
                {
                    DataGridViewRow sel = dataGridView1.Rows[salesRow];
                    selectedOrderID = sel.Cells["orderid"].Value?.ToString() ?? "";
                }

                using (frmRefund refundForm = new frmRefund(username, selectedOrderID))
                {
                    if (refundForm.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Refund processed successfully.", "Refund Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSalesReport(); // Refresh the sales report
                        LoadRefundsReport(); // Refresh the refunds report
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening refund form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}