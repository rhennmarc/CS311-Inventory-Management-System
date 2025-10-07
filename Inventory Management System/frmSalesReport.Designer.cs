namespace Inventory_Management_System
{
    partial class frmSalesReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.txtoveralltotal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtrefunds = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txttotal = new System.Windows.Forms.TextBox();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.actionPanel = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPrevRefund = new System.Windows.Forms.Button();
            this.btnNextRefund = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPrevSales = new System.Windows.Forms.Button();
            this.btnNextSales = new System.Windows.Forms.Button();
            this.actionGroupBox = new System.Windows.Forms.GroupBox();
            this.btnrefresh = new System.Windows.Forms.Button();
            this.btndeleteall = new System.Windows.Forms.Button();
            this.btndelete = new System.Windows.Forms.Button();
            this.btnrefund = new System.Windows.Forms.Button();
            this.btnview = new System.Windows.Forms.Button();
            this.btnexport = new System.Windows.Forms.Button();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnsearch = new System.Windows.Forms.Button();
            this.txtsearch = new System.Windows.Forms.TextBox();
            this.cmbduration = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.actionPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.actionGroupBox.SuspendLayout();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.mainPanel.Controls.Add(this.contentPanel);
            this.mainPanel.Controls.Add(this.actionPanel);
            this.mainPanel.Controls.Add(this.searchPanel);
            this.mainPanel.Controls.Add(this.headerPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(20);
            this.mainPanel.Size = new System.Drawing.Size(1492, 737);
            this.mainPanel.TabIndex = 3;
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.label8);
            this.contentPanel.Controls.Add(this.label7);
            this.contentPanel.Controls.Add(this.label6);
            this.contentPanel.Controls.Add(this.dataGridView2);
            this.contentPanel.Controls.Add(this.label5);
            this.contentPanel.Controls.Add(this.txtoveralltotal);
            this.contentPanel.Controls.Add(this.label4);
            this.contentPanel.Controls.Add(this.txtrefunds);
            this.contentPanel.Controls.Add(this.label1);
            this.contentPanel.Controls.Add(this.txttotal);
            this.contentPanel.Controls.Add(this.lblPageInfo);
            this.contentPanel.Controls.Add(this.dataGridView1);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(20, 160);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(0, 20, 200, 20);
            this.contentPanel.Size = new System.Drawing.Size(1270, 557);
            this.contentPanel.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(905, 516);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Page 1 of 1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label7.Location = new System.Drawing.Point(757, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 37);
            this.label7.TabIndex = 13;
            this.label7.Text = "Refunds";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label6.Location = new System.Drawing.Point(207, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 37);
            this.label6.TabIndex = 12;
            this.label6.Text = "Sales";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersHeight = 40;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dataGridView2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView2.EnableHeadersVisualStyles = false;
            this.dataGridView2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dataGridView2.Location = new System.Drawing.Point(525, 88);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 35;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(536, 425);
            this.dataGridView2.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label5.Location = new System.Drawing.Point(1128, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 19);
            this.label5.TabIndex = 10;
            this.label5.Text = "Overall Total";
            // 
            // txtoveralltotal
            // 
            this.txtoveralltotal.Enabled = false;
            this.txtoveralltotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtoveralltotal.Location = new System.Drawing.Point(1071, 177);
            this.txtoveralltotal.Name = "txtoveralltotal";
            this.txtoveralltotal.Size = new System.Drawing.Size(200, 31);
            this.txtoveralltotal.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label4.Location = new System.Drawing.Point(1124, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Total Refunds";
            // 
            // txtrefunds
            // 
            this.txtrefunds.Enabled = false;
            this.txtrefunds.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtrefunds.Location = new System.Drawing.Point(1071, 256);
            this.txtrefunds.Name = "txtrefunds";
            this.txtrefunds.Size = new System.Drawing.Size(200, 31);
            this.txtrefunds.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label1.Location = new System.Drawing.Point(1133, 314);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "Total Sales";
            // 
            // txttotal
            // 
            this.txttotal.Enabled = false;
            this.txttotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttotal.Location = new System.Drawing.Point(1071, 336);
            this.txttotal.Name = "txttotal";
            this.txttotal.Size = new System.Drawing.Size(200, 31);
            this.txttotal.TabIndex = 5;
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(363, 516);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(62, 13);
            this.lblPageInfo.TabIndex = 1;
            this.lblPageInfo.Text = "Page 1 of 1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeight = 40;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dataGridView1.Location = new System.Drawing.Point(3, 88);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 35;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(516, 425);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // actionPanel
            // 
            this.actionPanel.Controls.Add(this.groupBox2);
            this.actionPanel.Controls.Add(this.groupBox1);
            this.actionPanel.Controls.Add(this.actionGroupBox);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.actionPanel.Location = new System.Drawing.Point(1290, 160);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Padding = new System.Windows.Forms.Padding(20);
            this.actionPanel.Size = new System.Drawing.Size(182, 557);
            this.actionPanel.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnPrevRefund);
            this.groupBox2.Controls.Add(this.btnNextRefund);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBox2.Location = new System.Drawing.Point(20, 424);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(142, 125);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Refund Page";
            // 
            // btnPrevRefund
            // 
            this.btnPrevRefund.BackColor = System.Drawing.Color.DimGray;
            this.btnPrevRefund.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevRefund.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPrevRefund.FlatAppearance.BorderSize = 0;
            this.btnPrevRefund.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrevRefund.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevRefund.ForeColor = System.Drawing.Color.White;
            this.btnPrevRefund.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrevRefund.ImageIndex = 1;
            this.btnPrevRefund.Location = new System.Drawing.Point(10, 68);
            this.btnPrevRefund.Name = "btnPrevRefund";
            this.btnPrevRefund.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnPrevRefund.Size = new System.Drawing.Size(122, 40);
            this.btnPrevRefund.TabIndex = 1;
            this.btnPrevRefund.Text = "&Prev";
            this.btnPrevRefund.UseVisualStyleBackColor = false;
            this.btnPrevRefund.Click += new System.EventHandler(this.btnPrevRefund_Click);
            // 
            // btnNextRefund
            // 
            this.btnNextRefund.BackColor = System.Drawing.Color.DimGray;
            this.btnNextRefund.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextRefund.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNextRefund.FlatAppearance.BorderSize = 0;
            this.btnNextRefund.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNextRefund.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextRefund.ForeColor = System.Drawing.Color.White;
            this.btnNextRefund.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNextRefund.ImageIndex = 0;
            this.btnNextRefund.Location = new System.Drawing.Point(10, 28);
            this.btnNextRefund.Name = "btnNextRefund";
            this.btnNextRefund.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnNextRefund.Size = new System.Drawing.Size(122, 40);
            this.btnNextRefund.TabIndex = 0;
            this.btnNextRefund.Text = "&Next";
            this.btnNextRefund.UseVisualStyleBackColor = false;
            this.btnNextRefund.Click += new System.EventHandler(this.btnNextRefund_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnPrevSales);
            this.groupBox1.Controls.Add(this.btnNextSales);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBox1.Location = new System.Drawing.Point(20, 299);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(142, 125);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sales Page";
            // 
            // btnPrevSales
            // 
            this.btnPrevSales.BackColor = System.Drawing.Color.DimGray;
            this.btnPrevSales.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevSales.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPrevSales.FlatAppearance.BorderSize = 0;
            this.btnPrevSales.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrevSales.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrevSales.ForeColor = System.Drawing.Color.White;
            this.btnPrevSales.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrevSales.ImageIndex = 1;
            this.btnPrevSales.Location = new System.Drawing.Point(10, 68);
            this.btnPrevSales.Name = "btnPrevSales";
            this.btnPrevSales.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnPrevSales.Size = new System.Drawing.Size(122, 40);
            this.btnPrevSales.TabIndex = 1;
            this.btnPrevSales.Text = "&Prev";
            this.btnPrevSales.UseVisualStyleBackColor = false;
            this.btnPrevSales.Click += new System.EventHandler(this.btnPrevSales_Click);
            // 
            // btnNextSales
            // 
            this.btnNextSales.BackColor = System.Drawing.Color.DimGray;
            this.btnNextSales.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextSales.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNextSales.FlatAppearance.BorderSize = 0;
            this.btnNextSales.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNextSales.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNextSales.ForeColor = System.Drawing.Color.White;
            this.btnNextSales.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNextSales.ImageIndex = 0;
            this.btnNextSales.Location = new System.Drawing.Point(10, 28);
            this.btnNextSales.Name = "btnNextSales";
            this.btnNextSales.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnNextSales.Size = new System.Drawing.Size(122, 40);
            this.btnNextSales.TabIndex = 0;
            this.btnNextSales.Text = "&Next";
            this.btnNextSales.UseVisualStyleBackColor = false;
            this.btnNextSales.Click += new System.EventHandler(this.btnNextSales_Click);
            // 
            // actionGroupBox
            // 
            this.actionGroupBox.Controls.Add(this.btnrefresh);
            this.actionGroupBox.Controls.Add(this.btndeleteall);
            this.actionGroupBox.Controls.Add(this.btndelete);
            this.actionGroupBox.Controls.Add(this.btnrefund);
            this.actionGroupBox.Controls.Add(this.btnview);
            this.actionGroupBox.Controls.Add(this.btnexport);
            this.actionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionGroupBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.actionGroupBox.Location = new System.Drawing.Point(20, 20);
            this.actionGroupBox.Name = "actionGroupBox";
            this.actionGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.actionGroupBox.Size = new System.Drawing.Size(142, 279);
            this.actionGroupBox.TabIndex = 0;
            this.actionGroupBox.TabStop = false;
            this.actionGroupBox.Text = "Actions";
            // 
            // btnrefresh
            // 
            this.btnrefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnrefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnrefresh.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnrefresh.FlatAppearance.BorderSize = 0;
            this.btnrefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnrefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnrefresh.ForeColor = System.Drawing.Color.White;
            this.btnrefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnrefresh.ImageIndex = 6;
            this.btnrefresh.Location = new System.Drawing.Point(10, 228);
            this.btnrefresh.Name = "btnrefresh";
            this.btnrefresh.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnrefresh.Size = new System.Drawing.Size(122, 40);
            this.btnrefresh.TabIndex = 3;
            this.btnrefresh.Text = "&Refresh";
            this.btnrefresh.UseVisualStyleBackColor = false;
            this.btnrefresh.Click += new System.EventHandler(this.btnrefresh_Click);
            // 
            // btndeleteall
            // 
            this.btndeleteall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btndeleteall.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeleteall.Dock = System.Windows.Forms.DockStyle.Top;
            this.btndeleteall.FlatAppearance.BorderSize = 0;
            this.btndeleteall.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btndeleteall.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btndeleteall.ForeColor = System.Drawing.Color.White;
            this.btndeleteall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btndeleteall.ImageIndex = 5;
            this.btndeleteall.Location = new System.Drawing.Point(10, 188);
            this.btndeleteall.Name = "btndeleteall";
            this.btndeleteall.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btndeleteall.Size = new System.Drawing.Size(122, 40);
            this.btndeleteall.TabIndex = 2;
            this.btndeleteall.Text = "Delete &All";
            this.btndeleteall.UseVisualStyleBackColor = false;
            this.btndeleteall.Click += new System.EventHandler(this.btndeleteall_Click);
            // 
            // btndelete
            // 
            this.btndelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btndelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndelete.Dock = System.Windows.Forms.DockStyle.Top;
            this.btndelete.FlatAppearance.BorderSize = 0;
            this.btndelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btndelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btndelete.ForeColor = System.Drawing.Color.White;
            this.btndelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btndelete.ImageIndex = 1;
            this.btndelete.Location = new System.Drawing.Point(10, 148);
            this.btndelete.Name = "btndelete";
            this.btndelete.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btndelete.Size = new System.Drawing.Size(122, 40);
            this.btndelete.TabIndex = 1;
            this.btndelete.Text = "&Delete";
            this.btndelete.UseVisualStyleBackColor = false;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // btnrefund
            // 
            this.btnrefund.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnrefund.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnrefund.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnrefund.FlatAppearance.BorderSize = 0;
            this.btnrefund.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnrefund.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnrefund.ForeColor = System.Drawing.Color.White;
            this.btnrefund.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnrefund.ImageIndex = 6;
            this.btnrefund.Location = new System.Drawing.Point(10, 108);
            this.btnrefund.Name = "btnrefund";
            this.btnrefund.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnrefund.Size = new System.Drawing.Size(122, 40);
            this.btnrefund.TabIndex = 6;
            this.btnrefund.Text = "&Refund";
            this.btnrefund.UseVisualStyleBackColor = false;
            this.btnrefund.Click += new System.EventHandler(this.btnrefund_Click);
            // 
            // btnview
            // 
            this.btnview.BackColor = System.Drawing.Color.DimGray;
            this.btnview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnview.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnview.FlatAppearance.BorderSize = 0;
            this.btnview.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnview.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnview.ForeColor = System.Drawing.Color.White;
            this.btnview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnview.ImageIndex = 0;
            this.btnview.Location = new System.Drawing.Point(10, 68);
            this.btnview.Name = "btnview";
            this.btnview.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnview.Size = new System.Drawing.Size(122, 40);
            this.btnview.TabIndex = 4;
            this.btnview.Text = "&View Receipt";
            this.btnview.UseVisualStyleBackColor = false;
            this.btnview.Click += new System.EventHandler(this.btnview_Click);
            // 
            // btnexport
            // 
            this.btnexport.BackColor = System.Drawing.Color.Green;
            this.btnexport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnexport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnexport.FlatAppearance.BorderSize = 0;
            this.btnexport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnexport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnexport.ForeColor = System.Drawing.Color.White;
            this.btnexport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnexport.ImageIndex = 0;
            this.btnexport.Location = new System.Drawing.Point(10, 28);
            this.btnexport.Name = "btnexport";
            this.btnexport.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnexport.Size = new System.Drawing.Size(122, 40);
            this.btnexport.TabIndex = 5;
            this.btnexport.Text = "&Export";
            this.btnexport.UseVisualStyleBackColor = false;
            this.btnexport.Click += new System.EventHandler(this.btnexport_Click);
            // 
            // searchPanel
            // 
            this.searchPanel.BackColor = System.Drawing.Color.White;
            this.searchPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchPanel.Controls.Add(this.label3);
            this.searchPanel.Controls.Add(this.btnsearch);
            this.searchPanel.Controls.Add(this.txtsearch);
            this.searchPanel.Controls.Add(this.cmbduration);
            this.searchPanel.Controls.Add(this.label2);
            this.searchPanel.Controls.Add(this.dateTimePicker1);
            this.searchPanel.Controls.Add(this.lblSearch);
            this.searchPanel.Controls.Add(this.pictureBox1);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(20, 80);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Padding = new System.Windows.Forms.Padding(20);
            this.searchPanel.Size = new System.Drawing.Size(1452, 80);
            this.searchPanel.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label3.Location = new System.Drawing.Point(819, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "Duration:";
            // 
            // btnsearch
            // 
            this.btnsearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnsearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnsearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsearch.FlatAppearance.BorderSize = 0;
            this.btnsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnsearch.ForeColor = System.Drawing.Color.White;
            this.btnsearch.Location = new System.Drawing.Point(651, 27);
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.Size = new System.Drawing.Size(110, 35);
            this.btnsearch.TabIndex = 7;
            this.btnsearch.Text = "&Search";
            this.btnsearch.UseVisualStyleBackColor = false;
            this.btnsearch.Click += new System.EventHandler(this.btnsearch_Click);
            // 
            // txtsearch
            // 
            this.txtsearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtsearch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsearch.Location = new System.Drawing.Point(135, 29);
            this.txtsearch.Name = "txtsearch";
            this.txtsearch.Size = new System.Drawing.Size(510, 27);
            this.txtsearch.TabIndex = 6;
            // 
            // cmbduration
            // 
            this.cmbduration.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbduration.FormattingEnabled = true;
            this.cmbduration.Location = new System.Drawing.Point(896, 28);
            this.cmbduration.Name = "cmbduration";
            this.cmbduration.Size = new System.Drawing.Size(181, 28);
            this.cmbduration.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label2.Location = new System.Drawing.Point(71, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "Search:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(1171, 29);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(250, 27);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblSearch.Location = new System.Drawing.Point(1121, 33);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(44, 19);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Date:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(20, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(20, 20);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1452, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Sales Report";
            // 
            // frmSalesReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1492, 737);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSalesReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Report";
            this.Load += new System.EventHandler(this.frmSalesReport_Load);
            this.mainPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.actionPanel.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.actionGroupBox.ResumeLayout(false);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPrevSales;
        private System.Windows.Forms.Button btnNextSales;
        private System.Windows.Forms.GroupBox actionGroupBox;
        private System.Windows.Forms.Button btnrefresh;
        private System.Windows.Forms.Button btndeleteall;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txttotal;
        private System.Windows.Forms.Button btnsearch;
        private System.Windows.Forms.TextBox txtsearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnview;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbduration;
        private System.Windows.Forms.Button btnexport;
        private System.Windows.Forms.Button btnrefund;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtoveralltotal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtrefunds;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnPrevRefund;
        private System.Windows.Forms.Button btnNextRefund;
        private System.Windows.Forms.Label label8;
    }
}