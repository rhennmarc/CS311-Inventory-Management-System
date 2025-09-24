using inventory_management;
using System;
using System.Data;
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
            txtquantity.KeyPress += txtquantity_KeyPress;
            txtquantity.TextChanged += txtquantity_TextChanged;
            txtsearch.KeyPress += txtsearch_KeyPress;
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
            bool hasValidQuantity = !string.IsNullOrWhiteSpace(txtquantity.Text) &&
                                  int.TryParse(txtquantity.Text, out int qty) && qty > 0;

            btnadd.Enabled = hasProductSelection && hasValidQuantity;
            btndelete.Enabled = hasCartSelection;
            btndeleteall.Enabled = hasCartItems;
            btnpurchase.Enabled = hasCartItems;
            btndiscount.Enabled = hasCartItems; // Discount button enabled only if cart has items

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
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR on UpdateTotal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txttotal.Text = "₱0.00";
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

                if (string.IsNullOrWhiteSpace(txtquantity.Text))
                {
                    MessageBox.Show("Please enter quantity.", "Missing Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtquantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter a valid quantity (positive number).", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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

                // Clear quantity textbox and refresh cart
                txtquantity.Text = "";
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

        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow Enter key to trigger Add first
            if (e.KeyChar == (char)13)
            {
                e.Handled = true; // Prevent the beep sound
                if (!string.IsNullOrWhiteSpace(txtquantity.Text) &&
                    int.TryParse(txtquantity.Text, out int qty) && qty > 0)
                {
                    btnadd_Click(sender, e);
                }
                return;
            }

            // Allow only numbers and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnsearch_Click(sender, e);
            }
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

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
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

                decimal total = 0;
                foreach (DataRow row in cartData.Rows)
                {
                    if (row["subtotal"] != null && row["subtotal"] != DBNull.Value)
                    {
                        total += Convert.ToDecimal(row["subtotal"]);
                    }
                }

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to give a 20% discount?\n\nOriginal Total: {total:₱#,##0.00}\nDiscounted Total: {(total * 0.8m):₱#,##0.00}",
                    "Discount Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    discountApplied = true;
                    UpdateTotal();
                    MessageBox.Show("Discount applied successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // Calculate grand total (sum of subtotals), then apply discount if active for display/confirmation
                decimal grandTotal = 0;
                foreach (DataRow row in cartData.Rows)
                {
                    grandTotal += Convert.ToDecimal(row["subtotal"]);
                }

                decimal finalGrandTotal = discountApplied ? grandTotal * 0.8m : grandTotal;

                DialogResult result = MessageBox.Show($"Confirm purchase?\nTotal Amount: {finalGrandTotal:₱#,##0.00}",
                    "Purchase Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
                    string currentTime = DateTime.Now.ToString("HH:mm:ss");
                    string discountedFlag = discountApplied ? "Yes" : "No";

                    // Insert each cart item. If discounted, store the discounted subtotal for that item.
                    foreach (DataRow row in cartData.Rows)
                    {
                        string productName = row["products"].ToString();
                        int quantity = Convert.ToInt32(row["quantity"]);
                        decimal subtotal = Convert.ToDecimal(row["subtotal"]);
                        decimal savedSubtotal = discountApplied ? Math.Round(subtotal * 0.8m, 2) : subtotal;

                        // Insert into tblsales with discounted flag and the (possibly discounted) totalcost
                        string insertSale = "INSERT INTO tblsales (products, quantity, totalcost, discounted, datecreated, timecreated, createdby) " +
                            "VALUES ('" + productName.Replace("'", "''") + "', '" + quantity + "', '" + savedSubtotal.ToString("F2") + "', '" +
                            discountedFlag + "', '" + currentDate + "', '" + currentTime + "', '" + username.Replace("'", "''") + "')";
                        pos.executeSQL(insertSale);

                        // Update product stock
                        string updateStock = "UPDATE tblproducts SET currentstock = CAST(currentstock AS UNSIGNED) - " + quantity +
                            " WHERE products = '" + productName.Replace("'", "''") + "'";
                        pos.executeSQL(updateStock);
                    }

                    // Log the transaction; include discounted flag
                    pos.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                        "VALUES ('" + currentDate + "', '" + currentTime + "', 'PURCHASE', 'POS', 'TOTAL: " +
                        finalGrandTotal.ToString("F2") + " | DISCOUNTED: " + (discountApplied ? "Yes" : "No") + "', '" + username.Replace("'", "''") + "')");

                    MessageBox.Show($"Purchase completed successfully!\nTotal: {finalGrandTotal:₱#,##0.00}",
                        "Purchase Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear cart and reset discount
                    cartData.Clear();
                    discountApplied = false;
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
}
