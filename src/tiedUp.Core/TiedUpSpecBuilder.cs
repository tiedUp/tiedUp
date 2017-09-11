using System;
using System.Runtime.InteropServices;

namespace TiedUp.Core
{
    public class TiedUpSpecBuilder
    {
        private long start = 0;
        private long end = 0;
        private string id;
        public TiedUpSpecBuilder SetStart(long start)
        {
            this.start = start;
            return this;
        }

        public TiedUpSpecBuilder SetEnd(long end)
        {
            this.end = end;
            return this;
        }

        public TiedUpSpecBuilder SetId(string id)
        {
            this.id = id;
            return this;
        }

        public TiedUpSpec Build()
        {
            if (end <= start)
                throw new ArgumentException("TiedUp Range invalid. The end is greater than start");

            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("TiedUp Id invalid. You need a not null or empty Id to TiedUp");

            if (id.Length > Constants.MAX_LENGTH_FOR_ID)
                throw new ArgumentException("TiedUp Id must be have a length between 01 and 30 chars");

            TiedUpSpec tiedUpSpec = new TiedUpSpec();           


            tiedUpSpec.Start = this.start;
            tiedUpSpec.End = this.end;

            unsafe
            {
                // Initializing Header from struct
                tiedUpSpec.Header[0] = (byte)'T';
                tiedUpSpec.Header[1] = (byte)'I';
                tiedUpSpec.Header[2] = (byte)'E';
                
                char[] managedCharArray = id.ToCharArray();
                char* pId = tiedUpSpec.Id;

                Marshal.Copy(managedCharArray, 0, (IntPtr)pId, Math.Min(id.Length, Constants.MAX_LENGTH_FOR_ID));
            }

            return tiedUpSpec;
        }
    }

}