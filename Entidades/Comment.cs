using ForumUniversitario.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumUniversitario.Entidades
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 10)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        // Propriedade que representa o relacionamento com o usuário
        public string UserId { get; set; }
        public ForumUniversitarioUser User { get; set; }

        [ForeignKey("ParentId")] public int? ParentId { get; set; }

        [ForeignKey("ParentId")] public Comment ParentComment { get; set; }

        [Required] public int PublicationId { get; set; } // ID da publicação associada
        public Publication Publication { get; set; } // Referência para a publicação associada

        [NotMapped]
        public List<Comment> ChildComments { get; set; }
    }
}