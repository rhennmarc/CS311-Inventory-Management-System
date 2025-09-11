using System;
using System.Data;
using System.Windows.Forms;
using inventory_management;

namespace Inventory_Management_System
{
    public partial class frmAddSupplier : Form
    {
        private string username;
        public frmAddSupplier(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        Class1 newsupplier = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // --- VALIDATIONS ---

            // Supplier name (required + unique)
            if (string.IsNullOrEmpty(txtsupplier.Text.Trim()))
            {
                errorProvider1.SetError(txtsupplier, "Supplier name is required.");
                errorcount++;
            }
            else
            {
                DataTable dt = newsupplier.GetData("SELECT supplier FROM tblsupplier WHERE supplier='" + txtsupplier.Text.Trim().Replace("'", "''") + "' LIMIT 1");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtsupplier, "Supplier already exists.");
                    errorcount++;
                }
            }

            // Contact info (required)
            if (string.IsNullOrEmpty(txtcontactinfo.Text.Trim()))
            {
                errorProvider1.SetError(txtcontactinfo, "Contact info is required.");
                errorcount++;
            }

            // --- SAVE TO DATABASE ---
            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to add this supplier?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        string supplier = txtsupplier.Text.Trim().Replace("'", "''");
                        string description = txtdescription.Text.Trim().Replace("'", "''");
                        string contactinfo = txtcontactinfo.Text.Trim().Replace("'", "''");
                        string createdBy = username.Replace("'", "''");
                        string dateCreated = DateTime.Now.ToString("MM-dd-yyyy");

                        string insertSupplier =
                            "INSERT INTO tblsupplier (supplier, description, contactinfo, createdby, datecreated) " +
                            "VALUES ('" + supplier + "', '" + description + "', '" + contactinfo + "', '" + createdBy + "', '" + dateCreated + "')";

                        newsupplier.executeSQL(insertSupplier);

                        if (newsupplier.rowAffected > 0)
                        {
                            MessageBox.Show("New supplier added successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // log the action
                            newsupplier.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                                "VALUES ('" + DateTime.Now.ToString("MM-dd-yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                                "', 'ADD', 'SUPPLIER MANAGEMENT', '" + supplier + "', '" + username + "')");

                            this.Close();
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on adding new supplier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- Remove error when user starts typing ---
        private void txtsupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtsupplier.Text))
                errorProvider1.SetError(txtsupplier, "");
        }

        private void txtcontactinfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcontactinfo.Text))
                errorProvider1.SetError(txtcontactinfo, "");
        }
    }
}
