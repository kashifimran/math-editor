namespace Editor
{
    public sealed class BottomBracket : HorizontalBracket
    {
        public BottomBracket(EquationContainer parent, HorizontalBracketSignType signType)
             :base (parent, signType)
        {
            bottomEquation.FontFactor = SubFontFactor;
            ActiveChild = topEquation;
        }
    }
}
