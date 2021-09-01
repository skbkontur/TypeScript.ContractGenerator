namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptConstantDefinition : TypeScriptStatement
    {
        public TypeScriptConstantDefinition(string name, TypeScriptExpression value, string? typeName = null)
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
            var constantType = TypeName == null ? "" : $" : {TypeName}";
            return $"const {Name}{constantType} = {Value.GenerateCode(context)};";
        }
    }
}