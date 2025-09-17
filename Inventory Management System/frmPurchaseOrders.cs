using inventory_management;
using System;
using System.Data;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmPurchaseOrders : Form
    {
        private string username;
        private string selectedSupplier;
        private int row = -1;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;

        Class1 purchaseOrders = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmPurchaseOrders(string username, string supplierName = "")
        {
            InitializeComponent();
            this.username = username;
            this.selectedSupplier = supplierName;

            // Set the title label to show selected supplier
            if (!string.IsNullOrEmpty(selectedSupplier))
            {
                lblTitle.Text = $"Purchase Orders - {selectedSupplier}";
            }
            else
            {
                lblTitle.Text = "Purchase Orders - All Suppliers";
            }
        }

        private void UpdateButtonStates()
        {
            // Enable/disable buttons based on whether a row is selected
            bool hasSelection = row >= 0 && row < dataGridView1.Rows.Count;

            // Assuming your View button is named btnView
            btnupdate.Enabled = hasSelection;
            btndelete.Enabled = hasSelection;
            btnreceive.Enabled = hasSelection;
        }

        private void frmPurchaseOrders_Load(object sender, EventArgs e)
        {
            // Subscribe to selection changed to keep `row` in sync
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            LoadPurchaseOrders();
            UpdateButtonStates();
        }

        private void LoadPurchaseOrders(string search = "")
        {
            try
            {
                string query = "SELECT products, quantity, unitcost, totalcost, status, createdby, datecreated, datereceived, supplier FROM tblpurchase_order ";

                // Build WHERE clause
                string whereClause = "";

                if (!string.IsNullOrEmpty(selectedSupplier))
                {
                    whereClause = "WHERE supplier = '" + selectedSupplier.Replace("'", "''") + "' ";
                }

                if (!string.IsNullOrEmpty(search))
                {
                    if (string.IsNullOrEmpty(whereClause))
                    {
                        whereClause = "WHERE ";
                    }
                    else
                    {
                        whereClause += "AND ";
                    }
                    whereClause += "(products LIKE '%" + search + "%' OR status LIKE '%" + search + "%' OR createdby LIKE '%" + search + "%') ";
                }

                query += whereClause + "ORDER BY datecreated DESC";

                DataTable dtAll = purchaseOrders.GetData(query);

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

                // Ensure no leftover selection and reset row
                dataGridView1.ClearSelection();
                row = -1;
                UpdateButtonStates();

                // === Table Formatting ===
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView1.RowTemplate.Height = 28;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;

                dataGridView1.DefaultCellStyle.Padding = new Padding(5);
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // === Column headers + widths ===
                if (dataGridView1.Columns.Contains("products"))
                {
                    dataGridView1.Columns["products"].HeaderText = "Products";
                    dataGridView1.Columns["products"].Width = 150;
                }
                if (dataGridView1.Columns.Contains("quantity"))
                {
                    dataGridView1.Columns["quantity"].HeaderText = "Quantity";
                    dataGridView1.Columns["quantity"].Width = 80;
                    dataGridView1.Columns["quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dataGridView1.Columns.Contains("unitcost"))
                {
                    dataGridView1.Columns["unitcost"].HeaderText = "Unit Cost";
                    dataGridView1.Columns["unitcost"].Width = 100;
                    dataGridView1.Columns["unitcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["unitcost"].DefaultCellStyle.Format = "C2";
                }
                if (dataGridView1.Columns.Contains("totalcost"))
                {
                    dataGridView1.Columns["totalcost"].HeaderText = "Total Cost";
                    dataGridView1.Columns["totalcost"].Width = 100;
                    dataGridView1.Columns["totalcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["totalcost"].DefaultCellStyle.Format = "C2";
                }
                if (dataGridView1.Columns.Contains("status"))
                {
                    dataGridView1.Columns["status"].HeaderText = "Status";
                    dataGridView1.Columns["status"].Width = 100;
                }
                if (dataGridView1.Columns.Contains("createdby"))
                {
                    dataGridView1.Columns["createdby"].HeaderText = "Created By";
                    dataGridView1.Columns["createdby"].Width = 120;
                }
                if (dataGridView1.Columns.Contains("datecreated"))
                {
                    dataGridView1.Columns["datecreated"].HeaderText = "Date Created";
                    dataGridView1.Columns["datecreated"].Width = 130;
                }
                if (dataGridView1.Columns.Contains("datereceived"))
                {
                    dataGridView1.Columns["datereceived"].HeaderText = "Date Received";
                    dataGridView1.Columns["datereceived"].Width = 130;
                }
                if (dataGridView1.Columns.Contains("supplier"))
                {
                    dataGridView1.Columns["supplier"].HeaderText = "Supplier";
                    dataGridView1.Columns["supplier"].Width = 120;
                    // Hide supplier column if we're viewing orders for a specific supplier
                    if (!string.IsNullOrEmpty(selectedSupplier))
                    {
                        dataGridView1.Columns["supplier"].Visible = false;
                    }
                }

                // Calculate and display total
                CalculateTotal();

                // === Page info ===
                if (totalRecords == 0)
                {
                    lblPageInfo.Text = "No records found";
                    currentPage = 1;
                }
                else
                {
                    lblPageInfo.Text = $"Page {currentPage} of {totalPages}";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on LoadPurchaseOrders", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateTotal()
        {
            try
            {
                decimal total = 0;

                // Calculate total from the current DataTable instead of running a separate query
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["totalcost"].Value != null && row.Cells["totalcost"].Value != DBNull.Value)
                    {
                        decimal rowTotal = 0;
                        if (decimal.TryParse(row.Cells["totalcost"].Value.ToString(), out rowTotal))
                        {
                            total += rowTotal;
                        }
                    }
                }

                txttotal.Text = "₱" + total.ToString("N2");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on CalculateTotal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txttotal.Text = "₱0.00";
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            try
            {
                frmAddPurchaseOrder addPOForm = new frmAddPurchaseOrder(username, selectedSupplier);
                addPOForm.FormClosed += (s, args) => { LoadPurchaseOrders(); };
                addPOForm.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnadd_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a purchase order to update.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int selectedRow = dataGridView1.SelectedRows[0].Index;

                string products = dataGridView1.Rows[selectedRow].Cells["products"].Value.ToString();
                string quantity = dataGridView1.Rows[selectedRow].Cells["quantity"].Value.ToString();
                string unitcost = dataGridView1.Rows[selectedRow].Cells["unitcost"].Value.ToString();

                frmUpdatePurchaseOrder updatePOForm = new frmUpdatePurchaseOrder(products, quantity, unitcost, username);
                updatePOForm.FormClosed += (s, args) => { LoadPurchaseOrders(); };
                updatePOForm.Show();
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
                    MessageBox.Show("Please select a valid purchase order record first.",
                        "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string products = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string supplier = dataGridView1.Columns.Contains("supplier") && dataGridView1.Columns["supplier"].Visible
                    ? dataGridView1.Rows[row].Cells["supplier"].Value.ToString()
                    : selectedSupplier;

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this purchase order?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    string deleteQuery = "DELETE FROM tblpurchase_order WHERE products = '" + products.Replace("'", "''") + "'";
                    if (!string.IsNullOrEmpty(supplier))
                    {
                        deleteQuery += " AND supplier = '" + supplier.Replace("'", "''") + "'";
                    }

                    purchaseOrders.executeSQL(deleteQuery);

                    if (purchaseOrders.rowAffected > 0)
                    {
                        MessageBox.Show("Purchase order deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log deletion
                        purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE', 'PURCHASE ORDER MANAGEMENT', '" + products + "', '" + username + "')");
                    }

                    LoadPurchaseOrders();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndelete_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btndeleteall_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ALL purchase orders? This action cannot be undone.",
                    "Delete All Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    string deleteQuery = "DELETE FROM tblpurchase_order";
                    if (!string.IsNullOrEmpty(selectedSupplier))
                    {
                        deleteQuery += " WHERE supplier = '" + selectedSupplier.Replace("'", "''") + "'";
                    }

                    purchaseOrders.executeSQL(deleteQuery);

                    if (purchaseOrders.rowAffected > 0)
                    {
                        MessageBox.Show($"{purchaseOrders.rowAffected} purchase order(s) deleted.", "Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Log deletion
                        purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'DELETE ALL', 'PURCHASE ORDER MANAGEMENT', 'ALL RECORDS', '" + username + "')");
                    }
                    else
                    {
                        MessageBox.Show("No records to delete.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadPurchaseOrders();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndeleteall_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            txtsearch.Text = "";
            LoadPurchaseOrders();
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Protect against header or out-of-range clicks
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                {
                    row = e.RowIndex;

                    // Make sure the clicked row is selected visually and set current cell
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[row].Selected = true;

                    if (dataGridView1.Columns.Count > 0 && dataGridView1.Rows[row].Cells.Count > 0)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[0];
                    }
                }
                else
                {
                    row = -1;
                }

                UpdateButtonStates();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on dataGridView1_CellClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Keep row in sync if selection changes (keyboard, mouse, etc.)
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell != null)
                {
                    int idx = dataGridView1.CurrentCell.RowIndex;
                    if (idx >= 0 && idx < dataGridView1.Rows.Count)
                    {
                        row = idx;
                    }
                    else
                    {
                        row = -1;
                    }
                }
                else
                {
                    row = -1;
                }
                UpdateButtonStates();
            }
            catch
            {
                // swallow to avoid selection change exceptions
                row = -1;
                UpdateButtonStates();
            }
        }

        private void txtsearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) // Enter key
            {
                currentPage = 1;
                LoadPurchaseOrders(txtsearch.Text.Trim());
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadPurchaseOrders(txtsearch.Text.Trim());
        }

        // Pagination methods
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPurchaseOrders(txtsearch.Text.Trim());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPurchaseOrders(txtsearch.Text.Trim());
            }
        }

        private void btnrecieveall_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Mark ALL purchase orders as received?", "Receive All Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    string pendingQuery = "SELECT products, quantity, unitcost, supplier FROM tblpurchase_order WHERE status != 'Received'";
                    if (!string.IsNullOrEmpty(selectedSupplier))
                        pendingQuery += " AND supplier = '" + selectedSupplier.Replace("'", "''") + "'";

                    DataTable dtPending = purchaseOrders.GetData(pendingQuery);

                    string updateQuery = "UPDATE tblpurchase_order SET status = 'Received', datereceived = '" + DateTime.Now.ToString("MM/dd/yyyy") + "' WHERE status != 'Received'";
                    if (!string.IsNullOrEmpty(selectedSupplier))
                        updateQuery += " AND supplier = '" + selectedSupplier.Replace("'", "''") + "'";

                    purchaseOrders.executeSQL(updateQuery);

                    if (purchaseOrders.rowAffected > 0)
                    {
                        foreach (DataRow drRow in dtPending.Rows)
                        {
                            string products = drRow["products"].ToString();
                            string quantity = drRow["quantity"].ToString();
                            string unitcost = drRow["unitcost"].ToString();

                            int qtyToAdd = 0;
                            int.TryParse(quantity, out qtyToAdd);

                            string selectProductQuery = "SELECT currentstock FROM tblproducts WHERE LOWER(products) = LOWER('" + products.Replace("'", "''") + "')";
                            DataTable dtProd = purchaseOrders.GetData(selectProductQuery);

                            if (dtProd.Rows.Count > 0)
                            {
                                int currentStock = 0;
                                int.TryParse(dtProd.Rows[0]["currentstock"].ToString(), out currentStock);
                                int newStock = currentStock + qtyToAdd;

                                string updateProd = "UPDATE tblproducts SET currentstock = '" + newStock + "' WHERE LOWER(products) = LOWER('" + products.Replace("'", "''") + "')";
                                purchaseOrders.executeSQL(updateProd);
                            }
                            else
                            {
                                string insertProd = "INSERT INTO tblproducts (products, description, unitprice, currentstock, createdby, datecreated) " +
                                    "VALUES ('" + products.Replace("'", "''") + "', '', '" + unitcost.Replace("'", "''") + "', '" + quantity.Replace("'", "''") + "', '" + username.Replace("'", "''") + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";
                                purchaseOrders.executeSQL(insertProd);
                            }
                        }

                        MessageBox.Show($"{purchaseOrders.rowAffected} purchase order(s) marked as received.", "Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'RECEIVE ALL', 'PURCHASE ORDER MANAGEMENT', 'ALL PENDING ORDERS', '" + username + "')");
                    }

                    LoadPurchaseOrders();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnrecieveall_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnreceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a purchase order to receive.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string products = dataGridView1.Rows[row].Cells["products"].Value.ToString();
                string currentStatus = dataGridView1.Rows[row].Cells["status"].Value.ToString();

                if (currentStatus.ToUpper() == "RECEIVED")
                {
                    MessageBox.Show("This purchase order has already been received.", "Already Received",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult dr = MessageBox.Show($"Mark purchase order for '{products}' as received?", "Receive Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    string updateQuery = "UPDATE tblpurchase_order SET status = 'Received', datereceived = '" + DateTime.Now.ToString("MM/dd/yyyy") + "' WHERE products = '" +
                        products.Replace("'", "''") + "'";
                    if (!string.IsNullOrEmpty(selectedSupplier))
                        updateQuery += " AND supplier = '" + selectedSupplier.Replace("'", "''") + "'";

                    purchaseOrders.executeSQL(updateQuery);

                    if (purchaseOrders.rowAffected > 0)
                    {
                        string quantity = dataGridView1.Rows[row].Cells["quantity"].Value.ToString();
                        string unitcost = dataGridView1.Rows[row].Cells["unitcost"].Value.ToString();

                        int qtyToAdd = 0;
                        int.TryParse(quantity, out qtyToAdd);

                        string selectProductQuery = "SELECT currentstock FROM tblproducts WHERE LOWER(products) = LOWER('" + products.Replace("'", "''") + "')";
                        DataTable dtProd = purchaseOrders.GetData(selectProductQuery);

                        if (dtProd.Rows.Count > 0)
                        {
                            int currentStock = 0;
                            int.TryParse(dtProd.Rows[0]["currentstock"].ToString(), out currentStock);
                            int newStock = currentStock + qtyToAdd;

                            string updateProd = "UPDATE tblproducts SET currentstock = '" + newStock + "' WHERE LOWER(products) = LOWER('" + products.Replace("'", "''") + "')";
                            purchaseOrders.executeSQL(updateProd);
                        }
                        else
                        {
                            string insertProd = "INSERT INTO tblproducts (products, description, unitprice, currentstock, createdby, datecreated) " +
                                "VALUES ('" + products.Replace("'", "''") + "', '', '" + unitcost.Replace("'", "''") + "', '" + quantity.Replace("'", "''") + "', '" + username.Replace("'", "''") + "', '" + DateTime.Now.ToString("MM/dd/yyyy") + "')";
                            purchaseOrders.executeSQL(insertProd);
                        }

                        MessageBox.Show("Purchase order marked as received.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        purchaseOrders.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, performedto, performedby) " +
                            "VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy") + "', '" + DateTime.Now.ToShortTimeString() +
                            "', 'RECEIVE', 'PURCHASE ORDER MANAGEMENT', '" + products + "', '" + username + "')");
                    }

                    LoadPurchaseOrders();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnreceive_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
