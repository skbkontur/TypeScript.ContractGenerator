using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Roslyn
{
    public static class RoslynTypeExtensions
    {
        public static IAttributeInfo[] GetAttributesInfo(this ISymbol symbol)
        {
            return symbol.GetAttributes().Select(x => (IAttributeInfo)new RoslynAttributeInfo(x)).ToArray();
        }

        public static (ICustomTypeGenerator, IRootTypesProvider) GetCustomization(this Compilation compilation)
        {
            var customGenerationTypes = GetCustomGenerationTypes(compilation);
            var assembly = AdhocProject.CompileAssembly(customGenerationTypes);

            var customTypeGenerator = assembly.GetImplementations<ICustomTypeGenerator>().Single();
            var typesProvider = assembly.GetImplementations<IRootTypesProvider>().Single();

            return (customTypeGenerator, typesProvider);
        }

        public static bool IsEqualTo<T>(this ITypeSymbol typeSymbol)
        {
            var type = typeof(T);
            return typeSymbol.Name == type.Name && typeSymbol.ContainingNamespace?.ToString() == type.Namespace;
        }

        public static SyntaxTree[] GetNamespaceTypes(this Compilation compilation, Func<ITypeSymbol, bool> func)
        {
            var providerType = compilation.GlobalNamespace.GetAllTypes().Single(func);

            return providerType.ContainingNamespace.Locations
                               .Select(x => TypeInfoRewriter.Rewrite(compilation, x.SourceTree!))
                               .ToArray();
        }

        public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceSymbol @namespace)
        {
            foreach (var type in @namespace.GetTypeMembers())
            foreach (var nestedType in GetNestedTypes(type))
                yield return nestedType;

            foreach (var nestedNamespace in @namespace.GetNamespaceMembers())
            foreach (var type in GetAllTypes(nestedNamespace))
                yield return type;
        }

        private static IEnumerable<INamedTypeSymbol> GetNestedTypes(INamedTypeSymbol type)
        {
            yield return type;
            foreach (var nestedType in type.GetTypeMembers().SelectMany(GetNestedTypes))
                yield return nestedType;
        }

        private static SyntaxTree[] GetCustomGenerationTypes(Compilation compilation)
        {
            return GetNamespaceTypes(compilation, x => !x.IsEqualTo<RootTypesProvider>() &&
                                                       x.Interfaces.Any(i => i.IsEqualTo<IRootTypesProvider>()));
        }
    }
}