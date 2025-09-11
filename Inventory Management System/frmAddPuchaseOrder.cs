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

            // --- VALIDATIONS ---

            // Product name (required)
            if (string.IsNullOrEmpty(txtproduct.Text.Trim()))
            {
                errorProvider1.SetError(txtproduct, "Product name is required.");
                errorcount++;
            }

            // Quantity (required, > 0, numeric)
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

            // Unit cost (required, > 0, numeric)
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

            // Total cost validation (should be calculated automatically)
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

            // --- SAVE TO DATABASE ---
            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to add this purchase order?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        string product = txtproduct.Text.Trim().Replace("'", "''");
                        string quantity = txtquantity.Text.Trim().Replace("'", "''");
                        string unitcost = txtunitcost.Text.Trim().Replace("'", "''");
                        string totalcost = txttotalcost.Text.Trim().Replace("'", "''");
                        string supplier = supplierName.Replace("'", "''");
                        string createdBy = username.Replace("'", "''");
                        string dateCreated = DateTime.Now.ToString("MM/dd/yyyy");
                        string status = "Pending"; // Default status

                        string insertPurchaseOrder =
                            "INSERT INTO tblpurchase_order (products, quantity, unitcost, totalcost, status, createdby, datecreated, supplier) " +
                            "VALUES ('" + product + "', '" + quantity + "', '" + unitcost + "', '" + totalcost + "', '" + status + "', '" + createdBy + "', '" + dateCreated + "', '" + supplier + "')";

                        newPurchaseOrder.executeSQL(insertPurchaseOrder);

                        if (newPurchaseOrder.rowAffected > 0)
                        {
                            MessageBox.Show("New purchase order added successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Log the action
                            newPurchaseOrder.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                                "VALUES ('" + DateTime.Now.ToString("dd/MM/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
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

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Auto-calculate total cost when quantity or unit cost changes
        private void CalculateTotalCost()
        {
            try
            {
                // Clear error on total cost field when recalculating
                errorProvider1.SetError(txttotalcost, "");

                decimal quantity, unitcost;

                // Check if both fields have valid numeric values
                if (decimal.TryParse(txtquantity.Text.Trim(), out quantity) &&
                    decimal.TryParse(txtunitcost.Text.Trim(), out unitcost) &&
                    quantity > 0 && unitcost > 0)
                {
                    decimal totalcost = quantity * unitcost;
                    txttotalcost.Text = totalcost.ToString("F2");
                }
                else
                {
                    // Clear total cost if either quantity or unit cost is invalid
                    txttotalcost.Text = "";
                }
            }
            catch
            {
                txttotalcost.Text = "";
            }
        }

        // --- Remove error when user starts typing ---

        private void txttotalcost_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow numbers, decimal point, and control keys for total cost
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            if (!string.IsNullOrEmpty(txttotalcost.Text))
                errorProvider1.SetError(txttotalcost, "");
        }

        private void txtquantity_Leave(object sender, EventArgs e)
        {
            // Validate quantity when leaving the field
            if (!string.IsNullOrEmpty(txtquantity.Text.Trim()))
            {
                int quantity;
                if (!int.TryParse(txtquantity.Text.Trim(), out quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter a valid positive number for quantity.", "Invalid Quantity",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtquantity.Focus();
                    txtquantity.SelectAll();
                    return;
                }
            }

            // Recalculate after validation
            CalculateTotalCost();
        }

        private void frmAddPurchaseOrder_Load(object sender, EventArgs e)
        {
            // Set default values and focus
            txtproduct.Focus();

            // Initialize total cost field
            txttotalcost.Text = "";
        }

        private void txtquantity_KeyUp(object sender, KeyEventArgs e)
        {
            CalculateTotalCost();
        }

        private void txtunitcost_KeyUp(object sender, KeyEventArgs e)
        {
            CalculateTotalCost();
        }

        private void txtproduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtproduct.Text))
                errorProvider1.SetError(txtproduct, "");
        }

        private void txtproduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtproduct.Text))
                errorProvider1.SetError(txtproduct, "");
        }

        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers and control keys for quantity
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");
        }

        private void txtunitcost_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow numbers, decimal point, and control keys for unit cost
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            if (!string.IsNullOrEmpty(txtunitcost.Text))
                errorProvider1.SetError(txtunitcost, "");
        }

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            // Clear error when user starts typing
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");

            // Calculate total cost
            CalculateTotalCost();
        }

        private void txtunitcost_TextChanged(object sender, EventArgs e)
        {
            // Clear error when user starts typing
            if (!string.IsNullOrEmpty(txtunitcost.Text))
                errorProvider1.SetError(txtunitcost, "");

            // Calculate total cost
            CalculateTotalCost();
        }
    }
}