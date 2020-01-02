using System;

namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface ITypeInfo : IAttributeProvider, IEquatable<ITypeInfo>
    {
        Type Type { get; }

        string Name { get; }
        string FullName { get; }
        string Namespace { get; }
        bool IsEnum { get; }
        bool IsValueType { get; }
        bool IsArray { get; }
        bool IsAbstract { get; }
        bool IsGenericType { get; }
        bool IsGenericParameter { get; }
        bool IsGenericTypeDefinition { get; }
        ITypeInfo BaseType { get; }

        IMethodInfo[] GetMethods();
        IPropertyInfo[] GetProperties();
        ITypeInfo[] GetGenericArguments();
        ITypeInfo GetGenericTypeDefinition();
        ITypeInfo GetElementType();
    }
}