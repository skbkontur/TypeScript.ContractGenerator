namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptClassMemberDefinition
    {
        public string Name { get; set; }

        public TypeScriptDefinition Definition { get; set; }

        public string GenerateCode(ICodeGenerationContext context)
        {
            return Definition.GenerateCode(Name, context);
        }
    }
}