namespace Editor
{
    public sealed class TopBracket : HorizontalBracket
    {
        public TopBracket(EquationContainer parent, HorizontalBracketSignType signType)
             :base (parent, signType)
        {
            topEquation.FontFactor = SubFontFactor;
            ActiveChild = bottomEquation;
        }
    }
}
