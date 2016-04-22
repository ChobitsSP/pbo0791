namespace NetworkLib.Utilities
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct ValBytes
    {
        [FieldOffset(0)]
        public byte Byte0;
        [FieldOffset(1)]
        public byte Byte1;
        [FieldOffset(2)]
        public byte Byte2;
        [FieldOffset(3)]
        public byte Byte3;
        [FieldOffset(4)]
        public byte Byte4;
        [FieldOffset(5)]
        public byte Byte5;
        [FieldOffset(6)]
        public byte Byte6;
        [FieldOffset(7)]
        public byte Byte7;
        [FieldOffset(0)]
        public double DoubleVal;
        [FieldOffset(0)]
        public float FloatVal;
        [FieldOffset(0)]
        public int IntVal;
        [FieldOffset(0)]
        public long LongVal;
        [FieldOffset(0)]
        public short ShortVal;
    }
}

