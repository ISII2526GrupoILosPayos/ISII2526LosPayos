using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Name = "Luis";
        Surname = "Melero";
        Address = "Avda. España, 10, Albacete";
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
}

