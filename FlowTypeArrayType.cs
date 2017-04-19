namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeArrayType : FlowTypeType
    {
        public FlowTypeArrayType(FlowTypeType itemType)
        {
            ItemType = itemType;
        }

        public FlowTypeType ItemType { get; private set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return ItemType.GenerateCode(context) + "[]";
        }
    }
}