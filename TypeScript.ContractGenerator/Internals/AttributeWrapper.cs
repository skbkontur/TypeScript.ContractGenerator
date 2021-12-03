using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class AttributeWrapper : IAttributeInfo
    {
        public AttributeWrapper(object attribute)
        {
            Attribute = attribute;
            AttributeType = TypeInfo.From(attribute.GetType());
            AttributeData = attribute.GetType()
                                     .GetMembers()
                                     .Where(x => x.MemberType == MemberTypes.Field || x.MemberType == MemberTypes.Property)
                                     .ToDictionary(x => x.Name, x => Wrap(GetValue(x, attribute)));
        }

        public object Attribute { get; }

        public ITypeInfo AttributeType { get; }
        public Dictionary<string, object?> AttributeData { get; }

        private static object? GetValue(MemberInfo memberInfo, object attribute)
        {
            try
            {
                return memberInfo switch
                    {
                        FieldInfo field => field.GetValue(attribute),
                        PropertyInfo property => property.GetValue(attribute),
                        _ => null
                    };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static object? Wrap(object? value)
        {
            if (value is Type type)
                return TypeInfo.From(type);
            return value;
        }
    }
}