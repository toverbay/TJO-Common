using System;
using System.Collections.Generic;
using System.Text;

namespace TJO.TestTools.UnitTesting.Should.Core
{
    public abstract class DatePrecision
    {
        public static DatePrecision Second = new SecondPrecision();
        public static DatePrecision Minute = new MinutePrecision();
        public static DatePrecision Hour = new HourPrecision();
        public static DatePrecision Day = new DayPrecision();
        public static DatePrecision Month = new MonthPrecision();
        public static DatePrecision Year = new YearPrecision();

        public abstract DateTime Truncate(DateTime date);
        public abstract DateTimeOffset Truncate(DateTimeOffset date);

        public class SecondPrecision : DatePrecision
        {
            public override DateTime Truncate(DateTime date)
            {
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            }

            public override DateTimeOffset Truncate(DateTimeOffset date)
            {
                return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Offset);
            }
        }

        public class MinutePrecision : DatePrecision
        {
            public override DateTime Truncate(DateTime date)
            {
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
            }

            public override DateTimeOffset Truncate(DateTimeOffset date)
            {
                return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, date.Offset);
            }
        }

        public class HourPrecision : DatePrecision
        {
            public override DateTime Truncate(DateTime date)
            {
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
            }

            public override DateTimeOffset Truncate(DateTimeOffset date)
            {
                return new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, 0, 0, date.Offset);
            }
        }

        public class DayPrecision : DatePrecision
        {
            public override DateTime Truncate(DateTime date)
            {
                return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            }

            public override DateTimeOffset Truncate(DateTimeOffset date)
            {
                return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, date.Offset);
            }
        }

        public class MonthPrecision : DatePrecision
        {
            public override DateTime Truncate(DateTime date)
            {
                return new DateTime(date.Year, date.Month, 0, 0, 0, 0);
            }

            public override DateTimeOffset Truncate(DateTimeOffset date)
            {
                return new DateTimeOffset(date.Year, date.Month, 0, 0, 0, 0, date.Offset);
            }
        }

        public class YearPrecision : DatePrecision
        {
            public override DateTime Truncate(DateTime date)
            {
                return new DateTime(date.Year, 0, 0, 0, 0, 0);
            }

            public override DateTimeOffset Truncate(DateTimeOffset date)
            {
                return new DateTimeOffset(date.Year, 0, 0, 0, 0, 0, date.Offset);
            }
        }
    }

}
