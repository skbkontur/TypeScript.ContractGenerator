namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptExportStatement : TypeScriptStatement
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            return "export " + Declaration.GenerateCode(context);
        }

        public TypeScriptStatement Declaration { get; set; }
    }
}