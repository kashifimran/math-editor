using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public abstract class EquationAction
    {
        public bool UndoFlag { get; set; }
        public ISupportsUndo Executor { get; set; }
        public int FurtherUndoCount { get; set; }

        public EquationAction(ISupportsUndo executor)
        {
            Executor = executor;
            UndoFlag = true;
        }
    }
}

