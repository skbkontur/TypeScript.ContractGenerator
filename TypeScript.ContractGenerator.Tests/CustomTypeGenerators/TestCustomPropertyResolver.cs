using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class TestCustomPropertyResolver : ICustomTypeGenerator
    {
        public string GetTypeLocation(Type type)
        {
            return "";
        }

        public ITypeBuildingContext ResolveType(string initialUnitPath, Type type, ITypeScriptUnitFactory unitFactory)
        {
            return null;
        }

        public TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, Type type, PropertyInfo property)
        {
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