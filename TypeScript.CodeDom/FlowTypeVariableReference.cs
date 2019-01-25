namespace SKBKontur.Catalogue.FlowType.CodeDom
{
    public class FlowTypeVariableReference : FlowTypeExpression
    {
        public FlowTypeVariableReference(string name)
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