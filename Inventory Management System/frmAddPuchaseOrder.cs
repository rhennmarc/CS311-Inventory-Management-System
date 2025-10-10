using System;
using System.Data;
using System.Windows.Forms;
using inventory_management;

namespace Inventory_Management_System
{
    public partial class frmAddPurchaseOrder : Form
    {
        private string username;
        private string supplierName;

        public frmAddPurchaseOrder(string username, string supplierName)
        {
            InitializeComponent();
            this.username = username;
            this.supplierName = supplierName;
        }

        Class1 newPurchaseOrder = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // Product required
            if (cmbproduct.SelectedIndex == -1 || string.IsNullOrEmpty(cmbproduct.Text.Trim()))
            {
                errorProvider1.SetError(cmbproduct, "Product name is required.");
                errorcount++;
            }

            // Quantity required
            if (string.IsNullOrEmpty(txtquantity.Text.Trim()))
            {
                errorProvider1.SetError(txtquantity, "Quantity is required.");
                errorcount++;
            }
            else
            {
                int quantity;
                if (!int.TryParse(txtquantity.Text.Trim(), out quantity) || quantity <= 0)
                {
                    errorProvider1.SetError(txtquantity, "Quantity must be a positive number.");
                    errorcount++;
                }
            }

            // Unit cost required
            if (string.IsNullOrEmpty(txtunitcost.Text.Trim()))
            {
                errorProvider1.SetError(txtunitcost, "Unit cost is required.");
                errorcount++;
            }
            else
            {
                decimal unitcost;
                if (!decimal.TryParse(txtunitcost.Text.Trim(), out unitcost) || unitcost <= 0)
                {
                    errorProvider1.SetError(txtunitcost, "Unit cost must be a positive decimal number.");
                    errorcount++;
                }
            }

            // Total cost validation
            if (string.IsNullOrEmpty(txttotalcost.Text.Trim()))
            {
                errorProvider1.SetError(txttotalcost, "Total cost is required.");
                errorcount++;
            }
            else
            {
                decimal totalcost;
                if (!decimal.TryParse(txttotalcost.Text.Trim(), out totalcost) || totalcost <= 0)
                {
                    errorProvider1.SetError(txttotalcost, "Total cost must be a positive decimal number.");
                    errorcount++;
                }
            }

            // Save if valid
            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to add this purchase order?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        string product = cmbproduct.Text.Trim().Replace("'", "''");
                        string quantity = txtquantity.Text.Trim().Replace("'", "''");
                        string unitcost = txtunitcost.Text.Trim().Replace("'", "''");
                        string totalcost = txttotalcost.Text.Trim().Replace("'", "''");
                        string supplier = supplierName.Replace("'", "''");
                        string createdBy = username.Replace("'", "''");
                        string dateCreated = DateTime.Now.ToString("MM/dd/yyyy");
                        string timeCreated = DateTime.Now.ToString("hh:mm:ss tt");
                        string status = "Pending";

                        string insertPurchaseOrder =
                            "INSERT INTO tblpurchase_order (products, quantity, unitcost, totalcost, status, createdby, datecreated, timecreated, supplier) " +
                            "VALUES ('" + product + "', '" + quantity + "', '" + unitcost + "', '" + totalcost + "', '" + status + "', '" + createdBy + "', '" + dateCreated + "', '" + timeCreated + "', '" + supplier + "')";

                        newPurchaseOrder.executeSQL(insertPurchaseOrder);

                        if (newPurchaseOrder.rowAffected > 0)
                        {
                            // Log unit cost history for new purchase order
                            LogUnitCostHistory(product, unitcost, createdBy);

                            MessageBox.Show("New purchase order added successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            newPurchaseOrder.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                                "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("hh:mm:ss tt") +
                                "', 'ADD', 'PURCHASE ORDER MANAGEMENT', '" + product + "', '" + username + "')");

                            this.Close();
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on adding new purchase order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogUnitCostHistory(string product, string unitCost, string username)
        {
            try
            {
                Class1 db = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

                // Always log the unit cost history when a new purchase order is created
                string insertQuery = $@"INSERT INTO tblhistory (products, unitcost, datecreated, createdby) 
                                      VALUES ('{product.Replace("'", "''")}', '{unitCost.Replace("'", "''")}', 
                                              '{DateTime.Now:MM/dd/yyyy hh:mm:ss tt}', '{username.Replace("'", "''")}')";

                db.executeSQL(insertQuery);

                // Log the history action
                db.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                    "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("hh:mm:ss tt") +
                    "', 'HISTORY LOG', 'UNIT COST HISTORY', 'New unit cost for " + product.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");

                MessageBox.Show($"Unit cost history logged for {product}", "History Updated",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Show error to user for debugging
                MessageBox.Show("Error logging unit cost history: " + ex.Message, "History Logging Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- Auto calculation ---
        private void CalculateTotalCost()
        {
            try
            {
                errorProvider1.SetError(txttotalcost, "");
                decimal quantity, unitcost;
                if (decimal.TryParse(txtquantity.Text.Trim(), out quantity) &&
                    decimal.TryParse(txtunitcost.Text.Trim(), out unitcost) &&
                    quantity > 0 && unitcost > 0)
                {
                    decimal totalcost = quantity * unitcost;
                    txttotalcost.Text = totalcost.ToString("F2");
                }
                else
                {
                    txttotalcost.Text = "";
                }
            }
            catch
            {
                txttotalcost.Text = "";
            }
        }

        private void frmAddPurchaseOrder_Load(object sender, EventArgs e)
        {
            cmbproduct.Focus();
            txttotalcost.Text = "";

            try
            {
                // Update the form title to show the supplier
                this.Text = "Add Purchase Order - " + supplierName;

                // Only load products from the selected supplier
                string query = $"SELECT products FROM tblproducts WHERE supplier = '{supplierName.Replace("'", "''")}' ORDER BY products ASC";
                DataTable dt = newPurchaseOrder.GetData(query);

                cmbproduct.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cmbproduct.Items.Add(row["products"].ToString());
                }

                // Display message if no products found for this supplier
                if (cmbproduct.Items.Count == 0)
                {
                    MessageBox.Show($"No products found for supplier: {supplierName}\nPlease add products for this supplier first.",
                        "No Products Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbproduct.Enabled = false;
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

        // --- Load product details when product is selected ---
        private void LoadProductDetails(string productName)
        {
            try
            {
                if (!string.IsNullOrEmpty(productName))
                {
                    string query = $"SELECT currentstock, unitprice FROM tblproducts WHERE products = '{productName.Replace("'", "''")}'";
                    DataTable dt = newPurchaseOrder.GetData(query);

                    if (dt.Rows.Count > 0)
                    {
                        txtcurrentstock.Text = dt.Rows[0]["currentstock"].ToString();
                        txtunitprice.Text = dt.Rows[0]["unitprice"].ToString();
                        // Removed automatic setting of unit cost - user will enter manually
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

        // --- EVENTS ---
        private void txtquantity_KeyUp(object sender, KeyEventArgs e) => CalculateTotalCost();
        private void txtunitcost_KeyUp(object sender, KeyEventArgs e) => CalculateTotalCost();
        private void txtquantity_TextChanged(object sender, EventArgs e) { if (!string.IsNullOrEmpty(txtquantity.Text)) errorProvider1.SetError(txtquantity, ""); CalculateTotalCost(); }
        private void txtunitcost_TextChanged(object sender, EventArgs e) { if (!string.IsNullOrEmpty(txtunitcost.Text)) errorProvider1.SetError(txtunitcost, ""); CalculateTotalCost(); }

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

        private void cmbproduct_TextChanged(object sender, EventArgs e)
        {
            // Load product details when text changes (for auto-complete)
            if (!string.IsNullOrEmpty(cmbproduct.Text.Trim()) && cmbproduct.SelectedIndex == -1)
            {
                // You might want to add a delay or button to search for products
                // to avoid excessive database queries during typing
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

        // --- KEYPRESS VALIDATION ---
        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Only allow digits, backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtunitcost_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, backspace, and one decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
    }
}