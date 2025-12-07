using AppForSEII2526.Web.API;


namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        public decimal TotalPrice =>
            Purchase.PurchaseProducts.Sum(p => (decimal)p.Price * p.Quantity);



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
                .FirstOrDefault(p => p.ProductId == product.Id);

            if (existingItem == null)
            {
                if (product.Stock > 0) // solo añadimos si hay stock
                {
                    Purchase.PurchaseProducts.Add(new PurchaseProductForCreateDTO
                    {
                        ProductId = product.Id,
                        Name = product.Name,
                        Brand = product.Brand,
                        Quantity = 1,
                        Location = product.Location,
                        Price = product.Price,
                    });
                }
            }
            else
            {
                if (existingItem.Quantity < product.Stock)
                {
                    existingItem.Quantity++;
                }
            }

            NotifyStateChanged();
        }



        // remove one item
        public void RemoveProductFromPurchase(PurchaseProductForCreateDTO item)
        {
            var existingItem = Purchase.PurchaseProducts
                .FirstOrDefault(p => p.Name == item.Name);

            if (existingItem == null)
                return;

            if (existingItem.Quantity > 1)
            {
                existingItem.Quantity--;
            }
            else
            {
                Purchase.PurchaseProducts.Remove(existingItem);
            }

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
