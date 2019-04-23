namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class NullabilityModeRootType
    {
        [NotNull]
        public string NotNullString { get; set; }

        [CanBeNull]
        public string CanBeNullString { get; set; }

        public string MaybeNotNullString { get; set; }
    }
}