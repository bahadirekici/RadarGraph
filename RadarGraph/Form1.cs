using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace RadarGraph
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer _radarTimer;
        private float _angle = 0;
        private int _angleP = 1;
        private PointF _radarArcP1, _radarArcP2;
        public Form1()
        {
            InitializeComponent();
            _radarTimer = new Timer(10);
            _radarTimer.Elapsed += _radarTimer_Elapsed;
            _radarTimer.Start();
        }

        void _radarTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _angle += _angleP;
            var len = _clipRect.Height / 2-10;
            _radarArcP1 = new PointF((_clipRect.X + _clipRect.Width / 2), (_clipRect.Height / 2));
            _radarArcP2 = new PointF((float)(_radarArcP1.X + Math.Cos((Math.PI / 180.0) * _angle) * len), (float)(_radarArcP1.Y + Math.Sin((Math.PI / 180.0) * _angle) * len));
            InvokeRefresh(null);

        }
        GraphicsPath _radarPath = new GraphicsPath();
        private RectangleF _clipRect;
        private PointF _centerPoint;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _radarPath.Reset();
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            _clipRect = new RectangleF(Width / 2 - Height / 2, 3, Height, Height);
            //orta nokta
            _centerPoint = new PointF((_clipRect.X + _clipRect.Width / 2), (_clipRect.Height / 2));

            _radarPath.AddEllipse(_clipRect);

            //clip
            e.Graphics.SetClip(_radarPath);


            //radar alanı
            using (var gBrush = new LinearGradientBrush(_clipRect, Color.DarkGreen, Color.LimeGreen, 130))
            {
                e.Graphics.FillPath(gBrush, _radarPath);
            }

            //orta nokta
            var radius = 6;
            var ceRect = new RectangleF((_clipRect.X + _clipRect.Width / 2) - radius / 2, (_clipRect.Height / 2) - radius / 2, radius, radius);
            using (var cPen = new Pen(Color.Lime, 1))
            {
                e.Graphics.DrawEllipse(cPen, ceRect);
            }

            // daireler
            using (var circPen = new Pen(Color.Lime))
            {
                for (int i = 0; i < Height; i++)
                {
                    if (i % 100 == 0)
                    {
                        var centerRect = new RectangleF((_clipRect.X + _clipRect.Width / 2) - i / 2, (_clipRect.Height / 2) - i / 2, i, i);
                        e.Graphics.DrawEllipse(circPen, centerRect);
                    }
                }
            }

            //dikey-yatay
            using (var ydPen = new Pen(Color.Lime, 1))
            {
                e.Graphics.DrawLine(ydPen, _centerPoint.X, 0, _centerPoint.X, _clipRect.Height);
                e.Graphics.DrawLine(ydPen, 0, _centerPoint.Y, _centerPoint.X + _clipRect.Width / 2, _centerPoint.Y);
            }
            //açı çizgileri
            var ang = 30;
            int an = 0;
            for (int i = 0; i < 360 / ang; i++)
            {
                an += ang;
                var len = _clipRect.Height/2;
                var center = _centerPoint;
                var p2 = new PointF((float)(center.X + Math.Cos((Math.PI / 180.0) * an) * len), (float)(center.Y + Math.Sin((Math.PI / 180.0) * an) * len));
                e.Graphics.DrawLine(new Pen(Color.Lime) { Width = 1 }, center.X, center.Y, p2.X, p2.Y);
            }


            //radar çizgisi
            var lineGBrush = new LinearGradientBrush(_radarArcP1, _radarArcP2, Color.Transparent, Color.Lime);
            using (var radarLine = new Pen(lineGBrush, 20))
            {
                e.Graphics.DrawLine(radarLine, _radarArcP1, _radarArcP2);
            }
            lineGBrush.Dispose();

            //çerçeve
            var cBrush = new LinearGradientBrush(_clipRect, Color.Transparent, Color.Lime, _angle);
            using (var cPen = new Pen(cBrush, 20))
            {
                e.Graphics.DrawEllipse(cPen, _clipRect);
            }
            cBrush.Dispose();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        void InvokeRefresh(object obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Object>(InvokeRefresh), obj);
            }
            else
            {
                Invalidate(_clipRect.ToRectangle(), true);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _angleP = trackBar1.Value;
        }
    }
}
