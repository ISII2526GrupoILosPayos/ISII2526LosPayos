// AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs.ReturnPurchaseOrderDTO
using System.Collections.Generic;

namespace AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs
{
    public class ReturnPurchaseOrderDTO
    {
        // === Constructor existente (NO se toca) ===
        public ReturnPurchaseOrderDTO(
            string customerName,
            string customerFirstSurname,
            string customerAddress,
            string customerTelephoneNumber,
            IList<ReturnedProductDTO> returnedProducts)
        {
            CustomerName = customerName;
            CustomerFirstSurname = customerFirstSurname;
            CustomerAddress = customerAddress;
            CustomerTelephoneNumber = customerTelephoneNumber;
            ReturnedProducts = returnedProducts;
            // Dejar null por defecto para compatibilidad
            ReturningOptionSelected = null;
            Rating = null;
        }

        // === NUEVO constructor con los 2 campos extra ===
        public ReturnPurchaseOrderDTO(
            string customerName,
            string customerFirstSurname,
            string customerAddress,
            string customerTelephoneNumber,
            IList<ReturnedProductDTO> returnedProducts,
            string? returningOptionSelected,
            int? rating)
            : this(customerName, customerFirstSurname, customerAddress, customerTelephoneNumber, returnedProducts)
        {
            ReturningOptionSelected = returningOptionSelected;
            Rating = rating;
        }

        // Datos del cliente
        public string CustomerName { get; set; }
        public string CustomerFirstSurname { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTelephoneNumber { get; set; }

        // Productos devueltos
        public IList<ReturnedProductDTO> ReturnedProducts { get; set; }

        // === NUEVAS propiedades opcionales (no rompen nada) ===
        public string? ReturningOptionSelected { get; set; } // "Bizum" / "PayPal" / "CreditCard"...
        public int? Rating { get; set; }
    }
}
