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
            return Selectors.First().Name;
        }

        public string ToCss()
        {
            var builder = new StringBuilder();
            builder.AppendLine(GetSelectors());
            builder.AppendLine("{");
            foreach(var property in Properties)
            {
                builder.AppendLine("\t" + property.ToCss());
            }
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}