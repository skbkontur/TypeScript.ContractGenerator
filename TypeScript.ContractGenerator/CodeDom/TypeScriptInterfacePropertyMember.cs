using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptInterfacePropertyMember : TypeScriptInterfaceMember
    {
        public TypeScriptInterfacePropertyMember([NotNull] string name, [NotNull] TypeScriptType result)
        {
            Name = name;
            Result = result;
        }

        public string Name { get; }
        public TypeScriptType Result { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"{Name}: {Result.GenerateCode(context)}";
        }
    }
}