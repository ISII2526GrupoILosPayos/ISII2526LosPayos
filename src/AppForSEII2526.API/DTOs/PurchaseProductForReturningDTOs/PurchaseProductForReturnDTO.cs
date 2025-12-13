
namespace AppForSEII2526.API.DTOs.ReturnProductDTOs
{
    public class PurchaseProductForReturnDTO
    {
        public PurchaseProductForReturnDTO(int id, string name, string brand, int quantity, string location,bool returnable, int productid)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Quantity = quantity;
            Location = location;
            IsReturnable = returnable; // Por defecto, el producto es retornable
            ProductId = productid;
        }

        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }

        //Nombre del Producto a Devolver
        [StringLength(50, ErrorMessage = "Name of the Product cannot be longer than 50 characters")]
        public string Name { get; set; }

        public string Brand { get; set; }

        public int Quantity { get; set; }

        public string Location { get; set; }

        public bool IsReturnable { get; set; }   // <-- clave

        public int ProductId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseProductForReturnDTO dTO &&
                   Id == dTO.Id &&
                   PurchaseOrderId == dTO.PurchaseOrderId &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Quantity == dTO.Quantity &&
                   Location == dTO.Location&&
                   IsReturnable == dTO.IsReturnable&&
                   ProductId == dTO.ProductId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, PurchaseOrderId, Name, Brand, Quantity, Location);
        }
    }

    }
