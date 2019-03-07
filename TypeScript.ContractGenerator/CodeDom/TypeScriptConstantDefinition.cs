namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptConstantDefinition : TypeScriptStatement
    {
        public string Name { get; set; }
        public TypeScriptExpression Value { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("const {0} = {1};", Name, Value.GenerateCode(context));
        }
    }
}