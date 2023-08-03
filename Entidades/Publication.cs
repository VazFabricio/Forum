using ForumUniversitario.Areas.Identity.Data;

namespace ForumUniversitario.Entidades
{
    public class Publication
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Propriedade que representa o relacionamento com o usuário
        public string UserId { get; set; }

        public ForumUniversitarioUser User { get; set; }



    }
}
