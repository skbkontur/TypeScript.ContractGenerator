namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptVaribaleDefinition : TypeScriptStatement
    {
        public TypeScriptVaribaleDefinition(string name, TypeScriptExpression value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public TypeScriptExpression Value { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"var {Name} = {Value.GenerateCode(context)};";
        }
    }
}