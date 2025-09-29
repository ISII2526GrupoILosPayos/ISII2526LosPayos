using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ReturnPurchaseOrder> ReturnPurchaseOrders { get; set; }
    public DbSet<ReturnProduct> ReturnProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseProduct> PurchaseProducts { get; set; }
}
