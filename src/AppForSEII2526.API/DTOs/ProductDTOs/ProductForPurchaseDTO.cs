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
        
        public decimal Price { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ProductForPurchaseDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Location == dTO.Location &&
                   Stock == dTO.Stock;
        }
    }
}
