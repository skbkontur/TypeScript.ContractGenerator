namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class GenericRootType<TType>
        where TType : CustomTypeBase
    {
        public TType Type { get; set; }
    }

    public class CustomTypeBase
    {
        public int A { get; set; }
    }

    public class CustomType : CustomTypeBase
    {
        public int B { get; set; }
    }
}