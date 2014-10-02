using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Editor
{
    public static class DrawingContextHelper
    {
        public static void DrawPolyline(this DrawingContext dc, Point startPoint, PointCollection points, Pen pen)
        {
            PathGeometry geometry = new PathGeometry();
            PolyLineSegment segment = new PolyLineSegment();
            segment.Points = points;
            PathFigure fig = new PathFigure(startPoint, new[] { segment }, false);
            geometry.Figures.Add(fig);
            dc.DrawGeometry(null, pen, geometry);
        }

        public static void FillPolylineGeometry(this DrawingContext dc, Point startPoint, PointCollection points)
        {
            PathGeometry geometry = new PathGeometry();
            PolyLineSegment segment = new PolyLineSegment();
            segment.Points = points;
            PathFigure fig = new PathFigure(startPoint, new[] { segment }, true);
            geometry.Figures.Add(fig);
            dc.DrawGeometry(Brushes.Black, null, geometry);
        }
    }
}
