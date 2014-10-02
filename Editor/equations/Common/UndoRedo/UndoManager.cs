using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{   
    public static class UndoManager
    {
        public static bool DisableAddingActions { get; set; }
        static Stack<EquationAction> undoStack = new Stack<EquationAction>();
        static Stack<EquationAction> redoStack = new Stack<EquationAction>();

        public static event EventHandler<UndoEventArgs> CanUndo = (a, b) => { };
        public static event EventHandler<UndoEventArgs> CanRedo = (a, b) => { };

        public static void AddUndoAction(EquationAction equationAction)
        {
            if (!DisableAddingActions)
            {
                undoStack.Push(equationAction);
                redoStack.Clear();
                CanUndo(null, new UndoEventArgs(true));
                CanRedo(null, new UndoEventArgs(false));
            }
        }
        
        public static void Undo()
        {
            if (undoStack.Count > 0)
            {
                EquationAction temp = undoStack.Peek();
                for (int i = 0; i <= temp.FurtherUndoCount; i++)
                {
                    EquationAction action = undoStack.Pop();
                    action.Executor.ProcessUndo(action);
                    action.UndoFlag = !action.UndoFlag;
                    redoStack.Push(action);
                }
                if (undoStack.Count == 0)
                {
                    CanUndo(null, new UndoEventArgs(false));
                }
                CanRedo(null, new UndoEventArgs(true));                    
            }
        }

        public static void Redo()
        {
            if (redoStack.Count > 0)
            {
                EquationAction temp = redoStack.Peek();
                for (int i = 0; i <= temp.FurtherUndoCount; i++)
                {
                    EquationAction action = redoStack.Pop();
                    action.Executor.ProcessUndo(action);
                    action.UndoFlag = !action.UndoFlag;
                    undoStack.Push(action);
                }
                if (redoStack.Count == 0)
                {
                    CanRedo(null, new UndoEventArgs(false));
                }
                CanUndo(null, new UndoEventArgs(true));                    
            }
        }

        public static void ClearAll()
        {
            undoStack.Clear();
            redoStack.Clear();
            CanUndo(null, new UndoEventArgs(false));
            CanRedo(null, new UndoEventArgs(false));
        }

        public static int UndoCount
        {
            get { return undoStack.Count; }
        }

        public static void ChangeUndoCountOfLastAction(int newCount)
        {
            undoStack.Peek().FurtherUndoCount = newCount;
            for (int i = 0; i < newCount; i++)
            {
                redoStack.Push(undoStack.Pop());
            }
            undoStack.Peek().FurtherUndoCount = newCount;
            for (int i = 0; i < newCount; i++)
            {
                undoStack.Push(redoStack.Pop());
            }
        }
    }
}

