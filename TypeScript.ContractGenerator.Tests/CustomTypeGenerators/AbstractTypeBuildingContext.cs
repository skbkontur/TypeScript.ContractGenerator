using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.TypeBuilders;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.CustomTypeGenerators
{
    public class AbstractTypeBuildingContext : TypeBuildingContext
    {
        public AbstractTypeBuildingContext(TypeScriptUnit unit, ITypeInfo type)
            : base(unit, type)
        {
        }

        public override void Initialize(ITypeGenerator typeGenerator)
        {
            var types = typeGenerator.TypesProvider
                                     .GetAssemblyTypes(Type)
                                     .Where(x => x.BaseType != null && x.BaseType.Equals(Type))
                                     .ToArray();

            Declaration = new TypeScriptTypeDeclaration
                {
                    Name = Type.Name,
                    Definition = new TypeScriptUnionType(types.Select(x =>
                        {
                            var resultType = typeGenerator.BuildAndImportType(Unit, x, x);
                            if (resultType is INullabilityWrapperType nullableType)
                            {
                                return nullableType.InnerType;
                            }
                            return resultType;
                        }).ToArray())
                };
            base.Initialize(typeGenerator);
        }
    }
}