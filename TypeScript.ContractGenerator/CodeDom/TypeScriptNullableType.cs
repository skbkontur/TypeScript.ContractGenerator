namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptNullableType : TypeScriptType
    {
        public TypeScriptNullableType(TypeScriptType innerType)
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

        private readonly TypeScriptType innerType;
    }
}