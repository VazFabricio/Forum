using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using ForumUniversitario.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumUniversitario.Controllers
{
    public class CommentController : Controller
    {


        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ForumUniversitarioUser> _userManager;
        private readonly ForumUniversitarioContext db;
        private readonly CommentModel _commentModel;
        private readonly PublicationModel _publicationModel;

        public CommentController(ForumUniversitarioContext contexto,
            UserManager<ForumUniversitarioUser> userManager,
            CommentModel commentModel,
            PublicationModel publicationModel)
        {
            db = contexto;
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment collection)
        {
            collection.UserId = _userManager.GetUserId(this.User);
            _commentModel.SaveComment(collection, collection.UserId);
            int publicationId = collection.PublicationId;

            return RedirectToAction("Details", "Publication", new { id = publicationId });
        }

    }
}
