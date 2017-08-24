using System;

namespace TiedUp.Core
{
    public unsafe struct TiedUpSpec
    {
        
        private fixed char id[30];
        private string strId;
        private long start;
        private long end;
        public long Start
        {
            get
            {
                return start;
            }
        }
        public long End
        {
            get
            {
                return end;
            }
        }

        public string Id
        {
            get
            {
                if (strId == null)
                    strId = new string(id, 0, Array.IndexOf(id, '\0'));
                return strId;
            }
        }

        public TiedUpSpec(string id, long start, long end)
        {
            if (end <= start)
                throw new ArgumentException("TiedUp Range invalid. The end is greater than start");

            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("TiedUp Id invalid. You need a not null or empty Id to TiedUp");

            if (id.Length > Constants.MAX_LENGTH_FOR_ID)
                throw new ArgumentException("TiedUp Id must be have a length between 01 and 30 chars");

            strId = null;

            this.start = start;
            this.end = end;



            char[] arId = id.ToCharArray();

            fixed (char* charPtr = id)
            {
                Array.Copy(arId, this.id, Math.Max(30, arId.Length));
            }

            Array.Copy(arId, this.id, arId.Length);
        }
    }
}
