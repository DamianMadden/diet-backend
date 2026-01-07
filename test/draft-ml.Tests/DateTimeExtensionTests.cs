using draft_ml.Extensions;

namespace draft_ml.UnitTests
{
    public class DateTimeExtensionTests
    {
        [Fact]
        public void Given_Ascending_When_CalendarYearDifference_Then_Correct()
        {
            DateTime date1 = new DateTime(2025, 11, 10);
            DateTime date2 = new DateTime(2027, 3, 8);
            Assert.Equal(1, date1.CalendarYearDifference(date2));

            DateTime date3 = new DateTime(2025, 5, 10);
            DateTime date4 = new DateTime(2027, 6, 8);
            Assert.Equal(2, date3.CalendarYearDifference(date4));

            DateTime date5 = new DateTime(2024, 5, 10);
            DateTime date6 = new DateTime(2026, 5, 10);
            Assert.Equal(2, date5.CalendarYearDifference(date6));
        }

        [Fact]
        public void Given_Descending_When_CalendarYearDifference_Then_Correct()
        {
            DateTime date1 = new DateTime(2028, 5, 10);
            DateTime date2 = new DateTime(2024, 6, 8);
            Assert.Equal(3, date1.CalendarYearDifference(date2));

            DateTime date3 = new DateTime(2028, 8, 10);
            DateTime date4 = new DateTime(2024, 3, 8);
            Assert.Equal(4, date3.CalendarYearDifference(date4));

            DateTime date5 = new DateTime(2027, 5, 10);
            DateTime date6 = new DateTime(2024, 5, 10);
            Assert.Equal(3, date5.CalendarYearDifference(date6));
        }
    }
}
