using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class ParameterWrapper : IParameterInfo
    {
        public ParameterWrapper(ParameterInfo parameter)
        {
            Parameter = parameter;
        }

        public ParameterInfo Parameter { get; }

        public string Name => Parameter.Name;
        public ITypeInfo ParameterType => TypeInfo.From(Parameter.ParameterType).WithMemberInfo(this);
        public IMethodInfo Method => new MethodWrapper((MethodInfo)Parameter.Member);

        public IAttributeInfo[] GetAttributes(bool inherit)
        {
            return Parameter.GetAttributes(inherit);
        }
    }
}