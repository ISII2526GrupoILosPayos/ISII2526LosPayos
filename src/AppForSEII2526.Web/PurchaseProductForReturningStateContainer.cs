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
            var existingItem = ReturnOrder.Items
                .FirstOrDefault(i => i.ProductId == product.ProductId &&
                                     i.PurchaseOrderId == product.PurchaseOrderId);

            if (existingItem == null)
            {
                ReturnOrder.Items.Add(new ReturnItemForCreateDTO
                {
                    ProductId = product.ProductId,
                    PurchaseOrderId = product.PurchaseOrderId,
                    Quantity = quantity,
                    Reason = reason,

                    // ✅ INFO PARA PINTAR EN LA UI (Create)
                    ProductName = product.Name,
                    BrandName = product.Brand,
                    BrandLocation = product.Location
                });
            }
            else
            {
                existingItem.Quantity += quantity;

                if (!string.IsNullOrWhiteSpace(reason))
                    existingItem.Reason = reason;

                // ✅ por si venía vacío (o por si quieres refrescarlo)
                if (string.IsNullOrWhiteSpace(existingItem.ProductName))
                    existingItem.ProductName = product.Name;

                if (string.IsNullOrWhiteSpace(existingItem.BrandName))
                    existingItem.BrandName = product.Brand;

                if (string.IsNullOrWhiteSpace(existingItem.BrandLocation))
                    existingItem.BrandLocation = product.Location;
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

        public void SetCustomerData(
   string userName,
   string name,
   string surname,
   string address,
   string telephone)
        {
            ReturnOrder.CustomerUserName = userName;
            ReturnOrder.CustomerName = name;
            ReturnOrder.CustomerSurname = surname;
            ReturnOrder.CustomerAddress = address;
            ReturnOrder.CustomerTelephone = telephone;

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
