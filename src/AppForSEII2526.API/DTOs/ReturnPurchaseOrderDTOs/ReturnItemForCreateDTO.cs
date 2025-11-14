using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs
{
    public class ReturnItemForCreateDTO
    {
        [Required]
        public int ProductId { get; set; }

        // El pedido original en el que se compró ese producto
        [Required]
        public int PurchaseOrderId { get; set; }

        // Cuántas unidades devuelve este cliente de ese producto
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        // Motivo concreto de la devolución para ESTE producto
        // (esto cubre "the reason why he is returning those products")
        [Required]
        public string Reason { get; set; }
    }
}
