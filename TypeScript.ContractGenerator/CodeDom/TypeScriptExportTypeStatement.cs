namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptExportTypeStatement : TypeScriptStatement
    {
        public TypeScriptTypeDeclaration Declaration { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"export {Declaration.GenerateCode(context)}";
        }
    }
}