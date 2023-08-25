using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumUniversitario.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ForumUniversitarioUser class
public class ForumUniversitarioUser : IdentityUser
{


    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string AccountName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public DateTime CreatedAt { get; set; }


}

