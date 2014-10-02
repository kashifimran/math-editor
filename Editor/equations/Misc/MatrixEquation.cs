using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Input;
using System.Windows;

namespace Editor
{
    public class MatrixEquation : EquationContainer
    {
        int columns = 1;
        int rows = 1;
        double CellSpace { get { return FontSize * .7; } }

        public override Thickness Margin 
        {
            get { return new Thickness(FontSize * .15, 0, FontSize * .15, 0); }
        }

        public MatrixEquation(EquationContainer parent, int rows, int columns)
            : base(parent)
        {
            this.rows = rows;
            this.columns = columns;
            for (int i = 0; i < columns * rows; i++)
            {
                childEquations.Add(new RowContainer(this));
            }
            ActiveChild = childEquations.First();
        }

        public override XElement Serialize()
        {
            XElement thisElement = new XElement(GetType().Name);
            XElement parameters = new XElement("parameters");
            parameters.Add(new XElement(typeof(int).FullName, rows));
            parameters.Add(new XElement(typeof(int).FullName, columns));
            thisElement.Add(parameters);
            foreach (EquationBase eb in childEquations)
            {
                thisElement.Add(eb.Serialize());
            }
            return thisElement;
        }

        public override void DeSerialize(XElement xElement)
        {
            XElement[] elements = xElement.Elements(typeof(RowContainer).Name).ToArray();
            for (int i = 0; i < childEquations.Count; i++)
            {
                childEquations[i].DeSerialize(elements[i]);
            }
            CalculateSize();
        }

        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                double[] rowRefYs = new double[rows];
                double[] topOffsets = new double[rows + 1];

                for (int i = 0; i < rows; i++)
                {
                    rowRefYs[i] = childEquations.Skip(i * columns).Take(columns).Max(x => x.RefY);
                    topOffsets[i + 1] = childEquations.Skip(i * columns).Take(columns).Max(x => x.Height) + topOffsets[i];
                }

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        childEquations[i * columns + j].MidY = Top + rowRefYs[i] + topOffsets[i] + CellSpace * i;
                    }
                }
            }
        }

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                double[] columnRefXs = new double[columns];
                double[] leftOffsets = new double[columns + 1];
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        columnRefXs[i] = Math.Max(childEquations[j * columns + i].RefX, columnRefXs[i]);
                        leftOffsets[i + 1] = Math.Max(childEquations[j * columns + i].Width, leftOffsets[i + 1]);
                    }
                    leftOffsets[i + 1] += leftOffsets[i];
                }
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        childEquations[j * columns + i].MidX = value + columnRefXs[i] + leftOffsets[i] + CellSpace * i;
                    }
                }
            }
        }

        protected override void CalculateWidth()
        {
            double[] columnWidths = new double[columns];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    columnWidths[i] = Math.Max(childEquations[j * columns + i].Width, columnWidths[i]);
                }
            }
            Width = columnWidths.Sum() + CellSpace * (columns - 1);
        }

        protected override void CalculateHeight()
        {
            double[] rowHeights = new double[rows];
            for (int i = 0; i < rows; i++)
            {
                rowHeights[i] = childEquations.Skip(i * columns).Take(columns).Max(x => x.Height);
            }
            Height = rowHeights.Sum() + CellSpace * (rows - 1);
        }

        public override double RefY
        {
            get
            {
                if (rows == 1)
                {
                    return childEquations.Max(x => x.RefY);
                }
                else if (rows % 2 == 0)
                {
                    //return childEquations.Take(rows / 2 * columns).Sum(x => x.Height) - CellSpace / 2 + FontSize * .3;
                    double[] rowHeights = new double[rows / 2];
                    for (int i = 0; i < rows / 2; i++)
                    {
                        rowHeights[i] = childEquations.Skip(i * columns).Take(columns).Max(x => x.Height);
                    }
                    return rowHeights.Sum() + CellSpace * rows/2 - CellSpace/2 + FontSize * .1;
                }
                else
                {
                    //return childEquations.Skip(rows / 2 * columns).Take(columns).Max(x => x.MidY) - Top;
                    double[] rowHeights = new double[rows / 2 + 1];
                    for (int i = 0; i < rows / 2; i++)
                    {
                        rowHeights[i] = childEquations.Skip(i * columns).Take(columns).Max(x => x.Height);
                    }
                    rowHeights[rows / 2] = childEquations.Skip(rows / 2 * columns).Take(columns).Max(x => x.RefY);
                    return rowHeights.Sum() + CellSpace * (rows / 2);// -FontSize * .1;
                }
            }
        }

        public override bool ConsumeKey(Key key)
        {
            if (ActiveChild.ConsumeKey(key))
            {
                CalculateSize();
                return true;
            }
            int currentIndex = childEquations.IndexOf(ActiveChild);
            if (key == Key.Right)
            {
                if (currentIndex % columns < columns - 1)//not last column?
                {
                    ActiveChild = childEquations[currentIndex + 1];
                    return true;
                }
            }
            else if (key == Key.Left)
            {
                if (currentIndex % columns > 0)//not last column?
                {
                    ActiveChild = childEquations[currentIndex - 1];
                    return true;
                }
            }
            else if (key == Key.Up)
            {
                if (currentIndex / columns > 0)//not in first row?
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = childEquations[currentIndex - columns]; ;
                    point.Y = ActiveChild.Top + 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
            }
            else if (key == Key.Down)
            {
                if (currentIndex / columns < rows - 1)//not in last row?
                {
                    Point point = ActiveChild.GetVerticalCaretLocation();
                    ActiveChild = childEquations[currentIndex + columns]; ;
                    point.Y = ActiveChild.Top + 1;
                    ActiveChild.SetCursorOnKeyUpDown(key, point);
                    return true;
                }
            }
            return false;
        }
    }
}
