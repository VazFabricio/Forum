using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Controllers;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ForumUniversitario.Models
{
    public class CommentModel : Comment
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ForumUniversitarioUser> _userManager;
        private readonly ForumUniversitarioContext db;

        public CommentModel(ForumUniversitarioContext contexto)
        {
            db = contexto;
        }

        public List<Comment> GetComments()
        {
            return db.COMMENT.ToList();
        }

        public void SaveComment(Comment comment, string UserId)
        {
            comment.UserId = UserId;
            comment.CreatedAt = DateTime.Now;

            db.COMMENT.Add(comment);
            db.SaveChanges();

        }

        public List<Comment> GetDirectComments(int publicationId)
        {
            return db.COMMENT
                .Where(comment => comment.PublicationId == publicationId && comment.ParentId == null)
                .Include(comment => comment.User) // Carrega informações do usuário
                .ToList();
        }

        public List<Comment> GetAllCommentsForPublication(int publicationId)
        {
            var comments = db.COMMENT
                .Where(c => c.PublicationId == publicationId)
                .ToList();

            foreach (var comment in comments)
            {
                //Se um comentário tem um superior 
                if (comment.ParentId.HasValue)
                {
                    //Pega o valor desse superior 
                    var parentComment = comments.FirstOrDefault(c => c.Id == comment.ParentId);

                    //Garantindo a presença de um valor para casos de erros, mas não é obrigatório
                    if (parentComment != null)
                    {
                        //Se o comentário pai nao tiver sua lista existente, ele cria a lista
                        if (parentComment.ChildComments == null)
                        {
                            parentComment.ChildComments = new List<Comment>();
                        }

                        //pega o comentário que foi definido como filho e adiciona na lista de filhos do pai
                        parentComment.ChildComments.Add(comment);
                    }
                }

                //se o comentário nao tiver um pai, siguinifica que ele é direto na publicação, por isso ele retorna com 
                //sua propriedade de List<ChildComents> vazia.
            }

            //Retorna na lista final somente os que principais, pois eles já tem os filhos armazenados na sua propriedade.
            return comments.Where(c => !c.ParentId.HasValue).ToList();
        }

        public ForumUniversitarioUser GetUserByCommentId(int CommentId)
        {
            var comment = db.COMMENT
                .Include(p => p.User) // Carrega informações do usuário
                .FirstOrDefault(p => p.Id == CommentId);

            if (comment != null)
            {
                return comment.User;
            }

            return null;
        }

        public Comment GetCommentById(int? commentId)
        {
            var comment = db.COMMENT.FirstOrDefault(p => p.Id == commentId);
            return comment;
        }
    }
}