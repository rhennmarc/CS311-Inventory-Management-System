using inventory_management;
using System;
using System.Data;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmUpdateAdjustment : Form
    {
        private string username;
        private string originalProduct;

        public frmUpdateAdjustment(string product, string quantity, string reason, string createdby, string username)
        {
            InitializeComponent();

            cmbproduct.Text = product; // ✅ combo instead of textbox
            txtquantity.Text = quantity;
            txtreason.Text = reason;

            originalProduct = product;
            this.username = username;
        }

        Class1 updateadjustment = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private void LoadAdjustment(string product)
        {
            DataTable dt = updateadjustment.GetData("SELECT * FROM tbladjustment WHERE products='" + product.Replace("'", "''") + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                cmbproduct.Text = row["products"].ToString();
                txtquantity.Text = row["quantity"].ToString();
                txtreason.Text = row["reason"].ToString();

                originalProduct = cmbproduct.Text;
            }
        }

        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // Validate product
            if (cmbproduct.SelectedIndex == -1 || string.IsNullOrEmpty(cmbproduct.Text.Trim()))
            {
                errorProvider1.SetError(cmbproduct, "Product name is required.");
                errorcount++;
            }

            // Validate quantity
            if (string.IsNullOrEmpty(txtquantity.Text.Trim()))
            {
                errorProvider1.SetError(txtquantity, "Quantity is required.");
                errorcount++;
            }
            else
            {
                int qty;
                if (!int.TryParse(txtquantity.Text, out qty) || qty < 1)
                {
                    errorProvider1.SetError(txtquantity, "Quantity must be a valid number greater than or equal to 1.");
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
                    DialogResult dr = MessageBox.Show("Are you sure you want to update this adjustment?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string product = cmbproduct.Text.Replace("'", "''");
                        string quantity = txtquantity.Text.Replace("'", "''");
                        string reason = txtreason.Text.Replace("'", "''");
                        string user = string.IsNullOrEmpty(username) ? "" : username.Replace("'", "''");

                        string sql =
                            "UPDATE tbladjustment SET " +
                            "products='" + product + "', " +
                            "quantity='" + quantity + "', " +
                            "reason='" + reason + "', " +
                            "createdby='" + user + "' " +
                            "WHERE products='" + originalProduct.Replace("'", "''") + "'";

                        updateadjustment.executeSQL(sql);

                        if (updateadjustment.rowAffected > 0)
                        {
                            MessageBox.Show("Adjustment updated successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            updateadjustment.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" +
                                DateTime.Now.ToString("MM/dd/yyyy") + "' , '" + DateTime.Now.ToShortTimeString() +
                                "' , 'UPDATE', 'ADJUSTMENT MANAGEMENT', '" + product + "', '" + username + "')");

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

        private void cmbproduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbproduct.Text))
                errorProvider1.SetError(cmbproduct, "");
        }

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");
        }

        private void txtreason_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtreason.Text))
                errorProvider1.SetError(txtreason, "");
        }
    }
}
