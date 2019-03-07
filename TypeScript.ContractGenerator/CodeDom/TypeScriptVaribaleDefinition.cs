namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptVaribaleDefinition : TypeScriptStatement
    {
        public string Name { get; set; }
        public TypeScriptExpression Value { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("var {0} = {1};", Name, Value.GenerateCode(context));
        }
    }
}