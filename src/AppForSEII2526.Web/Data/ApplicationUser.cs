using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string? Surname { get; set; }
    public string? Address { get; set; }
    public DateTime AccountCreationDate { get; set; }
}

