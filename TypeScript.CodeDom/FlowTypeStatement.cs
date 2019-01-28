namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public abstract class FlowTypeStatement
    {
        public abstract string GenerateCode(ICodeGenerationContext context);
    }
}