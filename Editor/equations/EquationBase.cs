using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace Editor
{
    public abstract class EquationBase
    {
        //do not use this.. just for debugging
        int IndexInChildrenOfParent
        {
            get
            {
                return ParentEquation.GetIndex(this);
            }
        }
        
        protected static TextManager textManager = new TextManager();
        static Thickness ZeroMargin = new Thickness();
        public virtual Thickness Margin
        {
            get { return ZeroMargin; } 
        }
        
        static protected double lineFactor = 0.06;
        public virtual bool ApplySymbolGap { get; set; }

        public virtual HashSet<int> GetUsedTextFormats() { return null; }
        public virtual void ResetTextFormats(Dictionary<int, int> formatMapping) { }

        protected double LineThickness { get { return fontSize * lineFactor; } }
        protected double ThinLineThickness { get { return fontSize * lineFactor * 0.7; } }
        protected Pen StandardPen { get { return PenManager.GetPen(LineThickness); } }
        protected Pen ThinPen { get { return PenManager.GetPen(ThinLineThickness); } }

        protected Pen StandardMiterPen { get { return PenManager.GetPen(LineThickness, PenLineJoin.Miter); } }
        protected Pen ThinMiterPen { get { return PenManager.GetPen(ThinLineThickness, PenLineJoin.Miter); } }

        protected Pen StandardRoundPen { get { return PenManager.GetPen(LineThickness, PenLineJoin.Round); } }
        protected Pen ThinRoundPen { get { return PenManager.GetPen(ThinLineThickness, PenLineJoin.Round); } }

        public HAlignment HAlignment { get; set; }
        public VAlignment VAlignment { get; set; }
        public bool IsStatic { get; set; }
        public int SubLevel { get; set; }
        protected double SubFontFactor = 0.6;
        protected double SubSubFontFactor = 0.7;
        public static event EventHandler<EventArgs> SelectionAvailable;
        public static event EventHandler<EventArgs> SelectionUnavailable;

        static bool isSelecting;
        protected static bool IsSelecting
        {
            get { return isSelecting; }
            set
            {
                isSelecting = value;
                if (isSelecting)
                {
                    SelectionAvailable(null, EventArgs.Empty); //there MUST always be one handler attached!
                }
                else
                {
                    SelectionUnavailable(null, EventArgs.Empty); //there MUST always be one handler attached!
                }
            }
        }

        public static bool ShowNesting { get; set; }
        public EquationContainer ParentEquation { get; set; }
        //protected static Pen BluePen = new Pen(Brushes.Blue, 1);
        Point location = new Point();
        //Point refPoint = new Point();
        double width;
        double height;
        double fontSize = 20;
        double fontFactor = 1;
        Pen boxPen = new Pen(Brushes.Black, 1);
        public int SelectionStartIndex { get; set; }
        public int SelectedItems { get; set; } //this is a directed value (as on a real line!!)

        protected Brush debugBrush;
        byte r = 80;
        byte g = 80;
        byte b = 80;

        public EquationBase(EquationContainer parent)
        {
            this.ParentEquation = parent;
            if (parent != null)
            {
                SubLevel = parent.SubLevel;
                fontSize = parent.fontSize;
                ApplySymbolGap = parent.ApplySymbolGap;
                r = (byte)(parent.r + 15);
                g = (byte)(parent.r + 15);
                b = (byte)(parent.r + 15);
            }
            debugBrush = new SolidColorBrush(Color.FromArgb(100, r, g, b));
            debugBrush.Freeze();
            boxPen.Freeze();
        }

        public virtual bool ConsumeMouseClick(Point mousePoint) { return false; }
        public virtual void HandleMouseDrag(Point mousePoint) { }

        public virtual EquationBase Split(EquationContainer newParent) { return null; }
        public virtual void ConsumeText(string text) { }
        public virtual void ConsumeFormattedText(string text, int[] formats, EditorMode[] modes, CharacterDecorationInfo[] decorations, bool addUndo) { }
        public virtual bool ConsumeKey(Key key) { return false; }
        public virtual Point GetVerticalCaretLocation() { return location; }
        public virtual double GetVerticalCaretLength() { return height; }
        protected virtual void CalculateWidth() { }
        protected virtual void CalculateHeight() { }
        public virtual XElement Serialize() { return null; }
        public virtual void DeSerialize(XElement xElement) { }
        public virtual void StartSelection() { SelectedItems = 0; }
        public virtual bool Select(Key key) { return false; }
        public virtual void DeSelect() { SelectedItems = 0; }
        public virtual void RemoveSelection(bool registerUndo) { }
        public virtual Rect GetSelectionBounds() { return Rect.Empty; }
        public virtual CopyDataObject Copy(bool removeSelection) { return null; } //copy & cut
        public virtual void Paste(XElement xe) { }
        public virtual void SetCursorOnKeyUpDown(Key key, Point point) { }
        public virtual void ModifySelection(string operation, string argument, bool applied, bool addUndo) { }

        public virtual void CalculateSize()
        {
            CalculateWidth();
            CalculateHeight();
        }
        public virtual void SelectAll() { }
        public virtual string GetSelectedText() { return string.Empty; }
      
        public virtual void DrawEquation(DrawingContext dc)
        {
            if (ShowNesting)
            {
                dc.DrawRectangle(debugBrush, null, Bounds);
            }
        }

        public virtual double FontFactor
        {
            get { return fontFactor; }
            set
            {
                fontFactor = value;
                FontSize = fontSize; //fontsize needs adjustement!
            }
        }

        public virtual double FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = Math.Min(1000, Math.Max(value * fontFactor, 4));
            }
        }

        public virtual double RefX
        {
            get { return width / 2; }
        }

        public virtual double RefY
        {
            get { return height / 2; }
        }        

        public double RefYReverse
        {
            get { return height - RefY; }
        }

        public virtual double Width
        {
            get { return width; }
            set
            {
                width = value;
            }
        }

        public virtual double Height
        {
            get { return height; }
            set
            {
                height = value > 0 ? value : 0;
            }
        }

        public Point Location
        {
            get { return location; }
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public virtual double Left
        {
            get { return location.X; }
            set { location.X = value; }
        }
        public virtual double Top
        {
            get { return location.Y; }
            set { location.Y = value; }
        }

        public double MidX
        {
            get { return location.X + RefX; }
            set { Left = value - RefX; }
        }

        public double MidY
        {
            get { return location.Y + RefY; }
            set { Top = value - RefY; }
        }

        public virtual double Right
        {
            get { return location.X + width; }
            set { Left = value - width; }
        }

        public virtual double Bottom
        {
            get { return location.Y + height; }
            set { Top = value - height; }
        }

        public Size Size
        {
            get { return new Size(width, height); }
        }

        public Rect Bounds
        {
            get { return new Rect(location, Size); }
        }
    }
}
