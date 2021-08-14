namespace Editor.MathML3
{
    /// <summary>
    /// XML Linking Language Attributes
    /// </summary>
    /// <remarks>
    /// https://www.w3.org/TR/xlink11/
    /// </remarks>
    public interface IXLinkAttributes
    {
        public string My { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }
        public string Role { get; set; }
        public string Title { get; set; }
        public string Show { get; set; }
        public string Actuate { get; set; }
        public string Label { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
