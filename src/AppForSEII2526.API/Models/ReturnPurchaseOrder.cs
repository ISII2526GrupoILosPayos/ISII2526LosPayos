namespace AppForSEII2526.API.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class ReturnPurchaseOrder
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "Maximun 20, minimun 10",MinimumLength =10)]
        public string Name { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        [Precision(10, 2)]
        public decimal NewTotalPrice { get; set; }

        [Precision(10, 2)]
        public double MoneyToReturn { get; set; }



    }
}
