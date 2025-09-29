using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<ReturnPurchaseOrder> ReturnPurchaseOrders { get; set; }
    public DbSet<PurchaseProduct> PurchaseProducts { get; set; }

}
