namespace AppForSEII2526.API.DTOs.ProductDTOs
{
    public class ProductForPurchaseDTO
    {
        public ProductForPurchaseDTO(int id, string name, string brandName, string location, int stock)
        {
            Id = id;
            Name = name;
            Brand = brandName;
            Location = location;
            Stock = stock;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Location { get; set; }

        public int Stock { get; set; }
    }
}
