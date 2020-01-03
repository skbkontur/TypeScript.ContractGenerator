using System;
using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class BuiltinTypeBuildingContext : ITypeBuildingContext
    {
        public BuiltinTypeBuildingContext(ITypeInfo type)
        {
            this.type = type;
        }

        public static bool Accept(ITypeInfo type)
        {
            return builtinTypes.ContainsKey(type);
        }

        public TypeScriptType ReferenceFrom(TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
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

        private readonly ITypeInfo type;

        private static readonly Dictionary<ITypeInfo, string> builtinTypes = new Dictionary<ITypeInfo, string>
            {
                {TypeInfo.From<bool>(), "boolean"},
                {TypeInfo.From<int>(), "number"},
                {TypeInfo.From<uint>(), "number"},
                {TypeInfo.From<short>(), "number"},
                {TypeInfo.From<ushort>(), "number"},
                {TypeInfo.From<byte>(), "number"},
                {TypeInfo.From<sbyte>(), "number"},
                {TypeInfo.From<float>(), "number"},
                {TypeInfo.From<double>(), "number"},
                {TypeInfo.From<decimal>(), "number"},
                {TypeInfo.From<DateTime>(), "(Date | string)"},
                {TypeInfo.From<TimeSpan>(), "(number | string)"},
                {TypeInfo.From<string>(), "string"},
                {TypeInfo.From<long>(), "string"},
                {TypeInfo.From<ulong>(), "string"},
                {TypeInfo.From<byte[]>(), "string"},
                {TypeInfo.From<Guid>(), "string"},
                {TypeInfo.From<char>(), "string"},
                {TypeInfo.From(typeof(void)), "void"}
            };
    }
}