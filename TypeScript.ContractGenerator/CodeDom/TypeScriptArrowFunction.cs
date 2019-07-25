using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptArrowFunction : TypeScriptExpression
    {
        public List<TypeScriptArgumentDeclaration> Arguments => arguments;
        public TypeScriptExpression Body { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"({Arguments.GenerateCodeCommaSeparated(context)}) => {Body.GenerateCode(context)}";
        }

        private readonly List<TypeScriptArgumentDeclaration> arguments = new List<TypeScriptArgumentDeclaration>();
    }
}