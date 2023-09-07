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

        private readonly ForumUniversitarioContext _db;

        public PublicationModel(ForumUniversitarioContext contexto)
        {
            _db = contexto;
        }

        public List<Publication> GetPublications()
        {
            var publications =
                _db.PUBLICATION.Include(p => p.User).Include(p => p.Community)
                    .ToList();

            return publications;
        }

        public bool IsAbleToPost(string userId, int secondsBetweenPosts)
        {

            var lastPost = _db.PUBLICATION.Where(p => p.UserId == userId).OrderByDescending(p => p.CreatedAt).FirstOrDefault();

            if (lastPost != null)
            {
                // Verificar se já se passaram 30 segundos
                TimeSpan elapsedTime = DateTime.Now - lastPost.CreatedAt;
                if (elapsedTime.TotalSeconds < secondsBetweenPosts)
                {
                    return false;
                }

                return true;
            }

            return true;
        }

        public Community GetCommunityByName(string communityName)
        {
            return _db.COMMUNITY.FirstOrDefault(c => c.Name == communityName);
        }

        public void SavePublication(Publication publication, int communityId, string UserId)
        {
            
            publication.UserId = UserId;
            publication.CreatedAt = DateTime.Now;
            publication.CommunityId = communityId;
        
            _db.PUBLICATION.Add(publication);
            _db.SaveChanges();
        }
        
        // public void SavePublication(Publication publication, int communityId, string UserId)
        // {
        //     
        //     var newPublication = new Publication
        //     {
        //         Id = Guid.NewGuid(),
        //         Title = publication.Title,
        //         Content = publication.Content,
        //         CreatedAt = DateTime.UtcNow,
        //         UserId = publication.UserId,
        //         CommunityId = publication.CommunityId
        //     };
        //     
        //
        //     _db.PUBLICATION.Add(newPublication);
        //     _db.SaveChanges();
        // }

        public Publication GetPublicationById(int? id)
        {
            return _db.PUBLICATION.FirstOrDefault(p => p.Id == id);
        }



        public ForumUniversitarioUser GetUserByPublicationId(int publicationId)
        {
            var publication = _db.PUBLICATION
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
            _db.PUBLICATION.Update(publication);
            _db.SaveChanges();
        }

    }



}