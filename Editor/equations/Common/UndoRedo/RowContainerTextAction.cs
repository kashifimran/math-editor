using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{   
    public class RowContainerTextAction : EquationAction
    {        
        public int SelectionStartIndex { get; set; }
        public int SelectedItems { get; set; }

        public EquationBase ActiveEquation { get; set; }
        public EquationBase ActiveEquationAfterChange { get; set; }
        public int ActiveEquationSelectionIndex { get; set; }
        public int ActiveEquationSelectedItems { get; set; }
        
        public TextEquation ActiveTextInRow { get; set; }
        public int CaretIndexOfActiveText { get; set; }
        public int SelectionStartIndexOfTextEquation { get; set; }
        public int SelectedItemsOfTextEquation { get; set; }
        public string TextEquationContents { get; set; }
        public int[] TextEquationFormats { get; set; }
        public EditorMode[] TextEquationModes { get; set; }
        public CharacterDecorationInfo[] TextEquationDecoration { get; set; }

        public string FirstLineOfInsertedText { get; set; }
        public int[] FirstFormatsOfInsertedText { get; set; }
        public EditorMode[] FirstModesOfInsertedText { get; set; }
        public CharacterDecorationInfo[] FirstDecorationsOfInsertedText { get; set; }

        public List<EquationRow> Equations { get; set; }

        public RowContainerTextAction(ISupportsUndo executor)
            : base(executor)
        {   
        }
    }    
}

