namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptInterfacePropertyMember : TypeScriptInterfaceMember
    {
        public string Name { get; set; }
        public TypeScriptType Result { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{Name}: {Result.GenerateCode(context)}";
        }
    }
}