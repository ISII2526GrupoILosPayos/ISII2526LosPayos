namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
{
    public class ReturnedProductDTO
    {
        public ReturnedProductDTO(int quantity, string productName, string brandName, string warehouseLocation, string reason)
        {
            Quantity = quantity;
            ProductName = productName;
            BrandName = brandName;
            WarehouseLocation = warehouseLocation;
            Reason = reason;
        }

        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string WarehouseLocation { get; set; }
        public string Reason { get; set; }
    }
}
