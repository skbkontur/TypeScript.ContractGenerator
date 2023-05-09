using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders.ApiController
{
    public static class AttributeInfoExtensions
    {
        public static bool HasName(this IAttributeInfo attributeInfo, string name)
        {
            var attributeName = attributeInfo.AttributeType.Name;
            return attributeName == name || attributeName == $"{name}Attribute";
        }
    }
}