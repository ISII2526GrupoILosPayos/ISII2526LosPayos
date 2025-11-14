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

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
