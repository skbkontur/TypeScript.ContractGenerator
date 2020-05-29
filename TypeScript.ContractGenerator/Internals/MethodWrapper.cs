using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class MethodWrapper : IMethodInfo
    {
        public MethodWrapper(MethodInfo method)
        {
            Method = method;
        }

        public MethodInfo Method { get; }

        public string Name => Method.Name;
        public ITypeInfo ReturnType => TypeInfo.From(Method.ReturnType).WithMemberInfo(this);
        public ITypeInfo? DeclaringType => TypeInfo.From(Method.DeclaringType);

        public IParameterInfo[] GetParameters()
        {
            return Method.GetParameters().Select(x => (IParameterInfo)new ParameterWrapper(x)).ToArray();
        }

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return Method.GetAttributes(inherit);
        }
    }
}