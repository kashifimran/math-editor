using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class RowAction : EquationAction
    {
        public int Index { get; set; }
        public int CaretIndex { get; set; }
        public EquationBase Equation { get; set; }
        public TextEquation EquationAfter { get; set; }
        public bool Added { get; set; }

        public RowAction(ISupportsUndo executor, EquationBase equation, TextEquation equationAfter, int index, int caretIndex, bool added)
            : base(executor)
        {
            Index = index;
            Equation = equation;
            CaretIndex = caretIndex;
            EquationAfter = equationAfter;
            Added = added;
        }
    }
}

