namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseProductForCreateDTO
    {
        public PurchaseProductForCreateDTO()
        {
        }

        public PurchaseProductForCreateDTO(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public PurchaseProductForCreateDTO(int productId, string name, string brand, int quantity, decimal price)
        {
            ProductId = productId;
            Name = name;
            Brand = brand;
            Quantity = quantity;
            Price = price;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }

        public int ProductId { get; set; }

        public string Brand { get; set; }
        public string Location { get; set; }

        public string Colour { get; set; }
    }
}
