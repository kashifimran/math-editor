﻿using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Group
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class Group
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mgroup");
        }
    }
}
