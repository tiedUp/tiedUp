using System.Runtime.InteropServices;

namespace TiedUp.Core
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1)]
    public unsafe struct TiedUpSpec
    {
        [FieldOffset(0)]
        public fixed byte Header[Constants.SIZE_HEADER_ID];
        [FieldOffset(7)]
        public fixed char Id[Constants.MAX_LENGTH_FOR_ID]; // 30 * 2 = 60 Bytes
        [FieldOffset(69)]
        public long Start; // 8 Bytes
        [FieldOffset(77)]
        public long End;   // 8 Bytes
        [FieldOffset(86)]
        public fixed byte Filler[426]; // 512 - sizeof(TiedUpSpec) - Filler
    }
}
