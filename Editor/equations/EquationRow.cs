using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace Editor
{
    public class EquationRow : EquationContainer, ISupportsUndo
    {
        protected EquationContainer deleteable = null;
        static Pen boxPen = new Pen(Brushes.Blue, 1.1) { StartLineCap = PenLineCap.Flat, EndLineCap = PenLineCap.Flat };

        static EquationRow()
        {
            boxPen.DashStyle = DashStyles.Dash;
            boxPen.Freeze();
        }

        public EquationRow(EquationContainer parent)
            : base(parent)
        {
            TextEquation textEq = new TextEquation(this);
            ActiveChild = textEq;
            AddChild(textEq);
            CalculateSize();
        }

        public sealed override void CalculateSize()
        {
            base.CalculateSize();
        }

        public TextEquation GetFirstSelectionText()
        {
            return (TextEquation)childEquations[SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems];
        }

        public TextEquation GetLastSelectionText()
        {
            int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
            int otherOffset = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex);
            return (TextEquation)childEquations[otherOffset];
        }

        public void AddChildren(List<EquationBase> equations, bool insertAtStart)
        {
            if (insertAtStart)
            {
                childEquations.InsertRange(0, equations);
            }
            else
            {
                childEquations.AddRange(equations);
            }
            CalculateSize();
        }

        public TextEquation GetFirstTextEquation()
        {
            return childEquations.First() as TextEquation;
        }

        public TextEquation GetLastTextEquation()
        {
            return childEquations.Last() as TextEquation;
        }

        public List<EquationBase> GetSelectedEquations()
        {
            List<EquationBase> list = new List<EquationBase>();
            int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
            int endIndex = SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex;
            for (int i = startIndex; i <= endIndex; i++)
            {
                list.Add(childEquations[i]);
            }
            return list;
        }

        public List<EquationBase> DeleteTail()
        {
            List<EquationBase> removedList = new List<EquationBase>();
            int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
            if (SelectedItems != 0)
            {
                int endIndex = SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex;
                for (int i = endIndex; i > startIndex; i--)
                {
                    removedList.Add(childEquations[i]);
                    childEquations.RemoveAt(i);
                }
                removedList.Reverse();
            }
            ActiveChild = childEquations[startIndex];
            return removedList;
        }

        public List<EquationBase> DeleteHead()
        {
            List<EquationBase> removedList = new List<EquationBase>();
            int startIndex = (SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems);
            if (SelectedItems != 0)
            {
                int endIndex = SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex;
                for (int i = endIndex - 1; i >= startIndex; i--)
                {
                    removedList.Add(childEquations[i]);
                    childEquations.RemoveAt(i);
                }
                removedList.Reverse();
            }
            ActiveChild = childEquations[startIndex];
            return removedList;
        }

        public override void RemoveSelection(bool registerUndo)
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int otherIndex = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex);
                TextEquation firstEquation = (TextEquation)childEquations[startIndex];
                TextEquation lastEquation = (TextEquation)childEquations[otherIndex];
                List<EquationBase> equations = new List<EquationBase>();
                RowRemoveAction action = new RowRemoveAction(this)
                                         {
                                             ActiveEquation = ActiveChild,
                                             HeadTextEquation = firstEquation,
                                             TailTextEquation = lastEquation,
                                             SelectionStartIndex = SelectionStartIndex,
                                             SelectedItems = SelectedItems,
                                             FirstTextCaretIndex = firstEquation.CaretIndex,
                                             LastTextCaretIndex = lastEquation.CaretIndex,
                                             FirstTextSelectionIndex = firstEquation.SelectionStartIndex,
                                             LastTextSelectionIndex = lastEquation.SelectionStartIndex,
                                             FirstTextSelectedItems = firstEquation.SelectedItems,
                                             LastTextSelectedItems = lastEquation.SelectedItems,
                                             FirstText = firstEquation.Text,
                                             LastText = lastEquation.Text,
                                             FirstFormats = firstEquation.GetFormats(),
                                             LastFormats = lastEquation.GetFormats(),
                                             FirstModes = firstEquation.GetModes(),
                                             LastModes = lastEquation.GetModes(),
                                             FirstDecorations = firstEquation.GetDecorations(),
                                             LastDecorations = lastEquation.GetDecorations(),
                                             Equations = equations
                                         };
                firstEquation.RemoveSelection(false);
                lastEquation.RemoveSelection(false);
                firstEquation.Merge(lastEquation);
                for (int i = otherIndex; i > startIndex; i--)
                {
                    equations.Add(childEquations[i]);
                    childEquations.RemoveAt(i);
                }
                SelectedItems = 0;
                equations.Reverse();
                ActiveChild = firstEquation;
                if (registerUndo)
                {
                    UndoManager.AddUndoAction(action);
                }
            }
            else
            {
                ActiveChild.RemoveSelection(registerUndo);
            }
            CalculateSize();
        }

        public override bool Select(Key key)
        {
            if (key == Key.Left)
            {
                return HandleLeftSelect(key);
            }
            else if (key == Key.Right)
            {
                return HandleRightSelect(key);
            }
            return false;
        }
        private bool HandleRightSelect(Key key)
        {
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                if (ActiveChild.Select(key))
                {
                    return true;
                }
                else if (ActiveChild == childEquations.Last())
                {
                    return false;
                }
                else
                {
                    SelectedItems += 2;
                    ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) + 2];
                    childEquations[childEquations.IndexOf(ActiveChild) - 1].DeSelect();
                    if (SelectedItems > 0)
                    {
                        ((TextEquation)ActiveChild).MoveToStart();
                        ActiveChild.StartSelection();
                    }
                    return true;
                }
            }
            else
            {
                if (!ActiveChild.Select(key))
                {
                    TextEquation previsouText = (TextEquation)childEquations[SelectionStartIndex - 1];
                    TextEquation nextText = (TextEquation)childEquations[SelectionStartIndex + 1];
                    previsouText.MoveToEnd();
                    previsouText.StartSelection();
                    nextText.MoveToStart();
                    nextText.StartSelection();
                    SelectionStartIndex--;
                    SelectedItems += 2;
                    ActiveChild = nextText;
                }
                return true;
            }
        }
        private bool HandleLeftSelect(Key key)
        {
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                if (ActiveChild.Select(key))
                {
                    return true;
                }
                else if (ActiveChild == childEquations.First())
                {
                    return false;
                }
                else
                {
                    SelectedItems -= 2;
                    ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 2];
                    childEquations[childEquations.IndexOf(ActiveChild) + 1].DeSelect();
                    if (SelectedItems < 0)
                    {
                        ((TextEquation)ActiveChild).MoveToEnd();
                        ActiveChild.StartSelection();
                    }
                    return true;
                }
            }
            else
            {
                if (!ActiveChild.Select(key))
                {
                    TextEquation previsouText = (TextEquation)childEquations[SelectionStartIndex - 1];
                    TextEquation nextText = (TextEquation)childEquations[SelectionStartIndex + 1];
                    previsouText.MoveToEnd();
                    previsouText.StartSelection();
                    nextText.MoveToStart();
                    nextText.StartSelection();
                    SelectionStartIndex++;
                    SelectedItems -= 2;
                    ActiveChild = previsouText;
                }
                return true;
            }
        }

        public override Rect GetSelectionBounds()
        {
            try
            {
                if (IsSelecting)
                {
                    int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                    int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                    Rect firstRect = childEquations[startIndex].GetSelectionBounds();
                    if (firstRect == Rect.Empty)
                    {
                        firstRect = new Rect(childEquations[startIndex].Right, childEquations[startIndex].Top, 0, 0);
                    }
                    if (count > 0)
                    {
                        Rect lastRect = childEquations[count + startIndex].GetSelectionBounds();
                        if (lastRect == Rect.Empty)
                        {
                            lastRect = new Rect(childEquations[count + startIndex].Left, childEquations[count + startIndex].Top, 0, childEquations[count + startIndex].Height);
                        }
                        for (int i = startIndex + 1; i < startIndex + count; i++)
                        {
                            EquationBase equation = childEquations[i];
                            lastRect.Union(equation.Bounds);
                        }
                        firstRect.Union(lastRect);
                    }
                    return new Rect(firstRect.TopLeft, firstRect.BottomRight);
                }
            }
            catch
            {

            }
            return Rect.Empty;
        }

        public override void Paste(XElement xe)
        {
            if (ActiveChild.GetType() == typeof(TextEquation) && xe.Name.LocalName == GetType().Name)
            {
                XElement children = xe.Element("ChildEquations");
                List<EquationBase> newChildren = new List<EquationBase>();
                foreach (XElement xElement in children.Elements())
                {
                    newChildren.Add(CreateChild(xElement));
                }
                if (newChildren.Count > 0)
                {
                    EquationRowPasteAction action = new EquationRowPasteAction(this)
                    {
                        ActiveTextEquation = (TextEquation)ActiveChild,
                        ActiveChildCaretIndex = ((TextEquation)ActiveChild).CaretIndex,
                        SelectedItems = SelectedItems,
                        SelectionStartIndex = SelectionStartIndex,
                        ActiveChildSelectedItems = ActiveChild.SelectedItems,
                        ActiveChildSelectionStartIndex = ActiveChild.SelectionStartIndex,
                        ActiveChildText = ((TextEquation)ActiveChild).Text,
                        ActiveChildFormats = ((TextEquation)ActiveChild).GetFormats(),
                        ActiveChildModes = ((TextEquation)ActiveChild).GetModes(),
                        ActiveChildDecorations = ((TextEquation)ActiveChild).GetDecorations(),
                        FirstNewText = ((TextEquation)newChildren.First()).Text,
                        LastNewText = ((TextEquation)newChildren.Last()).Text,
                        FirstNewFormats = ((TextEquation)newChildren.First()).GetFormats(),
                        LastNewFormats = ((TextEquation)newChildren.Last()).GetFormats(),
                        FirstNewModes = ((TextEquation)newChildren.First()).GetModes(),
                        LastNewModes = ((TextEquation)newChildren.Last()).GetModes(),                                                
                        FirstNewDecorations = ((TextEquation)newChildren.First()).GetDecorations(),
                        LastNewDecorations = ((TextEquation)newChildren.Last()).GetDecorations(),
                        Equations = newChildren
                    };
                    EquationBase newChild = ActiveChild.Split(this);
                    int index = childEquations.IndexOf(ActiveChild) + 1;
                    newChildren.RemoveAt(0);
                    childEquations.InsertRange(index, newChildren);
                    ((TextEquation)ActiveChild).ConsumeFormattedText(action.FirstNewText, action.FirstNewFormats, action.FirstNewModes, action.FirstNewDecorations, false);
                    ((TextEquation)newChildren.Last()).Merge((TextEquation)newChild);
                    ActiveChild = newChildren.Last();
                    UndoManager.AddUndoAction(action);
                }
                CalculateSize();
            }
            else
            {
                base.Paste(xe);
            }
        }

        public override void DeSelect()
        {
            base.DeSelect();
            deleteable = null;
        }

        public override CopyDataObject Copy(bool removeSelection)
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                string firstText = ((TextEquation)childEquations[startIndex]).GetSelectedText();
                string lastText = ((TextEquation)childEquations[startIndex + count]).GetSelectedText();
                int[] firstFormats = ((TextEquation)childEquations[startIndex]).GetSelectedFormats();
                EditorMode[] firstModes = ((TextEquation)childEquations[startIndex]).GetSelectedModes();
                CharacterDecorationInfo[] firstDecorations = ((TextEquation)childEquations[startIndex]).GetSelectedDecorations();
                int[] lastFormats = ((TextEquation)childEquations[startIndex + count]).GetSelectedFormats();
                EditorMode[] lastModes = ((TextEquation)childEquations[startIndex + count]).GetSelectedModes();
                CharacterDecorationInfo[] lastDecorations = ((TextEquation)childEquations[startIndex + count]).GetSelectedDecorations();
                TextEquation firstEquation = new TextEquation(this);
                TextEquation lastEquation = new TextEquation(this);
                firstEquation.ConsumeFormattedText(firstText, firstFormats, firstModes, firstDecorations, false);
                lastEquation.ConsumeFormattedText(lastText, lastFormats, lastModes, lastDecorations, false);
                List<EquationBase> equations = new List<EquationBase>();
                equations.Add(firstEquation);
                for (int i = startIndex + 1; i < startIndex + count; i++)
                {
                    equations.Add(childEquations[i]);
                }
                equations.Add(lastEquation);
                double left = 0;
                foreach (EquationBase eb in equations)
                {
                    eb.Left = 1 + left;
                    left += eb.Width;
                }
                double maxUpperHalf = 0;
                double maxBottomHalf = 0;
                foreach (EquationBase eb in childEquations)
                {
                    if (eb.RefY > maxUpperHalf) { maxUpperHalf = eb.RefY; }
                    if (eb.Height - eb.RefY > maxBottomHalf) { maxBottomHalf = eb.Height - eb.RefY; }
                }
                double width = 0;
                foreach (EquationBase eb in equations)
                {
                    eb.Top = 1 + maxUpperHalf - eb.RefY;
                    width += eb.Width;
                }
                Rect rect = GetSelectionBounds();
                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)(Math.Ceiling(width + 2)), (int)(Math.Ceiling(maxUpperHalf + maxBottomHalf + 2)), 96, 96, PixelFormats.Default);
                DrawingVisual dv = new DrawingVisual();
                IsSelecting = false;
                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, bitmap.Width, bitmap.Height));
                    foreach (EquationBase eb in equations)
                    {
                        eb.DrawEquation(dc);
                    }
                }
                IsSelecting = true;
                bitmap.Render(dv);
                XElement thisElement = new XElement(GetType().Name);
                XElement children = new XElement("ChildEquations");
                foreach (EquationBase eb in equations)
                {
                    eb.SelectAll();
                    children.Add(eb.Serialize());
                }
                thisElement.Add(children);
                //data.SetText(GetSelectedText());
                foreach (EquationBase eb in equations)
                {
                    eb.DeSelect();
                }
                Left = Left;
                Top = Top;
                if (removeSelection)
                {
                    RemoveSelection(true);
                }
                return new CopyDataObject { Image = bitmap, Text = null, XElement = thisElement };
            }
            else
            {
                return base.Copy(removeSelection);
            }
        }

        public override void DrawEquation(DrawingContext dc)
        {
            base.DrawEquation(dc);
            if (deleteable != null)
            {
                Brush brush = new SolidColorBrush(Colors.Gray);
                brush.Opacity = 0.5;
                dc.DrawRectangle(brush, null, new Rect(deleteable.Location, deleteable.Size));
            }
            if (childEquations.Count == 1)
            {
                TextEquation firstEquation = (TextEquation)childEquations.First();
                if (firstEquation.TextLength == 0)
                {
                    if (IsSelecting)
                    {
                        //dc.DrawRectangle(Brushes.LightGray, null, new Rect(new Point(Left - 1, Top), new Size(FontSize / 2.5, Height)));
                    }
                    dc.DrawRectangle(null, boxPen, new Rect(Left, Top, Width, Height + ThinLineThickness));//new Rect(new Point(Left - 1, Top), new Size(FontSize / 2.5, Height)));
                }
            }
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement children = new XElement("ChildEquations");
            foreach (EquationBase childEquation in childEquations)
            {
                children.Add(childEquation.Serialize());
            }
            thisElement.Add(children);
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            XElement children = xElement.Element("ChildEquations");
            childEquations.Clear();
            foreach (XElement xe in children.Elements())
            {
                childEquations.Add(CreateChild(xe));
            }
            if (childEquations.Count == 0)
            {
                childEquations.Add(new TextEquation(this));
            }
            ActiveChild = childEquations.First();
            CalculateSize();
        }

        EquationBase CreateChild(XElement xElement)
        {
            Type type = Type.GetType(GetType().Namespace + "." + xElement.Name);
            List<object> paramz = new List<object>();
            paramz.Add(this);
            XElement parameters = xElement.Element("parameters");
            if (parameters != null)
            {
                foreach (XElement xe in parameters.Elements())
                {
                    Type paramType = Type.GetType(GetType().Namespace + "." + xe.Name);
                    if (paramType == null)
                    {
                        paramType = Type.GetType(xe.Name.ToString());
                    }
                    if (paramType.IsEnum)
                    {
                        paramz.Add((Enum.Parse(paramType, xe.Value)));
                    }
                    else if (paramType == typeof(bool))
                    {
                        paramz.Add(bool.Parse(xe.Value));
                    }
                    else if (paramType == typeof(int))
                    {
                        paramz.Add(int.Parse(xe.Value));
                    }
                    else
                    {
                        paramz.Add(xe.Value);
                    }
                }
            }
            EquationBase child = (EquationBase)Activator.CreateInstance(type, paramz.ToArray());
            child.DeSerialize(xElement);
            child.FontSize = FontSize;
            return child;
        }

        public static bool UseItalicIntergalOnNew { get; set; }

        public override void ExecuteCommand(CommandType commandType, object data)
        {
            deleteable = null;
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                EquationBase newEquation = null;
                switch (commandType)
                {
                    case CommandType.Composite:
                        newEquation = CompositeFactory.CreateEquation(this, (Position)data);
                        break;
                    case CommandType.CompositeBig:
                        newEquation = BigCompositeFactory.CreateEquation(this, (Position)data);
                        break;
                    case CommandType.Division:
                        newEquation = DivisionFactory.CreateEquation(this, (DivisionType)data);
                        break;
                    case CommandType.SquareRoot:
                        newEquation = new SquareRoot(this);
                        break;
                    case CommandType.nRoot:
                        newEquation = new nRoot(this);
                        break;
                    case CommandType.LeftBracket:
                        newEquation = new LeftBracket(this, (BracketSignType)data);
                        break;
                    case CommandType.RightBracket:
                        newEquation = new RightBracket(this, (BracketSignType)data);
                        break;
                    case CommandType.LeftRightBracket:
                        newEquation = new LeftRightBracket(this, ((BracketSignType[])data)[0], ((BracketSignType[])data)[1]);
                        break;
                    case CommandType.Sub:
                        newEquation = new Sub(this, (Position)data);
                        break;
                    case CommandType.Super:
                        newEquation = new Super(this, (Position)data);
                        break;
                    case CommandType.SubAndSuper:
                        newEquation = new SubAndSuper(this, (Position)data);
                        break;
                    case CommandType.TopBracket:
                        newEquation = new TopBracket(this, (HorizontalBracketSignType)data);
                        break;
                    case CommandType.BottomBracket:
                        newEquation = new BottomBracket(this, (HorizontalBracketSignType)data);
                        break;
                    case CommandType.DoubleArrowBarBracket:
                        newEquation = new DoubleArrowBarBracket(this);
                        break;
                    case CommandType.SignComposite:
                        newEquation = SignCompositeFactory.CreateEquation(this, (Position)(((object[])data)[0]), (SignCompositeSymbol)(((object[])data)[1]), UseItalicIntergalOnNew);
                        break;
                    case CommandType.Decorated:
                        newEquation = new Decorated(this, (DecorationType)(((object[])data)[0]), (Position)(((object[])data)[1]));
                        break;
                    case CommandType.Arrow:
                        newEquation = new Arrow(this, (ArrowType)(((object[])data)[0]), (Position)(((object[])data)[1]));
                        break;
                    case CommandType.Box:
                        newEquation = new Box(this, (BoxType)data);
                        break;
                    case CommandType.Matrix:
                        newEquation = new MatrixEquation(this, ((int[])data)[0], ((int[])data)[1]);
                        break;
                    case CommandType.DecoratedCharacter:
                        if (((TextEquation)ActiveChild).CaretIndex > 0)
                        {
                            //newEquation = new DecoratedCharacter(this,
                            //                                     (TextEquation)ActiveChild,
                            //                                     (CharacterDecorationType)((object[])data)[0],
                            //                                     (Position)((object[])data)[1],
                            //                                     (string)((object[])data)[2]);
                            ((TextEquation)ActiveChild).AddDecoration((CharacterDecorationType)((object[])data)[0],
                                                                      (Position)((object[])data)[1],
                                                                      (string)((object[])data)[2]);
                            CalculateSize();
                        }
                        break;
                }
                if (newEquation != null)
                {
                    EquationBase newText = ActiveChild.Split(this);
                    int caretIndex = ((TextEquation)ActiveChild).TextLength;
                    AddChild(newEquation);
                    AddChild(newText);                    
                    newEquation.CalculateSize();
                    ActiveChild = newEquation;
                    CalculateSize();                    
                    UndoManager.AddUndoAction(new RowAction(this, ActiveChild, (TextEquation)newText, childEquations.IndexOf(ActiveChild), caretIndex));
                }
            }
            else if (ActiveChild != null)
            {
                ((EquationContainer)ActiveChild).ExecuteCommand(commandType, data);
                CalculateSize();
            }
        }

        void AddChild(EquationBase newChild)
        {
            int index = 0;
            if (childEquations.Count > 0)
            {
                index = childEquations.IndexOf(ActiveChild) + 1;
            }
            childEquations.Insert(index, newChild);
            newChild.ParentEquation = this;
            ActiveChild = newChild;
        }

        void RemoveChild(EquationBase child)
        {
            childEquations.Remove(child);
            CalculateSize();
        }

        public override void HandleMouseDrag(Point mousePoint)
        {
            if (mousePoint.X < ActiveChild.Left)
            {
                HandleLeftDrag(mousePoint);
            }
            else if (mousePoint.X > ActiveChild.Right)
            {
                HandleRightDrag(mousePoint);
            }
            else
            {
                ActiveChild.HandleMouseDrag(mousePoint);
            }
            SelectedItems = childEquations.IndexOf(ActiveChild) - SelectionStartIndex;
        }

        private void HandleRightDrag(Point mousePoint)
        {
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                ((TextEquation)ActiveChild).SelectToEnd();
                if (ActiveChild != childEquations.Last())
                {
                    if (mousePoint.X > childEquations[childEquations.IndexOf(ActiveChild) + 1].MidX)
                    {
                        childEquations[childEquations.IndexOf(ActiveChild) + 1].SelectAll();
                        ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) + 2];
                        //childEquations[childEquations.IndexOf(ActiveChild) - 1].DeSelect();
                        if (childEquations.IndexOf(ActiveChild) > SelectionStartIndex) // old-> (SelectedItems > 0)
                        {
                            ((TextEquation)ActiveChild).MoveToStart();
                            ActiveChild.StartSelection();
                            ActiveChild.HandleMouseDrag(mousePoint);
                        }
                    }
                }
            }
            else
            {
                TextEquation previsouText = (TextEquation)childEquations[SelectionStartIndex - 1];
                TextEquation nextText = (TextEquation)childEquations[SelectionStartIndex + 1];
                previsouText.MoveToEnd();
                previsouText.StartSelection();
                nextText.MoveToStart();
                nextText.StartSelection();
                SelectionStartIndex--;
                ActiveChild = nextText;
                ActiveChild.HandleMouseDrag(mousePoint);
            }
        }

        private void HandleLeftDrag(Point mousePoint)
        {
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                ((TextEquation)ActiveChild).SelectToStart();
                if (ActiveChild != childEquations.First())
                {
                    if (mousePoint.X < childEquations[childEquations.IndexOf(ActiveChild) - 1].MidX)
                    {
                        childEquations[childEquations.IndexOf(ActiveChild) - 1].SelectAll();
                        ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 2];
                        //childEquations[childEquations.IndexOf(ActiveChild) + 1].DeSelect();
                        if (childEquations.IndexOf(ActiveChild) < SelectionStartIndex)      // old -> (SelectedItems < 0)
                        {
                            ((TextEquation)ActiveChild).MoveToEnd();
                            ActiveChild.StartSelection();
                            ActiveChild.HandleMouseDrag(mousePoint);
                        }
                    }
                }
            }
            else
            {
                TextEquation previsouText = (TextEquation)childEquations[SelectionStartIndex - 1];
                TextEquation nextText = (TextEquation)childEquations[SelectionStartIndex + 1];
                previsouText.MoveToEnd();
                previsouText.StartSelection();
                nextText.MoveToStart();
                nextText.StartSelection();
                SelectionStartIndex++;
                ActiveChild = previsouText;
                ActiveChild.HandleMouseDrag(mousePoint);
            }
        }

        public override void SetCursorOnKeyUpDown(Key key, Point point)
        {
            foreach (EquationBase eb in childEquations)
            {
                if (eb.Right >= point.X)
                {
                    eb.SetCursorOnKeyUpDown(key, point);
                    ActiveChild = eb;
                    break;
                }
            }
        }

        public override bool ConsumeMouseClick(Point mousePoint)
        {
            deleteable = null;
            ActiveChild = null;
            foreach (EquationBase eb in childEquations)
            {
                if (eb.Right >= mousePoint.X && eb.Left <= mousePoint.X)
                {
                    ActiveChild = eb;
                    break;
                }
            }
            if (ActiveChild == null)
            {
                if (mousePoint.X <= MidX)
                    ActiveChild = childEquations.First();
                else
                    ActiveChild = childEquations.Last();
            }
            if (!ActiveChild.ConsumeMouseClick(mousePoint))
            {
                bool moveToStart = true;
                if (childEquations.Count == 1)
                {
                    if (ActiveChild.MidX < mousePoint.X)
                    {
                        moveToStart = false;
                    }
                }
                else if (mousePoint.X < ActiveChild.MidX)
                {
                    if (ActiveChild != childEquations.First())
                    {
                        ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 1];
                        moveToStart = false;
                    }
                }
                else if (ActiveChild != childEquations.Last())
                {
                    ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) + 1];
                }
                else
                {
                    moveToStart = false;
                }
                TextEquation equation = ActiveChild as TextEquation;
                if (equation != null)
                {
                    if (moveToStart)
                    {
                        equation.MoveToStart();
                    }
                    else
                    {
                        equation.MoveToEnd();
                    }
                }
            }
            return true;
        }

        public override bool ConsumeKey(Key key)
        {
            bool result = false;
            if (key == Key.Home)
            {
                ActiveChild = childEquations.First();
            }
            else if (key == Key.End)
            {
                ActiveChild = childEquations.Last();
            }
            if (ActiveChild.ConsumeKey(key))
            {
                deleteable = null;
                result = true;
            }
            else if (key == Key.Delete)
            {
                if (ActiveChild.GetType() == typeof(TextEquation) && ActiveChild != childEquations.Last())
                {
                    if (childEquations[childEquations.IndexOf(ActiveChild) + 1] == deleteable)
                    {
                        UndoManager.AddUndoAction(new RowAction(this, deleteable, (TextEquation)childEquations[childEquations.IndexOf(deleteable) + 1], 
                                                                childEquations.IndexOf(deleteable), TextLength) { UndoFlag = false});
                        childEquations.Remove(deleteable);
                        deleteable = null;
                        ((TextEquation)ActiveChild).Merge((TextEquation)childEquations[childEquations.IndexOf(ActiveChild) + 1]);
                        childEquations.Remove(childEquations[childEquations.IndexOf(ActiveChild) + 1]);
                    }
                    else
                    {
                        deleteable = (EquationContainer)childEquations[childEquations.IndexOf(ActiveChild) + 1];
                    }
                    result = true;
                }
            }
            else if (key == Key.Back)
            {
                if (ActiveChild.GetType() == typeof(TextEquation))
                {
                    if (ActiveChild != childEquations.First())
                    {
                        if ((EquationContainer)childEquations[childEquations.IndexOf(ActiveChild) - 1] == deleteable)
                        {
                            TextEquation equationAfter = (TextEquation)ActiveChild;
                            ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 2];
                            UndoManager.AddUndoAction(new RowAction(this, deleteable, equationAfter, childEquations.IndexOf(deleteable), TextLength) { UndoFlag = false });
                            childEquations.Remove(deleteable);
                            ((TextEquation)ActiveChild).Merge(equationAfter);
                            childEquations.Remove(equationAfter);
                            deleteable = null;
                        }
                        else
                        {
                            deleteable = (EquationContainer)childEquations[childEquations.IndexOf(ActiveChild) - 1];
                        }
                        result = true;
                    }
                }
                else
                {
                    if (deleteable == ActiveChild)
                    {
                        TextEquation equationAfter = (TextEquation)childEquations[childEquations.IndexOf(ActiveChild) + 1];
                        ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 1];
                        UndoManager.AddUndoAction(new RowAction(this, deleteable, equationAfter, childEquations.IndexOf(deleteable), TextLength) { UndoFlag = false });
                        childEquations.Remove(deleteable);
                        ((TextEquation)ActiveChild).Merge(equationAfter);
                        childEquations.Remove(equationAfter);
                        deleteable = null;
                    }
                    else
                    {
                        deleteable = (EquationContainer)ActiveChild;
                    }
                    result = true;
                }
            }
            if (!result)
            {
                deleteable = null;
                if (key == Key.Right)
                {
                    if (ActiveChild != childEquations.Last())
                    {
                        ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) + 1];
                        result = true;
                    }
                }
                else if (key == Key.Left)
                {
                    if (ActiveChild != childEquations.First())
                    {
                        ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 1];
                        result = true;
                    }
                }
            }
            CalculateSize();
            return result;
        }

        public void Merge(EquationRow secondLine)
        {
            ((TextEquation)childEquations.Last()).Merge((TextEquation)secondLine.childEquations.First()); //first and last are always of tyep TextEquation
            for (int i = 1; i < secondLine.childEquations.Count; i++)
            {
                AddChild(secondLine.childEquations[i]);
            }
            CalculateSize();
        }

        void SplitRow(EquationRow newRow)
        {
            int index = childEquations.IndexOf(ActiveChild) + 1;
            EquationBase newChild = ActiveChild.Split(newRow);

            if (newChild != null)
            {
                newRow.RemoveChild(newRow.ActiveChild);
                newRow.AddChild(newChild);
                int i = index;
                for (; i < childEquations.Count; i++)
                {
                    newRow.AddChild(childEquations[i]);
                }
                for (i = childEquations.Count - 1; i >= index; i--)
                {
                    RemoveChild(childEquations[i]);
                }
            }
        }

        public override EquationBase Split(EquationContainer newParent)
        {
            deleteable = null;
            EquationRow newRow = null;
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                newRow = new EquationRow(newParent);
                SplitRow(newRow);
                newRow.CalculateSize();
            }
            else
            {
                ActiveChild.Split(this);
            }
            CalculateSize();
            return newRow;
        }

        public void Truncate()
        {
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                deleteable = null;
                ((TextEquation)ActiveChild).Truncate();
                int index = childEquations.IndexOf(ActiveChild) + 1;
                int i = index;
                for (i = childEquations.Count - 1; i >= index; i--)
                {
                    RemoveChild(childEquations[i]);
                }
            }
            CalculateSize();
        }

        protected override void CalculateWidth()
        {
            double width = 0;
            foreach (EquationBase eb in childEquations)
            {
                //eb.Left = Left + left;
                width += eb.Width + eb.Margin.Left + eb.Margin.Right;
            }
            if (childEquations.Count > 1)
            {
                width -= childEquations.Last().Width == 0 ? childEquations[childEquations.Count - 2].Margin.Right : 0;
                width -= childEquations.First().Width == 0 ? childEquations[1].Margin.Left : 0;
            }
            Width = width;
        }

        protected override void CalculateHeight()
        {
            double maxUpperHalf = 0;
            double maxBottomHalf = 0;
            foreach (EquationBase eb in childEquations)
            {
                if (eb.GetType() == typeof(Super) || eb.GetType() == typeof(Sub) || eb.GetType() == typeof(SubAndSuper))
                {
                    SubSuperBase subSuperBase = (SubSuperBase)eb;
                    if (subSuperBase.Position == Position.Right)
                    {
                        subSuperBase.SetBuddy(PreviousNonEmptyChild(subSuperBase));
                    }
                    else
                    {
                        subSuperBase.SetBuddy(NextNonEmptyChild(subSuperBase));
                    }
                }
                double childRefY = eb.RefY;
                double childHeight = eb.Height;
                if (childRefY > maxUpperHalf)
                {
                    maxUpperHalf = childRefY;
                }
                if (childHeight - childRefY > maxBottomHalf)
                {
                    maxBottomHalf = childHeight - childRefY;
                }
            }
            Height = maxUpperHalf + maxBottomHalf;
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                double left = 0;
                for (int i = 0; i < childEquations.Count; i++)
                {
                    childEquations[i].Left = left + value + (left == 0 && i == 1 ? 0 : childEquations[i].Margin.Left);
                    left += childEquations[i].Width + childEquations[i].Margin.Right + (left == 0 && i == 1 ? 0 : childEquations[i].Margin.Left);
                }
            }
        }

        public override double RefY
        {
            get
            {
                return childEquations.First().MidY - Top;
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                double maxUpperHalf = 0;
                foreach (EquationBase eb in childEquations)
                {
                    maxUpperHalf = Math.Max(maxUpperHalf, eb.RefY);
                }
                foreach (EquationBase eb in childEquations)
                {
                    eb.Top = (Top + maxUpperHalf) - eb.RefY;
                }
            }
        }

        void AdjustChildrenVertical(double maxUpperHalf)
        {
            foreach (EquationBase eb in childEquations)
            {
                eb.Top = (Top + maxUpperHalf) - eb.RefY;
            }
        }

        public override double Width
        {
            get { return base.Width; }
            set
            {
                if (value > 0)
                {
                    base.Width = value;
                }
                else
                {
                    base.Width = FontSize / 2;
                }
            }
        }

        public void ProcessUndo(EquationAction action)
        {
            deleteable = null;
            if (action.GetType() == typeof(RowAction))
            {
                ProcessRowAction(action);
                IsSelecting = false;
            }
            else if (action.GetType() == typeof(EquationRowPasteAction))
            {
                ProcessRowPasteAction(action);
            }
            else if (action.GetType() == typeof(EquationRowFormatAction))
            {
                ProcessEquationRowFormatAction(action);
            }
            else
            {
                ProcessRowRemoveAction(action);
            }
            CalculateSize();
            if (ParentEquation != null)
            {
                ParentEquation.ChildCompletedUndo(this);
            }
        }

        public void ResetRowEquation(int activeChildIndex, int selectionStartIndex, int selectedItems, List<EquationBase> items, bool appendAtEnd)
        {
            this.SelectionStartIndex = selectionStartIndex;
            this.SelectedItems = selectedItems;
            int index = 0;
            if (appendAtEnd)
            {
                index = childEquations.Count;
            }
            for (int i = 0; i < items.Count; i++)
            {
                childEquations.Insert(i + index, items[i]);
            }
            this.ActiveChild = childEquations[activeChildIndex];
        }

        public void ResetRowEquation(int activeChildIndex, int selectionStartIndex, int selectedItems)
        {
            this.SelectionStartIndex = selectionStartIndex;
            this.SelectedItems = selectedItems;
            this.ActiveChild = childEquations[activeChildIndex];
        }

        public void ResetRowEquation(EquationBase activeChild, int selectionStartIndex, int selectedItems)
        {
            this.SelectionStartIndex = selectionStartIndex;
            this.SelectedItems = selectedItems;
            this.ActiveChild = activeChild;
        }

        private void ProcessRowRemoveAction(EquationAction action)
        {
            RowRemoveAction rowAction = action as RowRemoveAction;
            rowAction.HeadTextEquation.ResetTextEquation(rowAction.FirstTextCaretIndex, rowAction.FirstTextSelectionIndex,
                                                         rowAction.FirstTextSelectedItems, rowAction.FirstText, rowAction.FirstFormats, 
                                                         rowAction.FirstModes, rowAction.FirstDecorations);
            rowAction.TailTextEquation.ResetTextEquation(rowAction.LastTextCaretIndex, rowAction.LastTextSelectionIndex,
                                                         rowAction.LastTextSelectedItems, rowAction.LastText, 
                                                         rowAction.LastFormats, rowAction.LastModes, rowAction.LastDecorations);
            if (rowAction.UndoFlag)
            {
                childEquations.InsertRange(childEquations.IndexOf(rowAction.HeadTextEquation) + 1, rowAction.Equations);
                ActiveChild = rowAction.ActiveEquation;
                foreach (EquationBase eb in rowAction.Equations)
                {
                    eb.FontSize = FontSize;
                }
                SelectedItems = rowAction.SelectedItems;
                SelectionStartIndex = rowAction.SelectionStartIndex;
                IsSelecting = true;                               
            }
            else
            {
                rowAction.HeadTextEquation.RemoveSelection(false); //.DeleteSelectedText();
                rowAction.TailTextEquation.RemoveSelection(false); //.DeleteSelectedText();
                rowAction.HeadTextEquation.Merge(rowAction.TailTextEquation);
                int index = childEquations.IndexOf(rowAction.HeadTextEquation);
                for (int i = index + rowAction.Equations.Count; i > index; i--)
                {
                    childEquations.RemoveAt(i);
                }
                ActiveChild = rowAction.HeadTextEquation;
                this.SelectedItems = 0;
                IsSelecting = false; 
            }
        }

        private void ProcessRowPasteAction(EquationAction action)
        {
            EquationRowPasteAction pasteAction = action as EquationRowPasteAction;
            TextEquation activeText = pasteAction.ActiveTextEquation;
            activeText.ResetTextEquation(pasteAction.ActiveChildCaretIndex, pasteAction.ActiveChildSelectionStartIndex, pasteAction.ActiveChildSelectedItems,
                                         pasteAction.ActiveChildText, pasteAction.ActiveChildFormats, pasteAction.ActiveChildModes, pasteAction.ActiveChildDecorations);
            ActiveChild = activeText;
            if (pasteAction.UndoFlag)
            {
                SelectedItems = pasteAction.SelectedItems;
                SelectionStartIndex = pasteAction.SelectionStartIndex;
                foreach (EquationBase eb in pasteAction.Equations)
                {
                    childEquations.Remove(eb);
                }
            }
            else
            {
                ((TextEquation)pasteAction.Equations.Last()).ResetTextEquation(0, 0, 0, pasteAction.LastNewText, pasteAction.LastNewFormats, pasteAction.LastNewModes, pasteAction.LastNewDecorations);
                EquationBase newChild = ActiveChild.Split(this);
                int index = childEquations.IndexOf(ActiveChild) + 1;
                childEquations.InsertRange(index, pasteAction.Equations);
                ((TextEquation)ActiveChild).ConsumeFormattedText(pasteAction.FirstNewText, pasteAction.FirstNewFormats, pasteAction.FirstNewModes, pasteAction.FirstNewDecorations, false);
                ((TextEquation)pasteAction.Equations.Last()).Merge((TextEquation)newChild);
                ActiveChild = childEquations[index + pasteAction.Equations.Count - 1];
                foreach (EquationBase eb in pasteAction.Equations)
                {
                    eb.FontSize = FontSize;
                }
                SelectedItems = 0;
            }
        }

        private void ProcessRowAction(EquationAction action)
        {
            RowAction rowAction = action as RowAction;
            if (rowAction.UndoFlag)
            {
                childEquations.Remove(rowAction.Equation);
                ActiveChild = childEquations.ElementAt(rowAction.Index - 1);
                ((TextEquation)ActiveChild).Merge(rowAction.EquationAfter);
                childEquations.RemoveAt(rowAction.Index);
            }
            else
            {
                ActiveChild = childEquations[rowAction.Index - 1];
                ((TextEquation)ActiveChild).Truncate(rowAction.CaretIndex);
                childEquations.Insert(rowAction.Index, rowAction.Equation);
                childEquations.Insert(rowAction.Index + 1, rowAction.EquationAfter);
                ActiveChild = rowAction.Equation;
                rowAction.Equation.FontSize = this.FontSize;
                rowAction.EquationAfter.FontSize = this.FontSize;
            }
        }

        private void ProcessEquationRowFormatAction(EquationAction action)
        {
            EquationRowFormatAction ecfa = action as EquationRowFormatAction;
            if (ecfa != null)
            {
                IsSelecting = true;
                this.SelectedItems = ecfa.SelectedItems;
                this.SelectionStartIndex = ecfa.SelectionStartIndex;
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int endIndex = SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex;
                childEquations[startIndex].SelectionStartIndex = ecfa.FirstChildSelectionStartIndex;
                childEquations[startIndex].SelectedItems = ecfa.FirstChildSelectedItems;
                childEquations[endIndex].SelectionStartIndex = ecfa.LastChildSelectionStartIndex;
                childEquations[endIndex].SelectedItems = ecfa.LastChildSelectedItems;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (i > startIndex && i < endIndex)
                    {
                        childEquations[i].SelectAll();
                    }
                    childEquations[i].ModifySelection(ecfa.Operation, ecfa.Argument, ecfa.UndoFlag ? !ecfa.Applied : ecfa.Applied, false);
                }
                CalculateSize();
                ParentEquation.ChildCompletedUndo(this);
            }
        }

        public void Truncate(int indexFrom, int keepCount)
        {
            childEquations.RemoveRange(indexFrom, childEquations.Count - indexFrom);
            ((TextEquation)childEquations[indexFrom - 1]).Truncate(keepCount);
            CalculateSize();
        }

        public void SetCurrentChild(int childIndex, int caretIndex)
        {
            TextEquation textEquation = childEquations[childIndex] as TextEquation;
            textEquation.CaretIndex = caretIndex;
            ActiveChild = textEquation;
        }

        public bool IsEmpty
        {
            get
            {
                if (childEquations.Count == 1)
                {
                    if (string.IsNullOrEmpty(((TextEquation)ActiveChild).Text))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public int ActiveChildIndex
        {
            get
            {
                return childEquations.IndexOf(ActiveChild);
            }
            set
            {
                ActiveChild = childEquations[value];
            }
        }

        public int TextLength
        {
            get
            {
                return ((TextEquation)ActiveChild).TextLength;
            }
        }

        public void SelectToStart()
        {
            if (childEquations[SelectionStartIndex].GetType() == typeof(TextEquation))
            {
                ((TextEquation)childEquations[SelectionStartIndex]).SelectToStart();
            }
            else
            {
                SelectionStartIndex++;
                ((TextEquation)childEquations[SelectionStartIndex]).MoveToStart();
                childEquations[SelectionStartIndex].StartSelection();
            }
            for (int i = SelectionStartIndex - 2; i >= 0; i -= 2)
            {
                ((TextEquation)childEquations[i]).MoveToEnd();
                childEquations[i].StartSelection();
                ((TextEquation)childEquations[i]).SelectToStart();
                childEquations[i + 1].SelectAll();
            }
            SelectedItems = -SelectionStartIndex;
            ActiveChild = childEquations[0];
        }

        public void SelectToEnd()
        {
            if (childEquations[SelectionStartIndex].GetType() == typeof(TextEquation))
            {
                ((TextEquation)childEquations[SelectionStartIndex]).SelectToEnd();
            }
            else
            {
                SelectionStartIndex--;
                ((TextEquation)childEquations[SelectionStartIndex]).MoveToEnd();
                childEquations[SelectionStartIndex].StartSelection();
            }
            for (int i = SelectionStartIndex + 2; i < childEquations.Count; i += 2)
            {
                ((TextEquation)childEquations[i]).MoveToStart();
                childEquations[i].StartSelection();
                ((TextEquation)childEquations[i]).SelectToEnd();
                childEquations[i - 1].SelectAll();
            }
            SelectedItems = childEquations.Count - SelectionStartIndex - 1;
            ActiveChild = childEquations.Last();
        }

        public EquationBase PreviousNonEmptyChild(EquationContainer equation)
        {
            int index = childEquations.IndexOf(equation) - 1;
            if (index >= 0)
            {
                if (index >= 1 && ((TextEquation)childEquations[index]).TextLength == 0)
                {
                    index--;
                }
                return childEquations[index];
            }
            else
            {
                return null;
            }
        }

        public EquationBase NextNonEmptyChild(EquationContainer equation)
        {
            int index = childEquations.IndexOf(equation) + 1;
            if (index < childEquations.Count)
            {
                if (index < childEquations.Count - 1 && ((TextEquation)childEquations[index]).TextLength == 0)
                {
                    index++;
                }
                return childEquations[index];
            }
            else
            {
                return null;
            }
        }

        public override void SelectAll()
        {
            base.SelectAll();
            ((TextEquation)childEquations.Last()).MoveToEnd();
        }

        public void MoveToStart()
        {
            ActiveChild = childEquations[0];
            ((TextEquation)ActiveChild).MoveToStart();
        }

        public void MoveToEnd()
        {
            ActiveChild = childEquations.Last();
            ((TextEquation)ActiveChild).MoveToEnd();
        }

        public override double GetVerticalCaretLength()
        {
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                return Height;
            }
            else
            {
                return ActiveChild.GetVerticalCaretLength();
            }
        }

        public override Point GetVerticalCaretLocation()
        {
            if (ActiveChild.GetType() == typeof(TextEquation))
            {
                return new Point(ActiveChild.GetVerticalCaretLocation().X, Top);
            }
            else
            {
                return ActiveChild.GetVerticalCaretLocation();
            }
        }

        public override void ModifySelection(string operation, string argument, bool applied, bool addUndo)
        {
            if (IsSelecting)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int endIndex = SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex;
                if (endIndex - startIndex > 0)
                {
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        childEquations[i].ModifySelection(operation, argument, applied, false);
                    }
                    if (addUndo)
                    {
                        EquationRowFormatAction ecfa = new EquationRowFormatAction(this)
                        {
                            Operation = operation,
                            Argument = argument,
                            Applied = applied,
                            SelectionStartIndex = SelectionStartIndex,
                            SelectedItems = SelectedItems,
                            FirstChildSelectionStartIndex = childEquations[startIndex].SelectionStartIndex,
                            FirstChildSelectedItems = childEquations[startIndex].SelectedItems,
                            LastChildSelectionStartIndex = childEquations[endIndex].SelectionStartIndex,
                            LastChildSelectedItems = childEquations[endIndex].SelectedItems,
                        };
                        UndoManager.AddUndoAction(ecfa);
                    }
                }
                else
                {
                    ActiveChild.ModifySelection(operation, argument, applied, addUndo);
                }
                CalculateSize();
            }
        }
    }
}
