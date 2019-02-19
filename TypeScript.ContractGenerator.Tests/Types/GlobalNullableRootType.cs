namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class GlobalNullableRootType
    {
        public int Int { get; set; }
        public int? NullableInt { get; set; }
        public int?[] NullableInts { get; set; }
        public GenericClass<int> IntGeneric { get; set; }
        public GenericClass<int?> NullableIntGeneric { get; set; }
    }

    public class GenericClass<T>
    {
        public T GenericType { get; set; }
    }
}