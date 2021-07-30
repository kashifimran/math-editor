namespace Editor
{
    public sealed class DecorationAction : EquationAction
    {
        public CharacterDecorationInfo [] CharacterDecorations { get; set; }
        public bool Added { get; set; }

        public DecorationAction(ISupportsUndo executor, CharacterDecorationInfo [] cdi, bool added)
            : base(executor)
        {
            Added = added;
            CharacterDecorations = cdi;
        }

        public DecorationAction(ISupportsUndo executor, CharacterDecorationInfo[] cdi)
            : base(executor)
        {
            CharacterDecorations = cdi;
        }
    }
}