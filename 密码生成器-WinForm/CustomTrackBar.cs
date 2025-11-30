using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace 密码生成器_WinForm
{
    public class CustomTrackBar : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private int _barHeight = 8;
    private int _thumbRadius = 10;
    private int _trackCornerRadius = 4;
    private bool _isDragging = false;
    // hover state for the thumb (true when mouse is over the thumb)
    private bool _isHoverThumb = false;

    // Hover appearance for thumb
    [DefaultValue(typeof(Color), "Color.FromArgb(80,130,230)")]
    public Color ThumbHoverFillColor { get; set; } = Color.FromArgb(80, 130, 230);

    [DefaultValue(typeof(Color), "Color.White")]
    public Color ThumbHoverBorderColor { get; set; } = Color.White;

    [DefaultValue(2)]
    public float ThumbHoverBorderWidth { get; set; } = 2f;

        public event EventHandler? ValueChanged;

        [DefaultValue(0)]
        public int Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                CoerceValue();
                Invalidate();
            }
        }

        [DefaultValue(100)]
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = Math.Max(value, _minimum + 1);
                CoerceValue();
                Invalidate();
            }
        }

        [DefaultValue(0)]
        public int Value
        {
            get => _value;
            set
            {
                var newVal = Math.Max(_minimum, Math.Min(_maximum, value));
                if (newVal == _value) return;
                _value = newVal;
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [DefaultValue(typeof(Color), "Color.FromArgb(60,130,230)")]
        public Color FillColor { get; set; } = Color.FromArgb(60, 130, 230);

        [DefaultValue(typeof(Color), "Color.Gray")]
        public Color UnfilledColor { get; set; } = Color.FromArgb(120, 120, 120);

        [DefaultValue(8)]
        public int BarHeight
        {
            get => _barHeight;
            set
            {
                _barHeight = Math.Max(2, value);
                Invalidate();
            }
        }

        [DefaultValue(6)]
        public int ThumbWidth
        {
            get => _thumbRadius;
            set
            {
                _thumbRadius = Math.Max(4, value);
                Invalidate();
            }
        }

        [DefaultValue(16)]
        public int ThumbHeight { get; set; } = 16;

        [DefaultValue(typeof(Color), "Color.FromArgb(240, 240, 240)")]
        public Color ThumbFillColor { get; set; } = Color.FromArgb(60, 60, 60);

        [DefaultValue(typeof(Color), "Color.FromArgb(120, 120, 120)")]
        public Color ThumbBorderColor { get; set; } = Color.LightBlue;
        
        public int ThumbCornerRadius { get; set; } = 4;
        
        [DefaultValue(4)]
        public int TrackCornerRadius
        {
            get { return _trackCornerRadius; }
            set { _trackCornerRadius = value; Invalidate(); }
        }

        public CustomTrackBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.FromArgb(60, 130, 230);
            this.Size = new Size(360, 44);
            this.Minimum = 6;
            this.Maximum = 50;
            this.Value = 8;
            this.TabStop = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(this.BackColor);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int trackY = this.Height / 2 - _barHeight / 2;
            int padding = Math.Max(_thumbRadius + 2, 6);
            int trackX = padding;
            int trackWidth = Math.Max(4, this.Width - padding * 2);

            Rectangle fullRect = new Rectangle(trackX, trackY, trackWidth, _barHeight);

            // draw unfilled rounded track
            using (var b = new SolidBrush(Color.FromArgb(20, 20, 20)))
                FillRoundedRectangle(g, b, fullRect, _trackCornerRadius);

            // draw filled portion
            float frac = (_maximum == _minimum) ? 0f : (float)(_value - _minimum) / (_maximum - _minimum);
            int filledWidth = (int)Math.Round(frac * trackWidth);
            Rectangle filledRect = new Rectangle(trackX, trackY, filledWidth, _barHeight);
            using (var b2 = new SolidBrush(Color.FromArgb(60, 130, 230)))
                FillRoundedRectangle(g, b2, filledRect, _trackCornerRadius);
            
            // draw blue border around the entire track
            using (var borderPen = new Pen(Color.FromArgb(60, 130, 230)))
                DrawRoundedRectangle(g, borderPen, fullRect, _trackCornerRadius);

            // draw thumb (rounded rectangle, vertical longer than horizontal)
            int thumbX = trackX + filledWidth;
            int thumbY = this.Height / 2;
            int roundedRadius = this.ThumbCornerRadius; // Use the ThumbCornerRadius property

            // choose hover or normal appearance for the thumb
            var currentThumbFill = _isHoverThumb ? this.ThumbHoverFillColor : this.ThumbFillColor;
            var currentThumbBorder = _isHoverThumb ? this.ThumbHoverBorderColor : this.ThumbBorderColor;
            var currentThumbBorderWidth = _isHoverThumb ? this.ThumbHoverBorderWidth : 1f;

            using (var b3 = new SolidBrush(currentThumbFill))
            using (var pen = new Pen(currentThumbBorder, currentThumbBorderWidth))
            {
                try { pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset; } catch { }
                // Create a rounded rectangle that is taller than it is wide
                RectangleF thumbRect = new RectangleF(
                    thumbX - _thumbRadius,
                    thumbY - ThumbHeight,
                    _thumbRadius * 1.5f,
                    ThumbHeight * 2f);

                // Fill the rounded rectangle
                FillRoundedRectangle(g, b3, thumbRect, roundedRadius);
                // Draw the border around the rounded rectangle
                DrawRoundedRectangle(g, pen, thumbRect, roundedRadius);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                Capture = true;
                SetValueFromPoint(e.Location);
                // update hover state on mouse down (if clicked on thumb or on the track)
                var was = _isHoverThumb;
                _isHoverThumb = IsPointInThumb(e.Location) || IsPointOverTrack(e.Location);
                if (was != _isHoverThumb) Invalidate();
            }
        }
 
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            // update hover state even when not dragging
            var wasHover = _isHoverThumb;
            // hover if mouse is over thumb OR over the track area
            _isHoverThumb = IsPointInThumb(e.Location) || IsPointOverTrack(e.Location);
            if (wasHover != _isHoverThumb) Invalidate();
 
            if (_isDragging)
            {
                SetValueFromPoint(e.Location);
            }
        }
 
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_isDragging)
            {
                _isDragging = false;
                Capture = false;
            }
            // keep hover if mouse still over thumb; already handled in MouseMove, but ensure redraw
            var was = _isHoverThumb;
            _isHoverThumb = IsPointInThumb(e.Location) || IsPointOverTrack(e.Location);
            if (was != _isHoverThumb) Invalidate();
        }
 
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_isHoverThumb) { _isHoverThumb = false; Invalidate(); }
        }
 
        // helper: determine if a point (client coords) is inside the thumb rect used in painting
        private bool IsPointInThumb(Point p)
        {
            int padding = Math.Max(_thumbRadius + 2, 6);
            int trackWidth = Math.Max(4, this.Width - padding * 2);
            float frac = (_maximum == _minimum) ? 0f : (float)(_value - _minimum) / (_maximum - _minimum);
            int filledWidth = (int)Math.Round(frac * trackWidth);
            int trackX = padding;
            int thumbX = trackX + filledWidth;
            int thumbY = this.Height / 2;
            RectangleF thumbRect = new RectangleF(
                thumbX - _thumbRadius,
                thumbY - ThumbHeight,
                _thumbRadius * 2f,
                ThumbHeight * 2f);
            return thumbRect.Contains(p.X, p.Y);
        }

        // helper: determine if a point (client coords) is over the track area (with small vertical tolerance)
        private bool IsPointOverTrack(Point p)
        {
            int padding = Math.Max(_thumbRadius + 2, 6);
            int trackWidth = Math.Max(4, this.Width - padding * 2);
            int trackX = padding;
            int trackY = this.Height / 2 - _barHeight / 2;
            int tolerance = Math.Max(8, _barHeight * 2); // allow a little vertical tolerance for easier hovering
            Rectangle trackRect = new Rectangle(trackX, trackY - tolerance / 2, trackWidth, _barHeight + tolerance);
            return trackRect.Contains(p);
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            int delta = e.Delta > 0 ? 1 : -1;
            this.Value = Math.Max(this.Minimum, Math.Min(this.Maximum, this.Value + delta));
        }

        private void SetValueFromPoint(Point p)
        {
            int padding = Math.Max(_thumbRadius + 2, 6);
            int trackWidth = Math.Max(4, this.Width - padding * 2);
            int x = Math.Max(0, Math.Min(trackWidth, p.X - padding));
            float frac = (float)x / (float)trackWidth;
            int newVal = _minimum + (int)Math.Round(frac * (_maximum - _minimum));
            this.Value = newVal;
        }

        // Method for drawing rounded rectangles
        private void DrawRoundedRectangle(Graphics g, Pen pen, RectangleF rect, float radius)
        {
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                gp.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                gp.AddArc(rect.X + rect.Width - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                gp.AddArc(rect.X, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                gp.CloseFigure();
                g.DrawPath(pen, gp);
            }
        }

        // Method for filling rounded rectangles
        private void FillRoundedRectangle(Graphics g, Brush brush, RectangleF rect, float radius)
        {
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                gp.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                gp.AddArc(rect.X + rect.Width - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 0, 90);
                gp.AddArc(rect.X, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 90, 90);
                gp.CloseFigure();
                g.FillPath(brush, gp);
            }
        }

        private void CoerceValue()
        {
            _value = Math.Max(_minimum, Math.Min(_maximum, _value));
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // keep a reasonable height
            if (this.Height < 24) this.Height = 24;
            Invalidate();
        }
    }
}
