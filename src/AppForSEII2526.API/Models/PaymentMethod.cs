namespace AppForSEII2526.API.Models
{
    public abstract class PaymentMethod
    {
        public PaymentMethod() { }

        public PaymentMethod(int id)
        {
            Id = id;
        }

        [Required]
        [Key]
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PaymentMethod other)
                return Id == other.Id;
            return false;
        }
    }
}