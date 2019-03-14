using System;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal class NullCustomTypeGenerator : ICustomTypeGenerator
    {
        [NotNull]
        public string GetTypeLocation([NotNull] Type type)
        {
            return "";
        }

        [CanBeNull]
        public ITypeBuildingContext ResolveType([NotNull] string initialUnitPath, [NotNull] Type type, [NotNull] ITypeScriptUnitFactory unitFactory)
        {
            return null;
        }

        [CanBeNull]
        public TypeScriptTypeMemberDeclaration ResolveProperty([NotNull] ITypeGenerator typeGenerator, [NotNull] Type type, [NotNull] PropertyInfo property)
        {
            return null;
        }
    }
}