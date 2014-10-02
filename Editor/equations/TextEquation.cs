using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace Editor
{
    public class TextEquation : EquationBase, ISupportsUndo
    {
        static HashSet<char> symbols = new HashSet<char>() 
        { 
            '+', '\u2212', '-', '=',  '>', '<',
            '\u2190', '\u2191', '\u2192', '\u2193', '\u2194', '\u2195', '\u2196', 
            '\u2197', '\u2198', '\u2199', '\u219A', '\u219B', '\u219C', '\u219D', 
            '\u219E', '\u219F', '\u21A0', '\u21A1', '\u21A2', '\u21A3', '\u21A4', 
            '\u21A5', '\u21A6', '\u21A7', '\u21A8', '\u21A9', '\u21AA', '\u21AB', 
            '\u21AC', '\u21AD', '\u21AE', '\u21AF', '\u21B0', '\u21B1', '\u21B2',
            '\u21B3', '\u21B4', '\u21B5', '\u21B6', '\u21B7', '\u21B8', '\u21B9',
            '\u21BA', '\u21BB', '\u21BC', '\u21BD', '\u21BE', '\u21BF', '\u21C0',
            '\u21C1', '\u21C2', '\u21C3', '\u21C4', '\u21C5', '\u21C6', '\u21C7',
            '\u21C8', '\u21C9', '\u21CA', '\u21CB', '\u21CC', '\u21CD', '\u21CE',
            '\u21CF', '\u21D0', '\u21D1', '\u21D2', '\u21D3', '\u21D4', '\u21D5', 
            '\u21D6', '\u21D7', '\u21D8', '\u21D8', '\u21D9', '\u21DA', '\u21DB',
            '\u21DC',  
            '\u00d7', '\u00b7', '\u00f7', '\u00b1', 
            '\u2200', '\u2208', '\u2209', '\u220B', '\u220C', 
            '\u2217', '\u2227', '\u2228', 
            '\u2229', '\u222A', '\u2234', '\u2235', '\u2237', '\u2238', '\u2264', 
            '\u2265', '\u226e', '\u226f',  
            '\u25E0', '\u25E1',
        };

        public static event PropertyChangedEventHandler InputPropertyChanged = (x, y) => { };
        static bool inputBold, inputItalic, inputUnderline;
        static EditorMode editorMode = EditorMode.Math;
        public static bool InputBold
        {
            get { return inputBold; }
            set
            {
                if (inputBold != value)
                {
                    inputBold = value;
                    InputPropertyChanged(null, new PropertyChangedEventArgs("InputBold"));
                }
            }
        }
        public static bool InputItalic
        {
            get { return inputItalic; }
            set
            {
                if (inputItalic != value)
                {
                    inputItalic = value;
                    InputPropertyChanged(null, new PropertyChangedEventArgs("InputItalic"));
                }
            }
        }
        public static bool InputUnderline
        {
            get { return inputUnderline; }
            set
            {
                if (inputUnderline != value)
                {
                    inputUnderline = value;
                    InputPropertyChanged(null, new PropertyChangedEventArgs("InputUnderline"));
                }
            }
        }
        public static EditorMode EditorMode
        {
            get { return editorMode; }
            set
            {
                if (editorMode != value)
                {
                    editorMode = value;
                    InputPropertyChanged(null, new PropertyChangedEventArgs("EditorMode"));
                }
            }
        }

        private StringBuilder textData = new StringBuilder();
        int caretIndex = 0;
        static FontType fontType = FontType.STIXGeneral;
        List<CharacterDecorationInfo> decorations = new List<CharacterDecorationInfo>();
        List<int> formats = new List<int>();
        List<EditorMode> modes = new List<EditorMode>();

        public TextEquation(EquationContainer parent)
            : base(parent)
        {
            CalculateSize();
        }

        void SetCaretIndex(int index)
        {
            caretIndex = index;
            if (formats.Count > 0)
            {
                int formatIndexToUse = index;
                if (index > 0)
                {
                    formatIndexToUse--;
                }
                InputBold = textManager.IsBold(formats[formatIndexToUse]);
                InputItalic = textManager.IsItalic(formats[formatIndexToUse]);
                InputUnderline = textManager.IsUnderline(formats[formatIndexToUse]);
                EditorMode = modes[formatIndexToUse];
                FontType = textManager.GetFontType(formats[formatIndexToUse]);
            }
        }

        public static FontType FontType
        {
            get { return fontType; }
            set
            {
                if (fontType != value)
                {
                    fontType = value;
                    InputPropertyChanged(null, new PropertyChangedEventArgs("FontType"));
                }
            }
        }

        public override void StartSelection()
        {
            SelectedItems = 0;  // = base.StartSelection();
            SelectionStartIndex = caretIndex;
        }

        public override bool Select(Key key)
        {
            if (key == Key.Left)
            {
                if (caretIndex > 0)
                {
                    SelectedItems--;
                    SetCaretIndex(caretIndex - 1);
                    return true;
                }
            }
            else if (key == Key.Right)
            {
                if (caretIndex < textData.Length)
                {
                    SelectedItems++;
                    SetCaretIndex(caretIndex + 1);
                    return true;
                }
            }
            return false;
        }

        public string Text
        {
            get
            {
                return textData.ToString();
            }
        }

        public override string GetSelectedText()
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                return textData.ToString(startIndex, count);
            }
            return "";
        }

        public int[] GetSelectedFormats()
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                return formats.GetRange(startIndex, count).ToArray();
            }
            return new int[0];
        }

        public EditorMode[] GetSelectedModes()
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                return modes.GetRange(startIndex, count).ToArray();
            }
            return new EditorMode[0];
        }

        public CharacterDecorationInfo[] GetSelectedDecorations()
        {
            if (SelectedItems != 0 && decorations.Count > 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int endIndex = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex);
                var selected = (from d in decorations where d.Index >= startIndex && d.Index < endIndex select d).ToArray();
                foreach (var s in selected)
                {
                    s.Index = s.Index - startIndex;
                }
                return selected;
            }
            else
            {
                return new CharacterDecorationInfo[0];
            }
        }

        public int[] GetFormats()
        {
            return formats.ToArray();
        }

        public EditorMode[] GetModes()
        {
            return modes.ToArray();
        }

        public CharacterDecorationInfo[] GetDecorations()
        {
            if (decorations.Count > 0)
            {
                return decorations.ToArray();
            }
            else
            {
                return new CharacterDecorationInfo[0];
            }
        }

        public override void SelectAll()
        {
            SelectionStartIndex = 0;
            SelectedItems = textData.Length;
            CaretIndex = textData.Length;
        }

        public void ResetTextEquation(int caretIndex, int selectionStartIndex, int selectedItems, string text, int[] formats,
                                      EditorMode[] modes, CharacterDecorationInfo[] cdiList)
        {
            textData.Clear();
            textData.Append(text);
            this.caretIndex = caretIndex;
            this.SelectionStartIndex = selectionStartIndex;
            this.SelectedItems = selectedItems;
            this.formats.Clear();
            this.formats.AddRange(formats);
            this.modes.Clear();
            this.modes.AddRange(modes);
            this.decorations.Clear();
            decorations.AddRange(cdiList);
            FormatText();
        }

        public override void RemoveSelection(bool registerUndo)
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                if (registerUndo)
                {
                    TextRemoveAction textRemoveAction = new TextRemoveAction(this, startIndex, textData.ToString(startIndex, count),
                                                                             SelectionStartIndex, SelectedItems, ParentEquation.SelectionStartIndex,
                                                                             formats.GetRange(startIndex, count).ToArray(),
                                                                             modes.GetRange(startIndex, count).ToArray(),
                                                                             (from d in decorations where d.Index >= startIndex && d.Index < startIndex + count select d).ToArray()
                                                                             );
                    UndoManager.AddUndoAction(textRemoveAction);
                }
                RemoveContent(startIndex, count);
                SetCaretIndex(startIndex);
                FormatText();
                SelectedItems = 0;
            }
        }

        private void RemoveContent(int startIndex, int count)
        {
            decorations.RemoveAll(x => x.Index >= startIndex && x.Index < startIndex + count);
            var list = from d in decorations where d.Index >= startIndex + count select d;
            foreach (var v in list)
            {
                v.Index = v.Index - count;
            }
            textData.Remove(startIndex, count);
            formats.RemoveRange(startIndex, count);
            modes.RemoveRange(startIndex, count);
        }

        public void SelectToStart()
        {
            SetCaretIndex(0);
            SelectedItems = -SelectionStartIndex;
        }

        public void SelectToEnd()
        {
            SetCaretIndex(textData.Length);
            SelectedItems = caretIndex - SelectionStartIndex;
        }

        public void MoveToStart()
        {
            SetCaretIndex(0);
        }

        public void MoveToEnd()
        {
            SetCaretIndex(textData.Length);
        }

        public int CaretIndex
        {
            get { return caretIndex; }
            set { SetCaretIndex(value); }
        }

        public int TextLength
        {
            get { return textData.Length; }
        }

        public override Rect GetSelectionBounds()
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                if (count > 0)
                {
                    double beforeWidth = GetWidth(0, startIndex);
                    double selectedWith = GetWidth(startIndex, count);
                    return new Rect(Left + beforeWidth, Top, selectedWith, Height);
                }
            }
            return Rect.Empty;
        }

        public override CopyDataObject Copy(bool removeSelection)
        {
            string selectedText = GetSelectedText();
            int[] selectedFormats = GetSelectedFormats();
            if (selectedText.Length > 0)
            {
                FormattedText formattedText = textManager.GetFormattedText(selectedText, selectedFormats.ToList());
                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)(Math.Ceiling(Width + 4)), (int)(Math.Ceiling(Height + 4)), 96, 96, PixelFormats.Default);
                DrawingVisual dv = new DrawingVisual();
                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, bitmap.Width, bitmap.Height));
                    DrawEquation(dc);
                }
                bitmap.Render(dv);
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                XElement xElement = CreateXElement(startIndex, count);
                if (removeSelection)
                {
                    RemoveSelection(true);
                }
                return new CopyDataObject { Image = bitmap, Text = selectedText, XElement = xElement };
            }
            return null;
        }

        public override void Paste(XElement xElement)
        {
            if (xElement.Name.LocalName == GetType().Name)
            {
                //textData.Insert(caretIndex, xElement.Element("Text").Value);
                string text = xElement.Element("Text").Value;
                string[] formatStrings = xElement.Element("Formats").Value.Split(',');
                string[] modeStrings = xElement.Element("Modes").Value.Split(',');
                int[] formats = new int[text.Length];
                EditorMode[] modes = new EditorMode[text.Length];
                var decos = xElement.Elements().FirstOrDefault(x => x.Name == "Decorations");
                CharacterDecorationInfo[] decorations = null;
                if (decos != null)
                {
                    var children = decos.Elements("d").ToList();
                    decorations = new CharacterDecorationInfo[children.Count];
                    for (int i = 0; i < children.Count; i++)
                    {
                        string[] list = children[i].Value.Split(',');
                        decorations[i] = new CharacterDecorationInfo();
                        decorations[i].DecorationType = (CharacterDecorationType)Enum.Parse(typeof(CharacterDecorationType), list[0]);
                        decorations[i].Index = int.Parse(list[1]);
                        decorations[i].Position = (Position)Enum.Parse(typeof(Position), list[2]);
                        decorations[i].UnicodeString = list[3];
                    }
                }
                try
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        formats[i] = int.Parse(formatStrings[i]);
                        modes[i] = (EditorMode)Enum.Parse(typeof(EditorMode), modeStrings[i]);
                    }
                }
                catch
                {
                    int formatId = textManager.GetFormatId(FontSize, fontType, FontStyles.Normal, FontWeights.Normal, Brushes.Black, false);
                    for (int i = 0; i < text.Length; i++)
                    {
                        formats[i] = formatId;
                        modes[i] = Editor.EditorMode.Math;
                    }
                }
                ConsumeFormattedText(text, formats, modes, decorations, true);
            }
        }

        public override XElement Serialize()
        {
            return CreateXElement(0, textData.Length);
        }

        XElement CreateXElement(int start, int count)
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement text = new XElement("Text", textData.ToString(start, count));
            StringBuilder strBuilder = new StringBuilder();
            StringBuilder modeStr = new StringBuilder();
            for (int i = start; i < start + count; i++)
            {
                strBuilder.Append(formats[i] + ",");
                modeStr.Append((int)modes[i] + ",");
            }
            if (strBuilder.Length > 0)
            {
                strBuilder.Remove(strBuilder.Length - 1, 1);
                modeStr.Remove(modeStr.Length - 1, 1);
            }
            XElement formatsElement = new XElement("Formats", strBuilder.ToString());
            XElement modesElement = new XElement("Modes", modeStr.ToString());
            thisElement.Add(text);
            thisElement.Add(formatsElement);
            thisElement.Add(modesElement);
            var d = (from x in decorations where x.Index >= start && x.Index < start + count select x).ToList();
            if (d.Count > 0)
            {
                XElement decorationsElement = new XElement("Decorations");
                foreach (var x in d)
                {
                    XElement xe = new XElement("d", x.DecorationType + "," + (x.Index - start) + "," + x.Position + "," + x.UnicodeString);
                    decorationsElement.Add(xe);
                }
                thisElement.Add(decorationsElement);
            }
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            textData.Append(xElement.Element("Text").Value);
            try
            {
                string[] formatStrings = xElement.Element("Formats").Value.Split(',');
                string[] modeStrings = xElement.Element("Modes").Value.Split(',');
                for (int i = 0; i < formatStrings.Length; i++)
                {
                    formats.Add(int.Parse(formatStrings[i]));
                    modes.Add((EditorMode)Enum.Parse(typeof(EditorMode), modeStrings[i]));
                }
                var decos = xElement.Elements().FirstOrDefault(x => x.Name == "Decorations");
                if (decos != null)
                {
                    var children = decos.Elements("d").ToList();
                    for (int i = 0; i < children.Count; i++)
                    {
                        string[] list = children[i].Value.Split(',');
                        var d = new CharacterDecorationInfo();
                        d.DecorationType = (CharacterDecorationType)Enum.Parse(typeof(CharacterDecorationType), list[0]);
                        d.Index = int.Parse(list[1]);
                        d.Position = (Position)Enum.Parse(typeof(Position), list[2]);
                        d.UnicodeString = list[3];
                        decorations.Add(d);
                    }
                }
            }
            catch
            {
                formats.Clear();
                modes.Clear();
                decorations.Clear();
                int formatId = textManager.GetFormatId(FontSize, fontType, FontStyles.Normal, FontWeights.Normal, Brushes.Black, false);
                for (int i = 0; i < textData.Length; i++)
                {
                    formats.Add(formatId);
                    modes.Add(Editor.EditorMode.Math);
                }
            }
            FormatText();
        }

        public override double FontSize
        {
            get { return base.FontSize; }
            set
            {
                base.FontSize = value;
                for (int i = 0; i < formats.Count; i++)
                {
                    formats[i] = textManager.GetFormatIdForNewSize(formats[i], value);
                }
                FormatText();
            }
        }

        public override EquationBase Split(EquationContainer newParent)
        {
            TextEquation newText = new TextEquation(newParent);
            int itemCount = textData.Length - caretIndex;
            newText.textData.Append(textData.ToString(caretIndex, itemCount));
            textData.Remove(caretIndex, itemCount);
            newText.formats.AddRange(formats.GetRange(caretIndex, itemCount));
            formats.RemoveRange(caretIndex, itemCount);
            newText.modes.AddRange(modes.GetRange(caretIndex, itemCount));
            modes.RemoveRange(caretIndex, itemCount);
            var list = (from d in decorations where d.Index >= caretIndex select d).ToList();
            foreach (var v in list)
            {
                decorations.Remove(v);
                v.Index = v.Index - caretIndex;
                newText.decorations.Add(v);
            }
            SetCaretIndex(textData.Length);
            FormatText();
            newText.FormatText();
            return newText;
        }

        public void Merge(TextEquation secondText)
        {
            SetCaretIndex(textData.Length);
            foreach (var v in secondText.decorations)
            {
                v.Index = v.Index + textData.Length;
                decorations.Add(v);
            }
            //secondText.decorations.Clear();
            textData.Append(secondText.textData.ToString());
            formats.AddRange(secondText.formats);
            modes.AddRange(secondText.modes);
            FormatText();
        }

        public override void ConsumeText(string text)
        {
            //text = "\U0001D400";
            //string someText = char.ConvertFromUtf32(0x1D7D9);
            var list = from d in decorations where d.Index >= caretIndex select d;
            foreach (var v in list)
            {
                v.Index = v.Index + text.Length;
            }
            FontStyle style = InputItalic ? FontStyles.Italic : FontStyles.Normal;
            textData.Insert(caretIndex, text);
            string name = FunctionNames.CheckForFunctionName(textData.ToString(0, caretIndex + text.Length));
            if (name != null && EditorMode == Editor.EditorMode.Math && caretIndex - (name.Length - text.Length) >= 0)
            {
                for (int i = caretIndex - (name.Length - text.Length); i < caretIndex; i++)
                {
                    formats[i] = textManager.GetFormatIdForNewStyle(formats[i], FontStyles.Normal);
                    modes[i] = Editor.EditorMode.Math;
                }
                style = FontStyles.Normal;
            }
            else if (text.Length == 1 && EditorMode == Editor.EditorMode.Math)
            {
                if (((int)text[0] >= 65 && (int)text[0] <= 90 || (int)text[0] >= 97 && (int)text[0] <= 122) || char.IsWhiteSpace(text[0]))
                {
                    style = FontStyles.Italic;
                }
                else
                {
                    style = FontStyles.Normal;
                }
            }

            int formatId = textManager.GetFormatId(FontSize, fontType, style, InputBold ? FontWeights.Bold : FontWeights.Normal,
                                                   Brushes.Black, InputUnderline);
            int[] tempFormats = new int[text.Length];
            EditorMode[] tempModes = new EditorMode[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                formats.Insert(i + caretIndex, formatId);
                modes.Insert(i + caretIndex, EditorMode);
                tempFormats[i] = formatId;
                tempModes[i] = EditorMode;
            }
            UndoManager.AddUndoAction(new TextAction(this, caretIndex, text, tempFormats, tempModes, new CharacterDecorationInfo[0]) { UndoFlag = false });
            SetCaretIndex(caretIndex + text.Length);
            FormatText();
        }

        public override void ConsumeFormattedText(string text, int[] formats, EditorMode[] modes, CharacterDecorationInfo[] decorations, bool addUndo)
        {
            //this.decorations.AddRange(decorations);
            textData.Insert(caretIndex, text);
            this.formats.InsertRange(caretIndex, formats);
            this.modes.InsertRange(caretIndex, modes);
            if (decorations != null)
            {
                foreach (var d in decorations)
                {
                    d.Index = d.Index + caretIndex;
                }
                this.decorations.AddRange(decorations);
            }
            if (addUndo)
            {
                UndoManager.AddUndoAction(new TextAction(this, caretIndex, text, formats, modes, decorations ?? new CharacterDecorationInfo[0]) { UndoFlag = false });
            }
            SetCaretIndex(caretIndex + text.Length);
            FormatText();
        }

        public void SetFormattedText(string text, int[] formats, EditorMode[] modes)
        {
            textData.Clear();
            this.formats.Clear();
            this.modes.Clear();
            textData.Append(text);
            this.formats.AddRange(formats);
            this.modes.AddRange(modes);
            SetCaretIndex(text.Length);
            FormatText();
        }

        public override bool ConsumeKey(Key key)
        {
            bool consumed = false;
            switch (key)
            {
                case Key.Home:
                    SetCaretIndex(0);
                    consumed = true;
                    break;
                case Key.End:
                    SetCaretIndex(textData.Length);
                    consumed = true;
                    break;
                case Key.Delete:
                    if (textData.Length > 0 && caretIndex < textData.Length)
                    {
                        var cdi = (from d in decorations where d.Index == caretIndex select d).LastOrDefault();
                        if (cdi != null)
                        {
                            RemoveDecoration(cdi);
                        }
                        else
                        {
                            RemoveChar(caretIndex);
                        }
                        FormatText();
                        consumed = true;
                    }
                    break;
                case Key.Back:
                    if (caretIndex > 0)
                    {
                        var cdi = (from d in decorations where d.Index == caretIndex - 1 select d).LastOrDefault();
                        if (cdi != null)
                        {
                            RemoveDecoration(cdi);
                        }
                        else
                        {
                            RemoveChar(caretIndex - 1);
                            SetCaretIndex(caretIndex - 1);
                        }
                        FormatText();
                        consumed = true;
                    }
                    break;
                case Key.Right:
                    if (caretIndex < textData.Length)
                    {
                        SetCaretIndex(caretIndex + 1);
                        consumed = true;
                    }
                    break;
                case Key.Left:
                    if (caretIndex > 0)
                    {
                        SetCaretIndex(caretIndex - 1);
                        consumed = true;
                    }
                    break;
            }
            return consumed;
        }

        void RemoveDecoration(CharacterDecorationInfo cdi)
        {
            decorations.Remove(cdi);
            UndoManager.AddUndoAction(new DecorationAction(this, new[] { cdi }) { UndoFlag = false });
        }

        public override void ModifySelection(string operation, string argument, bool applied, bool addUndo)
        {
            if (SelectedItems != 0)
            {
                if (operation == "format")
                {
                    ModifyFormat(argument, applied, addUndo);
                }
                else if (operation == "font")
                {
                    ModifyFont((FontType)Enum.Parse(typeof(FontType), argument), applied, addUndo);
                }
                else if (operation == "mode")
                {
                    ModifyMode(argument, addUndo);
                }
                FormatText();
            }
        }

        private void ModifyFormat(string format, bool applied, bool addUndo)
        {
            int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
            int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
            int[] oldFormats = formats.GetRange(startIndex, count).ToArray();
            for (int i = startIndex; i < startIndex + count; i++)
            {
                switch (format.ToLower())
                {
                    case "underline":
                        formats[i] = textManager.GetFormatIdForNewUnderline(formats[i], applied);
                        break;
                    case "bold":
                        formats[i] = textManager.GetFormatIdForNewWeight(formats[i], applied ? FontWeights.Bold : FontWeights.Normal);
                        break;
                    case "italic":
                        formats[i] = textManager.GetFormatIdForNewStyle(formats[i], applied ? FontStyles.Italic : FontStyles.Normal);
                        break;
                    default:
                        throw new ArgumentException("Incorrect value passed with font format change request."); ;
                }
            }
            if (addUndo)
            {
                TextFormatAction tfa = new TextFormatAction(this, startIndex, oldFormats, formats.GetRange(startIndex, count).ToArray());
                UndoManager.AddUndoAction(tfa);
            }
        }

        private void ModifyMode(string newMode, bool addUndo)
        {
            int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
            int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
            EditorMode[] oldModes = modes.GetRange(startIndex, count).ToArray();
            for (int i = startIndex; i < startIndex + count; i++)
            {
                switch (newMode.ToLower())
                {
                    case "math":
                        modes[i] = EditorMode.Math;
                        break;
                    case "text":
                        modes[i] = EditorMode.Text;
                        break;
                    default:
                        throw new ArgumentException("Incorrect value passed with mode change request.");
                }
            }
            if (addUndo)
            {
                ModeChangeAction mca = new ModeChangeAction(this, startIndex, oldModes, modes.GetRange(startIndex, count).ToArray());
                UndoManager.AddUndoAction(mca);
            }
        }

        private void ModifyFont(FontType fontType, bool applied, bool addUndo)
        {
            int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
            int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
            int[] oldFormats = formats.GetRange(startIndex, count).ToArray();
            for (int i = startIndex; i < startIndex + count; i++)
            {
                formats[i] = textManager.GetFormatIdForNewFont(formats[i], fontType);
            }
            if (addUndo)
            {
                TextFormatAction tfa = new TextFormatAction(this, startIndex, oldFormats, formats.GetRange(startIndex, count).ToArray());
                UndoManager.AddUndoAction(tfa);
            }
        }

        void FormatText()
        {
            CalculateSize();
        }

        public override bool ConsumeMouseClick(Point mousePoint)
        {
            if (Left <= mousePoint.X && Right >= mousePoint.X)
            {
                SetCaretPosition(mousePoint);
                return true;
            }
            return false;
        }

        private void SetCaretPosition(Point mousePoint)
        {
            double left = Left;
            caretIndex = textData.Length;
            for (; caretIndex > 0; caretIndex--)
            {
                FormattedText lastChar = textManager.GetFormattedText(textData.ToString(caretIndex - 1, 1), formats.GetRange(caretIndex - 1, 1));
                //FormattedText textPart = textManager.GetFormattedText(textData.ToString(0, caretIndex), formats.GetRange(0, caretIndex));
                left = GetWidth(0, caretIndex) + Left;
                //left = textPart.GetFullWidth() + Left;
                if (left <= mousePoint.X + lastChar.GetFullWidth() / 2)
                    break;
            }
            SetCaretIndex(caretIndex);
        }

        public override void SetCursorOnKeyUpDown(Key key, Point point)
        {
            SetCaretPosition(point);
        }

        public override void HandleMouseDrag(Point mousePoint)
        {
            if (Left <= mousePoint.X && Right >= mousePoint.X)
            {
                SetCaretPosition(mousePoint);
                SelectedItems = caretIndex - SelectionStartIndex;
            }
        }

        public override Point GetVerticalCaretLocation()
        {
            if (caretIndex > 0)
            {
                return new Point(Left + GetWidth(0, caretIndex), Top);
            }
            else
            {
                return Location;
            }
        }

        protected override void CalculateWidth()
        {
            Width = GetWidth(0, textData.Length);
        }

        private double GetWidth(int start, int count)
        {
            double width = 0;
            if (count > 0 && start + count <= textData.Length)
            {
                List<int> symIndexes = FindSymbolIndexes(start, start + count);
                var groups = (from d in decorations orderby d.Index where d.Index >= start && d.Index < start + count group d by d.Index).ToList();
                int done = start;
                for (int i = 0; i <= symIndexes.Count; i++)
                {
                    int limit = i < symIndexes.Count ? symIndexes[i] : start + count;
                    if (limit - done > 0)
                    {
                        var subGroups = (from g in groups where g.Key >= done && g.Key < limit select g).ToList();
                        for (int j = 0; j <= subGroups.Count; j++)
                        {
                            int subLimit = j < subGroups.Count ? subGroups[j].Key : limit;
                            string text = textData.ToString(done, subLimit - done);
                            if (text.Length > 0)
                            {
                                FormattedText ft = textManager.GetFormattedText(text, formats.Skip(done).Take(text.Length).ToList());
                                width += ft.GetFullWidth();
                                done += ft.Text.Length;
                            }
                            if (j < subGroups.Count)
                            {
                                var charFt = textManager.GetFormattedText(textData[subGroups[j].Key].ToString(), formats[subGroups[j].Key]);
                                double hCenter;
                                double charLeft;
                                double decoWidth = GetDecoratedCharWidth(charFt, subGroups[j].ToList(), done, out charLeft, out hCenter);
                                width += decoWidth;
                                done++;
                                if (done < start + count)
                                {
                                    width += charFt.OverhangTrailing;
                                }
                            }
                        }
                    }
                    if (i < symIndexes.Count)
                    {
                        bool addSpaceBefore = symIndexes[i] > 0 ? !symbols.Contains(textData[symIndexes[i] - 1]) : true;
                        width += addSpaceBefore ? SymSpace : 0;
                        FormattedText ft = textManager.GetFormattedText(textData[symIndexes[i]].ToString(), formats[symIndexes[i]]);
                        var group = groups.FirstOrDefault(x => x.Key == symIndexes[i]);
                        width += ft.GetFullWidth() + SymSpace;
                        //dc.DrawLine(new Pen(Brushes.Purple, 1), new Point(left, Top), new Point(left, Bottom));
                        done++;
                    }
                }
            }
            return width;
        }

        double refY = 0;
        public override double RefY
        {
            get { return refY; }
        }

        double topExtra = 0;
        public override void DrawEquation(DrawingContext dc)
        {
            base.DrawEquation(dc);
            //dc.DrawRectangle(Brushes.Yellow, null, Bounds);
            //dc.DrawLine(new Pen(Brushes.Red, 1), new Point(Left, refY + Top), new Point(Left + Width, refY + Top));
            //dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(0, Top), new Point(10000, Top));
            if (textData.Length > 0)
            {
                var groups = (from d in decorations orderby d.Index group d by d.Index).ToList();
                List<int> symIndexes = FindSymbolIndexes();
                int done = 0;
                double left = Left;
                for (int i = 0; i <= symIndexes.Count; i++)
                {
                    int limit = i < symIndexes.Count ? symIndexes[i] : textData.Length;
                    if (limit - done > 0)
                    {
                        var subGroups = (from g in groups where g.Key >= done && g.Key < limit select g).ToList();
                        for (int j = 0; j <= subGroups.Count; j++)
                        {
                            int subLimit = j < subGroups.Count ? subGroups[j].Key : limit;
                            string text = textData.ToString(done, subLimit - done);
                            if (text.Length > 0)
                            {
                                FormattedText ft = textManager.GetFormattedText(text, formats.Skip(done).Take(text.Length).ToList());
                                ft.DrawTextLeftAligned(dc, new Point(left, Top - topExtra));
                                left += ft.GetFullWidth();
                                //dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(left, Top), new Point(left, Bottom));
                                if (done == 0 && !char.IsWhiteSpace(textData[0]))
                                {
                                    //left -= ft.OverhangLeading;
                                    //dc.DrawLine(new Pen(Brushes.Red, 1), new Point(left, Top), new Point(left, Bottom));
                                }
                                if (!char.IsWhiteSpace(text[text.Length - 1]))
                                {
                                    //left -= ft.OverhangTrailing;
                                    //dc.DrawLine(new Pen(Brushes.Green, 1), new Point(left, Top), new Point(left, Bottom));
                                }
                                done += ft.Text.Length;
                            }
                            if (j < subGroups.Count)
                            {
                                var charFt = textManager.GetFormattedText(textData[subGroups[j].Key].ToString(), formats[subGroups[j].Key]);
                                double hCenter;
                                double charLeft;
                                double decoWidth = GetDecoratedCharWidth(charFt, subGroups[j].ToList(), done, out charLeft, out hCenter);
                                charFt.DrawTextCenterAligned(dc, new Point(left + hCenter, Top - topExtra));
                                DrawAllDecorations(dc, left, hCenter, Top - topExtra, charLeft, charFt, done, subGroups[j].ToList());
                                left += decoWidth + charFt.OverhangLeading;// +(diff > 0 ? diff : 0);
                                done++;
                                //if (done < limit)
                                //{
                                //    left += charFt.OverhangTrailing;
                                //}
                            }
                        }
                    }
                    if (i < symIndexes.Count)
                    {
                        bool addSpaceBefore = symIndexes[i] > 0 ? !symbols.Contains(textData[symIndexes[i] - 1]) : true;
                        left += addSpaceBefore ? SymSpace : 0;
                        FormattedText ft = textManager.GetFormattedText(textData[symIndexes[i]].ToString(), formats[symIndexes[i]]);
                        ft.DrawTextLeftAligned(dc, new Point(left, Top - topExtra));
                        //dc.DrawLine(new Pen(Brushes.Red, 1), new Point(Left, refY + Top), new Point(Right, refY + Top));
                        //dc.DrawLine(new Pen(Brushes.Red, 1), new Point(Left, Top - topExtra), new Point(Right, Top - topExtra));
                        var group = groups.FirstOrDefault(x => x.Key == symIndexes[i]);
                        if (group != null)
                        {
                            DrawAllDecorations(dc, left, ft.GetFullWidth() / 2, Top - topExtra, 0, ft, group.Key, group.ToList());
                        }
                        left += ft.GetFullWidth() + SymSpace;
                        //dc.DrawLine(new Pen(Brushes.Purple, 1), new Point(left, Top), new Point(left, Bottom));
                        done++;
                    }
                }
            }
        }

        double SymSpace
        {
            get { return FontSize * 0.22; }
        }

        protected override void CalculateHeight()
        {
            if (textData.ToString().Trim().Length > 0)
            {
                var groups = (from d in decorations orderby d.Index group d by d.Index).ToList();
                double upperHalf = 0;
                double lowerHalf = 0;
                double extra = double.MaxValue;
                int done = 0;
                for (int i = 0; i <= groups.Count; i++)
                {
                    List<CharacterDecorationInfo> list = null;
                    int end = i < groups.Count ? groups[i].Key : textData.Length;
                    if (end - done > 0)
                    {
                        HeightMetrics hm = GetChunkHeightMetrics(done, end, list);
                        upperHalf = Math.Max(hm.UpperHalf, upperHalf);
                        lowerHalf = Math.Max(hm.LowerHalf, lowerHalf);
                        extra = Math.Min(extra, hm.TopExtra);
                        done += end - done;
                    }
                    if (i < groups.Count)
                    {
                        list = groups[i].ToList();
                        HeightMetrics hm = GetChunkHeightMetrics(end, end + 1, list);
                        upperHalf = Math.Max(hm.UpperHalf, upperHalf);
                        lowerHalf = Math.Max(hm.LowerHalf, lowerHalf);
                        extra = Math.Min(extra, hm.TopExtra);
                        done++;
                    }
                }
                topExtra = extra;
                refY = upperHalf;
                Height = upperHalf + lowerHalf;
            }
            else
            {
                int formatId = textManager.GetFormatId(FontSize, fontType, InputItalic ? FontStyles.Italic : FontStyles.Normal, InputBold ? FontWeights.Bold : FontWeights.Normal,
                                                   Brushes.Black, InputUnderline);
                var ft = textManager.GetFormattedText("d", formatId);
                //hm.TopExtra = Math.Min(ft.Baseline * .30, ft.TopExtra());
                topExtra = ft.Baseline * .26;
                refY = ft.Baseline * .5;
                Height = refY + ft.Descent() + ft.Baseline * .24;
            }
        }

        class HeightMetrics
        {
            internal double TopExtra { get; set; }
            internal double UpperHalf { get; set; }
            internal double LowerHalf { get; set; }
        }

        HeightMetrics GetChunkHeightMetrics(int start, int limit, List<CharacterDecorationInfo> list)
        {
            HeightMetrics hm = new HeightMetrics();
            if (list == null)
            {
                var text = textData.ToString(start, limit - start);
                var ft = textManager.GetFormattedText(text, formats.Skip(start).Take(text.Length).ToList());
                //hm.TopExtra = Math.Min(ft.Baseline * .30, ft.TopExtra());
                hm.TopExtra = ft.Baseline * .26;
                hm.UpperHalf = ft.Baseline * .5;
                hm.LowerHalf = ft.Descent() + ft.Baseline * .24;
            }
            else
            {
                var charFt = textManager.GetFormattedText(textData[limit - 1].ToString(), formats[limit - 1]);
                var bottomGroup = (from x in list where x.Position == Position.Bottom select x).ToList();
                var topGroup = (from x in list where x.Position == Position.Top select x).ToList();
                double lowerHalf = charFt.Descent() + charFt.Baseline * .24;
                double upperHalf = charFt.Baseline * .50;
                double topHeight = 0;
                double bottomHeight = 0;
                foreach (var v in topGroup)
                {
                    var sign = textManager.GetFormattedText(v.UnicodeString, formats[v.Index]);
                    topHeight += sign.Extent + FontSize * .1;
                }
                foreach (var v in bottomGroup)
                {
                    var sign = textManager.GetFormattedText(v.UnicodeString, formats[v.Index]);
                    bottomHeight += sign.Extent + FontSize * .1;
                }
                double diff = upperHalf - (charFt.Extent - lowerHalf + topHeight);
                hm.TopExtra = Math.Min(charFt.Baseline * .26, charFt.TopExtra() - topHeight);
                hm.UpperHalf = upperHalf - (diff > 0 ? 0 : diff);
                hm.LowerHalf = lowerHalf + bottomHeight;
            }
            return hm;
        }

        void RemoveChar(int index)
        {
            var decos = (from d in decorations where d.Index == index select d).ToArray();
            UndoManager.AddUndoAction(new TextAction(this, index, textData.ToString(index, 1), formats.GetRange(index, 1).ToArray(), modes.GetRange(index, 1).ToArray(), decos));
            decorations.RemoveAll(x => x.Index == index);
            var list = from d in decorations where d.Index > index select d;
            foreach (var v in list)
            {
                v.Index -= 1;
            }
            textData.Remove(index, 1);
            formats.RemoveAt(index);
            modes.RemoveAt(index);
        }

        public void ProcessUndo(EquationAction action)
        {
            if (action.GetType() == typeof(TextAction))
            {
                ProcessTextEquation(action);
            }
            else if (action.GetType() == typeof(TextFormatAction))
            {
                TextFormatAction tfa = action as TextFormatAction;
                caretIndex = tfa.Index;
                formats.RemoveRange(tfa.Index, tfa.NewFormats.Length);
                IsSelecting = true;
                if (tfa.UndoFlag)
                {
                    for (int i = 0; i < tfa.OldFormats.Length; i++)
                    {
                        tfa.OldFormats[i] = textManager.GetFormatIdForNewSize(tfa.OldFormats[i], FontSize);
                    }
                    formats.InsertRange(tfa.Index, tfa.OldFormats);
                }
                else
                {
                    for (int i = 0; i < tfa.NewFormats.Length; i++)
                    {
                        tfa.NewFormats[i] = textManager.GetFormatIdForNewSize(tfa.NewFormats[i], FontSize);
                    }
                    formats.InsertRange(tfa.Index, tfa.NewFormats);
                }
            }
            else if (action.GetType() == typeof(ModeChangeAction))
            {
                ModeChangeAction mca = action as ModeChangeAction;
                caretIndex = mca.Index;
                modes.RemoveRange(mca.Index, mca.NewModes.Length);
                if (mca.UndoFlag)
                {
                    modes.InsertRange(mca.Index, mca.OldModes);
                }
                else
                {
                    modes.InsertRange(mca.Index, mca.NewModes);
                }
            }
            else if (action.GetType() == typeof(DecorationAction))
            {
                DecorationAction da = action as DecorationAction;
                if (da.UndoFlag)
                {
                    foreach (var cdi in da.CharacterDecorations)
                    {
                        decorations.Remove(cdi);
                    }
                }
                else
                {
                    decorations.AddRange(da.CharacterDecorations);
                }
            }
            else
            {
                ProcessTextRemoveAction(action);
            }
            FormatText();
            ParentEquation.ChildCompletedUndo(this);
        }

        private void ProcessTextRemoveAction(EquationAction action)
        {
            TextRemoveAction textAction = action as TextRemoveAction;
            int count = textAction.Text.Length;
            if (textAction.UndoFlag)
            {
                textData.Insert(textAction.Index, textAction.Text);
                for (int i = 0; i < count; i++)
                {
                    textAction.Formats[i] = textManager.GetFormatIdForNewSize(textAction.Formats[i], FontSize);
                }
                formats.InsertRange(textAction.Index, textAction.Formats);
                modes.InsertRange(textAction.Index, textAction.Modes);
                decorations.AddRange(textAction.Decorations);
                var list = from d in decorations where d.Index >= textAction.Index select d;
                foreach (var v in list)
                {
                    v.Index += count;
                }
                SelectedItems = textAction.SelectionCount;
                SelectionStartIndex = textAction.SelectionStartIndex;
                ParentEquation.SelectionStartIndex = textAction.ParentSelectionStartIndex;
                ParentEquation.SelectedItems = 0;
                IsSelecting = true;
            }
            else
            {
                textData.Remove(textAction.Index, count);
                formats.RemoveRange(textAction.Index, count);
                modes.RemoveRange(textAction.Index, count);
                decorations.RemoveAll(x => x.Index >= textAction.Index && x.Index < textAction.Index + count);
                var list = from d in decorations where d.Index >= textAction.Index + count select d;
                foreach (var v in list)
                {
                    v.Index -= count;
                }
                SelectedItems = 0;
                SelectionStartIndex = textAction.Index;
                IsSelecting = false;
            }
            SetCaretIndex(textAction.Index);
        }

        private void ProcessTextEquation(EquationAction action)
        {
            TextAction textAction = action as TextAction;
            int count = textAction.Text.Length;
            if (textAction.UndoFlag)
            {
                textData.Insert(textAction.Index, textAction.Text);
                for (int i = 0; i < count; i++)
                {
                    textAction.Formats[i] = textManager.GetFormatIdForNewSize(textAction.Formats[i], FontSize);
                }
                formats.InsertRange(textAction.Index, textAction.Formats);
                modes.InsertRange(textAction.Index, textAction.Modes);
                decorations.AddRange(textAction.Decorations);
                var list = from d in decorations where d.Index >= textAction.Index select d;
                foreach (var v in list)
                {
                    v.Index += textAction.Modes.Length;
                }
                SetCaretIndex(textAction.Index + count);
            }
            else
            {
                textData.Remove(textAction.Index, count);
                formats.RemoveRange(textAction.Index, count);
                modes.RemoveRange(textAction.Index, count);
                decorations.RemoveAll(x => x.Index >= textAction.Index && x.Index < textAction.Index + count);
                var list = from d in decorations where d.Index >= textAction.Index + count select d;
                foreach (var v in list)
                {
                    v.Index -= count;
                }
                SetCaretIndex(textAction.Index);
            }
            IsSelecting = false;
        }

        public void Truncate(int keepCount)
        {
            var list = (from d in decorations where d.Index >= keepCount select d).ToList();
            foreach (var v in list)
            {
                decorations.Remove(v);
                v.Index = v.Index - keepCount;
            }
            //decorations.RemoveAll(x => x.Index >= keepCount);            
            textData.Length = keepCount;
            formats.RemoveRange(keepCount, formats.Count - keepCount);
            modes.RemoveRange(keepCount, modes.Count - keepCount);
            SetCaretIndex(Math.Min(caretIndex, textData.Length));
            FormatText();
        }

        public void Truncate()
        {
            var list = (from d in decorations where d.Index >= caretIndex select d).ToList();
            foreach (var v in list)
            {
                decorations.Remove(v);
                v.Index = v.Index - caretIndex;
            }
            //decorations.RemoveAll(x => x.Index >= caretIndex);            
            textData.Length = caretIndex;
            formats.RemoveRange(caretIndex, formats.Count - caretIndex);
            modes.RemoveRange(caretIndex, modes.Count - caretIndex);
            FormatText();
        }

        public void AddDecoration(CharacterDecorationType cdt, Position position, string sign)
        {
            if (cdt == CharacterDecorationType.None)
            {
                var decoArray = (from d in decorations where d.Index == caretIndex - 1 select d).ToArray();
                UndoManager.AddUndoAction(new DecorationAction(this, decoArray) { UndoFlag = false });
                decorations.RemoveAll(x => x.Index == caretIndex - 1);
            }
            else if (!char.IsWhiteSpace(textData[caretIndex - 1]))
            {
                CharacterDecorationInfo cdi = new CharacterDecorationInfo();
                cdi.DecorationType = cdt;
                cdi.Position = position;
                cdi.UnicodeString = sign;
                cdi.Index = caretIndex - 1;
                decorations.Add(cdi);
                UndoManager.AddUndoAction(new DecorationAction(this, new[] { cdi }));
            }
            FormatText();
        }

        private List<int> FindSymbolIndexes(int start = 0, int limit = 0) // limit is 1 past the last to be checked
        {
            List<int> symIndexes = new List<int>();
            if (ApplySymbolGap)
            {
                if (ParentEquation.GetIndex(this) == 0 && start == 0)
                {
                    start = 1;
                }
                limit = limit > 0 ? limit : textData.Length;
                for (int i = start; i < limit; i++)
                {
                    bool isSymbol = true;
                    if (i > 0 && symbols.Contains(textData[i - 1]))
                    {
                        if (i + 1 == textData.Length || (i + 1 < textData.Length && !symbols.Contains(textData[i + 1])))
                        {
                            isSymbol = false;
                        }
                    }
                    if (symbols.Contains(textData[i]) && isSymbol)
                    {
                        symIndexes.Add(i);
                    }
                }
            }
            return symIndexes;
        }

        private void DrawAllDecorations(DrawingContext dc, double left, double hCenter, double top, double charLeft,
                                        FormattedText charFt, int index, List<CharacterDecorationInfo> decorationList)
        {
            DrawRightDecorations(dc, decorationList, top, left + charLeft + charFt.GetFullWidth(), formats[index]);
            DrawLeftDecorations(dc, decorationList, top, left + charLeft, formats[index]);
            DrawFaceDecorations(dc, charFt, decorationList, top, left + hCenter);
            DrawTopDecorations(dc, charFt, decorationList, top, left + hCenter, formats[index]);
            DrawBottomDecorations(dc, charFt, decorationList, top, left + hCenter, formats[index]);
        }

        /*
        void DrawDecorations(DrawingContext dc, List<CharacterDecorationInfo> decorationList, FormattedText ft, int index, double hCenter)
        {
            double offset = FontSize * .05;
            //character metrics    
            double topPixel = ft.Height + ft.OverhangAfter - ft.Extent; //ft.Baseline - ft.Extent + descent;
            double descent = ft.Height - ft.Baseline + ft.OverhangAfter;
            double halfCharWidth = ft.GetFullWidth() / 2;
            double right = hCenter + halfCharWidth + offset;
            double left = hCenter - halfCharWidth - offset;
            double top = Top + topPixel - offset;
            double bottom = Top + ft.Baseline + descent + offset;
        }
        */
        private void DrawTopDecorations(DrawingContext dc, FormattedText ft, List<CharacterDecorationInfo> cdiList, double top, double center, int formatId)
        {
            var topDecorations = (from x in cdiList where x.Position == Position.Top select x).ToList();
            if (topDecorations.Count > 0)
            {
                top += ft.Height + ft.OverhangAfter - ft.Extent - FontSize * .1;
                foreach (var d in topDecorations)
                {
                    string text = d.UnicodeString;
                    var sign = textManager.GetFormattedText(text, textManager.GetFormatIdForNewStyle(formatId, FontStyles.Normal));
                    sign.DrawTextBottomCenterAligned(dc, new Point(center, top));
                    top -= sign.Extent + FontSize * .1;
                }
            }
        }

        private void DrawBottomDecorations(DrawingContext dc, FormattedText ft, List<CharacterDecorationInfo> cdiList, double top, double hCenter, int formatId)
        {
            var bottomDecorations = (from x in cdiList where x.Position == Position.Bottom select x).ToList();
            if (bottomDecorations.Count > 0)
            {
                top += ft.Height + ft.OverhangAfter + FontSize * .1;
                foreach (var d in bottomDecorations)
                {
                    string text = d.UnicodeString;
                    var sign = textManager.GetFormattedText(text, textManager.GetFormatIdForNewStyle(formatId, FontStyles.Normal));
                    sign.DrawTextTopCenterAligned(dc, new Point(hCenter, top));
                    top += sign.Extent + FontSize * .1;
                }
            }
        }

        private void DrawLeftDecorations(DrawingContext dc, List<CharacterDecorationInfo> cdiList, double top, double left, int formatId)
        {
            var leftDecorations = (from x in cdiList where x.Position == Position.TopLeft select x).ToList();
            if (leftDecorations.Count > 0)
            {
                string s = "";
                foreach (var d in leftDecorations)
                {
                    s = s + d.UnicodeString;
                }
                var formattedText = textManager.GetFormattedText(s, textManager.GetFormatIdForNewStyle(formatId, FontStyles.Normal));
                formattedText.DrawTextRightAligned(dc, new Point(left - FontSize * .05, top));
            }
        }

        private void DrawRightDecorations(DrawingContext dc, List<CharacterDecorationInfo> cdiList, double top, double right, int formatId)
        {
            var rightDecorations = (from x in cdiList where x.Position == Position.TopRight select x).ToList();
            if (rightDecorations.Count > 0)
            {
                string s = "";
                foreach (var d in rightDecorations)
                {
                    s = s + d.UnicodeString;
                }
                var var = textManager.GetFormattedText(s, textManager.GetFormatIdForNewStyle(formatId, FontStyles.Normal));
                var.DrawTextLeftAligned(dc, new Point(right, top));
            }
        }

        //index = index of the decorated character in this.textData
        private void DrawFaceDecorations(DrawingContext dc, FormattedText charText, List<CharacterDecorationInfo> cdiList, double top, double hCenter)
        {
            var decorations = (from x in cdiList where x.Position == Position.Over select x).ToList();
            if (decorations.Count > 0)
            {
                double offset = 0;
                top += charText.Height + charText.OverhangAfter - charText.Extent; //ft.Baseline - ft.Extent + descent;
                double bottom = top + charText.Extent; //charText.Height - charText.Baseline + charText.OverhangAfter;
                double vCenter = top + charText.Extent / 2;
                double left = hCenter - charText.GetFullWidth() / 2 - offset;
                double right = hCenter + charText.GetFullWidth() / 2 + offset;

                Pen pen = PenManager.GetPen(FontSize * .035);
                foreach (var d in decorations)
                {
                    switch (d.DecorationType)
                    {
                        case CharacterDecorationType.Cross:
                            dc.DrawLine(pen, new Point(left, top), new Point(right, bottom));
                            dc.DrawLine(pen, new Point(left, bottom), new Point(right, top));
                            break;
                        case CharacterDecorationType.LeftCross:
                            dc.DrawLine(pen, new Point(left, top), new Point(right, bottom));
                            break;
                        case CharacterDecorationType.RightCross:
                            dc.DrawLine(pen, new Point(left, bottom), new Point(right, top));
                            break;
                        case CharacterDecorationType.LeftUprightCross:
                            dc.DrawLine(pen, new Point(hCenter - FontSize * .08, top - FontSize * 0.04), new Point(hCenter + FontSize * .08, bottom + FontSize * 0.04));
                            break;
                        case CharacterDecorationType.RightUprightCross:
                            dc.DrawLine(pen, new Point(hCenter + FontSize * .08, top - FontSize * 0.04), new Point(hCenter - FontSize * .08, bottom + FontSize * 0.04));
                            break;
                        case CharacterDecorationType.StrikeThrough:
                            dc.DrawLine(pen, new Point(left, vCenter), new Point(right, vCenter));
                            break;
                        case CharacterDecorationType.DoubleStrikeThrough:
                            dc.DrawLine(pen, new Point(left, vCenter - FontSize * .05), new Point(right, vCenter - FontSize * .05));
                            dc.DrawLine(pen, new Point(left, vCenter + FontSize * .05), new Point(right, vCenter + FontSize * .05));
                            break;
                        case CharacterDecorationType.VStrikeThrough:
                            dc.DrawLine(pen, new Point(hCenter, top - FontSize * .05), new Point(hCenter, bottom + FontSize * .05));
                            break;
                        case CharacterDecorationType.VDoubleStrikeThrough:
                            dc.DrawLine(pen, new Point(hCenter - FontSize * .05, top - FontSize * .05), new Point(hCenter - FontSize * .05, bottom + FontSize * .05));
                            dc.DrawLine(pen, new Point(hCenter + FontSize * .05, top - FontSize * .05), new Point(hCenter + FontSize * .05, bottom + FontSize * .05));
                            break;
                    }
                }
            }
        }

        private double GetDecoratedCharWidth(FormattedText ft, List<CharacterDecorationInfo> decorationList,
                                             int index, out double charLeft, out double hCenter)
        {
            charLeft = 0;
            double width = ft.GetFullWidth();
            hCenter = width / 2;
            double charWidth = width;
            var lhList = from d in decorationList where d.Position == Position.TopLeft select d;
            var rhList = from d in decorationList where d.Position == Position.TopRight select d;
            var topList = from d in decorationList where d.Position == Position.Top select d;
            var bottomList = from d in decorationList where d.Position == Position.Bottom select d;
            //var vList = from d in decorationList where d.Position == Position.Top || d.Position == Position.Bottom select d;
            var oList = from d in decorationList where d.Position == Position.Over select d;
            string text = "";
            foreach (var v in lhList)
            {
                text += v.UnicodeString;
            }
            if (text.Length > 0)
            {
                var t = textManager.GetFormattedText(text, textManager.GetFormatIdForNewStyle(formats[index], FontStyles.Normal));
                width += t.GetFullWidth();
                charLeft = t.GetFullWidth();
                hCenter += charLeft;
            }
            foreach (var v in topList)
            {
                var f = textManager.GetFormattedText("f", formats[index]);
                var fTop = f.TopExtra();
                var ftTop = ft.TopExtra();
                if (fTop < ftTop)
                {
                    var t = textManager.GetFormattedText(v.UnicodeString, formats[index]);
                    double diff = t.GetFullWidth() - charWidth;
                    if (diff > 0)
                    {
                        charWidth += diff;
                        charLeft += diff / 2;
                        hCenter += diff / 2;
                    }
                }
            }
            foreach (var v in bottomList)
            {
                var t = textManager.GetFormattedText(v.UnicodeString, formats[index]);
                double diff = t.GetFullWidth() - charWidth;
                if (diff > 0)
                {
                    charWidth += diff;
                    charLeft += diff / 2;
                    hCenter += diff / 2;
                }
            }
            width = Math.Max(width, charWidth);
            foreach (var v in oList)
            {
                if (v.DecorationType == CharacterDecorationType.Cross || v.DecorationType == CharacterDecorationType.DoubleStrikeThrough ||
                                v.DecorationType == CharacterDecorationType.LeftCross || v.DecorationType == CharacterDecorationType.RightCross ||
                                v.DecorationType == CharacterDecorationType.StrikeThrough)
                {
                    double diff = (ft.GetFullWidth() + FontSize * .1) - width;
                    if (diff > 0)
                    {
                        width = ft.GetFullWidth() + FontSize * .1;
                        charLeft += diff / 2;
                        hCenter += diff / 2;
                    }
                }
            }
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

        public double OverhangTrailing
        {
            get
            {
                if (textData.Length > 0 && !char.IsWhiteSpace(textData[textData.Length - 1]))
                {
                    var ft = textManager.GetFormattedText(textData[textData.Length - 1].ToString(), formats[formats.Count - 1]);
                    return ft.OverhangTrailing;
                }
                else
                {
                    return 0;
                }
            }
        }

        public double OverhangAfter
        {
            get
            {
                if (textData.Length > 0)
                {
                    var ft = textManager.GetFormattedText(textData.ToString(), formats);
                    return ft.OverhangAfter;
                }
                else
                {
                    return 0;
                }
            }
        }

        public double GetCornerDescent(Position position)
        {
            if (textData.Length > 0)
            {
                if (position == Position.Right)
                {
                    if (!char.IsWhiteSpace(textData[textData.Length - 1]))
                    {
                        var ft = textManager.GetFormattedText(textData[textData.Length - 1].ToString(), formats[formats.Count - 1]);
                        return ft.Descent();
                    }
                }
                else
                {
                    if (!char.IsWhiteSpace(textData[0]))
                    {
                        var ft = textManager.GetFormattedText(textData[0].ToString(), formats[0]);
                        return ft.Descent();
                    }
                }
            }
            return 0;
        }

        //lolz.. I had to delete and recreate this method, as VS was raising an error that "not all code paths return a value"!!
        public override HashSet<int> GetUsedTextFormats()
        {
            return new HashSet<int>(formats);
        }

        public override void ResetTextFormats(Dictionary<int, int> formatMapping)
        {
            for (int i = 0; i < formats.Count; i++)
            {
                formats[i] = formatMapping[formats[i]];
            }
        }
    }
}
