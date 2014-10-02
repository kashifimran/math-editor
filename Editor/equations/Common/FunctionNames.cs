using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor
{
    static class FunctionNames
    {
        static List<string> names = new List<string>();

        static FunctionNames()
        {
            names.AddRange(new string[] {  "arccos", "arcsin", "arctan", "arg", "cos", "cosh", "cot", "coth",
                                           "cov", "csc", "curl", "deg", "det", "dim", "div", "erf", "exp", "gcd", "glb", "grad", "hom", "lm",
                                           "inf", "int", "ker", "lg", "lim", "ln", "log", "lub", "max",
                                           "min", "mod", "Pr", "Re", "rot", "sec", "sgn", "sin", "sinh", "sup", "tan", "tanh", "var",                                           
                                        });
        }

        public static bool IsFunctionName(string text)
        {
            return names.Contains(text);
        }

        public static string CheckForFunctionName(string text)
        {
            foreach (string s in names)
            {
                if (text.EndsWith(s))
                {
                    return s;
                }
            }
            return null;
        }
    }
}
