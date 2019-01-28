namespace TypeScript.CodeDom
{
    public class FlowTypeVaribaleDefinition : FlowTypeStatement
    {
        public string Name { get; set; }
        public FlowTypeExpression Value { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("var {0} = {1};", Name, Value.GenerateCode(context));
        }
    }
}