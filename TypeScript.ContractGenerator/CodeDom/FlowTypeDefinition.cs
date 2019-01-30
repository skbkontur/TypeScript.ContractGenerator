namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public abstract class FlowTypeDefinition
    {
        public abstract string GenerateCode(string name, ICodeGenerationContext context);
    }
}