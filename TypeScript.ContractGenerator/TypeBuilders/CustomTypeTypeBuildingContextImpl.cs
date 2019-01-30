using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class CustomTypeTypeBuildingContextImpl : TypeBuildingContext
    {
        public CustomTypeTypeBuildingContextImpl(FlowTypeUnit unit, Type type)
            : base(unit, type)
        {
        }

        public override bool IsDefinitionBuilded => Declaration.Definition != null;

        private FlowTypeTypeDeclaration CreateComplexFlowTypeDeclarationWithoutDefintion(Type type)
        {
            var result = new FlowTypeTypeDeclaration
                {
                    Name = type.IsGenericType ? new Regex("`.*$").Replace(type.GetGenericTypeDefinition().Name, "") : type.Name,
                    Definition = null,
                    GenericTypeArguments = Type.IsGenericTypeDefinition ? Type.GetGenericArguments().Select(x => x.Name).ToArray() : null
                };
            return result;
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            if (Type.BaseType != typeof(object) && Type.BaseType != typeof(ValueType) && Type.BaseType != typeof(MarshalByRefObject) && Type.BaseType != null)
            {
                typeGenerator.ResolveType(Type.BaseType);
            }
            Declaration = CreateComplexFlowTypeDeclarationWithoutDefintion(Type);
            Unit.Body.Add(new FlowTypeExportTypeStatement {Declaration = Declaration});
        }

        public override void BuildDefiniion(ITypeGenerator typeGenerator)
        {
            Declaration.Definition = CreateComplexFlowTypeDefintion(typeGenerator);
        }

        protected virtual FlowTypeTypeDefintion CreateComplexFlowTypeDefintion(ITypeGenerator typeGenerator)
        {
            var result = new FlowTypeTypeDefintion();
            var properties = CreateTypeProperties(Type);
            foreach (var property in properties)
            {
                var (isNullable, type) = FlowTypeGeneratorHelpers.ProcessNullable(property, property.PropertyType);

                var propertyType = typeGenerator.BuildAndImportType(Unit, null, type);
                result.Members.Add(new FlowTypeTypeMemberDeclaration
                    {
                        Name = BuildPropertyName(property.Name),
                        Optional = isNullable,
                        Type = property.PropertyType.IsGenericParameter
                                   ? new FlowTypeTypeReference(property.PropertyType.Name)
                                   : isNullable ? OrNull(propertyType) : propertyType,
                    });
            }
            return result;
        }

        private FlowTypeUnionType OrNull(FlowTypeType buildAndImportType)
        {
            return new FlowTypeUnionType(
                new[]
                    {
                        new FlowTypeBuildInType("null"),
                        buildAndImportType
                    }
                );
        }

        private string BuildPropertyName(string propertyName)
        {
            return propertyName.ToLowerCamelCase();
        }

        protected virtual PropertyInfo[] CreateTypeProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
    }
}