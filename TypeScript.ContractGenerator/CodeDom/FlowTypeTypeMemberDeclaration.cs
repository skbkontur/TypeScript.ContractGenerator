namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeTypeMemberDeclaration : FlowTypeTypeMemberDeclarationBase
    {
        public string Name { get; set; }
        public FlowTypeType Type { get; set; }
        public bool Optional { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return Name + (Optional ? "?" : "") + ": " + Type.GenerateCode(context) + ";";
        }
    }
}