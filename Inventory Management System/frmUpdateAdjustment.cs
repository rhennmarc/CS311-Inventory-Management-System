using inventory_management;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Inventory_Management_System
{
    public partial class frmUpdateAdjustment : Form
    {
        private string username;
        private string originalProduct;
        private int originalQuantity;
        private decimal originalPrice;
        private string originalDateAdjusted;
        private string originalTimeAdjusted;

        public frmUpdateAdjustment(string product, string quantity, string unitprice, string reason, string createdby, string username, string dateadjusted, string timeadjusted)
        {
            InitializeComponent();
            InitializeActionComboBox();

            cmbproduct.Text = product;

            // Handle empty quantity
            if (string.IsNullOrEmpty(quantity))
            {
                originalQuantity = 0;
                txtquantity.Text = "";
                cmbaction.SelectedIndex = -1;
            }
            else
            {
                // Parse the original quantity to determine action and absolute value
                int qty = 0;
                int.TryParse(quantity, out qty);
                originalQuantity = qty;

                if (qty >= 0)
                {
                    cmbaction.Text = "Add";
                    txtquantity.Text = Math.Abs(qty).ToString();
                }
                else
                {
                    cmbaction.Text = "Remove";
                    txtquantity.Text = Math.Abs(qty).ToString();
                }
            }

            // Only set price if it's not empty
            if (!string.IsNullOrEmpty(unitprice))
            {
                txtprice.Text = unitprice;
            }

            txtreason.Text = reason;

            originalProduct = product;
            originalDateAdjusted = dateadjusted;
            originalTimeAdjusted = timeadjusted;

            // Parse original price
            if (!string.IsNullOrEmpty(unitprice))
            {
                decimal.TryParse(unitprice, out originalPrice);
            }

            this.username = username;
        }

        Class1 updateadjustment = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

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

        private void LoadAdjustment(string product, string dateadjusted, string timeadjusted)
        {
            DataTable dt = updateadjustment.GetData("SELECT * FROM tbladjustment WHERE products='" + product.Replace("'", "''") +
                "' AND dateadjusted='" + dateadjusted.Replace("'", "''") + "' AND timeadjusted='" + timeadjusted.Replace("'", "''") + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                cmbproduct.Text = row["products"].ToString();

                // Handle empty quantity
                if (row["quantity"] == DBNull.Value || string.IsNullOrEmpty(row["quantity"].ToString()))
                {
                    originalQuantity = 0;
                    txtquantity.Text = "";
                    cmbaction.SelectedIndex = -1;
                }
                else
                {
                    // Parse quantity for action/value
                    int qty = 0;
                    int.TryParse(row["quantity"].ToString(), out qty);
                    originalQuantity = qty;

                    if (qty >= 0)
                    {
                        cmbaction.Text = "Add";
                        txtquantity.Text = Math.Abs(qty).ToString();
                    }
                    else
                    {
                        cmbaction.Text = "Remove";
                        txtquantity.Text = Math.Abs(qty).ToString();
                    }
                }

                // Only set price if it's not null or empty
                if (row["unitprice"] != DBNull.Value && !string.IsNullOrEmpty(row["unitprice"].ToString()))
                {
                    txtprice.Text = row["unitprice"].ToString();
                }

                txtreason.Text = row["reason"].ToString();

                originalProduct = cmbproduct.Text;
                originalDateAdjusted = row["dateadjusted"].ToString();
                originalTimeAdjusted = row["timeadjusted"].ToString();

                if (row["unitprice"] != DBNull.Value && !string.IsNullOrEmpty(row["unitprice"].ToString()))
                {
                    decimal.TryParse(row["unitprice"].ToString(), out originalPrice);
                }
            }
        }

        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // Validate product - only check if text is empty, not SelectedIndex
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

            // Validate action (only if quantity is provided)
            if (hasQuantity && (cmbaction.SelectedIndex == -1 || string.IsNullOrEmpty(cmbaction.Text)))
            {
                errorProvider1.SetError(cmbaction, "Action is required when adjusting quantity. Select Add or Remove.");
                errorcount++;
            }

            // Validate quantity (if provided)
            if (hasQuantity)
            {
                int qty;
                if (!int.TryParse(txtquantity.Text, out qty) || qty <= 0)
                {
                    errorProvider1.SetError(txtquantity, "Quantity must be a valid number greater than 0.");
                    errorcount++;
                }
            }

            // Validate price (if provided)
            if (hasPrice)
            {
                decimal price;
                if (!decimal.TryParse(txtprice.Text.Trim(), out price) || price <= 0)
                {
                    errorProvider1.SetError(txtprice, "Price must be numeric and greater than 0.");
                    errorcount++;
                }
            }

            // Validate reason
            if (string.IsNullOrEmpty(txtreason.Text.Trim()))
            {
                errorProvider1.SetError(txtreason, "Reason is required.");
                errorcount++;
            }

            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to update this adjustment?\n\nNote: This will reverse the original adjustment and apply the new one.",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string product = cmbproduct.Text.Trim().Replace("'", "''");
                        string action = hasQuantity ? cmbaction.Text.Trim() : "PRICE UPDATE";
                        int quantity = hasQuantity ? int.Parse(txtquantity.Text.Trim()) : 0;
                        decimal unitprice = hasPrice ? decimal.Parse(txtprice.Text.Trim()) : 0;
                        string reason = txtreason.Text.Trim().Replace("'", "''");
                        string user = string.IsNullOrEmpty(username) ? "" : username.Replace("'", "''");

                        // Check if product exists in tblproducts
                        string checkProductQuery = "SELECT currentstock, unitprice FROM tblproducts WHERE LOWER(products) = LOWER('" + product + "')";
                        DataTable dtProduct = updateadjustment.GetData(checkProductQuery);

                        if (dtProduct.Rows.Count == 0)
                        {
                            MessageBox.Show("Product '" + product + "' does not exist in the products table.",
                                "Product Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Get current stock and price
                        int currentStock = 0;
                        decimal currentPrice = 0;
                        int.TryParse(dtProduct.Rows[0]["currentstock"].ToString(), out currentStock);
                        decimal.TryParse(dtProduct.Rows[0]["unitprice"].ToString(), out currentPrice);

                        int finalStock = currentStock;
                        decimal finalPrice = currentPrice;

                        // Calculate stock changes only if quantity is provided
                        if (hasQuantity)
                        {
                            // Calculate the net effect of changing the adjustment
                            // First, reverse the original adjustment effect
                            int stockAfterReversal = currentStock - originalQuantity;

                            // Then apply the new adjustment
                            int newAdjustmentValue = action.ToUpper() == "ADD" ? quantity : -quantity;
                            finalStock = stockAfterReversal + newAdjustmentValue;

                            // Check for negative stock
                            if (finalStock < 0)
                            {
                                MessageBox.Show($"Cannot update adjustment. This would result in negative stock.\n\nCurrent Stock: {currentStock}\nAfter reversing original: {stockAfterReversal}\nAfter new adjustment: {finalStock}",
                                    "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Set final price if provided
                        if (hasPrice)
                        {
                            finalPrice = unitprice;
                        }

                        // Prepare quantity for SQL - use NULL if empty
                        string sqlQuantity = "";
                        if (hasQuantity)
                        {
                            sqlQuantity = (action.ToUpper() == "ADD" ? quantity.ToString() : ("-" + quantity.ToString()));
                        }

                        // Use NULL if price is not provided, otherwise use the new price
                        string recordPrice = hasPrice ? "'" + unitprice.ToString("F2") + "'" : "NULL";

                        // Prepare quantity for SQL query
                        string quantitySqlValue = hasQuantity ? "'" + sqlQuantity + "'" : "NULL";

                        // Use dateadjusted and timeadjusted to uniquely identify the record
                        string sql =
                            "UPDATE tbladjustment SET " +
                            "products='" + product + "', " +
                            "quantity=" + quantitySqlValue + ", " +
                            "unitprice=" + recordPrice + ", " +
                            "reason='" + reason + "', " +
                            "createdby='" + user + "' " +
                            "WHERE products='" + originalProduct.Replace("'", "''") +
                            "' AND dateadjusted='" + originalDateAdjusted.Replace("'", "''") +
                            "' AND timeadjusted='" + originalTimeAdjusted.Replace("'", "''") + "'";

                        updateadjustment.executeSQL(sql);

                        if (updateadjustment.rowAffected > 0)
                        {
                            // Build dynamic UPDATE query - only update fields that have values
                            string updateProductQuery = "UPDATE tblproducts SET ";
                            List<string> updateFields = new List<string>();

                            if (hasQuantity)
                            {
                                updateFields.Add("currentstock = '" + finalStock + "'");
                            }
                            if (hasPrice)
                            {
                                updateFields.Add("unitprice = '" + finalPrice.ToString("F2") + "'");
                            }

                            if (updateFields.Count > 0)
                            {
                                updateProductQuery += string.Join(", ", updateFields);
                                updateProductQuery += " WHERE LOWER(products) = LOWER('" + product + "')";

                                updateadjustment.executeSQL(updateProductQuery);

                                if (updateadjustment.rowAffected > 0)
                                {
                                    string message = $"Adjustment updated successfully!\n\nProduct: {product}";

                                    if (hasQuantity)
                                    {
                                        message += $"\nOriginal Adjustment: {(originalQuantity >= 0 ? "+" : "")}{originalQuantity}";
                                        message += $"\nNew Adjustment: {(action.ToUpper() == "ADD" ? "+" : "-")}{quantity}";
                                        message += $"\nStock Change: {currentStock} → {finalStock}";
                                    }
                                    if (hasPrice)
                                    {
                                        message += $"\nPrice: ₱{currentPrice:N2} → ₱{finalPrice:N2}";
                                    }

                                    MessageBox.Show(message, "Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Log the update action
                                    string logAction = hasQuantity ? "UPDATE ADJUSTMENT" : "UPDATE PRICE";
                                    string logDetails = "Product: " + product;

                                    if (hasQuantity)
                                    {
                                        logDetails += $", Old: {(originalQuantity >= 0 ? "+" : "")}{originalQuantity}";
                                        logDetails += $", New: {(action.ToUpper() == "ADD" ? "+" : "-")}{quantity}";
                                        logDetails += $", Stock: {currentStock}→{finalStock}";
                                    }
                                    if (hasPrice)
                                    {
                                        logDetails += $", Price: {currentPrice:N2}→{finalPrice:N2}";
                                    }

                                    updateadjustment.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" +
                                        DateTime.Now.ToString("MM/dd/yyyy") + "' , '" + DateTime.Now.ToShortTimeString() +
                                        "' , '" + logAction + "', 'ADJUSTMENT MANAGEMENT', '" + logDetails + "', '" + username + "')");

                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Adjustment record updated but failed to update product. Please check manually.",
                                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Adjustment record updated but no product fields were changed.",
                                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please verify the Product Name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on updating adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateAdjustment_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titleLabel;

            // Load products into combo
            try
            {
                DataTable dt = updateadjustment.GetData("SELECT products FROM tblproducts ORDER BY products ASC");
                cmbproduct.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cmbproduct.Items.Add(row["products"].ToString());
                }

                cmbproduct.DropDownStyle = ComboBoxStyle.DropDown;
                cmbproduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbproduct.AutoCompleteSource = AutoCompleteSource.ListItems;
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
                        DataTable dt = updateadjustment.GetData(query);

                        if (dt.Rows.Count > 0)
                        {
                            // Only auto-populate if this is a different product than the original and price field is empty
                            if (selectedProduct != originalProduct && string.IsNullOrEmpty(txtprice.Text))
                            {
                                string currentPrice = dt.Rows[0]["unitprice"].ToString();
                                txtprice.Text = currentPrice;
                            }
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
            if (cmbaction.SelectedIndex != -1)
                errorProvider1.SetError(cmbaction, "");

            // Update preview only if quantity is provided
            if (!string.IsNullOrEmpty(txtquantity.Text.Trim()))
                UpdateStockPreview();
        }

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");

            // Enable/disable action combobox based on whether quantity is provided
            cmbaction.Enabled = !string.IsNullOrEmpty(txtquantity.Text.Trim());

            // Update preview only if product is selected and action is set
            if (!string.IsNullOrEmpty(cmbproduct.Text) && cmbaction.SelectedIndex != -1)
                UpdateStockPreview();

            // If quantity is cleared, clear the action selection
            if (string.IsNullOrEmpty(txtquantity.Text.Trim()))
            {
                cmbaction.SelectedIndex = -1;
            }
        }

        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits and backspace
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
                    txtprice.Text = price.ToString("F2");
                }
            }
        }

        private void txtreason_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtreason.Text))
                errorProvider1.SetError(txtreason, "");
        }

        private void txtprice_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtprice.Text))
                errorProvider1.SetError(txtprice, "");
        }

        private void UpdateStockPreview()
        {
            // Show preview of what the update will do (only if quantity is provided)
            try
            {
                if (!string.IsNullOrEmpty(cmbproduct.Text) && !string.IsNullOrEmpty(txtquantity.Text) && cmbaction.SelectedIndex != -1)
                {
                    string selectedProduct = cmbproduct.Text.Trim();
                    string selectedAction = cmbaction.Text;

                    if (int.TryParse(txtquantity.Text, out int newQuantity) && newQuantity > 0)
                    {
                        // Get current stock
                        string query = "SELECT currentstock FROM tblproducts WHERE LOWER(products) = LOWER('" + selectedProduct.Replace("'", "''") + "')";
                        DataTable dt = updateadjustment.GetData(query);

                        if (dt.Rows.Count > 0)
                        {
                            int currentStock = 0;
                            int.TryParse(dt.Rows[0]["currentstock"].ToString(), out currentStock);

                            // Calculate the effect of the update
                            int stockAfterReversal = currentStock - originalQuantity;
                            int newAdjustmentValue = selectedAction.ToUpper() == "ADD" ? newQuantity : -newQuantity;
                            int finalStock = stockAfterReversal + newAdjustmentValue;

                            string previewText;
                            if (finalStock < 0)
                            {
                                previewText = $"Warning: Update would result in {finalStock} stock (insufficient!)";
                            }
                            else
                            {
                                previewText = $"Preview: {currentStock} → {finalStock} (Original: {(originalQuantity >= 0 ? "+" : "")}{originalQuantity}, New: {(selectedAction.ToUpper() == "ADD" ? "+" : "-")}{newQuantity})";
                            }

                            // You could add a label to show the preview: lblPreview.Text = previewText;
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