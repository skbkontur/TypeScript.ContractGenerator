namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeObjectLiteralSpread : FlowTypeObjectLiteralInitializer
    {
        public FlowTypeObjectLiteralSpread(FlowTypeVariableReference expression)
        {
            Expression = expression;
        }

        public FlowTypeVariableReference Expression { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("...{0}", Expression.GenerateCode(context));
        }
    }
}