
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sublytic.Models
{
    public static class SublyticInitializer
    {
        public static void Seed(SublyticDbContext context)
        {
            if (context.Categories.Any()) return;

            var categories = new List<Category>
            {
                new() { Name = "Entertainment", Description = "Streaming services, music, movies", ColorHex = "#667eea", IsActive = true },
                new() { Name = "Productivity", Description = "Office tools, cloud storage, development", ColorHex = "#f093fb", IsActive = true },
                new() { Name = "Utilities", Description = "Cloud services, hosting, domain", ColorHex = "#4facfe", IsActive = true },
                new() { Name = "Fitness & Health", Description = "Gym memberships, health apps", ColorHex = "#43e97b", IsActive = true },
                new() { Name = "Education", Description = "Learning platforms, courses", ColorHex = "#fa709a", IsActive = true },
                new() { Name = "Lifestyle", Description = "Food delivery, beauty, subscription boxes", ColorHex = "#ffd700", IsActive = true },
                new() { Name = "News & Media", Description = "Newspapers, magazines, news sites", ColorHex = "#00f2fe", IsActive = true },
                new() { Name = "Software", Description = "Licensed software, SaaS tools", ColorHex = "#a8edea", IsActive = true }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var now = DateTime.Now;
            var subscriptions = new List<Subscription>
            {
                new() { Name = "Netflix Premium", Description = "4K streaming subscription", Price = 15.99m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddYears(-2).AddDays(15), NextRenewalDate = now.AddDays(5), IsActive = true, AutoRenew = true, CategoryId = 1, WebsiteUrl = "https://www.netflix.com", AccountEmail = "user@example.com", Notes = "Family plan shared with 2 others", CreatedAt = now.AddYears(-2), UpdatedAt = now.AddMonths(-1) },
                new() { Name = "Spotify Premium", Description = "Ad-free music streaming", Price = 9.99m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddYears(-2).AddMonths(2).AddDays(20), NextRenewalDate = now.AddDays(12), IsActive = true, AutoRenew = true, CategoryId = 1, WebsiteUrl = "https://www.spotify.com", AccountEmail = "user@example.com", Notes = "Individual plan", CreatedAt = now.AddYears(-2).AddMonths(2), UpdatedAt = now.AddMonths(-1) },
                new() { Name = "Microsoft 365 Family", Description = "Office apps for up to 6 people", Price = 99.99m, BillingCycle = BillingCycle.Annually, StartDate = now.AddYears(-3).AddMonths(-1), NextRenewalDate = now.AddDays(30), IsActive = true, AutoRenew = true, CategoryId = 2, WebsiteUrl = "https://www.microsoft.com", AccountEmail = "user@example.com", Notes = "Includes 1TB OneDrive per person", CreatedAt = now.AddYears(-3).AddMonths(-1), UpdatedAt = now.AddMonths(-2) },
                new() { Name = "Adobe Creative Cloud", Description = "All apps creative suite", Price = 54.99m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddYears(-1).AddMonths(5).AddDays(10), NextRenewalDate = now.AddDays(8), IsActive = true, AutoRenew = true, CategoryId = 2, WebsiteUrl = "https://www.adobe.com", AccountEmail = "user@example.com", Notes = "20+ creative apps", CreatedAt = now.AddYears(-1).AddMonths(5), UpdatedAt = now.AddMonths(-1) },
                new() { Name = "AWS Cloud Services", Description = "Cloud hosting services", Price = 29.99m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddYears(-1).AddMonths(8), NextRenewalDate = now.AddDays(3), IsActive = true, AutoRenew = true, CategoryId = 3, WebsiteUrl = "https://aws.amazon.com", AccountEmail = "admin@example.com", Notes = "EC2 + S3 usage", CreatedAt = now.AddYears(-1).AddMonths(8), UpdatedAt = now.AddDays(-5) },
                new() { Name = "Google Workspace", Description = "Business email and productivity", Price = 72.00m, BillingCycle = BillingCycle.Annually, StartDate = now.AddYears(-1).AddMonths(2), NextRenewalDate = now.AddDays(45), IsActive = true, AutoRenew = true, CategoryId = 3, WebsiteUrl = "https://workspace.google.com", AccountEmail = "admin@example.com", Notes = "3 users at $6/month each annual plan", CreatedAt = now.AddYears(-1).AddMonths(2), UpdatedAt = now.AddMonths(-3) },
                new() { Name = "Peloton Digital", Description = "Fitness classes app", Price = 12.99m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddMonths(9), NextRenewalDate = now.AddDays(15), IsActive = true, AutoRenew = false, CategoryId = 4, WebsiteUrl = "https://www.peloton.com", AccountEmail = "user@example.com", Notes = "Considering cancellation", CreatedAt = now.AddMonths(9), UpdatedAt = now.AddDays(-10) },
                new() { Name = "Skillshare Premium", Description = "Online learning platform", Price = 168.00m, BillingCycle = BillingCycle.Annually, StartDate = now.AddMonths(7).AddDays(15), NextRenewalDate = now.AddDays(60), IsActive = true, AutoRenew = true, CategoryId = 5, WebsiteUrl = "https://www.skillshare.com", AccountEmail = "user@example.com", Notes = "Unlimited classes", CreatedAt = now.AddMonths(7), UpdatedAt = null },
                new() { Name = "HelloFresh", Description = "Meal kit delivery", Price = 59.99m, BillingCycle = BillingCycle.Weekly, StartDate = now.AddMonths(4), NextRenewalDate = now.AddDays(2), IsActive = true, AutoRenew = true, CategoryId = 6, WebsiteUrl = "https://www.hellofresh.com", AccountEmail = "user@example.com", Notes = "3 meals for 2 people", CreatedAt = now.AddMonths(4), UpdatedAt = now.AddDays(-2) },
                new() { Name = "The New York Times", Description = "Digital news subscription", Price = 17.00m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddYears(-1).AddMonths(1), NextRenewalDate = now.AddDays(22), IsActive = true, AutoRenew = true, CategoryId = 7, WebsiteUrl = "https://www.nytimes.com", AccountEmail = "user@example.com", Notes = "All digital access", CreatedAt = now.AddYears(-1).AddMonths(1), UpdatedAt = now.AddMonths(-2) },
                new() { Name = "Visual Studio Enterprise", Description = "Professional IDE license", Price = 250.00m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddYears(-1), NextRenewalDate = now.AddDays(1), IsActive = true, AutoRenew = true, CategoryId = 8, WebsiteUrl = "https://visualstudio.microsoft.com", AccountEmail = "admin@example.com", Notes = "Work expense reimbursed", CreatedAt = now.AddYears(-1), UpdatedAt = now.AddMonths(-1) },
                new() { Name = "Disney+", Description = "Family streaming", Price = 7.99m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddMonths(10).AddDays(25), NextRenewalDate = now.AddDays(10), IsActive = true, AutoRenew = true, CategoryId = 1, WebsiteUrl = "https://www.disneyplus.com", AccountEmail = "user@example.com", Notes = "With Hulu bundle option", CreatedAt = now.AddMonths(10), UpdatedAt = null },
                new() { Name = "Dropbox Plus", Description = "Cloud storage 2TB", Price = 11.99m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddYears(-2).AddMonths(9).AddDays(10), NextRenewalDate = now.AddDays(18), IsActive = false, AutoRenew = false, CategoryId = 2, WebsiteUrl = "https://www.dropbox.com", AccountEmail = "user@example.com", Notes = "Switched to OneDrive", CreatedAt = now.AddYears(-2).AddMonths(9), UpdatedAt = now.AddMonths(-4) },
                new() { Name = "HBO Max", Description = "Premium streaming", Price = 14.99m, BillingCycle = BillingCycle.Monthly, StartDate = now.AddYears(-1).AddMonths(6), NextRenewalDate = now.AddDays(25), IsActive = true, AutoRenew = true, CategoryId = 1, WebsiteUrl = "https://www.hbomax.com", AccountEmail = "user@example.com", Notes = "Ad-free tier", CreatedAt = now.AddYears(-1).AddMonths(6), UpdatedAt = now.AddMonths(-1) }
            };
            context.Subscriptions.AddRange(subscriptions);
            context.SaveChanges();
        }
    }
}
