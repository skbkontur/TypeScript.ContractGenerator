namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeConstantExpression : FlowTypeExpression
    {
        public FlowTypeConstantExpression(string value)
        {
            this.value = value;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return value;
        }

        private readonly string value;
    }
}