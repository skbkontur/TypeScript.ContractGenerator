namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptStringLiteral : TypeScriptExpression
    {
        public TypeScriptStringLiteral()
        {
        }

        public TypeScriptStringLiteral(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("'{0}'", Value);
        }
    }
}