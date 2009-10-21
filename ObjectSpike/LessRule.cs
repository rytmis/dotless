using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectSpike
{
    class LessRule : ICloneable
    {
        public List<LessRule> Rules = new List<LessRule>();
        public List<LessProperty> Properties = new List<LessProperty>();
        public List<LessSelector> Selectors = new List<LessSelector>();
        
        public object Clone()
        {
            var newrule = new LessRule();
            newrule.Rules.AddRange(Rules);
            newrule.Properties.AddRange(Properties);
            newrule.Selectors.AddRange(Selectors);

            return newrule;
        }

        public string GetSelectors()
        {
            return string.Join(",\r\n", Selectors.Select(x => x.Name).ToArray());
        }

        public string ToCss()
        {
            var builder = new StringBuilder();
            builder.Append(GetSelectors());
            builder.Append(" {");
            
            // print all properties
            foreach(var property in Properties)
            {
                builder.Append(property.ToCss());
            }

            // print all sub-rules
            foreach(var rule in Rules)
            {
                builder.AppendLine("\t" + rule.ToCss());   
            }

            builder.Append(" }");
            return builder.ToString();
        }
    }
}