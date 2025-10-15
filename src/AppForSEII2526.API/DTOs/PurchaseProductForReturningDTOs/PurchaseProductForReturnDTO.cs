namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
{
    public class PurchaseProductForReturnDTO
    {
        public PurchaseProductForReturnDTO(int id, string name, string brandName)
        {
            Id = id;
            Name = name;
            Brand = brandName;
        }

        public int Id { get; set; }

        //Nombre del Producto a Devolver
        [StringLength(50, ErrorMessage = "Name of the Product cannot be longer than 50 characters")]
        public string Name { get; set; }

        public string Brand { get; set; }
        }
}
