﻿using System;
using System.Xml.Linq;

namespace Editor.MathML3
{
    /// <summary>
    /// Line
    /// </summary>
    [Obsolete("not supported in Firefox")]
    public sealed class mline
    {
        public XElement ToXElement()
        {
            return new XElement(Ns.MathML + "mline");
        }
    }
}