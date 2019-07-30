using System;
using System.Linq;

namespace SkbKontur.TypeScript.ContractGenerator.TypeProviders
{
    public class CompositeRootTypeProvider : IRootTypesProvider
    {
        public CompositeRootTypeProvider(IRootTypesProvider[] providers)
        {
            this.providers = providers;
        }

        public Type[] GetRootTypes()
        {
            return providers.SelectMany(x => x.GetRootTypes()).ToArray();
        }

        private readonly IRootTypesProvider[] providers;
    }
}