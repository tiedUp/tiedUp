using System;
using Xunit;
using TiedUp.Core;
using System.Runtime.InteropServices;
using System.Text;

namespace test
{
    public class TiedUpSpecTest
    {

        private const long start = 0X80000000000000;
        private const long end = 0X80000000000001;

        [Fact]
        public void tiedUpSpec_must_have_512_bytes()
        {
            int size = Marshal.SizeOf<TiedUpSpec>();
            Assert.Equal(512, size);
        }

        [Fact]
        public void tiedUpSpec_must_have_be_identified_in_a_header()
        {
            string id = new string('A', Constants.MAX_LENGTH_FOR_ID);

            TiedUpSpecBuilder tiedUpSpecBuilder = new TiedUpSpecBuilder();
            TiedUpSpec tiedUpSpec = tiedUpSpecBuilder
                        .SetId(id)
                        .SetStart(start)
                        .SetEnd(end)
                        .Build();

            unsafe
            {
                Assert.Equal((byte)'T', tiedUpSpec.Header[0]);
                Assert.Equal((byte)'I', tiedUpSpec.Header[1]);
                Assert.Equal((byte)'E', tiedUpSpec.Header[2]);
            }            
        }

        [Fact]
        public void tiedUpSpec_must_store_raw_data()
        {
            string expectedId = new string('A', Constants.MAX_LENGTH_FOR_ID);

            TiedUpSpecBuilder tiedUpSpecBuilder = new TiedUpSpecBuilder();
            TiedUpSpec tiedUpSpec = tiedUpSpecBuilder
                        .SetId(expectedId)
                        .SetStart(start)
                        .SetEnd(end)
                        .Build();

            string actual = TiedUpSpecHelper.Id(tiedUpSpec);

            Assert.Equal(expectedId, actual);
            Assert.Equal(start, tiedUpSpec.Start);
            Assert.Equal(end, tiedUpSpec.End);
        }
    }
}