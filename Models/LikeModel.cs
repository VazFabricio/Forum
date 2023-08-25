using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Controllers;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using Microsoft.AspNetCore.Identity;

namespace ForumUniversitario.Models;

public class LikeModel : Like
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ForumUniversitarioUser> _userManager;
    private readonly ForumUniversitarioContext db;

    public LikeModel(ForumUniversitarioContext contexto)
    {
        db = contexto;
    }


    public int GetLikesCount(int publicationId)
    {
        return db.LIKES.Count(l => l.PublicationId == publicationId);
    }

    public Like? GetLikeByUserAndPublication(string userId, int publicationId)
    {
        return db.LIKES.FirstOrDefault(l => l.UserId == userId && l.PublicationId == publicationId);
    }

    public void AddLike(string userId, int publicationId)
    {
        var like = new Like
        {
            UserId = userId,
            PublicationId = publicationId
        };

        db.LIKES.Add(like);
        db.SaveChanges();
    }

    public void RemoveLike(Like like)
    {
        db.LIKES.Remove(like);
        db.SaveChanges();
    }

}