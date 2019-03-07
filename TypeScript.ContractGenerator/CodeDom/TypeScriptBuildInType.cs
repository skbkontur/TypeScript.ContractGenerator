namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptBuildInType : TypeScriptType
    {
        public TypeScriptBuildInType(string name)
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