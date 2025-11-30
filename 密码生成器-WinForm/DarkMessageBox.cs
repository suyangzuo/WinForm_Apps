using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace 密码生成器_WinForm
{
    public class DarkMessageBox : Form
    {
        private Label lblMessage;
        private readonly string _dialogMessage;
        private Button btnOK;

        public DarkMessageBox(string message, string title)
        {
            _dialogMessage = message ?? string.Empty;
            this.Text = title;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(400, 200);
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // We'll render the text manually so we can center the whole block horizontally
            lblMessage = new Label()
            {
                Text = string.Empty, // we paint the message ourselves from _dialogMessage
                ForeColor = Color.White,
                BackColor = Color.FromArgb(20, 20, 20),
                AutoSize = false,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            lblMessage.Paint += LblMessage_Paint;

            btnOK = new Button()
            {
                Text = "确定",
                DialogResult = DialogResult.OK,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Size = new Size(80, 30),
                Location = new Point(300, 150)
            };

            btnOK.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 70);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60); // 悬停背景色

            this.Controls.Add(lblMessage);
            this.Controls.Add(btnOK);

            this.AcceptButton = btnOK;
        }

        private void LblMessage_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // client rect minus padding
            var rc = lblMessage.ClientRectangle;
            var pad = lblMessage.Padding;
            var inner = new Rectangle(rc.Left + pad.Left, rc.Top + pad.Top, Math.Max(0, rc.Width - pad.Horizontal), Math.Max(0, rc.Height - pad.Vertical));

            // split lines preserving empty lines
            var lines = _dialogMessage.Replace("\r\n", "\n").Split(new[] { '\n' }, StringSplitOptions.None);

            // measure each line and compute max width and total height
            float maxWidth = 0;
            var heights = new float[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var sz = g.MeasureString(lines[i], lblMessage.Font);
                if (sz.Width > maxWidth) maxWidth = sz.Width;
                heights[i] = sz.Height;
            }

            float totalHeight = 0;
            for (int i = 0; i < heights.Length; i++) totalHeight += heights[i];
            // small gap between lines (approx 20% of font height)
            float gap = lblMessage.Font.GetHeight(g) * 0.3f;
            totalHeight += gap * Math.Max(0, lines.Length - 1);

            // compute starting positions so the block is centered horizontally and vertically within inner rect
            float startX = inner.Left + Math.Max(0, (inner.Width - maxWidth) / 2f);
            float startY = inner.Top + Math.Max(0, (inner.Height - totalHeight) / 2f);

            using (var brush = new SolidBrush(lblMessage.ForeColor))
            {
                float y = startY;
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    g.DrawString(line, lblMessage.Font, brush, new PointF(startX, y));
                    y += heights[i] + gap;
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            EnableDarkTitleBar();
        }

        private void EnableDarkTitleBar()
        {
            try
            {
                var handle = this.Handle;
                int useDark = 1;
                const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
                const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;

                int hr = DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark, Marshal.SizeOf(useDark));
                if (hr != 0)
                {
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE_OLD, ref useDark, Marshal.SizeOf(useDark));
                }
            }
            catch
            {
                // Ignore errors for unsupported OS versions
            }
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public static void Show(string message, string title)
        {
            using var dialog = new DarkMessageBox(message, title);
            dialog.ShowDialog();
        }
    }
}
