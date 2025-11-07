namespace AppForSEII2526.API.Models
{
    public class PurchaseOrder
    {
        public PurchaseOrder() { }
        public PurchaseOrder(string nameSurname, ApplicationUser applicationUser, string street, string city, string postalCode, DateTime date, PaymentMethod paymentMethod, IList<PurchaseProduct> products)
        {
            NameSurname = nameSurname;
            ApplicationUser = applicationUser;
            Street = street;
            City = city;
            PostalCode = postalCode;
            Date = date;
            PaymentMethod = paymentMethod;
            Products = products;
        }
        public PurchaseOrder(string city, decimal totalPrice, DateTime date, string? description, string nameSurname, string postalCode, string street)
        {
            City = city;
            TotalPrice = totalPrice;
            Date = date;
            Description = description;
            NameSurname = nameSurname;
            PostalCode = postalCode;
            Street = street;
        }

        //public PurchaseOrder(string city, decimal totalPrice, DateTime date, string? description, string nameSurname, string postalCode, string street, int? rating, PurchaseState state, string applicationUserId)
        //    : this(city, totalPrice, date, description, nameSurname, postalCode, street)
        //{
        //    Rating = rating;
        //    State = state;
        //    ApplicationUserId = applicationUserId;
        //}

        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Name of City can be neither longer than 20 characters nor shorter than 1", MinimumLength =1)]
        public string City { get; set; }


        [Display(Name = "Total price")]
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }


        [StringLength(100, ErrorMessage = "Description can be neither longer than 100 characters")]
        public string? Description { get; set; }

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

        public IList<PurchaseProduct> Products { get; set; }


        //  Clave foránea hacia ApplicationUser
        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PurchaseState State { get; set; }

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
