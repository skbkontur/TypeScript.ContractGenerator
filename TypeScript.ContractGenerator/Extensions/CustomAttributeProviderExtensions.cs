using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.Extensions
{
    public static class CustomAttributeProviderExtensions
    {
        public static bool IsNameDefined([NotNull] this ICustomAttributeProvider attributeProvider, [NotNull] string name, bool inherit = true)
        {
            return attributeProvider.GetCustomAttributes(inherit).Any(a => a.GetType().Name == name);
        }
    }
}