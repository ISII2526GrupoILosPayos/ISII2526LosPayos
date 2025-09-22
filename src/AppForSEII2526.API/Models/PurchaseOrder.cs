namespace AppForSEII2526.API.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "Name of City can be neither longer than 20 characters nor shorter than 1", MinimumLength =1)]
        public string City { get; set; }


        [Display(Name = "Total price")]
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
       
    }
}
