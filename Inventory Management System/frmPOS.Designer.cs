namespace Inventory_Management_System
{
    partial class frmPOS
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
            this.label6 = new System.Windows.Forms.Label();
            this.txtchange = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtpayment = new System.Windows.Forms.TextBox();
            this.numquantity = new System.Windows.Forms.NumericUpDown();
            this.lblPageInfoProd = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvcart = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txttotal = new System.Windows.Forms.TextBox();
            this.lblPageInfoCart = new System.Windows.Forms.Label();
            this.dgvproducts = new System.Windows.Forms.DataGridView();
            this.actionPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btncartPrev = new System.Windows.Forms.Button();
            this.btncartNext = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnprodPrev = new System.Windows.Forms.Button();
            this.btnprodNext = new System.Windows.Forms.Button();
            this.actionGroupBox = new System.Windows.Forms.GroupBox();
            this.btnpurchase = new System.Windows.Forms.Button();
            this.btndiscount = new System.Windows.Forms.Button();
            this.btndeleteall = new System.Windows.Forms.Button();
            this.btndelete = new System.Windows.Forms.Button();
            this.btnadd = new System.Windows.Forms.Button();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.btnsearch = new System.Windows.Forms.Button();
            this.txtsearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numquantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvcart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvproducts)).BeginInit();
            this.actionPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.mainPanel.Size = new System.Drawing.Size(1328, 698);
            this.mainPanel.TabIndex = 4;
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.label6);
            this.contentPanel.Controls.Add(this.label5);
            this.contentPanel.Controls.Add(this.txtpayment);
            this.contentPanel.Controls.Add(this.txtchange);
            this.contentPanel.Controls.Add(this.numquantity);
            this.contentPanel.Controls.Add(this.lblPageInfoProd);
            this.contentPanel.Controls.Add(this.label4);
            this.contentPanel.Controls.Add(this.label3);
            this.contentPanel.Controls.Add(this.label2);
            this.contentPanel.Controls.Add(this.dgvcart);
            this.contentPanel.Controls.Add(this.label1);
            this.contentPanel.Controls.Add(this.txttotal);
            this.contentPanel.Controls.Add(this.lblPageInfoCart);
            this.contentPanel.Controls.Add(this.dgvproducts);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(20, 160);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(0, 20, 200, 20);
            this.contentPanel.Size = new System.Drawing.Size(1088, 518);
            this.contentPanel.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label6.Location = new System.Drawing.Point(997, 421);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 19);
            this.label6.TabIndex = 15;
            this.label6.Text = "Change";
            // 
            // txtchange
            // 
            this.txtchange.Enabled = false;
            this.txtchange.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtchange.Location = new System.Drawing.Point(966, 443);
            this.txtchange.Name = "txtchange";
            this.txtchange.Size = new System.Drawing.Size(115, 31);
            this.txtchange.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label5.Location = new System.Drawing.Point(991, 271);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 19);
            this.label5.TabIndex = 13;
            this.label5.Text = "Payment";
            // 
            // txtpayment
            // 
            this.txtpayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpayment.Location = new System.Drawing.Point(967, 293);
            this.txtpayment.Name = "txtpayment";
            this.txtpayment.Size = new System.Drawing.Size(115, 31);
            this.txtpayment.TabIndex = 12;
            // 
            // numquantity
            // 
            this.numquantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numquantity.Location = new System.Drawing.Point(966, 86);
            this.numquantity.Name = "numquantity";
            this.numquantity.Size = new System.Drawing.Size(115, 31);
            this.numquantity.TabIndex = 11;
            this.numquantity.ValueChanged += new System.EventHandler(this.numquantity_ValueChanged);
            // 
            // lblPageInfoProd
            // 
            this.lblPageInfoProd.AutoSize = true;
            this.lblPageInfoProd.Location = new System.Drawing.Point(365, 477);
            this.lblPageInfoProd.Name = "lblPageInfoProd";
            this.lblPageInfoProd.Size = new System.Drawing.Size(62, 13);
            this.lblPageInfoProd.TabIndex = 10;
            this.lblPageInfoProd.Text = "Page 1 of 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label4.Location = new System.Drawing.Point(689, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 37);
            this.label4.TabIndex = 9;
            this.label4.Text = "Cart";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label3.Location = new System.Drawing.Point(990, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Quantity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label2.Location = new System.Drawing.Point(175, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 37);
            this.label2.TabIndex = 6;
            this.label2.Text = "Products";
            // 
            // dgvcart
            // 
            this.dgvcart.AllowUserToAddRows = false;
            this.dgvcart.AllowUserToDeleteRows = false;
            this.dgvcart.BackgroundColor = System.Drawing.Color.White;
            this.dgvcart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvcart.ColumnHeadersHeight = 40;
            this.dgvcart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvcart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgvcart.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvcart.EnableHeadersVisualStyles = false;
            this.dgvcart.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvcart.Location = new System.Drawing.Point(487, 77);
            this.dgvcart.MultiSelect = false;
            this.dgvcart.Name = "dgvcart";
            this.dgvcart.ReadOnly = true;
            this.dgvcart.RowHeadersVisible = false;
            this.dgvcart.RowHeadersWidth = 51;
            this.dgvcart.RowTemplate.Height = 35;
            this.dgvcart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvcart.Size = new System.Drawing.Size(456, 397);
            this.dgvcart.TabIndex = 5;
            this.dgvcart.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvcart_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label1.Location = new System.Drawing.Point(1002, 344);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "Total";
            // 
            // txttotal
            // 
            this.txttotal.Enabled = false;
            this.txttotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttotal.Location = new System.Drawing.Point(966, 367);
            this.txttotal.Name = "txttotal";
            this.txttotal.Size = new System.Drawing.Size(115, 31);
            this.txttotal.TabIndex = 2;
            // 
            // lblPageInfoCart
            // 
            this.lblPageInfoCart.AutoSize = true;
            this.lblPageInfoCart.Location = new System.Drawing.Point(832, 477);
            this.lblPageInfoCart.Name = "lblPageInfoCart";
            this.lblPageInfoCart.Size = new System.Drawing.Size(62, 13);
            this.lblPageInfoCart.TabIndex = 1;
            this.lblPageInfoCart.Text = "Page 1 of 1";
            // 
            // dgvproducts
            // 
            this.dgvproducts.AllowUserToAddRows = false;
            this.dgvproducts.AllowUserToDeleteRows = false;
            this.dgvproducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvproducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvproducts.ColumnHeadersHeight = 40;
            this.dgvproducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvproducts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgvproducts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvproducts.EnableHeadersVisualStyles = false;
            this.dgvproducts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvproducts.Location = new System.Drawing.Point(25, 77);
            this.dgvproducts.MultiSelect = false;
            this.dgvproducts.Name = "dgvproducts";
            this.dgvproducts.ReadOnly = true;
            this.dgvproducts.RowHeadersVisible = false;
            this.dgvproducts.RowHeadersWidth = 51;
            this.dgvproducts.RowTemplate.Height = 35;
            this.dgvproducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvproducts.Size = new System.Drawing.Size(456, 397);
            this.dgvproducts.TabIndex = 0;
            this.dgvproducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvproducts_CellClick);
            // 
            // actionPanel
            // 
            this.actionPanel.Controls.Add(this.groupBox1);
            this.actionPanel.Controls.Add(this.groupBox2);
            this.actionPanel.Controls.Add(this.actionGroupBox);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.actionPanel.Location = new System.Drawing.Point(1108, 160);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Padding = new System.Windows.Forms.Padding(20);
            this.actionPanel.Size = new System.Drawing.Size(200, 518);
            this.actionPanel.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btncartPrev);
            this.groupBox1.Controls.Add(this.btncartNext);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBox1.Location = new System.Drawing.Point(20, 381);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(160, 116);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cart Page";
            // 
            // btncartPrev
            // 
            this.btncartPrev.BackColor = System.Drawing.Color.DimGray;
            this.btncartPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btncartPrev.Dock = System.Windows.Forms.DockStyle.Top;
            this.btncartPrev.FlatAppearance.BorderSize = 0;
            this.btncartPrev.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btncartPrev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncartPrev.ForeColor = System.Drawing.Color.White;
            this.btncartPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncartPrev.ImageIndex = 1;
            this.btncartPrev.Location = new System.Drawing.Point(10, 68);
            this.btncartPrev.Name = "btncartPrev";
            this.btncartPrev.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btncartPrev.Size = new System.Drawing.Size(140, 40);
            this.btncartPrev.TabIndex = 1;
            this.btncartPrev.Text = "Pre&v";
            this.btncartPrev.UseVisualStyleBackColor = false;
            this.btncartPrev.Click += new System.EventHandler(this.btncartPrev_Click);
            // 
            // btncartNext
            // 
            this.btncartNext.BackColor = System.Drawing.Color.DimGray;
            this.btncartNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btncartNext.Dock = System.Windows.Forms.DockStyle.Top;
            this.btncartNext.FlatAppearance.BorderSize = 0;
            this.btncartNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btncartNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncartNext.ForeColor = System.Drawing.Color.White;
            this.btncartNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncartNext.ImageIndex = 0;
            this.btncartNext.Location = new System.Drawing.Point(10, 28);
            this.btncartNext.Name = "btncartNext";
            this.btncartNext.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btncartNext.Size = new System.Drawing.Size(140, 40);
            this.btncartNext.TabIndex = 0;
            this.btncartNext.Text = "N&ext";
            this.btncartNext.UseVisualStyleBackColor = false;
            this.btncartNext.Click += new System.EventHandler(this.btncartNext_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnprodPrev);
            this.groupBox2.Controls.Add(this.btnprodNext);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.groupBox2.Location = new System.Drawing.Point(20, 265);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(160, 116);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Products Page";
            // 
            // btnprodPrev
            // 
            this.btnprodPrev.BackColor = System.Drawing.Color.DimGray;
            this.btnprodPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnprodPrev.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnprodPrev.FlatAppearance.BorderSize = 0;
            this.btnprodPrev.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnprodPrev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnprodPrev.ForeColor = System.Drawing.Color.White;
            this.btnprodPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnprodPrev.ImageIndex = 1;
            this.btnprodPrev.Location = new System.Drawing.Point(10, 68);
            this.btnprodPrev.Name = "btnprodPrev";
            this.btnprodPrev.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnprodPrev.Size = new System.Drawing.Size(140, 40);
            this.btnprodPrev.TabIndex = 1;
            this.btnprodPrev.Text = "P&rev";
            this.btnprodPrev.UseVisualStyleBackColor = false;
            this.btnprodPrev.Click += new System.EventHandler(this.btnprodPrev_Click);
            // 
            // btnprodNext
            // 
            this.btnprodNext.BackColor = System.Drawing.Color.DimGray;
            this.btnprodNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnprodNext.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnprodNext.FlatAppearance.BorderSize = 0;
            this.btnprodNext.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnprodNext.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnprodNext.ForeColor = System.Drawing.Color.White;
            this.btnprodNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnprodNext.ImageIndex = 0;
            this.btnprodNext.Location = new System.Drawing.Point(10, 28);
            this.btnprodNext.Name = "btnprodNext";
            this.btnprodNext.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnprodNext.Size = new System.Drawing.Size(140, 40);
            this.btnprodNext.TabIndex = 0;
            this.btnprodNext.Text = "&Next";
            this.btnprodNext.UseVisualStyleBackColor = false;
            this.btnprodNext.Click += new System.EventHandler(this.btnprodNext_Click);
            // 
            // actionGroupBox
            // 
            this.actionGroupBox.Controls.Add(this.btnpurchase);
            this.actionGroupBox.Controls.Add(this.btndiscount);
            this.actionGroupBox.Controls.Add(this.btndeleteall);
            this.actionGroupBox.Controls.Add(this.btndelete);
            this.actionGroupBox.Controls.Add(this.btnadd);
            this.actionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionGroupBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.actionGroupBox.Location = new System.Drawing.Point(20, 20);
            this.actionGroupBox.Name = "actionGroupBox";
            this.actionGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.actionGroupBox.Size = new System.Drawing.Size(160, 245);
            this.actionGroupBox.TabIndex = 0;
            this.actionGroupBox.TabStop = false;
            this.actionGroupBox.Text = "Actions";
            // 
            // btnpurchase
            // 
            this.btnpurchase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnpurchase.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnpurchase.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnpurchase.FlatAppearance.BorderSize = 0;
            this.btnpurchase.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnpurchase.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpurchase.ForeColor = System.Drawing.Color.White;
            this.btnpurchase.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnpurchase.ImageIndex = 0;
            this.btnpurchase.Location = new System.Drawing.Point(10, 188);
            this.btnpurchase.Name = "btnpurchase";
            this.btnpurchase.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnpurchase.Size = new System.Drawing.Size(140, 40);
            this.btnpurchase.TabIndex = 7;
            this.btnpurchase.Text = "&Purchase";
            this.btnpurchase.UseVisualStyleBackColor = false;
            this.btnpurchase.Click += new System.EventHandler(this.btnpurchase_Click);
            // 
            // btndiscount
            // 
            this.btndiscount.BackColor = System.Drawing.Color.Green;
            this.btndiscount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndiscount.Dock = System.Windows.Forms.DockStyle.Top;
            this.btndiscount.FlatAppearance.BorderSize = 0;
            this.btndiscount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btndiscount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btndiscount.ForeColor = System.Drawing.Color.White;
            this.btndiscount.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btndiscount.ImageIndex = 0;
            this.btndiscount.Location = new System.Drawing.Point(10, 148);
            this.btndiscount.Name = "btndiscount";
            this.btndiscount.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btndiscount.Size = new System.Drawing.Size(140, 40);
            this.btndiscount.TabIndex = 6;
            this.btndiscount.Text = "&Discount (20%)";
            this.btndiscount.UseVisualStyleBackColor = false;
            this.btndiscount.Click += new System.EventHandler(this.btndiscount_Click);
            // 
            // btndeleteall
            // 
            this.btndeleteall.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btndeleteall.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeleteall.Dock = System.Windows.Forms.DockStyle.Top;
            this.btndeleteall.FlatAppearance.BorderSize = 0;
            this.btndeleteall.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btndeleteall.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btndeleteall.ForeColor = System.Drawing.Color.White;
            this.btndeleteall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btndeleteall.ImageIndex = 5;
            this.btndeleteall.Location = new System.Drawing.Point(10, 108);
            this.btndeleteall.Name = "btndeleteall";
            this.btndeleteall.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btndeleteall.Size = new System.Drawing.Size(140, 40);
            this.btndeleteall.TabIndex = 4;
            this.btndeleteall.Text = "Re&move All";
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
            this.btndelete.ImageIndex = 5;
            this.btndelete.Location = new System.Drawing.Point(10, 68);
            this.btndelete.Name = "btndelete";
            this.btndelete.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btndelete.Size = new System.Drawing.Size(140, 40);
            this.btndelete.TabIndex = 2;
            this.btndelete.Text = "&Remove";
            this.btndelete.UseVisualStyleBackColor = false;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // btnadd
            // 
            this.btnadd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnadd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnadd.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnadd.FlatAppearance.BorderSize = 0;
            this.btnadd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnadd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnadd.ForeColor = System.Drawing.Color.White;
            this.btnadd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnadd.ImageIndex = 0;
            this.btnadd.Location = new System.Drawing.Point(10, 28);
            this.btnadd.Name = "btnadd";
            this.btnadd.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnadd.Size = new System.Drawing.Size(140, 40);
            this.btnadd.TabIndex = 0;
            this.btnadd.Text = "&Add";
            this.btnadd.UseVisualStyleBackColor = false;
            this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
            // 
            // searchPanel
            // 
            this.searchPanel.BackColor = System.Drawing.Color.White;
            this.searchPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchPanel.Controls.Add(this.btnsearch);
            this.searchPanel.Controls.Add(this.txtsearch);
            this.searchPanel.Controls.Add(this.lblSearch);
            this.searchPanel.Controls.Add(this.pictureBox1);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(20, 80);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Padding = new System.Windows.Forms.Padding(20);
            this.searchPanel.Size = new System.Drawing.Size(1288, 80);
            this.searchPanel.TabIndex = 1;
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
            this.btnsearch.Location = new System.Drawing.Point(1148, 25);
            this.btnsearch.Name = "btnsearch";
            this.btnsearch.Size = new System.Drawing.Size(110, 35);
            this.btnsearch.TabIndex = 2;
            this.btnsearch.Text = "&Search";
            this.btnsearch.UseVisualStyleBackColor = false;
            this.btnsearch.Click += new System.EventHandler(this.btnsearch_Click);
            // 
            // txtsearch
            // 
            this.txtsearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtsearch.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsearch.Location = new System.Drawing.Point(150, 28);
            this.txtsearch.Name = "txtsearch";
            this.txtsearch.Size = new System.Drawing.Size(983, 27);
            this.txtsearch.TabIndex = 1;
            this.txtsearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtsearch_KeyPress);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblSearch.Location = new System.Drawing.Point(75, 32);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(58, 19);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search:";
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
            this.headerPanel.Size = new System.Drawing.Size(1288, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(60, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "POS";
            // 
            // frmPOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1328, 698);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmPOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POS";
            this.Load += new System.EventHandler(this.frmPOS_Load);
            this.mainPanel.ResumeLayout(false);
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numquantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvcart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvproducts)).EndInit();
            this.actionPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvcart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txttotal;
        private System.Windows.Forms.Label lblPageInfoCart;
        private System.Windows.Forms.DataGridView dgvproducts;
        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnprodPrev;
        private System.Windows.Forms.Button btnprodNext;
        private System.Windows.Forms.GroupBox actionGroupBox;
        private System.Windows.Forms.Button btnadd;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Button btnsearch;
        private System.Windows.Forms.TextBox txtsearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPageInfoProd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btncartPrev;
        private System.Windows.Forms.Button btncartNext;
        private System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.Button btndeleteall;
        private System.Windows.Forms.Button btndiscount;
        private System.Windows.Forms.Button btnpurchase;
        private System.Windows.Forms.NumericUpDown numquantity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtchange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtpayment;
    }
}