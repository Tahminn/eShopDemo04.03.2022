using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace eShopDemo.ViewModels.ProductVMs
{
    public class ProductCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int UnitsSold { get; set; } = 0;
        public int UnitsQuantity { get; set; }
        public int Rating { get; set; } = 0;
        public int CategoryId { get; set; }
        public List<IFormFile> Photos { get; set; }

    }
}
