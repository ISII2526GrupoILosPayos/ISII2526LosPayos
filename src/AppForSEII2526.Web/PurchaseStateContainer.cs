using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        // an order is created when the container is instantiated
        public PurchaseOrderForCreateDTO Purchase { get; private set; } = new PurchaseOrderForCreateDTO()
        {
            PurchaseProducts = new List<PurchaseProductForCreateDTO>()
        };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();


        // add product to cart
        public void AddProductToPurchase(ProductForPurchaseDTO product)
        {
            var existingItem = Purchase.PurchaseProducts
                .FirstOrDefault(p => p.Name == product.Name);

            if (existingItem == null)
            {
                Purchase.PurchaseProducts.Add(new PurchaseProductForCreateDTO()
                {
                    Name = product.Name,
                    Quantity = 1
                });
            }
            else
            {
                existingItem.Quantity++;
            }

            NotifyStateChanged();
        }


        // remove one item
        public void RemoveProductFromPurchase(PurchaseProductForCreateDTO item)
        {
            Purchase.PurchaseProducts.Remove(item);
            NotifyStateChanged();
        }


        // clear cart
        public void ClearPurchaseCart()
        {
            Purchase.PurchaseProducts.Clear();
            NotifyStateChanged();
        }


        // after finishing purchase
        public void PurchaseProcessed()
        {
            Purchase = new PurchaseOrderForCreateDTO()
            {
                PurchaseProducts = new List<PurchaseProductForCreateDTO>()
            };

            NotifyStateChanged();
        }
    }
}
