namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class ExplicitNullabilityRootType
    {
        [NotNull]
        public SomeClass SomeNotNullClass { get; set; }

        public SomeClass SomeNullableClass { get; set; }

        [NotNull]
        public string NotNullString { get; set; }

        public string NullableString { get; set; }

        public int NotNullInt { get; set; }

        public int? NullableInt { get; set; }

        [NotNull]
        public int[] NotNullArray { get; set; }

        public int[] NullableArray { get; set; }

        [NotNull]
        public int?[] NotNullNullablesArray { get; set; }

        public int?[] NullableNullablesArray { get; set; }
    }
}
