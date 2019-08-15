namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptNullableType : TypeScriptType, INullabilityWrapperType
    {
        public TypeScriptNullableType(TypeScriptType innerType)
        {
            InnerType = innerType;
        }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            if (context.TypeChecker == JavaScriptTypeChecker.TypeScript)
            {
                return $"Nullable<{InnerType.GenerateCode(context)}>";
            }
            return "?" + InnerType.GenerateCode(context);
        }

        public TypeScriptType InnerType { get; }
    }
}