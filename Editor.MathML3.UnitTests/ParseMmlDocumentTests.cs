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
            var element = MathMLDeserializer.ToMathElement(document);

            // log result for manual inspection
            var doc = element.ToXDocument();
            var builder = new StringBuilder();
            using var writer = new StringWriter(builder);
            doc.Save(writer);
            Log.Information(builder.ToString());

            Assert.True(element != null);
        }
    }
}
