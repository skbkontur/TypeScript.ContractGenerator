using System;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public class RootTypesProvider : IRootTypesProvider
    {
        public RootTypesProvider(params Type[] rootTypes)
        {
            this.rootTypes = rootTypes ?? new Type[0];
        }

        public ITypeInfo[] GetRootTypes()
        {
            return rootTypes.Select(TypeInfo.From).ToArray();
        }

        private readonly Type[] rootTypes;
    }
}