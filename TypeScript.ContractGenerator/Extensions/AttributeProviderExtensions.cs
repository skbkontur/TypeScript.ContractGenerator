using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Extensions
{
    public static class AttributeProviderExtensions
    {
        public static IAttributeInfo[] GetAttributes(this IAttributeProvider attributeProvider, ITypeInfo attributeType)
        {
            return attributeProvider.GetAttributes(true).Where(x => x.AttributeType.Equals(attributeType)).ToArray();
        }

        public static bool IsNameDefined(this IAttributeProvider attributeProvider, string name)
        {
            return attributeProvider.GetAttributes(true).Any(x => x.AttributeType.Name == name);
        }

        public static T GetValue<T>(this IAttributeInfo attributeInfo, string key, T defaultValue = default)
        {
            var data = attributeInfo.AttributeData;
            if (data.TryGetValue(key, out var value) || data.TryGetValue(key.ToLowerCamelCase(), out value))
                return (T)value;
            return defaultValue;
        }
    }
}