using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace Editor
{
    public class DivHorizontal : DivBase
    {
        double ExtraWidth { get { return FontSize * .3; } }

        public DivHorizontal(EquationContainer parent)
            : base(parent, false)
        {
        }

        public DivHorizontal(EquationContainer parent, bool isSmall)
            : base (parent, isSmall)
        {  
        }
        
        public override void DrawEquation(DrawingContext dc)
        {
            base.DrawEquation(dc);
            dc.DrawLine(StandardPen, new Point(bottomEquation.Left - ExtraWidth/10, Top), new Point(topEquation.Right + ExtraWidth/10, Bottom));   
        }     

        public override double Left
        {
            get { return base.Left; }
            set
            {
                base.Left = value;
                AdjustHorizontal();
            }
        }

        private void AdjustHorizontal()
        {

            topEquation.Left = this.Left;
            bottomEquation.Left = topEquation.Right + ExtraWidth;
        }
        public override double Top
        {
            get { return base.Top; }
            set
            {
                base.Top = value;
                topEquation.MidY = MidY;
                bottomEquation.MidY = MidY;
            }
        }

        public override double RefY
        {
            get
            {
                return Math.Max(topEquation.RefY, bottomEquation.RefY);
            }
        }


        protected override void CalculateWidth()
        {            
            Width = topEquation.Width + bottomEquation.Width + ExtraWidth;
            AdjustHorizontal();
        }

        protected override void CalculateHeight()
        {            
            Height = Math.Max(topEquation.Height , bottomEquation.Height);            
        }

        public override bool ConsumeKey(Key key)
        {
            if (ActiveChild.ConsumeKey(key))
            {
                CalculateSize();
                return true;
            }
            if (key == Key.Right)
            {
                if (ActiveChild == topEquation)
                {
                    ActiveChild = bottomEquation;
                    return true;
                }
            }
            else if (key == Key.Left)
            {
                if (ActiveChild == bottomEquation)
                {
                    ActiveChild = topEquation;
                    return true;
                }
            }
            return false;
        }
    }
}
