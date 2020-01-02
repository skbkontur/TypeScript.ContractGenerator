using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;
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
            definition.Members.AddRange(Type.Type
                                            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                            .Select(x => new TypeScriptInterfaceFunctionMember(x.Name, typeGenerator.BuildAndImportType(Unit, x, new TypeWrapper(x.ReturnType)))));
            Declaration = new TypeScriptInterfaceDeclaration {Definition = definition, Name = Type.Name};

            base.Initialize(typeGenerator);
        }
    }
}