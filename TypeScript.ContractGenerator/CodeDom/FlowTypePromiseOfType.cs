namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypePromiseOfType : FlowTypeType
    {
        public FlowTypePromiseOfType()
        {
        }

        public FlowTypePromiseOfType(FlowTypeType targetType)
        {
            TargetType = targetType;
        }

        public FlowTypeType TargetType { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("Promise<{0}>", TargetType.GenerateCode(context));
        }
    }
}