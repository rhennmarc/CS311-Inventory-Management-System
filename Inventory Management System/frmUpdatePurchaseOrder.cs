using inventory_management;
using System;
using System.Data;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmUpdatePurchaseOrder : Form
    {
        private string username;
        private string originalProduct;

        public frmUpdatePurchaseOrder(string products, string quantity, string unitcost, string username)
        {
            InitializeComponent();

            // Populate fields with provided values
            txtproduct.Text = products;
            txtquantity.Text = quantity;
            txtunitcost.Text = unitcost;
            txttotalcost.Text = CalculateTotalFromFields(quantity, unitcost); // keep total consistent

            originalProduct = products;
            this.username = username;
        }

        Class1 updatePO = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private void LoadPurchaseOrder(string product)
        {
            string q = "SELECT * FROM tblpurchase_order WHERE products='" + product.Replace("'", "''") + "'";

            DataTable dt = updatePO.GetData(q);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtproduct.Text = row["products"].ToString();
                txtquantity.Text = row["quantity"].ToString();
                txtunitcost.Text = row["unitcost"].ToString();
                txttotalcost.Text = row["totalcost"].ToString();

                originalProduct = txtproduct.Text;
            }
        }

        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // --- VALIDATIONS ---

            if (string.IsNullOrEmpty(txtproduct.Text.Trim()))
            {
                errorProvider1.SetError(txtproduct, "Product name is required.");
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

            // Check duplicate if product changed
            if (!string.IsNullOrEmpty(txtproduct.Text) && txtproduct.Text != originalProduct)
            {
                string dupQuery = "SELECT * FROM tblpurchase_order WHERE products = '" + txtproduct.Text.Replace("'", "''") + "'";
                DataTable dtDup = updatePO.GetData(dupQuery);
                if (dtDup.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtproduct, "A purchase order for this product already exists.");
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
                        string product = txtproduct.Text.Trim().Replace("'", "''");
                        string quantity = txtquantity.Text.Trim().Replace("'", "''");
                        string unitcost = txtunitcost.Text.Trim().Replace("'", "''");
                        string totalcost = txttotalcost.Text.Trim().Replace("'", "''");
                        string user = string.IsNullOrEmpty(username) ? "" : username.Replace("'", "''");

                        string sql = "UPDATE tblpurchase_order SET " +
                                     "products = '" + product + "', " +
                                     "quantity = '" + quantity + "', " +
                                     "unitcost = '" + unitcost + "', " +
                                     "totalcost = '" + totalcost + "' " +
                                     "WHERE products = '" + originalProduct.Replace("'", "''") + "'";

                        updatePO.executeSQL(sql);

                        if (updatePO.rowAffected > 0)
                        {
                            MessageBox.Show("Purchase order updated successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            updatePO.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" +
                                DateTime.Now.ToString("MM/dd/yyyy") + "' , '" + DateTime.Now.ToShortTimeString() +
                                "' , 'UPDATE', 'PURCHASE ORDER MANAGEMENT', '" + product + "', '" + username + "')");

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please verify the original Product.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on updating purchase order", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdatePurchaseOrder_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titleLabel;
        }

        // Utility to calculate total cost from quantity and unitcost (returns empty if invalid)
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

        private void txtproduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtproduct.Text))
                errorProvider1.SetError(txtproduct, "");
        }

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");

            // Recalculate total
            txttotalcost.Text = CalculateTotalFromFields(txtquantity.Text, txtunitcost.Text);
        }

        private void txtunitcost_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtunitcost.Text))
                errorProvider1.SetError(txtunitcost, "");

            // Recalculate total
            txttotalcost.Text = CalculateTotalFromFields(txtquantity.Text, txtunitcost.Text);
        }

        private void txttotalcost_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txttotalcost.Text))
                errorProvider1.SetError(txttotalcost, "");
        }
    }
}
