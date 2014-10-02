using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace Editor
{
    public class DecoratedCharacter : EquationBase
    {
        List<CharacterDecorationInfo> decorations = new List<CharacterDecorationInfo>();
        FormattedText charFt;
        public TextEquation Previous { get; set; }
        public TextEquation Next     { get; set; }

        public DecoratedCharacter(EquationContainer parent, TextEquation previous, CharacterDecorationType cdt, Position position, string sign)
            : base(parent)
        {
            this.Previous = previous;
            this.charFt = textManager.GetFormattedText(previous.Text[previous.CaretIndex - 1].ToString(), previous.GetFormats()[previous.CaretIndex - 1]);            
            previous.ConsumeKey(System.Windows.Input.Key.Back);
            Height = FontSize;
            decorations.Add(new CharacterDecorationInfo() { DecorationType = cdt, Position = position, UnicodeString = sign });
            Width = charFt.Width;
        }

        public override void DrawEquation(DrawingContext dc)
        {
            base.DrawEquation(dc);
            charFt.DrawTextLeftAligned(dc, Location);
            int done = 0;
            double left = Left;
            double hCenter;
            FormattedText ft = null;
            string text = "";
            int count;
            for (int i = 0; i < decorations.Count; i++)
            {
                //var item = decorations[i];
                //var charFt = textManager.GetFormattedText(character, 0);
                //double decoWidth = 0; //;GetDecoratedCharWidth(charFt, item.ToList(), i, out hCenter);
                //count = item.Key - done;
                //if (count > 0)
                //{
                //    text = textData.ToString(done, count);
                //    ft = textManager.GetFormattedText(text, formats.Skip(done).Take(count).ToList());
                //    ft.DrawTextLeftAligned(dc, new Point(left, Top));
                //    done += ft.Text.Length;
                //    left += ft.GetFullWidth() + ft.OverhangTrailing;
                //}
                //hCenter -= charFt.OverhangTrailing;
                //if (item.Key > 0)
                //{
                //    hCenter += charFt.OverhangLeading;
                //}
                //dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(left + hCenter, Top), new Point(left + hCenter, Bottom));
                //dc.DrawLine(new Pen(Brushes.Red, 1), new Point(left, Top), new Point(left, Bottom));
                //charFt.DrawTextCenterAligned(dc, new Point(left + hCenter, Top));
                ////DrawDecorations(dc, item.ToList(), charFt, i, left + hCenter);
                ////DrawRightDecorations(dc, item.ToList(), left + charFt.GetRight() - (item.Key > 0 ? 0 : charFt.OverhangLeading), formats[i]);
                ////DrawLeftDecorations(dc, item.ToList(), left, formats[i]);
                ////DrawFaceDecorations(dc, charFt, item.ToList(), hCenter);
                ////DrawTopDecorations(dc, charFt, item.ToList(), hCenter, formats[i]);
                ////DrawBottomDecorations(dc, charFt, item.ToList(), hCenter, formats[i]);
                //left += decoWidth + charFt.OverhangTrailing + charFt.OverhangLeading;
                //done++;
            }
        }

        //void DrawDecorations(DrawingContext dc, List<CharacterDecorationInfo> decorationList, FormattedText ft, int index, double hCenter)
        //{
        //    double offset = FontSize * .05;
        //    //character metrics    
        //    double topPixel = ft.Height + ft.OverhangAfter - ft.Extent; //ft.Baseline - ft.Extent + descent;
        //    double descent = ft.Height - ft.Baseline + ft.OverhangAfter;
        //    double halfCharWidth = ft.GetFullWidth() / 2;
        //    double right = hCenter + halfCharWidth + offset;
        //    double left = hCenter - halfCharWidth - offset;
        //    double top = Top + topPixel - offset;
        //    double bottom = Top + ft.Baseline + descent + offset;
        //}

        //private void DrawTopDecorations(DrawingContext dc, FormattedText ft, List<CharacterDecorationInfo> cdiList, double center, int formatId)
        //{
        //    var topDecorations = (from x in cdiList where x.Position == Position.Top select x).ToList();
        //    if (topDecorations.Count > 0)
        //    {
        //        double top = Top + ft.Height + ft.OverhangAfter - ft.Extent - FontSize * .1;
        //        foreach (var d in topDecorations)
        //        {
        //            string text = d.UnicodeString;
        //            var sign = textManager.GetFormattedText(text, textManager.GetFormatIdForNewStyle(formatId, FontStyles.Normal));
        //            sign.DrawTextBottomCenterAligned(dc, new Point(center, top));
        //            top -= sign.Extent + FontSize * .08;
        //        }
        //    }
        //}

        //private void DrawBottomDecorations(DrawingContext dc, FormattedText ft, List<CharacterDecorationInfo> cdiList, double hCenter, int formatId)
        //{
        //    var bottomDecorations = (from x in cdiList where x.Position == Position.Bottom select x).ToList();
        //    if (bottomDecorations.Count > 0)
        //    {
        //        double bottom = Top + ft.Height + ft.OverhangAfter + FontSize * .2;
        //        foreach (var d in bottomDecorations)
        //        {
        //            string text = d.UnicodeString;
        //            var sign = textManager.GetFormattedText(text, textManager.GetFormatIdForNewStyle(formatId, FontStyles.Normal));
        //            sign.DrawTextBottomCenterAligned(dc, new Point(hCenter, bottom));
        //            bottom += sign.Extent + FontSize * .08;
        //        }
        //    }
        //}

        //private void DrawLeftDecorations(DrawingContext dc, List<CharacterDecorationInfo> cdiList, double left, int formatId)
        //{
        //    var leftDecorations = (from x in cdiList where x.Position == Position.TopLeft select x).ToList();
        //    if (leftDecorations.Count > 0)
        //    {
        //        string s = "";
        //        foreach (var d in leftDecorations)
        //        {
        //            s = s + d.UnicodeString;
        //        }
        //        var formattedText = textManager.GetFormattedText(s, textManager.GetFormatIdForNewStyle(formatId, FontStyles.Normal));
        //        formattedText.DrawTextRightAligned(dc, new Point(left, Top));
        //    }
        //}

        //private void DrawRightDecorations(DrawingContext dc, List<CharacterDecorationInfo> cdiList, double right, int formatId)
        //{
        //    var rightDecorations = (from x in cdiList where x.Position == Position.TopRight select x).ToList();
        //    if (rightDecorations.Count > 0)
        //    {
        //        string s = "";
        //        foreach (var d in rightDecorations)
        //        {
        //            s = s + d.UnicodeString;
        //        }
        //        dc.DrawText(textManager.GetFormattedText(s, textManager.GetFormatIdForNewStyle(formatId, FontStyles.Normal)), new Point(right, Top));
        //    }
        //}

        ////index = index of the decorated character in this.textData
        //private void DrawFaceDecorations(DrawingContext dc, FormattedText charText, List<CharacterDecorationInfo> cdiList, double hCenter)
        //{
        //    var decorations = (from x in cdiList where x.Position == Position.Over select x).ToList();
        //    if (decorations.Count > 0)
        //    {
        //        double offset = FontSize * .05;
        //        double top = Top + charText.Height + charText.OverhangAfter - charText.Extent; //ft.Baseline - ft.Extent + descent;
        //        double bottom = top + charText.Extent; //charText.Height - charText.Baseline + charText.OverhangAfter;
        //        double vCenter = top + charText.Extent / 2;
        //        double left = hCenter - charText.GetFullWidth() / 2 - offset;
        //        double right = hCenter + charText.GetFullWidth() / 2 + offset;

        //        Pen pen = PenManager.GetPen(FontSize * .035);
        //        foreach (var d in decorations)
        //        {
        //            switch (d.DecorationType)
        //            {
        //                case CharacterDecorationType.Cross:
        //                    dc.DrawLine(pen, new Point(left, top), new Point(right, bottom));
        //                    dc.DrawLine(pen, new Point(left, bottom), new Point(right, top));
        //                    break;
        //                case CharacterDecorationType.LeftCross:
        //                    dc.DrawLine(pen, new Point(left, top), new Point(right, bottom));
        //                    break;
        //                case CharacterDecorationType.RightCross:
        //                    dc.DrawLine(pen, new Point(left, bottom), new Point(right, top));
        //                    break;
        //                case CharacterDecorationType.LeftUprightCross:
        //                    dc.DrawLine(pen, new Point(hCenter - FontSize * .08, top - FontSize * 0.04), new Point(hCenter + FontSize * .08, bottom + FontSize * 0.04));
        //                    break;
        //                case CharacterDecorationType.RightUprightCross:
        //                    dc.DrawLine(pen, new Point(hCenter + FontSize * .08, top - FontSize * 0.04), new Point(hCenter - FontSize * .08, bottom + FontSize * 0.04));
        //                    break;
        //                case CharacterDecorationType.StrikeThrough:
        //                    dc.DrawLine(pen, new Point(left, vCenter), new Point(right, vCenter));
        //                    break;
        //                case CharacterDecorationType.DoubleStrikeThrough:
        //                    dc.DrawLine(pen, new Point(left, vCenter - FontSize * .05), new Point(right, vCenter - FontSize * .05));
        //                    dc.DrawLine(pen, new Point(left, vCenter + FontSize * .05), new Point(right, vCenter + FontSize * .05));
        //                    break;
        //                case CharacterDecorationType.VStrikeThrough:
        //                    dc.DrawLine(pen, new Point(hCenter, top - FontSize * .05), new Point(hCenter, bottom + FontSize * .05));
        //                    break;
        //                case CharacterDecorationType.VDoubleStrikeThrough:
        //                    dc.DrawLine(pen, new Point(hCenter - FontSize * .05, top - FontSize * .05), new Point(hCenter - FontSize * .05, bottom + FontSize * .05));
        //                    dc.DrawLine(pen, new Point(hCenter + FontSize * .05, top - FontSize * .05), new Point(hCenter + FontSize * .05, bottom + FontSize * .05));
        //                    break;
        //            }
        //        }
        //    }
        //}


        public void AddDecoration(CharacterDecorationInfo cdi)
        {
            //if (cdi.DecorationType == CharacterDecorationType.None)
            //{
            //    decorations.RemoveAll(x => x.Index == caretIndex - 1);
            //}
            //else if (!char.IsWhiteSpace(textData[caretIndex - 1]))
            //{
            //    cdi.Index = caretIndex - 1;
            //    decorations.Add(cdi);
            //}
            //FormatText();
        }

        /*
        private double GetDecoratedCharWidth(FormattedText ft, List<CharacterDecorationInfo> decorationList, int index, out double hCenter)
        {
            double width = ft.GetFullWidth();
            double charWidth = width;
            var lhList = from d in decorationList where d.Position == Position.TopLeft select d;
            var rhList = from d in decorationList where d.Position == Position.TopRight select d;
            var vList = from d in decorationList where d.Position == Position.Top || d.Position == Position.Bottom select d;
            var oList = from d in decorationList where d.Position == Position.Over select d;
            string text = "";
            foreach (var v in lhList)
            {
                text += v.UnicodeString;
            }
            if (text.Length > 0)
            {
                var t = textManager.GetFormattedText(text, formats[index]);
                width += t.GetFullWidth();
            }
            foreach (var v in vList)
            {
                var t = textManager.GetFormattedText(v.UnicodeString, formats[index]);
                charWidth = Math.Max(t.GetFullWidth(), charWidth);
            }
            width = Math.Max(width, charWidth);
            foreach (var v in oList)
            {
                if (v.DecorationType == CharacterDecorationType.Cross || v.DecorationType == CharacterDecorationType.DoubleStrikeThrough ||
                                v.DecorationType == CharacterDecorationType.LeftCross || v.DecorationType == CharacterDecorationType.RightCross ||
                                v.DecorationType == CharacterDecorationType.StrikeThrough)
                {
                    width = Math.Max(width, ft.GetFullWidth() + FontSize * .05);
                }
            }
            hCenter = width - charWidth / 2;
            text = "";
            foreach (var v in rhList)
            {
                text += v.UnicodeString;
            }
            if (text.Length > 0)
            {
                var t = textManager.GetFormattedText(text, formats[index]);
                width += t.GetFullWidth();
            }
            return width;
        }
        */
    }
}
