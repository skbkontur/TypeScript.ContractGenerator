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
            return string.Format("'{0}'", value);
        }

        private readonly string value;
    }
}