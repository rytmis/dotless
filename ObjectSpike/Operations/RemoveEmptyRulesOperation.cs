using System.Linq;

namespace ObjectSpike.Operations
{
    internal class RemoveEmptyRulesOperation : IDocumentOperation
    {
        public LessDocument Execute(LessDocument source)
        {
            var document = new LessDocument();
            document.Rules = source.Rules.Where(x => x.Properties.Count > 0).ToList();
            return document;
        }
    }
}