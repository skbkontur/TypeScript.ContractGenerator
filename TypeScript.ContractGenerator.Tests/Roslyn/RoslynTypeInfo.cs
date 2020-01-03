using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Roslyn
{
    public class RoslynTypeInfo : ITypeInfo
    {
        public RoslynTypeInfo(ITypeSymbol typeSymbol)
        {
            this.typeSymbol = typeSymbol;
        }

        public bool IsNameDefined(string name)
        {
            return typeSymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public Type Type { get; }
        public string Name => typeSymbol.MetadataName;
        public string FullName => typeSymbol.Name;
        public string Namespace => typeSymbol.ContainingNamespace?.ToString();
        public bool IsEnum => typeSymbol.TypeKind == TypeKind.Enum;
        public bool IsValueType => typeSymbol.IsValueType;
        public bool IsArray => typeSymbol.TypeKind == TypeKind.Array;
        public bool IsClass => typeSymbol.TypeKind == TypeKind.Class || typeSymbol.TypeKind == TypeKind.Array || typeSymbol.TypeKind == TypeKind.TypeParameter;
        public bool IsInterface => typeSymbol.TypeKind == TypeKind.Interface;
        public bool IsAbstract => typeSymbol.IsAbstract;
        public bool IsGenericType => typeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType;
        public bool IsGenericParameter => typeSymbol.TypeKind == TypeKind.TypeParameter;
        public bool IsGenericTypeDefinition => IsGenericType && typeSymbol.IsDefinition;
        public ITypeInfo BaseType => null;

        public IMethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return typeSymbol.GetMembers()
                             .OfType<IMethodSymbol>()
                             .Where(x => !bindingAttr.HasFlag(BindingFlags.Public) || x.DeclaredAccessibility == Accessibility.Public)
                             .Select(x => (IMethodInfo)new RoslynMethodInfo(x))
                             .ToArray();
        }

        public IPropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return typeSymbol.GetMembers()
                             .OfType<IPropertySymbol>()
                             .Where(x => !bindingAttr.HasFlag(BindingFlags.Public) || x.DeclaredAccessibility == Accessibility.Public)
                             .Select(x => (IPropertyInfo)new RoslynPropertyInfo(x))
                             .ToArray();
        }

        public ITypeInfo[] GetGenericArguments()
        {
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                return namedTypeSymbol.TypeArguments.Select(x => (ITypeInfo)new RoslynTypeInfo(x)).ToArray();
            return new ITypeInfo[0];
        }

        public ITypeInfo[] GetInterfaces()
        {
            throw new NotImplementedException();
        }

        public ITypeInfo GetGenericTypeDefinition()
        {
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                return new RoslynTypeInfo(namedTypeSymbol.OriginalDefinition);
            return null;
        }

        public ITypeInfo GetElementType()
        {
            if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
                return new RoslynTypeInfo(arrayTypeSymbol.ElementType);
            return null;
        }

        public string[] GetEnumNames()
        {
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
                return namedTypeSymbol.MemberNames.OrderBy(x => x).ToArray();
            return new string[0];
        }

        public bool IsAssignableFrom(ITypeInfo type)
        {
            throw new NotImplementedException();
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
            return typeSymbol.ToString();
        }

        private readonly ITypeSymbol typeSymbol;
    }
}