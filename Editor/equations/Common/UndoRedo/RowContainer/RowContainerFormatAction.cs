using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class RowContainerFormatAction : EquationAction
    {
        public EquationBase ActiveChild { get; set; }
        public int SelectionStartIndex { get; set; }
        public int SelectedItems { get; set; }
        
        public int FirstRowActiveChildIndex { get; set; }
        public int FirstRowSelectionStartIndex { get; set; }
        public int FirstRowSelectedItems { get; set; }
        
        public int LastRowActiveChildIndex { get; set; }
        public int LastRowSelectionStartIndex { get; set; }
        public int LastRowSelectedItems { get; set; }

        public int FirstTextCaretIndex { get; set; }
        public int FirstTextSelectionStartIndex { get; set; }
        public int FirstTextSelectedItems{ get; set; }

        public int LastTextCaretIndex { get; set; }
        public int LastTextSelectionStartIndex { get; set; }
        public int LastTextSelectedItems { get; set; }
        
        public string Operation { get; set; }
        public string Argument { get; set; }
        public bool Applied { get; set; }

        public RowContainerFormatAction(ISupportsUndo executor)
            : base(executor)
        {
        }
    }
}
