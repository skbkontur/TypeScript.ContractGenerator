using System.Threading.Tasks;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class MethodRootType
    {
        [CanBeNull]
        public object Get([CanBeNull] string s)
        {
            return null;
        }

        [NotNull]
        public object GetNotNull([NotNull] string s)
        {
            return null;
        }

        [NotNull, ItemNotNull]
        public async Task<object> GetAsync([NotNull] string s)
        {
            return await Task.FromResult(new CommonType()).ConfigureAwait(false);
        }

        [CanBeNull]
        public async Task PostAsync([CanBeNull] string s)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        [NotNull]
        public string String { get; set; }

        [CanBeNull]
        public string NullableString { get; set; }
    }

#nullable enable

    public class NullableReferenceMethodType
    {
        [NotNull]
        public object? Get([NotNull] string? s, [CanBeNull] string nns)
        {
            return null;
        }

        [CanBeNull]
        public object GetNotNull(string s)
        {
            return null;
        }

        [CanBeNull]
        public async Task<object> GetAsync([CanBeNull] string s)
        {
            return await Task.FromResult(new CommonType()).ConfigureAwait(false);
        }

        [NotNull]
        public async Task? PostAsync([NotNull] string? s)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        [CanBeNull]
        public string String { get; set; }

        [NotNull]
        public string? NullableString { get; set; }
    }

#nullable disable
}