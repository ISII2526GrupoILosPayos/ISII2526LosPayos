
namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
{
    public class ReturnedProductDTO
    {
        public ReturnedProductDTO(
            int quantity,
            string productName,
            string brandName,
            string warehouseLocation,
            string reason)
        {
            Quantity = quantity;
            ProductName = productName;
            BrandName = brandName;
            WarehouseLocation = warehouseLocation;
            Reason = reason;
        }

        public int Quantity { get; set; }              // the quantity
        public string ProductName { get; set; }        // name
        public string BrandName { get; set; }          // name of the firm
        public string WarehouseLocation { get; set; }  // location of the warehouse
        public string Reason { get; set; }             // returning option selected

        public override bool Equals(object? obj)
        {
            return obj is ReturnedProductDTO dTO &&
                   Quantity == dTO.Quantity &&
                   ProductName == dTO.ProductName &&
                   BrandName == dTO.BrandName &&
                   WarehouseLocation == dTO.WarehouseLocation &&
                   Reason == dTO.Reason;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Quantity, ProductName, BrandName, WarehouseLocation, Reason);
        }
    }
}
