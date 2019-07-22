namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptOrNullType : TypeScriptUnionType, INullabilityWrapperType
    {
        public TypeScriptOrNullType(TypeScriptType innerType)
            : base(new[]
                {
                    new TypeScriptBuildInType("null"),
                    innerType
                })
        {
            InnerType = innerType;
        }

        public TypeScriptType InnerType { get; }
    }
}