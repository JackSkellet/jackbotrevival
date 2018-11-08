using System;

namespace jack.Extensions
{
    class DaySufix
    {
        public static string DSufix(int DayInt)
        {
            String[] suffixes = { "th", "st", "nd", "rd", "th", "th", "th", "th", "th", "th", "th", "th", "th", "th", "th", "th", "th", "th", "th", "th", "th", "st", "nd", "rd", "th", "th", "th", "th", "th", "th", "th", "st" };

            String dayStr = DayInt + suffixes[DayInt];
            return dayStr;
        }
    }

    class TimeSufix
    {
        public static string yearSufixs(DateTime date1)
        {

            DateTime date2 = DateTime.UtcNow.Date;

            int dday = date2.Day;
            while (dday == date2.Day)
            {
                date1 = date1.AddDays(-1);
                date2 = date2.AddDays(-1);
            }
            int years = 0, days = 0;
            while (date2.CompareTo(date1) >= 0)
            {
                years++;
                date2 = date2.AddYears(-1);
            }
            date2 = date2.AddYears(1);
            years--;

            while (date2.CompareTo(date1) >= 0)
            {
                days++;
                date2 = date2.AddDays(-1);
            }
            date2 = date2.AddDays(1);
            days--;
            return years.ToString();
        }

        public static string daySufixs(DateTime date1)
        {

            DateTime date2 = DateTime.UtcNow.Date;

            int dday = date2.Day;
            while (dday == date2.Day)
            {
                date1 = date1.AddDays(-1);
                date2 = date2.AddDays(-1);
            }
            int years = 0, days = 0;
            while (date2.CompareTo(date1) >= 0)
            {
                years++;
                date2 = date2.AddYears(-1);
            }
            date2 = date2.AddYears(1);
            years--;

            while (date2.CompareTo(date1) >= 0)
            {
                days++;
                date2 = date2.AddDays(-1);
            }
            date2 = date2.AddDays(1);
            days--;
            return days.ToString();
        }
    }
}