namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptObjectLiteralProperty : TypeScriptObjectLiteralInitializer
    {
        public TypeScriptObjectLiteralProperty(TypeScriptExpression name, TypeScriptExpression value)
        {
            Name = name;
            Value = value;
        }

        public TypeScriptExpression Name { get; }
        public TypeScriptExpression Value { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"[{Name.GenerateCode(context)}]: {Value.GenerateCode(context)}";
        }
    }
}