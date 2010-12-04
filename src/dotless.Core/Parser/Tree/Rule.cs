using dotless.Core.Exceptions;

namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Rule : Node
    {
        public string Name { get; set; }
        public Node Value { get; set; }
        public bool Variable { get; set; }

        public Rule(string name, Node value)
        {
            Name = name;
            Value = value;
            Variable = name != null ? name[0] == '@' : false;
        }

        public override Node Evaluate(Env env)
        {
            env.Rule = this;

            if (Value == null)
            {
                throw new ParsingException("No value found for rule " + Name, Index);
            }
            
            var rule = new Rule(Name, Value.Evaluate(env)) {Index = Index};

            env.Rule = null;
            
            return rule;
        }

        public override string ToCSS(Env env)
        {
            if (Variable)
                return "";

            return Name + (env.Compress ? ":" : ": ") + Value.ToCSS(env) + ";";
        }
    }
}