namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseProductDTO
    {
        public PurchaseProductDTO(int productId, string name, string brand, int quantity, decimal price)
        {
            ProductId = productId;
            Name = name;
            Brand = brand;
            Quantity = quantity;
            Price = price;
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseProductDTO dto &&
                   ProductId == dto.ProductId &&
                   Name == dto.Name &&
                   Brand == dto.Brand &&
                   Quantity == dto.Quantity &&
                   Price == dto.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductId, Name, Brand, Quantity, Price);
        }
    }
}
