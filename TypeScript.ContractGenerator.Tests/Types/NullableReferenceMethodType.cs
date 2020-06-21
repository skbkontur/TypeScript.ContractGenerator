using System.Threading.Tasks;

// note (p.vostretsov, 27.05.2020): без nullable enable тест RoslynTests.CustomGeneratorWithMethodsTest не проходит,
// так как мы скормили рослину отдельные файлы и про настройки проекта он не знает
#nullable enable

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class NullableReferenceMethodType
    {
        [NotNull]
        public object? Get([NotNull] string? s, [CanBeNull] string nns)
        {
            return null;
        }

        [CanBeNull]
        public object GetNotNull([CanBeNull] string s)
        {
            return null!;
        }

        [CanBeNull, ItemCanBeNull]
        public async Task<object?> GetAsync([CanBeNull] string s)
        {
            return await Task.FromResult(new object()).ConfigureAwait(false);
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
}