using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ProductId), nameof(PurchaseOrderId))]
    public class PurchaseProduct
    {
        public PurchaseProduct() { }

        public PurchaseProduct(Product product, int productId, PurchaseOrder purchaseOrder, int quantity, decimal price)
        {
            Product = product;
            ProductId = productId;
            PurchaseOrder = purchaseOrder;
            Quantity = quantity;
            Price = price;
        }

        public PurchaseProduct(int productId, int purchaseOrderId, decimal price, int quantity)
        {
            ProductId = productId;
            PurchaseOrderId = purchaseOrderId;
            Price = price;
            Quantity = quantity;
        }

        public PurchaseProduct(int productId, PurchaseOrder purchaseOrder, decimal price, int quantity)
        {
            ProductId = productId;
            PurchaseOrder = purchaseOrder;
            Price = price;
            Quantity = quantity;
        }

        [Required]
        public int ProductId { get; set; }
       
        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        // 0..1: puede o no tener devolución asociada
        public ReturnProduct? ReturnProduct { get; set; }

        [ForeignKey("ProductId")]

        public Product Product { get; set; }
        
        [ForeignKey("PurchaseOrderId")]

        public PurchaseOrder PurchaseOrder { get; set; }

        [Required]
        [Precision(10, 2)]
        [Range(0, 9999999.99)]
        public decimal Price { get; set; }
        
     
        public override bool Equals(object? obj)
        {
            if (obj is PurchaseProduct other)
                return ProductId == other.ProductId;
            return false;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }
    }
}
