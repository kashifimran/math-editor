namespace Editor
{
    public sealed class ModeChangeAction : EquationAction
    {
        public int Index { get; set; }
        public EditorMode[] OldModes { get; set; }
        public EditorMode[] NewModes { get; set; }

        public ModeChangeAction(ISupportsUndo executor, int index, EditorMode[] oldModes, EditorMode[] newModes)
            : base(executor)
        {
            Index = index;
            OldModes = oldModes;
            NewModes = newModes;
        }
    }
}
