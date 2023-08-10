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
        private readonly ForumUniversitarioContext db;
        private readonly PublicationModel _publicationModel;

        public PublicationController(ForumUniversitarioContext contexto, UserManager<ForumUniversitarioUser> userManager, PublicationModel publicationModel)
        {
            db = contexto;
            this._userManager = userManager;
            _publicationModel = publicationModel;
        }


        // Método para exibir todas as publicações do usuário logado
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(this.User);
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            var publicacoes = db.PUBLICATION.Include(p => p.User).Include(p => p.Community).ToList(); // Inclui o usuário associado à publicação.ToListAsync();
            string nomeUsuario = user.AccountName;
            ViewData["UserName"] = nomeUsuario;
            return View(publicacoes);
        }

        public IActionResult Create(int? communityId)
        {
            string userId = _userManager.GetUserId(this.User);
            ViewData["UserID"] = userId;
            //ViewData["CommunityId"] = communityId;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Publication collection)

        {

            if (!_publicationModel.IsCommunityIdValid(collection.CommunityId))
            {
                ModelState.AddModelError("CommunityId", "A comunidade informada não existe.");
                return View(collection);
            }


            collection.UserId = _userManager.GetUserId(this.User);
            collection.CreatedAt = DateTime.Now;
            db.PUBLICATION.Add(collection);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
