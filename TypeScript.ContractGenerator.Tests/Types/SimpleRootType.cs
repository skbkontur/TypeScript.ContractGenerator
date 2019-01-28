using System;

namespace SKBKontur.Catalogue.WebWorms.Tests.FlowTypeGeneratorTests.Types
{
    public class SimpleRootType
    {
        public long Long { get; set; }
        public ulong ULong { get; set; }
        public int Int { get; set; }
        public uint UInt { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public bool Bool { get; set; }
        public double Double { get; set; }
        public float Float { get; set; }
        public decimal Decimal { get; set; }
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public char Char { get; set; }
        public string String { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public Guid Guid { get; set; }
    }
}