using System;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Extensions
{
    public static class AttributeProviderExtensions
    {
        public static T[] GetCustomAttributes<T>(this IAttributeProvider attributeProvider)
            where T : Attribute
        {
            return attributeProvider.GetCustomAttributes(true).Where(x => x.GetType() == typeof(T)).Cast<T>().ToArray();
        }
    }
}