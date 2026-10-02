
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sublytic.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [StringLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(7)]
        [Display(Name = "Color (Hex)")]
        public string ColorHex { get; set; }

        [DefaultValue(true)]
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
