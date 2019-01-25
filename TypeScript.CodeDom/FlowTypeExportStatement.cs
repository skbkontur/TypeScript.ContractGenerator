namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeExportStatement : FlowTypeStatement
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            return "export " + Declaration.GenerateCode(context);
        }

        public FlowTypeStatement Declaration { get; set; }
    }
}