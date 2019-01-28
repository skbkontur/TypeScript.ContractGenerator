namespace TypeScript.CodeDom
{
    public class FlowTypeCastExpression : FlowTypeExpression
    {
        public FlowTypeCastExpression(FlowTypeExpression expression, FlowTypeType targetType)
        {
            Expression = expression;
            TargetType = targetType;
        }

        public FlowTypeExpression Expression { get; }
        public FlowTypeType TargetType { get; }

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