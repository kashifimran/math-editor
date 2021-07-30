namespace Editor
{
    public sealed class CharacterDecorationInfo
    {
        public CharacterDecorationType DecorationType { get; set; }
        public Position Position { get; set; }
        public string UnicodeString { get; set; } //Only if DecorationType == CharacterDecorationType.Unicode
        public int Index { get; set; } //Should be -1 when not appplicable or invalid

        //public CharacterDecorationInfo(CharacterDecorationType decorationType, Position position)
        //{
        //    DecorationType = decorationType;
        //    Position = position;
        //    //Index = -1;
        //}

        //public CharacterDecorationInfo Clone()
        //{
        //    CharacterDecorationInfo cdi = new CharacterDecorationInfo(this.DecorationType, this.Position);
        //    //cdi.Index = this.Index;
        //    cdi.UnicodeString = this.UnicodeString;
        //    return cdi;
        //}
    }
}
