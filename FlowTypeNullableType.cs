namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeNullableType : FlowTypeType
    {
        public FlowTypeNullableType(FlowTypeType innerType)
        {
            this.innerType = innerType;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return "?" + innerType.GenerateCode(context);
        }

        private readonly FlowTypeType innerType;
    }
}