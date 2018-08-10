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
            var effectiveEnd = End;
            if (budget.LastDay() < End)
            {
                effectiveEnd = budget.LastDay();
            }

            var effectiveStart = Start;
            if (budget.FirstDay() > Start)
            {
                effectiveStart = budget.FirstDay();
            }

            return Period.Days(effectiveStart, effectiveEnd);
        }
    }
}