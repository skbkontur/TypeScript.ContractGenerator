namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeTypeReference : FlowTypeType
    {
        public FlowTypeTypeReference(string name)
        {
            this.name = name;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return name;
        }

        private readonly string name;
    }
}