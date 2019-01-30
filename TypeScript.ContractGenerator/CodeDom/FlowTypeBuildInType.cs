namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeBuildInType : FlowTypeType
    {
        public FlowTypeBuildInType(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return Name;
        }
    }
}