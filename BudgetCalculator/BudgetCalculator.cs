using System;
using System.Linq;

namespace BudgetCalculator
{
    public class BudgetCalculator
    {
        private IBudgetRepo _repo;

        public BudgetCalculator(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public decimal TotalAmount(DateTime start, DateTime end)
        {
            var period = new Period(start, end);

            var budgetList = _repo.GetAll();
            if (budgetList.Count == 0)
            {
                return 0;
            }

            var totalAmount = 0m;

            var middleStart = new DateTime(period.Start.Year, period.Start.Month, 1);
            var middleEnd = new DateTime(period.End.Year, period.End.Month,
                DateTime.DaysInMonth(period.End.Year, period.End.Month));

            while (middleStart < middleEnd)
            {
                var budget = budgetList.FirstOrDefault(a => a.YearMonth == middleStart.ToString("yyyyMM"));
                if (budget != null)
                {
                    totalAmount += budget.EffectiveAmount(period);
                }

                middleStart = middleStart.AddMonths(1);
            }

            return totalAmount;
        }
    }
}