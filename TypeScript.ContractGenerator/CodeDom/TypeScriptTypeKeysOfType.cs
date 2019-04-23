namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptTypeKeysOfType : TypeScriptType
    {
        public TypeScriptType TargetType { get; set; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            return $"$Keys<{TargetType.GenerateCode(context)}>";
        }
    }
}