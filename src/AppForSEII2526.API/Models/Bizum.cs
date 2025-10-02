namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod
    {
        public Bizum() { }

        public Bizum(int telephoneNumber)
        {
            TelephoneNumber = telephoneNumber;
        }

        
        public int TelephoneNumber { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Bizum other)
                return TelephoneNumber == other.TelephoneNumber;
            return false;
        }
    }
}