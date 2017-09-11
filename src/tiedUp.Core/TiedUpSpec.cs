using System.Runtime.InteropServices;

namespace TiedUp.Core
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi, Pack = 1)]
    public unsafe struct TiedUpSpec
    {
        [FieldOffset(0)]
        public fixed byte Header[Constants.SIZE_HEADER_ID];
        [FieldOffset(4)]
        public fixed char Id[Constants.MAX_LENGTH_FOR_ID]; // 30 * 2 = 60 Bytes
        [FieldOffset(66)]
        public long Start; // 8 Bytes
        [FieldOffset(74)]
        public long End;   // 8 Bytes
        [FieldOffset(83)]
        public fixed byte Filler[429]; // 512 - sizeof(TiedUpSpec) - Filler
    }
}
