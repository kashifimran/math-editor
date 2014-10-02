using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{   
    public class RowContainerPasteAction : EquationAction
    {
        public int SelectionStartIndex { get; set; }
        public int SelectedItems { get; set; }

        public EquationBase ActiveEquation { get; set; }
        public int ActiveEquationSelectionIndex { get; set; }
        public int ActiveEquationSelectedItems { get; set; }
        
        public TextEquation ActiveTextInChildRow { get; set; }
        public int CaretIndexOfActiveText { get; set; }
        public int SelectionStartIndexOfTextEquation { get; set; }
        public int SelectedItemsOfTextEquation { get; set; }
        public string TextEquationContents { get; set; }

        public int[] TextEquationFormats { get; set; }
        public EditorMode[] TextEquationModes { get; set; }
        public CharacterDecorationInfo[] TextEquationDecorations { get; set; }

        public string HeadTextOfPastedRows { get; set; }
        public string TailTextOfPastedRows { get; set; }

        public int[] HeadFormatsOfPastedRows { get; set; }
        public int[] TailFormatsOfPastedRows { get; set; }

        public EditorMode[] HeadModeOfPastedRows { get; set; }
        public EditorMode[] TailModesOfPastedRows { get; set; }


        public CharacterDecorationInfo[] HeadDecorationsOfPastedRows { get; set; }
        public CharacterDecorationInfo[] TailDecorationsOfPastedRows { get; set; }

        public List<EquationRow> Equations { get; set; }

        public RowContainerPasteAction(ISupportsUndo executor)
            : base(executor)
        {
        }
    }
}

