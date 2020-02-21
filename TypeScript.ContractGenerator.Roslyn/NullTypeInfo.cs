using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public class NullTypeInfo : ITypeInfo
    {
        public NullTypeInfo(string name)
        {
        }

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            throw new System.NotImplementedException();
        }

        public bool Equals(ITypeInfo other)
        {
            throw new System.NotImplementedException();
        }

        public string Name { get; }
        public string FullName { get; }
        public string Namespace { get; }
        public bool IsEnum { get; }
        public bool IsValueType { get; }
        public bool IsArray { get; }
        public bool IsClass { get; }
        public bool IsInterface { get; }
        public bool IsAbstract { get; }
        public bool IsGenericType { get; }
        public bool IsGenericParameter { get; }
        public bool IsGenericTypeDefinition { get; }
        public ITypeInfo? BaseType { get; }

        public IMethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            throw new System.NotImplementedException();
        }

        public IPropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            throw new System.NotImplementedException();
        }

        public IFieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            throw new System.NotImplementedException();
        }

        public ITypeInfo[] GetGenericArguments()
        {
            throw new System.NotImplementedException();
        }

        public ITypeInfo[] GetInterfaces()
        {
            throw new System.NotImplementedException();
        }

        public ITypeInfo GetGenericTypeDefinition()
        {
            throw new System.NotImplementedException();
        }

        public ITypeInfo GetElementType()
        {
            throw new System.NotImplementedException();
        }

        public string[] GetEnumNames()
        {
            throw new System.NotImplementedException();
        }

        public bool IsAssignableFrom(ITypeInfo type)
        {
            throw new System.NotImplementedException();
        }
    }
}