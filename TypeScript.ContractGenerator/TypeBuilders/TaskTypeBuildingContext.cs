using System.Threading.Tasks;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class TaskTypeBuildingContext : TypeBuildingContextBase
    {
        public TaskTypeBuildingContext(ITypeInfo taskType)
            : base(taskType)
        {
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.Equals(TypeInfo.From<Task>()) || type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(Task<>)));
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            var itemType = Type.IsGenericType ? Type.GetGenericArguments()[0] : TypeInfo.From(typeof(void));
            var itemTypeScriptType = typeGenerator.BuildAndImportType(targetUnit, itemType);
            return new TypeScriptPromiseOfType(itemTypeScriptType);
        }
    }
}