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
            // todo хочу сюда для каждого inner типа [ 0 , 1 , 0 ]
        }
        
        

        public TypeScriptType InnerType { get; }
    }
}