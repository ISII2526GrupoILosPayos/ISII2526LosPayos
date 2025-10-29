using System.Collections.Generic;

namespace AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs
{
    // DTO principal devuelto por el caso de uso:
    // "The system shows the returning process performed..."
    public class ReturnPurchaseOrderDTO
    {
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
        }

        // Datos del cliente
        public string CustomerName { get; set; }              // Name
        public string CustomerFirstSurname { get; set; }      // First Surname
        // Second Surname no se incluye porque no está en el modelo actual
        public string CustomerAddress { get; set; }           // Address
        public string CustomerTelephoneNumber { get; set; }   // Telephone number

        // Información de los productos devueltos
        public IList<ReturnedProductDTO> ReturnedProducts { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReturnPurchaseOrderDTO dTO &&
                   CustomerName == dTO.CustomerName &&
                   CustomerFirstSurname == dTO.CustomerFirstSurname &&
                   CustomerAddress == dTO.CustomerAddress &&
                   CustomerTelephoneNumber == dTO.CustomerTelephoneNumber &&
                   EqualityComparer<IList<ReturnedProductDTO>>.Default.Equals(ReturnedProducts, dTO.ReturnedProducts);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomerName, CustomerFirstSurname, CustomerAddress, CustomerTelephoneNumber, ReturnedProducts);
        }
    }
}
