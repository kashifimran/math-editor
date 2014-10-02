using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace Editor
{ 
    public class FontFactory
    {
        private FontFactory() { }
        static Dictionary<FontType, FontFamily> fontFamilies = new Dictionary<FontType, FontFamily>();

        static FontFactory()
        {
            foreach (FontType ft in Enum.GetValues(typeof(FontType)))
            {
                fontFamilies.Add(ft, CreateFontFamily(ft));
            }
        }
        
        public static FormattedText GetFormattedText(string textToFormat, FontType fontType, double fontSize)
        {
            return GetFormattedText(textToFormat, fontType, fontSize, FontStyles.Normal, FontWeights.Normal);
        }

        public static FormattedText GetFormattedText(string textToFormat, FontType fontType, double fontSize, FontWeight fontWeight)
        {
            return GetFormattedText(textToFormat, fontType, fontSize, FontStyles.Normal, fontWeight);
        }
        
        public static FormattedText GetFormattedText(string textToFormat, FontType fontType, double fontSize, FontStyle fontStyle, FontWeight fontWeight)
        {
            return GetFormattedText(textToFormat, fontType, fontSize, fontStyle, fontWeight, Brushes.Black);
        }

        public static FormattedText GetFormattedText(string textToFormat, FontType fontType, double fontSize, Brush brush)
        {
            return GetFormattedText(textToFormat, fontType, fontSize, FontStyles.Normal, FontWeights.Normal, brush);
        }


        public static FormattedText GetFormattedText(string textToFormat, FontType fontType, double fontSize, FontStyle fontStyle, FontWeight fontWeight, Brush brush)
        {
            Typeface typeface = GetTypeface(fontType, fontStyle, fontWeight);
            return new FormattedText(textToFormat, CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, typeface, fontSize, brush);
        }

        public static FontFamily GetFontFamily(FontType fontType)
        {
            if (fontFamilies.Keys.Contains(fontType))
            {
                return fontFamilies[fontType];
            }
            else
            {
                return new FontFamily("Segoe UI");
            }
        }

        static FontFamily CreateFontFamily(FontType ft)
        {
            switch (ft)
            {
                case FontType.STIXGeneral:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXGeneral");
                case FontType.STIXIntegralsD:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXIntegralsD");
                case FontType.STIXIntegralsSm:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXIntegralsSm");
                case FontType.STIXIntegralsUp:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXIntegralsUp");
                case FontType.STIXIntegralsUpD:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXIntegralsUpD");
                case FontType.STIXIntegralsUpSm:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXIntegralsUpSm");
                case FontType.STIXNonUnicode:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXNonUnicode");
                case FontType.STIXSizeFiveSym:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXSizeFiveSym");
                case FontType.STIXSizeFourSym:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXSizeFourSym");
                case FontType.STIXSizeOneSym:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXSizeOneSym");
                case FontType.STIXSizeThreeSym:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXSizeThreeSym");
                case FontType.STIXSizeTwoSym:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXSizeTwoSym");
                case FontType.STIXVariants:
                    return new FontFamily(new Uri("pack://application:,,,/STIX/"), "./#STIXVariants");
                case FontType.Arial:
                    return new FontFamily("Arial");
                case FontType.ArialBlack:
                    return new FontFamily("Arial Black");
                case FontType.ComicSansMS:
                    return new FontFamily("Comic Sans MS");
                case FontType.Courier:
                    return new FontFamily("Courier");      
                case FontType.CourierNew:
                    return new FontFamily("Courier New");   
                case FontType.Georgia:
                    return new FontFamily("Georgia");
                case FontType.Impact:
                    return new FontFamily("Impact");
                case FontType.LucidaConsole:
                    return new FontFamily("Lucida Console");
                case FontType.LucidaSansUnicode:
                    return new FontFamily("Lucida Sans Unicode");
                case FontType.MSSerif:
                    return new FontFamily("MS Serif");
                case FontType.MSSansSerif:
                    return new FontFamily("MS Sans Serif");
                case FontType.PalatinoLinotype:
                    return new FontFamily("Palatino Linotype");
                case FontType.Segoe:
                    return new FontFamily("Segoe UI");
                case FontType.Symbol:
                    return new FontFamily("Symbol");
                case FontType.Tahoma:
                    return new FontFamily("Tahoma");
                case FontType.TimesNewRoman:
                    return new FontFamily("Times New Roman");
                case FontType.TrebuchetMS:
                    return new FontFamily("Trebuchet MS");
                case FontType.Verdana:
                    return new FontFamily("Verdana");
                case FontType.Webdings:
                    return new FontFamily("Webdings");
                case FontType.Wingdings:
                    return new FontFamily("Wingdings");                
            }
            return new FontFamily("Segoe UI");
        }

        public static Typeface GetTypeface(FontType fontType, FontStyle fontStyle, FontWeight fontWeight)
        {
            return new Typeface(GetFontFamily(fontType), fontStyle, fontWeight, FontStretches.Normal, GetFontFamily(FontType.STIXGeneral));
        }        
    }
}