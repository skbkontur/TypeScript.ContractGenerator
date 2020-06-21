using System;
using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Roslyn;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Helpers
{
    public class RoslynTypesProvider : IRootTypesProvider
    {
        public RoslynTypesProvider(Compilation compilation, params Type[] rootTypes)
        {
            this.compilation = compilation;
            this.rootTypes = rootTypes;
        }

        public ITypeInfo[] GetRootTypes()
        {
            return rootTypes.Select(x => RoslynTypeInfo.From(compilation.GetTypeByMetadataName(x.FullName!)!)).ToArray();
        }

        private readonly Compilation compilation;
        private readonly Type[] rootTypes;
    }
}