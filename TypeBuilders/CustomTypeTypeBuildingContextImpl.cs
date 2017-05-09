using System;
using System.Reflection;
using System.Text.RegularExpressions;

using SKBKontur.Catalogue.FlowType.CodeDom;
using SKBKontur.Catalogue.FlowType.ContractGenerator.Extensions;

namespace SKBKontur.Catalogue.FlowType.ContractGenerator.TypeBuilders
{
    public class CustomTypeTypeBuildingContextImpl : TypeBuildingContext
    {
        public CustomTypeTypeBuildingContextImpl(FlowTypeUnit unit, Type type)
            : base(unit, type)
        {
        }

        public override bool IsDefinitionBuilded { get { return Declaration.Definition != null; } }

        private FlowTypeTypeDeclaration CreateComplexFlowTypeDeclarationWithoutDefintion(Type type)
        {
            var result = new FlowTypeTypeDeclaration
                {
                    Name = type.IsGenericType ? new Regex("`.*$").Replace(type.GetGenericTypeDefinition().Name, "") : type.Name,
                    Definition = null,
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

        private FlowTypeTypeDefintion CreateComplexFlowTypeDefintion(ITypeGenerator typeGenerator)
        {
            var result = new FlowTypeTypeDefintion();
            var properties = CreateTypeProperties(Type);
            foreach(var property in properties)
            {
                result.Members.Add(new FlowTypeTypeMemberDeclaration
                    {
                        Name = BuildPropertyName(property.Name),
                        Type = typeGenerator.BuildAndImportType(Unit, property, property.PropertyType),
                    });
            }
            return result;
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