namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public abstract class FlowTypeExpression
    {
        public abstract string GenerateCode(ICodeGenerationContext context);
    }
}