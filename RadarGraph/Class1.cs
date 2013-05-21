using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RadarGraph
{
    public static class Ext
    {
        public static Rectangle ToRectangle(this RectangleF rectangleF)
        {
            return new Rectangle((int)rectangleF.X, (int)rectangleF.Y, (int)rectangleF.Width,
                                     (int)rectangleF.Height);
        }
        public static RectangleF Constrict(this RectangleF rectangleF, double radius)
        {
            return new RectangleF((float)(rectangleF.X - radius/2),
                (float)(rectangleF.Y - radius/2),
                (float)(rectangleF.Width - radius), 
                (float)(rectangleF.Height - radius));
        }

    }
}
