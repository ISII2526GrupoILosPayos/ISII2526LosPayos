

namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseOrderForCreateDTO
    {
        public PurchaseOrderForCreateDTO(
            string customerUserName,
            string nameSurname,
            string street,
            string city,
            string postalCode,
            int paymentMethodId,
            IList<PurchaseProductForCreateDTO> purchaseProducts)
        {
            CustomerUserName = customerUserName ?? throw new ArgumentNullException(nameof(customerUserName));
            NameSurname = nameSurname ?? throw new ArgumentNullException(nameof(nameSurname));
            Street = street ?? throw new ArgumentNullException(nameof(street));
            City = city ?? throw new ArgumentNullException(nameof(city));
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
            PaymentMethodId = paymentMethodId;
            PurchaseProducts = purchaseProducts ?? throw new ArgumentNullException(nameof(purchaseProducts));
        }

        public PurchaseOrderForCreateDTO()
        {
            PurchaseProducts = new List<PurchaseProductForCreateDTO>();
        }

        
        [Required]
        public string CustomerUserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name and Surname")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Name and Surname must have at least 5 characters")]
        public string NameSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your street address")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Street must have at least 5 characters")]
        public string Street { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your city")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City must have at least 2 characters")]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your postal code")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Postal code must have between 3 and 10 characters")]
        public string PostalCode { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [Required]
        public IList<PurchaseProductForCreateDTO> PurchaseProducts { get; set; }

        [Range(0, 5)]
        public int? Rating { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseOrderForCreateDTO dto &&
                   CustomerUserName == dto.CustomerUserName &&
                   NameSurname == dto.NameSurname &&
                   Street == dto.Street &&
                   City == dto.City &&
                   PostalCode == dto.PostalCode &&
                   PaymentMethodId == dto.PaymentMethodId &&
                   PurchaseProducts.SequenceEqual(dto.PurchaseProducts) &&
                   Rating == dto.Rating;
        }
    }
}
