namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypeReference : TypeScriptType
    {
        public TypeScriptTypeReference(string name)
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