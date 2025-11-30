using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace 密码生成器_WinForm
{
    // A lightweight modern-looking CheckBox: rounded square + smooth checkmark
    public class ModernCheckBox : CheckBox
    {
        private Color _boxBack = Color.FromArgb(30, 30, 30);
        private Color _boxBorder = Color.FromArgb(100, 100, 100);
        private Color _origBoxBorder;
        private Color _boxChecked = Color.FromArgb(0, 150, 80);
        private Color _checkColor = Color.White;
        private Color _hoverBorder = Color.FromArgb(180, 180, 180);
        private bool _isHover = false;

        [Category("Appearance")]
        public Color BoxBackColor
        {
            get => _boxBack;
            set
            {
                _boxBack = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BoxBorderColor
        {
            get => _boxBorder;
            set
            {
                _boxBorder = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BoxCheckedColor
        {
            get => _boxChecked;
            set
            {
                _boxChecked = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color CheckColor
        {
            get => _checkColor;
            set
            {
                _checkColor = value;
                Invalidate();
            }
        }

        [Category("Behavior")] public bool AllowUncheck { get; set; } = true;

        // guard to avoid recursion when forcing Checked back to true
        private bool _suppressCheckedChangeCorrection = false;

        public ModernCheckBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            // Prevent the control from being selectable (stop Windows drawing the focus rectangle on hover/click)
            SetStyle(ControlStyles.Selectable, false);
            this.AutoSize = false;
            // leave AutoCheck=true (default) so base CheckBox toggles Checked once on click
            // this.AutoCheck = true; // default
            this.Height = 44;
            this.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            this.ForeColor = Color.Silver;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.Cursor = Cursors.Hand;
            _origBoxBorder = _boxBorder;
            // Don't show the default focus rectangle around the whole control
            this.TabStop = false;
        }

        // Prevent Windows from drawing focus cues (dotted rectangle around the whole control)
        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHover = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            // Fill background
            using (var b = new SolidBrush(this.BackColor))
                g.FillRectangle(b, this.ClientRectangle);

            int boxSize = 25;
            int boxMargin = 8; // distance from left edge
            int boxTop = (this.Height - boxSize) / 2;
            var boxRect = new Rectangle(boxMargin, boxTop, boxSize, boxSize);

            // Effective border color: fully transparent when checked; when hovering use hover color; otherwise use configured border
            Color effectiveBorder;
            if (this.Checked)
            {
                effectiveBorder = Color.FromArgb(0, _boxBorder.R, _boxBorder.G, _boxBorder.B);
            }
            else if (_isHover)
            {
                effectiveBorder = _hoverBorder;
            }
            else
            {
                effectiveBorder = _boxBorder;
            }

            // draw rounded box background using the effective border color; configure pen for smooth joins
            // set compositing for consistent rendering
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            using (var pb = new SolidBrush(this.Checked ? _boxChecked : _boxBack))
            using (var pen = new Pen(effectiveBorder, 1f))
            {
                pen.LineJoin = LineJoin.Round;
                // draw stroke inside the shape to avoid half-pixel aliasing artifacts at control edges
                try
                {
                    pen.Alignment = PenAlignment.Inset;
                }
                catch
                {
                }

                DrawRoundRect(g, pb, pen, boxRect, 4);
            }

            // draw checkmark if checked
            if (this.Checked)
            {
                // simple checkmark path
                using (var pen = new Pen(_checkColor, 2.5f) { EndCap = LineCap.Round, StartCap = LineCap.Round })
                {
                    var start = new Point(boxRect.Left + 6, boxRect.Top + boxRect.Height / 2);
                    var mid = new Point(boxRect.Left + boxRect.Width / 2 - 1, boxRect.Bottom - 7);
                    var end = new Point(boxRect.Right - 6, boxRect.Top + 7);
                    g.DrawLines(pen, new[] { start, mid, end });
                }
            }

            // draw text
            // nudge the text up by 1 pixel to improve vertical alignment with the custom box
            var textRect = new Rectangle(boxRect.Right + 8, -2, this.Width - boxRect.Right - 8, this.Height);
            TextRenderer.DrawText(g, this.Text, this.Font, textRect, Color.Silver,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }

        private void DrawRoundRect(Graphics g, Brush fill, Pen pen, Rectangle rect, int radius)
        {
            using (var gp = new GraphicsPath())
            {
                gp.AddArc(rect.Left, rect.Top, radius, radius, 180, 90);
                gp.AddArc(rect.Right - radius, rect.Top, radius, radius, 270, 90);
                gp.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                gp.AddArc(rect.Left, rect.Bottom - radius, radius, radius, 90, 90);
                gp.CloseFigure();
                g.FillPath(fill, gp);
                g.DrawPath(pen, gp);
            }
        }

        // Prevent unchecking when AllowUncheck == false. If the box is currently checked and unchecking is disallowed,
        // swallow the mouse down so the base CheckBox won't toggle it. Otherwise fall back to default behavior.
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left && this.Checked && !this.AllowUncheck)
            {
                // ignore click to keep checked state
                return;
            }

            base.OnMouseDown(mevent);
        }

        // Prevent keyboard toggling (Space/Enter) when unchecking is disallowed.
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!this.AllowUncheck && this.Checked && (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter))
            {
                // swallow key to prevent toggle
                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }

        // Defensive: if for any reason the Checked state became false while AllowUncheck is false,
        // restore it here to guarantee the 'always-checked' contract.
        protected override void OnClick(EventArgs e)
        {
            if (!this.AllowUncheck && !this.Checked)
            {
                // restore checked state and avoid raising extra events
                this.Checked = true;
                this.Invalidate();
                return;
            }

            base.OnClick(e);
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            // If unchecking is disallowed, correct any state changes (including programmatic ones)
            if (!this.AllowUncheck && !this.Checked && !_suppressCheckedChangeCorrection)
            {
                try
                {
                    _suppressCheckedChangeCorrection = true;
                    this.Checked = true;
                }
                finally
                {
                    _suppressCheckedChangeCorrection = false;
                }
            }

            base.OnCheckedChanged(e);
        }
    }
}