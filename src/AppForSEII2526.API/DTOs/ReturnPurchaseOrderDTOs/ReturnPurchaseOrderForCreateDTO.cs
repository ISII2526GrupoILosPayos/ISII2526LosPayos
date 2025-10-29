using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs
{
    public class ReturnPurchaseOrderForCreateDTO
    {
        public ReturnPurchaseOrderForCreateDTO(
            string customerUserName,
            string paymentMethod,
            int? rating,
            IList<ReturnItemForCreateDTO> items
        )
        {
            CustomerUserName = customerUserName;
            PaymentMethod = paymentMethod;
            Rating = rating;
            Items = items;
        }

        // Quién está haciendo la devolución.
        // Igual que en tu endpoint de productos para devolver,
        // tú filtrabas por userName => usamos lo MISMO aquí.
        [Required]
        public string CustomerUserName { get; set; }

        // Método para reembolsar dinero.
        // Paso 5 del caso de uso: "select an option for returning the money
        // (credit card, paypal or other)" => obligatorio
        [Required]
        public string PaymentMethod { get; set; } // "CreditCard", "PayPal", "Other", etc.

        // Valoración opcional del sistema de entrega (1..5)
        [Range(1, 5)]
        public int? Rating { get; set; }

        // Lista de productos que está devolviendo en esta operación.
        // Debe haber al menos 1 (si no, alt flow de error).
        [Required]
        [MinLength(1, ErrorMessage = "You must include at least one product to return")]
        public IList<ReturnItemForCreateDTO> Items { get; set; }
    }
}
