using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class CustomTypeTypeBuildingContext : TypeBuildingContext
    {
        public CustomTypeTypeBuildingContext([NotNull] TypeScriptUnit unit, [NotNull] ITypeInfo type)
            : base(unit, type)
        {
        }

        public override bool IsDefinitionBuilt => Declaration.Definition != null;

        private TypeScriptTypeDeclaration CreateComplexTypeScriptDeclarationWithoutDefinition(ITypeInfo type)
        {
            var result = new TypeScriptTypeDeclaration
                {
                    Name = type.IsGenericType ? new Regex("`.*$").Replace(type.GetGenericTypeDefinition().Name, "") : type.Name,
                    Definition = null,
                    GenericTypeArguments = Type.IsGenericTypeDefinition ? Type.GetGenericArguments().Select(x => x.Name).ToArray() : null
                };
            return result;
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            Declaration = CreateComplexTypeScriptDeclarationWithoutDefinition(Type);
            Unit.Body.Add(new TypeScriptExportTypeStatement {Declaration = Declaration});

            var baseType = Type.BaseType.Type;
            if (baseType != typeof(object) && baseType != typeof(ValueType) && baseType != typeof(MarshalByRefObject) && baseType != null)
                typeGenerator.ResolveType(Type.BaseType);
        }

        public override void BuildDefinition(ITypeGenerator typeGenerator)
        {
            Declaration.Definition = CreateComplexTypeScriptDefinition(typeGenerator);
        }

        protected virtual TypeScriptTypeDefintion CreateComplexTypeScriptDefinition(ITypeGenerator typeGenerator)
        {
            var result = new TypeScriptTypeDefintion();
            result.Members.AddRange(Type.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                        .Select(x => typeGenerator.ResolveProperty(Unit, Type, new PropertyWrapper(x)))
                                        .Where(x => x != null));
            return result;
        }
    }
}