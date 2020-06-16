using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.Tests.Types;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class BuilderCustomGenerator : CustomTypeGenerator
    {
        public BuilderCustomGenerator()
        {
            WithTypeLocation(TypeInfo.From<AnotherCustomType>(), x => "a/b/c")
                .WithTypeRedirect(TypeInfo.From<byte[]>(), "ByteArray", @"DataTypes\ByteArray")
                .WithTypeLocation(TypeInfo.From<HashSet<string>>(), x => "a/b")
                .WithTypeBuildingContext(TypeInfo.From<HashSet<string>>(), x => new CollectionTypeBuildingContext(x));
        }
    }
}