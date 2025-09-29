namespace AppForSEII2526.API.Models
{
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
