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
    }
}