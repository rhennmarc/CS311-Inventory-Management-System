using System;
using System.Data;
using System.Windows.Forms;
using inventory_management;

namespace Inventory_Management_System
{
    public partial class frmAddProduct : Form
    {
        private string username;
        public frmAddProduct(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        Class1 newproduct = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");
        private int errorcount;

        private void frmAddProduct_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            try
            {
                // Clear the combo box first
                cmbsupplier.Items.Clear();

                // Add default empty option
                cmbsupplier.Items.Add("");

                // Get suppliers from database - use the EXACT SAME query as your update form
                DataTable dtSuppliers = newproduct.GetData("SELECT supplier FROM tblsupplier ORDER BY supplier");

                // Debug: Check what we're getting
                Console.WriteLine($"Suppliers DataTable: {dtSuppliers?.Rows.Count ?? 0} rows");

                if (dtSuppliers == null)
                {
                    MessageBox.Show("Database connection failed or query returned null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dtSuppliers.Rows.Count == 0)
                {
                    MessageBox.Show("No suppliers found in database. Please add suppliers first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Add suppliers to combo box
                foreach (DataRow row in dtSuppliers.Rows)
                {
                    string supplierName = row["supplier"].ToString();
                    if (!string.IsNullOrEmpty(supplierName))
                    {
                        cmbsupplier.Items.Add(supplierName);
                    }
                }

                // Select the first item (the empty option)
                if (cmbsupplier.Items.Count > 0)
                {
                    cmbsupplier.SelectedIndex = 0;
                }

                // Force the combo box to refresh
                cmbsupplier.Refresh();

                // Debug output
                Console.WriteLine($"ComboBox now has {cmbsupplier.Items.Count} items");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // --- VALIDATIONS ---

            // Product name (required + unique)
            if (string.IsNullOrEmpty(txtproduct.Text.Trim()))
            {
                errorProvider1.SetError(txtproduct, "Product name is required.");
                errorcount++;
            }
            else
            {
                DataTable dt = newproduct.GetData("SELECT products FROM tblproducts WHERE products='" + txtproduct.Text.Trim().Replace("'", "''") + "' LIMIT 1");
                if (dt != null && dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtproduct, "Product already exists.");
                    errorcount++;
                }
            }

            // Unit price (required, > 0, numeric)
            if (string.IsNullOrEmpty(txtunitprice.Text.Trim()))
            {
                errorProvider1.SetError(txtunitprice, "Unit price is required.");
                errorcount++;
            }
            else
            {
                decimal unitprice;
                if (!decimal.TryParse(txtunitprice.Text.Trim(), out unitprice) || unitprice <= 0)
                {
                    errorProvider1.SetError(txtunitprice, "Unit price must be numeric and greater than 0.");
                    errorcount++;
                }
            }

            // Current stock (required, >= 0, numeric)
            if (string.IsNullOrEmpty(txtcurrentstock.Text.Trim()))
            {
                errorProvider1.SetError(txtcurrentstock, "Current stock is required.");
                errorcount++;
            }
            else
            {
                int stock;
                if (!int.TryParse(txtcurrentstock.Text.Trim(), out stock) || stock < 0)
                {
                    errorProvider1.SetError(txtcurrentstock, "Current stock must be numeric and not negative.");
                    errorcount++;
                }
            }

            // Supplier (REQUIRED validation) - FIXED: Check if it's the default empty option
            if (string.IsNullOrEmpty(cmbsupplier.Text.Trim()) || cmbsupplier.SelectedIndex == 0 || cmbsupplier.Text == "-- Select Supplier --")
            {
                errorProvider1.SetError(cmbsupplier, "Supplier is required.");
                errorcount++;
            }
            else
            {
                DataTable dt = newproduct.GetData("SELECT supplier FROM tblsupplier WHERE supplier='" + cmbsupplier.Text.Trim().Replace("'", "''") + "' LIMIT 1");
                if (dt == null || dt.Rows.Count == 0)
                {
                    errorProvider1.SetError(cmbsupplier, "Selected supplier does not exist.");
                    errorcount++;
                }
            }

            // --- SAVE TO DATABASE ---
            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to add this product?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        string product = txtproduct.Text.Trim().Replace("'", "''");
                        string description = txtdescription.Text.Trim().Replace("'", "''");

                        // Format unit price to always show 2 decimal places
                        decimal unitPriceValue = decimal.Parse(txtunitprice.Text.Trim());
                        string unitprice = unitPriceValue.ToString("F2").Replace("'", "''");

                        string stock = txtcurrentstock.Text.Trim().Replace("'", "''");
                        string supplier = cmbsupplier.Text.Trim().Replace("'", "''");
                        string createdBy = username.Replace("'", "''");
                        string dateCreated = DateTime.Now.ToString("MM/dd/yyyy");

                        string insertProduct =
                            "INSERT INTO tblproducts (products, description, unitprice, currentstock, supplier, createdby, datecreated) " +
                            "VALUES ('" + product + "', '" + description + "', '" + unitprice + "', '" + stock + "', '" + supplier + "', '" + createdBy + "', '" + dateCreated + "')";

                        newproduct.executeSQL(insertProduct);

                        if (newproduct.rowAffected > 0)
                        {
                            MessageBox.Show("New product added successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // log the action
                            newproduct.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                                "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'ADD', 'PRODUCTS MANAGEMENT', '" + product + "', '" + username + "')");

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add product. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("ERROR on adding new product: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- Remove error when user starts typing ---
        private void txtproduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtproduct.Text))
                errorProvider1.SetError(txtproduct, "");
        }

        private void txtunitprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtunitprice.Text))
                errorProvider1.SetError(txtunitprice, "");

            // Allow only numbers, decimal point, and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && txtunitprice.Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtcurrentstock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcurrentstock.Text))
                errorProvider1.SetError(txtcurrentstock, "");

            // Allow only numbers and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtunitprice_Leave(object sender, EventArgs e)
        {
            // Format the unit price when leaving the textbox
            if (!string.IsNullOrEmpty(txtunitprice.Text.Trim()))
            {
                decimal price;
                if (decimal.TryParse(txtunitprice.Text.Trim(), out price) && price > 0)
                {
                    txtunitprice.Text = price.ToString("F2");
                }
            }
        }

        private void cmbsupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbsupplier.Text) && cmbsupplier.SelectedIndex > 0)
                errorProvider1.SetError(cmbsupplier, "");
        }

        private void cmbsupplier_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbsupplier.Text) && cmbsupplier.SelectedIndex > 0)
                errorProvider1.SetError(cmbsupplier, "");
        }
    }
}