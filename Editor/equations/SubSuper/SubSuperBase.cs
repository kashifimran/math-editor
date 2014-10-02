using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Editor
{
    public abstract class SubSuperBase : EquationContainer
    {
        public Position Position { get; set; }                
        protected double Padding 
        { 
            get 
            {
                if (buddy != null && buddy.GetType() == typeof(TextEquation))
                {
                    return FontSize * .01;
                }
                else
                {
                    return FontSize * .05;
                }
            } 
        }
        protected double SuperOverlap { get { return FontSize * 0.35; } }
        protected double SubOverlap 
        { 
            get 
            {
                TextEquation te = buddy as TextEquation;
                double oha = 0;
                if (te != null)
                {
                    oha = te.GetCornerDescent(this.Position);
                }
                return FontSize * .1 - oha; 
            } 
        }
        
        EquationBase buddy = null;
        protected EquationBase Buddy
        {
            get { return buddy ?? ParentEquation.ActiveChild; }
            set { buddy = value; }
        }
        
        public SubSuperBase(EquationRow parent, Position position)
            : base(parent)
        {
            ApplySymbolGap = false;
            SubLevel++;
            Position = position;            
        }

        public void SetBuddy(EquationBase buddy)
        {
            this.Buddy = buddy;
            CalculateHeight();
        }
    }
}
