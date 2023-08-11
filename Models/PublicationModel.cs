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

        public bool IsCommunityIdValid(int communityId)
        {

            var community = db.COMMUNITY.Find(communityId);
            return community != null;
        }

        public string GetCommunityName(int publicationId)
        {
            var publication = db.PUBLICATION
                .Where(p => p.Id == publicationId)
                .FirstOrDefault();

            if (publication != null)
            {
                // Certifique-se de que a propriedade Community esteja carregada corretamente
                db.Entry(publication).Reference(p => p.Community).Load();
                return publication.Community?.Name; // Retorna o nome da comunidade
            }

            return null; // Retorna null caso a publicação não seja encontrada
        }


    }



}
