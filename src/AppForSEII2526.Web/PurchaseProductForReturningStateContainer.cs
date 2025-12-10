using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ReturnPurchaseOrderStateContainer
    {
        // Objeto que iremos rellenando durante el proceso de devolución
        public ReturnPurchaseOrderForCreateDTO ReturnOrder { get; private set; } =
            new ReturnPurchaseOrderForCreateDTO()
            {
                CustomerUserName = "",
                ReturningOptionSelected = "",
                Rating = null,
                Items = new List<ReturnItemForCreateDTO>()
            };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        // Añadir un producto que el usuario quiere devolver
        public void AddProductForReturn(PurchaseProductForReturnDTO product, int quantity, string reason)
        {
            // ¿Ya está este producto de ese pedido en el carrito?
            var existingItem = ReturnOrder.Items
                .FirstOrDefault(i => i.ProductId == product.Id &&
                                     i.PurchaseOrderId == product.PurchaseOrderId);

            if (existingItem == null)
            {
                // No estaba → lo añadimos
                ReturnOrder.Items.Add(new ReturnItemForCreateDTO
                {
                    ProductId = product.Id,
                    PurchaseOrderId = product.PurchaseOrderId,
                    Quantity = quantity,
                    Reason = reason
                });
            }
            else
            {
                // Ya estaba → sumamos cantidad y, si viene, actualizamos razón
                existingItem.Quantity += quantity;
                if (!string.IsNullOrWhiteSpace(reason))
                    existingItem.Reason = reason;
            }

            NotifyStateChanged();
        }

        // Eliminar un producto de la devolución
        public void RemoveItem(ReturnItemForCreateDTO item)
        {
            ReturnOrder.Items.Remove(item);
            NotifyStateChanged();
        }

        // Vaciar el carrito de devolución
        public void ClearReturningCart()
        {
            ReturnOrder.Items.Clear();
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

        // Hemos terminado una devolución → empezamos otra desde 0
        public void ResetAfterProcessing()
        {
            ReturnOrder = new ReturnPurchaseOrderForCreateDTO()
            {
                CustomerUserName = "",
                ReturningOptionSelected = "",
                Rating = null,
                Items = new List<ReturnItemForCreateDTO>()
            };

            NotifyStateChanged();
        }
    }
}
