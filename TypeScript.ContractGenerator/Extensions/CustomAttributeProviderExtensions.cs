using System;
using System.Linq;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Extensions
{
    public static class CustomAttributeProviderExtensions
    {
        public static bool IsNameDefined([NotNull] this IAttributeProvider attributeProvider, [NotNull] string name, bool inherit = true)
        {
            return GetCustomAttributes(attributeProvider).Any(x => x.GetType().Name == name);
        }

        private static object[] GetCustomAttributes(IAttributeProvider attributeContainer)
        {
            if (attributeContainer is IPropertyInfo propertyInfo)
                return Attribute.GetCustomAttributes(propertyInfo.Property).Cast<object>().ToArray();
            if (attributeContainer is IMethodInfo methodInfo)
                return Attribute.GetCustomAttributes(methodInfo.Method).Cast<object>().ToArray();

            return attributeContainer.GetCustomAttributes(true);
        }
    }
}