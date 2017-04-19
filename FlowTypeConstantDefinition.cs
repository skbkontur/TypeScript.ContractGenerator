namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeConstantDefinition : FlowTypeStatement
    {
        public string Name { get; set; }
        public FlowTypeExpression Value { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("const {0} = {1};", Name, Value.GenerateCode(context));
        }
    }
}