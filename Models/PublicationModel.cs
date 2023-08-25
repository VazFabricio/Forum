using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Controllers;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ForumUniversitario.Models
{
    public class PublicationModel : Publication
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ForumUniversitarioUser> _userManager;
        private readonly ForumUniversitarioContext db;

        public PublicationModel(ForumUniversitarioContext contexto)
        {
            db = contexto;
        }

        public List<Publication> getPublications()
        {
            var publications =
                db.PUBLICATION.Include(p => p.User).Include(p => p.Community)
                    .ToList();

            return publications;
        }

        public bool isAbleToPost(string userId, int SecondsBetweenPosts)
        {

            var lastPost = db.PUBLICATION.Where(p => p.UserId == userId).OrderByDescending(p => p.CreatedAt).FirstOrDefault();

            if (lastPost != null)
            {
                // Verificar se já se passaram 30 segundos
                TimeSpan elapsedTime = DateTime.Now - lastPost.CreatedAt;
                if (elapsedTime.TotalSeconds < SecondsBetweenPosts)
                {
                    return false;
                }

                return true;
            }

            return true;
        }

        public Community GetCommunityByName(string communityName)
        {
            return db.COMMUNITY.FirstOrDefault(c => c.Name == communityName);
        }

        public void SavePublication(Publication publication, int communityId, string UserId)
        {
            publication.UserId = UserId;
            publication.CreatedAt = DateTime.Now;
            publication.CommunityId = communityId;

            db.PUBLICATION.Add(publication);
            db.SaveChanges();
        }

        public Publication GetPublicationById(int? id)
        {
            return db.PUBLICATION.FirstOrDefault(p => p.Id == id);
        }



        public ForumUniversitarioUser GetUserByPublicationId(int publicationId)
        {
            var publication = db.PUBLICATION
                .Include(p => p.User) // Carrega informações do usuário
                .FirstOrDefault(p => p.Id == publicationId);

            if (publication != null)
            {
                return publication.User;
            }
            return null;
        }

        public void UpdatePublication(Publication publication)
        {
            db.PUBLICATION.Update(publication);
            db.SaveChanges();
        }

    }



}