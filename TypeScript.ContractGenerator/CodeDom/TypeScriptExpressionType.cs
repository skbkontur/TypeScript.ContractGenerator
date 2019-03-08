namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptExpressionType : TypeScriptType
    {
        public TypeScriptExpressionType(TypeScriptExpression expression)
        {
            Expression = expression;
        }
        
        public TypeScriptExpression Expression { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return Expression.GenerateCode(context);
        }
    }
}