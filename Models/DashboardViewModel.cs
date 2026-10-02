using System.Collections.Generic;

namespace Sublytic.Models
{
    public class DashboardViewModel
    {
        public decimal TotalMonthlySpend { get; set; }

        public decimal TotalAnnualSpend { get; set; }

        public int ActiveSubscriptionsCount { get; set; }

        public int UpcomingRenewalCount { get; set; }

        public decimal AverageSubscriptionPrice { get; set; }

        public string MostExpensiveSubscriptionName { get; set; }

        public decimal MostExpensiveSubscriptionPrice { get; set; }

        public List<Subscription> UpcomingRenewals { get; set; }

        public List<Subscription> RecentSubscriptions { get; set; }

        public List<object> MonthlySpendingTrend { get; set; }

        public List<object> SpendingByCategory { get; set; }

        public List<object> SubscriptionsByBillingCycle { get; set; }

        public DashboardViewModel()
        {
            MostExpensiveSubscriptionName = string.Empty;

            UpcomingRenewals = new List<Subscription>();

            RecentSubscriptions = new List<Subscription>();

            MonthlySpendingTrend = new List<object>();

            SpendingByCategory = new List<object>();

            SubscriptionsByBillingCycle = new List<object>();
        }
    }
}
