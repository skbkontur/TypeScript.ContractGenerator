namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeReturnStatement : FlowTypeStatement
    {
        public FlowTypeReturnStatement()
        {
        }

        public FlowTypeReturnStatement(FlowTypeExpression expression)
        {
            Expression = expression;
        }

        public FlowTypeExpression Expression { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return "return " + Expression.GenerateCode(context) + ";";
        }
    }
}