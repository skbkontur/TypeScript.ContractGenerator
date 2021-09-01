namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptConstantDefinition : TypeScriptStatement
    {
        public TypeScriptConstantDefinition(string name, TypeScriptExpression value, TypeScriptType? type = null)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public string Name { get; }
        public TypeScriptExpression Value { get; }
        public TypeScriptType? Type { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var constantType = Type == null ? "" : $": {Type.GenerateCode(context)}";
            return $"const {Name}{constantType} = {Value.GenerateCode(context)};";
        }
    }
}