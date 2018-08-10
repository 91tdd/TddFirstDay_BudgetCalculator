﻿using System;
using System.Linq;

namespace BudgetCalculator
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException();

            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public bool IsQuerySingleMonth()
        {
            return Start.ToString("yyyyMM") == End.ToString("yyyyMM");
        }
    }

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
                return EffectiveAmount(effectiveStart, effectiveEnd, budget);
            }
            else
            {
                var totalAmount = 0m;

                //var startAmount = StartAmount(period, budgetList.FirstOrDefault(a => a.YearMonth == period.Start.ToString("yyyyMM")));
                //totalAmount += startAmount;

                //var lastAmount = LastAmount(period, budgetList.FirstOrDefault(a => a.YearMonth == period.End.ToString("yyyyMM")));
                //totalAmount += lastAmount;

                //var middleStart = new DateTime(period.Start.Year, period.Start.Month, 1).AddMonths(1);
                //var middleEnd = new DateTime(period.End.Year, period.End.Month, DateTime.DaysInMonth(period.End.Year, period.End.Month)).AddMonths(-1);

                var middleStart = new DateTime(period.Start.Year, period.Start.Month, 1);
                var middleEnd = new DateTime(period.End.Year, period.End.Month,
                    DateTime.DaysInMonth(period.End.Year, period.End.Month));

                while (middleStart < middleEnd)
                {
                    var budget = budgetList.FirstOrDefault(a => a.YearMonth == middleStart.ToString("yyyyMM"));
                    if (budget != null)
                    {
                        if (budget.YearMonth == period.Start.ToString("yyyyMM"))
                        {
                            var startAmount = StartAmount(period, budgetList.FirstOrDefault(a => a.YearMonth == middleStart.ToString("yyyyMM")));
                            totalAmount += startAmount;
                        }
                        else if (budget.YearMonth == period.End.ToString("yyyyMM"))
                        {
                            var lastAmount = LastAmount(period, budgetList.FirstOrDefault(a => a.YearMonth == middleStart.ToString("yyyyMM")));
                            totalAmount += lastAmount;
                        }
                        else
                        {
                            totalAmount += budget?.Amount ?? 0;
                        }
                    }
                    middleStart = middleStart.AddMonths(1);
                }

                return totalAmount;
            }
        }

        private decimal LastAmount(Period period, Budget budget)
        {
            var lastMonthStartDay = new DateTime(budget.Year, budget.Month, 1);
            return EffectiveAmount(lastMonthStartDay, period.End, budget);
        }

        private decimal StartAmount(Period period, Budget budget)
        {
            var firstMonthEndDay = new DateTime(budget.Year, budget.Month, DateTime.DaysInMonth(period.Start.Year, period.Start.Month));

            return EffectiveAmount(period.Start, firstMonthEndDay, budget);
        }

        private decimal EffectiveAmount(DateTime start, DateTime end, Budget budget)
        {
            if (budget != null)
            {
                int dayDiffs = (end - start).Days + 1;

                return budget.DailyAmount() * (dayDiffs);
            }
            else
                return 0;
        }
    }
}