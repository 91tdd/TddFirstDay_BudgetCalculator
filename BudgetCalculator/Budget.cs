using System;

namespace BudgetCalculator
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public int Year => Convert.ToInt16(YearMonth.Substring(0, 4));
        public int Month => Convert.ToInt16(YearMonth.Substring(4, 2));
    }
}