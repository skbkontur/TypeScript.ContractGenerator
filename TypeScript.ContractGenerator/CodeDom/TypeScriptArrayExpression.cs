using System.Collections.Generic;
using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptArrayExpression : TypeScriptExpression
    {
        public TypeScriptArrayExpression(params TypeScriptExpression[] arguments)
        {
            Items.AddRange(arguments);
        }

        public List<TypeScriptExpression> Items => items;

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("[{0}]", string.Join(", ", Items.Select(x => x.GenerateCode(context))));
        }

        private readonly List<TypeScriptExpression> items = new List<TypeScriptExpression>();
    }
}