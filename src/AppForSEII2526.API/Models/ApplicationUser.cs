using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{


    public ApplicationUser() { }

    public ApplicationUser(string id, string name, string surname, string address)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Address = address;
        AccountCreationDate = DateTime.Now;

    }

    public ApplicationUser(string id, string name, string surname, string address, string username)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Address = address;
        AccountCreationDate = DateTime.Now;
        UserName = username;
        Email = username;

    }

    public ApplicationUser(string name, string surname, string address)
    {
        Name = name;
        Surname = surname;
        Address = address;
        AccountCreationDate = DateTime.Now;
    }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [Display(Name = "Surname")]
    public string Surname { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Address")]
    public string Address { get; set; }


    [Display(Name = "Account Creation Date")]
    public DateTime AccountCreationDate { get; set; }

    public IList<ReturnPurchaseOrder> ReturnOrders { get; set; } = new List<ReturnPurchaseOrder>();

    public IList<PurchaseOrder> PurchaseOrders { get; set; }

    public List<Complaint> Complaint { get; set; }

    public List<ReportCustomer> ReportCustomer { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is ApplicationUser other)
            return Id == other.Id;
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }


}