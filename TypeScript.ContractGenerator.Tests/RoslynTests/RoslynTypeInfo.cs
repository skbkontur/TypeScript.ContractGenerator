using System;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.RoslynTests
{
    public class RoslynTypeInfo : ITypeInfo
    {
        private RoslynTypeInfo(ITypeSymbol typeSymbol)
        {
            TypeSymbol = typeSymbol;
        }

        public static ITypeInfo From(ITypeSymbol typeSymbol)
        {
            return typeSymbol == null ? null : new RoslynTypeInfo(typeSymbol);
        }

        public bool IsNameDefined(string name)
        {
            return TypeSymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public ITypeSymbol TypeSymbol { get; }

        public string Name => TypeSymbol.MetadataName;
        public string FullName => TypeSymbol.Name;
        public string Namespace => TypeSymbol.ContainingNamespace?.ToString();
        public bool IsEnum => TypeSymbol.TypeKind == TypeKind.Enum;
        public bool IsValueType => TypeSymbol.IsValueType;
        public bool IsArray => TypeSymbol.TypeKind == TypeKind.Array;
        public bool IsClass => TypeSymbol.TypeKind == TypeKind.Class || TypeSymbol.TypeKind == TypeKind.Array || TypeSymbol.TypeKind == TypeKind.TypeParameter;
        public bool IsInterface => TypeSymbol.TypeKind == TypeKind.Interface;
        public bool IsAbstract => TypeSymbol.IsAbstract;
        public bool IsGenericType => TypeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType;
        public bool IsGenericParameter => TypeSymbol.TypeKind == TypeKind.TypeParameter;
        public bool IsGenericTypeDefinition => IsGenericType && TypeSymbol.IsDefinition;
        public ITypeInfo BaseType => From(TypeSymbol.BaseType);

        public IMethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return TypeSymbol.GetMembers()
                             .OfType<IMethodSymbol>()
                             .Where(x => !bindingAttr.HasFlag(BindingFlags.Public) || x.DeclaredAccessibility == Accessibility.Public)
                             .Select(x => (IMethodInfo)new RoslynMethodInfo(x))
                             .ToArray();
        }

        public IPropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return TypeSymbol.GetMembers()
                             .OfType<IPropertySymbol>()
                             .Where(x => !bindingAttr.HasFlag(BindingFlags.Public) || x.DeclaredAccessibility == Accessibility.Public)
                             .Select(x => (IPropertyInfo)new RoslynPropertyInfo(x))
                             .ToArray();
        }

        public IFieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return TypeSymbol.GetMembers()
                             .OfType<IFieldSymbol>()
                             .Where(x => !bindingAttr.HasFlag(BindingFlags.Public) || x.DeclaredAccessibility == Accessibility.Public)
                             .Select(x => (IFieldInfo)new RoslynFieldInfo(x))
                             .ToArray();
        }

        public ITypeInfo[] GetGenericArguments()
        {
            if (TypeSymbol is INamedTypeSymbol namedTypeSymbol)
                return namedTypeSymbol.TypeArguments.Select(From).ToArray();
            return new ITypeInfo[0];
        }

        public ITypeInfo[] GetInterfaces()
        {
            return TypeSymbol.AllInterfaces.Select(From).ToArray();
        }

        public ITypeInfo GetGenericTypeDefinition()
        {
            if (TypeSymbol is INamedTypeSymbol namedTypeSymbol)
                return new RoslynTypeInfo(namedTypeSymbol.OriginalDefinition);
            return null;
        }

        public ITypeInfo GetElementType()
        {
            if (TypeSymbol is IArrayTypeSymbol arrayTypeSymbol)
                return new RoslynTypeInfo(arrayTypeSymbol.ElementType);
            return null;
        }

        public string[] GetEnumNames()
        {
            if (TypeSymbol is INamedTypeSymbol namedTypeSymbol)
                return namedTypeSymbol.MemberNames.OrderBy(x => x).ToArray();
            return new string[0];
        }

        public bool IsAssignableFrom(ITypeInfo type)
        {
            return TypeInfoHelpers.IsAssignableFrom(this, type);
        }

        public bool Equals(ITypeInfo other)
        {
            return TypeInfoHelpers.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj is ITypeInfo typeInfo && Equals(typeInfo);
        }

        public override int GetHashCode()
        {
            return TypeInfoHelpers.GetHashCode(this);
        }

        public override string ToString()
        {
            return TypeSymbol.ToString();
        }
    }
}