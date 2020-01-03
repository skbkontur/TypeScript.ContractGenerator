using System.Reflection;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Roslyn
{
    public class RoslynPropertyInfo : IPropertyInfo
    {
        public RoslynPropertyInfo(IPropertySymbol propertySymbol)
        {
            this.propertySymbol = propertySymbol;
        }

        public bool IsNameDefined(string name)
        {
            return propertySymbol.IsNameDefined(name);
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return new object[0];
        }

        public PropertyInfo Property { get; }
        public string Name => propertySymbol.Name;
        public ITypeInfo PropertyType => new RoslynTypeInfo(propertySymbol.Type);

        private readonly IPropertySymbol propertySymbol;
    }
}