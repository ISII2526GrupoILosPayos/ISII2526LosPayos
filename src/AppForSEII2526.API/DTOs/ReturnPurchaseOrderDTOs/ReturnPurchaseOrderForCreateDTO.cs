namespace AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
    {
        public class ReturnPurchaseOrderForCreateDTO
        {
            public ReturnPurchaseOrderForCreateDTO(
                string customerUserName,
                string customerName,
                string customerSurname,
                string customerAddress,
                string customerTelephoneNumber,
                string paymentMethod,   // "credit card", "paypal", "other"
                string reason,          // motivo devolución
                int? rating,            // 1..5 opcional
                IList<ReturnProductForCreateDTO> productsToReturn
            )
            {
                CustomerUserName = customerUserName;
                CustomerName = customerName;
                CustomerSurname = customerSurname;
                CustomerAddress = customerAddress;
                CustomerTelephoneNumber = customerTelephoneNumber;
                PaymentMethod = paymentMethod;
                Reason = reason;
                Rating = rating;
                ProductsToReturn = productsToReturn;
            }

            [Required]
            public string CustomerUserName { get; set; } // quién está devolviendo (similar a CustomerUserName en rental)

            [Required]
            public string CustomerName { get; set; }     // Name

            [Required]
            public string CustomerSurname { get; set; }  // Surname (solo uno, como decidimos)

            [Required]
            public string CustomerAddress { get; set; }  // Address

            [Required]
            public string CustomerTelephoneNumber { get; set; } // Telephone number

            [Required]
            public string PaymentMethod { get; set; }    // credit card / paypal / other

            [Required]
            public string Reason { get; set; }           // motivo obligatorio del paso 5

            [Range(1, 5)]
            public int? Rating { get; set; }             // opcional

            [Required]
            public IList<ReturnProductForCreateDTO> ProductsToReturn { get; set; }
        }
    }

}
