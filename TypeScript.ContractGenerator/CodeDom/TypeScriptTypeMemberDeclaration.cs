namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypeMemberDeclaration : TypeScriptTypeMemberDeclarationBase
    {
        public string Name { get; set; }
        public TypeScriptType Type { get; set; }
        public bool Optional { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return Name + (Optional ? "?" : "") + ": " + Type.GenerateCode(context) + ";";
        }
    }
}