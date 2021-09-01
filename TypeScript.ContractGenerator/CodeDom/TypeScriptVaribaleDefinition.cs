namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptVariableDefinition : TypeScriptStatement
    {
        public TypeScriptVariableDefinition(string name, TypeScriptExpression value, string? typeName = null)
        {
            Name = name;
            Value = value;
            TypeName = typeName;
        }

        public string Name { get; }
        public TypeScriptExpression Value { get; }
        public string? TypeName { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var variableType = TypeName == null ? "" : $" : {TypeName}";
            return $"var {Name}{variableType} = {Value.GenerateCode(context)};";
        }
    }
}