using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Common XML Namespaces
    /// </summary>
    public static class Ns
    {
        /// <summary>
        /// Mathematical Markup Language
        /// </summary>
        public static XNamespace MathML { get; } = @"http://www.w3.org/1998/Math/MathML";

        /// <summary>
        /// Hyper Text Markup Language v5
        /// </summary>
        public static XNamespace HTML5 { get; } = @"http://www.w3.org/1999/xhtml";

        /// <summary>
        /// Scalable Vector Graphics
        /// </summary>
        public static XNamespace SVG { get; } = @"http://www.w3.org/2000/svg";

        /// <summary>
        /// Dublin Core
        /// </summary>
        public static XNamespace DC { get; } = @"http://purl.org/dc/elements/1.1/";

        /// <summary>
        /// Extensible Markup Language
        /// </summary>
        public static XNamespace XML { get; } = @"http://www.w3.org/XML/1998/namespace";

        /// <summary>
        /// Resource Description Framework
        /// </summary>
        public static XNamespace RDF { get; } = @"http://www.w3.org/1999/02/22-rdf-syntax-ns#";

        /// <summary>
        /// RDF Math Annotations
        /// </summary>
        public static XNamespace Math { get; } = @"http://www.w3.org/2000/10/swap/math#";

        /// <summary>
        /// XML Link
        /// </summary>
        /// <remarks>
        /// Prefer Href attribute where available.
        /// </remarks>
        public static XNamespace XLink { get; } = @"http://www.w3.org/1999/xlink";
    }
}
