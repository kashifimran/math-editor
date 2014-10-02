using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class EquationRowPasteAction : EquationAction
    {
        public TextEquation ActiveTextEquation { get; set; }
        public int SelectedItems { get; set; }
        public int SelectionStartIndex { get; set; }

        public int ActiveChildCaretIndex { get; set; }
        public int ActiveChildSelectedItems { get; set; }
        public int ActiveChildSelectionStartIndex { get; set; }
        public string ActiveChildText { get; set; }
        public int[] ActiveChildFormats { get; set; }
        public EditorMode[] ActiveChildModes { get; set; }
        public CharacterDecorationInfo[] ActiveChildDecorations { get; set; }

        public string FirstNewText { get; set; }
        public int[] FirstNewFormats { get; set; }
        public EditorMode[] FirstNewModes { get; set; }
        public CharacterDecorationInfo[] FirstNewDecorations { get; set; }

        public string LastNewText { get; set; }
        public int[] LastNewFormats { get; set; }                
        public EditorMode[] LastNewModes { get; set; }        
        public CharacterDecorationInfo[] LastNewDecorations { get; set; }
        
        public List<EquationBase> Equations { get; set; }

        public EquationRowPasteAction(ISupportsUndo executor)
            : base(executor)
        {
        }
    }
}

