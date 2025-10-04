namespace Inventory_Management_System
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.topPanel = new System.Windows.Forms.Panel();
            this.btnToggleSidePanel = new System.Windows.Forms.Button();
            this.userMenuStrip = new System.Windows.Forms.MenuStrip();
            this.userDropdownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sidePanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnViewLogs = new System.Windows.Forms.Button();
            this.btnSales = new System.Windows.Forms.Button();
            this.btnAdjustments = new System.Windows.Forms.Button();
            this.btnSuppliers = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.btnPOS = new System.Windows.Forms.Button();
            this.lblPharmacyName = new System.Windows.Forms.Label();
            this.buttonAnimator = new System.Windows.Forms.Timer(this.components);
            this.sidePanelAnimator = new System.Windows.Forms.Timer(this.components);
            this.topPanel.SuspendLayout();
            this.userMenuStrip.SuspendLayout();
            this.sidePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.topPanel.Controls.Add(this.btnToggleSidePanel);
            this.topPanel.Controls.Add(this.userMenuStrip);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1200, 50);
            this.topPanel.TabIndex = 0;
            // 
            // btnToggleSidePanel
            // 
            this.btnToggleSidePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnToggleSidePanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToggleSidePanel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnToggleSidePanel.FlatAppearance.BorderSize = 2;
            this.btnToggleSidePanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnToggleSidePanel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnToggleSidePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleSidePanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnToggleSidePanel.ForeColor = System.Drawing.Color.White;
            this.btnToggleSidePanel.Location = new System.Drawing.Point(10, 10);
            this.btnToggleSidePanel.Name = "btnToggleSidePanel";
            this.btnToggleSidePanel.Size = new System.Drawing.Size(40, 30);
            this.btnToggleSidePanel.TabIndex = 1;
            this.btnToggleSidePanel.Text = "☰";
            this.btnToggleSidePanel.UseVisualStyleBackColor = false;
            this.btnToggleSidePanel.Click += new System.EventHandler(this.btnToggleSidePanel_Click);
            // 
            // userMenuStrip
            // 
            this.userMenuStrip.BackColor = System.Drawing.Color.Transparent;
            this.userMenuStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.userMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userDropdownMenuItem});
            this.userMenuStrip.Location = new System.Drawing.Point(1090, 0);
            this.userMenuStrip.Name = "userMenuStrip";
            this.userMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.userMenuStrip.Size = new System.Drawing.Size(110, 50);
            this.userMenuStrip.TabIndex = 0;
            // 
            // userDropdownMenuItem
            // 
            this.userDropdownMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.userDropdownMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.userDropdownMenuItem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.userDropdownMenuItem.ForeColor = System.Drawing.Color.White;
            this.userDropdownMenuItem.Image = global::Inventory_Management_System.Properties.Resources.user;
            this.userDropdownMenuItem.Name = "userDropdownMenuItem";
            this.userDropdownMenuItem.Size = new System.Drawing.Size(97, 23);
            this.userDropdownMenuItem.Text = "Username";
            // 
            // accountToolStripMenuItem
            // 
            this.accountToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.accountToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.accountToolStripMenuItem.Image = global::Inventory_Management_System.Properties.Resources.settings;
            this.accountToolStripMenuItem.Name = "accountToolStripMenuItem";
            this.accountToolStripMenuItem.Size = new System.Drawing.Size(230, 24);
            this.accountToolStripMenuItem.Text = "&Accounts Management";
            this.accountToolStripMenuItem.Click += new System.EventHandler(this.accountToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.logoutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.logoutToolStripMenuItem.Image = global::Inventory_Management_System.Properties.Resources.logout;
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(230, 24);
            this.logoutToolStripMenuItem.Text = "&Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // sidePanel
            // 
            this.sidePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.sidePanel.Controls.Add(this.pictureBox1);
            this.sidePanel.Controls.Add(this.btnViewLogs);
            this.sidePanel.Controls.Add(this.btnSales);
            this.sidePanel.Controls.Add(this.btnAdjustments);
            this.sidePanel.Controls.Add(this.btnSuppliers);
            this.sidePanel.Controls.Add(this.btnProducts);
            this.sidePanel.Controls.Add(this.btnPOS);
            this.sidePanel.Controls.Add(this.lblPharmacyName);
            this.sidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidePanel.Location = new System.Drawing.Point(0, 50);
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(220, 578);
            this.sidePanel.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Inventory_Management_System.Properties.Resources.AMGC3;
            this.pictureBox1.Location = new System.Drawing.Point(10, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 138);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // btnViewLogs
            // 
            this.btnViewLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnViewLogs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewLogs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnViewLogs.FlatAppearance.BorderSize = 2;
            this.btnViewLogs.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnViewLogs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnViewLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewLogs.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewLogs.ForeColor = System.Drawing.Color.White;
            this.btnViewLogs.Location = new System.Drawing.Point(10, 445);
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.Size = new System.Drawing.Size(200, 45);
            this.btnViewLogs.TabIndex = 8;
            this.btnViewLogs.Text = "&View Logs";
            this.btnViewLogs.UseVisualStyleBackColor = false;
            this.btnViewLogs.Click += new System.EventHandler(this.btnViewLogs_Click);
            this.btnViewLogs.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnViewLogs.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnSales
            // 
            this.btnSales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnSales.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSales.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnSales.FlatAppearance.BorderSize = 2;
            this.btnSales.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnSales.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSales.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSales.ForeColor = System.Drawing.Color.White;
            this.btnSales.Location = new System.Drawing.Point(10, 394);
            this.btnSales.Name = "btnSales";
            this.btnSales.Size = new System.Drawing.Size(200, 45);
            this.btnSales.TabIndex = 7;
            this.btnSales.Text = "Sal&es";
            this.btnSales.UseVisualStyleBackColor = false;
            this.btnSales.Click += new System.EventHandler(this.btnSales_Click);
            this.btnSales.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnSales.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnAdjustments
            // 
            this.btnAdjustments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnAdjustments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdjustments.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnAdjustments.FlatAppearance.BorderSize = 2;
            this.btnAdjustments.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnAdjustments.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnAdjustments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjustments.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdjustments.ForeColor = System.Drawing.Color.White;
            this.btnAdjustments.Location = new System.Drawing.Point(10, 343);
            this.btnAdjustments.Name = "btnAdjustments";
            this.btnAdjustments.Size = new System.Drawing.Size(200, 45);
            this.btnAdjustments.TabIndex = 6;
            this.btnAdjustments.Text = "A&djustments";
            this.btnAdjustments.UseVisualStyleBackColor = false;
            this.btnAdjustments.Click += new System.EventHandler(this.btnAdjustments_Click);
            this.btnAdjustments.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnAdjustments.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnSuppliers
            // 
            this.btnSuppliers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnSuppliers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSuppliers.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnSuppliers.FlatAppearance.BorderSize = 2;
            this.btnSuppliers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnSuppliers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuppliers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSuppliers.ForeColor = System.Drawing.Color.White;
            this.btnSuppliers.Location = new System.Drawing.Point(10, 292);
            this.btnSuppliers.Name = "btnSuppliers";
            this.btnSuppliers.Size = new System.Drawing.Size(200, 45);
            this.btnSuppliers.TabIndex = 5;
            this.btnSuppliers.Text = "S&uppliers";
            this.btnSuppliers.UseVisualStyleBackColor = false;
            this.btnSuppliers.Click += new System.EventHandler(this.btnSuppliers_Click);
            this.btnSuppliers.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnSuppliers.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnProducts
            // 
            this.btnProducts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnProducts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProducts.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnProducts.FlatAppearance.BorderSize = 2;
            this.btnProducts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnProducts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProducts.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnProducts.ForeColor = System.Drawing.Color.White;
            this.btnProducts.Location = new System.Drawing.Point(10, 241);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(200, 45);
            this.btnProducts.TabIndex = 4;
            this.btnProducts.Text = "&Products";
            this.btnProducts.UseVisualStyleBackColor = false;
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            this.btnProducts.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnProducts.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnPOS
            // 
            this.btnPOS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnPOS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPOS.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnPOS.FlatAppearance.BorderSize = 2;
            this.btnPOS.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnPOS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnPOS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPOS.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPOS.ForeColor = System.Drawing.Color.White;
            this.btnPOS.Location = new System.Drawing.Point(10, 190);
            this.btnPOS.Name = "btnPOS";
            this.btnPOS.Size = new System.Drawing.Size(200, 45);
            this.btnPOS.TabIndex = 3;
            this.btnPOS.Text = "&POS";
            this.btnPOS.UseVisualStyleBackColor = false;
            this.btnPOS.Click += new System.EventHandler(this.btnPOS_Click);
            this.btnPOS.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnPOS.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // lblPharmacyName
            // 
            this.lblPharmacyName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPharmacyName.ForeColor = System.Drawing.Color.White;
            this.lblPharmacyName.Location = new System.Drawing.Point(10, 147);
            this.lblPharmacyName.Name = "lblPharmacyName";
            this.lblPharmacyName.Size = new System.Drawing.Size(200, 40);
            this.lblPharmacyName.TabIndex = 1;
            this.lblPharmacyName.Text = "AMGC Pharmacy";
            this.lblPharmacyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAnimator
            // 
            this.buttonAnimator.Interval = 15;
            this.buttonAnimator.Tick += new System.EventHandler(this.buttonAnimator_Tick);
            // 
            // sidePanelAnimator
            // 
            this.sidePanelAnimator.Interval = 10;
            this.sidePanelAnimator.Tick += new System.EventHandler(this.sidePanelAnimator_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 628);
            this.Controls.Add(this.sidePanel);
            this.Controls.Add(this.topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.userMenuStrip;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AMGC Pharmacy - Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.userMenuStrip.ResumeLayout(false);
            this.userMenuStrip.PerformLayout();
            this.sidePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.MenuStrip userMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem userDropdownMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.Panel sidePanel;
        private System.Windows.Forms.Label lblPharmacyName;
        private System.Windows.Forms.Button btnPOS;
        private System.Windows.Forms.Button btnProducts;
        private System.Windows.Forms.Button btnSuppliers;
        private System.Windows.Forms.Button btnAdjustments;
        private System.Windows.Forms.Button btnSales;
        private System.Windows.Forms.Button btnViewLogs;
        private System.Windows.Forms.Button btnToggleSidePanel;
        private System.Windows.Forms.Timer buttonAnimator;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer sidePanelAnimator;
    }
}