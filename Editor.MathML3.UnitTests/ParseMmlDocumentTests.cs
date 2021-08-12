using Serilog;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Editor.MathML3.UnitTests
{
    public sealed class ParseMmlDocumentTests
    {
        private static ILogger Log => Serilog.Log.ForContext<ParseMmlDocumentTests>();

        public ParseMmlDocumentTests(ITestOutputHelper output)
        {
            Serilog.Log.Logger = LoggingFactory.CreateLogger(output);
        }

        [Theory]
        [InlineData("Templates/General/Clipboard/clipboard1.mml")]
        [InlineData("Templates/General/Clipboard/clipboard2.mml")]
        [InlineData("Templates/General/Math/emptymath2.mml")]
        [InlineData("Templates/General/Math/math1.mml")]
        [InlineData("Templates/General/Math/math3.mml")]
        [InlineData("Templates/General/Math/mathAdisplay1-1.mml")]
        [InlineData("Templates/General/Math/mathAdisplay1-2.mml")]
        [InlineData("Templates/General/Math/mathAdisplay2-1.mml")]
        [InlineData("Templates/General/Math/mathAdisplay2-2.mml")]
        [InlineData("Templates/General/Math/mathAmacros1.mml")]
        [InlineData("Templates/General/Math/mathAmode1-1.mml")]
        [InlineData("Templates/General/Math/mathAmode1-2.mml")]
        [InlineData("Templates/General/Math/mathBevelledFrac.mml")]
        [InlineData("Templates/General/Math/mathColorSize.mml")]
        [InlineData("Templates/General/Math/mathDir.mml")]
        [InlineData("Templates/General/Math/mathTable1.mml")]
        public void ShouldDeserializeDocument(string filePath)
        {
            var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (folder == null) Assert.False(true, "Assembly location could not be located.");
            Debug.Assert(folder != null);
            var document = XDocument.Load(Path.Combine(folder, filePath));
            var expected = SerializeDocument(document);

            var element = MathMLDeserializer.ToMathElement(document);
            var result = SerializeDocument(element?.ToXDocument());

            // couldn't find a proper working XML Diff library
            // so we log and compare manually
            // TODO: implement a proper XML comparison
            Log.Information(expected);
            Log.Information(result);

            Assert.True(element != null);
        }

        private static string SerializeDocument(XDocument? doc)
        {
            if (doc == null) return string.Empty;
            var builder = new StringBuilder();
            using var writer = new StringWriter(builder);
            doc.Save(writer);
            return builder.ToString();
        }
    }
}
