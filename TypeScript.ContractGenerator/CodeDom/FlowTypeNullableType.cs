namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class FlowTypeNullableType : FlowTypeType
    {
        public FlowTypeNullableType(FlowTypeType innerType)
        {
            this.innerType = innerType;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            if (context.TypeChecker == JavaScriptTypeChecker.TypeScript)
            {
                return $"Nullable<{innerType.GenerateCode(context)}>";
            }
            return "?" + innerType.GenerateCode(context);
        }

        private readonly FlowTypeType innerType;
    }
}