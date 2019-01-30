namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
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