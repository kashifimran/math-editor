using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace Editor
{
    class DecorationDrawing : EquationBase
    {
        DecorationType decorationType;
        FormattedText firstSign; //only used by certain decorations
        FormattedText secondSign; //only used by certain decorations
        FormattedText bar;

        public DecorationDrawing(EquationContainer parent, DecorationType decorationType)
            : base(parent)
        {
            this.decorationType = decorationType;
            IsStatic = true;
            CreateDecorations();
            DetermineHeight();
        }

        void CreateDecorations()
        {
            switch (decorationType)
            {
                case DecorationType.DoubleArrow:
                    firstSign = FontFactory.GetFormattedText("\u02C2", FontType.STIXGeneral, FontSize * .7);
                    secondSign = FontFactory.GetFormattedText("\u02C3", FontType.STIXGeneral, FontSize * .7);
                    break;
                case DecorationType.LeftArrow:
                    firstSign = FontFactory.GetFormattedText("\u02C2", FontType.STIXGeneral, FontSize * .7);
                    break;
                case DecorationType.RightArrow:
                    firstSign = FontFactory.GetFormattedText("\u02C3", FontType.STIXGeneral, FontSize * .7);
                    break;
                //case DecorationType.RightHarpoonUpBarb:
                //case DecorationType.LeftHarpoonUpBarb:
                //case DecorationType.RightHarpoonDownBarb:
                //case DecorationType.LeftHarpoonDownBarb:
                //    firstSign = FontFactory.GetFormattedText("\u21BC", FontType.STIXGeneral, FontSize);
                //    break;
                case DecorationType.Parenthesis:
                    CreateParenthesisSigns();
                    break;
                case DecorationType.Tilde:
                    CreateTildeText();
                    break;
            }
        }

        void CreateParenthesisSigns()
        {
            if (Width < FontSize * .8)
            {
                FitFirstSignToWidth(FontType.STIXGeneral, "\u23DC", FontWeights.Bold);
            }
            else if (Width < FontSize * 2)
            {
                FitFirstSignToWidth(FontType.STIXSizeOneSym, "\u23DC");
            }
            else if (Width < FontSize * 3)
            {
                FitFirstSignToWidth(FontType.STIXSizeTwoSym, "\u23DC");
            }
            else
            {
                firstSign = FontFactory.GetFormattedText("\uE142", FontType.STIXNonUnicode, FontSize * .55);
                secondSign = FontFactory.GetFormattedText("\uE143", FontType.STIXNonUnicode, FontSize * .55);
                bar = FontFactory.GetFormattedText("\uE14A", FontType.STIXNonUnicode, FontSize * .55);
            }
        }

        public override double Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                if (decorationType == DecorationType.Tilde || decorationType == DecorationType.Parenthesis ||
                    decorationType == DecorationType.Hat)
                {
                    CreateDecorations();
                    DetermineHeight();
                }
            }
        }

        private void CreateTildeText()
        {
            if (Width < FontSize / 2)
            {
                FitFirstSignToWidth(FontType.STIXGeneral, "\u0303");
            }
            else if (Width < FontSize)
            {
                FitFirstSignToWidth(FontType.STIXSizeOneSym, "\u0303");
            }
            else if (Width < FontSize * 2)
            {
                FitFirstSignToWidth(FontType.STIXSizeTwoSym, "\u0303");
            }
            else if (Width < FontSize * 3)
            {
                FitFirstSignToWidth(FontType.STIXSizeThreeSym, "\u0303");
            }
            else if (Width < FontSize * 4)
            {
                FitFirstSignToWidth(FontType.STIXSizeFourSym, "\u0303");
            }
            else
            {
                FitFirstSignToWidth(FontType.STIXSizeFiveSym, "\u0303");
            }
        }

        private void FitFirstSignToWidth(FontType fontType, string unicodeChar)
        {
            FitFirstSignToWidth(fontType, unicodeChar, FontWeights.Normal);
        }

        private void FitFirstSignToWidth(FontType fontType, string unicodeChar, FontWeight weight)
        {
            double factor = .1;
            do
            {
                firstSign = FontFactory.GetFormattedText(unicodeChar, fontType, FontSize * factor);
                factor += .1;
            }
            while (Width > firstSign.Width - firstSign.OverhangLeading - firstSign.OverhangTrailing);
        }

        protected override void CalculateHeight()
        {
            DetermineHeight();
        }

        public override double Left
        {
            get
            {
                return base.Left;
            }
            set
            {
                base.Left = Math.Floor(value) + .5;
            }
        }
        public override double Top
        {
            get
            {
                return base.Top;
            }
            set
            {
                base.Top = Math.Floor(value) + .5;
            }
        }

        public override double Bottom
        {
            get
            {
                return Math.Floor(base.Bottom) + .5;
            }
            set
            {
                base.Bottom = value;
            }
        }

        public override double FontSize
        {
            get
            {
                return base.FontSize;
            }
            set
            {
                base.FontSize = value;
                CreateDecorations();
                DetermineHeight();
            }
        }

        void DetermineHeight()
        {
            switch (decorationType)
            {
                case DecorationType.Cross:
                case DecorationType.LeftCross:
                case DecorationType.RightCross:
                case DecorationType.StrikeThrough:
                    Height = 0;
                    break;
                case DecorationType.Bar:
                    Height = ThinPen.Thickness;
                    break;
                case DecorationType.DoubleBar:
                    Height = ThinPen.Thickness * 2 + FontSize * .1;
                    break;
                case DecorationType.Hat:
                    Height = FontSize * .2 + Width * .02;
                    break;
                case DecorationType.LeftArrow:
                case DecorationType.RightArrow:
                case DecorationType.DoubleArrow:
                case DecorationType.Parenthesis:
                case DecorationType.Tilde:
                    Height = firstSign.Extent;
                    break;
                case DecorationType.RightHarpoonUpBarb:
                case DecorationType.LeftHarpoonUpBarb:
                case DecorationType.RightHarpoonDownBarb:
                case DecorationType.LeftHarpoonDownBarb:
                    Height = FontSize * .2;
                    break;
                case DecorationType.Tortoise:
                    if (Width > FontSize * .333)
                    {
                        Height = FontSize * .25;
                    }
                    else
                    {
                        Height = FontSize * .2;
                    }
                    break;
            }
        }

        public override void DrawEquation(DrawingContext dc)
        {
            switch (decorationType)
            {
                case DecorationType.Bar:
                    dc.DrawLine(ThinPen, Location, new Point(Right, Top));
                    break;
                case DecorationType.DoubleBar:
                    dc.DrawLine(ThinPen, Location, new Point(Right, Top));
                    dc.DrawLine(ThinPen, new Point(Left, Bottom - ThinPen.Thickness),
                                             new Point(Right, Bottom - ThinPen.Thickness));
                    break;
                case DecorationType.Hat:
                    dc.DrawPolyline(new Point(Left, Bottom - FontSize * .02),
                                    new PointCollection 
                                    {   
                                        new Point(MidX, Top + FontSize * .03), 
                                        new Point(Right, Bottom - FontSize * .02) 
                                    },
                                    ThinPen);
                    break;
                case DecorationType.LeftArrow:
                    firstSign.DrawTextTopLeftAligned(dc, Location);
                    dc.DrawLine(ThinPen, new Point(Left + FontSize * .06, MidY), new Point(Right, MidY));
                    break;
                case DecorationType.RightArrow:
                    firstSign.DrawTextTopRightAligned(dc, new Point(Right, Top));
                    dc.DrawLine(ThinPen, new Point(Left, MidY), new Point(Right - FontSize * .06, MidY));
                    break;
                case DecorationType.DoubleArrow:
                    DrawDoubleArrow(dc);
                    break;
                case DecorationType.Parenthesis:
                    DrawParentheses(dc);
                    break;
                case DecorationType.RightHarpoonUpBarb:
                    DrawRightHarpoonUpBarb(dc);
                    break;
                case DecorationType.RightHarpoonDownBarb:
                    DrawRightHarpoonDownBarb(dc);
                    break;
                case DecorationType.LeftHarpoonUpBarb:
                    DrawLeftHarpoonUpBarb(dc);
                    break;
                case DecorationType.LeftHarpoonDownBarb:
                    DrawLeftHarpoonDownBarb(dc);
                    break;
                case DecorationType.Tilde:
                    firstSign.DrawTextTopLeftAligned(dc, Location);
                    break;
                case DecorationType.Tortoise:
                    DrawTortoise(dc);
                    break;
                case DecorationType.Cross:
                    dc.DrawLine(ThinPen, ParentEquation.Location, new Point(Right, ParentEquation.Bottom));
                    dc.DrawLine(ThinPen, new Point(Left, ParentEquation.Bottom), new Point(Right, ParentEquation.Top));
                    break;
                case DecorationType.LeftCross:
                    dc.DrawLine(ThinPen, ParentEquation.Location, new Point(Right, ParentEquation.Bottom));
                    break;
                case DecorationType.RightCross:
                    dc.DrawLine(ThinPen, new Point(Left, ParentEquation.Bottom), new Point(Right, ParentEquation.Top));
                    break;
                case DecorationType.StrikeThrough:
                    dc.DrawLine(ThinPen, new Point(Left, ParentEquation.MidY), new Point(Right, ParentEquation.MidY));
                    break;
            }
        }

        private void DrawDoubleArrow(DrawingContext dc)
        {
            if (Width < FontSize * 0.8)
            {
                FormattedText text = FontFactory.GetFormattedText("\u2194", FontType.STIXGeneral, Width * 1.5);
                double factor = .1;
                do
                {
                    text = FontFactory.GetFormattedText("\u2194", FontType.STIXGeneral, FontSize * factor);
                    factor += .1;
                }
                while (Width > text.GetFullWidth());                
                text.DrawTextTopLeftAligned(dc, Location);
            }
            else
            {
                firstSign.DrawTextTopLeftAligned(dc, Location);
                secondSign.DrawTextTopRightAligned(dc, new Point(Right, Top));
                dc.DrawLine(ThinPen, new Point(Left + FontSize * .06, MidY), new Point(Right - FontSize * .06, MidY));
            }
        }

        private void DrawParentheses(DrawingContext dc)
        {
            if (Width < FontSize * 3)
            {
                firstSign.DrawTextTopLeftAligned(dc, Location);
            }
            else
            {
                firstSign.DrawTextTopLeftAligned(dc, Location);
                secondSign.DrawTextTopLeftAligned(dc, new Point(Right - secondSign.Width + secondSign.OverhangLeading, Top));
                //dc.DrawLine(StandardPen, new Point(Left + secondSign.Width + secondSign.OverhangLeading, Top + FontSize * .03), new Point(Right - (secondSign.Width + secondSign.OverhangLeading), Top + FontSize * .03));
                double left = Left + firstSign.GetFullWidth() * .85;
                double right = Right - secondSign.GetFullWidth() * .85;
                while (left < right)
                {
                    bar.DrawTextTopLeftAligned(dc, new Point(left, Top));
                    left += bar.GetFullWidth() * .8;
                    double shoot = (left + bar.GetFullWidth() * .8) - right;
                    if (shoot > 0)
                    {
                        left -= shoot;
                        bar.DrawTextTopLeftAligned(dc, new Point(left, Top));
                        break;
                    }
                }
            }
        }

        private void DrawLeftHarpoonUpBarb(DrawingContext dc)
        {
            PointCollection points = new PointCollection {  
                                                            new Point(Left + FontSize * .3, Top),
                                                            //new Point(Left + FontSize * .31, Top + FontSize * .041),
                                                            new Point(Left + FontSize * .18, Bottom - FontSize * .06),
                                                            new Point(Right, Bottom - FontSize * .06),
                                                            new Point(Right, Bottom- FontSize * .02)
                                                         };
            dc.FillPolylineGeometry(new Point(Left, Bottom - FontSize * .02), points);
        }

        private void DrawRightHarpoonUpBarb(DrawingContext dc)
        {
            PointCollection points = new PointCollection {  
                                                            new Point(Right - FontSize * .3, Top),
                                                            //new Point(Right - FontSize * .31, Top + FontSize * .041),
                                                            new Point(Right - FontSize * .18, Bottom - FontSize * .06),
                                                            new Point(Left, Bottom - FontSize * .06),
                                                            new Point(Left, Bottom - FontSize * .02)
                                                         };
            dc.FillPolylineGeometry(new Point(Right, Bottom - FontSize * .02), points);
        }

        private void DrawLeftHarpoonDownBarb(DrawingContext dc)
        {
            PointCollection points = new PointCollection {  
                                                            new Point(Left + FontSize * .3, Bottom),
                                                            //new Point(Left + FontSize * .31, Bottom - FontSize * .041),
                                                            new Point(Left + FontSize * .18, Top + FontSize * .06),
                                                            new Point(Right, Top + FontSize * .06),
                                                            new Point(Right, Top + FontSize * .02)
                                                         };
            dc.FillPolylineGeometry(new Point(Left, Top + FontSize * .02), points);
        }

        private void DrawRightHarpoonDownBarb(DrawingContext dc)
        {
            PointCollection points = new PointCollection {  
                                                            new Point(Right - FontSize * .3, Bottom),
                                                            //new Point(Right - FontSize * .31, Bottom - FontSize * .041),
                                                            new Point(Right - FontSize * .18, Top + FontSize * .06),
                                                            new Point(Left, Top + FontSize * .06),
                                                            new Point(Left, Top + FontSize * .02)
                                                         };
            dc.FillPolylineGeometry(new Point(Right, Top + FontSize * .02), points);
        }

        private void DrawTortoise(DrawingContext dc)
        {
            PointCollection points = new PointCollection {  
                                                            new Point(Left + Height * .5, Top),
                                                            new Point(Right - Height * .5, Top),
                                                            new Point(Right, Bottom),
                                                            new Point(Right - Height * .2, Bottom),
                                                            new Point(Right - Height * .7, Top + Height * .3),
                                                            new Point(Left + Height * .7, Top + Height * .3),
                                                            new Point(Left + Height * .2, Bottom)
                                                         };
            dc.FillPolylineGeometry(new Point(Left, Bottom), points);
        }
    }
}
