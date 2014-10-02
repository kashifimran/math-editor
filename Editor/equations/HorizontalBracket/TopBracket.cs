using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public class TopBracket : HorizontalBracket
    {
        public TopBracket(EquationContainer parent, HorizontalBracketSignType signType)
             :base (parent, signType)
        {
            topEquation.FontFactor = SubFontFactor;
            ActiveChild = bottomEquation;
        }
    }
}
