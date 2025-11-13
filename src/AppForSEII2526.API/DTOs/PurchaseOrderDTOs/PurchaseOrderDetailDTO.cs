using AppForSEII2526.API.Models;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.PurchaseOrderDTOs
{
    public class PurchaseOrderDetailDTO
    {
        public PurchaseOrderDetailDTO(int id,DateTime date,string customerName,string customerSurname,string street,string city,string postalCode,PurchaseState state,decimal totalPrice, string paymentMethodName, IList<PurchaseProductDTO> purchaseProducts)
        {
            Id = id;
            Date = date;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            Street = street;
            City = city;
            PostalCode = postalCode;
            State = state;
            TotalPrice = totalPrice;
            PaymentMethodName = paymentMethodName;
            PurchaseProducts = purchaseProducts;
        }
        
        public PurchaseOrderDetailDTO(
            int id,
            DateTime date,
            string customerName,
            string customerSurname,
            string street,
            string city,
            string postalCode,
            IList<PurchaseProductDTO> purchaseProducts)
        {
            Id = id;
            Date = date;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            Street = street;
            City = city;
            PostalCode = postalCode;
            PurchaseProducts = purchaseProducts;
        }

        public PurchaseOrderDetailDTO(int id, DateTime date, string customerName, string customerSurname, string street, string city, string postalCode, decimal totalPrice, string paymentMethodName, IList<PurchaseProductDTO> purchaseProducts)
        {
            Id = id;
            Date = date;
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            Street = street;
            City = city;
            PostalCode = postalCode;
            TotalPrice = totalPrice;
            PaymentMethodName = paymentMethodName;
            PurchaseProducts = purchaseProducts;
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public PurchaseState State { get; set; }

        public decimal TotalPrice { get; set; }

        public IList<PurchaseProductDTO> PurchaseProducts { get; set; }
        //public string NameSurname { get; }
        public string PaymentMethodName { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is PurchaseOrderDetailDTO dto &&
                   Id == dto.Id &&
                   Date == dto.Date &&
                   CustomerName == dto.CustomerName &&
                   CustomerSurname == dto.CustomerSurname &&
                   Street == dto.Street &&
                   City == dto.City &&
                   PostalCode == dto.PostalCode &&
                   State == dto.State &&
                   TotalPrice == dto.TotalPrice &&
                   PurchaseProducts.SequenceEqual(dto.PurchaseProducts);
        }

        
    }
}