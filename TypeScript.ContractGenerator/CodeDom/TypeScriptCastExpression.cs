namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptCastExpression : TypeScriptExpression
    {
        public TypeScriptCastExpression(TypeScriptExpression expression, TypeScriptType targetType)
        {
            Expression = expression;
            TargetType = targetType;
        }

        public TypeScriptExpression Expression { get; }
        public TypeScriptType TargetType { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            if (context.TypeChecker == JavaScriptTypeChecker.TypeScript)
            {
                return $"({Expression.GenerateCode(context)}) as {TargetType.GenerateCode(context)}";
            }
            return $"(({Expression.GenerateCode(context)}): {TargetType.GenerateCode(context)})";
        }
    }
}