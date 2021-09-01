namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptVariableDefinition : TypeScriptStatement
    {
        public TypeScriptVariableDefinition(string name, TypeScriptExpression value, TypeScriptType? type = null)
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
            var variableType = Type == null ? "" : $": {Type.GenerateCode(context)}";
            return $"var {Name}{variableType} = {Value.GenerateCode(context)};";
        }
    }
}