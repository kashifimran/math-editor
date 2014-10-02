using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System.Xml.Linq;

namespace Editor
{
    //Only those EquationBase classes should use it which are able to remeber the formats (as of May 15, 2013 only TextEquation)!!
    public class TextManager
    {
        List<TextFormat> formattingList = new List<TextFormat>();
        List<TextDecorationCollection> decorations = new List<TextDecorationCollection>();
        Dictionary<int, int> mapping = new Dictionary<int, int>();
        List<TextFormat> formattingListBeforeSave = null;

        public TextManager()
        {
            TextDecorationCollection tdc = new TextDecorationCollection();
            tdc.Add(TextDecorations.Underline);
            decorations.Add(tdc);
        }

        public void OptimizeForSave(EquationRoot root)
        {
            mapping.Clear();
            List<TextFormat> newList = new List<TextFormat>();
            HashSet<int> usedOnes = root.GetUsedTextFormats();                 
            foreach (int i in usedOnes)
            {
                TextFormat tf = formattingList[i];
                tf.Index = newList.Count;
                newList.Add(tf);
                mapping.Add(i, tf.Index);
            }
            root.ResetTextFormats(mapping);
            formattingListBeforeSave = formattingList;
            formattingList = newList;
        }

        public void RestoreAfterSave(EquationRoot root)
        {   
            Dictionary<int, int> oldMapping = new Dictionary<int, int>();
            foreach (int i in mapping.Keys)
            {
                oldMapping.Add(mapping[i], i);
                TextFormat tf = formattingListBeforeSave[i];
                tf.Index = i;
            }            
            root.ResetTextFormats(oldMapping);
            formattingList = formattingListBeforeSave;
        }

        public XElement Serialize()
        {            
            XElement thisElement = new XElement(GetType().Name);
            XElement children = new XElement("Formats");
            foreach (TextFormat tf in formattingList)
            {
                children.Add(tf.Serialize());
            }
            thisElement.Add(children);
            return thisElement;
        }

        void AddToList(TextFormat tf)
        {
            tf.Index = formattingList.Count;
            formattingList.Add(tf);
        }

        public void DeSerialize(XElement xElement)
        {
            formattingList.Clear();
            XElement children = xElement.Element("Formats");
            foreach (XElement xe in children.Elements())
            {                
                AddToList(TextFormat.DeSerialize(xe));
            }
        }

        public void ProcessPastedXML(XElement rootXE)
        {
            //XElement thisElement = rootXE.Element(GetType().Name);
            XElement[] formatElements = rootXE.Element(GetType().Name).Elements("Formats").Elements().ToArray();
            IEnumerable<XElement> formats = rootXE.Descendants(typeof(TextEquation).Name).Descendants("Formats");
            Dictionary<int, int> allFormatIds = new Dictionary<int, int>();
            foreach (XElement xe in formats)
            {
                if (xe.Value.Length > 0)
                {
                    string[] formatStrings = xe.Value.Split(',');
                    foreach (string s in formatStrings)
                    {
                        int id = int.Parse(s);
                        if (!allFormatIds.Keys.Contains(id))
                        {
                            allFormatIds.Add(id, id);
                        }
                    }
                }
            }
            for (int i=0;i < allFormatIds.Count;i++)
            {
                int key = allFormatIds.ElementAt(i).Key;
                TextFormat tf = TextFormat.DeSerialize(formatElements[key]);
                TextFormat match = formattingList.Where(x =>
                    {
                        return x.FontSize == Math.Round(tf.FontSize, 1) &&
                               x.FontType == tf.FontType &&
                               x.FontStyle == tf.FontStyle &&
                               x.UseUnderline == tf.UseUnderline &&
                               Color.AreClose(x.TextBrush.Color, tf.TextBrush.Color) &&
                               x.FontWeight == tf.FontWeight;

                    }).FirstOrDefault();

                int newValue = 0;
                if (match == null)
                {
                    AddToList(tf);
                    newValue = tf.Index;
                }
                else
                {
                    newValue = match.Index;
                }
                allFormatIds[key] = newValue;
            }
            IEnumerable<XElement> textElements = rootXE.Descendants(typeof(TextEquation).Name);
            foreach (XElement xe in textElements)
            {
                XElement formatsElement = xe.Elements("Formats").FirstOrDefault();
                if (formatsElement != null)
                {
                    StringBuilder strBuilder = new StringBuilder();
                    string[] formatStrings = formatsElement.Value.Split(',');
                    foreach (string s in formatStrings)
                    {
                        if (s.Length > 0)
                        {
                            int id = int.Parse(s);
                            strBuilder.Append(allFormatIds[id] + ",");
                        }
                    }
                    if (strBuilder.Length > 0)
                    {
                        strBuilder.Remove(strBuilder.Length - 1, 1);
                    }
                    formatsElement.Value = strBuilder.ToString();
                }                
            }
        }

        public int GetFormatId(double fontSize, FontType fontType, FontStyle fontStyle, FontWeight fontWeight, SolidColorBrush textBrush, bool useUnderline)
        {
            double num = Math.Round(fontSize, 1);
            TextFormat tf = formattingList.Where(x =>
            {
                return x.FontSize == Math.Round(fontSize, 1) &&
                       x.FontType == fontType &&
                       x.FontStyle == fontStyle &&
                       x.UseUnderline == useUnderline &&
                       Color.AreClose(x.TextBrush.Color, textBrush.Color) &&
                       x.FontWeight == fontWeight;

            }).FirstOrDefault();
            if (tf == null)
            {
                tf = new TextFormat(fontSize, fontType, fontStyle, fontWeight, textBrush, useUnderline);
                AddToList(tf);
            } 
            return tf.Index;
        }

        public int GetFormatIdForNewFont(int oldId, FontType fontType)
        {
            TextFormat oldFormat = formattingList[oldId];

            TextFormat tf = formattingList.Where(x =>
            {
                return x.FontSize == oldFormat.FontSize &&
                       x.FontType == fontType &&
                       x.FontStyle == oldFormat.FontStyle &&
                       x.UseUnderline == oldFormat.UseUnderline &&
                       Color.AreClose(x.TextBrush.Color, oldFormat.TextBrush.Color) &&
                       x.FontWeight == oldFormat.FontWeight;

            }).FirstOrDefault();
            if (tf == null)
            {
                tf = new TextFormat(oldFormat.FontSize, fontType, oldFormat.FontStyle, oldFormat.FontWeight, oldFormat.TextBrush, oldFormat.UseUnderline);
                AddToList(tf);
            }
            return tf.Index;
        }
        
        public int GetFormatIdForNewSize(int oldId, double newSize)
        {
            TextFormat oldFormat = formattingList[oldId];

            TextFormat tf = formattingList.Where(x =>
            {
                return x.FontSize == Math.Round(newSize, 1) &&
                       x.FontType == oldFormat.FontType &&
                       x.FontStyle == oldFormat.FontStyle &&
                       x.UseUnderline == oldFormat.UseUnderline &&
                       Color.AreClose(x.TextBrush.Color, oldFormat.TextBrush.Color) &&
                       x.FontWeight == oldFormat.FontWeight;

            }).FirstOrDefault();
            if (tf == null)
            {
                tf = new TextFormat(newSize, oldFormat.FontType, oldFormat.FontStyle, oldFormat.FontWeight, oldFormat.TextBrush, oldFormat.UseUnderline);
                AddToList(tf);
            }
            return tf.Index;
        }

        public int GetFormatIdForNewStyle(int oldId, FontStyle newStyle)
        {
            TextFormat oldFormat = formattingList[oldId];

            TextFormat tf = formattingList.Where(x =>
            {
                return x.FontSize == oldFormat.FontSize &&
                       x.FontType == oldFormat.FontType &&
                       x.FontStyle == newStyle &&
                       x.UseUnderline == oldFormat.UseUnderline &&
                       Color.AreClose(x.TextBrush.Color, oldFormat.TextBrush.Color) &&
                       x.FontWeight == oldFormat.FontWeight;

            }).FirstOrDefault();
            if (tf == null)
            {
                tf = new TextFormat(oldFormat.FontSize, oldFormat.FontType, newStyle, oldFormat.FontWeight, oldFormat.TextBrush, oldFormat.UseUnderline);
                AddToList(tf);
            }
            return tf.Index;
        }

        public int GetFormatIdForNewWeight(int oldId, FontWeight newWeight)
        {
            TextFormat oldFormat = formattingList[oldId];

            TextFormat tf = formattingList.Where(x =>
            {
                return x.FontSize == oldFormat.FontSize &&
                       x.FontType == oldFormat.FontType &&
                       x.FontStyle == oldFormat.FontStyle &&
                       x.UseUnderline == oldFormat.UseUnderline &&
                       Color.AreClose(x.TextBrush.Color, oldFormat.TextBrush.Color) &&
                       x.FontWeight == newWeight;

            }).FirstOrDefault();
            if (tf == null)
            {
                tf = new TextFormat(oldFormat.FontSize, oldFormat.FontType, oldFormat.FontStyle, newWeight, oldFormat.TextBrush, oldFormat.UseUnderline);
                AddToList(tf);
            }
            return tf.Index;
        }

        public int GetFormatIdForNewUnderline(int oldId, bool newUnderline)
        {
            TextFormat oldFormat = formattingList[oldId];

            TextFormat tf = formattingList.Where(x =>
            {
                return x.FontSize == oldFormat.FontSize &&
                       x.FontType == oldFormat.FontType &&
                       x.FontStyle == oldFormat.FontStyle &&
                       x.UseUnderline == newUnderline &&
                       Color.AreClose(x.TextBrush.Color, oldFormat.TextBrush.Color) &&
                       x.FontWeight == oldFormat.FontWeight;

            }).FirstOrDefault();
            if (tf == null)
            {
                tf = new TextFormat(oldFormat.FontSize, oldFormat.FontType, oldFormat.FontStyle, oldFormat.FontWeight, oldFormat.TextBrush, newUnderline);
                AddToList(tf);
            }
            return tf.Index;
        }

        public FormattedText GetFormattedText(string text, List<int> formats)
        {
            FormattedText formattedText = new FormattedText(text,
                                                            CultureInfo.InvariantCulture,
                                                            FlowDirection.LeftToRight,
                                                            formattingList[formats[0]].TypeFace,
                                                            formattingList[formats[0]].FontSize,
                                                            formattingList[formats[0]].TextBrush);
            for (int i = 0; i < formats.Count; i++)
            {
                FormatText(formats, formattedText, i);
            }
            return formattedText;
        }

        private void FormatText(List<int> formats, FormattedText formattedText, int i)
        {
            formattedText.SetFontFamily(formattingList[formats[i]].FontFamily, i, 1);
            formattedText.SetFontSize(formattingList[formats[i]].FontSize, i, 1);
            formattedText.SetFontStyle(formattingList[formats[i]].FontStyle, i, 1);
            formattedText.SetFontWeight(formattingList[formats[i]].FontWeight, i, 1);
            formattedText.SetForegroundBrush(formattingList[formats[i]].TextBrush, i, 1);
            if (formattingList[formats[i]].UseUnderline)
            {
                formattedText.SetTextDecorations(decorations[0], i, 1);
            }
        }

        public bool IsBold(int formatId)
        {
            return formattingList[formatId].FontWeight == FontWeights.Bold;
        }

        public bool IsItalic(int formatId)
        {
            return formattingList[formatId].FontStyle == FontStyles.Italic;
        }

        public bool IsUnderline(int formatId)
        {
            return formattingList[formatId].UseUnderline;
        }

        public FontType GetFontType(int formatId)
        {
            return formattingList[formatId].FontType;
        }

        public FormattedText GetFormattedText(string text, int format)
        {
            FormattedText formattedText = new FormattedText(text,
                                                            CultureInfo.InvariantCulture,
                                                            FlowDirection.LeftToRight,
                                                            formattingList[format].TypeFace,
                                                            formattingList[format].FontSize,
                                                            formattingList[format].TextBrush);
            
            
            formattedText.SetFontStyle(formattingList[format].FontStyle);
            formattedText.SetFontWeight(formattingList[format].FontWeight);            
            return formattedText;
        }

        public double GetBaseline(int formatId)
        {
            return formattingList[formatId].FontFamily.Baseline;
        }

        public double GetLineSpacing(int formatId)
        {
            return formattingList[formatId].FontFamily.LineSpacing;
        }

        public double GetFontHeight(int formatId)
        {
            double fontDpiSize = 16;
            return fontDpiSize * formattingList[formatId].FontFamily.LineSpacing;
        }
    }
}
