using eShopDemo.Models;
using System.Collections.Generic;

namespace eShopDemo.ViewModels
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Slider> Sliders { get; set; }
    }
}
