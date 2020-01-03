using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public class CustomTypeGenerator : ICustomTypeGenerator
    {
        public virtual string GetTypeLocation(ITypeInfo typeInfo)
        {
            if (typeLocations.TryGetValue(typeInfo, out var getLocation))
                return getLocation(typeInfo);
            foreach (var typeLocationRule in typeLocationRules)
                if (typeLocationRule.Accept(typeInfo))
                    return typeLocationRule.GetLocation(typeInfo);
            return string.Empty;
        }

        public virtual ITypeBuildingContext ResolveType(string initialUnitPath, ITypeInfo typeInfo, ITypeScriptUnitFactory unitFactory)
        {
            if (typeRedirects.TryGetValue(typeInfo, out var redirect))
                return TypeBuilding.RedirectToType(redirect.Name, redirect.Location, typeInfo);
            if (typeBuildingContexts.TryGetValue(typeInfo, out var createContext))
                return createContext(typeInfo);
            foreach (var typeBuildingContextWithAcceptanceChecking in typeBuildingContextsWithAcceptanceChecking)
                if (typeBuildingContextWithAcceptanceChecking.Accept(typeInfo))
                    return typeBuildingContextWithAcceptanceChecking.CreateContext(unitFactory.GetOrCreateTypeUnit(initialUnitPath), typeInfo);
            return null;
        }

        public virtual TypeScriptTypeMemberDeclaration ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            foreach (var propertyResolver in propertyResolvers)
            {
                var result = propertyResolver.ResolveProperty(unit, typeGenerator, typeInfo, propertyInfo);
                if (result != null)
                    return result;
            }
            return null;
        }

        public CustomTypeGenerator WithTypeLocation<T>(Func<ITypeInfo, string> getLocation)
        {
            typeLocations[TypeInfo.From<T>()] = getLocation;
            return this;
        }

        public CustomTypeGenerator WithTypeLocationRule(Func<ITypeInfo, bool> accept, Func<ITypeInfo, string> getLocation)
        {
            typeLocationRules.Add((accept, getLocation));
            return this;
        }

        public CustomTypeGenerator WithTypeRedirect<T>(string name, string location)
        {
            typeRedirects[TypeInfo.From<T>()] = new TypeLocation {Name = name, Location = location};
            return this;
        }

        public CustomTypeGenerator WithTypeBuildingContext<T>(Func<ITypeInfo, ITypeBuildingContext> createContext)
        {
            typeBuildingContexts[TypeInfo.From<T>()] = createContext;
            return this;
        }

        public CustomTypeGenerator WithTypeBuildingContext(Func<ITypeInfo, bool> accept, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> createContext)
        {
            typeBuildingContextsWithAcceptanceChecking.Add((accept, createContext));
            return this;
        }

        public CustomTypeGenerator WithPropertyResolver(IPropertyResolver propertyResolver)
        {
            propertyResolvers.Add(propertyResolver);
            return this;
        }

        [NotNull]
        public static ICustomTypeGenerator Null => new NullCustomTypeGenerator();

        private readonly Dictionary<ITypeInfo, Func<ITypeInfo, string>> typeLocations = new Dictionary<ITypeInfo, Func<ITypeInfo, string>>();
        private readonly Dictionary<ITypeInfo, TypeLocation> typeRedirects = new Dictionary<ITypeInfo, TypeLocation>();
        private readonly Dictionary<ITypeInfo, Func<ITypeInfo, ITypeBuildingContext>> typeBuildingContexts = new Dictionary<ITypeInfo, Func<ITypeInfo, ITypeBuildingContext>>();
        private readonly List<(Func<ITypeInfo, bool> Accept, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> CreateContext)> typeBuildingContextsWithAcceptanceChecking = new List<(Func<ITypeInfo, bool> Accept, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> CreateContext)>();
        private readonly List<(Func<ITypeInfo, bool> Accept, Func<ITypeInfo, string> GetLocation)> typeLocationRules = new List<(Func<ITypeInfo, bool> Accept, Func<ITypeInfo, string> GetLocation)>();
        private readonly List<IPropertyResolver> propertyResolvers = new List<IPropertyResolver>();
    }
}