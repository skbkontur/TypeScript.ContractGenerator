namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTemplateStringLiteral : TypeScriptExpression
    {
        public TypeScriptTemplateStringLiteral(string value, TypeScriptVariableReference? tag = null)
        {
            Value = value;
            Tag = tag;
        }

        public TypeScriptVariableReference? Tag { get; }
        public string Value { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var tag = Tag == null ? string.Empty : Tag.GenerateCode(context);
            return $"{tag}`{Value}`";
        }
    }
}