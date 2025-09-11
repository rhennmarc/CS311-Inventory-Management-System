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
                if (dt.Rows.Count > 0)
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
                        string unitprice = txtunitprice.Text.Trim().Replace("'", "''");
                        string stock = txtcurrentstock.Text.Trim().Replace("'", "''");
                        string createdBy = username.Replace("'", "''");
                        string dateCreated = DateTime.Now.ToString("MM/dd/yyyy");

                        string insertProduct =
                            "INSERT INTO tblproducts (products, description, unitprice, currentstock, createdby, datecreated) " +
                            "VALUES ('" + product + "', '" + description + "', '" + unitprice + "', '" + stock + "', '" + createdBy + "', '" + dateCreated + "')";

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
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on adding new product", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }

        private void txtcurrentstock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcurrentstock.Text))
                errorProvider1.SetError(txtcurrentstock, "");
        }

        private void cardPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
