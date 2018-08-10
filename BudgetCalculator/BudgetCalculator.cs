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
                return EffectiveAmount(budget, Period.Days(effectiveStart, effectiveEnd));
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
                        var effectiveDays = period.EffectiveDays(budget.CreatePeriod());
                        totalAmount += EffectiveAmount(budget, effectiveDays);
                    }

                    middleStart = middleStart.AddMonths(1);
                }

                return totalAmount;
            }
        }

        private decimal EffectiveAmount(Budget budget, int effectiveDays)
        {
            if (budget != null)
            {
                return budget.DailyAmount() * (effectiveDays);
            }
            else
                return 0;
        }
    }
}