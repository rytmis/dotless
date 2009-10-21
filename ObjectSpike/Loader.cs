using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using nLess;
using Peg.Base;

namespace ObjectSpike
{
    class Loader
    {
        public string Src { get; set; }

        public string PrintTree(PegNode root, int indent)
        {
            var builder = new StringBuilder();
            builder.Append(new string(' ', indent));
            builder.AppendLine(root.id_.ToEnLess().ToString());

            foreach(var cursor in root.AsEnumerable())
            {
                builder.Append(PrintTree(cursor, indent + 1));
            }
            return builder.ToString();
        }

        public LessDocument ToDocument(PegNode node)
        {
            var document = new LessDocument();

            // assumption: Root will ALWAYS contain only 'primary' nodes.
            foreach(var childNode in node.child_.AsEnumerable())
            {
                switch (childNode.id_.ToEnLess())
                {
                    case EnLess.ruleset:
                        var ruleset = ToRule(childNode.child_);
                        document.Rules.AddRange(ruleset);
                        break;
                    default:
                        System.Console.WriteLine(childNode.id_.ToEnLess());
                        break;
                }
            }

            return document;
        }

        private IEnumerable<LessRule> ToRule(PegNode node)
        {
            var rule = new LessRule();
            var selectors = new List<LessSelector>();
            foreach(var childNode in node.AsEnumerable())
            {
                switch (childNode.id_.ToEnLess())
                {
                    case EnLess.selectors:
                        selectors.AddRange(childNode.AsEnumerable().Select(x => ToSelectors(x)));
                        break;

                    case EnLess.primary:
                        foreach(var cursor in childNode.AsEnumerable())
                        {
                            switch(cursor.id_.ToEnLess())
                            {
                                case EnLess.ruleset:
                                    var ruleset = ToRule(cursor.child_);
                                    rule.Rules.AddRange(ruleset);
                                    break;
                                case EnLess.declaration:
                                    var prop = ToProperty(cursor.child_);
                                    rule.Properties.Add(prop);
                                    break;
                                default:
                                    System.Console.WriteLine(cursor.id_.ToEnLess());
                                    break;

                            }
                        }
                        break;

                    default:
                        Debug.WriteLine(childNode.id_.ToEnLess());
                        break;
                }
            }
            foreach(var selector in selectors)
            {
                var r = (LessRule)rule.Clone();
                r.Selectors.Add(selector);
                yield return r;
            }
        }

        private LessProperty ToProperty(PegNode node)
        {
            var property = new LessProperty();
            foreach(var cursor in node.AsEnumerable())
            {
                switch(cursor.id_.ToEnLess())
                {
                    case EnLess.ident:
                        property.Key = cursor.GetAsString(Src);
                        break;
                    case EnLess.expressions:
                        property.Value = cursor.GetAsString(Src);
                        break;
                }
            }
            return property;
        }

        private LessSelector ToSelectors(PegNode node)
        {
            var selector = new LessSelector {Name = node.GetAsString(Src).Trim()};
            return selector;
        }
        
    }
}