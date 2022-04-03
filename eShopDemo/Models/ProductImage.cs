using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShopDemo.Models
{
    public class ProductImage : BaseEntity
    {
        public string Image { get; set; }
        public bool IsMain { get; set; } = false;
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [NotMapped, Required]
        public IFormFile Photo { get; set; }
    }
}
