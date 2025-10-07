using inventory_management;
using System;
using System.Data;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmUpdateProduct : Form
    {
        private string username;
        private string originalProduct;

        public frmUpdateProduct(string product, string description, string unitprice, string currentstock, string username)
        {
            InitializeComponent();

            txtproduct.Text = product;
            txtdescription.Text = description;
            txtunitprice.Text = unitprice;
            txtcurrentstock.Text = currentstock;

            originalProduct = product;
            this.username = username;
        }

        Class1 updateproduct = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private void LoadProduct(string product)
        {
            DataTable dt = updateproduct.GetData("SELECT * FROM tblproducts WHERE products='" + product.Replace("'", "''") + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtproduct.Text = row["products"].ToString();
                txtdescription.Text = row["description"].ToString();
                txtunitprice.Text = row["unitprice"].ToString();
                txtcurrentstock.Text = row["currentstock"].ToString();

                originalProduct = txtproduct.Text;
            }
        }

        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            if (string.IsNullOrEmpty(txtproduct.Text))
            {
                errorProvider1.SetError(txtproduct, "Product name is empty.");
                errorcount++;
            }
            if (string.IsNullOrEmpty(txtunitprice.Text))
            {
                errorProvider1.SetError(txtunitprice, "Unit price is empty.");
                errorcount++;
            }
            else
            {
                decimal price;
                if (!decimal.TryParse(txtunitprice.Text, out price) || price <= 0)
                {
                    errorProvider1.SetError(txtunitprice, "Unit price must be greater than 0.");
                    errorcount++;
                }
            }
            if (string.IsNullOrEmpty(txtcurrentstock.Text))
            {
                errorProvider1.SetError(txtcurrentstock, "Current stock is empty.");
                errorcount++;
            }
            else
            {
                int stock;
                if (!int.TryParse(txtcurrentstock.Text, out stock) || stock < 0)
                {
                    errorProvider1.SetError(txtcurrentstock, "Current stock must be a valid non-negative integer.");
                    errorcount++;
                }
            }

            // Check for duplicate product name if changed
            if (!string.IsNullOrEmpty(txtproduct.Text) && txtproduct.Text != originalProduct)
            {
                DataTable dt = updateproduct.GetData("SELECT * FROM tblproducts WHERE products='" + txtproduct.Text.Replace("'", "''") + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtproduct, "Product already exists.");
                    errorcount++;
                }
            }

            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to update this product?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string product = txtproduct.Text.Replace("'", "''");
                        string description = txtdescription.Text.Replace("'", "''");

                        // Format unit price to always show 2 decimal places
                        decimal unitPriceValue = decimal.Parse(txtunitprice.Text.Trim());
                        string unitprice = unitPriceValue.ToString("F2").Replace("'", "''");

                        string currentstock = txtcurrentstock.Text.Replace("'", "''");
                        string user = string.IsNullOrEmpty(username) ? "" : username.Replace("'", "''");

                        string sql =
                            "UPDATE tblproducts SET " +
                            "products='" + product + "', " +
                            "description='" + description + "', " +
                            "unitprice='" + unitprice + "', " +
                            "currentstock='" + currentstock + "' " +
                            "WHERE products='" + originalProduct.Replace("'", "''") + "'";

                        updateproduct.executeSQL(sql);

                        if (updateproduct.rowAffected > 0)
                        {
                            MessageBox.Show("Product updated successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            updateproduct.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" +
                                DateTime.Now.ToString("MM/dd/yyyy") + "' , '" + DateTime.Now.ToShortTimeString() +
                                "' , 'UPDATE', 'PRODUCTS MANAGEMENT', '" + product + "', '" + username + "')");

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please verify the Product Name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on updating product", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateProduct_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titleLabel;
        }

        private void txtproduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtproduct.Text))
                errorProvider1.SetError(txtproduct, "");
        }

        private void txtunitprice_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtunitprice.Text))
                errorProvider1.SetError(txtunitprice, "");
        }

        private void txtcurrentstock_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcurrentstock.Text))
                errorProvider1.SetError(txtcurrentstock, "");
        }

        private void txtunitprice_KeyPress(object sender, KeyPressEventArgs e)
        {
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
    }
}