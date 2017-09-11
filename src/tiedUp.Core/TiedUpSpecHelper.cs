using System;
using System.Runtime.InteropServices;

namespace TiedUp.Core
{
    public static class TiedUpSpecHelper
    {
        public static string Id(TiedUpSpec tiedUpSpec)
        {
            unsafe
            {
                char* pId = tiedUpSpec.Id;
                string strId = new string(pId);
                strId = strId.TrimEnd('\0');
                return strId;
            }
        }
    }
}