using System;
using System.Linq;

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
            return buildInTyped.Contains(type);
        }

        public FlowTypeType ReferenceFrom(FlowTypeUnit targetUnit, ITypeGenerator typeGenerator)
        {
            if (type == typeof(string))
                return new FlowTypeBuildInType("string");
            else if (type == typeof(bool))
                return new FlowTypeBuildInType("boolean");
            else if (type == typeof(int))
                return new FlowTypeBuildInType("number");
            else if (type == typeof(decimal))
                return new FlowTypeBuildInType("number");
            else if (type == typeof(Int64))
                return new FlowTypeBuildInType("string");
            else if (type == typeof(DateTime))
                return new FlowTypeBuildInType("(Date | string)");
            else if (type == typeof(byte[]))
                return new FlowTypeBuildInType("string");
            else if (type == typeof(void))
                return new FlowTypeBuildInType("void");
            throw new ArgumentOutOfRangeException();
        }

        public bool IsDefinitionBuilt { get { return true; } }

        public void Initialize(ITypeGenerator typeGenerator)
        {
        }

        public void BuildDefinition(ITypeGenerator typeGenerator)
        {
        }

        private readonly Type type;

        private static readonly Type[] buildInTyped =
            {
                typeof(string),
                typeof(bool),
                typeof(int),
                typeof(decimal),
                typeof(Int64),
                typeof(DateTime),
                typeof(void),
                typeof(byte[]),
            };
    }
}