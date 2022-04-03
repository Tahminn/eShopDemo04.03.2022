using eShopDemo.Models;
using eShopDemo.Utilities.Pagination;
using eShopDemo.ViewModels.ProductVMs;
using System.Collections.Generic;

namespace eShopDemo.ViewModels
{
    public class ShopVM
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public Pagination<ProductListVM> PaginatedProduct { get; set; }
    }
}
