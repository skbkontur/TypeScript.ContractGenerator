using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.Extensions
{
    public static class CodeGenerationExtensions
    {
        [NotNull]
        public static string GenerateCodeCommaSeparated([NotNull, ItemNotNull] this IEnumerable<TypeScriptExpression> expressions, [NotNull] ICodeGenerationContext context)
        {
            return string.Join(", ", expressions.Select(x => x.GenerateCode(context)));
        }

        [NotNull]
        public static string GenerateCodeCommaSeparated([NotNull, ItemNotNull] this IEnumerable<TypeScriptType> types, [NotNull] ICodeGenerationContext context)
        {
            return string.Join(", ", types.Select(x => x.GenerateCode(context)));
        }
    }
}