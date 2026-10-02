using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sublytic.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }

        // The user who owns this subscription
        [Required]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Subscription name is required")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Service Name")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 99999.99)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Billing Cycle")]
        public BillingCycle BillingCycle { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(
            DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(
            DataFormatString = "{0:yyyy-MM-dd}",
            ApplyFormatInEditMode = true)]
        [Display(Name = "Next Renewal Date")]
        public DateTime NextRenewalDate { get; set; }

        [Display(Name = "Active")]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Display(Name = "Auto Renew")]
        [DefaultValue(true)]
        public bool AutoRenew { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [StringLength(255)]
        [DataType(DataType.Url)]
        [Display(Name = "Website URL")]
        public string WebsiteUrl { get; set; }

        [StringLength(50)]
        [Display(Name = "Account Email")]
        [DataType(DataType.EmailAddress)]
        public string AccountEmail { get; set; }

        [StringLength(500)]
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }

    public enum BillingCycle
    {
        [Display(Name = "Weekly")]
        Weekly = 0,

        [Display(Name = "Monthly")]
        Monthly = 1,

        [Display(Name = "Quarterly")]
        Quarterly = 3,

        [Display(Name = "Semi-Annually")]
        SemiAnnually = 6,

        [Display(Name = "Annually")]
        Annually = 12
    }
}
