using System;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public static class TypeBuilding
    {
        public static ITypeBuildingContext RedirectToType(string typeName, string path, Type type)
        {
            return new RedirectToTypeBuildingContext(typeName, path, type);
        }
    }
}