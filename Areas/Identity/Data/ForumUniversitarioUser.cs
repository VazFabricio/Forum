using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using ForumUniversitario.Entidades;
using Microsoft.AspNetCore.Identity;

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

