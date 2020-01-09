using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class TypeInfo : ITypeInfo
    {
        private TypeInfo(Type type)
        {
            Type = type;
        }

        public static ITypeInfo From<T>()
        {
            return new TypeInfo(typeof(T));
        }

        public static ITypeInfo From(Type type)
        {
            return type == null ? null : new TypeInfo(type);
        }

        public Type Type { get; }

        public string Name => Type.Name;
        public string FullName => Type.FullName;
        public string Namespace => Type.Namespace;
        public bool IsEnum => Type.IsEnum;
        public bool IsValueType => Type.IsValueType;
        public bool IsArray => Type.IsArray;
        public bool IsClass => Type.IsClass;
        public bool IsInterface => Type.IsInterface;
        public bool IsAbstract => Type.IsAbstract;
        public bool IsGenericType => Type.IsGenericType;
        public bool IsGenericParameter => Type.IsGenericParameter;
        public bool IsGenericTypeDefinition => Type.IsGenericTypeDefinition;
        public ITypeInfo BaseType => From(Type.BaseType);

        public IMethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return Type.GetMethods(bindingAttr).Select(x => (IMethodInfo)new MethodWrapper(x)).ToArray();
        }

        public IPropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return Type.GetProperties(bindingAttr).Select(x => (IPropertyInfo)new PropertyWrapper(x)).ToArray();
        }

        public ITypeInfo[] GetGenericArguments()
        {
            return Type.GetGenericArguments().Select(From).ToArray();
        }

        public ITypeInfo[] GetInterfaces()
        {
            return Type.GetInterfaces().Select(From).ToArray();
        }

        public ITypeInfo GetGenericTypeDefinition()
        {
            return From(Type.GetGenericTypeDefinition());
        }

        public ITypeInfo GetElementType()
        {
            return From(Type.GetElementType());
        }

        public string[] GetEnumNames()
        {
            return Type.GetEnumNames();
        }

        public bool IsAssignableFrom(ITypeInfo type)
        {
            return Type.IsAssignableFrom(type.Type);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return Type.GetCustomAttributes(inherit);
        }

        public bool IsNameDefined(string name)
        {
            return Type.GetCustomAttributes(true).Any(x => x.GetType().Name == name);
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
            return Type.ToString();
        }
    }

    public static class TypeInfoHelpers
    {
        public static bool Equals(ITypeInfo self, ITypeInfo other)
        {
            if (self == null && other == null)
                return true;

            if (self == null || other == null)
                return false;

            if (self.IsArray)
                return other.IsArray && self.GetElementType().Equals(other.GetElementType());

            if (self.IsGenericParameter)
                return other.IsGenericParameter && self.Name == other.Name;

            return self.Name == other.Name
                   && self.Namespace == other.Namespace
                   && self.IsGenericTypeDefinition == other.IsGenericTypeDefinition
                   && GenericArgumentsEquals(self.GetGenericArguments(), other.GetGenericArguments());
        }

        public static int GetHashCode(ITypeInfo self)
        {
            if (self.IsArray)
                return (self.IsArray, self.GetElementType()).GetHashCode();

            if (self.IsGenericParameter)
                return (self.IsGenericParameter, self.Name).GetHashCode();

            return (self.Name, self.Namespace, self.IsGenericTypeDefinition).GetHashCode();
        }

        private static bool GenericArgumentsEquals(ITypeInfo[] thisArgs, ITypeInfo[] otherArgs)
        {
            if (thisArgs.Length != otherArgs.Length)
                return false;
            for (var i = 0; i < thisArgs.Length; i++)
                if (!thisArgs[i].Equals(otherArgs[i]))
                    return false;
            return true;
        }
    }
}