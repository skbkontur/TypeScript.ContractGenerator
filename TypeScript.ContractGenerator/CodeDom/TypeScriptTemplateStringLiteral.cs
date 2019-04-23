namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTemplateStringLiteral : TypeScriptExpression
    {
        public TypeScriptTemplateStringLiteral(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"`{Value}`";
        }
    }
}