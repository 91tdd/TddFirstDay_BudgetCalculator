using System;

namespace BudgetCalculator
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public int Year => Convert.ToInt16(YearMonth.Substring(0, 4));
        public int Month => Convert.ToInt16(YearMonth.Substring(4, 2));

        public int DaysOfMonth()
        {
            return DateTime.DaysInMonth(Year, Month);
        }

        public decimal DailyAmount()
        {
            return Amount / (decimal)DaysOfMonth();
        }

        public DateTime LastDay()
        {
            return new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
        }

        public DateTime FirstDay()
        {
            var effectiveStart = new DateTime(Year, Month, 1);
            return effectiveStart;
        }

        public Period CreatePeriod()
        {
            var otherPeriod = new Period(FirstDay(), LastDay());
            return otherPeriod;
        }

        public decimal EffectiveAmount(Period period)
        {
            return this.DailyAmount() * period.EffectiveDays(this.CreatePeriod());
        }
    }
}