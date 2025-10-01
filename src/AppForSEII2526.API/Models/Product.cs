using Microsoft.CodeAnalysis;
using System.Drawing;

namespace AppForSEII2526.API.Models
{
    public class Product
    {

        public Product() { }

        public Product(int productId, string name, string description, string colour, decimal price, int stock)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Colour = colour;
            Price = decimal.Round(price, 2);
            Stock = stock;
           
        }

        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }


        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Colour { get; set; }

        [Required]
        [Precision(10, 2)]
        [Range(0, 9999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or positive")]
        public int Stock { get; set; }

        [Required]
        public bool IsReturnable { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Product other)
                return this.ProductId == other.ProductId;
            return false;
        }

        // Clave foránea hacia Brand
        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
