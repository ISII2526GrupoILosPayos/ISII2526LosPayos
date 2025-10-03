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
    public DbSet <PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Bizum> Bizums { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<PayPal> PayPals { get; set; }

    //Clases Hugo
    public DbSet<BanReport> BanReports { get; set; }
    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<ReportCustomer> ReportCustomers { get; set; }
    public DbSet<ComplaintType> ComplaintTypes { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ReturnProduct>()
            .HasOne(rp => rp.PurchaseProduct)
            .WithOne(pp => pp.ReturnProduct)
            .HasForeignKey<ReturnProduct>(rp => new { rp.ProductId, rp.PurchaseOrderId });

        builder.Entity<ReportCustomer>()
            .HasKey(rc => new { rc.BanReportId, rc.CustomerId });
        
        builder.Entity<PurchaseOrder>()
            .Property(p => p.State)
            .HasConversion<string>();

        builder.Entity<PurchaseOrder>()
            .HasOne(po => po.PaymentMethod)
            .WithMany() 
            .OnDelete(DeleteBehavior.Restrict);
    }
}
