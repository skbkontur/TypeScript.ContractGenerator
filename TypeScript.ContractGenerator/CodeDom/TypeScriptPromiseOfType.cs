namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptPromiseOfType : TypeScriptType
    {
        public TypeScriptPromiseOfType()
        {
        }

        public TypeScriptPromiseOfType(TypeScriptType targetType)
        {
            TargetType = targetType;
        }

        public TypeScriptType TargetType { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return string.Format("Promise<{0}>", TargetType.GenerateCode(context));
        }
    }
}