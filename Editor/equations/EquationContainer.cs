using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace Editor
{
    public abstract class EquationContainer : EquationBase
    {
        public int GetIndex(EquationBase child)
        {
            return childEquations.IndexOf(child);
        }

        public void ReleaseChild(EquationBase child)
        {
            if (childEquations.Contains(child))
            {
                childEquations.Remove(child);
            }
        }

        protected List<EquationBase> childEquations = new List<EquationBase>();
        EquationBase active;
        public EquationBase ActiveChild
        {
            get { return active; }
            set
            {
                if (value == null || !value.IsStatic)
                {
                    active = value;
                }
                else
                {
                }
            }
        }

        public EquationContainer(EquationContainer parent) : base(parent) { }

        public virtual void ExecuteCommand(CommandType commandType, object data)
        {
            if (ActiveChild is EquationContainer)
            {
                ((EquationContainer)ActiveChild).ExecuteCommand(commandType, data);
            }
            CalculateSize();
        }

        public override string GetSelectedText()
        {
            StringBuilder stringBulider = new StringBuilder("");
            foreach (EquationBase eb in childEquations)
            {
                stringBulider.Append(eb.GetSelectedText());
            }
            return stringBulider.ToString();
        }

        public override void StartSelection()
        {
            SelectedItems = 0; //=base.StartSelection();
            SelectionStartIndex = childEquations.IndexOf(ActiveChild);
            ActiveChild.StartSelection();
        }

        public override Rect GetSelectionBounds()
        {
            return ActiveChild.GetSelectionBounds();
        }

        public override void DrawEquation(DrawingContext dc)
        {
            base.DrawEquation(dc);
            foreach (EquationBase eb in childEquations)
            {
                eb.DrawEquation(dc);
            }
        }

        public override void RemoveSelection(bool registerUndo)
        {
            ActiveChild.RemoveSelection(registerUndo);
            CalculateSize();
        }

        public override CopyDataObject Copy(bool removeSelection)
        {
            CopyDataObject temp = ActiveChild.Copy(removeSelection);
            if (removeSelection)
            {
                CalculateSize();
            }
            return temp;
        }

        public override void Paste(XElement xe)
        {
            ActiveChild.Paste(xe);
            CalculateSize();
        }

        public override bool Select(Key key)
        {
            return ActiveChild.Select(key);
        }

        public override void DeSelect()
        {
            SelectedItems = 0; //base.Deselect()
            foreach (EquationBase eb in childEquations)
            {
                eb.DeSelect();
            }
        }

        public virtual void ChildCompletedUndo(EquationBase child)
        {
            ActiveChild = child;
            CalculateSize();
            if (ParentEquation != null)
            {
                ParentEquation.ChildCompletedUndo(this);
            }
        }

        public override void ConsumeText(string text)
        {
            ActiveChild.ConsumeText(text);
            CalculateSize();
        }

        public override void ConsumeFormattedText(string text, int[] formats, EditorMode[] modes, CharacterDecorationInfo[] decorations, bool addUndo)
        {
            ActiveChild.ConsumeFormattedText(text, formats, modes, decorations, addUndo);
            CalculateSize();
        }

        public override bool ConsumeKey(Key key)
        {
            bool temp = ActiveChild.ConsumeKey(key);
            CalculateSize();
            return temp;
        }

        public override Point GetVerticalCaretLocation()
        {
            return ActiveChild.GetVerticalCaretLocation();
        }

        public override double GetVerticalCaretLength()
        {
            return ActiveChild.GetVerticalCaretLength();
        }

        public virtual EquationContainer GetInnerMostEquationContainer()
        {
            if (ActiveChild is EquationContainer)
            {
                return ((EquationContainer)ActiveChild).GetInnerMostEquationContainer();
            }
            else
            {
                return this;
            }
        }

        public Point GetHorizontalCaretLocation()
        {
            if (ActiveChild is EquationContainer)
            {
                return ((EquationContainer)ActiveChild).GetHorizontalCaretLocation();
            }
            else
            {
                return new Point(this.Left, this.Bottom);
            }
        }

        public double GetHorizontalCaretLength()
        {
            if (ActiveChild is EquationContainer)
            {
                return ((EquationContainer)ActiveChild).GetHorizontalCaretLength();
            }
            else
            {
                return Width;
            }
        }

        public override EquationBase Split(EquationContainer newParent)
        {
            EquationBase result = ActiveChild.Split(this);
            CalculateSize();
            return result;
        }

        public override bool ConsumeMouseClick(Point mousePoint)
        {
            foreach (EquationBase eb in childEquations)
            {
                if (!eb.IsStatic && eb.Bounds.Contains(mousePoint))
                {
                    ActiveChild = eb;
                    return ActiveChild.ConsumeMouseClick(mousePoint);
                }
            }
            return false;
        }

        public override void SetCursorOnKeyUpDown(Key key, Point point)
        {
            if (key == Key.Up)
            {
                for (int i = childEquations.Count - 1; i >= 0; i--)
                {
                    Type type = childEquations[i].GetType();
                    if (type == typeof(RowContainer) || type == typeof(EquationRow))
                    {
                        childEquations[i].SetCursorOnKeyUpDown(key, point);
                        ActiveChild = childEquations[i];
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < childEquations.Count; i++)
                {
                    Type type = childEquations[i].GetType();
                    if (type == typeof(RowContainer) || type == typeof(EquationRow))
                    {
                        childEquations[i].SetCursorOnKeyUpDown(key, point);
                        ActiveChild = childEquations[i];
                        break;
                    }
                }
            }
        }

        public override void HandleMouseDrag(Point mousePoint)
        {
            ActiveChild.HandleMouseDrag(mousePoint);
        }

        public override double FontSize
        {
            get { return base.FontSize; }
            set
            {
                base.FontSize = value;
                foreach (EquationBase eb in childEquations)
                {
                    eb.FontSize = FontSize;
                }
                CalculateSize();
            }
        }

        public override void SelectAll()
        {
            SelectionStartIndex = 0;
            SelectedItems = childEquations.Count - 1;
            ActiveChild = childEquations.Last();
            foreach (var child in childEquations)
            {
                child.SelectAll();
            }
        }

        public override void ModifySelection(string operation, string argument, bool applied, bool addUndo)
        {
            foreach (EquationBase eb in childEquations)
            {
                eb.ModifySelection(operation, argument, applied, addUndo);
            }
            CalculateSize();
        }

        public override HashSet<int> GetUsedTextFormats()
        {
            HashSet<int> list = new HashSet<int>();
            foreach (EquationBase eb in childEquations)
            {
                var listFormats = eb.GetUsedTextFormats();
                if (listFormats != null) //This check is necessary as the base returns 'null'
                {
                    foreach (int i in listFormats)
                    {
                        if (!list.Contains(i))
                        {
                            list.Add(i);
                        }
                    }
                }
            }
            return list;
        }

        public override void ResetTextFormats(Dictionary<int, int> formatMapping)
        {
            foreach (EquationBase eb in childEquations)
            {
                eb.ResetTextFormats(formatMapping);
            }
        }
        public override bool ApplySymbolGap
        {
            get
            {
                return base.ApplySymbolGap;
            }
            set
            {
                base.ApplySymbolGap = value;
                foreach (EquationBase eb in childEquations)
                {
                    eb.ApplySymbolGap = value;
                }
            }
        }
    }
}
