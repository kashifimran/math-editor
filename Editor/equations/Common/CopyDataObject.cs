using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Editor
{
    public sealed class CopyDataObject
    {
        public BitmapSource Image { get; set; }
        public string Text { get; set; }
        public XElement XElement { get; set; }
    }
}
