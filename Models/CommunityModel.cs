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
            // Mensagem de depuração
            Console.WriteLine($"Seguindo a comunidade {communityId}");

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
        else
        {
            // Mensagem de depuração
            Console.WriteLine($"Usuário já segue a comunidade {communityId}");
        }
    }

    
    public void UnfollowCommunity(string userId, int communityId)
    {
        // Verificar se o usuário já está seguindo a comunidade
        if (IsUserFollowing(userId, communityId))
        {
            // Mensagem de depuração
            Console.WriteLine($"Deixar de seguir a comunidade {communityId}");

            var membership = db.MEMBERSHIP.SingleOrDefault(m => m.UserId == userId && m.CommunityId == communityId);

            int membershipId = membership.Id;

            db.MEMBERSHIP.Remove(membership);
            db.SaveChanges();
        }
        else
        {
            // Mensagem de depuração
            Console.WriteLine($"Usuário não segue a comunidade {communityId}");
        }
    }

    public bool IsUserFollowing(string userId, int communityId)
    {
        return db.MEMBERSHIP.Any(m => m.UserId == userId && m.CommunityId == communityId);
    }

    public string GetCommunityNameById(int communityId)
    {
        var community = db.COMMUNITY.FirstOrDefault(c => c.Id == communityId);
        return community != null ? community.Name : null;
    }
}