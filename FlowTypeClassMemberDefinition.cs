namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeClassMemberDefinition
    {
        public string Name { get; set; }

        public FlowTypeDefinition Definition { get; set; }

        public string GenerateCode(ICodeGenerationContext context)
        {
            return Definition.GenerateCode(Name, context);
        }
    }
}