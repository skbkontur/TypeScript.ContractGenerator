using System;
using System.Reflection;

namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface ITypeInfo : IAttributeProvider, IEquatable<ITypeInfo>
    {
        string Name { get; }
        string FullName { get; }
        string Namespace { get; }
        bool IsEnum { get; }
        bool IsValueType { get; }
        bool IsArray { get; }
        bool IsClass { get; }
        bool IsInterface { get; }
        bool IsAbstract { get; }
        bool IsGenericType { get; }
        bool IsGenericParameter { get; }
        bool IsGenericTypeDefinition { get; }
        ITypeInfo? BaseType { get; }

        IMethodInfo[] GetMethods(BindingFlags bindingAttr);
        IPropertyInfo[] GetProperties(BindingFlags bindingAttr);
        ITypeInfo[] GetGenericArguments();
        ITypeInfo[] GetInterfaces();
        ITypeInfo GetGenericTypeDefinition();
        ITypeInfo GetElementType();
        string[] GetEnumNames();
        bool IsAssignableFrom(ITypeInfo type);
    }
}