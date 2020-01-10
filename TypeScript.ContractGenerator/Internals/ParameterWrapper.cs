using System.Linq;
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
        public ITypeInfo ParameterType => TypeInfo.From(Parameter.ParameterType);

        public object[] GetCustomAttributes(bool inherit)
        {
            return Parameter.GetCustomAttributes(inherit);
        }

        public bool IsNameDefined(string name)
        {
            return Parameter.GetCustomAttributes(true).Any(x => x.GetType().Name == name);
        }
    }
}