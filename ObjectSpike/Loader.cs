using System.Collections.Generic;
using System.Diagnostics;
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
            foreach(var childNode in node.AsEnumerable())
            {
                switch (childNode.id_.ToEnLess())
                {
                    case EnLess.standard_ruleset:
                        var ruleset = ToRule(childNode);
                        document.Rules.AddRange(ruleset);
                        break;
                    default:
                        System.Console.WriteLine(childNode.id_.ToEnLess());
                        break;
                }
            }

            return document;
        }

        private string PrintTree(PegNode root)
        {
            return PrintTree(root, 0);
        }

        private IEnumerable<LessRule> ToRule(PegNode node)
        {
            var rule = new LessRule();
            var selectors = new List<LessSelector>();
            foreach(var childNode in node.AsEnumerable())
            {
                switch (childNode.id_.ToEnLess())
                {
                    case EnLess.selector:
                        var selector = ToSelectors(childNode);
                        selectors.Add(selector);
                        break;

                    case EnLess.standard_ruleset:
                        var ruleset = ToRule(childNode);
                        rule.Rules.AddRange(ruleset);
                        break;
                    
                    case EnLess.standard_declaration:
                        if(childNode.child_.id_.ToEnLess() == EnLess.variable)
                        {
                            var variable = ToVariable(childNode);
                            
                        }
                        else
                        {
                            var prop = ToProperty(childNode);
                            rule.Properties.Add(prop);                            
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
            var selector = new LessSelector { Name = node.GetAsString(Src).Trim() };
            return selector;
        }

        private LessVariable ToVariable(PegNode node)
        {
            return new LessVariable
               {
                   Name = node.child_.GetAsString(Src).Substring(1),
                   Value = node.child_.next_.GetAsString(Src)
               };
        }
    }
}