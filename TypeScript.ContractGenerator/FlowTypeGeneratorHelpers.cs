using System;
using System.Linq;
using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public static class FlowTypeGeneratorHelpers
    {
        public static (bool, Type) ProcessNullable(ICustomAttributeProvider attributeContainer, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = type.GetGenericArguments()[0];
                return (true, underlyingType);
            }
            if (attributeContainer != null && type.IsClass && attributeContainer.GetCustomAttributes(true).All(x => x.GetType().Name != "NotNullAttribute"))
                return (true, type);
            return (false, type);
        }
    }
}