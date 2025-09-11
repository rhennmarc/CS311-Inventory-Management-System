using System;
using System.Data;
using System.Windows.Forms;
using inventory_management;

namespace Inventory_Management_System
{
    public partial class frmUpdateSupplier : Form
    {
        private string username;
        private string originalSupplier;

        public frmUpdateSupplier(string supplier, string description, string contactinfo, string username)
        {
            InitializeComponent();

            txtsupplier.Text = supplier;
            txtdescription.Text = description;
            txtcontactinfo.Text = contactinfo;

            originalSupplier = supplier;
            this.username = username;
        }

        Class1 updatesupplier = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        private void LoadSupplier(string supplier)
        {
            DataTable dt = updatesupplier.GetData("SELECT * FROM tblsupplier WHERE supplier='" + supplier.Replace("'", "''") + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                txtsupplier.Text = row["supplier"].ToString();
                txtdescription.Text = row["description"].ToString();
                txtcontactinfo.Text = row["contactinfo"].ToString();

                originalSupplier = txtsupplier.Text;
            }
        }

        private int errorcount;

        private void btnsave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            errorcount = 0;

            // Validation checks
            if (string.IsNullOrEmpty(txtsupplier.Text.Trim()))
            {
                errorProvider1.SetError(txtsupplier, "Supplier name is empty.");
                errorcount++;
            }

            if (string.IsNullOrEmpty(txtcontactinfo.Text.Trim()))
            {
                errorProvider1.SetError(txtcontactinfo, "Contact info is empty.");
                errorcount++;
            }

            // Check for duplicate supplier name if changed
            if (!string.IsNullOrEmpty(txtsupplier.Text.Trim()) && txtsupplier.Text.Trim() != originalSupplier)
            {
                DataTable dt = updatesupplier.GetData("SELECT * FROM tblsupplier WHERE supplier='" + txtsupplier.Text.Trim().Replace("'", "''") + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtsupplier, "Supplier already exists.");
                    errorcount++;
                }
            }

            if (errorcount == 0)
            {
                try
                {
                    DialogResult dr = MessageBox.Show("Are you sure you want to update this supplier?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (dr == DialogResult.Yes)
                    {
                        string supplier = txtsupplier.Text.Trim().Replace("'", "''");
                        string description = txtdescription.Text.Trim().Replace("'", "''");
                        string contactinfo = txtcontactinfo.Text.Trim().Replace("'", "''");
                        string user = string.IsNullOrEmpty(username) ? "" : username.Replace("'", "''");

                        string sql =
                            "UPDATE tblsupplier SET " +
                            "supplier='" + supplier + "', " +
                            "description='" + description + "', " +
                            "contactinfo='" + contactinfo + "' " +
                            "WHERE supplier='" + originalSupplier.Replace("'", "''") + "'";

                        updatesupplier.executeSQL(sql);

                        if (updatesupplier.rowAffected > 0)
                        {
                            MessageBox.Show("Supplier updated successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Insert into logs
                            updatesupplier.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) VALUES ('" +
                                DateTime.Now.ToString("MM/dd/yyyy") + "' , '" + DateTime.Now.ToShortTimeString() +
                                "' , 'UPDATE', 'SUPPLIER MANAGEMENT', '" + supplier + "', '" + username + "')");

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Please verify the Supplier Name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "ERROR on updating supplier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Clear errors when user starts typing
        private void txtsupplier_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtsupplier.Text))
                errorProvider1.SetError(txtsupplier, "");
        }

        private void txtcontactinfo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcontactinfo.Text))
                errorProvider1.SetError(txtcontactinfo, "");
        }

        private void frmUpdateSupplier_Load(object sender, EventArgs e)
        {
            this.ActiveControl = titleLabel;
        }
    }
}
