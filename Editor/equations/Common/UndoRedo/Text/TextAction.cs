using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class TextAction : EquationAction
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public int[] Formats { get; set; }
        public EditorMode[] Modes { get; set; }
        public CharacterDecorationInfo[] Decorations { get; set; }
        public bool Added { get; set; }

        public TextAction(ISupportsUndo executor)
            : base(executor)
        {            
        }
    }
}