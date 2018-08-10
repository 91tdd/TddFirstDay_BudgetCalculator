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

            if (period.IsQuerySingleMonth())
            {
                var effectiveStart = period.Start;
                var effectiveEnd = period.End;
                var budget = budgetList.FirstOrDefault(a => a.YearMonth == period.Start.ToString("yyyyMM"));
                return budget != null ? budget.DailyAmount() * Period.Days(effectiveStart, effectiveEnd) : 0;
            }
            else
            {
                var totalAmount = 0m;

                var middleStart = new DateTime(period.Start.Year, period.Start.Month, 1);
                var middleEnd = new DateTime(period.End.Year, period.End.Month,
                    DateTime.DaysInMonth(period.End.Year, period.End.Month));

                while (middleStart < middleEnd)
                {
                    var budget = budgetList.FirstOrDefault(a => a.YearMonth == middleStart.ToString("yyyyMM"));
                    if (budget != null)
                    {
                        totalAmount += EffectiveAmount(budget, period);
                    }

                    middleStart = middleStart.AddMonths(1);
                }

                return totalAmount;
            }
        }

        private decimal EffectiveAmount(Budget budget, Period period)
        {
            return budget.DailyAmount() * period.EffectiveDays(budget.CreatePeriod());
        }
    }
}