using ForumUniversitario.Entidades;
using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Controllers;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ForumUniversitario.Models;

public class CommunityModel : Community
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ForumUniversitarioUser> _userManager;
    private readonly ForumUniversitarioContext db;

    public List<Publication> Publications { get; set; }

    public CommunityModel(ForumUniversitarioContext contexto)
    {
        db = contexto;
        Publications = new List<Publication>();
    }

    public List<Publication> GetPublicationsForCommunity(int communityId)
    {
        Publications = db.PUBLICATION
    .Include(p => p.User) // Include the User navigation property
    .Where(p => p.CommunityId == communityId)
    .ToList();
        return Publications;
    }

    public void FollowCommunity(string userId, int communityId)


    {

        // Verificar se o usuário já está seguindo a comunidade
        if (!IsUserFollowing(userId, communityId))
        {
            Membership membership = new Membership
            {
                UserId = userId,
                CommunityId = communityId,
                JoinDate = DateTime.Now,
                controlLevel = 1 // Defina o nível de controle adequado aqui
            };

            db.MEMBERSHIP.Add(membership);
            db.SaveChanges();

        }

    }

    public bool IsUserFollowing(string userId, int communityId)
    {
        return db.MEMBERSHIP.Any(m => m.UserId == userId && m.CommunityId == communityId);
    }
}