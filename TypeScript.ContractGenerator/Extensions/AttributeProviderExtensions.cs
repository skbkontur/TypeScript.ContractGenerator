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
    }
}