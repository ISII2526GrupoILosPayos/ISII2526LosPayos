namespace AppForSEII2526.API.Models
{
    public class CreditCard : PaymentMethod
    {
        public CreditCard() { }

        public CreditCard(string creditCardNumber, DateTime expirationDate)
        {
            CreditCardNumber = creditCardNumber;
            ExpirationDate = expirationDate;
        }

        [Required]
        [Key]
        public string CreditCardNumber { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CreditCard other)
                return CreditCardNumber == other.CreditCardNumber;
            return false;
        }
    }
}