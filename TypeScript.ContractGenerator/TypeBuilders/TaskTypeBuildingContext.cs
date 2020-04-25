using System.Threading.Tasks;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class TaskTypeBuildingContext : ITypeBuildingContext
    {
        public TaskTypeBuildingContext(ITypeInfo taskType, TypeScriptGenerationOptions options)
        {
            this.taskType = taskType;
            this.options = options;
        }

        public static bool Accept(ITypeInfo type)
        {
            return type.Equals(TypeInfo.From<Task>()) || type.IsGenericType && type.GetGenericTypeDefinition().Equals(TypeInfo.From(typeof(Task<>)));
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, IAttributeProvider? attributeProvider)
        {
            var itemType = taskType.IsGenericType ? taskType.GetGenericArguments()[0] : TypeInfo.From(typeof(void));
            var itemTypeScriptType = typeGenerator.ResolveType(itemType).ReferenceFrom(targetUnit, typeGenerator, null);
            return new TypeScriptPromiseOfType(itemTypeScriptType);
        }

        private readonly ITypeInfo taskType;
        private readonly TypeScriptGenerationOptions options;
    }
}