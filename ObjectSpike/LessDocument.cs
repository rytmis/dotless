using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSpike
{
    internal class LessDocument
    {
        public List<LessRule> Rules = new List<LessRule>();

        public string ToCss()
        {
            return string.Join(Environment.NewLine, Rules.Select(x => x.ToCss()).ToArray());
        }
    }
}