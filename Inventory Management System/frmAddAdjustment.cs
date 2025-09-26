using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using inventory_management;

namespace Inventory_Management_System
{
    public partial class frmAddAdjustment : Form
    {
        private string username;

        public frmAddAdjustment(string username)
        {
            InitializeComponent();
            this.username = username;
            InitializeActionComboBox();
        }

        private void InitializeActionComboBox()
        {
            // Initialize action combobox
            if (cmbaction != null)
            {
                cmbaction.Items.Clear();
                cmbaction.Items.AddRange(new string[] { "Add", "Remove" });
                cmbaction.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbaction.SelectedIndex = 0; // Default to "Add"
            }
        }

        Class1 newAdjustment = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");
        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // --- VALIDATIONS ---

            // Product (required) - only check if text is empty, not SelectedIndex
            if (string.IsNullOrEmpty(cmbproduct.Text.Trim()))
            {
                errorProvider1.SetError(cmbproduct, "Product name is required.");
                errorcount++;
            }

            // At least one of quantity or price must be provided
            bool hasQuantity = !string.IsNullOrEmpty(txtquantity.Text.Trim());
            bool hasPrice = !string.IsNullOrEmpty(txtprice.Text.Trim());

            if (!hasQuantity && !hasPrice)
            {
                errorProvider1.SetError(txtquantity, "Either quantity or price must be provided.");
                errorProvider1.SetError(txtprice, "Either quantity or price must be provided.");
                errorcount++;
            }

            // Action (required only if quantity is provided)
            if (hasQuantity && (cmbaction.SelectedIndex == -1 || string.IsNullOrEmpty(cmbaction.Text)))
            {
                errorProvider1.SetError(cmbaction, "Action is required when adjusting quantity. Select Add or Remove.");
                errorcount++;
            }

            // Quantity validation (if provided)
            if (hasQuantity)
            {
                int qty;
                if (!int.TryParse(txtquantity.Text.Trim(), out qty) || qty <= 0)
                {
                    errorProvider1.SetError(txtquantity, "Quantity must be numeric and greater than 0.");
                    errorcount++;
                }
            }

            // Price validation (if provided)
            if (hasPrice)
            {
                decimal price;
                if (!decimal.TryParse(txtprice.Text.Trim(), out price) || price <= 0)
                {
                    errorProvider1.SetError(txtprice, "Price must be numeric and greater than 0.");
                    errorcount++;
                }
            }

            // Reason (required)
            if (string.IsNullOrEmpty(txtreason.Text.Trim()))
            {
                errorProvider1.SetError(txtreason, "Reason is required.");
                errorcount++;
            }

            // --- SAVE TO DATABASE ---
            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to add this adjustment?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        string product = cmbproduct.Text.Trim().Replace("'", "''");
                        string action = hasQuantity ? cmbaction.Text.Trim().Replace("'", "''") : "PRICE UPDATE";
                        string quantity = hasQuantity ? txtquantity.Text.Trim().Replace("'", "''") : "";
                        string unitprice = hasPrice ? txtprice.Text.Trim().Replace("'", "''") : "";
                        string reason = txtreason.Text.Trim().Replace("'", "''");
                        string createdBy = username.Replace("'", "''");
                        string dateAdjusted = DateTime.Now.ToString("MM/dd/yyyy");
                        string timeAdjusted = DateTime.Now.ToString("HH:mm:ss"); // Add time

                        // Check if product exists in tblproducts
                        string checkProductQuery = "SELECT currentstock, unitprice FROM tblproducts WHERE LOWER(products) = LOWER('" + product + "')";
                        DataTable dtProduct = newAdjustment.GetData(checkProductQuery);

                        if (dtProduct.Rows.Count == 0)
                        {
                            MessageBox.Show("Product '" + cmbproduct.Text + "' does not exist in the products table. Please add the product first.",
                                "Product Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Get current stock and price
                        int currentStock = 0;
                        decimal currentPrice = 0;
                        string currentStockStr = dtProduct.Rows[0]["currentstock"].ToString();
                        string currentPriceStr = dtProduct.Rows[0]["unitprice"].ToString();
                        int.TryParse(currentStockStr, out currentStock);
                        decimal.TryParse(currentPriceStr, out currentPrice);

                        int newStock = currentStock;
                        decimal newPrice = currentPrice;
                        string adjustmentSign = "";
                        int adjustmentQty = 0;

                        // Calculate new stock if quantity is provided
                        if (hasQuantity)
                        {
                            adjustmentQty = int.Parse(quantity);

                            if (action.ToUpper() == "ADD")
                            {
                                newStock = currentStock + adjustmentQty;
                                adjustmentSign = "+";
                            }
                            else if (action.ToUpper() == "REMOVE")
                            {
                                newStock = currentStock - adjustmentQty;
                                adjustmentSign = "-";

                                // Check if removal would result in negative stock
                                if (newStock < 0)
                                {
                                    MessageBox.Show($"Cannot remove {adjustmentQty} items. Current stock is only {currentStock}. This would result in negative stock.",
                                        "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        // Set new price if provided
                        if (hasPrice)
                        {
                            newPrice = decimal.Parse(unitprice);
                        }

                        // Prepare quantity for record (signed if it's a stock adjustment) - use NULL if empty
                        string recordQuantity = hasQuantity ?
                            (action.ToUpper() == "ADD" ? quantity : ("-" + quantity)) :
                            "";

                        // Prepare price for record - use NULL if not provided
                        string recordPrice = hasPrice ? unitprice : "NULL";

                        // Prepare quantity for SQL - use NULL if empty
                        string sqlQuantity = hasQuantity ?
                            "'" + (action.ToUpper() == "ADD" ? quantity : ("-" + quantity)) + "'" :
                            "NULL";

                        // Insert adjustment record with timeadjusted
                        string insertAdjustment =
                            "INSERT INTO tbladjustment (products, quantity, unitprice, reason, createdby, dateadjusted, timeadjusted) " +
                            "VALUES ('" + product + "', " + sqlQuantity + ", " + recordPrice + ", '" + reason + "', '" + createdBy + "', '" + dateAdjusted + "', '" + timeAdjusted + "')";

                        newAdjustment.executeSQL(insertAdjustment);

                        if (newAdjustment.rowAffected > 0)
                        {
                            // Build dynamic UPDATE query - only update fields that have values
                            string updateProductQuery = "UPDATE tblproducts SET ";
                            List<string> updateFields = new List<string>();

                            if (hasQuantity)
                            {
                                updateFields.Add("currentstock = '" + newStock + "'");
                            }
                            if (hasPrice)
                            {
                                updateFields.Add("unitprice = '" + newPrice.ToString("F2") + "'");
                            }

                            if (updateFields.Count > 0)
                            {
                                updateProductQuery += string.Join(", ", updateFields);
                                updateProductQuery += " WHERE LOWER(products) = LOWER('" + product + "')";

                                newAdjustment.executeSQL(updateProductQuery);

                                if (newAdjustment.rowAffected > 0)
                                {
                                    string message = "Adjustment completed successfully!\n\nProduct: " + cmbproduct.Text;

                                    if (hasQuantity)
                                    {
                                        message += $"\nAction: {action}\nQuantity: {adjustmentQty}\nPrevious Stock: {currentStock}\nNew Stock: {newStock}";
                                    }
                                    if (hasPrice)
                                    {
                                        message += $"\nPrice Updated: ₱{currentPrice:N2} → ₱{newPrice:N2}";
                                    }

                                    MessageBox.Show(message, "Adjustment Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Log the action
                                    string logAction = hasQuantity ? action.ToUpper() + " ADJUSTMENT" : "PRICE UPDATE";
                                    string logDetails = "Product: " + product;

                                    if (hasQuantity)
                                    {
                                        logDetails += ", Qty: " + adjustmentSign + quantity + ", Stock: " + currentStock + "→" + newStock;
                                    }
                                    if (hasPrice)
                                    {
                                        logDetails += ", Price: " + currentPrice.ToString("F2") + "→" + newPrice.ToString("F2");
                                    }

                                    newAdjustment.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                                        "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                                        "', '" + logAction + "', 'ADJUSTMENT MANAGEMENT', '" + logDetails + "', '" + username + "')");

                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Adjustment saved but failed to update product. Please check manually.",
                                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Adjustment record saved but no fields were updated.",
                                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on adding new adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- LOAD COMBOBOX ---
        private void frmAddAdjustment_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = newAdjustment.GetData("SELECT products FROM tblproducts ORDER BY products ASC");
                cmbproduct.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cmbproduct.Items.Add(row["products"].ToString());
                }

                cmbproduct.DropDownStyle = ComboBoxStyle.DropDown;
                cmbproduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbproduct.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbproduct.Focus();

                // Initialize quantity as empty
                txtquantity.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- AUTO-POPULATE CURRENT PRICE WHEN PRODUCT IS SELECTED ---
        private void cmbproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbproduct.Text.Trim()))
                {
                    string selectedProduct = cmbproduct.Text.Trim();
                    if (!string.IsNullOrEmpty(selectedProduct))
                    {
                        // Get current product details
                        string query = "SELECT unitprice, currentstock FROM tblproducts WHERE LOWER(products) = LOWER('" + selectedProduct.Replace("'", "''") + "')";
                        DataTable dt = newAdjustment.GetData(query);

                        if (dt.Rows.Count > 0)
                        {
                            // Only auto-populate if price field is empty
                            if (string.IsNullOrEmpty(txtprice.Text.Trim()))
                            {
                                string currentPrice = dt.Rows[0]["unitprice"].ToString();
                                txtprice.Text = currentPrice;
                            }

                            // Show current stock in a label or tooltip (optional)
                            string currentStock = dt.Rows[0]["currentstock"].ToString();
                            // You could add a label to show current stock: lblCurrentStock.Text = $"Current Stock: {currentStock}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Don't show error for auto-populate issues
                System.Diagnostics.Debug.WriteLine("Auto-populate error: " + ex.Message);
            }
        }

        // --- Remove error when user starts typing/selecting ---
        private void cmbproduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbproduct.Text))
                errorProvider1.SetError(cmbproduct, "");
        }

        private void cmbaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear error when action is selected
            if (cmbaction.SelectedIndex != -1)
                errorProvider1.SetError(cmbaction, "");

            // Update the preview when action changes (only if quantity is provided)
            if (!string.IsNullOrEmpty(txtquantity.Text.Trim()))
                UpdateStockPreview();
        }

        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");

            // Only allow digits and backspace (no negative sign since we use action dropdown)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtprice.Text))
                errorProvider1.SetError(txtprice, "");

            // Allow digits, decimal point, and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && txtprice.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtprice_Leave(object sender, EventArgs e)
        {
            // Format price when user leaves the field only if there's a value
            if (!string.IsNullOrEmpty(txtprice.Text))
            {
                if (decimal.TryParse(txtprice.Text, out decimal price))
                {
                    txtprice.Text = price.ToString("F2"); // Format to 2 decimal places
                }
            }
        }

        private void txtreason_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtreason.Text))
                errorProvider1.SetError(txtreason, "");
        }

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            // Clear quantity error when user types
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");

            // Update preview when quantity changes (only if product is selected)
            if (!string.IsNullOrEmpty(cmbproduct.Text) && cmbaction.SelectedIndex != -1)
                UpdateStockPreview();

            // Enable/disable action combobox based on whether quantity is provided
            cmbaction.Enabled = !string.IsNullOrEmpty(txtquantity.Text.Trim());

            // If quantity is cleared, clear the action selection
            if (string.IsNullOrEmpty(txtquantity.Text.Trim()))
            {
                cmbaction.SelectedIndex = -1;
            }
        }

        private void txtprice_TextChanged(object sender, EventArgs e)
        {
            // Clear price error when user types
            if (!string.IsNullOrEmpty(txtprice.Text))
                errorProvider1.SetError(txtprice, "");
        }

        private void UpdateStockPreview()
        {
            // Show preview of what the adjustment will do (only if quantity is provided)
            try
            {
                if (!string.IsNullOrEmpty(cmbproduct.Text) && !string.IsNullOrEmpty(txtquantity.Text) && cmbaction.SelectedIndex != -1)
                {
                    string selectedProduct = cmbproduct.Text.Trim();
                    string selectedAction = cmbaction.Text;

                    if (int.TryParse(txtquantity.Text, out int adjustmentQty) && adjustmentQty > 0)
                    {
                        // Get current stock
                        string query = "SELECT currentstock FROM tblproducts WHERE LOWER(products) = LOWER('" + selectedProduct.Replace("'", "''") + "')";
                        DataTable dt = newAdjustment.GetData(query);

                        if (dt.Rows.Count > 0)
                        {
                            int currentStock = 0;
                            int.TryParse(dt.Rows[0]["currentstock"].ToString(), out currentStock);

                            int newStock;
                            string previewText;

                            if (selectedAction.ToUpper() == "ADD")
                            {
                                newStock = currentStock + adjustmentQty;
                                previewText = $"Preview: {currentStock} + {adjustmentQty} = {newStock}";
                            }
                            else // REMOVE
                            {
                                newStock = currentStock - adjustmentQty;
                                if (newStock < 0)
                                {
                                    previewText = $"Warning: {currentStock} - {adjustmentQty} = {newStock} (Insufficient stock!)";
                                }
                                else
                                {
                                    previewText = $"Preview: {currentStock} - {adjustmentQty} = {newStock}";
                                }
                            }

                            // You could add a label to show the preview: lblPreview.Text = previewText;
                            // For now, we'll just store it for potential use
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ignore errors in preview calculation
            }
        }
    }
}