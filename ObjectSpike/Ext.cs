using System.Collections.Generic;
using nLess;
using Peg.Base;

namespace ObjectSpike
{
    public static class Ext
    {
        public static PegNode NextNode(this PegNode node)
        {
            var ignore = new List<EnLess> { EnLess.primary, EnLess.standard_ruleset, EnLess.mixin_ruleset };
            var currentNode = node;
            while (ignore.Contains(currentNode.id_.ToEnLess())) currentNode = node.child_;
            return currentNode;
        }

        internal static EnLess ToEnLess(this int id)
        {
            return (EnLess) id;
        }

    }
}