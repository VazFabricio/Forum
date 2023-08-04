using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForumUniversitario.Data;

public class ForumUniversitarioContext : IdentityDbContext<ForumUniversitarioUser>
{
    public ForumUniversitarioContext(DbContextOptions<ForumUniversitarioContext> options)
        : base(options)
    {
    }

    public DbSet<Publication> PUBLICATION { get; set; }

    public DbSet<Community> COMMUNITY { get; set; }

    public async Task<bool> IsUserNameUnique(string userName)
    {
        // Realiza a consulta no banco de dados para verificar se o nome de usuário já existe
        return await Users.AllAsync(u => u.UserName == userName);
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
