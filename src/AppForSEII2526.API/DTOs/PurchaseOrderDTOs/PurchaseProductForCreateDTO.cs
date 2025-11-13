namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseProductForCreateDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
