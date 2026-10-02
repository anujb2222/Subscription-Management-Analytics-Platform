using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sublytic.Models;

namespace Sublytic.Controllers
{
    public class HomeController : Controller
    {
        private readonly SublyticDbContext db;

        public HomeController(SublyticDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var subscriptions = db.Subscriptions.ToList();

            var activeSubscriptions = subscriptions
                .Where(s => s.IsActive)
                .ToList();

            var monthlySpend = CalculateMonthlySpend(activeSubscriptions);
            var annualSpend = CalculateAnnualSpend(activeSubscriptions);

            var upcomingRenewals = activeSubscriptions
                .Where(s => s.NextRenewalDate >= DateTime.Today)
                .OrderBy(s => s.NextRenewalDate)
                .Take(5)
                .ToList();

            var upcomingRenewalCount = activeSubscriptions
                .Count(s =>
                    s.NextRenewalDate >= DateTime.Today &&
                    s.NextRenewalDate <= DateTime.Today.AddDays(30));

            var averageSubscriptionPrice = activeSubscriptions.Any()
                ? monthlySpend / activeSubscriptions.Count
                : 0m;

            var mostExpensiveSubscription = activeSubscriptions
                .OrderByDescending(s => GetMonthlyPrice(s))
                .FirstOrDefault();

            var recentSubscriptions = subscriptions
                .OrderByDescending(s => s.CreatedAt)
                .Take(5)
                .ToList();

            var model = new DashboardViewModel
            {
                TotalMonthlySpend = monthlySpend,

                TotalAnnualSpend = annualSpend,

                ActiveSubscriptionsCount =
                    activeSubscriptions.Count,

                UpcomingRenewals =
                    upcomingRenewals,

                UpcomingRenewalCount =
                    upcomingRenewalCount,

                AverageSubscriptionPrice =
                    averageSubscriptionPrice,

                RecentSubscriptions =
                    recentSubscriptions,

                MostExpensiveSubscriptionName =
                    mostExpensiveSubscription?.Name
                    ?? string.Empty,

                MostExpensiveSubscriptionPrice =
                    mostExpensiveSubscription != null
                        ? GetMonthlyPrice(mostExpensiveSubscription)
                        : 0m,

                MonthlySpendingTrend =
                    BuildMonthlySpendingTrend(activeSubscriptions),

                SpendingByCategory =
                    BuildSpendingByCategory(activeSubscriptions),

                SubscriptionsByBillingCycle =
                    BuildBillingCycleData(activeSubscriptions)
            };

            return View(model);
        }

        // ============================================================
        // MONTHLY SPENDING
        // ============================================================

        public static decimal CalculateMonthlySpend(
            List<Subscription> subscriptions)
        {
            decimal total = 0m;

            foreach (var subscription in subscriptions)
            {
                total += GetMonthlyPrice(subscription);
            }

            return Math.Round(total, 2);
        }

        // ============================================================
        // ANNUAL SPENDING
        // ============================================================

        public static decimal CalculateAnnualSpend(
            List<Subscription> subscriptions)
        {
            decimal total = 0m;

            foreach (var subscription in subscriptions)
            {
                switch (subscription.BillingCycle)
                {
                    case BillingCycle.Weekly:
                        total += subscription.Price * 52m;
                        break;

                    case BillingCycle.Monthly:
                        total += subscription.Price * 12m;
                        break;

                    case BillingCycle.Quarterly:
                        total += subscription.Price * 4m;
                        break;

                    case BillingCycle.SemiAnnually:
                        total += subscription.Price * 2m;
                        break;

                    case BillingCycle.Annually:
                        total += subscription.Price;
                        break;
                }
            }

            return Math.Round(total, 2);
        }

        // ============================================================
        // MONTHLY PRICE FOR ONE SUBSCRIPTION
        // ============================================================

        private static decimal GetMonthlyPrice(
            Subscription subscription)
        {
            switch (subscription.BillingCycle)
            {
                case BillingCycle.Weekly:
                    return subscription.Price * 52m / 12m;

                case BillingCycle.Monthly:
                    return subscription.Price;

                case BillingCycle.Quarterly:
                    return subscription.Price / 3m;

                case BillingCycle.SemiAnnually:
                    return subscription.Price / 6m;

                case BillingCycle.Annually:
                    return subscription.Price / 12m;

                default:
                    return 0m;
            }
        }

        // ============================================================
        // MONTHLY SPENDING TREND
        // ============================================================

        private List<object> BuildMonthlySpendingTrend(
            List<Subscription> subscriptions)
        {
            var result = new List<object>();

            var today = DateTime.Today;

            for (int i = 5; i >= 0; i--)
            {
                var month = today.AddMonths(-i);

                var monthlyTotal = subscriptions
                    .Where(s =>
                        s.CreatedAt.Year < month.Year ||
                        (
                            s.CreatedAt.Year == month.Year &&
                            s.CreatedAt.Month <= month.Month
                        ))
                    .Sum(GetMonthlyPrice);

                result.Add(new
                {
                    month = month.ToString("MMM"),
                    amount = Math.Round(monthlyTotal, 2)
                });
            }

            return result;
        }

        // ============================================================
        // SPENDING BY CATEGORY
        // ============================================================

        private List<object> BuildSpendingByCategory(
            List<Subscription> subscriptions)
        {
            var result = subscriptions
                .GroupBy(s => s.Category)
                .Select(g => new
                {
                    category =
                        g.Key?.Name ?? "Other",

                    amount = Math.Round(
                        g.Sum(GetMonthlyPrice),
                        2)
                })
                .OrderByDescending(x => x.amount)
                .ToList();

            return result
                .Cast<object>()
                .ToList();
        }

        // ============================================================
        // BILLING CYCLES
        // ============================================================

        private List<object> BuildBillingCycleData(
            List<Subscription> subscriptions)
        {
            var result = subscriptions
                .GroupBy(s => s.BillingCycle)
                .Select(g => new
                {
                    cycle = g.Key.ToString(),
                    count = g.Count()
                })
                .OrderByDescending(x => x.count)
                .ToList();

            return result
                .Cast<object>()
                .ToList();
        }

     
        public IActionResult Error()
        {
            return View();
        }
    }
}