using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForMovies.UT;
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using Humanizer.Localisation;

namespace AppForSEII2526.UT.PurchaseOrdersController_test
{
    public class CreatePurchaseOrder_test : AppForSEII25264SqliteUT
    {
        private const string _userName = "luis@uclm.es";
        private const string _customerNameSurname = "Luis Melero";
        private const string _street = "Cueva de Montesinos";
        private const string _city = "Villarrobledo";
        private const string _postalCode = "02600";

        private const string _product1Name = "Water";
        private const string _product1Brand = "Bezoya";
        private const string _product2Name = "Hoodie";
        private const string _product2Brand = "Nike";

        public CreatePurchaseOrder_test()
        {
            var brands = new List<Brand>()
            {
                new Brand(_product1Brand,"Palencia"),
                new Brand(_product2Brand, "Madrid"),
            };

            var products = new List<Product>()
            {
                new Product(_product1Name, "For hydratate yourself", "Blue", 1, 100, brands[0]),
                new Product(_product1Name, "For warming yourself", "Red", 69, 100, brands[1]),
            };

            ApplicationUser user = new ApplicationUser("1", "Luis", "Melero Jareño", _street);

            var paymentMethod = new Bizum(666666666)
            {
                User = user
            };

            var purchaseOrder = new PurchaseOrder(_customerNameSurname, user, _street, _city, _postalCode, DateTime.Now, paymentMethod, new List<PurchaseProduct>());

            purchaseOrder.Products.Add(new PurchaseProduct(products[0], products[0].ProductId, purchaseOrder, 1, products[0].Price));
            purchaseOrder.Products.Add(new PurchaseProduct(products[1], products[1].ProductId, purchaseOrder, 1, products[1].Price));

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.Add(user);
            _context.Add(paymentMethod);
            _context.Add(purchaseOrder);
            _context.SaveChanges();
        }
    }
}
