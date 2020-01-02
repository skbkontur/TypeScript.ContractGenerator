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
                {TypeInfo.FromType<bool>(), "boolean"},
                {TypeInfo.FromType<int>(), "number"},
                {TypeInfo.FromType<uint>(), "number"},
                {TypeInfo.FromType<short>(), "number"},
                {TypeInfo.FromType<ushort>(), "number"},
                {TypeInfo.FromType<byte>(), "number"},
                {TypeInfo.FromType<sbyte>(), "number"},
                {TypeInfo.FromType<float>(), "number"},
                {TypeInfo.FromType<double>(), "number"},
                {TypeInfo.FromType<decimal>(), "number"},
                {TypeInfo.FromType<DateTime>(), "(Date | string)"},
                {TypeInfo.FromType<TimeSpan>(), "(number | string)"},
                {TypeInfo.FromType<string>(), "string"},
                {TypeInfo.FromType<long>(), "string"},
                {TypeInfo.FromType<ulong>(), "string"},
                {TypeInfo.FromType<byte[]>(), "string"},
                {TypeInfo.FromType<Guid>(), "string"},
                {TypeInfo.FromType<char>(), "string"},
                {new TypeWrapper(typeof(void)), "void"}
            };
    }
}