namespace Editor
{
    public sealed class DivRegularSmall : DivRegular
    {
        public DivRegularSmall(EquationContainer parent)
            : base(parent, true)
        {            
        }
    }

    public sealed class DivDoubleBar : DivRegular
    {
        public DivDoubleBar(EquationContainer parent)
            : base(parent)
        {
            barCount = 2;
        }
    }

    public sealed class DivTripleBar : DivRegular
    {
        public DivTripleBar(EquationContainer parent)
            : base(parent)
        {
            barCount = 3;
        }
    }

    public sealed class DivSlantedSmall : DivSlanted
    {
        public DivSlantedSmall(EquationContainer parent)
            : base(parent, true)
        {            
        }
    }

    public sealed class DivHorizSmall : DivHorizontal
    {
        public DivHorizSmall(EquationContainer parent)
            : base(parent, true)
        {           
        }
    }   

}
