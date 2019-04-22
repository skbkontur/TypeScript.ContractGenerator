namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptRequireStatement : TypeScriptStatement
    {
        public TypeScriptStringLiteral Path { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"require({Path.GenerateCode(context)})";
        }
    }
}