using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;

using TypeInfo = SkbKontur.TypeScript.ContractGenerator.Internals.TypeInfo;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class CustomTypeTypeBuildingContext : TypeBuildingContext
    {
        public CustomTypeTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
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
                    GenericTypeArguments = Type.IsGenericTypeDefinition ? Type.GetGenericArguments().Select(x => x.Name).ToArray() : new string[0]
                };
            return result;
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            Declaration = CreateComplexTypeScriptDeclarationWithoutDefinition(Type);
            Unit.Body.Add(new TypeScriptExportTypeStatement {Declaration = Declaration});

            var baseType = Type.BaseType;
            if (baseType != null && !baseType.Equals(TypeInfo.From<object>()) && !baseType.Equals(TypeInfo.From<ValueType>()) && !baseType.Equals(TypeInfo.From<MarshalByRefObject>()))
                typeGenerator.ResolveType(baseType);
        }

        public override void BuildDefinition(ITypeGenerator typeGenerator)
        {
            Declaration.Definition = CreateComplexTypeScriptDefinition(typeGenerator);
        }

        protected virtual TypeScriptTypeDefintion CreateComplexTypeScriptDefinition(ITypeGenerator typeGenerator)
        {
            var result = new TypeScriptTypeDefintion();
            result.Members.AddRange(Type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                        .Select(x => typeGenerator.ResolveProperty(Unit, Type, x))
                                        .Where(x => x != null)
                                        .Select(x => x!));
            return result;
        }
    }
}