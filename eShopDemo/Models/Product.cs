using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShopDemo.Models
{
    public class Product : BaseEntity
    {
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Display(Name = "Discount Price")]
        public decimal ActualPrice { get; set; }
        [MinLength(0)]
        [Display(Name = "Discount Price")]
        public decimal DiscountPrice { get; set; }
        [MinLength(0)]
        [Display(Name = "Sold Units")]
        public int UnitsSold { get; set; } = 0;
        [MinLength(0)]
        [Display(Name = "Quantity in stock")]
        public int UnitsQuantity { get; set; }
        [Range(0, 5)]
        public int Rating { get; set; } = 0;
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }

    }
}
