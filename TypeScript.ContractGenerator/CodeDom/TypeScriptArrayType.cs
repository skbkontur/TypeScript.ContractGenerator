using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptArrayType : TypeScriptType
    {
        public TypeScriptArrayType([NotNull] TypeScriptType itemType)
        {
            ItemType = itemType;
        }

        private TypeScriptType ItemType { get; }

        public override string GenerateCode(ICodeGenerationContext context)
        {
            var innerTypeCode = ItemType.GenerateCode(context);
            if (!(ItemType is TypeScriptUnionType))
                return innerTypeCode + "[]";

            return $"Array<{innerTypeCode}>";
        }
    }
}