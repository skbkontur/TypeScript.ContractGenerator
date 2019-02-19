namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeArrayType : FlowTypeType
    {
        public FlowTypeArrayType(FlowTypeType itemType)
        {
            ItemType = itemType;
        }

        private FlowTypeType ItemType { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var innerTypeCode = ItemType.GenerateCode(context);
            if (!(ItemType is FlowTypeUnionType))
                return innerTypeCode + "[]";

            return $"Array<{innerTypeCode}>";
        }
    }
}