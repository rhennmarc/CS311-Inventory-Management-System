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
            if (cmbproduct.SelectedIndex == -1 || string.IsNullOrEmpty(cmbproduct.Text.Trim()))
            {
                errorProvider1.SetError(cmbproduct, "Product name is required.");
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
                        string product = cmbproduct.Text.Trim().Replace("'", "''");
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

        // --- LOAD COMBOBOX ---
        private void frmAddAdjustment_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = newAdjustment.GetData("SELECT products FROM tblproducts ORDER BY products ASC");
                cmbproduct.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cmbproduct.Items.Add(row["products"].ToString());
                }

                cmbproduct.DropDownStyle = ComboBoxStyle.DropDown;
                cmbproduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbproduct.AutoCompleteSource = AutoCompleteSource.ListItems;
                cmbproduct.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Remove error when user starts typing/selecting ---
        private void cmbproduct_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbproduct.Text))
                errorProvider1.SetError(cmbproduct, "");
        }

        private void txtquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtquantity.Text))
                errorProvider1.SetError(txtquantity, "");

            // Only allow digits
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtreason_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtreason.Text))
                errorProvider1.SetError(txtreason, "");
        }
    }
}
