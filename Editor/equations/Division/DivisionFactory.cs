namespace Editor
{
    public static class DivisionFactory
    {
        public static EquationBase CreateEquation(EquationContainer equationParent, DivisionType divType)
        {
            EquationBase equation = null;
            switch (divType)
            {
                case DivisionType.DivRegular:
                    equation = new DivRegular(equationParent);
                    break;
                case DivisionType.DivRegularSmall:
                    equation = new DivRegularSmall(equationParent);
                    break;
                case DivisionType.DivDoubleBar:
                    equation = new DivDoubleBar(equationParent);
                    break;
                case DivisionType.DivTripleBar:
                    equation = new DivTripleBar(equationParent);
                    break;

                case DivisionType.DivHoriz:
                    equation = new DivHorizontal(equationParent);
                    break;
                case DivisionType.DivHorizSmall:
                    equation = new DivHorizSmall(equationParent);
                    break;                

                case DivisionType.DivMath:
                    equation = new DivMath(equationParent);
                    break;
                case DivisionType.DivMathWithTop:
                    equation = new DivMathWithTop(equationParent);
                    break;
                
                case DivisionType.DivSlanted:
                    equation = new DivSlanted(equationParent);
                    break;  
                case DivisionType.DivSlantedSmall:
                    equation = new DivSlantedSmall(equationParent);
                    break;  

                case DivisionType.DivMathInverted:
                    equation = new DivMathInverted(equationParent);
                    break;
                case DivisionType.DivInvertedWithBottom:
                    equation = new DivMathWithBottom(equationParent);
                    break;
                case DivisionType.DivTriangleFixed:
                    equation = new DivTriangle(equationParent, true);
                    break;
                case DivisionType.DivTriangleExpanding:
                    equation = new DivTriangle(equationParent, false);
                    break;                
            }
            return equation;
        }
    }
}
