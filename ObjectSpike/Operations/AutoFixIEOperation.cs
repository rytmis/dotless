using System.Collections.Generic;
using System.Linq;

namespace ObjectSpike.Operations
{
    internal class AutoFixIEOperation : IDocumentOperation
    {
        public LessDocument Execute(LessDocument source)
        {
            var document = new LessDocument {Rules = new List<LessRule>(source.Rules)};
            foreach(var rule in document.Rules.Where(x => x.Properties.Any(p => p.Key == "opacity")))
            {
                rule.Properties.Add(new LessProperty {Key = "-moz-opacity", Value = "HEY! YOU NEED TO FIX THIS!"});
            }
            return document;
        }
    }
}