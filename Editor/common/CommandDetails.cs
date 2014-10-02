using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Editor
{
    public class CommandDetails
    {
        public Image Image { get; set; }
        public string UnicodeString { get; set; }
        public CommandType CommandType { get; set; }
        public object CommandParam { get; set; }
    }
}
