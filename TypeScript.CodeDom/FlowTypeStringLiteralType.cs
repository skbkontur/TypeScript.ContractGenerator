namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeStringLiteralType : FlowTypeType
    {
        public FlowTypeStringLiteralType(string value)
        {
            this.value = value;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("'{0}'", value);
        }

        private readonly string value;
    }
}