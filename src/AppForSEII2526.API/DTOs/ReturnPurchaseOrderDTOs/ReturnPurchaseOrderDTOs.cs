using System.Collections.Generic;

namespace AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs
{
    public class ReturnPurchaseOrderDTO
    {
        public ReturnPurchaseOrderDTO(
            string customerName,
            string customerFirstSurname,
            string customerAddress,
            string customerTelephoneNumber,
            IList<ReturnedProductDTO> returnedProducts,
            string returningOptionSelected)
        {
            CustomerName = customerName;
            CustomerFirstSurname = customerFirstSurname;
            CustomerAddress = customerAddress;
            CustomerTelephoneNumber = customerTelephoneNumber;
            ReturnedProducts = returnedProducts;
            ReturningOptionSelected = returningOptionSelected;
        }

        // Datos del cliente
        public string CustomerName { get; set; }
        public string CustomerFirstSurname { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTelephoneNumber { get; set; }

        // Productos devueltos
        public IList<ReturnedProductDTO> ReturnedProducts { get; set; }

        // Opción de devolución seleccionada
        public string ReturningOptionSelected { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReturnPurchaseOrderDTO dTO &&
                   CustomerName == dTO.CustomerName &&
                   CustomerFirstSurname == dTO.CustomerFirstSurname &&
                   CustomerAddress == dTO.CustomerAddress &&
                   CustomerTelephoneNumber == dTO.CustomerTelephoneNumber &&
                   EqualityComparer<IList<ReturnedProductDTO>>.Default.Equals(ReturnedProducts, dTO.ReturnedProducts) &&
                   ReturningOptionSelected == dTO.ReturningOptionSelected;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomerName, CustomerFirstSurname, CustomerAddress, CustomerTelephoneNumber, ReturnedProducts, ReturningOptionSelected);
        }
    }
}
