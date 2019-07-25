using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.Extensions
{
    public static class CustomAttributeProviderExtensions
    {
        public static bool IsNameDefined([NotNull] this ICustomAttributeProvider attributeProvider, [NotNull] string name, bool inherit = true)
        {
            return GetCustomAttributes(attributeProvider).Any(x => x.GetType().Name == name);
        }
        
        private static object[] GetCustomAttributes(ICustomAttributeProvider attributeContainer)
        {
            var memberInfo = attributeContainer as MemberInfo;
            if(memberInfo != null)
            {
                return Attribute.GetCustomAttributes(memberInfo).Cast<object>().ToArray();
            }

            return attributeContainer.GetCustomAttributes(true);
        }

    }
}