using System.Threading.Tasks;

#nullable disable

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class MethodRootType
    {
        [CanBeNull]
        public object Get([CanBeNull] string s, [NotNull] string nns)
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
}