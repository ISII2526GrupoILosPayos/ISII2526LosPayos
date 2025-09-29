using Microsoft.CodeAnalysis;

namespace AppForSEII2526.API.Models
{
    public class PurchaseProduct
    {
        public PurchaseProduct() { }

        public PurchaseProduct(int productId, int purchaseOrderId, decimal price, int quantity)
        {
            ProductId = productId;
            PurchaseOrderId = purchaseOrderId;
            Price = price;
            Quantity = quantity;
        }


        [Required]
        [Precision(10, 2)]
        [Range(0, 9999999.99)]
        public decimal Price { get; set; }   // precio del producto en este pedido

        [Required]
        public int ProductId { get; set; }   // FK hacia Product

        [Required]
        public int PurchaseOrderId { get; set; }  // FK hacia PurchaseOrder

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }   // cantidad de este producto en el pedido


    }
}
