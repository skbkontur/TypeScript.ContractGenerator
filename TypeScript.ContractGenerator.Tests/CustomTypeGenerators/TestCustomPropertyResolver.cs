using System;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Roslyn;
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

        public TypeScriptTypeMemberDeclaration? ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            if (!typeInfo.Equals(TypeInfo.From<EnumWithConstGetterContainingRootType>()) || !propertyInfo.PropertyType.IsEnum)
                return null;

            var value = typeInfo is TypeInfo
                            ? GetValueFromPropertyInfo(typeInfo, propertyInfo)
                            : GetValueFromPropertySymbol(typeInfo, propertyInfo);
            if (!string.IsNullOrEmpty(value))
            {
                return new TypeScriptTypeMemberDeclaration
                    {
                        Name = propertyInfo.Name.ToLowerCamelCase(),
                        Optional = false,
                        Type = new TypeScriptStringLiteralType(value),
                    };
            }

            return null;
        }

        private static string? GetValueFromPropertyInfo(ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            var type = ((TypeInfo)typeInfo).Type;
            var property = ((PropertyWrapper)propertyInfo).Property;
            if (!property.CanWrite)
                return property.GetMethod.Invoke(Activator.CreateInstance(type), null).ToString();
            return null;
        }

        private static string? GetValueFromPropertySymbol(ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            var property = ((RoslynPropertyInfo)propertyInfo).PropertySymbol;
            if (property.SetMethod != null)
                return null;

            var syntaxNode = property.GetMethod.DeclaringSyntaxReferences.Single().GetSyntax();
            if (syntaxNode is ArrowExpressionClauseSyntax arrowExpression && arrowExpression.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                if (memberAccess.Expression is IdentifierNameSyntax identifier && identifier.Identifier.Text == propertyInfo.PropertyType.Name)
                    return memberAccess.Name.Identifier.ToString();
            }

            return null;
        }
    }
}