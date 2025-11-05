namespace AppForSEII2526.API.DTOs.ReturnPurchaseOrderDTOs
{
    public class ReturnedProductDTO
    {
        public ReturnedProductDTO(
            int quantity,
            string productName,
            string brandName,
            string warehouseLocation)
        {
            Quantity = quantity;
            ProductName = productName;
            BrandName = brandName;
            WarehouseLocation = warehouseLocation;
        }

        public int Quantity { get; set; }              // Quantity
        public string ProductName { get; set; }        // Name
        public string BrandName { get; set; }          // Firm name
        public string WarehouseLocation { get; set; }  // Warehouse location

        public override bool Equals(object? obj)
        {
            return obj is ReturnedProductDTO dTO &&
                   Quantity == dTO.Quantity &&
                   ProductName == dTO.ProductName &&
                   BrandName == dTO.BrandName &&
                   WarehouseLocation == dTO.WarehouseLocation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Quantity, ProductName, BrandName, WarehouseLocation);
        }
    }
}
