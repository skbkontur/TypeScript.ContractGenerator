using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Extensions;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class MethodTypeBuildingContext : TypeBuildingContext
    {
        public MethodTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            var definition = new TypeScriptInterfaceDefinition();

            definition.Members.AddRange(Type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                            .Where(m => !m.Name.Contains("_"))
                                            .Select(m => new TypeScriptInterfaceFunctionMember(m.Name.ToLowerCamelCase(), typeGenerator.BuildAndImportType(Unit, m, m.ReturnType),
                                                                                               m.GetParameters()
                                                                                                .Select(p => new TypeScriptArgumentDeclaration
                                                                                                    {
                                                                                                        Name = p.Name,
                                                                                                        Optional = false,
                                                                                                        Type = typeGenerator.BuildAndImportType(Unit, p, p.ParameterType),
                                                                                                    })
                                                                                                .ToArray())));

            definition.Members.AddRange(Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Select(x => new TypeScriptInterfacePropertyMember(x.Name.ToLowerCamelCase(), typeGenerator.BuildAndImportType(Unit, x, x.PropertyType))));

            Declaration = new TypeScriptInterfaceDeclaration {Definition = definition, Name = Type.Name};

            base.Initialize(typeGenerator);
        }
    }
}