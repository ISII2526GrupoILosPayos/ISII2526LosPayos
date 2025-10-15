namespace AppForSEII2526.API.DTOs.ProductDTOs
{
    public class ProductForPurchaseDTO
    {
        public ProductForPurchaseDTO(int id, string name, string brandName)
        {
            Id = id;
            Name = name;
            Brand = brandName;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }
    }
}
