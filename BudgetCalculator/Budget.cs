using System;

namespace BudgetCalculator
{
    public class Budget
    {
        public int Amount { get; set; }
        public int Month => Convert.ToInt16(YearMonth.Substring(4, 2));
        public int Year => Convert.ToInt16(YearMonth.Substring(0, 4));
        public string YearMonth { get; set; }

        private decimal DailyAmount => Amount / (decimal)DaysOfMonth;

        private int DaysOfMonth => DateTime.DaysInMonth(Year, Month);

        private DateTime FirstDay => new DateTime(Year, Month, 1);

        private DateTime LastDay => new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));

        public decimal EffectiveAmount(Period period)
        {
            return DailyAmount * period.EffectiveDays(CreatePeriod());
        }

        private Period CreatePeriod()
        {
            return new Period(FirstDay, LastDay);
        }
    }
}