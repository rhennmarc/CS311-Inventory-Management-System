using System;
using System.Data;
using System.Windows.Forms;
using inventory_management;

namespace Inventory_Management_System
{
    public partial class frmUpdatePurchaseOrder : Form
    {
        private string username;
        private string originalProduct;
        private string originalUnitCost;
        private string originalTimeCreated;
        private string originalDateCreated;
        private string originalSupplier;

        Class1 updatePO = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmUpdatePurchaseOrder(string products, string quantity, string unitcost, string username, string timecreated)
        {
            InitializeComponent();

            originalProduct = products;
            originalUnitCost = unitcost;
            originalTimeCreated = timecreated;
            this.username = username;

            cmbproduct.Text = products;
            txtquantity.Text = quantity;
            txtunitcost.Text = unitcost;
            txttotalcost.Text = CalculateTotalFromFields(quantity, unitcost);
        }

        private void frmUpdatePurchaseOrder_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titleLabel;

            try
            {
                // Load the original supplier from the purchase order
                LoadPurchaseOrderData();

                // Only load products from the same supplier
                DataTable dt = updatePO.GetData($"SELECT products FROM tblproducts WHERE supplier = '{originalSupplier.Replace("'", "''")}' ORDER BY products ASC");
                cmbproduct.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cmbproduct.Items.Add(row["products"].ToString());
                }

                // Display message if no products found for this supplier
                if (cmbproduct.Items.Count == 0)
                {
                    MessageBox.Show($"No products found for supplier: {originalSupplier}\nPlease add products for this supplier first.",
                        "No Products Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbproduct.Enabled = false;
                }

                cmbproduct.DropDownStyle = ComboBoxStyle.DropDown;
                cmbproduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbproduct.AutoCompleteSource = AutoCompleteSource.ListItems;

                // Load initial product details
                LoadProductDetails(originalProduct);

                // Update form title to show supplier
                this.Text = "Update Purchase Order - " + originalSupplier;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading form data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPurchaseOrderData()
        {
            try
            {
                string query = $"SELECT datecreated, supplier FROM tblpurchase_order WHERE products = '{originalProduct.Replace("'", "''")}' AND timecreated = '{originalTimeCreated.Replace("'", "''")}'";

                DataTable dt = updatePO.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    originalDateCreated = dt.Rows[0]["datecreated"].ToString();
                    originalSupplier = dt.Rows[0]["supplier"].ToString();
                }
                else
                {
                    originalDateCreated = DateTime.Now.ToString("MM/dd/yyyy");
                    originalSupplier = "Unknown";
                }
            }
            catch (Exception)
            {
                originalDateCreated = DateTime.Now.ToString("MM/dd/yyyy");
                originalSupplier = "Unknown";
            }
        }

        // --- Load product details when product is selected ---
        private void LoadProductDetails(string productName)
        {
            try
            {
                if (!string.IsNullOrEmpty(productName))
                {
                    string query = $"SELECT currentstock, unitprice FROM tblproducts WHERE products = '{productName.Replace("'", "''")}'";
                    DataTable dt = updatePO.GetData(query);

                    if (dt.Rows.Count > 0)
                    {
                        txtcurrentstock.Text = dt.Rows[0]["currentstock"].ToString();
                        txtunitprice.Text = dt.Rows[0]["unitprice"].ToString();
                    }
                    else
                    {
                        txtcurrentstock.Text = "N/A";
                        txtunitprice.Text = "N/A";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(cmbproduct.Text.Trim()))
            {
                errorProvider1.SetError(cmbproduct, "Product name is required.");
                return;
            }

            if (string.IsNullOrEmpty(txtquantity.Text.Trim()) || !int.TryParse(txtquantity.Text.Trim(), out int q) || q <= 0)
            {
                errorProvider1.SetError(txtquantity, "Valid quantity is required.");
                return;
            }

            if (string.IsNullOrEmpty(txtunitcost.Text.Trim()) || !decimal.TryParse(txtunitcost.Text.Trim(), out decimal uc) || uc <= 0)
            {
                errorProvider1.SetError(txtunitcost, "Valid unit cost is required.");
                return;
            }

            DialogResult dr = MessageBox.Show("Are you sure you want to update this purchase order?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    string product = cmbproduct.Text.Trim();
                    string quantity = txtquantity.Text.Trim();
                    string unitcost = txtunitcost.Text.Trim();
                    string totalcost = CalculateTotalFromFields(quantity, unitcost);

                    string updateQuery = $@"UPDATE tblpurchase_order SET 
                                           products = '{product.Replace("'", "''")}', 
                                           quantity = '{quantity.Replace("'", "''")}', 
                                           unitcost = '{unitcost.Replace("'", "''")}', 
                                           totalcost = '{totalcost.Replace("'", "''")}' 
                                           WHERE products = '{originalProduct.Replace("'", "''")}' 
                                           AND timecreated = '{originalTimeCreated.Replace("'", "''")}'";

                    updatePO.executeSQL(updateQuery);

                    if (updatePO.rowAffected > 0)
                    {
                        if (unitcost != originalUnitCost)
                        {
                            UpdateUnitCostHistory(product, unitcost);
                        }

                        LogAction("UPDATE", "PURCHASE ORDER MANAGEMENT", product);

                        MessageBox.Show("Purchase order updated successfully.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No changes were made to the purchase order.", "Information",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating purchase order: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateUnitCostHistory(string product, string unitCost)
        {
            try
            {
                string historyTimestamp = $"{originalDateCreated} {originalTimeCreated}";

                string updateQuery = $@"UPDATE tblhistory 
                                      SET unitcost = '{unitCost.Replace("'", "''")}'
                                      WHERE products = '{product.Replace("'", "''")}' 
                                      AND datecreated = '{historyTimestamp.Replace("'", "''")}'";

                updatePO.executeSQL(updateQuery);

                if (updatePO.rowAffected == 0)
                {
                    string insertQuery = $@"INSERT INTO tblhistory (products, unitcost, datecreated, createdby) 
                                          VALUES ('{product.Replace("'", "''")}', 
                                                  '{unitCost.Replace("'", "''")}', 
                                                  '{historyTimestamp}', 
                                                  '{username.Replace("'", "''")}')";
                    updatePO.executeSQL(insertQuery);
                }

                LogAction("HISTORY UPDATE", "UNIT COST HISTORY", $"Updated unit cost for {product}");

            }
            catch (Exception ex)
            {
                LogAction("HISTORY ERROR", "UNIT COST HISTORY", $"Failed to update history: {ex.Message}");
            }
        }

        private void LogAction(string action, string module, string performedTo)
        {
            try
            {
                string logQuery = $@"INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) 
                                   VALUES ('{DateTime.Now:MM/dd/yyyy}', 
                                           '{DateTime.Now:hh:mm:ss tt}', 
                                           '{action.Replace("'", "''")}', 
                                           '{module.Replace("'", "''")}', 
                                           '{performedTo.Replace("'", "''")}', 
                                           '{username.Replace("'", "''")}')";
                updatePO.executeSQL(logQuery);
            }
            catch
            {
                // Ignore logging errors
            }
        }

        private string CalculateTotalFromFields(string quantityStr, string unitcostStr)
        {
            try
            {
                int quantity = int.Parse(quantityStr);
                decimal unitcost = decimal.Parse(unitcostStr);
                return (quantity * unitcost).ToString("F2");
            }
            catch
            {
                return "0.00";
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Event handlers for real-time validation and calculation
        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            txttotalcost.Text = CalculateTotalFromFields(txtquantity.Text, txtunitcost.Text);
        }

        private void txtunitcost_TextChanged(object sender, EventArgs e)
        {
            txttotalcost.Text = CalculateTotalFromFields(txtquantity.Text, txtunitcost.Text);
        }

        private void txttotalcost_TextChanged(object sender, EventArgs e)
        {
            // This can be empty or add validation if needed
            if (!string.IsNullOrEmpty(txttotalcost.Text))
            {
                errorProvider1.SetError(txttotalcost, "");
            }
        }

        // --- When product selection changes ---
        private void cmbproduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbproduct.SelectedIndex >= 0)
            {
                string selectedProduct = cmbproduct.Text.Trim();
                LoadProductDetails(selectedProduct);
                errorProvider1.SetError(cmbproduct, "");
            }
        }

        private void cmbproduct_Leave(object sender, EventArgs e)
        {
            // Load product details when leaving the combobox if there's text
            if (!string.IsNullOrEmpty(cmbproduct.Text.Trim()))
            {
                LoadProductDetails(cmbproduct.Text.Trim());
            }

            if (string.IsNullOrEmpty(cmbproduct.Text.Trim()))
            {
                errorProvider1.SetError(cmbproduct, "Product name is required.");
            }
            else
            {
                errorProvider1.SetError(cmbproduct, "");
            }
        }

        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtunitcost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
                e.Handled = true;
        }
    }
}