using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptPromiseOfType : TypeScriptType
    {
        public TypeScriptPromiseOfType([NotNull] TypeScriptType targetType)
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