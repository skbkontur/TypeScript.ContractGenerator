using System;
using System.Collections.Generic;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;
using SkbKontur.TypeScript.ContractGenerator.CodeDom;
using SkbKontur.TypeScript.ContractGenerator.Internals;

namespace SkbKontur.TypeScript.ContractGenerator.TypeBuilders
{
    public class BuiltinTypeBuildingContext : TypeBuildingContextBase
    {
        public BuiltinTypeBuildingContext(ITypeInfo type)
            : base(type)
        {
        }

        public static bool Accept(ITypeInfo type)
        {
            return builtinTypes.ContainsKey(type);
        }

        protected override TypeScriptType ReferenceFromInternal(ITypeInfo type, TypeScriptUnit targetUnit, ITypeGenerator typeGenerator)
        {
            if (builtinTypes.TryGetValue(type, out var typeScriptType))
                return new TypeScriptBuildInType(typeScriptType);
            throw new ArgumentOutOfRangeException(nameof(type), $"Type '{type}' is not found");
        }

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
                {TypeInfo.From<DateTimeOffset>(), "(Date | string)"},
                {TypeInfo.From<TimeSpan>(), "(number | string)"},
                {TypeInfo.From<string>(), "string"},
                {TypeInfo.From<long>(), "number"},
                {TypeInfo.From<ulong>(), "number"},
                {TypeInfo.From<byte[]>(), "string"},
                {TypeInfo.From<Guid>(), "string"},
                {TypeInfo.From<char>(), "string"},
                {TypeInfo.From<object>(), "object"},
                {TypeInfo.From(typeof(void)), "void"}
            };
    }
}