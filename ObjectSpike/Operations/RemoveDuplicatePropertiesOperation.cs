using System.Linq;

namespace ObjectSpike.Operations
{
    class RemoveDuplicatePropertiesOperation : IDocumentOperation
    {
        public LessDocument Execute(LessDocument source)
        {
            var document = new LessDocument();
            foreach(var rule in source.Rules)
            {
                var newRule = (LessRule)rule.Clone();
                newRule.Properties = rule.Properties.Distinct().ToList();   
                document.Rules.Add(newRule);
            }
            return document;
        }
    }
}
