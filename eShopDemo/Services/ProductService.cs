using eShopDemo.Data;
using eShopDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopDemo.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProductsWithImages(int takeProducts, int skipProduct)
        {
            try
            {
                if (takeProducts == -1)
                {
                    List<Product> products = await _context.Products
                        .Where(p => p.IsDeleted == false)
                        .Include(p => p.Category)
                        .Include(p => p.ProductImages)
                        .Skip(skipProduct)
                        .OrderByDescending(p => p.Id)
                        .ToListAsync();
                    return products;
                }
                else
                {
                    List<Product> products = await _context.Products
                        .Where(p => p.IsDeleted == false)
                        .Include(p => p.Category)
                        .Include(p => p.ProductImages)
                        .OrderByDescending(p => p.Id)
                        .Skip(skipProduct)
                        .Take(takeProducts)
                        .ToListAsync();
                    return products;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
