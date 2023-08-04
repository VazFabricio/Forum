using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Entidades;

namespace ForumUniversitario.Models
{
    public class ForumUniversitarioUserModel : ForumUniversitarioUser
    {

        // Lista de comunidades em que o usuário está inscrito
        public ICollection<Community> SubscribedCommunities { get; set; }

        // Lista de publicações associadas ao usuário
        public ICollection<Publication> Publications { get; set; }

    }
}
