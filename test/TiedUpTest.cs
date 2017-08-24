using System;
using Xunit;
using TiedUp.Core;

namespace test
{
    public class TiedUpTest
    {
        [Fact]
        public void tiedUp_storage_with_different_tiedUpSpec()
        {
            TiedUp.Core.TiedUpSpec schedulledDaysSpec = new TiedUp.Core.TiedUpSpec("schedulledDays", 1, 31);

            TiedUp.Core.TiedUp tiedUp = new TiedUp.Core.TiedUp(schedulledDaysSpec);

            int r = tiedUp.Mark(0);

            TiedUp.Core.TiedUpSpec anotherSpec = new TiedUp.Core.TiedUpSpec("schedulledDays", 1, 30);

            TiedUp.Core.TiedUp anotherTiedUp = new TiedUp.Core.TiedUp(anotherSpec);

            r = anotherTiedUp.Mark(0);

            Assert.Equal(r, TiedUp.Core.TiedUp.ERROR_TIEDUP_DIFFERENT_TIEDUP_SPEC);
        }

        [Fact]
        public void mark_outofrange_specified()
        {
            TiedUp.Core.TiedUpSpec schedulledDaysSpec = new TiedUp.Core.TiedUpSpec("schedulledDays", 1, 31);

            TiedUp.Core.TiedUp tiedUp = new TiedUp.Core.TiedUp(schedulledDaysSpec);

            int r = tiedUp.Mark(0);

            Assert.Equal(TiedUp.Core.TiedUp.ERROR_OUT_OF_RANGE, r);
        }

        [Fact]
        public void query_marked_outofrange_specified()
        {
            TiedUp.Core.TiedUpSpec schedulledDays = new TiedUp.Core.TiedUpSpec("schedulledDays", 1, 31);

            TiedUp.Core.TiedUp tiedUp = new TiedUp.Core.TiedUp(schedulledDays);

            bool marked = false;

            int r = tiedUp.Marked(0, ref marked);

            Assert.Equal(TiedUp.Core.TiedUp.ERROR_OUT_OF_RANGE, r);
        }
    }
}
