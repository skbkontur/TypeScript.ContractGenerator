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
            return $"Nullable<{InnerType.GenerateCode(context)}>";
        }

        public TypeScriptType InnerType { get; }
    }
}