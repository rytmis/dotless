using System;
using System.IO;
using ObjectSpike.Operations;

namespace ObjectSpike
{
    class Program
    {
        static readonly IDocumentOperation[] Operations = new IDocumentOperation[] {
                                                                      new FlattenDocumentOperation(),
                                                                      new GroupBySelectorOperation(),
                                                                      new GroupByPropertyOperation(),
                                                                      new RemoveDuplicatePropertiesOperation(),
                                                                      new RemoveEmptyRulesOperation(),
                                                                     // new AutoFixIEOperation(),
                                                                  };
        static void Main(string[] args)
        {
            var less = File.ReadAllText(@"..\..\..\nLess.Test\Spec\less\rulesets.less");
            var parser = new nLess.nLess(less, Console.Out);
            parser.Parse();
            var root = parser.GetRoot();
            var loader = new Loader {Src = less};
            var doc = loader.ToDocument(root);

            foreach(var operation in Operations)
            {
                doc = operation.Execute(doc);
            }
            Console.WriteLine(doc.ToCss());
        }
    }
}
