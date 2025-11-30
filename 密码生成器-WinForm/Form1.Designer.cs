namespace 密码生成器_WinForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_GetPwd = new Sunny.UI.UIButton();
            this.richTbx_Pwd = new Sunny.UI.UIRichTextBox();
            this.trackBar_PwdBits = new CustomTrackBar();
            this.lb_PwdBits = new Sunny.UI.UILabel();
            this.cbx_Letters = new ModernCheckBox();
            this.cbx_Digits = new ModernCheckBox();
            this.cbx_Symbols = new ModernCheckBox();
            this.flowLayoutPanel_Checks = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.M1_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.M2_About = new System.Windows.Forms.ToolStripMenuItem();
            this.M2_Guide = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_GetPwd
            // 
            this.btn_GetPwd.FillColor = System.Drawing.Color.FromArgb(0, 82, 164);
            this.btn_GetPwd.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_GetPwd.Location = new System.Drawing.Point(500, 570);
            this.btn_GetPwd.MinimumSize = new System.Drawing.Size(1, 1);
            this.btn_GetPwd.Name = "btn_GetPwd";
            this.btn_GetPwd.Size = new System.Drawing.Size(206, 74);
            this.btn_GetPwd.Style = Sunny.UI.UIStyle.Custom;
            this.btn_GetPwd.TabIndex = 0;
            this.btn_GetPwd.Text = "生成密码";
            this.btn_GetPwd.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.btn_GetPwd.ForeColor = System.Drawing.Color.White;
            // richTbx_Pwd
            // 
            this.richTbx_Pwd.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.richTbx_Pwd.FillColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.richTbx_Pwd.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTbx_Pwd.ForeColor = System.Drawing.Color.White;
            this.richTbx_Pwd.Location = new System.Drawing.Point(293, 99);
            this.richTbx_Pwd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTbx_Pwd.MinimumSize = new System.Drawing.Size(1, 1);
            this.richTbx_Pwd.Name = "richTbx_Pwd";
            // increase inner padding so the password text isn't flush against the control edges
            this.richTbx_Pwd.Padding = new System.Windows.Forms.Padding(12, 8, 8, 12);
            this.richTbx_Pwd.ReadOnly = true;
            this.richTbx_Pwd.ShowText = false;
            this.richTbx_Pwd.Size = new System.Drawing.Size(636, 159);
            this.richTbx_Pwd.Style = Sunny.UI.UIStyle.Custom;
            this.richTbx_Pwd.TabIndex = 1;
            this.richTbx_Pwd.TabStop = false;
            this.richTbx_Pwd.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.richTbx_Pwd.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.richTbx_Pwd.TextChanged += new System.EventHandler(this.richTbx_Pwd_TextChanged);
            // 
            // trackBar_PwdBits
            // 
            this.trackBar_PwdBits.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.trackBar_PwdBits.Location = new System.Drawing.Point(419, 437);
            this.trackBar_PwdBits.Maximum = 50;
            this.trackBar_PwdBits.Minimum = 6;
            this.trackBar_PwdBits.Name = "trackBar_PwdBits";
            this.trackBar_PwdBits.Size = new System.Drawing.Size(360, 44);
            this.trackBar_PwdBits.TabIndex = 1;
            this.trackBar_PwdBits.Value = 8;
            this.trackBar_PwdBits.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.trackBar_PwdBits.FillColor = System.Drawing.Color.Gray; // filled portion (already passed) -> gray
            this.trackBar_PwdBits.UnfilledColor = System.Drawing.Color.FromArgb(60, 130, 230); // remaining portion -> sky-blue
            // 
            // lb_PwdBits
            // 
            this.lb_PwdBits.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_PwdBits.Location = new System.Drawing.Point(469, 484);
            this.lb_PwdBits.Name = "lb_PwdBits";
            this.lb_PwdBits.Size = new System.Drawing.Size(276, 34);
            this.lb_PwdBits.TabIndex = 5;
            this.lb_PwdBits.Text = "密码位数：8";
            this.lb_PwdBits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_PwdBits.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.lb_PwdBits.ForeColor = System.Drawing.Color.White;
            // 
            // cbx_Letters
            // 
            this.cbx_Letters.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.cbx_Letters.Checked = true;
            this.cbx_Letters.AllowUncheck = false;
            this.cbx_Letters.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbx_Letters.Name = "cbx_Letters";
            this.cbx_Letters.Size = new System.Drawing.Size(106, 44);
            this.cbx_Letters.TabIndex = 2;
            this.cbx_Letters.Text = "字母";
            this.cbx_Letters.ForeColor = System.Drawing.Color.White;
            this.cbx_Letters.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            // 
            // cbx_Digits
            // 
            this.cbx_Digits.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.cbx_Digits.Checked = true;
            this.cbx_Digits.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbx_Digits.Name = "cbx_Digits";
            this.cbx_Digits.Size = new System.Drawing.Size(106, 44);
            this.cbx_Digits.TabIndex = 2;
            this.cbx_Digits.Text = "数字";
            this.cbx_Digits.ForeColor = System.Drawing.Color.White;
            this.cbx_Digits.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            // 
            // cbx_Symbols
            // 
            this.cbx_Symbols.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.cbx_Symbols.Checked = true;
            this.cbx_Symbols.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cbx_Symbols.Name = "cbx_Symbols";
            this.cbx_Symbols.Size = new System.Drawing.Size(106, 44);
            this.cbx_Symbols.TabIndex = 2;
            this.cbx_Symbols.Text = "字符";
            this.cbx_Symbols.ForeColor = System.Drawing.Color.White;
            this.cbx_Symbols.Margin = new System.Windows.Forms.Padding(0);
            // 
            // flowLayoutPanel_Checks
            // 
            this.flowLayoutPanel_Checks.Location = new System.Drawing.Point(450, 331);
            this.flowLayoutPanel_Checks.Name = "flowLayoutPanel_Checks";
            this.flowLayoutPanel_Checks.Size = new System.Drawing.Size(350, 44);
            this.flowLayoutPanel_Checks.TabIndex = 3;
            this.flowLayoutPanel_Checks.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowLayoutPanel_Checks.WrapContents = false;
            this.flowLayoutPanel_Checks.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.flowLayoutPanel_Checks.Controls.Add(this.cbx_Letters);
            this.flowLayoutPanel_Checks.Controls.Add(this.cbx_Digits);
            this.flowLayoutPanel_Checks.Controls.Add(this.cbx_Symbols);
            // 
            // menuStrip_Main
            // 
            this.menuStrip_Main.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.menuStrip_Main.ForeColor = System.Drawing.Color.White;
            this.menuStrip_Main.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.M1_Help});
            this.menuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Main.Name = "menuStrip_Main";
            this.menuStrip_Main.Size = new System.Drawing.Size(1204, 35);
            this.menuStrip_Main.TabIndex = 6;
            this.menuStrip_Main.Text = "menuStrip1";
            // make dropdown menus dark using custom renderer
            // assign Renderer first, then set RenderMode to Custom (RenderMode = Custom throws if Renderer is null)
            this.menuStrip_Main.Renderer = new DarkToolStripRenderer();
             // ensure top-level and dropdown items use dark colors
            this.menuStrip_Main.ForeColor = System.Drawing.Color.White;
            this.M1_Help.ForeColor = System.Drawing.Color.White;
            this.M1_Help.DropDown.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.M1_Help.DropDown.ForeColor = System.Drawing.Color.White;
            this.M2_Guide.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.M2_Guide.ForeColor = System.Drawing.Color.White;
            this.M2_About.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.M2_About.ForeColor = System.Drawing.Color.White;
            // 
            // M1_Help
            // 
            this.M1_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.M2_Guide,
            this.M2_About});
            this.M1_Help.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.M1_Help.Name = "M1_Help";
            this.M1_Help.Size = new System.Drawing.Size(68, 31);
            this.M1_Help.Text = "帮助";
            this.M1_Help.ForeColor = System.Drawing.Color.White;
            // 
            // M2_About
            // 
            this.M2_About.Name = "M2_About";
            this.M2_About.Size = new System.Drawing.Size(270, 36);
            this.M2_About.Text = "关于";
            this.M2_About.ForeColor = System.Drawing.Color.White;
            // 
            // M2_Guide
            // 
            this.M2_Guide.Name = "M2_Guide";
            this.M2_Guide.Size = new System.Drawing.Size(270, 36);
            this.M2_Guide.Text = "使用说明";
            this.M2_Guide.ForeColor = System.Drawing.Color.White;
            // 
            // Form1
            // 
            this.AcceptButton = this.btn_GetPwd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.ForeColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1204, 698);
            this.Controls.Add(this.flowLayoutPanel_Checks);
            this.Controls.Add(this.lb_PwdBits);
            this.Controls.Add(this.trackBar_PwdBits);
            this.Controls.Add(this.richTbx_Pwd);
            this.Controls.Add(this.btn_GetPwd);
            this.Controls.Add(this.menuStrip_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip_Main;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "密码生成器";
            this.menuStrip_Main.ResumeLayout(false);
            this.menuStrip_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Sunny.UI.UIButton btn_GetPwd;
        private Sunny.UI.UIRichTextBox richTbx_Pwd;
        private CustomTrackBar trackBar_PwdBits;
         private Sunny.UI.UILabel lb_PwdBits;
         private ModernCheckBox cbx_Letters;
         private ModernCheckBox cbx_Digits;
         private ModernCheckBox cbx_Symbols;
         private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Checks;
         private MenuStrip menuStrip_Main;
         private ToolStripMenuItem M1_Help;
         private ToolStripMenuItem M2_About;
         private ToolStripMenuItem M2_Guide;
     }
 }