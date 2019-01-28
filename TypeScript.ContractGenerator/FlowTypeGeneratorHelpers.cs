using System;
using System.Linq;
using System.Reflection;

namespace TypeScript.ContractGenerator
{
    public static class FlowTypeGeneratorHelpers
    {
        public static (bool, Type) ProcessNullable(ICustomAttributeProvider attributeContainer, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var undelayingType = type.GetGenericArguments()[0];
                return (true, undelayingType);
            }
            if (attributeContainer != null && type.IsClass && attributeContainer.GetCustomAttributes(true).All(x => x.GetType().Name != "NotNullAttribute"))
                return (true, type);
            return (false, type);
        }
    }
}