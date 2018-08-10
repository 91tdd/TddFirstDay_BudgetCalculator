using System;

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

        public static int Days(DateTime start, DateTime end)
        {
            int dayDiffs = (end - start).Days + 1;
            return dayDiffs;
        }

        public int EffectiveDays(Budget budget)
        {
            var effectiveStart = Start;
            var effectiveEnd = End;
            if (budget.YearMonth == Start.ToString("yyyyMM"))
            {
                effectiveEnd = budget.LastDay();
            }
            else if (budget.YearMonth == End.ToString("yyyyMM"))
            {
                effectiveStart = budget.FirstDay();
            }
            else
            {
                effectiveStart = budget.FirstDay();
                effectiveEnd = budget.LastDay();
            }

            var effectiveDays = Period.Days(effectiveStart, effectiveEnd);
            return effectiveDays;
        }
    }
}