using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{   
    public class RowContainerRemoveAction : RowRemoveAction
    {
        public EquationRow HeadEquationRow { get; set; }
        public EquationRow TailEquationRow { get; set; }
        public int FirstRowActiveIndex { get; set; }
        public int LastRowActiveIndex { get; set; }
        public int FirstRowSelectionIndex { get; set; }
        public int LastRowSelectionIndex { get; set; }
        public int FirstRowSelectedItems { get; set; }
        public int LastRowSelectedItems { get; set; }
        public int FirstRowActiveIndexAfterRemoval { get; set; }

        public List<EquationBase> FirstRowDeletedContent { get; set; }
        public List<EquationBase> LastRowDeletedContent { get; set; }

        public RowContainerRemoveAction(ISupportsUndo executor)
            : base(executor)
        {
        }
    }    
}

