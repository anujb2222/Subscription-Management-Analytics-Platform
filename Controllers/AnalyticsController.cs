using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sublytic.Models;

namespace Sublytic.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly SublyticDbContext db;

        public AnalyticsController(SublyticDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var subscriptions = db.Subscriptions
                .Where(s => s.IsActive)
                .ToList();

            ViewBag.MonthlySpend =
                HomeController.CalculateMonthlySpend(subscriptions);

            ViewBag.AnnualSpend =
                HomeController.CalculateAnnualSpend(subscriptions);

            ViewBag.TotalSubscriptions =
                subscriptions.Count;

            return View();
        }
    }
}
