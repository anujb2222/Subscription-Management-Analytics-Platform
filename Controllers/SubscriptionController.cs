using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sublytic.Models;

namespace Sublytic.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly SublyticDbContext db;
        private readonly UserManager<IdentityUser> userManager;

        public SubscriptionController(
            SublyticDbContext context,
            UserManager<IdentityUser> userManager)
        {
            db = context;
            this.userManager = userManager;
        }

        

        [HttpGet]
        public IActionResult Index()
        {
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var subscriptions = db.Subscriptions
                .Include(s => s.Category)
                .Where(s => s.UserId == userId)
                .ToList();

            return View(subscriptions);
        }

    

        [HttpGet]
        public IActionResult Details(int id)
        {
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var subscription = db.Subscriptions
                .Include(s => s.Category)
                .FirstOrDefault(s =>
                    s.SubscriptionId == id &&
                    s.UserId == userId);

            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

       

        [HttpGet]
        public IActionResult Create()
        {
            LoadCategories();

            return View();
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Subscription subscription)
        {
            
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

           
            subscription.UserId = userId;

          
            ModelState.Remove(nameof(Subscription.UserId));

            if (!ModelState.IsValid)
            {
                LoadCategories();

                return View(subscription);
            }

            subscription.CreatedAt = DateTime.Now;
            subscription.IsActive = true;

            db.Subscriptions.Add(subscription);
            db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // EDIT - GET
        // =========================================================

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var subscription = db.Subscriptions
                .FirstOrDefault(s =>
                    s.SubscriptionId == id &&
                    s.UserId == userId);

            if (subscription == null)
            {
                return NotFound();
            }

            LoadCategories();

            return View(subscription);
        }

        // =========================================================
        // EDIT - POST
        // =========================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Subscription subscription)
        {
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            ModelState.Remove(nameof(Subscription.UserId));

            if (!ModelState.IsValid)
            {
                LoadCategories();

                return View(subscription);
            }

            var existingSubscription = db.Subscriptions
                .FirstOrDefault(s =>
                    s.SubscriptionId == subscription.SubscriptionId &&
                    s.UserId == userId);

            if (existingSubscription == null)
            {
                return NotFound();
            }

            // Do not allow UserId to be changed.
            existingSubscription.Name =
                subscription.Name;

            existingSubscription.Description =
                subscription.Description;

            existingSubscription.Price =
                subscription.Price;

            existingSubscription.BillingCycle =
                subscription.BillingCycle;

            existingSubscription.StartDate =
                subscription.StartDate;

            existingSubscription.NextRenewalDate =
                subscription.NextRenewalDate;

            existingSubscription.IsActive =
                subscription.IsActive;

            existingSubscription.AutoRenew =
                subscription.AutoRenew;

            existingSubscription.CategoryId =
                subscription.CategoryId;

            existingSubscription.WebsiteUrl =
                subscription.WebsiteUrl;

            existingSubscription.AccountEmail =
                subscription.AccountEmail;

            existingSubscription.Notes =
                subscription.Notes;

            existingSubscription.UpdatedAt =
                DateTime.Now;

            db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var subscription = db.Subscriptions
                .Include(s => s.Category)
                .FirstOrDefault(s =>
                    s.SubscriptionId == id &&
                    s.UserId == userId);

            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var subscription = db.Subscriptions
                .FirstOrDefault(s =>
                    s.SubscriptionId == id &&
                    s.UserId == userId);

            if (subscription != null)
            {
                db.Subscriptions.Remove(subscription);
                db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        

        private void LoadCategories()
        {
            ViewBag.Categories = db.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToList();
        }
    }
}