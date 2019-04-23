using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptObjectLiteralSpread : TypeScriptObjectLiteralInitializer
    {
        public TypeScriptObjectLiteralSpread([NotNull] TypeScriptVariableReference expression)
        {
            Expression = expression;
        }

        public TypeScriptVariableReference Expression { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"...{Expression.GenerateCode(context)}";
        }
    }
}