namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptStringLiteralType : TypeScriptType
    {
        public TypeScriptStringLiteralType(string value)
        {
            this.value = value;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"'{value}'";
        }

        private readonly string value;
    }
}