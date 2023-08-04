using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ForumUniversitario.Areas.Identity.Data;

using ForumUniversitario.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ForumUniversitario.Entidades;

public class Community
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string Description { get; set; }


    [Column(TypeName = "nvarchar(60)")]
     public string Name { get; set; }

    [PersonalData]
    public DateTime CreatedAt { get; set; }

    // ID do usuário que criou a comunidade

    [PersonalData]
    [Column(TypeName = "nvarchar(255)")]
    public string UserId { get; set; }
    
     // Referência ao usuário que criou a comunidade
     public ForumUniversitarioUser User { get; set; }
    

}