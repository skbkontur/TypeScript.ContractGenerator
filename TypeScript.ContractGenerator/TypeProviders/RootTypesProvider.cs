using System;

using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator.TypeProviders
{
    public class RootTypesProvider : IRootTypesProvider
    {
        public RootTypesProvider(params Type[] rootTypes)
        {
            this.rootTypes = rootTypes ?? new Type[0];
        }

        [NotNull, ItemNotNull]
        public Type[] GetRootTypes()
        {
            return rootTypes;
        }

        [NotNull]
        public static readonly IRootTypesProvider Default = new RootTypesProvider();

        private readonly Type[] rootTypes;
    }
}