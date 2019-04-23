using System.Collections.Generic;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptMethodCallExpression : TypeScriptExpression
    {
        public TypeScriptMethodCallExpression([NotNull] TypeScriptExpression subject, [NotNull] string methodName,
                                              [NotNull, ItemNotNull] params TypeScriptExpression[] arguments)
        {
            Subject = subject;
            MethodName = methodName;
            Arguments = new List<TypeScriptExpression>(arguments);
        }

        public TypeScriptExpression Subject { get; }
        public string MethodName { get; }
        public List<TypeScriptExpression> Arguments { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{Subject.GenerateCode(context)}.{MethodName}({Arguments.GenerateCodeCommaSeparated(context)})";
        }
    }
}