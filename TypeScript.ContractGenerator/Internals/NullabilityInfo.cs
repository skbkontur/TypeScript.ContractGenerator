using System;
using System.Linq;

using SkbKontur.TypeScript.ContractGenerator.Abstractions;

namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    public class NullabilityInfo
    {
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
            return new NullabilityInfo(NullableContext, NullableInfo, HasItemNotNull, HasItemCanBeNull, false, false);
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
            if (nullabilityMode == NullabilityMode.NullableReference)
                return (NullableInfo?[0] ?? NullableContext) == 2;
            return nullabilityMode == NullabilityMode.Pessimistic ? !HasNotNull : HasCanBeNull;
        }

        public byte? NullableContext { get; }
        public byte[]? NullableInfo { get; }

        public bool HasNotNull { get; }
        public bool HasCanBeNull { get; }
        public bool HasItemNotNull { get; }
        public bool HasItemCanBeNull { get; }
    }
}