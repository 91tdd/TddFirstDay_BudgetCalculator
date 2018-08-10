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

        public DateTime End { get; private set; }
        public DateTime Start { get; private set; }

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

            return (effectiveEnd - effectiveStart).Days + 1;
        }

        private bool HasNoOverlap(Period otherPeriod)
        {
            return Start > otherPeriod.End || End < otherPeriod.Start;
        }
    }
}