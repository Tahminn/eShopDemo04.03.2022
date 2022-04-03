using eShopDemo.Data;
using eShopDemo.Models;
using eShopDemo.Services;
using eShopDemo.Utilities.Pagination;
using eShopDemo.ViewModels;
using eShopDemo.ViewModels.ProductVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace eShopDemo.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ShopController(ILogger<HomeController> logger, AppDbContext context, ProductService productService, CategoryService categoryService)
        {
            _context = context;
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<ActionResult> Index(string category, string price, string sortOrder, int after, int size, int page = 1)
        {
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Date" : "Date-Desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price-Desc" : "Price";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name-Desc" : "Name";

            ViewData["ProductCount"] = await _context.Products.AsNoTracking().CountAsync() + 1;
            if (after == 0) after = (int)ViewData["ProductCount"];
            if (size == 0) size = 9;

            var products = await _context.Products
                .Where(x => x.Id < after)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .ToListAsync();
            if(category != null) products = products
                .Where(p => p.Category.Name == category)
                .ToList();
            string priceRange = price;
            //if (price != 0) products = products
            //     .Where(p => p.DiscountPrice = price)
            //     .ToList();
            //if (belowPrice != 0 && abovePrice == 0) products = products
            //     .Where(p => p.DiscountPrice <= belowPrice && p.DiscountPrice >= abovePrice)
            //     .ToList();
            int count = GetPageCount(products, size);
            switch (sortOrder)
            {
                case "Name":
                    products = products.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Name-Desc":
                    products = products.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Price":
                    products = products.OrderBy(s => s.DiscountPrice).ToList();
                    break;
                case "Price-Desc":
                    products = products.OrderByDescending(s => s.DiscountPrice).ToList();
                    break;
                case "Date":
                    products = products.OrderBy(s => s.Id).ToList();
                    break;
                default:
                    products = products.OrderByDescending(s => s.Id).ToList();
                    break;
            }
            products = products
                .Take(size)
                .ToList();

            //ViewData["SortOrder"] = String.IsNullOrEmpty(sortOrder) ? "Date-Desc" : sortOrder;
            ViewData["SortOrder"] = sortOrder;
            ViewData["Size"] = size;
            ViewData["Category"] = category;


            var productsVM = GetMapDatas(products);
            Pagination<ProductListVM> paginatedProduct = new Pagination<ProductListVM>(productsVM, page, count);
            List<Category> categories = await _categoryService.GetCategories();
            ShopVM shopVM = new ShopVM
            {
                PaginatedProduct = paginatedProduct,
                Categories = categories,
            };
            return View(shopVM);
        }

        private int GetPageCount(List<Product> products, int size)
        {
            var productCount = products.Count();
            return (int)Math.Ceiling((decimal)productCount / size);
        }
        private List<ProductListVM> GetMapDatas(List<Product> products)
        {
            List<ProductListVM> productLists = new List<ProductListVM>();
            foreach (Product product in products)
            {
                ProductListVM newProduct = new ProductListVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    ActualPrice = product.ActualPrice,
                    DiscountPrice = product.DiscountPrice,
                    UnitsSold = product.UnitsSold,
                    UnitsQuantity = product.UnitsQuantity,
                    Rating = product.Rating,
                    CategoryName = product.Category.Name,
                    Images = product.ProductImages
                        .Where(p => p.IsMain)
                        .FirstOrDefault()?
                        .Image
                };
                productLists.Add(newProduct);
            }
            return productLists;
        }
    }
}
