namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod
    {
        public Bizum() { }

        public Bizum(long telephoneNumber)
        {
            TelephoneNumber = telephoneNumber;
        }

        [Required]
        [Range(100000000, 999999999, ErrorMessage = "Phone number must have exactly 9 digits")]
        public long TelephoneNumber { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Bizum other)
                return TelephoneNumber == other.TelephoneNumber;
            return false;
        }
    }
}