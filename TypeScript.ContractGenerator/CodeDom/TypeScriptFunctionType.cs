using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptFunctionType : TypeScriptType
    {
        public List<TypeScriptArgumentDeclaration> Arguments { get; } = new List<TypeScriptArgumentDeclaration>();

        public TypeScriptType Result { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"({Arguments.GenerateCodeCommaSeparated(context)}) => {Result.GenerateCode(context)}";
        }
    }
}