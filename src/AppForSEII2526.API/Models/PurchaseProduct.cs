namespace AppForSEII2526.API.Models
{
    public class PurchaseProduct
    {
        public int ProductID { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }

        public int PurchaseProductId { get; set; }

        public int Quantity { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is PurchaseProduct other)
                return ProductID == other.ProductID;
            return false;
        }

        public override int GetHashCode()
        {
            return ProductID.GetHashCode();
        }
    }
}
