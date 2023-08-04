using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Entidades;

namespace ForumUniversitario.Models;

public class CommunityModel : Community
{
        
        // Lista de usuários inscritos na comunidade
        public ICollection<ForumUniversitarioUser> Subscribers { get; set; }

        // Lista de publicações associadas à comunidade
        public ICollection<Publication> Publications { get; set; }

}