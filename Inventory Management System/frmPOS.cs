using inventory_management;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmPOS : Form
    {
        private string username;
        private int selectedProductRow = -1;
        private int selectedCartRow = -1;

        // Pagination for products
        private int currentProductPage = 1;
        private int productPageSize = 10;
        private int totalProductRecords = 0;
        private int totalProductPages = 0;

        // Pagination for cart
        private int currentCartPage = 1;
        private int cartPageSize = 10;
        private int totalCartRecords = 0;
        private int totalCartPages = 0;

        // Cart data storage
        private DataTable cartData;

        // Track discount
        private bool discountApplied = false;
        private string discountName = "";
        private string discountId = "";

        // Print-related variables
        private PrintDocument printDocument;
        private string printOrderID;
        private decimal printFinalTotal;
        private decimal printPayment;
        private decimal printChange;

        Class1 pos = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmPOS(string username)
        {
            InitializeComponent();
            this.username = username;

            // Initialize cart data table
            InitializeCartData();

            // Setup event handlers
            dgvproducts.CellClick += dgvproducts_CellClick;
            dgvcart.CellClick += dgvcart_CellClick;
            numquantity.ValueChanged += numquantity_ValueChanged;
            txtsearch.KeyPress += txtsearch_KeyPress;
            txtpayment.KeyPress += txtpayment_KeyPress;
            txtpayment.TextChanged += txtpayment_TextChanged;

            // Initialize NumericUpDown
            numquantity.Minimum = 1;
            numquantity.Maximum = 9999;
            numquantity.Value = 1;

            // Initialize print document
            InitializePrintDocument();
        }

        private void InitializePrintDocument()
        {
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 280, 600);
            printDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);
        }

        private void InitializeCartData()
        {
            cartData = new DataTable();
            cartData.Columns.Add("products", typeof(string));
            cartData.Columns.Add("unitprice", typeof(decimal));
            cartData.Columns.Add("quantity", typeof(int));
            cartData.Columns.Add("subtotal", typeof(decimal));
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadCart();
            UpdateButtonStates();
            UpdateTotal();
        }

        private void UpdateButtonStates()
        {
            bool hasProductSelection = selectedProductRow >= 0 && selectedProductRow < dgvproducts.Rows.Count;
            bool hasCartSelection = selectedCartRow >= 0 && selectedCartRow < dgvcart.Rows.Count;
            bool hasCartItems = cartData.Rows.Count > 0;
            bool hasValidQuantity = numquantity.Value > 0;

            btnadd.Enabled = hasProductSelection && hasValidQuantity;
            btndelete.Enabled = hasCartSelection;
            btndeleteall.Enabled = hasCartItems;
            btndiscount.Enabled = hasCartItems;

            // Purchase button enabled only if cart has items and payment is sufficient
            decimal totalAmount = GetTotalAmount();
            decimal paymentAmount = GetPaymentAmount();
            btnpurchase.Enabled = hasCartItems && paymentAmount >= totalAmount && totalAmount > 0;

            // Pagination buttons
            btnprodNext.Enabled = currentProductPage < totalProductPages;
            btnprodPrev.Enabled = currentProductPage > 1;
            btncartNext.Enabled = currentCartPage < totalCartPages;
            btncartPrev.Enabled = currentCartPage > 1;
        }

        private void LoadProducts(string search = "")
        {
            try
            {
                string query = @"SELECT products, unitprice, currentstock 
                                FROM tblproducts ";
                if (!string.IsNullOrEmpty(search))
                {
                    query += "WHERE products LIKE '%" + search.Replace("'", "''") + "%' ";
                }
                query += "ORDER BY products";

                DataTable dtAll = pos.GetData(query);
                totalProductRecords = dtAll.Rows.Count;
                totalProductPages = (int)Math.Ceiling(totalProductRecords / (double)productPageSize);
                if (totalProductPages == 0) totalProductPages = 1;

                // Paging
                DataTable dtPage = dtAll.Clone();
                int startIndex = (currentProductPage - 1) * productPageSize;
                int endIndex = Math.Min(startIndex + productPageSize, totalProductRecords);

                for (int i = startIndex; i < endIndex; i++)
                {
                    dtPage.ImportRow(dtAll.Rows[i]);
                }

                dgvproducts.DataSource = dtPage;
                StyleDataGridView(dgvproducts);

                if (dgvproducts.Columns.Contains("products"))
                {
                    dgvproducts.Columns["products"].HeaderText = "Product Name";
                    dgvproducts.Columns["products"].Width = 200;
                }
                if (dgvproducts.Columns.Contains("unitprice"))
                {
                    dgvproducts.Columns["unitprice"].HeaderText = "Unit Price";
                    dgvproducts.Columns["unitprice"].DefaultCellStyle.Format = "₱#,##0.00";
                    dgvproducts.Columns["unitprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvproducts.Columns["unitprice"].Width = 120;
                }
                if (dgvproducts.Columns.Contains("currentstock"))
                {
                    dgvproducts.Columns["currentstock"].HeaderText = "Stock";
                    dgvproducts.Columns["currentstock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvproducts.Columns["currentstock"].Width = 80;
                }

                dgvproducts.ClearSelection();
                selectedProductRow = -1;

                if (totalProductRecords == 0)
                {
                    lblPageInfoProd.Text = "No products found";
                }
                else
                {
                    lblPageInfoProd.Text = $"Page {currentProductPage} of {totalProductPages} ({totalProductRecords} products)";
                }

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on LoadProducts", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCart()
        {
            try
            {
                totalCartRecords = cartData.Rows.Count;
                totalCartPages = (int)Math.Ceiling(totalCartRecords / (double)cartPageSize);
                if (totalCartPages == 0) totalCartPages = 1;

                DataTable dtCartPage = cartData.Clone();
                int startIndex = (currentCartPage - 1) * cartPageSize;
                int endIndex = Math.Min(startIndex + cartPageSize, totalCartRecords);

                for (int i = startIndex; i < endIndex; i++)
                {
                    dtCartPage.ImportRow(cartData.Rows[i]);
                }

                dgvcart.DataSource = dtCartPage;
                StyleDataGridView(dgvcart);

                if (dgvcart.Columns.Contains("products"))
                {
                    dgvcart.Columns["products"].HeaderText = "Product";
                    dgvcart.Columns["products"].Width = 150;
                }
                if (dgvcart.Columns.Contains("unitprice"))
                {
                    dgvcart.Columns["unitprice"].HeaderText = "Unit Price";
                    dgvcart.Columns["unitprice"].DefaultCellStyle.Format = "₱#,##0.00";
                    dgvcart.Columns["unitprice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvcart.Columns["unitprice"].Width = 100;
                }
                if (dgvcart.Columns.Contains("quantity"))
                {
                    dgvcart.Columns["quantity"].HeaderText = "Qty";
                    dgvcart.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvcart.Columns["quantity"].Width = 60;
                }
                if (dgvcart.Columns.Contains("subtotal"))
                {
                    dgvcart.Columns["subtotal"].HeaderText = "Subtotal";
                    dgvcart.Columns["subtotal"].DefaultCellStyle.Format = "₱#,##0.00";
                    dgvcart.Columns["subtotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvcart.Columns["subtotal"].Width = 100;
                }

                dgvcart.ClearSelection();
                selectedCartRow = -1;

                if (totalCartRecords == 0)
                {
                    lblPageInfoCart.Text = "Cart is empty";
                }
                else
                {
                    lblPageInfoCart.Text = $"Page {currentCartPage} of {totalCartPages} ({totalCartRecords} items)";
                }

                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on LoadCart", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv.RowTemplate.Height = 28;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.DefaultCellStyle.Padding = new Padding(4);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void UpdateTotal()
        {
            try
            {
                decimal total = 0;
                foreach (DataRow row in cartData.Rows)
                {
                    if (row["subtotal"] != null && row["subtotal"] != DBNull.Value)
                    {
                        total += Convert.ToDecimal(row["subtotal"]);
                    }
                }

                // Apply discount if active
                if (discountApplied && total > 0)
                {
                    total = total * 0.8m;
                }

                txttotal.Text = total.ToString("₱#,##0.00");
                CalculateChange();
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on UpdateTotal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txttotal.Text = "₱0.00";
            }
        }

        private void CalculateChange()
        {
            try
            {
                decimal totalAmount = GetTotalAmount();
                decimal paymentAmount = GetPaymentAmount();

                // If no payment entered, show blank
                if (string.IsNullOrWhiteSpace(txtpayment.Text))
                {
                    txtchange.Text = "";
                    txtchange.ForeColor = System.Drawing.Color.Black;
                    return;
                }

                // If total is 0, show blank
                if (totalAmount == 0)
                {
                    txtchange.Text = "";
                    txtchange.ForeColor = System.Drawing.Color.Black;
                    return;
                }

                if (paymentAmount >= totalAmount)
                {
                    decimal change = paymentAmount - totalAmount;
                    txtchange.Text = change.ToString("₱#,##0.00");
                    txtchange.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    txtchange.Text = "Insufficient";
                    txtchange.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch
            {
                txtchange.Text = "";
                txtchange.ForeColor = System.Drawing.Color.Black;
            }
        }

        private decimal GetTotalAmount()
        {
            string totalText = txttotal.Text.Replace("₱", "").Replace(",", "");
            return decimal.TryParse(totalText, out decimal total) ? total : 0;
        }

        private decimal GetPaymentAmount()
        {
            string paymentText = txtpayment.Text.Replace("₱", "").Replace(",", "");
            return decimal.TryParse(paymentText, out decimal payment) ? payment : 0;
        }

        private string GenerateOrderID()
        {
            string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
            string currentTime = DateTime.Now.ToString("hhmmss");
            string amPm = DateTime.Now.ToString("tt").ToLower();
            return $"order-{currentDate}-{currentTime}-{amPm}";
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
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
                g.DrawString($"Date & Time: {DateTime.Now:MM/dd/yyyy hh:mm:ss tt}", regularFont, brush, leftMargin, yPos);
                yPos += 15;
                g.DrawString($"Cashier: {username}", regularFont, brush, leftMargin, yPos);
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
                foreach (DataRow row in cartData.Rows)
                {
                    string productName = row["products"].ToString();
                    int quantity = Convert.ToInt32(row["quantity"]);
                    decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                    decimal subtotal = Convert.ToDecimal(row["subtotal"]);
                    decimal itemTotal = discountApplied ? subtotal * 0.8m : subtotal;

                    // Truncate product name if too long for receipt
                    if (productName.Length > 18)
                        productName = productName.Substring(0, 15) + "...";

                    g.DrawString(productName, regularFont, brush, leftMargin, yPos);
                    g.DrawString(quantity.ToString(), regularFont, brush, 145, yPos);
                    g.DrawString(itemTotal.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                    yPos += 12;

                    // Unit price for multiple quantities
                    if (quantity > 1)
                    {
                        decimal displayUnitPrice = discountApplied ? unitPrice * 0.8m : unitPrice;
                        g.DrawString($"  @ {displayUnitPrice:₱#,##0.00} each", smallFont, Brushes.Gray, leftMargin + 5, yPos);
                        yPos += 10;
                    }
                }

                yPos += 10;

                // Line separator
                g.DrawLine(Pens.Black, leftMargin, yPos, e.PageBounds.Width - 20, yPos);
                yPos += 15;

                // Totals section
                if (discountApplied)
                {
                    decimal originalTotal = 0;
                    foreach (DataRow row in cartData.Rows)
                    {
                        originalTotal += Convert.ToDecimal(row["subtotal"]);
                    }

                    g.DrawString("Subtotal:", regularFont, brush, leftMargin, yPos);
                    g.DrawString(originalTotal.ToString("₱#,##0.00"), regularFont, brush, 180, yPos);
                    yPos += 12;

                    g.DrawString("Senior/PWD Discount (20%):", regularFont, brush, leftMargin, yPos);
                    g.DrawString((originalTotal * 0.2m).ToString("-₱#,##0.00"), regularFont, Brushes.Red, 180, yPos);
                    yPos += 12;

                    // Discount information
                    if (!string.IsNullOrEmpty(discountName))
                    {
                        g.DrawString($"Discount Name: {discountName}", smallFont, brush, leftMargin, yPos);
                        yPos += 10;
                    }
                    if (!string.IsNullOrEmpty(discountId))
                    {
                        g.DrawString($"Discount ID: {discountId}", smallFont, brush, leftMargin, yPos);
                        yPos += 10;
                    }

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

        private void ShowReceipt(string orderID, decimal finalTotal, decimal payment, decimal change)
        {
            try
            {
                // Store print data
                printOrderID = orderID;
                printFinalTotal = finalTotal;
                printPayment = payment;
                printChange = change;

                // Create a custom form for the receipt
                Form receiptForm = new Form();
                receiptForm.Text = "AMGC Pharmacy - Receipt";
                receiptForm.Size = new System.Drawing.Size(450, 750); // Increased size for better visibility
                receiptForm.StartPosition = FormStartPosition.CenterParent;
                receiptForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                receiptForm.MaximizeBox = false;
                receiptForm.MinimizeBox = false;
                receiptForm.BackColor = System.Drawing.Color.White;

                // Calculate the required height based on cart items
                int baseHeight = 450; // Increased base height
                int itemHeight = 20;
                int extraHeightForMultipleQty = 0;

                foreach (DataRow row in cartData.Rows)
                {
                    int quantity = Convert.ToInt32(row["quantity"]);
                    if (quantity > 1)
                        extraHeightForMultipleQty += 15;
                }

                // Add extra height for discount information if applied
                int discountSectionHeight = 0;
                if (discountApplied)
                {
                    discountSectionHeight = 25; // Extra space for discount name and ID
                }

                int calculatedHeight = baseHeight + (cartData.Rows.Count * itemHeight) + extraHeightForMultipleQty + discountSectionHeight;

                // FIXED: Ensure there's always extra scrollable space
                int panelHeight = Math.Max(600, calculatedHeight) + 100; // Always add 100px extra for scrolling

                // Create a scrollable panel for the receipt content
                Panel scrollablePanel = new Panel();
                scrollablePanel.Size = new System.Drawing.Size(420, 600); // Increased size
                scrollablePanel.Location = new System.Drawing.Point(10, 10);
                scrollablePanel.BackColor = System.Drawing.Color.White;
                scrollablePanel.BorderStyle = BorderStyle.FixedSingle;
                scrollablePanel.AutoScroll = true;
                scrollablePanel.AutoScrollMinSize = new System.Drawing.Size(0, panelHeight); // Important: This enables proper scrolling

                // Create the actual receipt content panel
                Panel receiptPanel = new Panel();
                receiptPanel.Size = new System.Drawing.Size(400, panelHeight); // Use calculated height
                receiptPanel.Location = new System.Drawing.Point(0, 0);
                receiptPanel.BackColor = System.Drawing.Color.White;

                // Create labels for receipt content
                int yPos = 15; // Increased starting position

                // Header
                Label headerLabel = CreateReceiptLabel("AMGC PHARMACY", new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold), 400);
                headerLabel.Location = new System.Drawing.Point(0, yPos);
                headerLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(headerLabel);
                yPos += 30; // Increased spacing

                Label taglineLabel = CreateReceiptLabel("Your Health, Our Priority", new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Italic), 400);
                taglineLabel.Location = new System.Drawing.Point(0, yPos);
                taglineLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(taglineLabel);
                yPos += 25; // Increased spacing

                // ADD ADDRESS BELOW TAGLINE
                Label addressLabel = CreateReceiptLabel("1340 G Tuazon St Sampaloc Manila", new System.Drawing.Font("Arial", 8), 400);
                addressLabel.Location = new System.Drawing.Point(0, yPos);
                addressLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(addressLabel);
                yPos += 25; // Increased spacing

                // Separator line
                Panel line1 = new Panel();
                line1.Size = new System.Drawing.Size(380, 1);
                line1.Location = new System.Drawing.Point(10, yPos);
                line1.BackColor = System.Drawing.Color.Black;
                receiptPanel.Controls.Add(line1);
                yPos += 15;

                // Order info
                Label orderInfoLabel = CreateReceiptLabel($"Order ID: {orderID}", new System.Drawing.Font("Arial", 9), 400);
                orderInfoLabel.Location = new System.Drawing.Point(10, yPos);
                receiptPanel.Controls.Add(orderInfoLabel);
                yPos += 20; // Increased spacing

                Label dateTimeLabel = CreateReceiptLabel($"Date & Time: {DateTime.Now:MM/dd/yyyy hh:mm:ss tt}", new System.Drawing.Font("Arial", 9), 400);
                dateTimeLabel.Location = new System.Drawing.Point(10, yPos);
                receiptPanel.Controls.Add(dateTimeLabel);
                yPos += 20; // Increased spacing

                Label cashierLabel = CreateReceiptLabel($"Cashier: {username}", new System.Drawing.Font("Arial", 9), 400);
                cashierLabel.Location = new System.Drawing.Point(10, yPos);
                receiptPanel.Controls.Add(cashierLabel);
                yPos += 20; // Increased spacing

                // ADD TIN BELOW CASHIER
                Label tinLabel = CreateReceiptLabel("Tin:", new System.Drawing.Font("Arial", 8), 400);
                tinLabel.Location = new System.Drawing.Point(10, yPos);
                receiptPanel.Controls.Add(tinLabel);
                yPos += 25; // Increased spacing

                // Product header
                Panel line2 = new Panel();
                line2.Size = new System.Drawing.Size(380, 1);
                line2.Location = new System.Drawing.Point(10, yPos);
                line2.BackColor = System.Drawing.Color.Black;
                receiptPanel.Controls.Add(line2);
                yPos += 15;

                // Product header with better alignment
                Label productHeaderLabel = new Label();
                productHeaderLabel.Text = "Product";
                productHeaderLabel.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
                productHeaderLabel.Size = new System.Drawing.Size(200, 18);
                productHeaderLabel.Location = new System.Drawing.Point(10, yPos);
                receiptPanel.Controls.Add(productHeaderLabel);

                Label qtyHeaderLabel = new Label();
                qtyHeaderLabel.Text = "Qty";
                qtyHeaderLabel.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
                qtyHeaderLabel.Size = new System.Drawing.Size(50, 18);
                qtyHeaderLabel.Location = new System.Drawing.Point(210, yPos);
                qtyHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(qtyHeaderLabel);

                Label amountHeaderLabel = new Label();
                amountHeaderLabel.Text = "Amount";
                amountHeaderLabel.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
                amountHeaderLabel.Size = new System.Drawing.Size(120, 18);
                amountHeaderLabel.Location = new System.Drawing.Point(270, yPos);
                amountHeaderLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(amountHeaderLabel);
                yPos += 22; // Increased spacing

                // Products
                foreach (DataRow row in cartData.Rows)
                {
                    string productName = row["products"].ToString();
                    int quantity = Convert.ToInt32(row["quantity"]);
                    decimal unitPrice = Convert.ToDecimal(row["unitprice"]);
                    decimal subtotal = Convert.ToDecimal(row["subtotal"]);
                    decimal itemTotal = discountApplied ? subtotal * 0.8m : subtotal;

                    // Truncate product name if too long
                    if (productName.Length > 22) // Increased character limit
                        productName = productName.Substring(0, 19) + "...";

                    // Product line
                    Label productLabel = new Label();
                    productLabel.Text = productName;
                    productLabel.Font = new System.Drawing.Font("Arial", 9);
                    productLabel.Size = new System.Drawing.Size(200, 18);
                    productLabel.Location = new System.Drawing.Point(10, yPos);
                    receiptPanel.Controls.Add(productLabel);

                    Label qtyLabel = new Label();
                    qtyLabel.Text = quantity.ToString();
                    qtyLabel.Font = new System.Drawing.Font("Arial", 9);
                    qtyLabel.Size = new System.Drawing.Size(50, 18);
                    qtyLabel.Location = new System.Drawing.Point(210, yPos);
                    qtyLabel.TextAlign = ContentAlignment.MiddleCenter;
                    receiptPanel.Controls.Add(qtyLabel);

                    Label amountLabel = new Label();
                    amountLabel.Text = itemTotal.ToString("₱#,##0.00");
                    amountLabel.Font = new System.Drawing.Font("Arial", 9);
                    amountLabel.Size = new System.Drawing.Size(120, 18);
                    amountLabel.Location = new System.Drawing.Point(270, yPos);
                    amountLabel.TextAlign = ContentAlignment.MiddleRight;
                    receiptPanel.Controls.Add(amountLabel);

                    yPos += 20; // Increased spacing

                    // Unit price for multiple quantities
                    if (quantity > 1)
                    {
                        decimal displayUnitPrice = discountApplied ? unitPrice * 0.8m : unitPrice;
                        Label unitPriceLabel = CreateReceiptLabel($"  @ {displayUnitPrice.ToString("₱#,##0.00")} each", new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Italic), 400);
                        unitPriceLabel.Location = new System.Drawing.Point(20, yPos);
                        unitPriceLabel.ForeColor = System.Drawing.Color.Gray;
                        receiptPanel.Controls.Add(unitPriceLabel);
                        yPos += 16; // Increased spacing
                    }
                }

                yPos += 12; // Increased spacing before totals

                // Separator line
                Panel line3 = new Panel();
                line3.Size = new System.Drawing.Size(380, 1);
                line3.Location = new System.Drawing.Point(10, yPos);
                line3.BackColor = System.Drawing.Color.Black;
                receiptPanel.Controls.Add(line3);
                yPos += 18;

                // Totals section
                if (discountApplied)
                {
                    decimal originalTotal = 0;
                    foreach (DataRow row in cartData.Rows)
                    {
                        originalTotal += Convert.ToDecimal(row["subtotal"]);
                    }

                    // Subtotal
                    Label subtotalLabel = new Label();
                    subtotalLabel.Text = "Subtotal:";
                    subtotalLabel.Font = new System.Drawing.Font("Arial", 9);
                    subtotalLabel.Size = new System.Drawing.Size(220, 18);
                    subtotalLabel.Location = new System.Drawing.Point(10, yPos);
                    receiptPanel.Controls.Add(subtotalLabel);

                    Label subtotalAmountLabel = new Label();
                    subtotalAmountLabel.Text = originalTotal.ToString("₱#,##0.00");
                    subtotalAmountLabel.Font = new System.Drawing.Font("Arial", 9);
                    subtotalAmountLabel.Size = new System.Drawing.Size(150, 18);
                    subtotalAmountLabel.Location = new System.Drawing.Point(240, yPos);
                    subtotalAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                    receiptPanel.Controls.Add(subtotalAmountLabel);
                    yPos += 20;

                    // Discount
                    Label discountLabel = new Label();
                    discountLabel.Text = "Senior/PWD Discount (20%):";
                    discountLabel.Font = new System.Drawing.Font("Arial", 9);
                    discountLabel.Size = new System.Drawing.Size(220, 18);
                    discountLabel.Location = new System.Drawing.Point(10, yPos);
                    receiptPanel.Controls.Add(discountLabel);

                    Label discountAmountLabel = new Label();
                    discountAmountLabel.Text = (originalTotal * 0.2m).ToString("-₱#,##0.00");
                    discountAmountLabel.Font = new System.Drawing.Font("Arial", 9);
                    discountAmountLabel.Size = new System.Drawing.Size(150, 18);
                    discountAmountLabel.Location = new System.Drawing.Point(240, yPos);
                    discountAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                    discountAmountLabel.ForeColor = System.Drawing.Color.Red;
                    receiptPanel.Controls.Add(discountAmountLabel);
                    yPos += 20;

                    // Discount Information
                    if (!string.IsNullOrEmpty(discountName))
                    {
                        Label discountNameLabel = new Label();
                        discountNameLabel.Text = $"Discount Name: {discountName}";
                        discountNameLabel.Font = new System.Drawing.Font("Arial", 8);
                        discountNameLabel.Size = new System.Drawing.Size(320, 15);
                        discountNameLabel.Location = new System.Drawing.Point(10, yPos);
                        discountNameLabel.ForeColor = System.Drawing.Color.DarkBlue;
                        receiptPanel.Controls.Add(discountNameLabel);
                        yPos += 16;
                    }

                    if (!string.IsNullOrEmpty(discountId))
                    {
                        Label discountIdLabel = new Label();
                        discountIdLabel.Text = $"Discount ID: {discountId}";
                        discountIdLabel.Font = new System.Drawing.Font("Arial", 8);
                        discountIdLabel.Size = new System.Drawing.Size(320, 15);
                        discountIdLabel.Location = new System.Drawing.Point(10, yPos);
                        discountIdLabel.ForeColor = System.Drawing.Color.DarkBlue;
                        receiptPanel.Controls.Add(discountIdLabel);
                        yPos += 16;
                    }

                    // Line before total
                    Panel line4 = new Panel();
                    line4.Size = new System.Drawing.Size(380, 1);
                    line4.Location = new System.Drawing.Point(10, yPos);
                    line4.BackColor = System.Drawing.Color.Black;
                    receiptPanel.Controls.Add(line4);
                    yPos += 12;
                }

                // Total
                Label totalLabel = new Label();
                totalLabel.Text = "TOTAL AMOUNT:";
                totalLabel.Font = new System.Drawing.Font("Arial", 11, System.Drawing.FontStyle.Bold);
                totalLabel.Size = new System.Drawing.Size(220, 20);
                totalLabel.Location = new System.Drawing.Point(10, yPos);
                receiptPanel.Controls.Add(totalLabel);

                Label totalAmountLabel = new Label();
                totalAmountLabel.Text = finalTotal.ToString("₱#,##0.00");
                totalAmountLabel.Font = new System.Drawing.Font("Arial", 11, System.Drawing.FontStyle.Bold);
                totalAmountLabel.Size = new System.Drawing.Size(150, 20);
                totalAmountLabel.Location = new System.Drawing.Point(240, yPos);
                totalAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(totalAmountLabel);
                yPos += 28;

                // Payment
                Label paymentLabel = new Label();
                paymentLabel.Text = "Cash Payment:";
                paymentLabel.Font = new System.Drawing.Font("Arial", 9);
                paymentLabel.Size = new System.Drawing.Size(220, 18);
                paymentLabel.Location = new System.Drawing.Point(10, yPos);
                receiptPanel.Controls.Add(paymentLabel);

                Label paymentAmountLabel = new Label();
                paymentAmountLabel.Text = payment.ToString("₱#,##0.00");
                paymentAmountLabel.Font = new System.Drawing.Font("Arial", 9);
                paymentAmountLabel.Size = new System.Drawing.Size(150, 18);
                paymentAmountLabel.Location = new System.Drawing.Point(240, yPos);
                paymentAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(paymentAmountLabel);
                yPos += 20;

                // Change
                Label changeLabel = new Label();
                changeLabel.Text = "Change:";
                changeLabel.Font = new System.Drawing.Font("Arial", 9);
                changeLabel.Size = new System.Drawing.Size(220, 18);
                changeLabel.Location = new System.Drawing.Point(10, yPos);
                receiptPanel.Controls.Add(changeLabel);

                Label changeAmountLabel = new Label();
                changeAmountLabel.Text = change.ToString("₱#,##0.00");
                changeAmountLabel.Font = new System.Drawing.Font("Arial", 9);
                changeAmountLabel.Size = new System.Drawing.Size(150, 18);
                changeAmountLabel.Location = new System.Drawing.Point(240, yPos);
                changeAmountLabel.TextAlign = ContentAlignment.MiddleRight;
                receiptPanel.Controls.Add(changeAmountLabel);
                yPos += 32;

                // Footer separator
                Panel line5 = new Panel();
                line5.Size = new System.Drawing.Size(380, 2);
                line5.Location = new System.Drawing.Point(10, yPos);
                line5.BackColor = System.Drawing.Color.Black;
                receiptPanel.Controls.Add(line5);
                yPos += 18;

                // Thank you message
                Label thankYouLabel = CreateReceiptLabel("THANK YOU!", new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold), 400);
                thankYouLabel.Location = new System.Drawing.Point(0, yPos);
                thankYouLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(thankYouLabel);
                yPos += 28;

                Label greatDayLabel = CreateReceiptLabel("Have a great day ahead!", new System.Drawing.Font("Arial", 10), 400);
                greatDayLabel.Location = new System.Drawing.Point(0, yPos);
                greatDayLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(greatDayLabel);
                yPos += 28;

                // Contact info
                Label contactLabel = CreateReceiptLabel("For concerns, please contact us at:", new System.Drawing.Font("Arial", 8), 400);
                contactLabel.Location = new System.Drawing.Point(0, yPos);
                contactLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(contactLabel);
                yPos += 18;

                Label phoneLabel = CreateReceiptLabel("Phone: (02) 123-4567", new System.Drawing.Font("Arial", 8), 400);
                phoneLabel.Location = new System.Drawing.Point(0, yPos);
                phoneLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(phoneLabel);
                yPos += 14;

                Label emailLabel = CreateReceiptLabel("Email: info@amgcpharmacy.com", new System.Drawing.Font("Arial", 8), 400);
                emailLabel.Location = new System.Drawing.Point(0, yPos);
                emailLabel.TextAlign = ContentAlignment.MiddleCenter;
                receiptPanel.Controls.Add(emailLabel);

                // Add the receipt panel to the scrollable panel
                scrollablePanel.Controls.Add(receiptPanel);

                // Add the scrollable panel to the form
                receiptForm.Controls.Add(scrollablePanel);

                // Buttons - positioned at the bottom of the form
                Button printButton = new Button();
                printButton.Text = "Print";
                printButton.Size = new System.Drawing.Size(80, 30);
                printButton.Location = new System.Drawing.Point(80, 620); // Adjusted position
                printButton.Click += (s, ev) => PrintReceipt();
                receiptForm.Controls.Add(printButton);

                Button saveButton = new Button();
                saveButton.Text = "Save as Image";
                saveButton.Size = new System.Drawing.Size(100, 30);
                saveButton.Location = new System.Drawing.Point(170, 620); // Adjusted position
                saveButton.Click += (s, ev) => SaveReceiptAsImage(receiptPanel, orderID);
                receiptForm.Controls.Add(saveButton);

                Button closeButton = new Button();
                closeButton.Text = "Close";
                closeButton.Size = new System.Drawing.Size(80, 30);
                closeButton.Location = new System.Drawing.Point(280, 620); // Adjusted position
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
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing receipt: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Label CreateReceiptLabel(string text, System.Drawing.Font font, int width)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = font;
            label.Size = new System.Drawing.Size(width, (int)(font.Size * 1.5));
            label.AutoSize = false;
            return label;
        }

        private void SaveReceiptAsImage(Panel receiptPanel, string orderID)
        {
            try
            {
                // Create a bitmap with the size of the panel
                Bitmap bitmap = new Bitmap(receiptPanel.Width, receiptPanel.Height);
                receiptPanel.DrawToBitmap(bitmap, new Rectangle(0, 0, receiptPanel.Width, receiptPanel.Height));

                // Show save file dialog
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

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentProductPage = 1;
            LoadProducts(txtsearch.Text.Trim());
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductRow < 0 || selectedProductRow >= dgvproducts.Rows.Count)
                {
                    MessageBox.Show("Please select a product first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int quantity = (int)numquantity.Value;

                DataGridViewRow selectedRow = dgvproducts.Rows[selectedProductRow];
                string productName = selectedRow.Cells["products"].Value?.ToString() ?? "";
                decimal unitPrice = 0;
                int currentStock = 0;

                // Parse unit price
                string unitPriceStr = selectedRow.Cells["unitprice"].Value?.ToString() ?? "0";
                string cleanPrice = Regex.Replace(unitPriceStr, @"[^\d\.\-]", "");
                if (!decimal.TryParse(cleanPrice, NumberStyles.Number | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out unitPrice))
                {
                    MessageBox.Show("Invalid unit price format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse current stock
                string stockStr = selectedRow.Cells["currentstock"].Value?.ToString() ?? "0";
                string cleanStock = Regex.Replace(stockStr, @"[^\d]", "");
                if (!int.TryParse(cleanStock, out currentStock))
                {
                    MessageBox.Show("Invalid stock format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if enough stock
                int totalQuantityInCart = GetProductQuantityInCart(productName);
                int totalRequestedQuantity = totalQuantityInCart + quantity;

                if (totalRequestedQuantity > currentStock)
                {
                    if (totalQuantityInCart > 0)
                    {
                        MessageBox.Show($"Low Stock Warning!\nProduct: {productName}\nAvailable Stock: {currentStock}\nCurrent in Cart: {totalQuantityInCart}\nRequested Additional: {quantity}\nInsufficient stock for this quantity.",
                            "Low Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show($"Low Stock Warning!\nProduct: {productName}\nAvailable Stock: {currentStock}\nRequested Quantity: {quantity}\nInsufficient stock.",
                            "Low Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return;
                }

                // Check if product already exists in cart
                bool productExists = false;
                foreach (DataRow row in cartData.Rows)
                {
                    if (row["products"].ToString() == productName)
                    {
                        int existingQty = Convert.ToInt32(row["quantity"]);
                        int newQty = existingQty + quantity;

                        row["quantity"] = newQty;
                        row["subtotal"] = unitPrice * newQty;
                        productExists = true;
                        break;
                    }
                }

                // Add new product to cart if it doesn't exist
                if (!productExists)
                {
                    DataRow newRow = cartData.NewRow();
                    newRow["products"] = productName;
                    newRow["unitprice"] = unitPrice;
                    newRow["quantity"] = quantity;
                    newRow["subtotal"] = unitPrice * quantity;
                    cartData.Rows.Add(newRow);
                }

                // Reset quantity to 1 and refresh cart
                numquantity.Value = 1;
                LoadCart();
                UpdateTotal();

                MessageBox.Show("Product added to cart.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on btnadd_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedCartRow < 0 || selectedCartRow >= dgvcart.Rows.Count)
                {
                    MessageBox.Show("Please select an item from cart first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dgvcart.Rows[selectedCartRow];
                string productName = selectedRow.Cells["products"].Value?.ToString() ?? "";

                DialogResult result = MessageBox.Show($"Remove {productName} from cart?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    for (int i = cartData.Rows.Count - 1; i >= 0; i--)
                    {
                        if (cartData.Rows[i]["products"].ToString() == productName)
                        {
                            cartData.Rows.RemoveAt(i);
                            break;
                        }
                    }

                    LoadCart();
                    UpdateTotal();

                    if (cartData.Rows.Count == 0)
                    {
                        discountApplied = false; // reset if empty
                        discountName = "";
                        discountId = "";
                        txtpayment.Text = "";
                        txtchange.Text = "";
                    }

                    MessageBox.Show("Item removed from cart.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (cartData.Rows.Count == 0)
                {
                    MessageBox.Show("Cart is already empty.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show("Clear all items from cart?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    cartData.Clear();
                    discountApplied = false; // reset
                    discountName = "";
                    discountId = "";
                    txtpayment.Text = "";
                    txtchange.Text = "";
                    LoadCart();
                    UpdateTotal();
                    MessageBox.Show("Cart cleared.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on btndeleteall_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnprodNext_Click(object sender, EventArgs e)
        {
            if (currentProductPage < totalProductPages)
            {
                currentProductPage++;
                LoadProducts(txtsearch.Text.Trim());
            }
        }

        private void btnprodPrev_Click(object sender, EventArgs e)
        {
            if (currentProductPage > 1)
            {
                currentProductPage--;
                LoadProducts(txtsearch.Text.Trim());
            }
        }

        private void btncartNext_Click(object sender, EventArgs e)
        {
            if (currentCartPage < totalCartPages)
            {
                currentCartPage++;
                LoadCart();
            }
        }

        private void btncartPrev_Click(object sender, EventArgs e)
        {
            if (currentCartPage > 1)
            {
                currentCartPage--;
                LoadCart();
            }
        }

        private void dgvproducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvproducts.Rows.Count)
                {
                    selectedProductRow = e.RowIndex;
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on dgvproducts_CellClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvcart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvcart.Rows.Count)
                {
                    selectedCartRow = e.RowIndex;
                    UpdateButtonStates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on dgvcart_CellClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numquantity_ValueChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnsearch_Click(sender, e);
            }
        }

        private void txtpayment_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, decimal point, and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if (e.KeyChar == '.' && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtpayment_TextChanged(object sender, EventArgs e)
        {
            CalculateChange();
            UpdateButtonStates();
        }

        private int GetProductQuantityInCart(string productName)
        {
            foreach (DataRow row in cartData.Rows)
            {
                if (row["products"].ToString() == productName)
                {
                    return Convert.ToInt32(row["quantity"]);
                }
            }
            return 0;
        }

        private void btndiscount_Click(object sender, EventArgs e)
        {
            try
            {
                if (cartData.Rows.Count == 0)
                {
                    MessageBox.Show("Cart is empty. Add products before applying discount.",
                        "No Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (discountApplied)
                {
                    MessageBox.Show("Discount already applied.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Show discount form
                frmDiscount discountForm = new frmDiscount();
                if (discountForm.ShowDialog() == DialogResult.OK)
                {
                    discountName = discountForm.DiscountName;
                    discountId = discountForm.DiscountId;

                    if (string.IsNullOrEmpty(discountName) || string.IsNullOrEmpty(discountId))
                    {
                        MessageBox.Show("Both name and discount ID are required.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    decimal total = 0;
                    foreach (DataRow row in cartData.Rows)
                    {
                        if (row["subtotal"] != null && row["subtotal"] != DBNull.Value)
                        {
                            total += Convert.ToDecimal(row["subtotal"]);
                        }
                    }

                    DialogResult result = MessageBox.Show(
                        $"Apply 20% discount for:\nName: {discountName}\nID: {discountId}\n\nOriginal Total: {total:₱#,##0.00}\nDiscounted Total: {(total * 0.8m):₱#,##0.00}",
                        "Discount Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        discountApplied = true;
                        UpdateTotal();
                        MessageBox.Show("Discount applied successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        discountName = "";
                        discountId = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on btndiscount_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnpurchase_Click(object sender, EventArgs e)
        {
            try
            {
                if (cartData.Rows.Count == 0)
                {
                    MessageBox.Show("Cart is empty. Add products before purchasing.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal paymentAmount = GetPaymentAmount();
                if (paymentAmount <= 0)
                {
                    MessageBox.Show("Please enter a valid payment amount.", "Invalid Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Calculate grand total (sum of subtotals), then apply discount if active for display/confirmation
                decimal grandTotal = 0;
                foreach (DataRow row in cartData.Rows)
                {
                    grandTotal += Convert.ToDecimal(row["subtotal"]);
                }

                decimal finalGrandTotal = discountApplied ? grandTotal * 0.8m : grandTotal;

                if (paymentAmount < finalGrandTotal)
                {
                    MessageBox.Show($"Insufficient payment!\nTotal: {finalGrandTotal:₱#,##0.00}\nPayment: {paymentAmount:₱#,##0.00}\nShortage: {(finalGrandTotal - paymentAmount):₱#,##0.00}",
                        "Insufficient Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal change = paymentAmount - finalGrandTotal;

                string discountInfo = discountApplied ?
                    $"\nDiscount Applied: Yes\nName: {discountName}\nID: {discountId}" :
                    "\nDiscount Applied: No";

                DialogResult result = MessageBox.Show($"Confirm purchase?\n\nTotal Amount: {finalGrandTotal:₱#,##0.00}\nPayment: {paymentAmount:₱#,##0.00}\nChange: {change:₱#,##0.00}{discountInfo}",
                    "Purchase Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
                    string currentTime = DateTime.Now.ToString("hh:mm:ss tt");
                    string discountedFlag = discountApplied ? "Yes" : "No";
                    string orderID = GenerateOrderID();

                    // Insert each cart item with orderID
                    foreach (DataRow row in cartData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal subtotal = Convert.ToDecimal(row["subtotal"]);
                        decimal savedSubtotal = discountApplied ? Math.Round(subtotal * 0.8m, 2) : subtotal;

                        // Insert into tblsales with orderid and payment and change information
                        string insertSale = "INSERT INTO tblsales (orderid, products, quantity, payment, paymentchange, totalcost, discounted, discountedname, discountedid, datecreated, timecreated, createdby) " +
                            "VALUES ('" + orderID.Replace("'", "''") + "', '" + productName.Replace("'", "''") + "', '" + quantity + "', '" + paymentAmount.ToString("F2") + "', '" +
                            change.ToString("F2") + "', '" + savedSubtotal.ToString("F2") + "', '" +
                            discountedFlag + "', '" + discountName.Replace("'", "''") + "', '" + discountId.Replace("'", "''") + "', '" + currentDate + "', '" + currentTime + "', '" + username.Replace("'", "''") + "')";
                        pos.executeSQL(insertSale);

                        // Update product stock
                        string updateStock = "UPDATE tblproducts SET currentstock = CAST(currentstock AS UNSIGNED) - " + quantity +
                            " WHERE products = '" + productName.Replace("'", "''") + "'";
                        pos.executeSQL(updateStock);
                    }

                    // Log the transaction; include payment and change info with orderID
                    pos.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                        "VALUES ('" + currentDate + "', '" + currentTime + "', 'PURCHASE', 'POS', 'ORDER ID: " + orderID +
                        " | TOTAL: " + finalGrandTotal.ToString("F2") + " | PAYMENT: " + paymentAmount.ToString("F2") + " | CHANGE: " + change.ToString("F2") +
                        " | DISCOUNTED: " + (discountApplied ? "Yes" : "No") + " | NAME: " + discountName + " | ID: " + discountId + "', '" + username.Replace("'", "''") + "')");

                    // Show receipt
                    ShowReceipt(orderID, finalGrandTotal, paymentAmount, change);

                    // Clear cart, reset discount, and clear payment fields
                    cartData.Clear();
                    discountApplied = false;
                    discountName = "";
                    discountId = "";
                    txtpayment.Text = "";
                    txtchange.Text = "";
                    LoadProducts(); // Refresh to show updated stock
                    LoadCart();
                    UpdateTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on btnpurchase_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Discount Form inside the same file
    public partial class frmDiscount : Form
    {
        public string DiscountName { get; private set; }
        public string DiscountId { get; private set; }

        private ErrorProvider errorProvider;

        public frmDiscount()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Form Properties
            this.Text = "Senior/PWD Discount";
            this.Size = new System.Drawing.Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);

            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // Main Panel
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = System.Drawing.Color.White;
            mainPanel.BorderStyle = BorderStyle.FixedSingle;

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 50;
            headerPanel.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            headerPanel.Padding = new Padding(20, 0, 0, 0);

            Label lblHeader = new Label();
            lblHeader.Text = "SENIOR/PWD DISCOUNT";
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, FontStyle.Bold);
            lblHeader.ForeColor = System.Drawing.Color.White;
            lblHeader.Dock = DockStyle.Fill;
            lblHeader.TextAlign = ContentAlignment.MiddleLeft;
            headerPanel.Controls.Add(lblHeader);

            // Content Panel
            Panel contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(30);

            // Title Label
            Label titleLabel = new Label();
            titleLabel.Text = "Please enter discount information";
            titleLabel.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            titleLabel.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            titleLabel.Size = new System.Drawing.Size(300, 25);
            titleLabel.Location = new System.Drawing.Point(30, 20);
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            contentPanel.Controls.Add(titleLabel);

            // Name Label
            Label nameLabel = new Label();
            nameLabel.Text = "Name:";
            nameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            nameLabel.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            nameLabel.Size = new System.Drawing.Size(80, 20);
            nameLabel.Location = new System.Drawing.Point(30, 70);
            contentPanel.Controls.Add(nameLabel);

            // Name TextBox
            TextBox txtName = new TextBox();
            txtName.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtName.Size = new System.Drawing.Size(280, 25);
            txtName.Location = new System.Drawing.Point(30, 95);
            txtName.MaxLength = 50;
            txtName.BorderStyle = BorderStyle.FixedSingle;
            contentPanel.Controls.Add(txtName);

            // ID Label
            Label idLabel = new Label();
            idLabel.Text = "Discount ID:";
            idLabel.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            idLabel.ForeColor = System.Drawing.Color.FromArgb(52, 73, 94);
            idLabel.Size = new System.Drawing.Size(80, 20);
            idLabel.Location = new System.Drawing.Point(30, 140);
            contentPanel.Controls.Add(idLabel);

            // ID TextBox
            TextBox txtId = new TextBox();
            txtId.Font = new System.Drawing.Font("Segoe UI", 9F);
            txtId.Size = new System.Drawing.Size(280, 25);
            txtId.Location = new System.Drawing.Point(30, 165);
            txtId.MaxLength = 50;
            txtId.BorderStyle = BorderStyle.FixedSingle;
            contentPanel.Controls.Add(txtId);

            // Button Panel
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 60;
            buttonPanel.Padding = new Padding(80, 10, 80, 10);
            buttonPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            // Confirm Button
            Button btnConfirm = new Button();
            btnConfirm.Text = "Confirm";
            btnConfirm.Size = new System.Drawing.Size(100, 35);
            btnConfirm.Location = new System.Drawing.Point(80, 10);
            btnConfirm.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            btnConfirm.ForeColor = System.Drawing.Color.White;
            btnConfirm.Cursor = Cursors.Hand;
            btnConfirm.Click += (s, e) => ConfirmDiscount(txtName, txtId);
            buttonPanel.Controls.Add(btnConfirm);

            // Cancel Button
            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new System.Drawing.Size(100, 35);
            btnCancel.Location = new System.Drawing.Point(190, 10);
            btnCancel.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.ForeColor = System.Drawing.Color.White;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };
            buttonPanel.Controls.Add(btnCancel);

            // Add panels to main container
            mainPanel.Controls.Add(contentPanel);
            mainPanel.Controls.Add(buttonPanel);
            mainPanel.Controls.Add(headerPanel);

            // Set form controls
            this.Controls.Add(mainPanel);
            this.AcceptButton = btnConfirm;
            this.CancelButton = btnCancel;
        }

        private void ConfirmDiscount(TextBox txtName, TextBox txtId)
        {
            errorProvider.Clear();

            bool isValid = true;

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                errorProvider.SetError(txtName, "Name is required");
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtId.Text.Trim()))
            {
                errorProvider.SetError(txtId, "Discount ID is required");
                isValid = false;
            }

            if (isValid)
            {
                DiscountName = txtName.Text.Trim();
                DiscountId = txtId.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Set focus to name field when form loads
            foreach (Control control in this.Controls)
            {
                if (control is Panel mainPanel)
                {
                    foreach (Control subControl in mainPanel.Controls)
                    {
                        if (subControl is Panel contentPanel)
                        {
                            foreach (Control field in contentPanel.Controls)
                            {
                                if (field is TextBox textBox && textBox.Name == "txtName")
                                {
                                    textBox.Focus();
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}