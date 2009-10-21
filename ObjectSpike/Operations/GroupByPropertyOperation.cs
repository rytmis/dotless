using System.Collections.Generic;
using System.Linq;

namespace ObjectSpike.Operations
{
    internal class GroupByPropertyOperation : IDocumentOperation
    {
        public LessDocument Execute(LessDocument source)
        {
            var document = new LessDocument();
            var rulesbag = new List<LessRule>(source.Rules);
            while(rulesbag.Count > 0)
            {
                var currentRule = rulesbag.First();
                var similarRules = rulesbag.Where(x => x.Properties.SequenceEqual(currentRule.Properties)).ToList();
                var newRule = new LessRule();
                foreach(var rule in similarRules)
                {
                    newRule.Selectors.AddRange(rule.Selectors);
                    newRule.Properties.AddRange(rule.Properties);
                    rulesbag.Remove(rule);
                }
                document.Rules.Add(newRule);
            }
            return document;
        }
    }
}