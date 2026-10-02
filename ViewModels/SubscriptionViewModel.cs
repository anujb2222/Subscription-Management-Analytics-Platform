
using System;
using System.ComponentModel.DataAnnotations;

namespace Sublytic.ViewModels
{
    public class SubscriptionViewModel
    {
        public int SubscriptionId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        [Display(Name = "Service Name")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 99999.99)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Billing Cycle")]
        public int BillingCycle { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Next Renewal Date")]
        public DateTime NextRenewalDate { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Auto Renew")]
        public bool AutoRenew { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [StringLength(255)]
        [DataType(DataType.Url)]
        [Display(Name = "Website")]
        public string WebsiteUrl { get; set; }

        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string AccountEmail { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
