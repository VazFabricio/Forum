﻿using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using ForumUniversitario.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumUniversitario.Controllers
{
    public class CommentController : Controller
    {

        
        private readonly UserManager<ForumUniversitarioUser> _userManager;
        private readonly CommentModel _commentModel;
        private readonly PublicationModel _publicationModel;

        public CommentController(
            UserManager<ForumUniversitarioUser> userManager,
            CommentModel commentModel,
            PublicationModel publicationModel)
        {
            this._userManager = userManager;
            _commentModel = commentModel;
            _publicationModel = publicationModel;
        }

        // GET: CommentController
        public ActionResult Index()
        {
            var comments = _commentModel.GetComments();

            return View(comments);
        }


        // GET: CommentController/Create
        public IActionResult Create(int? publicationId, int? fatherCommentId)
        {

            string userId = _userManager.GetUserId(this.User);
            ViewData["UserID"] = userId;
            
            var publication = _publicationModel.GetPublicationById(publicationId);
            var fatherComment = _commentModel.GetCommentById(fatherCommentId);
            
            if (publication == null)
            {
                // Redirecione ou retorne algum erro, pois a publicação não foi encontrada
                return RedirectToAction("Index", "Home"); // Exemplo de redirecionamento para a página inicial
            }
            
            ViewData["PublicationId"] = publication.Id;
            ViewData["FatherCommentId"] = fatherComment != null ? fatherComment.Id : null;
            
            
            ViewBag.Publication = publication;

            // Outras lógicas de preparação de dados

            return View();
        }
        
        // POST: CommentController/Create
        [HttpPost]
        public ActionResult Create(Comment collection)
        {
            collection.UserId = _userManager.GetUserId(this.User);
            _commentModel.SaveComment(collection, collection.UserId);
            int publicationId = collection.PublicationId;

            return RedirectToAction("Details", "Publication", new { id = publicationId });
        }
        
        
        [HttpPost]
        [Route("Comment/CreateComment")]
        public IActionResult CreateComment([FromBody] Comment comment)
        {
            
            // Verificar se a publicação existe (sugestão: use um serviço ou repositório para verificar)
            var publication = _publicationModel.GetPublicationById(comment.PublicationId);
            if (publication == null)
            {
                return NotFound("Publicação não encontrada");
            }


            if (comment.Content == "")
            {
                return NotFound("Comentário sem conteúdo");
            }

            // Se fatherCommentId for fornecido, verifique se o comentário pai existe
            Comment fatherComment = null;
            
            if (comment.ParentId.HasValue)
            {
                fatherComment = _commentModel.GetCommentById(comment.ParentId);
                if (fatherComment == null)
                {
                    return NotFound("Comentário pai não encontrado");
                }
            }


            // Salvar o comentário no banco de dados
            _commentModel.SaveComment(comment, _userManager.GetUserId(this.User));

            return Ok("Comentário criado com sucesso");
        }


    }
}
