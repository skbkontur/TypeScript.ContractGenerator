using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptReturnStatement : TypeScriptStatement
    {
        public TypeScriptReturnStatement([NotNull] TypeScriptExpression expression)
        {
            Expression = expression;
        }

        public TypeScriptExpression Expression { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"return {Expression.GenerateCode(context)};";
        }
    }
}