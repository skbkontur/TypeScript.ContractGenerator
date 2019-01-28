namespace TypeScript.ContractGenerator.Tests.Types
{
    public class EnumContainingRootType
    {
        public DefaultEnum DefaultEnum { get; set; }
        public DefaultEnum? NullableEnum { get; set; }
        public ExplicitEnum ExplicitEnum { get; set; }
    }

    public enum DefaultEnum
    {
        A,
        B,
    }

    public enum ExplicitEnum
    {
        C = 3,
        D = 4,
    }
}