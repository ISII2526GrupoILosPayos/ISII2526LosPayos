namespace AppForSEII2526.API.Models
{
    public class PayPal : PaymentMethod
    {
        public PayPal() { }

        public PayPal(string email)
        {
            Email = email;
        }

        [Required]
        [Key]
        public string Email { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PayPal other)
                return Email == other.Email;
            return false;
        }
    }
}