using System;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

using NullabilityInfo = SkbKontur.TypeScript.ContractGenerator.Internals.NullabilityInfo;
using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class RoslynTypeInfo : ITypeInfo
    {
        private RoslynTypeInfo(ITypeSymbol typeSymbol)
        {
            TypeSymbol = typeSymbol;
            NullabilityInfo = NullabilityInfo.Empty;
        }

        private RoslynTypeInfo(ITypeSymbol typeSymbol, NullabilityInfo nullabilityInfo)
        {
            TypeSymbol = typeSymbol;
            NullabilityInfo = nullabilityInfo;
        }

        private RoslynTypeInfo(ITypeSymbol typeSymbol, IAttributeProvider memberInfo)
        {
            TypeSymbol = typeSymbol;
            Member = memberInfo;
            NullabilityInfo = NullabilityInfo.FromRoslyn(memberInfo);
        }

        public static ITypeInfo From(ITypeSymbol typeSymbol)
        {
            return typeSymbol == null ? null : new RoslynTypeInfo(typeSymbol);
        }

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return TypeSymbol.GetAttributesInfo();
        }

        public ITypeSymbol TypeSymbol { get; }
        public NullabilityInfo NullabilityInfo { get; }

        public string Name => TypeSymbol.MetadataName;
        public string FullName => TypeSymbol.Name;
        public string Namespace => IsArray ? "System" : TypeSymbol.ContainingNamespace?.ToString();
        public bool IsEnum => BaseType != null && BaseType.Equals(TypeInfo.From<Enum>());
        public bool IsValueType => IsEnum || TypeSymbol.IsValueType;
        public bool IsArray => TypeSymbol.TypeKind == TypeKind.Array;
        public bool IsClass => !IsEnum && (TypeSymbol.TypeKind == TypeKind.Class || TypeSymbol.TypeKind == TypeKind.Array || TypeSymbol.TypeKind == TypeKind.TypeParameter);
        public bool IsInterface => TypeSymbol.TypeKind == TypeKind.Interface;
        public bool IsAbstract => TypeSymbol.IsAbstract;
        public bool IsGenericType => TypeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType;
        public bool IsGenericParameter => TypeSymbol.TypeKind == TypeKind.TypeParameter;
        public bool IsGenericTypeDefinition => IsGenericType && TypeSymbol.IsDefinition;
        public ITypeInfo BaseType => From(TypeSymbol.BaseType);
        public IAttributeProvider? Member { get; }
        public IAssemblyInfo Assembly => new RoslynAssemblyInfo(TypeSymbol.ContainingAssembly);

        public IMethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            var methods = TypeSymbol.GetMembers()
                                    .OfType<IMethodSymbol>()
                                    .Where(x => x.Name != ".ctor")
                                    .Where(x => !bindingAttr.HasFlag(BindingFlags.Public) || x.DeclaredAccessibility == Accessibility.Public)
                                    .Where(x => !bindingAttr.HasFlag(BindingFlags.Instance) || !x.IsStatic)
                                    .Select(x => (IMethodInfo)new RoslynMethodInfo(x))
                                    .ToArray();

            if (!bindingAttr.HasFlag(BindingFlags.DeclaredOnly) && BaseType != null)
                methods = methods.Concat(BaseType.GetMethods(bindingAttr).Where(x => methods.All(t => t.Name != x.Name))).ToArray();

            return methods;
        }

        public IPropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            var types = TypeSymbol.GetMembers()
                                  .OfType<IPropertySymbol>()
                                  .Where(x => !bindingAttr.HasFlag(BindingFlags.Public) || x.DeclaredAccessibility == Accessibility.Public)
                                  .Where(x => !bindingAttr.HasFlag(BindingFlags.Instance) || !x.IsStatic)
                                  .Select(x => (IPropertyInfo)new RoslynPropertyInfo(x))
                                  .ToArray();

            if (!bindingAttr.HasFlag(BindingFlags.DeclaredOnly) && BaseType != null)
                types = types.Concat(BaseType.GetProperties(bindingAttr).Where(x => types.All(t => t.Name != x.Name))).ToArray();

            return types;
        }

        public IFieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            var fields = TypeSymbol.GetMembers()
                                   .OfType<IFieldSymbol>()
                                   .Where(x => !bindingAttr.HasFlag(BindingFlags.Public) || x.DeclaredAccessibility == Accessibility.Public)
                                   .Where(x => !bindingAttr.HasFlag(BindingFlags.Instance) || !x.IsStatic)
                                   .Select(x => (IFieldInfo)new RoslynFieldInfo(x))
                                   .ToArray();

            if (!bindingAttr.HasFlag(BindingFlags.DeclaredOnly) && BaseType != null)
                fields = fields.Concat(BaseType.GetFields(bindingAttr).Where(x => fields.All(t => t.Name != x.Name))).ToArray();

            return fields;
        }

        public ITypeInfo[] GetGenericArguments()
        {
            if (!(TypeSymbol is INamedTypeSymbol namedTypeSymbol))
                return new ITypeInfo[0];

            if (!NullabilityInfo.IsEmpty() && this.HasItem())
                return new ITypeInfo[] {new RoslynTypeInfo(namedTypeSymbol.TypeArguments[0], NullabilityInfo.ForItem())};

            return namedTypeSymbol.TypeArguments.Select(From).ToArray();
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
                return new RoslynTypeInfo(arrayTypeSymbol.ElementType, NullabilityInfo.ForItem());
            return null;
        }

        public ITypeInfo WithMemberInfo(IAttributeProvider memberInfo)
        {
            return new RoslynTypeInfo(TypeSymbol, memberInfo);
        }

        public string[] GetEnumNames()
        {
            if (TypeSymbol is INamedTypeSymbol namedTypeSymbol)
                return namedTypeSymbol.GetMembers().Select(x => x.Name).Where(x => x != ".ctor" && x != "value__").ToArray();
            return new string[0];
        }

        public bool CanBeNull(NullabilityMode nullabilityMode)
        {
            if (this.NeverNull())
                return false;

            if (!nullabilityMode.HasFlag(NullabilityMode.NullableReference) || TypeSymbol.NullableAnnotation == NullableAnnotation.None)
                return NullabilityInfo.CanBeNull(nullabilityMode);

            return TypeSymbol.NullableAnnotation == NullableAnnotation.Annotated;
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