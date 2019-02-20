using System;
using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.CodeDom;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class BuildInTypeBuildingContext : ITypeBuildingContext
    {
        public BuildInTypeBuildingContext(Type type)
        {
            this.type = type;
        }

        public static bool Accept(Type type)
        {
            return builtinTypes.ContainsKey(type);
        }

        public FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            if (builtinTypes.ContainsKey(type))
                return new FlowTypeBuildInType(builtinTypes[type]);
            throw new ArgumentOutOfRangeException();
        }

        public bool IsDefinitionBuilt => true;

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        private readonly Type type;

        private static readonly Dictionary<Type, string> builtinTypes = new Dictionary<Type, string>
            {
                {typeof(string), "string"},
                {typeof(bool), "boolean"},
                {typeof(int), "number"},
                {typeof(decimal), "number"},
                {typeof(long), "string"},
                {typeof(DateTime), "(Date | string)"},
                {typeof(byte[]), "string"},
                {typeof(void), "void"}
            };
    }
}