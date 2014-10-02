using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class DecorationAction : EquationAction
    {
        public CharacterDecorationInfo [] CharacterDecorations { get; set; }
        public bool Added { get; set; }

        public DecorationAction(ISupportsUndo executor, CharacterDecorationInfo [] cdi, bool added)
            : base(executor)
        {
            Added = added;
            CharacterDecorations = cdi;
        }
    }
}