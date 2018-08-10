using System;

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

            var totalAmount = 0m;

            foreach (var budget in _repo.GetAll())
            {
                totalAmount += budget.EffectiveAmount(period);
            }
            return totalAmount;
        }
    }
}