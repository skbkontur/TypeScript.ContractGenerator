namespace SkbKontur.TypeScript.ContractGenerator.Cli.Utils
{
    internal static class CollectionValidationExtensions
    {
        public static CollectionValidator<T> StartCollectionValidator<T>(this T[] items) where T : class
        {
            return new CollectionValidator<T>(items);
        }
    }
}