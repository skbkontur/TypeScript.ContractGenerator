namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptClassDeclaration : TypeScriptStatement
    {
        public string Name { get; set; }

        public TypeScriptClassDefinition Defintion { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return Defintion.GenerateBody(Name, context) + ";";
        }
    }
}