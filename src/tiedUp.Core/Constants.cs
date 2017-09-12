using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("test")]

namespace TiedUp.Core
{
    internal static class Constants
    {
        public const int SIZE_HEADER_ID = 6;
        public const int MAX_LENGTH_FOR_ID = 30;
    }
}