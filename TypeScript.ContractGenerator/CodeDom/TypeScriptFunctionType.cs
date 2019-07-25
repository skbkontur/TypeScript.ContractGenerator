using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptFunctionType : TypeScriptExpression
    {
        public List<TypeScriptArgumentDeclaration> Arguments => arguments;
        public TypeScriptExpression Result { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"({Arguments.GenerateCodeCommaSeparated(context)}) => {Result.GenerateCode(context)}";
        }

        private readonly List<TypeScriptArgumentDeclaration> arguments = new List<TypeScriptArgumentDeclaration>();
    }
}