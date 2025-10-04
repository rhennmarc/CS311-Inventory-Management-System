using inventory_management;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace Inventory_Management_System
{
    public partial class frmRefund : Form
    {
        private string username;
        private string orderID;
        private Class1 db;
        private DataTable originalOrderData;
        private DataTable currentPageData;
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

        public frmRefund(string username, string orderID = null)
        {
            InitializeComponent();
            this.username = username;
            this.orderID = orderID;
            db = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

            // Initialize print document for refund receipts
            InitializeRefundPrintDocument();

            LoadOrderData();
        }

        private void InitializeRefundPrintDocument()
        {
            refundPrintDocument = new PrintDocument();
            refundPrintDocument.PrintPage += RefundPrintDocument_PrintPage;
            refundPrintDocument.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 280, 600);
            refundPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);
        }

        // Helper method to convert 24-hour time to 12-hour format with AM/PM
        private string ConvertTo12HourFormat(string time24)
        {
            try
            {
                if (string.IsNullOrEmpty(time24))
                    return string.Empty;

                // Handle different time formats that might come from the database
                string[] timeFormats = new[] { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm" };
                DateTime time;

                if (DateTime.TryParseExact(time24, timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
                {
                    return time.ToString("h:mm:ss tt");
                }

                // If parsing fails, try regular DateTime parsing as fallback
                if (DateTime.TryParse(time24, out time))
                {
                    return time.ToString("h:mm:ss tt");
                }

                return time24; // Return original if parsing fails
            }
            catch
            {
                return time24; // Return original if any error occurs
            }
        }

        // Helper method to get current time in 12-hour format
        private string GetCurrentTime12Hour()
        {
            return DateTime.Now.ToString("h:mm:ss tt");
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

                // Order details - EXACT same as sales report with 12-hour time format
                g.DrawString($"Order ID: {printOrderID}", regularFont, brush, leftMargin, yPos);
                yPos += 15;

                // Convert time to 12-hour format for display
                string displayTime = ConvertTo12HourFormat(printTimeCreated);
                g.DrawString($"Date: {printDateCreated} {displayTime}", regularFont, brush, leftMargin, yPos);
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
                        string refundTime = row["timerefunded"]?.ToString() ?? "";
                        string refundReason = row["reason"].ToString();

                        // Convert refund time to 12-hour format
                        string displayRefundTime = ConvertTo12HourFormat(refundTime);

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

                        // Refund details - Refunded on and reason - EXACT same as sales report with 12-hour time
                        g.DrawString($"Refunded on {refundDate} {displayRefundTime} - Reason: {refundReason}", smallFont, Brushes.DarkRed, leftMargin, yPos);
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
                if (printDiscounted)
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

        private void LoadOrderData()
        {
            try
            {
                if (!string.IsNullOrEmpty(orderID))
                {
                    txtOrderID.Text = orderID;
                    LoadOrderItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrderItems(string search = "")
        {
            try
            {
                if (string.IsNullOrEmpty(orderID))
                {
                    MessageBox.Show("Please select an order first.", "No Order Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = $"SELECT * FROM tblsales WHERE orderid = '{orderID.Replace("'", "''")}'";

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(search))
                {
                    query += $" AND products LIKE '%{search.Replace("'", "''")}%'";
                }

                originalOrderData = db.GetData(query);

                if (originalOrderData.Rows.Count == 0)
                {
                    MessageBox.Show("No items found for this order.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Create a copy for editing refund quantities
                DataTable refundData = originalOrderData.Clone();
                refundData.Columns.Add("RefundQuantity", typeof(int));
                refundData.Columns.Add("RefundAmount", typeof(decimal));
                refundData.Columns.Add("IsSelected", typeof(bool));

                foreach (DataRow row in originalOrderData.Rows)
                {
                    DataRow newRow = refundData.NewRow();
                    newRow["orderid"] = row["orderid"];
                    newRow["products"] = row["products"];
                    newRow["quantity"] = row["quantity"];
                    newRow["payment"] = row["payment"];
                    newRow["paymentchange"] = row["paymentchange"];
                    newRow["totalcost"] = row["totalcost"];
                    newRow["discounted"] = row["discounted"];
                    newRow["datecreated"] = row["datecreated"];
                    newRow["timecreated"] = row["timecreated"];
                    newRow["createdby"] = row["createdby"];
                    newRow["RefundQuantity"] = 0;
                    newRow["RefundAmount"] = 0;
                    newRow["IsSelected"] = false;
                    refundData.Rows.Add(newRow);
                }

                currentPageData = refundData;
                ApplyPagination(refundData);
                CalculateTotalRefund();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            dgvOrderItems.DataSource = dtPage;
            StyleDataGridView();

            lblPageInfo.Text = totalRecords == 0 ? "No items found" : $"Page {currentPage} of {totalPages} ({totalRecords} items)";
        }

        private void StyleDataGridView()
        {
            dgvOrderItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvOrderItems.RowTemplate.Height = 30;

            if (dgvOrderItems.Columns.Count > 0)
            {
                dgvOrderItems.Columns["IsSelected"].HeaderText = "Select";
                dgvOrderItems.Columns["IsSelected"].Width = 50;
                dgvOrderItems.Columns["IsSelected"].DisplayIndex = 0;

                dgvOrderItems.Columns["products"].HeaderText = "Product";
                dgvOrderItems.Columns["products"].Width = 150;
                dgvOrderItems.Columns["products"].ReadOnly = true;

                dgvOrderItems.Columns["quantity"].HeaderText = "Original Qty";
                dgvOrderItems.Columns["quantity"].Width = 80;
                dgvOrderItems.Columns["quantity"].ReadOnly = true;

                dgvOrderItems.Columns["RefundQuantity"].HeaderText = "Refund Qty";
                dgvOrderItems.Columns["RefundQuantity"].Width = 80;

                dgvOrderItems.Columns["totalcost"].HeaderText = "Total Cost";
                dgvOrderItems.Columns["totalcost"].Width = 90;
                dgvOrderItems.Columns["totalcost"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvOrderItems.Columns["totalcost"].ReadOnly = true;

                dgvOrderItems.Columns["RefundAmount"].HeaderText = "Refund Amount";
                dgvOrderItems.Columns["RefundAmount"].Width = 100;
                dgvOrderItems.Columns["RefundAmount"].DefaultCellStyle.Format = "₱#,##0.00";
                dgvOrderItems.Columns["RefundAmount"].ReadOnly = true;

                // Hide unnecessary columns
                string[] columnsToHide = { "orderid", "payment", "paymentchange", "discounted", "datecreated", "timecreated", "createdby" };
                foreach (string colName in columnsToHide)
                {
                    if (dgvOrderItems.Columns.Contains(colName))
                        dgvOrderItems.Columns[colName].Visible = false;
                }
            }
        }

        private void dgvOrderItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = dgvOrderItems.Columns[e.ColumnIndex].Name;

                if (columnName == "RefundQuantity" || columnName == "IsSelected")
                {
                    CalculateRefundAmount(e.RowIndex);
                    CalculateTotalRefund();
                    UpdateNumericUpDown();
                }
            }
        }

        private void CalculateRefundAmount(int rowIndex)
        {
            try
            {
                if (dgvOrderItems.Rows[rowIndex].Cells["RefundQuantity"].Value != null &&
                    dgvOrderItems.Rows[rowIndex].Cells["totalcost"].Value != null &&
                    dgvOrderItems.Rows[rowIndex].Cells["quantity"].Value != null)
                {
                    int refundQty = Convert.ToInt32(dgvOrderItems.Rows[rowIndex].Cells["RefundQuantity"].Value);
                    decimal totalCost = Convert.ToDecimal(dgvOrderItems.Rows[rowIndex].Cells["totalcost"].Value);
                    int originalQty = Convert.ToInt32(dgvOrderItems.Rows[rowIndex].Cells["quantity"].Value);

                    if (refundQty > originalQty)
                    {
                        MessageBox.Show("Refund quantity cannot exceed original quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvOrderItems.Rows[rowIndex].Cells["RefundQuantity"].Value = originalQty;
                        refundQty = originalQty;
                    }

                    if (refundQty < 0)
                    {
                        dgvOrderItems.Rows[rowIndex].Cells["RefundQuantity"].Value = 0;
                        refundQty = 0;
                    }

                    decimal unitPrice = totalCost / originalQty;
                    decimal refundAmount = unitPrice * refundQty;

                    // Apply discount if applicable
                    bool discounted = dgvOrderItems.Rows[rowIndex].Cells["discounted"].Value?.ToString().ToUpper() == "YES";
                    if (discounted)
                    {
                        refundAmount *= 0.8m; // 20% discount
                    }

                    dgvOrderItems.Rows[rowIndex].Cells["RefundAmount"].Value = refundAmount;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating refund amount: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateTotalRefund()
        {
            decimal totalRefund = 0;
            int totalItems = 0;

            foreach (DataGridViewRow row in dgvOrderItems.Rows)
            {
                if (row.Cells["RefundAmount"].Value != null)
                {
                    totalRefund += Convert.ToDecimal(row.Cells["RefundAmount"].Value);
                }
                if (row.Cells["IsSelected"]?.Value != null && Convert.ToBoolean(row.Cells["IsSelected"].Value))
                {
                    totalItems++;
                }
            }

            txtTotalRefund.Text = totalRefund.ToString("₱#,##0.00");
            lblSelectedItems.Text = $"{totalItems} item(s) selected for refund";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadOrderItems(txtSearch.Text.Trim());
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtReason.Text.Trim()))
                {
                    MessageBox.Show("Please provide a reason for the refund.", "Reason Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedItems = GetSelectedItemsForRefund();
                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("Please select at least one item to refund.", "No Items Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show($"Are you sure you want to refund {selectedItems.Count} item(s) totaling {txtTotalRefund.Text}?",
                    "Confirm Refund", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ProcessRefund(selectedItems);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing refund: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefundAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtReason.Text.Trim()))
                {
                    MessageBox.Show("Please provide a reason for the refund.", "Reason Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Select all items with their full quantities
                foreach (DataGridViewRow row in dgvOrderItems.Rows)
                {
                    row.Cells["IsSelected"].Value = true;
                    int originalQty = Convert.ToInt32(row.Cells["quantity"].Value);
                    row.Cells["RefundQuantity"].Value = originalQty;
                    CalculateRefundAmount(row.Index);
                }

                CalculateTotalRefund();

                var allItems = GetSelectedItemsForRefund();
                if (allItems.Count == 0)
                {
                    MessageBox.Show("No items found to refund.", "No Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show($"Are you sure you want to refund ALL {allItems.Count} item(s) totaling {txtTotalRefund.Text}?",
                    "Confirm Full Refund", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ProcessRefund(allItems, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing full refund: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<DataGridViewRow> GetSelectedItemsForRefund()
        {
            var selectedItems = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dgvOrderItems.Rows)
            {
                if (row.Cells["IsSelected"]?.Value != null && Convert.ToBoolean(row.Cells["IsSelected"].Value) &&
                    row.Cells["RefundQuantity"]?.Value != null && Convert.ToInt32(row.Cells["RefundQuantity"].Value) > 0)
                {
                    selectedItems.Add(row);
                }
            }

            return selectedItems;
        }

        private void ProcessRefund(List<DataGridViewRow> refundItems, bool isFullRefund = false)
        {
            try
            {
                string refundDate = DateTime.Now.ToString("MM/dd/yyyy");
                // Get current time in 12-hour format for storage and display
                string refundTime = GetCurrentTime12Hour(); // Store in 12-hour format

                // Get original order details for receipt
                DataRow firstRow = originalOrderData.Rows[0];
                string originalDate = firstRow["datecreated"].ToString();
                string originalTime = firstRow["timecreated"].ToString();
                string originalCashier = firstRow["createdby"].ToString();
                bool discounted = firstRow["discounted"].ToString().ToUpper() == "YES";
                decimal originalPayment = Convert.ToDecimal(firstRow["payment"]);
                decimal originalChange = Convert.ToDecimal(firstRow["paymentchange"]);

                // Convert original time to 12-hour format for display
                string displayOriginalTime = ConvertTo12HourFormat(originalTime);

                // Create DataTable for UPDATED products (remaining after refund)
                DataTable updatedProductsData = new DataTable();
                updatedProductsData.Columns.Add("products", typeof(string));
                updatedProductsData.Columns.Add("quantity", typeof(int));
                updatedProductsData.Columns.Add("unitprice", typeof(decimal));

                // Create DataTable for REFUNDED products
                DataTable refundReceiptData = new DataTable();
                refundReceiptData.Columns.Add("products", typeof(string));
                refundReceiptData.Columns.Add("quantity", typeof(int));
                refundReceiptData.Columns.Add("unitprice", typeof(decimal));
                refundReceiptData.Columns.Add("daterefunded", typeof(string));
                refundReceiptData.Columns.Add("timerefunded", typeof(string)); // Add time column
                refundReceiptData.Columns.Add("reason", typeof(string));

                decimal totalRefundAmount = 0;

                // Process each refund item
                foreach (DataGridViewRow row in refundItems)
                {
                    string product = row.Cells["products"].Value.ToString();
                    int refundQty = Convert.ToInt32(row.Cells["RefundQuantity"].Value);
                    decimal refundAmount = Convert.ToDecimal(row.Cells["RefundAmount"].Value);
                    decimal unitPrice = refundAmount / refundQty;

                    // Add to refund receipt data
                    DataRow refundRow = refundReceiptData.NewRow();
                    refundRow["products"] = product;
                    refundRow["quantity"] = refundQty;
                    refundRow["unitprice"] = unitPrice;
                    refundRow["daterefunded"] = refundDate;
                    refundRow["timerefunded"] = refundTime; // Store the 12-hour time
                    refundRow["reason"] = txtReason.Text.Trim();
                    refundReceiptData.Rows.Add(refundRow);

                    totalRefundAmount += refundAmount;

                    // Insert into tblrefunds - store time in 12-hour format
                    string insertRefund = $"INSERT INTO tblrefunds (orderid, products, quantity, unitprice, reason, daterefunded, timerefunded, refundedby) " +
                        $"VALUES ('{orderID.Replace("'", "''")}', '{product.Replace("'", "''")}', '{refundQty}', '{unitPrice:F2}', " +
                        $"'{txtReason.Text.Trim().Replace("'", "''")}', '{refundDate}', '{refundTime}', '{username.Replace("'", "''")}')";
                    db.executeSQL(insertRefund);

                    // Update product stock
                    UpdateProductStock(product, refundQty);

                    // Update sales record and get remaining products
                    UpdateSalesRecordAndGetRemaining(product, refundQty, refundAmount, updatedProductsData);
                }

                // Log the refund action - store time in 12-hour format
                db.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                    "VALUES ('" + refundDate + "', '" + refundTime + "', 'REFUND', 'SALES', " +
                    "'ORDER ID: " + orderID.Replace("'", "''") + " - " + refundItems.Count + " items', '" + username.Replace("'", "''") + "')");

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

                // Store print data for receipt - EXACT same structure as sales report
                printOrderID = orderID;
                printFinalTotal = updatedOrderTotal; // Updated order total after refund
                printPayment = originalPayment; // Original cash payment
                printChange = originalChange; // Original change
                printOrderData = updatedProductsData; // Updated products (remaining after refund)
                printRefundData = refundReceiptData; // Refunded items with details
                printDiscounted = discounted; // Keep original discount status
                printDateCreated = originalDate; // Original order date
                printTimeCreated = displayOriginalTime; // Original order time converted to 12-hour format
                printCreatedBy = originalCashier; // Original cashier
                printTotalRefund = totalRefundAmount; // Total refund amount

                // Show success message and receipt
                MessageBox.Show($"Successfully refunded {refundItems.Count} item(s).\nTotal Refund Amount: {txtTotalRefund.Text}",
                    "Refund Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Show refund receipt
                ShowRefundReceipt();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing refund: " + ex.Message, "Refund Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                // Order info - EXACT same layout as sales report with 12-hour time
                Label orderInfoLabel = CreateReceiptLabel($"Order ID: {printOrderID}", new Font("Arial", 9), 380);
                orderInfoLabel.Location = new Point(10, yPos);
                receiptPanel.Controls.Add(orderInfoLabel);
                yPos += 18;

                // Convert time to 12-hour format for display
                string displayTime = ConvertTo12HourFormat(printTimeCreated);
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
                        string refundTime = row["timerefunded"]?.ToString() ?? "";
                        string refundReason = row["reason"].ToString();

                        // Convert refund time to 12-hour format (already stored in 12-hour format)
                        string displayRefundTime = refundTime; // Already in 12-hour format

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

                        // Refund details - Refunded on and reason - EXACT same as sales report with 12-hour time
                        Label refundDetailsLabel = CreateReceiptLabel($"Refunded on {refundDate} {displayRefundTime} - Reason: {refundReason}", new Font("Arial", 7, FontStyle.Italic), 380);
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
                if (printDiscounted)
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

        private void UpdateProductStock(string product, int refundQty)
        {
            try
            {
                // Get current stock
                string getStock = $"SELECT currentstock FROM tblproducts WHERE products = '{product.Replace("'", "''")}'";
                DataTable stockData = db.GetData(getStock);

                if (stockData.Rows.Count > 0)
                {
                    int currentStock = Convert.ToInt32(stockData.Rows[0]["currentstock"]);
                    int newStock = currentStock + refundQty;

                    // Update stock
                    string updateStock = $"UPDATE tblproducts SET currentstock = '{newStock}' WHERE products = '{product.Replace("'", "''")}'";
                    db.executeSQL(updateStock);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating product stock for {product}: {ex.Message}");
            }
        }

        private void UpdateSalesRecordAndGetRemaining(string product, int refundQty, decimal refundAmount, DataTable updatedProductsData)
        {
            try
            {
                // Get current sales record
                string getSales = $"SELECT quantity, totalcost FROM tblsales WHERE orderid = '{orderID.Replace("'", "''")}' AND products = '{product.Replace("'", "''")}'";
                DataTable salesData = db.GetData(getSales);

                if (salesData.Rows.Count > 0)
                {
                    int currentQty = Convert.ToInt32(salesData.Rows[0]["quantity"]);
                    decimal currentTotal = Convert.ToDecimal(salesData.Rows[0]["totalcost"]);

                    int newQty = currentQty - refundQty;
                    decimal newTotal = currentTotal - refundAmount;

                    if (newQty <= 0)
                    {
                        // Remove the sales record if quantity becomes zero or negative
                        string deleteSales = $"DELETE FROM tblsales WHERE orderid = '{orderID.Replace("'", "''")}' AND products = '{product.Replace("'", "''")}'";
                        db.executeSQL(deleteSales);
                    }
                    else
                    {
                        // Update the sales record
                        string updateSales = $"UPDATE tblsales SET quantity = '{newQty}', totalcost = '{newTotal:F2}' " +
                            $"WHERE orderid = '{orderID.Replace("'", "''")}' AND products = '{product.Replace("'", "''")}'";
                        db.executeSQL(updateSales);

                        // Add to updated products data for receipt
                        DataRow updatedRow = updatedProductsData.NewRow();
                        updatedRow["products"] = product;
                        updatedRow["quantity"] = newQty;
                        updatedRow["unitprice"] = newTotal / newQty;
                        updatedProductsData.Rows.Add(updatedRow);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating sales record for {product}: {ex.Message}");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                ApplyPagination(currentPageData);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ApplyPagination(currentPageData);
            }
        }

        private void dgvOrderItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle checkbox clicks
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvOrderItems.Columns["IsSelected"].Index)
            {
                dgvOrderItems.EndEdit();
                UpdateNumericUpDown();
            }
        }

        private void dgvOrderItems_SelectionChanged(object sender, EventArgs e)
        {
            UpdateNumericUpDown();
        }

        private void UpdateNumericUpDown()
        {
            if (dgvOrderItems.CurrentRow != null &&
                dgvOrderItems.CurrentRow.Cells["IsSelected"].Value != null &&
                Convert.ToBoolean(dgvOrderItems.CurrentRow.Cells["IsSelected"].Value))
            {
                numQuantity.Enabled = true;
                int currentQty = Convert.ToInt32(dgvOrderItems.CurrentRow.Cells["RefundQuantity"].Value);
                int maxQty = Convert.ToInt32(dgvOrderItems.CurrentRow.Cells["quantity"].Value);

                numQuantity.Value = currentQty;
                numQuantity.Maximum = maxQty;
                numQuantity.Minimum = 0;
            }
            else
            {
                numQuantity.Enabled = false;
                numQuantity.Value = 0;
            }
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (dgvOrderItems.CurrentRow != null && numQuantity.Enabled)
            {
                dgvOrderItems.CurrentRow.Cells["RefundQuantity"].Value = (int)numQuantity.Value;
                CalculateRefundAmount(dgvOrderItems.CurrentRow.Index);
                CalculateTotalRefund();
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter key
            {
                currentPage = 1;
                LoadOrderItems(txtSearch.Text.Trim());
            }
        }
    }
}