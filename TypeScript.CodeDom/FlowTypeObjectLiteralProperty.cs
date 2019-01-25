namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeObjectLiteralProperty : FlowTypeObjectLiteralInitializer
    {
        public FlowTypeExpression Name { get; set; }
        public FlowTypeExpression Value { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("[{0}]: {1}", Name.GenerateCode(context), Value.GenerateCode(context));
        }
    }
}