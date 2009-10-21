using System.Collections;
using System.Collections.Generic;
using nLess;
using Peg.Base;

namespace ObjectSpike
{
    class NodeEnumerator : IEnumerable<PegNode>
    {
        private readonly PegNode rootNode;

        public NodeEnumerator(PegNode rootNode)
        {
            this.rootNode = rootNode;
        }

        public IEnumerator<PegNode> GetEnumerator()
        {
            var current = rootNode.child_;
            while(current != null)
            {
                switch (current.id_.ToEnLess())
                {
                    case EnLess.comment:
                        break;
                    default:
                        yield return current;
                        break;
                }
                current = current.next_;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
