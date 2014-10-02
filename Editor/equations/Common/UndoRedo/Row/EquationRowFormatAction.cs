using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class EquationRowFormatAction : EquationAction
    {
        public int SelectionStartIndex { get; set; }
        public int SelectedItems { get; set; }
        public int FirstChildSelectionStartIndex { get; set; }
        public int FirstChildSelectedItems { get; set; }
        public int LastChildSelectionStartIndex { get; set; }
        public int LastChildSelectedItems { get; set; }        
        public string Operation { get; set; }
        public string Argument { get; set; }
        public bool Applied { get; set; }

        public EquationRowFormatAction(ISupportsUndo executor)
            : base(executor)
        {
        }
    }
}
