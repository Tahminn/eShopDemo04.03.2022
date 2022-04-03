using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShopDemo.Models
{
    public class Slider : BaseEntity
    {
        public string Image { get; set; }
        [NotMapped, Required]
        public IFormFile Photo { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
    }
}
