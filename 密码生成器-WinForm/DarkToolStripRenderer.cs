using System.Drawing;
using System.Windows.Forms;

namespace 密码生成器_WinForm
{
    // Custom renderer to make ToolStrip / MenuStrip drop-downs dark
    class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        // default colors (used if ToolStrip.BackColor / ForeColor are not set)
        private readonly Color DefaultBack = Color.FromArgb(45, 45, 48);
        private readonly Color DefaultItemSelected = Color.FromArgb(0, 122, 204);
        private readonly Color DefaultItemText = Color.White;
        private readonly Color DefaultSeparator = Color.FromArgb(80, 80, 80);

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            var back = e.ToolStrip?.BackColor ?? DefaultBack;
            using (var b = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(b, e.AffectedBounds);
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var g = e.Graphics;
            var bounds = new Rectangle(Point.Empty, e.Item.Size);

            var toolStrip = e.Item?.Owner as ToolStrip;
            var back = toolStrip?.BackColor ?? DefaultBack;
            var selected = toolStrip?.ForeColor ?? DefaultItemSelected; // keep using ForeColor only as fallback for selection
            using (var b = new SolidBrush(e.Item.Selected ? DefaultItemSelected : back))
            {
                g.FillRectangle(b, bounds);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.ToolStrip?.ForeColor ?? DefaultItemText;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            var g = e.Graphics;
            var rc = e.Item.Bounds;
            int y = rc.Height / 2;
            var sep = e.ToolStrip?.ForeColor ?? DefaultSeparator;
            using (var p = new Pen(sep))
            {
                g.DrawLine(p, rc.Left + 2, rc.Top + y, rc.Right - 2, rc.Top + y);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // suppress default border for a cleaner dark look
        }
    }
}
