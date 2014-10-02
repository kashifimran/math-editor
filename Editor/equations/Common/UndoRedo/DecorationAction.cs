using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class DecorationAction : EquationAction
    {
        public CharacterDecorationInfo [] CharacterDecorations { get; set; }
        public DecorationAction(ISupportsUndo executor, CharacterDecorationInfo [] cdi)
            : base(executor)
        {
            CharacterDecorations = cdi;
        }
    }
}