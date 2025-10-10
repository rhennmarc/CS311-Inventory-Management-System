using inventory_management;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Inventory_Management_System
{
    public partial class frmMain : Form
    {
        private string username;
        private string usertype;
        private List<Form> openForms = new List<Form>();
        private bool sidePanelVisible = true;
        private bool isAnimating = false;
        private int targetSidePanelWidth = 220;
        private int currentSidePanelWidth = 220;
        private const int SIDE_PANEL_WIDTH = 220;
        private const int ANIMATION_SPEED = 15;

        private Dictionary<Button, Color> originalButtonColors = new Dictionary<Button, Color>();
        private Dictionary<Button, Color> hoverButtonColors = new Dictionary<Button, Color>();
        private Dictionary<Button, bool> buttonHoverStates = new Dictionary<Button, bool>();

        public frmMain(string username, string usertype)
        {
            InitializeComponent();

            this.username = username;
            this.usertype = usertype;

            userDropdownMenuItem.Text = username + " (" + usertype + ")";

            this.IsMdiContainer = true;

            // Set visibility based on user type
            if (usertype.ToUpper() == "PHARMACIST")
            {
                lblAdminActions.Visible = false;
                btnAccountsManagement.Visible = false;
                btnViewLogs.Visible = false;
            }
            else if (usertype.ToUpper() == "ADMINISTRATOR")
            {
                lblAdminActions.Visible = true;
                btnAccountsManagement.Visible = true;
                btnViewLogs.Visible = true;
            }

            // Setup button animations and colors
            SetupButtonAnimations();

            // Setup the dropdown menu to prevent white background
            SetupUserDropdownMenu();

            // Center all existing MDI children
            CenterAllMdiChildren();
        }

        private void SetupUserDropdownMenu()
        {
            userMenuStrip.Renderer = new CustomMenuRenderer();

            userDropdownMenuItem.DropDown.Opening += (sender, e) =>
            {
                userDropdownMenuItem.DropDownDirection = ToolStripDropDownDirection.BelowRight;
            };
        }

        private void SetupButtonAnimations()
        {
            Color sidePanelColor = Color.FromArgb(44, 62, 80);
            Color hoverColor = Color.FromArgb(52, 73, 94);

            var buttons = new[] { btnPOS, btnProducts, btnSuppliers, btnAdjustments, btnSales, btnViewLogs, btnAccountsManagement };

            foreach (var button in buttons)
            {
                originalButtonColors[button] = sidePanelColor;
                hoverButtonColors[button] = hoverColor;
                buttonHoverStates[button] = false;

                button.BackColor = sidePanelColor;
                button.FlatAppearance.BorderColor = hoverColor;
                button.FlatAppearance.MouseDownBackColor = hoverColor;
                button.FlatAppearance.MouseOverBackColor = hoverColor;
            }
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button button && hoverButtonColors.ContainsKey(button))
            {
                buttonHoverStates[button] = true;
                if (!buttonAnimator.Enabled)
                    buttonAnimator.Start();
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button button && originalButtonColors.ContainsKey(button))
            {
                buttonHoverStates[button] = false;
                if (!buttonAnimator.Enabled)
                    buttonAnimator.Start();
            }
        }

        private void buttonAnimator_Tick(object sender, EventArgs e)
        {
            bool anyAnimating = false;

            foreach (var button in buttonHoverStates.Keys.ToList())
            {
                Color targetColor = buttonHoverStates[button] ?
                    hoverButtonColors[button] :
                    originalButtonColors[button];

                if (button.BackColor != targetColor)
                {
                    button.BackColor = targetColor;
                    anyAnimating = true;
                }
            }

            if (!anyAnimating)
            {
                buttonAnimator.Stop();
            }
        }

        private void sidePanelAnimator_Tick(object sender, EventArgs e)
        {
            if (currentSidePanelWidth < targetSidePanelWidth)
            {
                currentSidePanelWidth += ANIMATION_SPEED;
                if (currentSidePanelWidth > targetSidePanelWidth)
                    currentSidePanelWidth = targetSidePanelWidth;
            }
            else if (currentSidePanelWidth > targetSidePanelWidth)
            {
                currentSidePanelWidth -= ANIMATION_SPEED;
                if (currentSidePanelWidth < targetSidePanelWidth)
                    currentSidePanelWidth = targetSidePanelWidth;
            }

            sidePanel.Width = currentSidePanelWidth;

            // Center all MDI children when panel size changes
            CenterAllMdiChildren();

            if (currentSidePanelWidth == targetSidePanelWidth)
            {
                sidePanelAnimator.Stop();
                isAnimating = false;

                // Update button text after animation completes
                btnToggleSidePanel.Text = sidePanelVisible ? "✕" : "☰";
            }
        }

        private void ShowOrActivateForm<T>(Func<T> createForm) where T : Form
        {
            var existingForm = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (existingForm != null)
            {
                existingForm.BringToFront();
                existingForm.WindowState = FormWindowState.Normal;
                existingForm.Focus();
                CenterMdiChild(existingForm);
            }
            else
            {
                var newForm = createForm();
                newForm.MdiParent = this;

                if (!openForms.Contains(newForm))
                {
                    openForms.Add(newForm);
                    newForm.FormClosed += (s, e) => openForms.Remove(newForm);
                }

                newForm.Show();
                CenterMdiChild(newForm);
                newForm.BringToFront();
            }

            // Auto-hide side panel when form is opened
            if (sidePanelVisible && !isAnimating)
            {
                HideSidePanel();
            }
        }

        private void CenterMdiChild(Form childForm)
        {
            if (childForm == null || childForm.MdiParent != this) return;

            try
            {
                // Get the actual client area available for MDI children
                // This accounts for the MDI client area which is different from the main form's client area
                Rectangle mdiClientRect = GetMdiClientArea();

                // Calculate center position within the MDI client area
                int x = mdiClientRect.Left + (mdiClientRect.Width - childForm.Width) / 2;
                int y = mdiClientRect.Top + (mdiClientRect.Height - childForm.Height) / 2;

                // Ensure the form doesn't go outside visible bounds
                x = Math.Max(mdiClientRect.Left, x);
                y = Math.Max(mdiClientRect.Top, y);

                childForm.Location = new Point(x, y);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error centering form: {ex.Message}");

                // Fallback: simple centering without MDI client area
                int x = (this.ClientSize.Width - childForm.Width) / 2;
                int y = (this.ClientSize.Height - childForm.Height) / 2;
                childForm.Location = new Point(x, y);
            }
        }

        private Rectangle GetMdiClientArea()
        {
            foreach (Control control in this.Controls)
            {
                if (control is MdiClient mdiClient)
                {
                    // The MdiClient is the actual container for MDI children
                    // It automatically accounts for the top panel and side panel
                    return mdiClient.ClientRectangle;
                }
            }

            // Fallback: calculate manually
            int mdiLeft = sidePanel.Width;
            int mdiTop = topPanel.Height;
            int mdiWidth = this.ClientSize.Width - sidePanel.Width;
            int mdiHeight = this.ClientSize.Height - topPanel.Height;

            return new Rectangle(mdiLeft, mdiTop, mdiWidth, mdiHeight);
        }

        private void CenterAllMdiChildren()
        {
            foreach (Form childForm in this.MdiChildren)
            {
                CenterMdiChild(childForm);
            }
        }

        private void btnToggleSidePanel_Click(object sender, EventArgs e)
        {
            ToggleSidePanel();
        }

        private void ToggleSidePanel()
        {
            if (isAnimating) return;

            if (sidePanelVisible)
            {
                HideSidePanel();
            }
            else
            {
                ShowSidePanel();
            }
        }

        private void HideSidePanel()
        {
            if (isAnimating || !sidePanelVisible) return;

            isAnimating = true;
            sidePanelVisible = false;
            targetSidePanelWidth = 0;
            sidePanelAnimator.Start();
        }

        private void ShowSidePanel()
        {
            if (isAnimating || sidePanelVisible) return;

            isAnimating = true;
            sidePanelVisible = true;
            targetSidePanelWidth = SIDE_PANEL_WIDTH;
            sidePanelAnimator.Start();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Center all MDI children when main form is resized
            CenterAllMdiChildren();

            if (sidePanelVisible)
            {
                sidePanel.BringToFront();
            }
        }

        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e);
            // Ensure the active MDI child stays centered
            if (this.ActiveMdiChild != null)
            {
                CenterMdiChild(this.ActiveMdiChild);
            }
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmPOS(username));
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmProducts(username));
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmSuppliers(username));
        }

        private void btnAdjustments_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmAdjustments(username, usertype));
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmSalesReport(username, usertype));
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmLogs(username));
        }

        private void btnAccountsManagement_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm(() => new frmAccounts(username));
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to logout? All open forms will be closed.",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                CloseAllFormsAndLogout();
            }
        }

        private void CloseAllFormsAndLogout()
        {
            try
            {
                foreach (Form childForm in this.MdiChildren)
                {
                    try
                    {
                        childForm.Close();
                        childForm.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error closing MDI child {childForm.Name}: {ex.Message}");
                    }
                }

                foreach (Form form in openForms.ToList())
                {
                    try
                    {
                        if (!form.IsDisposed)
                        {
                            form.Close();
                            form.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error closing tracked form {form.Name}: {ex.Message}");
                    }
                }
                openForms.Clear();

                var formsToClose = Application.OpenForms.Cast<Form>()
                    .Where(f => f != this &&
                           f.GetType().Name != "frmLogin" &&
                           !f.Name.StartsWith("frmLogin"))
                    .ToList();

                foreach (var form in formsToClose)
                {
                    try
                    {
                        if (!form.IsDisposed)
                        {
                            form.Close();
                            form.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error closing application form {form.Name}: {ex.Message}");
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                frmLogin login = new frmLogin();
                login.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Logout Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                frmLogin login = new frmLogin();
                login.Show();
                Application.Exit();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }

    // Custom menu renderer to prevent white background in dropdown
    public class CustomMenuRenderer : ToolStripProfessionalRenderer
    {
        public CustomMenuRenderer() : base(new CustomColorTable()) { }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
            {
                if (e.Item.IsOnDropDown == false && e.Item.Selected)
                {
                    var rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(44, 62, 80)), rect);
                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(52, 73, 94)), rect);
                }
                else if (e.Item.IsOnDropDown && e.Item.Selected)
                {
                    var rect = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(44, 62, 80)), rect);
                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(52, 73, 94)), rect);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(52, 73, 94)), e.AffectedBounds);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White;
            base.OnRenderItemText(e);
        }
    }

    public class CustomColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(44, 62, 80);
        public override Color MenuItemBorder => Color.FromArgb(52, 73, 94);
        public override Color MenuBorder => Color.FromArgb(52, 73, 94);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(44, 62, 80);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(44, 62, 80);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(44, 62, 80);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(44, 62, 80);
        public override Color ImageMarginGradientBegin => Color.FromArgb(52, 73, 94);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(52, 73, 94);
        public override Color ImageMarginGradientEnd => Color.FromArgb(52, 73, 94);
        public override Color ToolStripDropDownBackground => Color.FromArgb(52, 73, 94);
    }
}