using ForumUniversitario.Areas.Identity.Data;

namespace ForumUniversitario.Entidades;

public class Like
{
    public int Id { get; set; }
    public string UserId { get; set; } // Identificador do usuário que curtiu
    public int PublicationId { get; set; } // Identificador da publicação curtida
    public DateTime LikedAt { get; set; } // Data/hora da curtida

    public ForumUniversitarioUser User { get; set; } // Propriedade de navegação para o usuário
    public Publication Publication { get; set; } // Propriedade de navegação para a publicação curtida
}