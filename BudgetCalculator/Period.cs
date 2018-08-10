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

        public int EffectiveDays(Period otherPeriod)
        {
            if (HasNoOverlap(otherPeriod))
            {
                return 0;
            }

            var effectiveEnd = End;
            if (otherPeriod.End < End)
            {
                effectiveEnd = otherPeriod.End;
            }

            var effectiveStart = Start;
            if (otherPeriod.Start > Start)
            {
                effectiveStart = otherPeriod.Start;
            }

            return Period.Days(effectiveStart, effectiveEnd);
        }

        private bool HasNoOverlap(Period otherPeriod)
        {
            return Start > otherPeriod.End || End < otherPeriod.Start;
        }
    }
}