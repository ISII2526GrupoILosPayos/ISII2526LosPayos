using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ReturnPurchaseOrderStateContainer
    {
        // Instancia del objeto que se enviará en el POST
        public ReturnPurchaseOrderForCreateDTO ReturnOrder { get; private set; } =
            new ReturnPurchaseOrderForCreateDTO(
                customerUserName: "",
                returningOptionSelected: "",
                rating: null,
                items: new List<ReturnItemForCreateDTO>()
            );

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        // Añadir un producto que el usuario quiere devolver
        public void AddProductForReturn(PurchaseProductForReturnDTO product, int quantity, string reason)
        {
            if (!ReturnOrder.Items.Any(i =>
                    i.ProductId == product.Id &&
                    i.PurchaseOrderId == product.PurchaseOrderId))
            {
                ReturnOrder.Items.Add(new ReturnItemForCreateDTO
                {
                    ProductId = product.Id,
                    PurchaseOrderId = product.PurchaseOrderId,
                    Quantity = quantity,
                    Reason = reason
                });

                NotifyStateChanged();
            }
        }

        // Eliminar un producto de la devolución
        public void RemoveItem(ReturnItemForCreateDTO item)
        {
            ReturnOrder.Items.Remove(item);
            NotifyStateChanged();
        }

        // Cambiar el método de devolución del dinero
        public void SetReturningOption(string option)
        {
            ReturnOrder.ReturningOptionSelected = option;
            NotifyStateChanged();
        }

        // Cambiar el rating
        public void SetRating(int? rating)
        {
            ReturnOrder.Rating = rating;
            NotifyStateChanged();
        }

        // Establecer el usuario que realiza la devolución
        public void SetCustomerUserName(string username)
        {
            ReturnOrder.CustomerUserName = username;
            NotifyStateChanged();
        }

        // Reiniciar la devolución después del POST
        public void ResetAfterProcessing()
        {
            ReturnOrder = new ReturnPurchaseOrderForCreateDTO(
                customerUserName: "",
                returningOptionSelected: "",
                rating: null,
                items: new List<ReturnItemForCreateDTO>()
            );

            NotifyStateChanged();
        }
    }
}
