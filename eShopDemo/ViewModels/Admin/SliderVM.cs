using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eShopDemo.ViewModels.Admin
{
    public class SliderVM
    {
        public int Id { get; set; }
        [Required]
        public List<IFormFile> Photos { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }

    }
}
