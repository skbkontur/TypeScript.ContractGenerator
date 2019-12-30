using System;
using System.Numerics;
using System.Reflection;

using JetBrains.Annotations;

using SkbKontur.TypeScript.ContractGenerator.Extensions;

namespace SkbKontur.TypeScript.ContractGenerator
{
    internal class TypeDeclarationKey
    {
        public TypeDeclarationKey([NotNull] Type type, [CanBeNull] ICustomAttributeProvider customAttributeProvider)
        {
            this.type = type;
            required = customAttributeProvider?.IsNameDefined(AnnotationsNames.Required) ?? false;
            notNull = customAttributeProvider?.IsNameDefined(AnnotationsNames.NotNull) ?? false;
            canBeNull = customAttributeProvider?.IsNameDefined(AnnotationsNames.CanBeNull) ?? false;
            itemNotNull = customAttributeProvider?.IsNameDefined(AnnotationsNames.ItemNotNull) ?? false;
            itemCanBeNull = customAttributeProvider?.IsNameDefined(AnnotationsNames.ItemCanBeNull) ?? false;
            nullableFlags = TypeScriptGeneratorHelpers.GetNullableFlags(customAttributeProvider);
        }

        public override string ToString()
        {
            return $"Type: {type.Name}, NN: {notNull}, CBN: {canBeNull}, INN: {itemNotNull}, ICBN: {itemCanBeNull}, R: {required}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = type.GetHashCode();
                hashCode = (hashCode * 397) ^ notNull.GetHashCode();
                hashCode = (hashCode * 397) ^ canBeNull.GetHashCode();
                hashCode = (hashCode * 397) ^ itemNotNull.GetHashCode();
                hashCode = (hashCode * 397) ^ itemCanBeNull.GetHashCode();
                hashCode = (hashCode * 397) ^ required.GetHashCode();
                hashCode = (hashCode * 397) ^ GetFlagsHash();
                return hashCode;
            }
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TypeDeclarationKey)obj);
        }

        private bool Equals(TypeDeclarationKey other)
        {
            return type == other.type
                   && notNull == other.notNull
                   && canBeNull == other.canBeNull
                   && itemNotNull == other.itemNotNull
                   && itemCanBeNull == other.itemCanBeNull
                   && required == other.required
                   && NullableFlagsEquals(other.nullableFlags);
        }

        private bool NullableFlagsEquals(byte[] other)
        {
            if (nullableFlags.Length != other.Length)
                return false;

            for (var i = 0; i < nullableFlags.Length; i++)
            {
                if (nullableFlags[i] != other[i])
                    return false;
            }

            return true;
        }

        private int GetFlagsHash()
        {
            return new BigInteger(nullableFlags).GetHashCode();
        }

        [NotNull]
        private readonly Type type;

        private readonly bool notNull;
        private readonly bool canBeNull;
        private readonly bool itemNotNull;
        private readonly bool itemCanBeNull;

        [NotNull]
        private readonly byte[] nullableFlags;

        private readonly bool required;
    }
}