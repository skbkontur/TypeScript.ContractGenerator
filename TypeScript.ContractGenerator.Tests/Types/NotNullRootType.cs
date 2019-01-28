namespace TypeScript.ContractGenerator.Tests.Types
{
    public class NotNullRootType
    {
        [NotNull]
        public SomeClass SomeNotNullClass { get; set; }

        public SomeClass SomeNullableClass { get; set; }

        [NotNull]
        public string NotNullString { get; set; }
    }

    public class SomeClass
    {
        public int A { get; set; }
    }
}