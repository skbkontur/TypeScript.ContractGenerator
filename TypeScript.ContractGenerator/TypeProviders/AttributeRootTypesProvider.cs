using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Attributes;

namespace SkbKontur.TypeScript.ContractGenerator.TypeProviders
{
    public class AttributeRootTypesProvider : IRootTypesProvider
    {
        public AttributeRootTypesProvider(params Assembly[] assemblies)
        {
            attributeSearchTypes = assemblies.SelectMany(x => x.GetTypes()).ToArray();
        }

        public Type[] GetRootTypes()
        {
            return attributeSearchTypes.Where(x => x.GetCustomAttributes<ContractGeneratorGenerateAttribute>().Any())
                                       .SelectMany(x => GetTypes(x, x.GetCustomAttribute<ContractGeneratorGenerateAttribute>().Scope))
                                       .ToArray();
        }

        private IEnumerable<Type> GetTypes(Type type, Scope scope)
        {
            if (scope.HasFlag(Scope.Self))
                yield return type;

            if (!scope.HasFlag(Scope.Children))
                yield break;

            foreach (var t in attributeSearchTypes.Where(x => ShouldGenerateChild(type, x, scope)))
                yield return t;
        }

        private static bool ShouldGenerateChild(Type type, Type child, Scope scope)
        {
            return child != type
                   && type.IsAssignableFrom(child)
                   && (!scope.HasFlag(Scope.NonAbstract) || !child.IsAbstract)
                   && !child.GetCustomAttributes<ContractGeneratorIgnoreAttribute>().Any();
        }

        private readonly Type[] attributeSearchTypes;
    }
}