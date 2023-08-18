using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using ForumUniversitario.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;

namespace ForumUniversitario.Controllers
{
    [Authorize]
    public class PublicationController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ForumUniversitarioUser> _userManager;
        private readonly PublicationModel _publicationModel;
        private readonly CommunityModel _communityModel;
        private readonly CommentModel _commentModel;

        public PublicationController(UserManager<ForumUniversitarioUser> userManager,
            PublicationModel publicationModel,
            CommunityModel communityModel,
            CommentModel commentModel)
        {
            this._userManager = userManager;
            _publicationModel = publicationModel;
            _communityModel = communityModel;
            _commentModel = commentModel;

        }


        // Método para exibir todas as publicações do usuário logado
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(this.User);
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            var publications = _publicationModel.getPublications();

             // Inclui o usuário associado à publicação.ToListAsync();
            string nomeUsuario = user.AccountName;

            return View(publications);
        }

        public IActionResult Create(int? communityId)//Pode ou ñão receber um id pq pode ser que o user vai fazer o input
        {
            string userId = _userManager.GetUserId(this.User);
            ViewData["UserID"] = userId;

            var community = _communityModel.GetCommunityById(communityId);

            if (community == null)
            {
                return View();
            }

            ViewData["CommunityName"] = community.Name;
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Publication collection)
        {

            // Verificar se a comunidade existe pelo nome
            var community = _publicationModel.GetCommunityByName(collection.CommunityName);

            if (community == null)
            {
                ModelState.AddModelError("CommunityName", "A comunidade informada não existe.");
                return View(collection);
            }

            //Verifica se o User segue a comunidade que ele digitou
            bool isUserFollowing = _communityModel.IsUserFollowing(_userManager.GetUserId(this.User), community.Id);

            if (!isUserFollowing)
            {
                ModelState.AddModelError("CommunityName", "Você não segue a comunidade.");
                return View(collection);
            }

            bool isAbleToPost = _publicationModel.isAbleToPost(_userManager.GetUserId(this.User), 30);

            if (!isAbleToPost)
            {
                ModelState.AddModelError("", "Aguarde pelo menos 30 segundos antes de fazer outra postagem.");
                return View(collection);
            }

            _publicationModel.SavePublication(collection, community.Id, _userManager.GetUserId(this.User));

            return RedirectToAction("Index");
        }

        
        

        public IActionResult Details(int id)
        {

            var userPublication = _publicationModel.GetUserByPublicationId(id);
            
            var publication = _publicationModel.GetPublicationById(id);

            if (publication == null)
            {
                return NotFound();
            }
            
            
            var directComments = _commentModel.GetDirectComments(id); // allComments é a lista de todos os comentários

            ViewData["UserNamePublication"] = userPublication.AccountName;

            var allComments = _commentModel.GetAllCommentsForPublication(id);

            
            ViewBag.DirectComments = directComments;
            ViewBag.AllComments = allComments;

            return View(publication);
        }
        
    }
}
