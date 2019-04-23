namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypeSpreadMemberDeclaration : TypeScriptTypeMemberDeclarationBase
    {
        public TypeScriptTypeSpreadMemberDeclaration(TypeScriptType type)
        {
            Type = type;
        }

        public TypeScriptType Type { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"...{Type.GenerateCode(context)};";
        }
    }
}