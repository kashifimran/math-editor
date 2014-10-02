using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Editor
{
    public class DivMathWithBottom : DivMathWithOuterBase
    {
        public DivMathWithBottom(EquationContainer parent)
            : base(parent)
        {
            divMathSign.IsInverted = true; 
        }       

        protected override void AdjustVertical()
        {
            outerEquation.Bottom = Bottom;
            insideEquation.Top = Top;
            divMathSign.Bottom = outerEquation.Top - VerticalGap;
        } 
      
        public override double RefY
        {
            get
            {
                return insideEquation.Height - (insideEquation.FirstRow.Height - insideEquation.FirstRow.RefY);
            }
        }
    }
}
