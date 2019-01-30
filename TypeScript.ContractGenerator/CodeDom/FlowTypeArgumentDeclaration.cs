namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeArgumentDeclaration
    {
        public string Name { get; set; }
        public FlowTypeType Type { get; set; }

        public string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("{0}: {1}", Name, Type.GenerateCode(context));
        }
    }
}