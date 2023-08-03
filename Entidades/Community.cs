using ForumUniversitario.Areas.Identity.Data;

namespace ForumUniversitario.Entidades
{
    public class Community
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // ID do usuário que criou a comunidade
        public string UserId { get; set; }

        // Referência ao usuário que criou a comunidade
        public ForumUniversitarioUser User { get; set; }

        // Lista de usuários inscritos na comunidade
        public ICollection<ForumUniversitarioUser> Subscribers { get; set; }

        // Lista de publicações associadas à comunidade
        public ICollection<Publication> Publications { get; set; }

    }
}
