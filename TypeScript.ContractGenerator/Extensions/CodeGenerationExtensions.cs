using System.Collections.Generic;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Extensions
{
    public static class CodeGenerationExtensions
    {
        public static string GenerateCodeCommaSeparated(this IEnumerable<TypeScriptExpression> expressions, ICodeGenerationContext context)
        {
            return string.Join(", ", expressions.Select(x => x.GenerateCode(context)));
        }

        public static string GenerateCodeCommaSeparated(this IEnumerable<TypeScriptType> types, ICodeGenerationContext context)
        {
            return string.Join(", ", types.Select(x => x.GenerateCode(context)));
        }
    }
}