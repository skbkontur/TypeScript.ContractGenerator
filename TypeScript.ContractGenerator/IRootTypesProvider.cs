using System;

using JetBrains.Annotations;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public interface IRootTypesProvider
    {
        [NotNull]
        Type[] GetRootTypes();
    }
}