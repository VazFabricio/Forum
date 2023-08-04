using ForumUniversitario.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

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

        [Required(ErrorMessage = "Selecione uma comunidade.")]
        public int CommunityId { get; set; }
        public Community Community { get; set; }


    }
}
