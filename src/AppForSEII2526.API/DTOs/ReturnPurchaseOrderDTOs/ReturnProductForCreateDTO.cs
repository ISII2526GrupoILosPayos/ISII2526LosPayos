namespace AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs
{
    namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
    {
        public class ReturnProductForCreateDTO
        {
            public ReturnProductForCreateDTO(
                int productId,
                int purchaseOrderId,
                int quantityToReturn
            )
            {
                ProductId = productId;
                PurchaseOrderId = purchaseOrderId;
                QuantityToReturn = quantityToReturn;
            }

            // producto concreto que se devuelve
            [Required]
            public int ProductId { get; set; }

            // de qué pedido original viene ese producto
            [Required]
            public int PurchaseOrderId { get; set; }

            // cuántas unidades devuelve el cliente
            [Required]
            [Range(1, int.MaxValue)]
            public int QuantityToReturn { get; set; }
        }
    }

}
