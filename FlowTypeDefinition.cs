namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public abstract class FlowTypeDefinition
    {
        public abstract string GenerateCode(string name, ICodeGenerationContext context);
    }
}