using System;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.Abstractions
{
    public interface ITypeInfo : IAttributeProvider, IEquatable<ITypeInfo>
    {
        string Name { get; }
        string FullName { get; }
        string Namespace { get; }
        NullabilityInfo NullabilityInfo { get; }
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
        IAttributeProvider? Member { get; }

        IMethodInfo[] GetMethods(BindingFlags bindingAttr);
        IPropertyInfo[] GetProperties(BindingFlags bindingAttr);
        IFieldInfo[] GetFields(BindingFlags bindingAttr);
        ITypeInfo[] GetGenericArguments();
        ITypeInfo[] GetInterfaces();
        ITypeInfo GetGenericTypeDefinition();
        ITypeInfo GetElementType();
        ITypeInfo WithMemberInfo(IAttributeProvider memberInfo);
        string[] GetEnumNames();
        bool CanBeNull(NullabilityMode nullabilityMode);
        bool IsAssignableFrom(ITypeInfo type);
    }
}