using System.Collections.Generic;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptFunctionCallExpression : TypeScriptExpression
    {
        public TypeScriptFunctionCallExpression([NotNull] TypeScriptExpression methodName,
                                                [NotNull, ItemNotNull] params TypeScriptExpression[] arguments)
        {
            MethodName = methodName;
            TypeParameters = new List<TypeScriptType>();
            Arguments = new List<TypeScriptExpression>(arguments);
        }

        public TypeScriptFunctionCallExpression([NotNull] TypeScriptExpression methodName,
                                                [NotNull, ItemNotNull] TypeScriptType[] typeParameters,
                                                [NotNull, ItemNotNull] params TypeScriptExpression[] arguments)
        {
            MethodName = methodName;
            TypeParameters = new List<TypeScriptType>(typeParameters);
            Arguments = new List<TypeScriptExpression>(arguments);
        }

        public TypeScriptExpression MethodName { get; }
        public List<TypeScriptType> TypeParameters { get; }
        public List<TypeScriptExpression> Arguments { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var typeArguments = TypeParameters.Count == 0 ? string.Empty : $"<{TypeParameters.GenerateCodeCommaSeparated(context)}>";
            return $"{MethodName.GenerateCode(context)}{typeArguments}({Arguments.GenerateCodeCommaSeparated(context)})";
        }
    }
}