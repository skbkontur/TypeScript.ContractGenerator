using System;
using System.Collections.Generic;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class NullabilityInfo
    {
        private NullabilityInfo(byte? nullableContext, byte[]? nullableInfo)
        {
            NullableContext = nullableContext;
            NullableInfo = nullableInfo;
        }

        private NullabilityInfo(byte? nullableContext, byte[]? nullableInfo, bool hasNotNull, bool hasCanBeNull, bool hasItemNotNull, bool hasItemCanBeNull)
        {
            NullableContext = nullableContext;
            NullableInfo = nullableInfo;

            HasNotNull = hasNotNull;
            HasCanBeNull = hasCanBeNull;
            HasItemNotNull = hasItemNotNull;
            HasItemCanBeNull = hasItemCanBeNull;
        }

        private NullabilityInfo(byte? nullableContext, byte[]? nullableInfo, IAttributeInfo[] memberAttributes)
        {
            NullableContext = nullableContext;
            NullableInfo = nullableInfo;

            HasNotNull = HasAttribute(memberAttributes, AnnotationsNames.NotNull) || HasAttribute(memberAttributes, AnnotationsNames.Required);
            HasCanBeNull = HasAttribute(memberAttributes, AnnotationsNames.CanBeNull);
            HasItemNotNull = HasAttribute(memberAttributes, AnnotationsNames.ItemNotNull);
            HasItemCanBeNull = HasAttribute(memberAttributes, AnnotationsNames.ItemCanBeNull);
        }

        public static NullabilityInfo FromRoslyn(IAttributeProvider memberInfo)
        {
            return new NullabilityInfo(null, null, memberInfo.GetAttributes(true));
        }

        public static NullabilityInfo From(IAttributeProvider memberInfo)
        {
            if (memberInfo is IFieldInfo field)
                return From(field.DeclaringType, field);
            if (memberInfo is IMethodInfo method)
                return From(method.DeclaringType, method);
            if (memberInfo is IPropertyInfo property)
                return From(property.DeclaringType, property);
            if (memberInfo is IParameterInfo parameter)
                return From(parameter.Method.DeclaringType, parameter.Method, parameter);
            throw new InvalidOperationException("TODO");
        }

        private static NullabilityInfo From(IAttributeProvider? type, IAttributeProvider member)
        {
            var typeAttributes = type?.GetAttributes(inherit : true) ?? new IAttributeInfo[0];
            var memberAttributes = member.GetAttributes(inherit : true);
            return new NullabilityInfo(GetNullableContextFlag(memberAttributes) ?? GetNullableContextFlag(typeAttributes),
                                       GetNullableFlagsInternal(memberAttributes),
                                       memberAttributes);
        }

        private static NullabilityInfo From(IAttributeProvider? type, IAttributeProvider? method, IAttributeProvider parameter)
        {
            var typeAttributes = type?.GetAttributes(inherit : true) ?? new IAttributeInfo[0];
            var methodAttributes = method?.GetAttributes(inherit : true) ?? new IAttributeInfo[0];
            var parameterAttributes = parameter.GetAttributes(inherit : true);
            return new NullabilityInfo(GetNullableContextFlag(parameterAttributes) ?? GetNullableContextFlag(methodAttributes) ?? GetNullableContextFlag(typeAttributes),
                                       GetNullableFlagsInternal(parameterAttributes),
                                       parameterAttributes);
        }

        public NullabilityInfo ForItem()
        {
            var nullableInfo = NullableInfo == null || NullableInfo.Length == 1 ? NullableInfo : NullableInfo.Skip(1).ToArray();
            return new NullabilityInfo(NullableContext, nullableInfo, HasItemNotNull, HasItemCanBeNull, false, false);
        }

        public static NullabilityInfo[] ForGenericArguments(NullabilityInfo nullabilityInfo, Type[] args)
        {
            if (nullabilityInfo.NullableInfo == null || nullabilityInfo.NullableInfo.Length < 2)
                return args.Select(x => new NullabilityInfo(nullabilityInfo.NullableContext, nullabilityInfo.NullableInfo)).ToArray();

            var result = new List<NullabilityInfo>();
            var index = 1;
            for (var i = 0; i < args.Length; i++)
            {
                var argsLength = GetGenericArgumentsToSkip(args[i]);
                result.Add(argsLength == 0
                               ? Empty
                               : new NullabilityInfo(
                                     nullabilityInfo.NullableContext,
                                     nullabilityInfo.NullableInfo.Skip(index).Take(argsLength).ToArray()
                                     )
                    );
                index += argsLength;
            }
            return result.ToArray();
        }

        private static int GetGenericArgumentsToSkip(Type type)
        {
            if (type.IsArray)
                return 1 + GetGenericArgumentsToSkip(type.GetElementType());

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return 0;

            if (!type.IsGenericType)
                return type.IsValueType ? 0 : 1;

            var count = 1;
            foreach (var argument in type.GetGenericArguments())
                count += GetGenericArgumentsToSkip(argument);

            return count;
        }

        private static byte[]? GetNullableFlagsInternal(IAttributeInfo[] attributes)
        {
            var nullableAttribute = attributes.SingleOrDefault(a => a.AttributeType.Name == AnnotationsNames.Nullable);
            return nullableAttribute?.AttributeData["NullableFlags"] as byte[];
        }

        private static byte? GetNullableContextFlag(IAttributeInfo[] attributes)
        {
            var nullableAttribute = attributes.SingleOrDefault(a => a.AttributeType.Name == AnnotationsNames.NullableContext);
            return (byte?)nullableAttribute?.AttributeData["Flag"];
        }

        private static bool HasAttribute(IAttributeInfo[] attributes, string name)
        {
            return attributes.Any(a => a.AttributeType.Name == name);
        }

        public bool CanBeNull(NullabilityMode nullabilityMode)
        {
            if (nullabilityMode == NullabilityMode.None)
                return false;

            var nullableByte = NullableInfo?[0] ?? NullableContext ?? 0;
            if (nullabilityMode == NullabilityMode.NullableReference ||
                nullabilityMode.HasFlag(NullabilityMode.NullableReference) && nullableByte != 0)
                return nullableByte == 2;

            return nullabilityMode == NullabilityMode.Pessimistic ? !HasNotNull : HasCanBeNull;
        }

        public bool IsEmpty()
        {
            return NullableContext == null && NullableInfo == null && !HasNotNull && !HasCanBeNull && !HasItemNotNull && !HasItemCanBeNull;
        }

        public static NullabilityInfo Empty => new NullabilityInfo(null, null);

        public byte? NullableContext { get; }
        public byte[]? NullableInfo { get; }

        public bool HasNotNull { get; }
        public bool HasCanBeNull { get; }
        public bool HasItemNotNull { get; }
        public bool HasItemCanBeNull { get; }
    }
}