using System;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class TestCustomPropertyResolver : ICustomTypeGenerator
    {
        public string GetTypeLocation(ITypeInfo type)
        {
            return "";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, ITypeGenerator typeGenerator, ITypeInfo type, ITypeScriptUnitFactory unitFactory)
        {
            return null;
        }

        public TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            var type = ((TypeInfo)typeInfo).Type;
            var property = ((PropertyWrapper)propertyInfo).Property;
            if (type == typeof(EnumWithConstGetterContainingRootType) && property.PropertyType.IsEnum && !property.CanWrite)
            {
                return new TypeScriptTypeMemberDeclaration
                    {
                        Name = property.Name.ToLowerCamelCase(),
                        Optional = false,
                        Type = new TypeScriptStringLiteralType(property.GetMethod.Invoke(Activator.CreateInstance(type), null).ToString()),
                    };
            }
            return null;
        }
    }
}