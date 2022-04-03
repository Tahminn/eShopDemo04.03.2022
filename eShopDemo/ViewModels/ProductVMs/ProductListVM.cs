namespace eShopDemo.ViewModels.ProductVMs
{
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int UnitsSold { get; set; } = 0;
        public int UnitsQuantity { get; set; }
        public int Rating { get; set; } = 0;
        public string CategoryName { get; set; }
        public string Images { get; set; }
    }
}
