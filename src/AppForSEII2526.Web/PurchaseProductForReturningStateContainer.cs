
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

                
                if (string.IsNullOrWhiteSpace(existingItem.ProductName))
                    existingItem.ProductName = product.Name;

                if (string.IsNullOrWhiteSpace(existingItem.BrandName))
                    existingItem.BrandName = product.Brand;

                if (string.IsNullOrWhiteSpace(existingItem.BrandLocation))
                    existingItem.BrandLocation = product.Location;
            }

            NotifyStateChanged();
        }


      
        public void RemoveItem(ReturnItemForCreateDTO item)
        {
            ReturnOrder.Items.Remove(item);
            NotifyStateChanged();
        }

        public void ClearReturningCart()
        {
            Console.WriteLine(">>> ClearReturningCart() called");
            ReturnOrder.Items.Clear();
            NotifyStateChanged();
        }

        public void ResetAfterProcessing()
        {
            Console.WriteLine(">>> ResetAfterProcessing() called");
            ReturnOrder = new ReturnPurchaseOrderForCreateDTO()
            {
                CustomerUserName = "",
                ReturningOptionSelected = "",
                Rating = null,
                Items = new List<ReturnItemForCreateDTO>()
            };
            NotifyStateChanged();
        }


        public void SetReturningOption(string option)
        {
            ReturnOrder.ReturningOptionSelected = option;
            NotifyStateChanged();
        }

       
        public void SetRating(int? rating)
        {
            ReturnOrder.Rating = rating;
            NotifyStateChanged();
        }

       
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

        
       
    }
}
