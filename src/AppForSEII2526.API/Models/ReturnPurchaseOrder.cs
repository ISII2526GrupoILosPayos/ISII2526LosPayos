namespace AppForSEII2526.API.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class ReturnPurchaseOrder
    {
        public ReturnPurchaseOrder() { }

        public ReturnPurchaseOrder(string name, decimal totalPrice, decimal newTotalPrice, DateTime date, int? rating = null)
        {
            Name = name;
            TotalPrice = decimal.Round(totalPrice, 2);
            NewTotalPrice = decimal.Round(newTotalPrice, 2);
            MoneyToReturn = decimal.Round(TotalPrice - NewTotalPrice, 2);
            Date = date;
            Rating = rating;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Maximun 20, minimun 10",MinimumLength =10)]
        public string Name { get; set; }

        [Precision(10, 2)]
        [Range(0, 99999999.99)]
        public decimal TotalPrice { get; set; }

        [Precision(10, 2)]
        [Range(0, 99999999.99)]
        public decimal NewTotalPrice { get; set; }

        [Precision(10, 2)]
        [Range(0, 99999999.99)]
        public decimal MoneyToReturn { get; set; }


        
        public DateTime Date { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public PaymentMethod PaymentMethod { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is ReturnPurchaseOrder other)
                return Id == other.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public ApplicationUser Customer { get; set; }
        public string CustomerId { get; set; }          // FK explícita

        //public ReturnProduct ReturnProduct { get; set; }

        public IList<ReturnProduct> ReturnProducts { get; set; }
    }
}
