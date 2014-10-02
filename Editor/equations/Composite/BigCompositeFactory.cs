using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    public static class BigCompositeFactory
    {
        public static EquationBase CreateEquation(EquationContainer equationParent, Position position)
        {
            CompositeBase equation = null;
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
                case Position.Sub:
                    equation = new CompositeSub(equationParent);
                    break;
                case Position.Super:
                    equation = new CompositeSuper(equationParent);
                    break;
                case Position.SubAndSuper:
                    equation = new CompositeSubSuper(equationParent);
                    break;
            }
            equation.ChangeMainEquationSize(1.3);
            return equation;
        }
    }
}
