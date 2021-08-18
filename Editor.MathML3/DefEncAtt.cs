namespace Editor.MathML3
{
    public abstract class DefEncAtt : ElementBase
    {
        public string Encoding { get; set; } = string.Empty;
        public string DefinitionURL { get; set; } = string.Empty;
    }
}
