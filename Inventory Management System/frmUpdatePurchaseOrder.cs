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
        private string originalUnitCost; // Store original unit cost for comparison
        private string originalTimeCreated; // Store original timecreated for unique identification

        public frmUpdatePurchaseOrder(string products, string quantity, string unitcost, string username, string timecreated)
        {
            InitializeComponent();

            // Populate fields with provided values
            cmbproduct.Text = products; // ✅ combo instead of textbox
            txtquantity.Text = quantity;
            txtunitcost.Text = unitcost;
            txttotalcost.Text = CalculateTotalFromFields(quantity, unitcost);

            originalProduct = products;
            originalUnitCost = unitcost; // Store the original unit cost
            originalTimeCreated = timecreated; // Store the original timecreated
            this.username = username;
        }

        Class1 updatePO = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private void LoadPurchaseOrder(string product, string timecreated)
        {
            string q = "SELECT * FROM tblpurchase_order WHERE products='" + product.Replace("'", "''") + "' AND timecreated='" + timecreated.Replace("'", "''") + "'";

            DataTable dt = updatePO.GetData(q);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                cmbproduct.Text = row["products"].ToString();
                txtquantity.Text = row["quantity"].ToString();
                txtunitcost.Text = row["unitcost"].ToString();
                txttotalcost.Text = row["totalcost"].ToString();

                originalProduct = cmbproduct.Text;
                originalUnitCost = row["unitcost"].ToString(); // Store original unit cost
                originalTimeCreated = row["timecreated"].ToString(); // Store original timecreated
            }
        }

        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // --- VALIDATIONS ---
            if (cmbproduct.SelectedIndex == -1 || string.IsNullOrEmpty(cmbproduct.Text.Trim()))
            {
                errorProvider1.SetError(cmbproduct, "Product name is required.");
                errorcount++;
            }

            if (string.IsNullOrEmpty(txtquantity.Text.Trim()))
            {
                errorProvider1.SetError(txtquantity, "Quantity is required.");
                errorcount++;
            }
            else
            {
                int q;
                if (!int.TryParse(txtquantity.Text.Trim(), out q) || q <= 0)
                {
                    errorProvider1.SetError(txtquantity, "Quantity must be a positive integer.");
                    errorcount++;
                }
            }

            if (string.IsNullOrEmpty(txtunitcost.Text.Trim()))
            {
                errorProvider1.SetError(txtunitcost, "Unit cost is required.");
                errorcount++;
            }
            else
            {
                decimal uc;
                if (!decimal.TryParse(txtunitcost.Text.Trim(), out uc) || uc <= 0)
                {
                    errorProvider1.SetError(txtunitcost, "Unit cost must be a positive decimal.");
                    errorcount++;
                }
            }

            if (string.IsNullOrEmpty(txttotalcost.Text.Trim()))
            {
                errorProvider1.SetError(txttotalcost, "Total cost is required.");
                errorcount++;
            }
            else
            {
                decimal tc;
                if (!decimal.TryParse(txttotalcost.Text.Trim(), out tc) || tc <= 0)
                {
                    errorProvider1.SetError(txttotalcost, "Total cost must be a positive decimal.");
                    errorcount++;
                }
            }

            // Check duplicate if product changed (using product + timecreated as unique key)
            if (!string.IsNullOrEmpty(cmbproduct.Text) && (cmbproduct.Text != originalProduct))
            {
                string dupQuery = "SELECT * FROM tblpurchase_order WHERE products = '" + cmbproduct.Text.Replace("'", "''") + "' AND timecreated = '" + originalTimeCreated.Replace("'", "''") + "'";
                DataTable dtDup = updatePO.GetData(dupQuery);
                if (dtDup.Rows.Count > 0)
                {
                    errorProvider1.SetError(cmbproduct, "A purchase order for this product with the same creation time already exists.");
                    errorcount++;
                }
            }

            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to update this purchase order?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        string product = cmbproduct.Text.Trim().Replace("'", "''");
                        string quantity = txtquantity.Text.Trim().Replace("'", "''");
                        string unitcost = txtunitcost.Text.Trim().Replace("'", "''");
                        string totalcost = txttotalcost.Text.Trim().Replace("'", "''");
                        string user = string.IsNullOrEmpty(username) ? "" : username.Replace("'", "''");

                        // Use both original product and original timecreated to uniquely identify the record
                        string sql = "UPDATE tblpurchase_order SET " +
                                     "products = '" + product + "', " +
                                     "quantity = '" + quantity + "', " +
                                     "unitcost = '" + unitcost + "', " +
                                     "totalcost = '" + totalcost + "' " +
                                     "WHERE products = '" + originalProduct.Replace("'", "''") + "' " +
                                     "AND timecreated = '" + originalTimeCreated.Replace("'", "''") + "'";

                        updatePO.executeSQL(sql);

                        if (updatePO.rowAffected > 0)
                        {
                            // Log unit cost history only if unit cost has changed
                            if (unitcost != originalUnitCost)
                            {
                                LogUnitCostHistory(product, unitcost, user);
                            }

                            MessageBox.Show("Purchase order updated successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            updatePO.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" +
                                DateTime.Now.ToString("MM/dd/yyyy") + "' , '" + DateTime.Now.ToString("hh:mm:ss tt") +
                                "' , 'UPDATE', 'PURCHASE ORDER MANAGEMENT', '" + product + "', '" + username + "')");

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please verify the original Product and Time Created.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on updating purchase order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LogUnitCostHistory(string product, string unitCost, string username)
        {
            try
            {
                Class1 db = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

                // Always log the unit cost history when unit cost is changed in update
                string insertQuery = $@"INSERT INTO tblhistory (products, unitcost, datecreated, createdby) 
                                      VALUES ('{product.Replace("'", "''")}', '{unitCost.Replace("'", "''")}', 
                                              '{DateTime.Now:MM/dd/yyyy hh:mm:ss tt}', '{username.Replace("'", "''")}')";

                db.executeSQL(insertQuery);

                // Log the history action
                db.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                    "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToString("hh:mm:ss tt") +
                    "', 'HISTORY LOG', 'UNIT COST HISTORY', 'Updated unit cost for " + product.Replace("'", "''") + "', '" + username.Replace("'", "''") + "')");

                MessageBox.Show($"Unit cost history updated for {product}", "History Updated",
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

        private void frmUpdatePurchaseOrder_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titleLabel;

            // Load the specific record using product and timecreated
            LoadPurchaseOrder(originalProduct, originalTimeCreated);

            // Load products to combo
            try
            {
                DataTable dt = updatePO.GetData("SELECT products FROM tblproducts ORDER BY products ASC");
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

        // Utility to calculate total cost
        private string CalculateTotalFromFields(string quantityStr, string unitcostStr)
        {
            try
            {
                decimal q, uc;
                if (decimal.TryParse(quantityStr.Trim(), out q) && decimal.TryParse(unitcostStr.Trim(), out uc))
                {
                    decimal total = q * uc;
                    return total.ToString("F2");
                }
            }
            catch { }
            return "";
        }

        private void cmbproduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbproduct.Text))
                errorProvider1.SetError(cmbproduct, "");
        }

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");

            txttotalcost.Text = CalculateTotalFromFields(txtquantity.Text, txtunitcost.Text);
        }

        private void txtunitcost_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtunitcost.Text))
                errorProvider1.SetError(txtunitcost, "");

            txttotalcost.Text = CalculateTotalFromFields(txtquantity.Text, txtunitcost.Text);
        }

        private void txttotalcost_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txttotalcost.Text))
                errorProvider1.SetError(txttotalcost, "");
        }

        // --- KEYPRESS VALIDATION ---
        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtunitcost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
    }
}