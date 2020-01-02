using System;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public static class TypeBuilding
    {
        public static ITypeBuildingContext RedirectToType(string typeName, string path, ITypeInfo type)
        {
            return new RedirectToTypeBuildingContext(typeName, path, type);
        }
    }
}