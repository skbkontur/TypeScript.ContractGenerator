using System;
using System.Linq;
using System.Reflection;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class AbstractTypeBuildingContext : TypeBuildingContext
    {
        public AbstractTypeBuildingContext(TypeScriptUnit unit, Type type)
            : base(unit, type)
        {
            this.unit = unit;
            this.type = type;
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            var types = Assembly
                .GetAssembly(type)
                .GetTypes()
                .Where(x => x.BaseType == type).ToArray();

            Declaration = new TypeScriptTypeDeclaration
                {
                    Name = type.Name,
                    Definition = new TypeScriptUnionType(types.Select(x =>
                        {
                            var resultType = typeGenerator.BuildAndImportType(unit, x, x);
                            if(resultType is TypeScriptNullableType resultNullableType)
                            {
                                return resultNullableType.InnerType;
                            }
                            return resultType;
                        }).ToArray())
                };
            base.Initialize(typeGenerator);
        }

        private readonly TypeScriptUnit unit;
        private readonly Type type;
    }
}