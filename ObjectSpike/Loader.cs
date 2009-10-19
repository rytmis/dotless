using System;
using System.Collections.Generic;
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

            var cursor = root.child_;
            while (cursor != null)
            {
                builder.Append(PrintTree(cursor, indent + 1));
                cursor = cursor.next_;
            }
            return builder.ToString();
        }

        public LessDocument ToDocument(PegNode node)
        {
            var document = new LessDocument();

            var nextNode = node.child_.child_;

            // assumption: Root will ALWAYS contain only 'primary' nodes.
            while (nextNode != null)
            {
                switch (nextNode.id_.ToEnLess())
                {
                    case EnLess.ruleset:
                        var ruleset = ToRule(nextNode.child_);
                        document.Rules.AddRange(ruleset);
                        break;
                    default:
                        break;
                }
                nextNode = nextNode.next_;
            }

            return document;
        }

        private IEnumerable<LessRule> ToRule(PegNode node)
        {
            var rule = new LessRule();
            var selectors = new List<LessSelector>();
            var nextNode = node.child_;
            while (nextNode != null)
            {
                switch (nextNode.id_.ToEnLess())
                {
                    case EnLess.selectors:
                        var selectorNode = nextNode.child_;
                        while(selectorNode != null)
                        {
                            selectors.Add(ToSelectors(selectorNode));
                            selectorNode = selectorNode.next_;
                        }
                        break;

                    case EnLess.primary:
                        {
                        var cursor = nextNode.child_;
                        while (cursor != null)
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
                                    break;

                            }
                            cursor = cursor.next_;
                        }
                        break;
                        }

                    default:
                        break;
                }
                nextNode = nextNode.next_;
            }
            foreach(var selector in selectors)
            {
                var r = rule.Clone() as LessRule;
                r.Selectors.Add(selector);
                yield return r;
            }
        }

        private LessProperty ToProperty(PegNode node)
        {
            var property = new LessProperty();
            var cursor = node.child_;
            while(cursor != null)
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
                cursor = cursor.next_;
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