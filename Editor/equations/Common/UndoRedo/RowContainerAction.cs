using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{   
    public class RowContainerAction : EquationAction
    {
        public int Index { get; set; }
        public int ChildIndexInRow { get; set; }
        public int CaretIndex { get; set; }
        public EquationRow Equation { get; set; }

        public RowContainerAction(ISupportsUndo executor, int index, int childIndexInRow, int caretIndex, EquationRow equation)
            : base(executor)
        {
            Index = index;
            ChildIndexInRow = childIndexInRow;
            CaretIndex = caretIndex;
            Equation = equation;
        }
    }    
}

