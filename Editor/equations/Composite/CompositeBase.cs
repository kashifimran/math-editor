using System;
using System.Text;
using System.Windows.Media;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace Editor
{
    public abstract class CompositeBase : EquationContainer
    {
        protected RowContainer mainRowContainer;
        protected double bottomGap;
        protected double SubOverlap { get { return FontSize * .4; } }
        protected double SuperOverlap { get { return FontSize * 0.32; } }

        public CompositeBase(EquationContainer parent)
            : base(parent)
        {
            ActiveChild = mainRowContainer = new RowContainer(this);
            DetermineBottomGap();           
        }

        private void DetermineBottomGap()
        {
            bottomGap = FontSize / 20;
        }

        public override double FontSize
        {
            get
            {
                return base.FontSize;
            }
            set
            {
                base.FontSize = value;
                DetermineBottomGap();
            }
        }

        public void ChangeMainEquationSize(double newFontSizeFactor)
        {
            mainRowContainer.FontFactor = newFontSizeFactor;
        }
    }
}
