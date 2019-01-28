namespace TypeScript.CodeDom
{
    public class FlowTypeThisReference : FlowTypeExpression
    {
        public override string GenerateCode(ICodeGenerationContext context)
        {
            return "this";
        }
    }
}