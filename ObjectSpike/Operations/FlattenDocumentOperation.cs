using System.Collections.Generic;
using System.Linq;

namespace ObjectSpike.Operations
{
    class FlattenDocumentOperation : IDocumentOperation
    {
        public LessDocument Execute(LessDocument source)
        {
            var document = new LessDocument
                               {
                                   Rules = (from rule in source.Rules
                                            from flatrule in FlattenRule(rule, "")
                                            select flatrule).ToList()
                               };
            return document;
        }

        private static IEnumerable<LessRule> FlattenRule(LessRule rule, string selector)
        {
            var result = new List<LessRule>();
            var currentRule = (LessRule)rule.Clone();
            currentRule.Rules.Clear();
            currentRule.Selectors = currentRule.Selectors.Select(x => new LessSelector { Name = selector + " " + x.Name }).ToList();
            result.Add(currentRule);
            if (rule.Rules.Count > 0)
                result.AddRange(from r in rule.Rules from flatrule in FlattenRule(r, currentRule.GetSelectors()) select flatrule);
            return result;
        }

    }
}