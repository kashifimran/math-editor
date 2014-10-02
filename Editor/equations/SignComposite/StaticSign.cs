using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;

namespace Editor
{
    public class StaticSign : StaticText
    {
        bool integralSignItalic = false;
        public SignCompositeSymbol Symbol { get; set; }
        public bool UseItalicIntegralSign 
        { 
            get {return integralSignItalic;}
            set 
            { 
                integralSignItalic = value;
                DetermineMargin();
                DetermineFontType();
                ReformatSign();
            }
        }

        public StaticSign(EquationContainer parent, SignCompositeSymbol symbol, bool useItalic)
            : base(parent)
        {
            integralSignItalic = useItalic;
            Symbol = symbol;
            DetermineSignString();
            DetermineFontType();
            DetermineFontSizeFactor();
            DetermineMargin();
            ReformatSign();
        }

        void DetermineMargin()
        {            
            switch (Symbol)
            {
                case SignCompositeSymbol.Integral:
                case SignCompositeSymbol.DoubleIntegral:
                case SignCompositeSymbol.TripleIntegral:
                    LeftMarginFactor = 0.02;
                    break;
                case SignCompositeSymbol.ContourIntegral:
                case SignCompositeSymbol.SurfaceIntegral:
                case SignCompositeSymbol.VolumeIntegral:
                case SignCompositeSymbol.ClockContourIntegral:
                case SignCompositeSymbol.AntiClockContourIntegral:
                    RightMarginFactor = .2;
                    LeftMarginFactor = 0.1;
                    break;
                case SignCompositeSymbol.Union:
                case SignCompositeSymbol.Intersection:
                    LeftMarginFactor = 0.1;
                    RightMarginFactor = 0.05;
                    break;
                default:
                    RightMarginFactor = 0.05;
                    break;
            }            
        }

        void DetermineFontType()
        {
            FontType fontType = FontType.STIXSizeOneSym;
            switch (Symbol)
            {
                case SignCompositeSymbol.Integral:
                case SignCompositeSymbol.DoubleIntegral:
                case SignCompositeSymbol.TripleIntegral:
                case SignCompositeSymbol.ContourIntegral:
                case SignCompositeSymbol.SurfaceIntegral:
                case SignCompositeSymbol.VolumeIntegral:                
                case SignCompositeSymbol.ClockContourIntegral:
                case SignCompositeSymbol.AntiClockContourIntegral:
                    if (UseItalicIntegralSign)
                    {
                        fontType = FontType.STIXGeneral;
                    }
                    else
                    {
                        fontType = FontType.STIXIntegralsUp;
                    }
                    break;
                case SignCompositeSymbol.Intersection:
                case SignCompositeSymbol.Union:
                    fontType = FontType.STIXGeneral;
                    break;
            }            
            FontType = fontType;
        }

        void DetermineFontSizeFactor()
        {
            double factor = 1;
            switch (Symbol)
            {
                case SignCompositeSymbol.Integral:
                case SignCompositeSymbol.DoubleIntegral:
                case SignCompositeSymbol.TripleIntegral:
                case SignCompositeSymbol.ContourIntegral:
                case SignCompositeSymbol.SurfaceIntegral:
                case SignCompositeSymbol.VolumeIntegral:
                case SignCompositeSymbol.ClockContourIntegral:
                case SignCompositeSymbol.AntiClockContourIntegral:
                    factor = 1.5;
                    break;
                case SignCompositeSymbol.Intersection:
                case SignCompositeSymbol.Union:
                    factor = 1.2;
                    break;
            }
            FontSizeFactor = factor;
        }

        void DetermineSignString()
        {
            string signStr = "";
            switch (Symbol)
            {
                case SignCompositeSymbol.Sum:
                    signStr = "\u2211";
                    break;
                case SignCompositeSymbol.Product:
                    signStr = "\u220F";
                    break;
                case SignCompositeSymbol.CoProduct:
                    signStr = "\u2210";
                    break;
                case SignCompositeSymbol.Intersection:
                    signStr = "\u22C2";
                    break;
                case SignCompositeSymbol.Union:
                    signStr = "\u22C3";
                    break;
                case SignCompositeSymbol.Integral:
                    signStr = "\u222B";
                    break;
                case SignCompositeSymbol.DoubleIntegral:
                    signStr = "\u222C";
                    break;
                case SignCompositeSymbol.TripleIntegral:
                    signStr = "\u222D";
                    break;
                case SignCompositeSymbol.ContourIntegral:
                    signStr = "\u222E";
                    break;
                case SignCompositeSymbol.SurfaceIntegral:
                    signStr = "\u222F";
                    break;
                case SignCompositeSymbol.VolumeIntegral:
                    signStr = "\u2230";
                    break;
                case SignCompositeSymbol.ClockContourIntegral:
                    signStr = "\u2232";
                    break;
                case SignCompositeSymbol.AntiClockContourIntegral:
                    signStr = "\u2233";
                    break;
            }
            Text = signStr;
        }
    }
}
