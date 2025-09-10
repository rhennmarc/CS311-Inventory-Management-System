using inventory_management;
using System;
using System.Data;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmProducts : Form
    {
        private string username;
        private int row;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;

        Class1 products = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmProducts(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts(string search = "")
        {
            try
            {
                string query = "SELECT * FROM tblproducts ";
                if (!string.IsNullOrEmpty(search))
                {
                    query += "WHERE products LIKE '%" + search + "%' OR description LIKE '%" + search + "%' ";
                }
                query += "ORDER BY products";

                DataTable dtAll = products.GetData(query);

                totalRecords = dtAll.Rows.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Apply paging
                DataTable dtPage = dtAll.Clone();
                int startIndex = (currentPage - 1) * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalRecords);

                for (int i = startIndex; i < endIndex; i++)
                {
                    dtPage.ImportRow(dtAll.Rows[i]);
                }

                dataGridView1.DataSource = dtPage;

                lblPageInfo.Text = $"Page {currentPage} of {totalPages}";
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on LoadProducts", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadProducts();
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            
            frmAddProduct addProductForm = new frmAddProduct(username);
            addProductForm.FormClosed += (s, args) => { LoadProducts(); };
            addProductForm.Show();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a product to update.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int row = dataGridView1.SelectedRows[0].Index;

                string productname = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string description = dataGridView1.Rows[row].Cells["description"].Value.ToString();
                string unitprice = dataGridView1.Rows[row].Cells["unitprice"].Value.ToString();
                string currentstock = dataGridView1.Rows[row].Cells["currentstock"].Value.ToString();

                frmUpdateProduct updateProductForm = new frmUpdateProduct(productname, description, unitprice, currentstock, username);
                updateProductForm.FormClosed += (s, args) => { LoadProducts(); };
                updateProductForm.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnupdate_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a valid product record first.",
                        "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string productname = dataGridView1.Rows[row].Cells["products"].Value.ToString();

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this product?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    products.executeSQL("DELETE FROM tblproducts WHERE products = '" + productname + "'");

                    if (products.rowAffected > 0)
                    {
                        MessageBox.Show("Product deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log deletion
                        products.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("dd/MM/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE', 'PRODUCTS MANAGEMENT', '" + productname + "', '" + username + "')");
                    }

                    LoadProducts();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndelete_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Pagination buttons
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadProducts(txtsearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadProducts(txtsearch.Text.Trim());
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                row = (int)e.RowIndex;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on dataGridView1_CellClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadProducts(txtsearch.Text.Trim());
        }

        private void txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnsearch_Click(sender, e);
            }
        }
    }
}
