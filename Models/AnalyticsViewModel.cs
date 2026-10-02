
using System;
using System.Collections.Generic;

namespace Sublytic.Models
{
    public class AnalyticsViewModel
    {
        public DashboardViewModel DashboardSummary { get; set; }
        public Dictionary<string, decimal> YearlySpendingTrend { get; set; }
        public Dictionary<string, decimal> QuarterlySpending { get; set; }
        public List<CategorySpendingBreakdown> CategoryBreakdown { get; set; }
        public List<BillingCycleBreakdown> BillingCycleBreakdown { get; set; }
        public List<Subscription> ExpiringSoon { get; set; }
        public List<Subscription> HighestValueSubscriptions { get; set; }
        public decimal PotentialSavingsFromInactive { get; set; }
        public int NewSubscriptionsThisMonth { get; set; }
        public int CancelledSubscriptionsThisMonth { get; set; }
        public decimal MonthOverMonthChange { get; set; }
        public double MonthOverMonthPercentage { get; set; }
        public decimal ProjectedAnnualSpend { get; set; }
        public decimal TotalSpentLast12Months { get; set; }
    }

    public class CategorySpendingBreakdown
    {
        public string CategoryName { get; set; }
        public string CategoryColor { get; set; }
        public decimal MonthlyAmount { get; set; }
        public decimal AnnualAmount { get; set; }
        public int SubscriptionCount { get; set; }
        public double PercentageOfTotal { get; set; }
    }

    public class BillingCycleBreakdown
    {
        public string CycleName { get; set; }
        public int SubscriptionCount { get; set; }
        public decimal TotalAmount { get; set; }
        public double PercentageOfTotal { get; set; }
    }
}
