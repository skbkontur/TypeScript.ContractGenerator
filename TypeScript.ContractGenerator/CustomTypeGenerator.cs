using System;
using System.Collections.Generic;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator
{
    public class TypeLocationRule
    {
        public TypeLocationRule(Func<ITypeInfo, bool> accept, Func<ITypeInfo, string> getLocation)
        {
            Accept = accept;
            GetLocation = getLocation;
        }

        public Func<ITypeInfo, bool> Accept { get; }
        public Func<ITypeInfo, string> GetLocation { get; }
    }

    public class TypeBuildingContextRule
    {
        public TypeBuildingContextRule(Func<ITypeInfo, bool> accept, Func<ITypeInfo, ITypeBuildingContext> createContext)
        {
            Accept = accept;
            CreateContext = createContext;
        }

        public TypeBuildingContextRule(Func<ITypeInfo, bool> accept, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> createContextWithUnit)
        {
            Accept = accept;
            CreateContextWithUnit = createContextWithUnit;
        }

        public Func<ITypeInfo, bool> Accept { get; }
        public Func<ITypeInfo, ITypeBuildingContext> CreateContext { get; }
        public Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> CreateContextWithUnit { get; }
    }

    public class CustomTypeGenerator : ICustomTypeGenerator
    {
        public virtual string GetTypeLocation(ITypeInfo typeInfo)
        {
            return typeLocationRules.SingleOrDefault(x => x.Accept(typeInfo))?.GetLocation(typeInfo) ?? string.Empty;
        }

        public virtual ITypeBuildingContext? ResolveType(string initialUnitPath, ITypeGenerator typeGenerator, ITypeInfo typeInfo, ITypeScriptUnitFactory unitFactory)
        {
            if (typeRedirects.TryGetValue(typeInfo, out var redirect))
                return TypeBuilding.RedirectToType(redirect.Name, redirect.Location, typeInfo);

            var contextCreation = typeBuildingContextRules.SingleOrDefault(x => x.Accept(typeInfo));
            if (contextCreation == null)
                return null;

            return contextCreation.CreateContext != null
                       ? contextCreation.CreateContext(typeInfo)
                       : contextCreation.CreateContextWithUnit(unitFactory.GetOrCreateTypeUnit(initialUnitPath), typeInfo);
        }

        public virtual TypeScriptTypeMemberDeclaration? ResolveProperty(TypeScriptUnit unit, ITypeGenerator typeGenerator, ITypeInfo typeInfo, IPropertyInfo propertyInfo)
        {
            foreach (var propertyResolver in propertyResolvers)
            {
                var result = propertyResolver.ResolveProperty(unit, typeGenerator, typeInfo, propertyInfo);
                if (result != null)
                    return result;
            }
            return null;
        }

        public CustomTypeGenerator WithTypeLocation(ITypeInfo typeInfo, Func<ITypeInfo, string> getLocation)
        {
            typeLocationRules.Add(new TypeLocationRule(x => x.Equals(typeInfo), getLocation));
            return this;
        }

        public CustomTypeGenerator WithTypeLocationRule(Func<ITypeInfo, bool> accept, Func<ITypeInfo, string> getLocation)
        {
            typeLocationRules.Add(new TypeLocationRule(accept, getLocation));
            return this;
        }

        public CustomTypeGenerator WithTypeRedirect(ITypeInfo typeInfo, string name, string location)
        {
            typeRedirects[typeInfo] = new TypeLocation {Name = name, Location = location};
            return this;
        }

        public CustomTypeGenerator WithTypeBuildingContext(ITypeInfo typeInfo, Func<ITypeInfo, ITypeBuildingContext> createContext)
        {
            typeBuildingContextRules.Add(new TypeBuildingContextRule(x => x.Equals(typeInfo), createContext));
            return this;
        }

        public CustomTypeGenerator WithTypeBuildingContext(ITypeInfo typeInfo, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> createContext)
        {
            typeBuildingContextRules.Add(new TypeBuildingContextRule(x => x.Equals(typeInfo), createContext));
            return this;
        }

        public CustomTypeGenerator WithTypeBuildingContext(Func<ITypeInfo, bool> accept, Func<TypeScriptUnit, ITypeInfo, ITypeBuildingContext> createContext)
        {
            typeBuildingContextRules.Add(new TypeBuildingContextRule(accept, createContext));
            return this;
        }

        public CustomTypeGenerator WithPropertyResolver(IPropertyResolver propertyResolver)
        {
            propertyResolvers.Add(propertyResolver);
            return this;
        }

        public static ICustomTypeGenerator Null => new NullCustomTypeGenerator();

        private readonly List<IPropertyResolver> propertyResolvers = new List<IPropertyResolver>();
        private readonly List<TypeLocationRule> typeLocationRules = new List<TypeLocationRule>();
        private readonly List<TypeBuildingContextRule> typeBuildingContextRules = new List<TypeBuildingContextRule>();
        private readonly Dictionary<ITypeInfo, TypeLocation> typeRedirects = new Dictionary<ITypeInfo, TypeLocation>();
    }
}