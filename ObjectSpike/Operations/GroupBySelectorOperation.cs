using System.Collections.Generic;
using System.Linq;

namespace ObjectSpike.Operations
{
    class GroupBySelectorOperation : IDocumentOperation
    {
        public LessDocument Execute(LessDocument source)
        {
            var document = new LessDocument();
            var groups = source.Rules.GroupBy(x => x.GetSelectors());
            foreach (var grp in groups)
            {
                var properties = from r in grp
                                 from prop in r.Properties
                                 select prop;

                var rule = new LessRule { Properties = properties.ToList(), Selectors = new List<LessSelector> { new LessSelector { Name = grp.Key } } };
                document.Rules.Add(rule);
            }
            return document;
        }
    }
}