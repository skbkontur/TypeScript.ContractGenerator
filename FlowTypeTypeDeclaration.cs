namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeTypeDeclaration : FlowTypeStatement
    {
        public string Name { get; set; }
        public FlowTypeType Definition { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("type {0} = {1};", Name, Definition.GenerateCode(context));
        }
    }
}