namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeTemplateStringLiteral : FlowTypeExpression
    {
        public FlowTypeTemplateStringLiteral(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("`{0}`", Value);
        }
    }
}