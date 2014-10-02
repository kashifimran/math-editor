using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public static class CompositeFactory
    {
        public static EquationBase CreateEquation(EquationContainer equationParent, Position position)
        {
            EquationBase equation = null;
            switch (position)
            {                
                case Position.Bottom:
                    equation = new CompositeBottom(equationParent);
                    break;
                case Position.Top:
                    equation = new CompositeTop(equationParent);
                    break;
                case Position.BottomAndTop:
                    equation = new CompositeBottomTop(equationParent);
                    break;                
            }
            return equation;
        }
    }
}
