using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ProductId), nameof(PurchaseOrderId))]
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
        public decimal Price { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        // 0..1: puede o no tener devolución asociada
        public ReturnProduct? ReturnProduct { get; set; }

        public override bool Equals(object? obj) => obj is PurchaseProduct other && ProductId == other.ProductId && PurchaseOrderId == other.PurchaseOrderId;
        public override int GetHashCode() => HashCode.Combine(ProductId, PurchaseOrderId);
    }
}
