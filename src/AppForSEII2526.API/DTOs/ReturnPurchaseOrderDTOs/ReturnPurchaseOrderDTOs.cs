using System;
using System.Collections.Generic;

namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
{
    public class ReturnPurchaseOrderDTO
    {
        public ReturnPurchaseOrderDTO()
        {
        }

        public ReturnPurchaseOrderDTO(DateTime returnDate, string paymentMethod, decimal moneyToReturn, int? rating, string name, string firstSurname, string address, string phone, IList<ReturnedProductDTO> returnedProducts)
        {
            ReturnDate = returnDate;
            PaymentMethod = paymentMethod;
            MoneyToReturn = moneyToReturn;
            Rating = rating;
            Name = name;
            FirstSurname = firstSurname;
            Address = address;
            Phone = phone;
            ReturnedProducts = returnedProducts;
        }

        public DateTime ReturnDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal MoneyToReturn { get; set; }
        public int? Rating { get; set; }



        public string Name { get; set; }
        public string FirstSurname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }


        public IList<ReturnedProductDTO> ReturnedProducts { get; set; }
    }
}
