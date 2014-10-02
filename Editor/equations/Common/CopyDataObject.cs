using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Editor
{
    public class CopyDataObject
    {
        public BitmapSource Image { get; set; }
        public string Text { get; set; }
        public XElement XElement { get; set; }
    }
}
