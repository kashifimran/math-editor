using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Editor
{

    public static class PenManager
    {
        static Dictionary<double, Pen> bevelPens = new Dictionary<double, Pen>();
        static Dictionary<double, Pen> miterPens = new Dictionary<double, Pen>();
        static Dictionary<double, Pen> roundPens = new Dictionary<double, Pen>();
        static Dictionary<double, Pen> whitePens = new Dictionary<double, Pen>();

        static object bevelLock = new object();
        static object miterLock = new object();
        static object roundLock = new object();

        static object whiteLock = new object();

        public static Pen GetWhitePen(double thickness)
        {
            return GetPen(whiteLock, whitePens, thickness, PenLineJoin.Miter, Brushes.White);
        }

        public static Pen GetPen(double thickness, PenLineJoin lineJoin = PenLineJoin.Bevel)
        {
            if (lineJoin == PenLineJoin.Bevel)
            {
                return GetPen(bevelLock, bevelPens, thickness, lineJoin);
            }
            else if (lineJoin == PenLineJoin.Miter)
            {
                return GetPen(miterLock, miterPens, thickness, lineJoin);
            }
            else
            {
                return GetPen(roundLock, roundPens, thickness, lineJoin);
            }
        }

        static Pen GetPen(object lockObj, Dictionary<double, Pen> penDictionary, double thickness, PenLineJoin lineJoin, Brush brush=null)
        {
            lock (lockObj)
            {
                thickness = Math.Round(thickness, 1);
                if (!penDictionary.ContainsKey(thickness))
                {
                    Pen pen = new Pen(brush ?? Brushes.Black, thickness);
                    pen.LineJoin = lineJoin;
                    pen.Freeze();
                    penDictionary.Add(thickness, pen);
                }
                return penDictionary[thickness];
            }
        }
    }
}
