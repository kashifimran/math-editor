using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using System.IO;
using System.Windows.Media.Imaging;

namespace Editor
{
    public class RowContainer : EquationContainer, ISupportsUndo
    {
        double lineSpaceFactor;
        double LineSpace
        {
            get { return lineSpaceFactor * FontSize; }
        }

        public EquationBase FirstRow { get { return childEquations.First(); } }
        public EquationBase LastRow { get { return childEquations.Last(); } }

        public override void Paste(XElement xe)
        {
            if (((EquationRow)ActiveChild).ActiveChild.GetType() == typeof(TextEquation) && xe.Name.LocalName == GetType().Name)
            {
                XElement children = xe.Element("ChildRows");
                List<EquationRow> newRows = new List<EquationRow>();
                foreach (XElement xElement in children.Elements())
                {
                    EquationRow row = new EquationRow(this);
                    row.DeSerialize(xElement);
                    newRows.Add(row);
                    row.FontSize = FontSize;
                }
                if (newRows.Count > 0)
                {
                    TextEquation activeText = (TextEquation)((EquationRow)ActiveChild).ActiveChild;
                    RowContainerPasteAction action = new RowContainerPasteAction(this)
                    {
                        ActiveEquation = ActiveChild,
                        ActiveEquationSelectedItems = ActiveChild.SelectedItems,
                        ActiveEquationSelectionIndex = ActiveChild.SelectionStartIndex,
                        ActiveTextInChildRow = activeText,
                        TextEquationDecorations = activeText.GetDecorations(),
                        CaretIndexOfActiveText = activeText.CaretIndex,
                        TextEquationContents = activeText.Text,
                        TextEquationFormats = activeText.GetFormats(),
                        TextEquationModes = activeText.GetModes(),
                        SelectedItems = SelectedItems,
                        SelectionStartIndex = SelectionStartIndex,
                        SelectedItemsOfTextEquation = activeText.SelectedItems,
                        SelectionStartIndexOfTextEquation = activeText.SelectionStartIndex,
                        HeadTextOfPastedRows = newRows[0].GetFirstTextEquation().Text,
                        TailTextOfPastedRows = newRows.Last().GetLastTextEquation().Text,
                        HeadFormatsOfPastedRows = newRows[0].GetFirstTextEquation().GetFormats(),
                        TailFormatsOfPastedRows = newRows.Last().GetLastTextEquation().GetFormats(),
                        HeadModeOfPastedRows = newRows[0].GetFirstTextEquation().GetModes(),
                        TailModesOfPastedRows = newRows.Last().GetLastTextEquation().GetModes(),                        
                        HeadDecorationsOfPastedRows = newRows[0].GetFirstTextEquation().GetDecorations(),
                        TailDecorationsOfPastedRows = newRows.Last().GetLastTextEquation().GetDecorations(),
                        Equations = newRows
                    };
                    EquationRow newRow = (EquationRow)ActiveChild.Split(this);
                    ((EquationRow)ActiveChild).Merge(newRows[0]);
                    int index = childEquations.IndexOf(ActiveChild) + 1;
                    childEquations.InsertRange(index, newRows.Skip(1));
                    newRows.Last().Merge(newRow);
                    newRows.Add(newRow);
                    ActiveChild = childEquations[index + newRows.Count - 3];
                    UndoManager.AddUndoAction(action);
                }
                CalculateSize();
            }
            else
            {
                base.Paste(xe);
            }
        }

        public override void ConsumeText(string text)
        {
            if (((EquationRow)ActiveChild).ActiveChild.GetType() == typeof(TextEquation))
            {
                List<string> lines = new List<string>();
                using (StringReader reader = new StringReader(text))
                {
                    string s;
                    while ((s = reader.ReadLine()) != null)
                    {
                        lines.Add(s);
                    }
                }
                if (lines.Count == 1)
                {
                    ActiveChild.ConsumeText(lines[0]);
                }
                else if (lines.Count > 1)
                {
                    List<EquationRow> newEquations = new List<EquationRow>();
                    TextEquation activeText = (TextEquation)((EquationRow)ActiveChild).ActiveChild;
                    RowContainerTextAction action = new RowContainerTextAction(this)
                    {
                        ActiveEquation = ActiveChild,
                        SelectedItems = SelectedItems,
                        SelectionStartIndex = SelectionStartIndex,
                        ActiveEquationSelectedItems = ActiveChild.SelectedItems,
                        ActiveEquationSelectionIndex = ActiveChild.SelectionStartIndex,
                        ActiveTextInRow = activeText,
                        CaretIndexOfActiveText = activeText.CaretIndex,
                        SelectedItemsOfTextEquation = activeText.SelectedItems,
                        SelectionStartIndexOfTextEquation = activeText.SelectionStartIndex,
                        TextEquationContents = activeText.Text,
                        TextEquationFormats = activeText.GetFormats(),
                        FirstLineOfInsertedText = lines[0],
                        Equations = newEquations
                    };
                    UndoManager.DisableAddingActions = true;
                    ActiveChild.ConsumeText(lines[0]);
                    action.FirstFormatsOfInsertedText = activeText.GetFormats();
                    EquationRow splitRow = (EquationRow)ActiveChild.Split(this);
                    if (!splitRow.IsEmpty)
                    {
                        childEquations.Add(splitRow);
                    }
                    int activeIndex = childEquations.IndexOf(ActiveChild);
                    int i = 1;
                    for (; i < lines.Count; i++)
                    {
                        EquationRow row = new EquationRow(this);
                        row.ConsumeText(lines[i]);
                        childEquations.Insert(activeIndex + i, row);
                        newEquations.Add(row);
                    }
                    UndoManager.DisableAddingActions = false;
                    newEquations.Add(splitRow);
                    ActiveChild = childEquations[activeIndex + lines.Count - 1];
                    ((TextEquation)((EquationRow)ActiveChild).ActiveChild).MoveToEnd();
                    SelectedItems = 0;
                    action.ActiveEquationAfterChange = ActiveChild;
                    UndoManager.AddUndoAction(action);
                }
                CalculateSize();
            }
            else
            {
                base.ConsumeText(text);
            }
        }

        public void DrawVisibleRows(DrawingContext dc, double top, double bottom)
        {
            if (IsSelecting)
            {
                try { DrawSelectionRegion(dc); }
                catch { }
            }
            foreach (EquationBase eb in childEquations)
            {
                if (eb.Bottom >= top)
                {
                    eb.DrawEquation(dc);
                }
                if (eb.Bottom >= bottom)
                {
                    break;
                }
            }
        }

        public override void DrawEquation(DrawingContext dc)
        {
            if (IsSelecting)
            {
                DrawSelectionRegion(dc);
            }
            base.DrawEquation(dc);
        }

        private void DrawSelectionRegion(DrawingContext dc)
        {
            int topSelectedRowIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
            EquationBase topEquation = childEquations[topSelectedRowIndex];
            Rect rect = topEquation.GetSelectionBounds();
            dc.DrawRectangle(Brushes.LightGray, null, rect);

            int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - topSelectedRowIndex;
            if (count > 0)
            {
                rect.Union(new Point(topEquation.Right, rect.Bottom + LineSpace + 1));
                dc.DrawRectangle(Brushes.LightGray, null, rect);
                EquationBase bottomEquation = childEquations[topSelectedRowIndex + count];
                rect = bottomEquation.GetSelectionBounds();
                rect.Union(new Point(bottomEquation.Left, bottomEquation.Top));
                dc.DrawRectangle(Brushes.LightGray, null, rect);
                for (int i = topSelectedRowIndex + 1; i < topSelectedRowIndex + count; i++)
                {
                    EquationBase equation = childEquations[i];
                    rect = equation.Bounds;
                    rect.Union(new Point(rect.Left, rect.Bottom + LineSpace + 1));
                    dc.DrawRectangle(Brushes.LightGray, null, rect);
                }
            }
        }

        public override void RemoveSelection(bool registerUndo)
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int endIndex = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex);
                EquationRow firstRow = (EquationRow)childEquations[startIndex];
                EquationRow lastRow = (EquationRow)childEquations[endIndex];
                TextEquation firstText = firstRow.GetFirstSelectionText();
                TextEquation lastText = lastRow.GetLastSelectionText();
                List<EquationBase> equations = new List<EquationBase>();
                RowContainerRemoveAction action = new RowContainerRemoveAction(this)
                {
                    ActiveEquation = ActiveChild,
                    HeadEquationRow = firstRow,
                    TailEquationRow = lastRow,
                    HeadTextEquation = firstText,
                    TailTextEquation = lastText,
                    SelectionStartIndex = SelectionStartIndex,
                    SelectedItems = SelectedItems,
                    FirstRowActiveIndex = firstRow.ActiveChildIndex,
                    FirstRowSelectionIndex = firstRow.SelectionStartIndex,
                    FirstRowSelectedItems = firstRow.SelectedItems,
                    LastRowActiveIndex = lastRow.ActiveChildIndex,
                    LastRowSelectionIndex = lastRow.SelectionStartIndex,
                    LastRowSelectedItems = lastRow.SelectedItems,
                    FirstTextCaretIndex = firstText.CaretIndex,
                    LastTextCaretIndex = lastText.CaretIndex,
                    FirstTextSelectionIndex = firstText.SelectionStartIndex,
                    LastTextSelectionIndex = lastText.SelectionStartIndex,
                    FirstTextSelectedItems = firstText.SelectedItems,
                    LastTextSelectedItems = lastText.SelectedItems,
                    FirstText = firstText.Text,
                    LastText = lastText.Text,
                    FirstFormats = firstText.GetFormats(),
                    LastFormats = lastText.GetFormats(),
                    FirstModes = firstText.GetModes(),
                    LastModes = lastText.GetModes(),
                    FirstRowDeletedContent = firstRow.DeleteTail(),
                    LastRowDeletedContent = lastRow.DeleteHead(), 
                    FirstDecorations = firstRow.GetFirstTextEquation().GetDecorations(),
                    LastDecorations = lastRow.GetLastTextEquation().GetDecorations(), 
                    Equations = equations,
                };
                action.FirstRowActiveIndexAfterRemoval = firstRow.ActiveChildIndex;
                firstText.RemoveSelection(false); //.DeleteSelectedText();
                lastText.RemoveSelection(false); //.DeleteSelectedText();                
                firstRow.Merge(lastRow);
                for (int i = endIndex; i > startIndex; i--)
                {
                    equations.Add(childEquations[i]);
                    childEquations.RemoveAt(i);
                }
                SelectedItems = 0;
                equations.Reverse();
                ActiveChild = firstRow;
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

        public override CopyDataObject Copy(bool removeSelection)
        {
            if (SelectedItems != 0)
            {
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int count = (SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex) - startIndex;
                EquationRow firstRow = (EquationRow)childEquations[startIndex];
                EquationRow lastRow = (EquationRow)childEquations[startIndex + count];
                List<EquationBase> firstRowSelectedItems = firstRow.GetSelectedEquations();
                List<EquationBase> lastRowSelectedItems = lastRow.GetSelectedEquations();

                EquationRow newFirstRow = new EquationRow(this);
                EquationRow newLastRow = new EquationRow(this);
                newFirstRow.GetFirstTextEquation().ConsumeFormattedText(firstRowSelectedItems.First().GetSelectedText(),
                                                                        ((TextEquation)firstRowSelectedItems.First()).GetSelectedFormats(),
                                                                        ((TextEquation)firstRowSelectedItems.First()).GetSelectedModes(),
                                                                        ((TextEquation)firstRowSelectedItems.First()).GetSelectedDecorations(), false);
                newLastRow.GetFirstTextEquation().ConsumeFormattedText(lastRowSelectedItems.Last().GetSelectedText(),
                                                                       ((TextEquation)lastRowSelectedItems.Last()).GetSelectedFormats(),
                                                                       ((TextEquation)lastRowSelectedItems.Last()).GetSelectedModes(),
                                                                       ((TextEquation)lastRowSelectedItems.First()).GetSelectedDecorations(),
                                                                       false);

                firstRowSelectedItems.RemoveAt(0);
                lastRowSelectedItems.RemoveAt(lastRowSelectedItems.Count - 1);
                newFirstRow.AddChildren(firstRowSelectedItems, false);
                newLastRow.AddChildren(lastRowSelectedItems, true);
                List<EquationBase> equations = new List<EquationBase>();
                for (int i = startIndex + 1; i < startIndex + count; i++)
                {
                    equations.Add(childEquations[i]);
                }
                equations.Add(newLastRow);
                foreach (EquationBase eb in equations)
                {
                    eb.Left = 1;
                }
                double left = firstRow.GetFirstSelectionText().Right - this.Left;
                Rect firstTextRect = firstRow.GetFirstSelectionText().GetSelectionBounds();
                if (!firstTextRect.IsEmpty)
                {
                    left = firstTextRect.Left - Left;
                }
                newFirstRow.Left = left + 1;
                equations.Insert(0, newFirstRow);
                double nextY = 1;
                double width = firstRow.Width;
                double height = 0;
                foreach (EquationBase eb in equations)
                {
                    eb.Top = nextY;
                    nextY += eb.Height + LineSpace;
                    width = eb.Width > width ? eb.Width : width;
                    height += eb.Height + LineSpace;
                }
                height -= LineSpace;
                RenderTargetBitmap bitmap = new RenderTargetBitmap((int)(Math.Ceiling(width + 2)), (int)(Math.Ceiling(height + 2)), 96, 96, PixelFormats.Default);
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
                XElement children = new XElement("ChildRows");
                foreach (EquationBase eb in equations)
                {
                    eb.SelectAll();
                    children.Add(eb.Serialize());
                }
                thisElement.Add(children);
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

        public override void SelectAll()
        {
            base.SelectAll();
            ((EquationRow)childEquations.Last()).MoveToEnd();
        }

        public override string GetSelectedText()
        {
            StringBuilder stringBulider = new StringBuilder("");
            foreach (EquationBase eb in childEquations)
            {
                stringBulider.Append(eb.GetSelectedText() + Environment.NewLine);
            }
            return stringBulider.ToString();
        }

        public override bool Select(Key key)
        {
            if (key == Key.Left)
            {
                if (ActiveChild.Select(key))
                {
                    return true;
                }
                else if (ActiveChild != childEquations.First())
                {
                    ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 1];
                    SelectedItems--;
                    if (SelectedItems < 0)
                    {
                        ((EquationRow)ActiveChild).MoveToEnd();
                        ActiveChild.StartSelection();
                    }
                    return true;
                }
            }
            else if (key == Key.Right)
            {
                if (ActiveChild.Select(key))
                {
                    return true;
                }
                else if (ActiveChild != childEquations.Last())
                {
                    ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) + 1];
                    SelectedItems++;
                    if (SelectedItems > 0)
                    {
                        ((EquationRow)ActiveChild).MoveToStart();
                        ActiveChild.StartSelection();
                    }
                    return true;
                }
            }
            else if (key == Key.Home)
            {
                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && SelectionStartIndex > 0)
                {
                    ((EquationRow)childEquations[SelectionStartIndex]).SelectToStart();
                    for (int i = SelectionStartIndex - 1; i >= 0; i--)
                    {
                        ((EquationRow)childEquations[i]).MoveToEnd();
                        childEquations[i].StartSelection();
                        ((EquationRow)childEquations[i]).SelectToStart();
                    }
                    SelectedItems = -SelectionStartIndex;
                    ActiveChild = childEquations.First();
                }
                else
                {
                    ((EquationRow)ActiveChild).SelectToStart();
                }
                return true;
            }
            else if (key == Key.End)
            {
                if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && SelectionStartIndex < childEquations.Count - 1)
                {
                    ((EquationRow)childEquations[SelectionStartIndex]).SelectToEnd();
                    for (int i = SelectionStartIndex + 1; i < childEquations.Count; i++)
                    {
                        ((EquationRow)childEquations[i]).MoveToStart();
                        childEquations[i].StartSelection();
                        ((EquationRow)childEquations[i]).SelectToEnd();
                    }
                    SelectedItems = childEquations.Count - SelectionStartIndex - 1;
                    ActiveChild = childEquations.Last();
                }
                else
                {
                    ((EquationRow)ActiveChild).SelectToEnd();
                }
                return true;
            }
            else if (key == Key.Up && SelectionStartIndex >= 0 && childEquations.IndexOf(ActiveChild) > 0)
            {
                Point point = childEquations[SelectionStartIndex].GetVerticalCaretLocation();
                ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 1];
                ((EquationRow)childEquations[SelectionStartIndex]).SelectToStart();
                for (int i = SelectionStartIndex - 1; i > childEquations.IndexOf(ActiveChild); i--)
                {
                    ((EquationRow)childEquations[i]).MoveToEnd();
                    childEquations[i].StartSelection();
                    ((EquationRow)childEquations[i]).SelectToStart();
                }
                point.Y = ActiveChild.MidY;
                ((EquationRow)ActiveChild).MoveToEnd();
                ActiveChild.StartSelection();
                ActiveChild.HandleMouseDrag(point);
                SelectedItems = childEquations.IndexOf(ActiveChild) - SelectionStartIndex;
                return true;
            }
            else if (key == Key.Down && SelectionStartIndex < childEquations.Count && childEquations.IndexOf(ActiveChild) < childEquations.Count - 1)
            {
                Point point = childEquations[SelectionStartIndex].GetVerticalCaretLocation();
                ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) + 1];
                ((EquationRow)childEquations[SelectionStartIndex]).SelectToEnd();
                for (int i = SelectionStartIndex + 1; i < childEquations.Count; i++)
                {
                    ((EquationRow)childEquations[i]).MoveToStart();
                    childEquations[i].StartSelection();
                    ((EquationRow)childEquations[i]).SelectToEnd();
                }
                point.Y = ActiveChild.MidY;
                ((EquationRow)ActiveChild).MoveToStart();
                ActiveChild.StartSelection();
                ActiveChild.HandleMouseDrag(point);
                SelectedItems = childEquations.IndexOf(ActiveChild) - SelectionStartIndex;
                return true;
            }
            return false;
        }

        public RowContainer(EquationContainer parent, double lineSpaceFactor = 0)
            : base(parent)
        {
            EquationRow newLine = new EquationRow(this);
            AddLine(newLine);
            Height = newLine.Height;
            Width = newLine.Width;
            this.lineSpaceFactor = lineSpaceFactor;
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement children = new XElement("ChildRows");
            foreach (EquationBase childRow in childEquations)
            {
                children.Add(childRow.Serialize());
            }
            thisElement.Add(children);
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            XElement children = xElement.Element("ChildRows");
            childEquations.Clear();
            foreach (XElement xe in children.Elements())
            {
                EquationRow row = new EquationRow(this);
                row.DeSerialize(xe);
                childEquations.Add(row);
            }
            if (childEquations.Count == 0)
            {
                childEquations.Add(new EquationRow(this));
            }
            ActiveChild = childEquations.First();
            CalculateSize();
        }

        void AddLine(EquationRow newRow)
        {
            int index = 0;
            if (childEquations.Count > 0)
            {
                index = childEquations.IndexOf((EquationRow)ActiveChild) + 1;
            }
            childEquations.Insert(index, newRow);
            ActiveChild = newRow;
            CalculateSize();
        }

        public override EquationBase Split(EquationContainer newParent)
        {
            EquationRow newRow = (EquationRow)ActiveChild.Split(this);
            if (newRow != null)
            {
                EquationRow activeRow = ActiveChild as EquationRow;
                var rca = new RowContainerAction(this, childEquations.IndexOf(activeRow), activeRow.ActiveChildIndex, activeRow.TextLength, newRow) { UndoFlag = false };                
                UndoManager.AddUndoAction(rca);
                AddLine(newRow);
            }
            CalculateSize();
            return newRow;
        }

        public override bool ConsumeMouseClick(Point mousePoint)
        {
            foreach (EquationBase eb in childEquations)
            {
                Rect rect = new Rect(0, eb.Top, double.MaxValue, eb.Height);
                if (rect.Contains(mousePoint))
                {
                    ActiveChild = eb;
                    return eb.ConsumeMouseClick(mousePoint);
                }
            }
            return false;
        }

        public override void HandleMouseDrag(Point mousePoint)
        {
            if (mousePoint.Y > ActiveChild.Top - LineSpace && mousePoint.Y < ActiveChild.Bottom + LineSpace)
            {
                ActiveChild.HandleMouseDrag(mousePoint);
            }
            else
            {
                if (mousePoint.Y > ActiveChild.Bottom + LineSpace)
                {
                    for (int i = childEquations.IndexOf(ActiveChild) + 1; i < childEquations.Count; i++)
                    {
                        ActiveChild = childEquations[i];
                        if (ActiveChild.Top <= mousePoint.Y && ActiveChild.Bottom + LineSpace >= mousePoint.Y)
                        {
                            break;
                        }
                    }
                    for (int i = SelectionStartIndex + 1; i < childEquations.IndexOf(ActiveChild); i++)
                    {
                        ((EquationRow)childEquations[i]).MoveToStart();
                        childEquations[i].StartSelection();
                        ((EquationRow)childEquations[i]).SelectToEnd();
                    }
                    if (childEquations.IndexOf(ActiveChild) > SelectionStartIndex)
                    {
                        ((EquationRow)childEquations[SelectionStartIndex]).SelectToEnd();
                        EquationRow row = ActiveChild as EquationRow;
                        row.MoveToStart();
                        row.StartSelection();
                    }

                }
                else if (mousePoint.Y < ActiveChild.Top - LineSpace)
                {
                    for (int i = childEquations.IndexOf(ActiveChild) - 1; i >= 0; i--)
                    {
                        ActiveChild = childEquations[i];
                        if (ActiveChild.Top - LineSpace <= mousePoint.Y && ActiveChild.Bottom >= mousePoint.Y)
                        {
                            break;
                        }
                    }
                    for (int i = SelectionStartIndex - 1; i > childEquations.IndexOf(ActiveChild); i--)
                    {
                        ((EquationRow)childEquations[i]).MoveToEnd();
                        childEquations[i].StartSelection();
                        ((EquationRow)childEquations[i]).SelectToStart();
                    }
                    if (childEquations.IndexOf(ActiveChild) < SelectionStartIndex)
                    {
                        ((EquationRow)childEquations[SelectionStartIndex]).SelectToStart();
                        EquationRow row = ActiveChild as EquationRow;
                        row.MoveToEnd();
                        row.StartSelection();
                    }
                }
                ActiveChild.HandleMouseDrag(mousePoint);
                SelectedItems = childEquations.IndexOf(ActiveChild) - SelectionStartIndex;
            }
            StatusBarHelper.PrintStatusMessage("ActiveStart " + ActiveChild.SelectionStartIndex + ", ActiveItems" + ActiveChild.SelectedItems);
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                if (HAlignment == Editor.HAlignment.Left)
                {
                    foreach (EquationBase eb in childEquations)
                    {
                        eb.Left = value;
                    }
                }
                else if (HAlignment == Editor.HAlignment.Right)
                {
                    foreach (EquationBase eb in childEquations)
                    {
                        eb.Right = Right;
                    }
                }
                else if (HAlignment == Editor.HAlignment.Center)
                {
                    foreach (EquationBase eb in childEquations)
                    {
                        eb.MidX = MidX;
                    }
                }
            }
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                double nextY = value;
                foreach (EquationBase eb in childEquations)
                {
                    eb.Top = nextY;
                    nextY += eb.Height + LineSpace;
                }
            }
        }

        public override bool ConsumeKey(Key key)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (key == Key.Home)
                {
                    ActiveChild = childEquations.First();
                    ((EquationRow)ActiveChild).MoveToStart();
                }
                else if (key == Key.End)
                {
                    ActiveChild = childEquations.Last();
                    ((EquationRow)ActiveChild).MoveToEnd();
                }
                return true;
            }
            bool result = false;
            if (ActiveChild.ConsumeKey(key))
            {
                result = true;
            }
            else if (key == Key.Enter)
            {
                Split(this);
                ((EquationRow)ActiveChild).MoveToStart();
                result = true;
            }
            else if (key == Key.Delete)
            {
                if (ActiveChild != childEquations.Last())
                {
                    EquationRow activeRow = ActiveChild as EquationRow;
                    EquationRow rowToRemove = (EquationRow)childEquations[childEquations.IndexOf(activeRow) + 1];
                    UndoManager.AddUndoAction(new RowContainerAction(this, childEquations.IndexOf(activeRow), activeRow.ActiveChildIndex, activeRow.TextLength, rowToRemove));
                    activeRow.Merge(rowToRemove);
                    childEquations.RemoveAt(childEquations.IndexOf(activeRow) + 1);
                }
                result = true;
            }
            else if (!result)
            {
                if (key == Key.Up && ActiveChild != childEquations.First())
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) - 1];
                    point.Y = ActiveChild.Bottom - 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    result = true;
                }
                else if (key == Key.Down && ActiveChild != childEquations.Last())
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = childEquations[childEquations.IndexOf(ActiveChild) + 1];
                    point.Y = ActiveChild.Top + 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    result = true;
                }
                else if (key == Key.Left)
                {
                    if (ActiveChild != childEquations.First())
                    {
                        ActiveChild = childEquations[childEquations.IndexOf((EquationRow)ActiveChild) - 1];
                        result = true;
                    }
                }
                else if (key == Key.Right)
                {
                    if (ActiveChild != childEquations.Last())
                    {
                        ActiveChild = childEquations[childEquations.IndexOf((EquationRow)ActiveChild) + 1];
                        result = true;
                    }
                }
                else if (key == Key.Back)
                {
                    if (ActiveChild != childEquations.First())
                    {
                        EquationRow activeRow = ActiveChild as EquationRow;
                        EquationRow previousRow = (EquationRow)childEquations[childEquations.IndexOf(activeRow) - 1];
                        int index = previousRow.ActiveChildIndex;
                        previousRow.MoveToEnd();
                        UndoManager.AddUndoAction(new RowContainerAction(this, childEquations.IndexOf(previousRow), previousRow.ActiveChildIndex, previousRow.TextLength, activeRow));
                        previousRow.Merge(activeRow);
                        childEquations.Remove(activeRow);
                        ActiveChild = previousRow;
                        result = true;
                    }
                }
            }
            CalculateSize();
            return result;
        }

        protected override void CalculateWidth()
        {
            double maxLeftHalf = 0;
            double maxRightHalf = 0;
            foreach (EquationBase eb in childEquations)
            {
                if (eb.RefX > maxLeftHalf)
                {
                    maxLeftHalf = eb.RefX;
                }
                if (eb.Width - eb.RefX > maxRightHalf)
                {
                    maxRightHalf = eb.Width - eb.RefX;
                }
                eb.Left = Left;
            }
            Width = maxLeftHalf + maxRightHalf;
        }

        protected override void CalculateHeight()
        {
            double height = 0;
            foreach (EquationBase eb in childEquations)
            {
                height += eb.Height + LineSpace;
            }
            Height = height;
            double nextY = Top;
            foreach (EquationBase eb in childEquations)
            {
                eb.Top = nextY;
                nextY += eb.Height + LineSpace;
            }
        }

        public override double RefY
        {
            get
            {
                int count = childEquations.Count;
                if (count == 1)
                {
                    return childEquations.First().RefY;
                }
                else if (count % 2 == 0)
                {
                    return childEquations[(count + 1) / 2].Top - Top - LineSpace / 2;
                }
                else
                {
                    return childEquations[count / 2].MidY - Top;
                    //base.RefY;
                }
            }
        }

        public void ProcessUndo(EquationAction action)
        {
            Type type = action.GetType();
            if (type == typeof(RowContainerAction))
            {
                ProcessRowContainerAction(action);
                IsSelecting = false;
            }
            else if (type == typeof(RowContainerTextAction))
            {
                ProcessRowContainerTextAction(action);
            }
            else if (type == typeof(RowContainerPasteAction))
            {
                ProcessRowContainerPasteAction(action);
            }
            else if (type == typeof(RowContainerFormatAction))
            {
                ProcessRowContainerFormatAction(action);
            }
            else if (type == typeof(RowContainerRemoveAction))
            {
                ProcessRowContainerRemoveAction(action);
            }
            CalculateSize();
            ParentEquation.ChildCompletedUndo(this);
        }

        private void ProcessRowContainerPasteAction(EquationAction action)
        {
            RowContainerPasteAction pasteAction = action as RowContainerPasteAction;
            EquationRow activeRow = (EquationRow)pasteAction.ActiveEquation;
            if (pasteAction.UndoFlag)
            {
                SelectedItems = pasteAction.SelectedItems;
                SelectionStartIndex = pasteAction.SelectionStartIndex;
                pasteAction.ActiveTextInChildRow.ResetTextEquation(pasteAction.CaretIndexOfActiveText, pasteAction.SelectionStartIndexOfTextEquation,
                                                                   pasteAction.SelectedItemsOfTextEquation, pasteAction.TextEquationContents,
                                                                   pasteAction.TextEquationFormats, pasteAction.TextEquationModes,
                                                                   pasteAction.TextEquationDecorations);
                activeRow.ResetRowEquation(pasteAction.ActiveTextInChildRow, pasteAction.ActiveEquationSelectionIndex, pasteAction.ActiveEquationSelectedItems);
                activeRow.Truncate();
                activeRow.Merge(pasteAction.Equations.Last());
                foreach (EquationBase eb in pasteAction.Equations)
                {
                    childEquations.Remove(eb);
                }
                activeRow.CalculateSize();
                ActiveChild = activeRow;
            }
            else
            {
                activeRow.ResetRowEquation(pasteAction.ActiveTextInChildRow, pasteAction.ActiveEquationSelectionIndex, pasteAction.ActiveEquationSelectedItems);
                EquationRow newRow = (EquationRow)activeRow.Split(this);
                pasteAction.Equations[pasteAction.Equations.Count - 2].GetLastTextEquation().SetFormattedText(pasteAction.TailTextOfPastedRows, pasteAction.TailFormatsOfPastedRows, pasteAction.TailModesOfPastedRows);
                activeRow.Merge(pasteAction.Equations.First());
                int index = childEquations.IndexOf(ActiveChild) + 1;
                childEquations.InsertRange(index, pasteAction.Equations.Skip(1));
                childEquations.RemoveAt(childEquations.Count - 1);
                pasteAction.Equations[pasteAction.Equations.Count - 2].Merge(newRow);
                ActiveChild = childEquations[index + pasteAction.Equations.Count - 3];
                ((EquationRow)ActiveChild).MoveToEnd();
                FontSize = FontSize;
                SelectedItems = 0;
            }
        }

        private void ProcessRowContainerTextAction(EquationAction action)
        {
            RowContainerTextAction textAction = action as RowContainerTextAction;
            ActiveChild = textAction.ActiveEquation;
            EquationRow activeRow = (EquationRow)ActiveChild;
            activeRow.ResetRowEquation(textAction.ActiveTextInRow, textAction.ActiveEquationSelectionIndex, textAction.ActiveEquationSelectedItems);
            if (textAction.UndoFlag)
            {
                textAction.ActiveTextInRow.ResetTextEquation(textAction.CaretIndexOfActiveText, textAction.SelectionStartIndexOfTextEquation, textAction.SelectedItemsOfTextEquation, textAction.TextEquationContents, textAction.TextEquationFormats, textAction.TextEquationModes, textAction.TextEquationDecoration);
                UndoManager.DisableAddingActions = true;
                ActiveChild.ConsumeFormattedText(textAction.FirstLineOfInsertedText, textAction.FirstFormatsOfInsertedText, textAction.FirstModesOfInsertedText, textAction.FirstDecorationsOfInsertedText, false);
                UndoManager.DisableAddingActions = false;
                EquationRow splitRow = (EquationRow)ActiveChild.Split(this);
                childEquations.InsertRange(childEquations.IndexOf(ActiveChild) + 1, textAction.Equations);
                if (splitRow.IsEmpty)
                {
                    childEquations.Remove(textAction.Equations.Last());
                }
                ActiveChild = textAction.ActiveEquationAfterChange;
                textAction.ActiveTextInRow.MoveToEnd();
                SelectedItems = 0; 
            }
            else
            {
                this.SelectedItems = textAction.SelectedItems;
                this.SelectionStartIndex = textAction.SelectionStartIndex;
                activeRow.Merge((EquationRow)textAction.Equations.Last());
                textAction.ActiveTextInRow.ResetTextEquation(textAction.CaretIndexOfActiveText, textAction.SelectionStartIndexOfTextEquation, 
                                                             textAction.SelectedItemsOfTextEquation, textAction.TextEquationContents, 
                                                             textAction.TextEquationFormats, textAction.FirstModesOfInsertedText,
                                                             textAction.FirstDecorationsOfInsertedText);
                foreach (EquationBase eb in textAction.Equations)
                {
                    if (childEquations.Contains(eb))
                    {
                        childEquations.Remove(eb);
                    }
                }
            }
            activeRow.CalculateSize();
        }

        private void ProcessRowContainerRemoveAction(EquationAction action)
        {
            RowContainerRemoveAction rowAction = action as RowContainerRemoveAction;
            if (rowAction.UndoFlag)
            {
                childEquations.InsertRange(childEquations.IndexOf(rowAction.HeadEquationRow) + 1, rowAction.Equations);
                rowAction.HeadEquationRow.ActiveChildIndex = rowAction.FirstRowActiveIndexAfterRemoval;
                rowAction.HeadEquationRow.Truncate();
                rowAction.HeadEquationRow.ResetRowEquation(rowAction.FirstRowActiveIndex, rowAction.FirstRowSelectionIndex, rowAction.FirstRowSelectedItems, rowAction.FirstRowDeletedContent, true);
                rowAction.TailEquationRow.ResetRowEquation(rowAction.LastRowActiveIndex, rowAction.LastRowSelectionIndex, rowAction.LastRowSelectedItems, rowAction.LastRowDeletedContent, false);
                rowAction.HeadTextEquation.ResetTextEquation(rowAction.FirstTextCaretIndex, rowAction.FirstTextSelectionIndex, rowAction.FirstTextSelectedItems, rowAction.FirstText, rowAction.FirstFormats, rowAction.FirstModes, rowAction.FirstDecorations);
                rowAction.TailTextEquation.ResetTextEquation(rowAction.LastTextCaretIndex, rowAction.LastTextSelectionIndex, rowAction.LastTextSelectedItems, rowAction.LastText, rowAction.LastFormats, rowAction.LastModes, rowAction.LastDecorations);
                foreach (EquationBase eb in rowAction.Equations)
                {
                    eb.FontSize = FontSize;
                }
                rowAction.HeadEquationRow.FontSize = FontSize;
                rowAction.TailEquationRow.FontSize = FontSize;
                SelectedItems = rowAction.SelectedItems;
                SelectionStartIndex = rowAction.SelectionStartIndex;
                ActiveChild = rowAction.ActiveEquation;
                IsSelecting = true;
            }
            else
            {
                rowAction.HeadEquationRow.ResetRowEquation(rowAction.FirstRowActiveIndex, rowAction.FirstRowSelectionIndex, rowAction.FirstRowSelectedItems);
                rowAction.TailEquationRow.ResetRowEquation(rowAction.LastRowActiveIndex, rowAction.LastRowSelectionIndex, rowAction.LastRowSelectedItems);
                rowAction.HeadTextEquation.ResetTextEquation(rowAction.FirstTextCaretIndex, rowAction.FirstTextSelectionIndex, rowAction.FirstTextSelectedItems, rowAction.FirstText, rowAction.FirstFormats, rowAction.FirstModes, rowAction.FirstDecorations);
                rowAction.TailTextEquation.ResetTextEquation(rowAction.LastTextCaretIndex, rowAction.LastTextSelectionIndex, rowAction.LastTextSelectedItems, rowAction.LastText, rowAction.LastFormats, rowAction.LastModes, rowAction.LastDecorations);
                rowAction.HeadTextEquation.RemoveSelection(false); //.DeleteSelectedText();
                rowAction.TailTextEquation.RemoveSelection(false); //.DeleteSelectedText();
                rowAction.HeadEquationRow.DeleteTail();
                rowAction.TailEquationRow.DeleteHead();
                rowAction.HeadEquationRow.Merge(rowAction.TailEquationRow);
                int index = childEquations.IndexOf(rowAction.HeadEquationRow);
                for (int i = index + rowAction.Equations.Count; i > index; i--)
                {
                    childEquations.RemoveAt(i);
                }
                ActiveChild = rowAction.HeadEquationRow;
                SelectedItems = 0;
                IsSelecting = false;
            }
        }

        private void ProcessRowContainerAction(EquationAction action)
        {
            RowContainerAction containerAction = action as RowContainerAction;
            if (containerAction.UndoFlag)
            {
                EquationRow activeRow = (EquationRow)childEquations[containerAction.Index];
                activeRow.SetCurrentChild(containerAction.ChildIndexInRow, containerAction.CaretIndex);
                activeRow.Truncate(containerAction.ChildIndexInRow + 1, containerAction.CaretIndex);
                childEquations.Insert(containerAction.Index + 1, containerAction.Equation);
                ActiveChild = containerAction.Equation;
                ActiveChild.FontSize = this.FontSize;
            }
            else
            {
                ((EquationRow)childEquations[containerAction.Index]).Merge((EquationRow)childEquations[containerAction.Index + 1]);
                childEquations.Remove(childEquations[containerAction.Index + 1]);
                ActiveChild = childEquations[containerAction.Index];
                ((EquationRow)ActiveChild).SetCurrentChild(containerAction.ChildIndexInRow, containerAction.CaretIndex);
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
                        RowContainerFormatAction ecfa = new RowContainerFormatAction(this)
                        {
                            Operation = operation,
                            Argument = argument,
                            Applied = applied,
                            SelectionStartIndex = SelectionStartIndex,
                            SelectedItems = SelectedItems,
                            ActiveChild = ActiveChild,
                            FirstRowActiveChildIndex = ((EquationRow)childEquations[startIndex]).ActiveChildIndex,
                            FirstRowSelectionStartIndex = childEquations[startIndex].SelectionStartIndex,
                            FirstRowSelectedItems = childEquations[startIndex].SelectedItems,
                            LastRowActiveChildIndex = ((EquationRow)childEquations[endIndex]).ActiveChildIndex,
                            LastRowSelectionStartIndex = childEquations[endIndex].SelectionStartIndex,
                            LastRowSelectedItems = childEquations[endIndex].SelectedItems,
                            FirstTextCaretIndex = ((EquationRow)childEquations[startIndex]).GetFirstSelectionText().CaretIndex,
                            FirstTextSelectionStartIndex = ((EquationRow)childEquations[startIndex]).GetFirstSelectionText().SelectionStartIndex,
                            FirstTextSelectedItems = ((EquationRow)childEquations[startIndex]).GetFirstSelectionText().SelectedItems,
                            LastTextCaretIndex = ((EquationRow)childEquations[endIndex]).GetLastSelectionText().CaretIndex,
                            LastTextSelectionStartIndex = ((EquationRow)childEquations[endIndex]).GetLastSelectionText().SelectionStartIndex,
                            LastTextSelectedItems = ((EquationRow)childEquations[endIndex]).GetLastSelectionText().SelectedItems,
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

        private void ProcessRowContainerFormatAction(EquationAction action)
        {
            RowContainerFormatAction rcfa = action as RowContainerFormatAction;
            if (rcfa != null)
            {
                IsSelecting = true;
                ActiveChild = rcfa.ActiveChild;
                this.SelectedItems = rcfa.SelectedItems;
                this.SelectionStartIndex = rcfa.SelectionStartIndex;
                int startIndex = SelectedItems > 0 ? SelectionStartIndex : SelectionStartIndex + SelectedItems;
                int endIndex = SelectedItems > 0 ? SelectionStartIndex + SelectedItems : SelectionStartIndex;
                ((EquationRow)childEquations[startIndex]).ActiveChildIndex = rcfa.FirstRowActiveChildIndex;
                childEquations[startIndex].SelectionStartIndex = rcfa.FirstRowSelectionStartIndex;
                childEquations[startIndex].SelectedItems = rcfa.FirstRowSelectedItems;
                ((EquationRow)childEquations[endIndex]).ActiveChildIndex = rcfa.LastRowActiveChildIndex;
                childEquations[endIndex].SelectionStartIndex = rcfa.LastRowSelectionStartIndex;
                childEquations[endIndex].SelectedItems = rcfa.LastRowSelectedItems;
                ((EquationRow)childEquations[startIndex]).GetFirstTextEquation().CaretIndex = rcfa.FirstTextCaretIndex;
                ((EquationRow)childEquations[startIndex]).GetFirstTextEquation().SelectionStartIndex = rcfa.FirstTextSelectionStartIndex;
                ((EquationRow)childEquations[startIndex]).GetFirstTextEquation().SelectedItems = rcfa.FirstTextSelectedItems;
                ((EquationRow)childEquations[endIndex]).GetLastTextEquation().CaretIndex = rcfa.LastTextCaretIndex;
                ((EquationRow)childEquations[endIndex]).GetLastTextEquation().SelectionStartIndex = rcfa.LastTextSelectionStartIndex;
                ((EquationRow)childEquations[endIndex]).GetLastTextEquation().SelectedItems = rcfa.LastTextSelectedItems;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (i > startIndex && i < endIndex)
                    {
                        childEquations[i].SelectAll();
                    }
                    childEquations[i].ModifySelection(rcfa.Operation, rcfa.Argument, rcfa.UndoFlag ? !rcfa.Applied : rcfa.Applied, false);
                }
                CalculateSize();
                ParentEquation.ChildCompletedUndo(this);
            }
        }
    }
}
