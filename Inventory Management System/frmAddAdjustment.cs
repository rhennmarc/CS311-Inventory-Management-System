using System;
using System.Data;
using System.Windows.Forms;
using inventory_management;

namespace Inventory_Management_System
{
    public partial class frmAddAdjustment : Form
    {
        private string username;

        public frmAddAdjustment(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        Class1 newAdjustment = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");
        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // --- VALIDATIONS ---

            // Product (required)
            if (string.IsNullOrEmpty(txtproduct.Text.Trim()))
            {
                errorProvider1.SetError(txtproduct, "Product name is required.");
                errorcount++;
            }

            // Quantity (required, numeric, >= 0)
            if (string.IsNullOrEmpty(txtquantity.Text.Trim()))
            {
                errorProvider1.SetError(txtquantity, "Quantity is required.");
                errorcount++;
            }
            else
            {
                int qty;
                if (!int.TryParse(txtquantity.Text.Trim(), out qty) || qty < 1)
                {
                    errorProvider1.SetError(txtquantity, "Quantity must be numeric and not less than 1.");
                    errorcount++;
                }
            }

            // Reason (required)
            if (string.IsNullOrEmpty(txtreason.Text.Trim()))
            {
                errorProvider1.SetError(txtreason, "Reason is required.");
                errorcount++;
            }

            // --- SAVE TO DATABASE ---
            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to add this adjustment?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        string product = txtproduct.Text.Trim().Replace("'", "''");
                        string quantity = txtquantity.Text.Trim().Replace("'", "''");
                        string reason = txtreason.Text.Trim().Replace("'", "''");
                        string createdBy = username.Replace("'", "''");
                        string dateAdjusted = DateTime.Now.ToString("MM/dd/yyyy");

                        string insertAdjustment =
                            "INSERT INTO tbladjustment (products, quantity, reason, createdby, dateadjusted) " +
                            "VALUES ('" + product + "', '" + quantity + "', '" + reason + "', '" + createdBy + "', '" + dateAdjusted + "')";

                        newAdjustment.executeSQL(insertAdjustment);

                        if (newAdjustment.rowAffected > 0)
                        {
                            MessageBox.Show("New adjustment added successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // log the action
                            newAdjustment.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                                "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'ADD', 'ADJUSTMENT MANAGEMENT', '" + product + "', '" + username + "')");

                            this.Close();
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on adding new adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");
        }

        private void txtreason_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtreason.Text))
                errorProvider1.SetError(txtreason, "");
        }
    }
}
