using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSpike
{
    class Program
    {
        static string src = @"
#first > .second {
    font-size: 2em;
}
#first > .second, #second {
    color: black;
}
";
        static void Main(string[] args)
        {
            var less = src; 
            var parser = new nLess.nLess(less, Console.Out);
            parser.Parse();
            var root = parser.GetRoot();
            //PrintTree(root, 0);
            var loader = new Loader {Src = src};
            var doc = loader.ToDocument(root);
            var grouped = doc.Rules.GroupBy(x => x.GetSelectors());
            foreach(var g in grouped)
            {
                var properties = from r in g
                                 from prop in r.Properties
                                 select prop;
                var rule = new LessRule {Properties = properties.ToList(), Selectors = new List<LessSelector> {new LessSelector { Name = g.Key}}};
                Console.Write(rule.ToCss());
            }
        }
    }
}
