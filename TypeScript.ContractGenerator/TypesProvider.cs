using System;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public class TypesProvider : ITypesProvider
    {
        public TypesProvider(params Type[] rootTypes)
        {
            this.rootTypes = rootTypes ?? new Type[0];
        }

        public ITypeInfo[] GetRootTypes()
        {
            return rootTypes.Select(TypeInfo.From).ToArray();
        }

        public ITypeInfo[] GetAssemblyTypes(ITypeInfo type)
        {
            return type.Type.Assembly.GetTypes().Select(TypeInfo.From).ToArray();
        }

        public static readonly ITypesProvider Default = new TypesProvider();

        private readonly Type[] rootTypes;
    }
}