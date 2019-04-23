using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptArrayExpression : TypeScriptExpression
    {
        public TypeScriptArrayExpression(params TypeScriptExpression[] items)
        {
            Items = new List<TypeScriptExpression>(items);
        }

        public List<TypeScriptExpression> Items { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"[{Items.EnumerateWithComma(context)}]";
        }
    }
}