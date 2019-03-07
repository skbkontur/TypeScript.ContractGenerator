namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTemplateStringLiteral : TypeScriptExpression
    {
        public TypeScriptTemplateStringLiteral(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("`{0}`", Value);
        }
    }
}