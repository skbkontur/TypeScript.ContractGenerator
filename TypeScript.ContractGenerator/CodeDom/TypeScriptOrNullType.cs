namespace SkbKontur.TypeScript.ContractGenerator.CodeDom
{
    public class TypeScriptOrNullType : TypeScriptUnionType
    {
        public TypeScriptOrNullType(TypeScriptType innerType)
            : base(new[]
                {
                    new TypeScriptBuildInType("null"),
                    innerType
                })
        {
        }
    }
}