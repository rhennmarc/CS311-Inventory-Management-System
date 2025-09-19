using inventory_management;
using System;
using System.Data;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmLogs : Form
    {
        private string username;
        private int row;
        private int currentPage = 1;
        private int pageSize = 5;   // rows per page
        private int totalRecords = 0;
        private int totalPages = 0;

        Class1 logs = new Class1("127.0.0.1", "inventory_management", "rhennmarc", "mercado");

        public frmLogs(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void frmLogs_Load(object sender, EventArgs e)
        {
            LoadLogs();
        }

        private void LoadLogs(string where = "")
        {
            try
            {
                string query = "SELECT datelog, timelog, module, action, performedto, performedby FROM tbllogs ";
                if (!string.IsNullOrEmpty(where))
                {
                    query += where;
                }
                query += " ORDER BY datelog DESC, timelog DESC";

                DataTable dtAll = logs.GetData(query);

                totalRecords = dtAll.Rows.Count;
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                if (totalPages == 0) totalPages = 1;

                // Apply paging
                DataTable dtPage = dtAll.Clone();
                int startIndex = (currentPage - 1) * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalRecords);

                for (int i = startIndex; i < endIndex; i++)
                {
                    dtPage.ImportRow(dtAll.Rows[i]);
                }

                dataGridView1.DataSource = dtPage;

                // === Table Styling ===
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView1.DefaultCellStyle.Padding = new Padding(4);
                dataGridView1.RowTemplate.Height = 28;
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                // === Rename & Format Columns ===
                if (dataGridView1.Columns.Contains("datelog"))
                {
                    dataGridView1.Columns["datelog"].HeaderText = "Date";
                    dataGridView1.Columns["datelog"].DefaultCellStyle.Format = "MM-dd-yyyy";
                    dataGridView1.Columns["datelog"].Width = 100;
                }
                if (dataGridView1.Columns.Contains("timelog"))
                {
                    dataGridView1.Columns["timelog"].HeaderText = "Time";
                    dataGridView1.Columns["timelog"].Width = 80;
                }
                if (dataGridView1.Columns.Contains("module"))
                {
                    dataGridView1.Columns["module"].HeaderText = "Module";
                    dataGridView1.Columns["module"].Width = 120;
                }
                if (dataGridView1.Columns.Contains("action"))
                {
                    dataGridView1.Columns["action"].HeaderText = "Action";
                    dataGridView1.Columns["action"].Width = 120;
                }
                if (dataGridView1.Columns.Contains("performedto"))
                {
                    dataGridView1.Columns["performedto"].HeaderText = "Performed To";
                    dataGridView1.Columns["performedto"].Width = 150;
                }
                if (dataGridView1.Columns.Contains("performedby"))
                {
                    dataGridView1.Columns["performedby"].HeaderText = "Performed By";
                    dataGridView1.Columns["performedby"].Width = 150;
                }

                // === Page Info ===
                if (totalRecords == 0)
                {
                    lblPageInfo.Text = "No records found";
                    currentPage = 1;
                }
                else
                {
                    lblPageInfo.Text = $"Page {currentPage} of {totalPages} ({totalRecords} records)";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on LoadLogs", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                string where = "";
                if (!string.IsNullOrWhiteSpace(cmbaction.Text) && cmbaction.Text.ToUpperInvariant() != "ALL")
                {
                    string action = cmbaction.Text.Trim().ToUpperInvariant().Replace("'", "''");
                    where += "WHERE UPPER(action) = '" + action + "' ";
                }

                if (dateTimePicker1.Checked)
                {
                    string dmy = dateTimePicker1.Value.ToString("MM/dd/yyyy");
                    if (string.IsNullOrEmpty(where))
                        where = "WHERE datelog = '" + dmy + "' ";
                    else
                        where += "AND datelog = '" + dmy + "' ";
                }

                currentPage = 1;
                LoadLogs(where);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btnsearch_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            cmbaction.SelectedIndex = -1;
            dateTimePicker1.Checked = false;
            currentPage = 1;
            LoadLogs();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (row < 0 || row >= dataGridView1.Rows.Count)
                {
                    MessageBox.Show("Please select a log first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string datelog = dataGridView1.Rows[row].Cells["datelog"].Value.ToString();
                string timelog = dataGridView1.Rows[row].Cells["timelog"].Value.ToString();

                DialogResult dr = MessageBox.Show("Delete selected log?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    logs.executeSQL("DELETE FROM tbllogs WHERE datelog='" + datelog.Replace("'", "''") +
                                    "' AND timelog='" + timelog.Replace("'", "''") + "'");

                    if (logs.rowAffected > 0)
                    {
                        MessageBox.Show("Log deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadLogs();
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
                DialogResult dr = MessageBox.Show("Are you sure you want to delete ALL logs?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    logs.executeSQL("DELETE FROM tbllogs");
                    MessageBox.Show("All logs deleted.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLogs();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "ERROR on btndeleteall_Click", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Paging
        private void btnnext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadLogs();
            }
        }

        private void btnprev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadLogs();
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
    }
}
