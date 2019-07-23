using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class MethodTypeBuildingContext : TypeBuildingContext
    {
        public MethodTypeBuildingContext(TypeScriptUnit unit, Type type)
            : base(unit, type)
        {
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            var definition = new TypeScriptInterfaceDefinition();
            definition.Members.AddRange(Type
                                            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                            .Select(x => new TypeScriptInterfaceFunctionMember(x.Name, typeGenerator.BuildAndImportType(Unit, x, x.ReturnType))));
            Declaration = new TypeScriptInterfaceDeclaration {Definition = definition, Name = Type.Name};

            base.Initialize(typeGenerator);
        }
    }
}