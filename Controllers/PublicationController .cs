using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using ForumUniversitario.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumUniversitario.Controllers
{
  [Authorize]
    public class PublicationController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ForumUniversitarioUser> _userManager;
        private readonly PublicationModel _publicationModel;
        private readonly LikeModel _likeModel;
        private readonly CommunityModel _communityModel;
        private readonly CommentModel _commentModel;


        public PublicationController(UserManager<ForumUniversitarioUser> userManager,
            PublicationModel publicationModel,
            CommunityModel communityModel,
            CommentModel commentModel,
            LikeModel likeModel
             )
        {
            this._userManager = userManager;
            _publicationModel = publicationModel;
            _communityModel = communityModel;
            _commentModel = commentModel;
            _likeModel = likeModel;


        }


        // Método para exibir todas as publicações do usuário logado
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(this.User);
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            ViewData["UserName"] = user.AccountName;
            

            var publications = _publicationModel.GetPublications();

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
            var errors = new List<string>();

            var community = _publicationModel.GetCommunityByName(collection.CommunityName);

            if (community == null)
            {
                errors.Add("A comunidade informada não existe.");
            }

            if (community != null)
            {
                bool isUserFollowing = _communityModel.IsUserFollowing(_userManager.GetUserId(this.User), community.Id);
                if (!isUserFollowing)
                {
                    errors.Add("Você não segue a comunidade.");
                }
            }

            bool isAbleToPost = _publicationModel.IsAbleToPost(_userManager.GetUserId(this.User), 30);

            if (!isAbleToPost)
            {
                errors.Add("Aguarde pelo menos 30 segundos antes de fazer outra postagem.");
            }

            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
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

        public IActionResult Search(string query)
        {
            // Realize uma consulta no banco de dados ou na sua fonte de dados para obter as comunidades correspondentes
            var matchingCommunities = _communityModel.GetCommunitiesNameLike(query);

            return Json(matchingCommunities);
        }

        [HttpPost]
        public IActionResult LikePublication(int id)
        {
            var userId = _userManager.GetUserId(this.User);

            var publication = _publicationModel.GetPublicationById(id);

            
            var existingLike = _likeModel.GetLikeByUserAndPublication(userId,publication.Id);

            if (existingLike != null)
            {
               
                _likeModel.RemoveLike(existingLike);
            }
            else
            {
               
                _likeModel.AddLike(userId, publication.Id);
            }

            return Ok(); 
        }
        
        [HttpGet("/publication/{id}/isliked")]
        public IActionResult IsPublicationLiked(int id)
        {
            var userId = _userManager.GetUserId(this.User);
            var isLiked = _likeModel.IsPublicationLiked(userId, id);
            return Ok(isLiked);
        }
        
        
        


    }
}
