using System.ComponentModel.DataAnnotations.Schema; // Asegúrate de tener este using

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ProductId), nameof(PurchaseOrderId))]
    public class ReturnProduct
    {
        public ReturnProduct() { }

        public ReturnProduct(int quantity, string reason)
        {
            Quantity = quantity;
            Reason = reason;
        }

        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public string Reason { get; set; } = string.Empty;

        // Dependencia hacia PurchaseProduct
        public int ProductId { get; set; }
        public int PurchaseOrderId { get; set; }

        [ForeignKey(nameof(ProductId) + "," + nameof(PurchaseOrderId))]
        public PurchaseProduct PurchaseProduct { get; set; } = null!;

        public override bool Equals(object obj)
        {
            if (obj is ReturnProduct other)
                return Id == other.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
