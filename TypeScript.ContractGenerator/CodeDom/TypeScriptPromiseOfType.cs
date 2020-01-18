namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptPromiseOfType : TypeScriptType
    {
        public TypeScriptPromiseOfType(TypeScriptType targetType)
        {
            TargetType = targetType;
        }

        public TypeScriptType TargetType { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"Promise<{TargetType.GenerateCode(context)}>";
        }
    }
}