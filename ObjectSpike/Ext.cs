using nLess;
using Peg.Base;

namespace ObjectSpike
{
    public static class Ext
    {
        internal static EnLess ToEnLess(this int id)
        {
            return (EnLess) id;
        }

        /// <summary>
        /// Wraps the node in an IEnumerable&lt;PegNode&gt;, usable in foreach.
        /// </summary>
        /// <param name="rootNode">The node whose childnodes should be accessed via the enumerator.</param>
        /// <returns></returns>
        internal static NodeEnumerator AsEnumerable(this PegNode rootNode)
        {
            return new NodeEnumerator(rootNode);
        }

    }
}