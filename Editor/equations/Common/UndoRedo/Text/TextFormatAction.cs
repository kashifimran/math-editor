using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class TextFormatAction : EquationAction
    {
        public int Index { get; set; }
        public int[] OldFormats { get; set; }
        public int[] NewFormats { get; set; }

        public TextFormatAction(ISupportsUndo executor, int index, int[] oldFormats, int[] newFormats)
            : base(executor)
        {
            Index = index;
            OldFormats = oldFormats;
            NewFormats = newFormats;
        }
    }
}
