using System;
using System.Collections.Generic;

namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
{
    public class ReturnPurchaseOrderDTO
    {
        public ReturnPurchaseOrderDTO(DateTime date, PaymentMethod paymentMethod, decimal moneyToReturn, int? rating, string name, string surname, string address, string? phoneNumber)
        {
            Date = date;
            PaymentMethod1 = paymentMethod;
            MoneyToReturn = moneyToReturn;
            Rating = rating;
            Name = name;
            Surname = surname;
            Address = address;
            PhoneNumber = phoneNumber;
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
        public DateTime Date { get; }
        public PaymentMethod PaymentMethod1 { get; }
        public string Surname { get; }
        public string? PhoneNumber { get; }
    }
}
