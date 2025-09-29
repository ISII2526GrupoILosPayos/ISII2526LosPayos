namespace AppForSEII2526.API.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Name of City can be neither longer than 20 characters nor shorter than 1", MinimumLength =1)]
        public string City { get; set; }


        [Display(Name = "Total price")]
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Description can be neither longer than 100 characters nor shorter than 1", MinimumLength = 1)]
        public string Description { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Name and surname can be neither longer than 30 characters nor shorter than 1", MinimumLength = 1)]
        public string NameSurname { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Postal code can be neither longer than 20 characters nor shorter than 1", MinimumLength = 1)]
        public string PostalCode { get; set; }

        public int? Rating { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Name of street can be neither longer than 30 characters nor shorter than 1", MinimumLength = 1)]
        public string Street { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj is PurchaseOrder other)
                return Id == other.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
