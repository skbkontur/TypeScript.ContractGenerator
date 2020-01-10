using System;
using System.Collections.Generic;
using System.Reflection;

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

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator, ICustomAttributeProvider customAttributeProvider)
        {
            if (builtinTypes.ContainsKey(type))
                return new TypeScriptBuildInType(builtinTypes[type]);
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
                {typeof(bool), "boolean"},
                {typeof(int), "number"},
                {typeof(uint), "number"},
                {typeof(short), "number"},
                {typeof(ushort), "number"},
                {typeof(byte), "number"},
                {typeof(sbyte), "number"},
                {typeof(float), "number"},
                {typeof(double), "number"},
                {typeof(decimal), "number"},
                {typeof(DateTime), "(Date | string)"},
                {typeof(TimeSpan), "(number | string)"},
                {typeof(string), "string"},
                {typeof(long), "string"},
                {typeof(ulong), "string"},
                {typeof(byte[]), "string"},
                {typeof(Guid), "string"},
                {typeof(char), "string"},
                {typeof(void), "void"}
            };
    }
}