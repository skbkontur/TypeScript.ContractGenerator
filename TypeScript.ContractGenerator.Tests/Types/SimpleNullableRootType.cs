using System;

namespace SkbKontur.TypeScript.ContractGenerator.Tests.Types
{
    public class SimpleNullableRootType
    {
        public long? Long { get; set; }
        public ulong? ULong { get; set; }
        public int? Int { get; set; }
        public uint? UInt { get; set; }
        public short? Short { get; set; }
        public ushort? UShort { get; set; }
        public bool? Bool { get; set; }
        public double? Double { get; set; }
        public decimal? Decimal { get; set; }
        public float? Float { get; set; }
        public byte? Byte { get; set; }
        public sbyte? SByte { get; set; }
        public char? Char { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTimeOffset? DateTimeOffset { get; set; }
        public TimeSpan? TimeSpan { get; set; }
        public Guid? Guid { get; set; }
    }
}