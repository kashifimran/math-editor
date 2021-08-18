using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Editor.MathML3
{
    public static class MathMLDeserializer
    {
        private static Serilog.ILogger Log => Serilog.Log.ForContext(typeof(MathMLDeserializer));

        public static Math ToMathElement(XDocument document)
        {
            if (document.Declaration != null && document.Declaration?.Version != "1.0")
            {
                throw new ArgumentOutOfRangeException($"XML Version must be 1.0, but was {document.Declaration?.Version}");
            }

            var element = document.Root;
            if (element == null)
            {
                throw new ArgumentException("Root element is missing", nameof(document));
            }

            if (element.Name.LocalName != "math")
            {
                throw new ArgumentException($"Root element of MathML files must be named 'math', but was '{element.Name.LocalName}'.", nameof(document));
            }

            if (element.Name != Ns.MathML + "math")
            {
                throw new ArgumentException($"Root element 'math' is not in the proper XML namespace. Expected '{Ns.MathML}', but was '{element.Name.Namespace}'.", nameof(document));
            }

            return ParseMathElement(element);
        }

        private static Math ParseMathElement(XElement element)
        {
            // because of an bug in XElement, the attributes are without MathML namespace
            var mathElement = new Math
            {
                Class = element.Attribute("class")?.Value ?? string.Empty,
                Dir = element.Attribute("dir")?.Value ?? string.Empty,
                Display = element.Attribute("display")?.Value ?? string.Empty,
                Id = element.Attribute("id")?.Value ?? string.Empty,
                MathBackground = element.Attribute("mathbackground")?.Value ?? string.Empty,
                MathColor = element.Attribute("mathcolor")?.Value ?? string.Empty,
                Mode = element.Attribute("mode")?.Value ?? string.Empty,
                Style = element.Attribute("style")?.Value ?? string.Empty,
                LanguageCode = element.Attribute(Ns.XML + "lang")?.Value ?? string.Empty,
                Macros = element.Attribute("macros")?.Value ?? string.Empty,
                Bevelled = element.Attribute("bevelled")?.Value ?? string.Empty,
                Mathsize = element.Attribute("mathsize")?.Value ?? string.Empty
            };

            foreach (var child in GetChilden(element.Elements()))
            {
                mathElement.Children.Add(child);
            }

            return mathElement;
        }

        private static IEnumerable<IMathMLElement> GetChilden(IEnumerable<XElement> elements)
        {
            foreach (var element in elements)
            {
                #region Tokens
                if (element.Name == Ns.MathML + "mo")
                {
                    yield return ParseMoElement(element);
                }
                else if (element.Name == Ns.MathML + "mi")
                {
                    yield return ParseMiElement(element);
                }
                else if (element.Name == Ns.MathML + "ms")
                {
                    yield return ParseMsElement(element);
                }
                else if (element.Name == Ns.MathML + "mn")
                {
                    yield return ParseMnElement(element);
                }
                else if (element.Name == Ns.MathML + "mspace")
                {
                    yield return ParseMspaceElement(element);
                }
                else if (element.Name == Ns.MathML + "mtext")
                {
                    yield return ParseMtextElement(element);
                }
                else if (element.Name == Ns.MathML + "mfrac")
                {
                    yield return ParseMfracElement(element);
                }
                else if (element.Name == Ns.MathML + "mrow")
                {
                    yield return ParseMrowElement(element);
                }
                else
                {
                    Log.Warning("The element {elementname} is not supported", element.Name);
                }
                #endregion
            }

            yield break;
        }

        private static mrow ParseMrowElement(XElement element)
        {
            var row = new mrow
            {
                Class = element.Attribute(Ns.MathML + "class")?.Value ?? string.Empty,
                Id = element.Attribute(Ns.MathML + "id")?.Value ?? string.Empty,
                MathBackground = element.Attribute(Ns.MathML + "mathbackground")?.Value ?? string.Empty,
                MathColor = element.Attribute(Ns.MathML + "mathcolor")?.Value ?? string.Empty,
                Style = element.Attribute(Ns.MathML + "mode")?.Value ?? string.Empty,
                DisplayStyle = element.Attribute(Ns.MathML + "displaystyle")?.Value ?? string.Empty,
                Href = element.Attribute(Ns.MathML + "href")?.Value ?? string.Empty,
                LanguageCode = element.Attribute(Ns.XML + "lang")?.Value ?? string.Empty,
                Dir = element.Attribute(Ns.XML + "dir")?.Value ?? string.Empty,
            };

            foreach (var child in GetChilden(element.Elements()))
            {
                row.Content.Add(child);
            }

            return row;
        }

        private static Fraction ParseMfracElement(XElement element)
        {
            var frac = new Fraction
            {
                Class = element.Attribute(Ns.MathML + "class")?.Value ?? string.Empty,
                Id = element.Attribute(Ns.MathML + "id")?.Value ?? string.Empty,
                MathBackground = element.Attribute(Ns.MathML + "mathbackground")?.Value ?? string.Empty,
                MathColor = element.Attribute(Ns.MathML + "mathcolor")?.Value ?? string.Empty,
                Style = element.Attribute(Ns.MathML + "mode")?.Value ?? string.Empty,
                DisplayStyle = element.Attribute(Ns.MathML + "displaystyle")?.Value ?? string.Empty,
                Href = element.Attribute(Ns.MathML + "href")?.Value ?? string.Empty,
                LanguageCode = element.Attribute(Ns.XML + "lang")?.Value ?? string.Empty,
                Bevelled = element.Attribute(Ns.XML + "bevelled")?.Value ?? string.Empty,
                Denomalign = element.Attribute(Ns.XML + "denomalign")?.Value ?? string.Empty,
                LineThickness = element.Attribute(Ns.XML + "linethickness")?.Value ?? string.Empty,
                Numalign = element.Attribute(Ns.XML + "numalign")?.Value ?? string.Empty
            };

            foreach (var child in GetChilden(element.Elements()))
            {
                frac.Content.Add(child);
            }

            return frac;
        }

        private static Tokens.mtext ParseMtextElement(XElement element)
        {
            return new Tokens.mtext
            {
                Content = element.Value,
                Class = element.Attribute(Ns.MathML + "class")?.Value ?? string.Empty,
                Dir = element.Attribute(Ns.MathML + "dir")?.Value ?? string.Empty,
                Id = element.Attribute(Ns.MathML + "id")?.Value ?? string.Empty,
                MathBackground = element.Attribute(Ns.MathML + "mathbackground")?.Value ?? string.Empty,
                MathColor = element.Attribute(Ns.MathML + "mathcolor")?.Value ?? string.Empty,
                Style = element.Attribute(Ns.MathML + "mode")?.Value ?? string.Empty,
                DisplayStyle = element.Attribute(Ns.MathML + "displaystyle")?.Value ?? string.Empty,
                Href = element.Attribute(Ns.MathML + "href")?.Value ?? string.Empty,
                LanguageCode = element.Attribute(Ns.XML + "lang")?.Value ?? string.Empty,
                MathSize = element.Attribute(Ns.MathML + "mathsize")?.Value ?? string.Empty,
                MathVariant = element.Attribute(Ns.MathML + "mathvariant")?.Value ?? string.Empty
            };
        }

        private static mspace ParseMspaceElement(XElement element)
        {
            return new mspace
            {
                Class = element.Attribute(Ns.MathML + "class")?.Value ?? string.Empty,
                Id = element.Attribute(Ns.MathML + "id")?.Value ?? string.Empty,
                MathBackground = element.Attribute(Ns.MathML + "mathbackground")?.Value ?? string.Empty,
                Style = element.Attribute(Ns.MathML + "mode")?.Value ?? string.Empty,
                DisplayStyle = element.Attribute(Ns.MathML + "displaystyle")?.Value ?? string.Empty,
                Depth = element.Attribute(Ns.MathML + "depth")?.Value ?? string.Empty,
                Height = element.Attribute(Ns.MathML + "height")?.Value ?? string.Empty,
                Width = element.Attribute(Ns.MathML + "width")?.Value ?? string.Empty,
            };
        }

        private static mn ParseMnElement(XElement element)
        {
            return new mn
            {
                Content = element.Value,
                Class = element.Attribute(Ns.MathML + "class")?.Value ?? string.Empty,
                Dir = element.Attribute(Ns.MathML + "dir")?.Value ?? string.Empty,
                Id = element.Attribute(Ns.MathML + "id")?.Value ?? string.Empty,
                MathBackground = element.Attribute(Ns.MathML + "mathbackground")?.Value ?? string.Empty,
                MathColor = element.Attribute(Ns.MathML + "mathcolor")?.Value ?? string.Empty,
                Style = element.Attribute(Ns.MathML + "mode")?.Value ?? string.Empty,
                DisplayStyle = element.Attribute(Ns.MathML + "displaystyle")?.Value ?? string.Empty,
                Href = element.Attribute(Ns.MathML + "href")?.Value ?? string.Empty,
                LanguageCode = element.Attribute(Ns.XML + "lang")?.Value ?? string.Empty,
                MathSize = element.Attribute(Ns.MathML + "mathsize")?.Value ?? string.Empty,
                MathVariant = element.Attribute(Ns.MathML + "mathvariant")?.Value ?? string.Empty
            };
        }

        private static LiteralString ParseMsElement(XElement element)
        {
            return new LiteralString
            {
                Content = element.Value,
                Class = element.Attribute(Ns.MathML + "class")?.Value ?? string.Empty,
                Dir = element.Attribute(Ns.MathML + "dir")?.Value ?? string.Empty,
                Id = element.Attribute(Ns.MathML + "id")?.Value ?? string.Empty,
                MathBackground = element.Attribute(Ns.MathML + "mathbackground")?.Value ?? string.Empty,
                MathColor = element.Attribute(Ns.MathML + "mathcolor")?.Value ?? string.Empty,
                Style = element.Attribute(Ns.MathML + "mode")?.Value ?? string.Empty,
                DisplayStyle = element.Attribute(Ns.MathML + "displaystyle")?.Value ?? string.Empty,
                Href = element.Attribute(Ns.MathML + "href")?.Value ?? string.Empty,
                LanguageCode = element.Attribute(Ns.XML + "lang")?.Value ?? string.Empty,
                MathSize = element.Attribute(Ns.MathML + "mathsize")?.Value ?? string.Empty,
                MathVariant = element.Attribute(Ns.MathML + "mathvariant")?.Value ?? string.Empty,
                LeftQuote = element.Attribute(Ns.MathML + "lquote")?.Value ?? string.Empty,
                RightQuote = element.Attribute(Ns.MathML + "rquote")?.Value ?? string.Empty,
            };
        }

        private static mi ParseMiElement(XElement element)
        {
            return new mi
            {
                Content = element.Value,
                Class = element.Attribute(Ns.MathML + "class")?.Value ?? string.Empty,
                Dir = element.Attribute(Ns.MathML + "dir")?.Value ?? string.Empty,
                Id = element.Attribute(Ns.MathML + "id")?.Value ?? string.Empty,
                MathBackground = element.Attribute(Ns.MathML + "mathbackground")?.Value ?? string.Empty,
                MathColor = element.Attribute(Ns.MathML + "mathcolor")?.Value ?? string.Empty,
                Style = element.Attribute(Ns.MathML + "mode")?.Value ?? string.Empty,
                DisplayStyle = element.Attribute(Ns.MathML + "displaystyle")?.Value ?? string.Empty,
                Href = element.Attribute(Ns.MathML + "href")?.Value ?? string.Empty,
                LanguageCode = element.Attribute(Ns.XML + "lang")?.Value ?? string.Empty,
                MathSize = element.Attribute(Ns.MathML + "mathsize")?.Value ?? string.Empty,
                MathVariant = element.Attribute(Ns.MathML + "mathvariant")?.Value ?? string.Empty
            };
        }

        private static mo ParseMoElement(XElement element)
        {
            return new mo
            {
                Content = element.Value,
                Class = element.Attribute(Ns.MathML + "class")?.Value ?? string.Empty,
                Dir = element.Attribute(Ns.MathML + "dir")?.Value ?? string.Empty,
                Id = element.Attribute(Ns.MathML + "id")?.Value ?? string.Empty,
                MathBackground = element.Attribute(Ns.MathML + "mathbackground")?.Value ?? string.Empty,
                MathColor = element.Attribute(Ns.MathML + "mathcolor")?.Value ?? string.Empty,
                Style = element.Attribute(Ns.MathML + "mode")?.Value ?? string.Empty,
                DisplayStyle = element.Attribute(Ns.MathML + "displaystyle")?.Value ?? string.Empty,
                Href = element.Attribute(Ns.MathML + "href")?.Value ?? string.Empty,
                LanguageCode = element.Attribute(Ns.XML + "lang")?.Value ?? string.Empty,
                MathSize = element.Attribute(Ns.MathML + "mathsize")?.Value ?? string.Empty,
                MathVariant = element.Attribute(Ns.MathML + "mathvariant")?.Value ?? string.Empty,
            };
        }
    }
}
