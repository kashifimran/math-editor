using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{   
    public class RowRemoveAction : EquationAction
    {
        public EquationBase ActiveEquation { get; set; }
        public TextEquation HeadTextEquation { get; set; }
        public TextEquation TailTextEquation { get; set; }

        //public int ParentSelectionStartIndex { get; set; }
        public int SelectionStartIndex { get; set; }
        public int SelectedItems { get; set; }

        public int FirstTextCaretIndex { get; set; }
        public int LastTextCaretIndex { get; set; }
        public string FirstText { get; set; }
        public string LastText { get; set; }
        public int[] FirstFormats { get; set; }
        public EditorMode[] FirstModes { get; set; }
        public int[] LastFormats { get; set; }
        public EditorMode[] LastModes { get; set; }
        public CharacterDecorationInfo[] FirstDecorations { get; set; }
        public CharacterDecorationInfo[] LastDecorations { get; set; }
        public int FirstTextSelectionIndex { get; set; }
        public int LastTextSelectionIndex { get; set; }
        public int FirstTextSelectedItems { get; set; }
        public int LastTextSelectedItems { get; set; }

        public List<EquationBase> Equations { get; set; }

        public RowRemoveAction(ISupportsUndo executor)
            : base(executor)
        {
        }
    }   
}

