namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
{
    public class PurchaseProductForReturnDTO
    {
        public PurchaseProductForReturnDTO(int id, string name, string brand, int quantity, string location)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Quantity = quantity;
            Location = location;
        }

        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }

        //Nombre del Producto a Devolver
        [StringLength(50, ErrorMessage = "Name of the Product cannot be longer than 50 characters")]
        public string Name { get; set; }

        public string Brand { get; set; }

        public int Quantity { get; set; }

        public string Location { get; set; }
    }

    }
