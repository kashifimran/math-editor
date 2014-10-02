using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Editor
{
    public static class FormattedTextHelper
    {
        public static void DrawTextTopLeftAligned(this FormattedText text, DrawingContext dc, Point topLeft)
        {
            //double descent = text.Height - text.Baseline + text.OverhangAfter;
            //double topExtra = text.Baseline - text.Extent + descent;
            double topExtra = text.Height + text.OverhangAfter - text.Extent;
            double padding = 0;
            if (text.Text.Length > 0 && !char.IsWhiteSpace(text.Text[0]))
            {
                padding = text.OverhangLeading;
            }
            dc.DrawText(text, new Point(topLeft.X - padding, topLeft.Y - topExtra));
        }

        public static void DrawText (this FormattedText ft, DrawingContext dc, Point topLeft)
        {
            if (ft.Text.Length > 0)
            {
                dc.DrawText(ft, new Point(topLeft.X, topLeft.Y));
            }
        }

        public static void DrawTextLeftAligned(this FormattedText ft, DrawingContext dc, Point topLeft)
        {
            if (ft.Text.Length > 0)
            {
                if (char.IsWhiteSpace(ft.Text[0]))
                {
                    dc.DrawText(ft, new Point(topLeft.X, topLeft.Y));
                }
                else
                {
                    dc.DrawText(ft, new Point(topLeft.X - ft.OverhangLeading, topLeft.Y));
                }
            }
        }

        public static void DrawTextCenterAligned(this FormattedText ft, DrawingContext dc, Point hCenter)
        {
            if (ft.Text.Length > 0)
            {
                double width = ft.GetFullWidth();
                dc.DrawText(ft, new Point(hCenter.X - width / 2 - ft.OverhangLeading, hCenter.Y));
            }
        }

        public static void DrawTextRightAligned(this FormattedText text, DrawingContext dc, Point topRight)
        {
           dc.DrawText(text, new Point(topRight.X - text.GetFullWidth(), topRight.Y));
        }


        public static void DrawTextTopRightAligned(this FormattedText text, DrawingContext dc, Point topRight)
        {
            double descent = text.Height - text.Baseline + text.OverhangAfter;
            double topExtra = text.Baseline - text.Extent + descent;
            dc.DrawText(text, new Point(topRight.X - text.GetFullWidth() - text.OverhangLeading, topRight.Y - topExtra));
        }

        public static void DrawTextBottomLeftAligned(this FormattedText text, DrawingContext dc, Point bottomLeft)
        {
            double descent = text.Height - text.Baseline + text.OverhangAfter;
            double topExtra = text.Baseline - text.Extent + descent;
            dc.DrawText(text, new Point(bottomLeft.X - text.OverhangLeading, bottomLeft.Y - topExtra - text.Extent));
        }

        public static void DrawTextBottomCenterAligned(this FormattedText text, DrawingContext dc, Point bottomCenter)
        {
            double descent = text.Height - text.Baseline + text.OverhangAfter;
            double topExtra = text.Baseline - text.Extent + descent;
            dc.DrawText(text, new Point(bottomCenter.X - text.OverhangLeading - text.GetFullWidth()/2, bottomCenter.Y - topExtra - text.Extent));
        }

        public static void DrawTextTopCenterAligned(this FormattedText text, DrawingContext dc, Point topCenter)
        {
            double descent = text.Height - text.Baseline + text.OverhangAfter;
            double topExtra = text.Baseline - text.Extent + descent;
            dc.DrawText(text, new Point(topCenter.X - text.OverhangLeading - text.GetFullWidth() / 2, topCenter.Y - topExtra));
        }

        public static void DrawTextBottomRightAligned(this FormattedText text, DrawingContext dc, Point bottomRight)
        {
            double descent = text.Height - text.Baseline + text.OverhangAfter;
            double topExtra = text.Baseline - text.Extent + descent;
            dc.DrawText(text, new Point(bottomRight.X - text.GetFullWidth() - text.OverhangLeading, bottomRight.Y - topExtra - text.Extent));
        }

        public static double GetFullWidth(this FormattedText ft)
        {
            if (ft.Text.Length > 0)
            {
                if (char.IsWhiteSpace(ft.Text[0]) && char.IsWhiteSpace(ft.Text[ft.Text.Length-1]))
                {
                    return ft.WidthIncludingTrailingWhitespace;
                }
                else if (char.IsWhiteSpace(ft.Text[0]))
                {
                    return ft.WidthIncludingTrailingWhitespace - ft.OverhangTrailing;
                }
                else if (char.IsWhiteSpace(ft.Text[ft.Text.Length - 1]))
                {
                    return ft.WidthIncludingTrailingWhitespace - ft.OverhangLeading;
                }
                else
                {
                    return ft.WidthIncludingTrailingWhitespace - ft.OverhangLeading - ft.OverhangTrailing;
                }
            }
            else
            {
                return 0;
            }
        }
        /*
        //double width = formattedText.WidthIncludingTrailingWhitespace;
        //if (formattedText.Text.Length > 0)
        //{
        //    if (!char.IsSeparator(formattedText.Text[0]))
        //    {
        //        if (formattedText.OverhangLeading < 0)
        //        {
        //            width -= formattedText.OverhangLeading;
        //        }
        //    }
        //    if (!char.IsSeparator(formattedText.Text[formattedText.Text.Length - 1]))
        //    {
        //        width -= formattedText.OverhangTrailing;
        //    }
        //}
        //return width;
        */
        
        public static double Descent(this FormattedText ft)
        {
            return ft.Height - ft.Baseline + ft.OverhangAfter;
        }

        public static double TopExtra(this FormattedText ft)
        {
            // = ft.Baseline - ft.Extent + ft.Descent() 
            // = ft.Baseline - ft.Extent + (ft.Height - ft.Baseline + ft.OverhangAfter)
            return ft.Height - ft.Extent + ft.OverhangAfter;
        }

        public static double GetRight(this FormattedText ft)
        {
            if (ft.Text.Length > 0)
            {
                if (char.IsWhiteSpace(ft.Text[ft.Text.Length - 1]))
                {
                    return ft.WidthIncludingTrailingWhitespace;
                }                
                else
                {
                    return ft.WidthIncludingTrailingWhitespace - ft.OverhangTrailing;
                }
            }
            else
            {
                return 0;
            }
        }  
    }
}
