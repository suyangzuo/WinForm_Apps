using static 密码生成器_WinForm.Password;
using Sunny.UI;
using System.Runtime.InteropServices;

namespace 密码生成器_WinForm
{
    public partial class Form1 : Form
    {
        private PwdType pwdType = PwdType.All;

        // container used to keep all main controls centered together
        private System.Windows.Forms.Panel? centerPanel;

        public Form1()
        {
            InitializeComponent();
            CustomInitialize();
        }

        private void CustomInitialize()
        {
            this.ClientSize = new Size(900, 675);
            trackBar_PwdBits.ValueChanged += SavePasswordLength;
            cbx_Letters.CheckedChanged += PwdTypeSelect;
            cbx_Digits.CheckedChanged += PwdTypeSelect;
            cbx_Symbols.CheckedChanged += PwdTypeSelect;
            //cbx_Letters.MouseEnter += ChangeStyleWhenEnter;
            cbx_Digits.MouseEnter += ChangeStyleWhenEnter;
            cbx_Symbols.MouseEnter += ChangeStyleWhenEnter;
            //cbx_Letters.MouseLeave += ChangeStyleWhenLeave;
            cbx_Digits.MouseLeave += ChangeStyleWhenLeave;
            cbx_Symbols.MouseLeave += ChangeStyleWhenLeave;
            trackBar_PwdBits.ValueChanged += ShowPwdBits;
            // regenerate password live while the user adjusts the slider (dragging or clicking)
            trackBar_PwdBits.ValueChanged += ShowPwd;
            btn_GetPwd.Click += ShowPwd;
            M2_About.Click += About;
            M2_Guide.Click += Guide;
            richTbx_Pwd.DoubleClick += WhenDoubleClick;
            trackBar_PwdBits.Value = Properties.Settings.Default.PasswordLength;

            // ensure custom paint handler is registered for the password-length label so we can draw prefix and number in different colors
            lb_PwdBits.Paint -= lb_PwdBits_Paint;
            lb_PwdBits.Paint += lb_PwdBits_Paint;

            // enable dark title bar after form handle is created (handled in OnHandleCreated)
            // this.Load += (s, e) => EnableDarkTitleBar();

            // apply dark theme to SunnyUI trackbar at runtime (safe via reflection)
            ApplyDarkToSunnyTrackBar();
            // Apply a border color to the Sunny.UI.UIRichTextBox if it supports one (try several common property names)
            ApplyBorderColorToSunnyControl(richTbx_Pwd, Color.FromArgb(70, 70, 70));

            // Make menu bar and its dropdown items match the main form background at runtime
            try
            {
                this.menuStrip_Main.BackColor = this.BackColor;
                this.menuStrip_Main.ForeColor = this.ForeColor;

                // ensure top-level items and their dropdowns match
                foreach (ToolStripItem ti in this.menuStrip_Main.Items)
                {
                    if (ti is ToolStripMenuItem topItem)
                    {
                        topItem.ForeColor = this.ForeColor;
                        if (topItem.DropDown != null)
                        {
                            topItem.DropDown.BackColor = this.BackColor;
                            topItem.DropDown.ForeColor = this.ForeColor;
                            foreach (ToolStripItem sub in topItem.DropDown.Items)
                            {
                                sub.BackColor = this.BackColor;
                                sub.ForeColor = this.ForeColor;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            // 运行时设置：允许用户调整窗口大小，并设置一个最小尺寸以防界面布局被压缩
            // 说明：我选择了 1024x600 作为最小窗口尺寸（可根据需要调整）。
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(750, 675);

            // 将界面控件包装到一个容器中以便在窗体缩放时整体居中
            SetupCentering();
        }

        // Create a panel at runtime, move non-menu controls into it (preserve relative positions)
        // and arrange to keep it centered horizontally and vertically (below the menu strip).
        private void SetupCentering()
        {
            // Avoid multiple setups
            if (centerPanel != null) return;

            // Collect controls to move (exclude menuStrip_Main and any ToolStrip-derived controls)
            var moveList = new List<Control>();
            foreach (Control c in this.Controls)
            {
                if (c == this.menuStrip_Main) continue;
                if (c is ToolStrip) continue;
                moveList.Add(c);
            }

            if (moveList.Count == 0) return;

            // Compute bounding rectangle of the controls to move
            int minX = int.MaxValue, minY = int.MaxValue, maxRight = int.MinValue, maxBottom = int.MinValue;
            foreach (var c in moveList)
            {
                minX = Math.Min(minX, c.Left);
                minY = Math.Min(minY, c.Top);
                maxRight = Math.Max(maxRight, c.Right);
                maxBottom = Math.Max(maxBottom, c.Bottom);
            }

            const int padding = 8; // small padding inside the panel
            int panelWidth = Math.Max(1, (maxRight - minX) + padding * 2);
            int panelHeight = Math.Max(1, (maxBottom - minY) + padding * 2);

            centerPanel = new System.Windows.Forms.Panel();
            centerPanel.Name = "centerPanel";
            centerPanel.Size = new System.Drawing.Size(panelWidth, panelHeight);
            centerPanel.BackColor = this.BackColor; // match form background
            centerPanel.Anchor = AnchorStyles.None;
            centerPanel.TabIndex = 999; // keep behind other tab indices

            // Add panel to Controls first so reparenting child controls won't remove it
            this.Controls.Add(centerPanel);

            // Move each control into centerPanel and adjust its location to be relative to panel
            foreach (var c in moveList.ToArray())
            {
                // record old location relative to form
                var oldLoc = c.Location;
                // remove from form and add to panel
                this.Controls.Remove(c);
                centerPanel.Controls.Add(c);
                // new location relative to panel (preserve layout)
                c.Location = new System.Drawing.Point(oldLoc.X - minX + padding, oldLoc.Y - minY + padding);
            }

            // Center initially and on resize
            this.Resize += (_, _) => CenterControls();
            this.Shown += (_, _) => CenterControls();

            CenterControls();
        }

        // Center the centerPanel within the form's client area, below the menu strip if present
        private void CenterControls()
        {
            if (centerPanel == null) return;

            int menuBottom = (this.menuStrip_Main != null) ? this.menuStrip_Main.Bottom : 0;
            int availWidth = this.ClientSize.Width;
            int availHeight = Math.Max(0, this.ClientSize.Height - menuBottom);

            int x = (availWidth - centerPanel.Width) / 2;
            int y = menuBottom + (availHeight - centerPanel.Height) / 2;

            // clamp to non-negative
            x = Math.Max(0, x);
            y = Math.Max(menuBottom, y);

            centerPanel.Location = new System.Drawing.Point(x, y);
        }

        // Ensure dark title bar is applied whenever the window handle is created/recreated
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            EnableDarkTitleBar();
        }

        // Try to set common SunnyUI trackbar color/style properties if they exist (no compile-time dependency)
        private void ApplyDarkToSunnyTrackBar()
        {
            var t = this.trackBar_PwdBits;
            if (t == null) return;

            // base colors
            t.BackColor = Color.FromArgb(20, 20, 20);
            t.ForeColor = Color.White;

            var type = t.GetType();

            void TrySet(string propName, object val)
            {
                var p = type.GetProperty(propName);
                if (p == null || !p.CanWrite) return;
                try
                {
                    p.SetValue(t, val);
                }
                catch
                {
                }
            }

            // common property names in SunnyUI controls
            // make the trackbar fill color match the main form background
            TrySet("FillColor", this.BackColor);
            TrySet("BarColor", Color.FromArgb(70, 70, 70));
            TrySet("SliderColor", Color.FromArgb(120, 120, 120));
            TrySet("HandleColor", Color.FromArgb(200, 200, 200));
            TrySet("ThumbColor", Color.FromArgb(200, 200, 200));

            // try to set Style enum to Custom if available
            var pStyle = type.GetProperty("Style");
            if (pStyle == null || !pStyle.CanWrite) return;
            try
            {
                var enumType = pStyle.PropertyType;
                var enumNames = System.Enum.GetNames(enumType);
                if (System.Array.IndexOf(enumNames, "Custom") < 0) return;
                var enumVal = System.Enum.Parse(enumType, "Custom");
                pStyle.SetValue(t, enumVal);
            }
            catch
            {
            }
        }

        // Try to set common SunnyUI border-like properties via reflection. Many Sunny.UI controls expose properties
        // such as RectColor, BorderColor, RectBackColor, etc. This helper attempts several names safely.
        private void ApplyBorderColorToSunnyControl(object control, Color color)
        {
            if (control == null) return;
            var type = control.GetType();

            string[] names = new[]
            {
                "RectColor", "BorderColor", "RectBackColor", "RectInnerColor", "RectColor2", "FrameColor", "LineColor"
            };

            foreach (var name in names)
            {
                var p = type.GetProperty(name);
                if (p == null || !p.CanWrite) continue;
                try
                {
                    p.SetValue(control, color);
                }
                catch
                {
                }
            }

            // try common property for standard WinForms controls
            var pBorderStyle = type.GetProperty("BorderStyle");
            if (pBorderStyle != null && pBorderStyle.CanWrite)
            {
                try
                {
                    pBorderStyle.SetValue(control, System.Windows.Forms.BorderStyle.FixedSingle);
                }
                catch
                {
                }
            }
        }

        private void PwdTypeSelect(object? sender, EventArgs e)
        {
            PwdType type = PwdType.None;
            var pwdTypeSet = new List<PwdType>() { PwdType.Letters };

            if (cbx_Digits.Checked)
                pwdTypeSet.Add(PwdType.Digits);
            else
                pwdTypeSet.Remove(PwdType.Digits);

            if (cbx_Symbols.Checked)
                pwdTypeSet.Add(PwdType.Symbols);
            else
                pwdTypeSet.Remove(PwdType.Symbols);

            foreach (var item in pwdTypeSet)
            {
                type |= item;
            }

            pwdType = type;
        }

        private void ShowPwdBits(object? sender, EventArgs e)
        {
            lb_PwdBits.Text = $"密码长度：{trackBar_PwdBits.Value}";
            // trigger repaint so lb_PwdBits_Paint runs and draws with two colors
            lb_PwdBits.Invalidate();
        }

        private void ShowPwd(object? sender, EventArgs e)
        {
            richTbx_Pwd.Text = GetPassword(trackBar_PwdBits.Value, pwdType);
        }

        private void ChangeStyleWhenEnter(object? sender, EventArgs e)
        {
            if (sender is UICheckBox b)
            {
                b.CheckBoxColor = Color.DarkOrange;
                b.ForeColor = Color.BlueViolet;
            }
        }

        private void ChangeStyleWhenLeave(object? sender, EventArgs e)
        {
            if (sender is UICheckBox b)
            {
                b.CheckBoxColor = Color.FromArgb(80, 160, 255);
                // keep text white in dark theme
                b.ForeColor = Color.White;
            }
        }

        private void About(object? sender, EventArgs e)
        {
            DarkMessageBox.Show("开发名单：苏扬\n开发平台：.NET 6.0 WinForm\n开发时间：2022年6月2日", "关于");
        }

        private void Guide(object? sender, EventArgs e)
        {
            DarkMessageBox.Show("双击文本框，即可全选密码文本\n强密码推荐：字母+数字+字符，\n建议密码长度：8-20", "使用说明");
        }

        private void WhenDoubleClick(object? sender, EventArgs e)
        {
            richTbx_Pwd.SelectAll();
        }

        private void SavePasswordLength(object? sender, EventArgs e)
        {
            Properties.Settings.Default.PasswordLength = trackBar_PwdBits.Value;
            Properties.Settings.Default.Save();
        }

        // Enable dark title bar on Windows 10/11 using DWM
        private void EnableDarkTitleBar()
        {
            try
            {
                var handle = this.Handle;
                int useDark = 1;
                // Try attribute 20 (Windows 11 / newer) then 19 (some Windows 10 builds)
                const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
                const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;

                int hr = DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark,
                    Marshal.SizeOf(useDark));
                if (hr != 0)
                {
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE_OLD, ref useDark,
                        Marshal.SizeOf(useDark));
                }
            }
            catch
            {
                // ignore on failure; non-supported OS or DWM not available
            }
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(System.IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private void richTbx_Pwd_TextChanged(object? sender, EventArgs e)
        {
            // 暂时禁用文本框的事件以避免递归
            richTbx_Pwd.TextChanged -= richTbx_Pwd_TextChanged;

            int selectionStart = richTbx_Pwd.SelectionStart;
            int selectionLength = richTbx_Pwd.SelectionLength;

            // 保存当前文本
            string text = richTbx_Pwd.Text;

            // 清除所有格式
            richTbx_Pwd.SelectAll();
            richTbx_Pwd.SelectionColor = Color.White;

            // 遍历文本并设置颜色
            for (int i = 0; i < text.Length; i++)
            {
                richTbx_Pwd.Select(i, 1);
                char c = text[i];

                if (char.IsLetter(c))
                {
                    richTbx_Pwd.SelectionColor = Color.LightCoral; // 字母为绿色
                }
                else if (char.IsDigit(c))
                {
                    richTbx_Pwd.SelectionColor = Color.CornflowerBlue; // 数字为蓝色
                }
                else
                {
                    richTbx_Pwd.SelectionColor = Color.YellowGreen; // 特殊字符为红色
                }
            }

            // 恢复选择范围
            richTbx_Pwd.Select(selectionStart, selectionLength);
            richTbx_Pwd.SelectionColor = Color.White;

            // 重新启用文本框的事件
            richTbx_Pwd.TextChanged += richTbx_Pwd_TextChanged;
        }

        private void lb_PwdBits_Paint(object sender, PaintEventArgs e)
        {
            if (sender is UILabel label)
            {
                // 获取当前密码长度设置
                int pwdLength = trackBar_PwdBits.Value;

                // 定义前缀和数字部分的格式
                string prefix = "密码长度：";
                string numberPart = pwdLength.ToString();

                // 定义前缀和数字部分的字体（前缀较小）
                Font prefixFont = new Font(label.Font.FontFamily, label.Font.Size, FontStyle.Regular);
                Font numberFont = new Font("Consolas", label.Font.Size * 1.1f, FontStyle.Bold);

                // 定义前缀和数字部分的颜色
                Brush prefixBrush = new SolidBrush(Color.Silver);
                Brush numberBrush = new SolidBrush(Color.LightSkyBlue);

                try
                {
                    // 清除默认绘制以避免文字重叠（使用标签背景色）
                    e.Graphics.Clear(label.BackColor);
                    // 测量前缀文本和数字部分的宽度（使用实际 prefix 字符串，避免中英文冒号误差）
                    SizeF prefixSize = e.Graphics.MeasureString("密码长度:", prefixFont);
                    SizeF numberSize = e.Graphics.MeasureString("00", numberFont);

                    // 提高文本绘制质量
                    var oldHint = e.Graphics.TextRenderingHint;
                    try
                    {
                        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                        // 计算文本的中心点
                        PointF centerPoint = new PointF(label.Width / 2f, label.Height / 2f);

                        // 计算前缀的起始位置
                        PointF prefixPoint = new PointF(centerPoint.X - (prefixSize.Width + numberSize.Width) / 2f,
                            centerPoint.Y - prefixSize.Height / 2f);

                        // 绘制前缀文本（灰色）
                        e.Graphics.DrawString(prefix, prefixFont, prefixBrush, prefixPoint);

                        // 计算数字部分的起始位置并绘制（天蓝）
                        PointF numberPoint = new PointF(prefixPoint.X + prefixSize.Width,
                            centerPoint.Y - numberSize.Height / 2f);
                        e.Graphics.DrawString(numberPart, numberFont, numberBrush, numberPoint);
                    }
                    finally
                    {
                        e.Graphics.TextRenderingHint = oldHint;
                    }
                }
                finally
                {
                    // 确保释放字体和画刷资源
                    try
                    {
                        numberFont.Dispose();
                    }
                    catch
                    {
                    }

                    try
                    {
                        prefixFont.Dispose();
                    }
                    catch
                    {
                    }

                    try
                    {
                        prefixBrush.Dispose();
                    }
                    catch
                    {
                    }

                    try
                    {
                        numberBrush.Dispose();
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}