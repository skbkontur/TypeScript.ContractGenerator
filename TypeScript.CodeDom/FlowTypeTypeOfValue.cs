namespace TypeScript.CodeDom
{
    public class FlowTypeTypeOfValue : FlowTypeType
    {
        public FlowTypeExpression TargetValue { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("typeof {0}", TargetValue.GenerateCode(context));
        }
    }
}