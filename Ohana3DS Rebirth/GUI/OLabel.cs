﻿//OLabel made for Ohana3DS by gdkchan
//Custom Label control with Image support

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Ohana3DS_Rebirth.GUI
{
    public partial class OLabel : Control
    {
        private bool autoSize;
        private bool centered;

        public OLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
        }

        public bool AutomaticSize
        {
            get
            {
                return autoSize;
            }
            set
            {
                autoSize = value;
                Refresh();
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                Refresh();
            }
        }

        public bool Centered
        {
            get
            {
                return centered;
            }
            set
            {
                centered = value;
                Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            string text = DrawingHelper.clampText(e.Graphics, Text, Font, Width);
            SizeF textSize = DrawingHelper.measureText(e.Graphics, text, Font);
            if (autoSize) Size = new Size((int)textSize.Width, (int)textSize.Height);
            int x = centered ? (Width / 2) - ((int)textSize.Width / 2) : 0;
            e.Graphics.DrawString(text, Font, new SolidBrush(Enabled ? ForeColor : Color.Silver), new Point(x, (Height / 2) - ((int)textSize.Height / 2)));

            base.OnPaint(e);
        }
    }
}
