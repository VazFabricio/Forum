using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using ForumUniversitario.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumUniversitario.Controllers;

public class CommunityController : Controller
{
    private readonly ForumUniversitarioContext db;
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ForumUniversitarioUser> _userManager;
    private readonly CommunityModel _communityModel;

    public CommunityController(ForumUniversitarioContext context, UserManager<ForumUniversitarioUser> userManager, CommunityModel communityModel)
    {
        db = context;
        this._userManager = userManager;
        _communityModel = communityModel;
    }
    
    // GET
     public IActionResult Index()
     {
        return View(db.COMMUNITY.ToList());
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Community collection)
    {
        collection.UserId = _userManager.GetUserId(this.User);
        collection.CreatedAt = DateTime.Now;

        string communiTyName = collection.Name;

        // Verificar se o nome da comunidade já existe
        bool isCommunityNameUnique = !db.COMMUNITY.Any(u => u.Name == communiTyName);

        if (!isCommunityNameUnique)
        {
            ModelState.AddModelError("Name", "Já existe uma comunidade com este nome.");
            return View(collection);
        }

        db.COMMUNITY.Add(collection);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
    
    
    public IActionResult CommunityMainPage(int id)
    {
        var community = db.COMMUNITY.FirstOrDefault(c => c.Id == id);

        if (community == null)
        {
            return RedirectToAction("Index");
        }

        // Set the Publications property of the CommunityModel
        var publications = _communityModel.GetPublicationsForCommunity(id);

        ViewData["CommunityName"] = community.Name;
        ViewData["CommunityDesc"] = community.Description;
        ViewData["CommunityCreatedAt"] = community.CreatedAt.ToString("dd/MM/yyyy");
        ViewData["CommunityId"] = community.Id;

        // Get the current user's ID
        string userId = _userManager.GetUserId(this.User);

        ViewData["UserId"] = userId;

        ViewData["IsFollowing"] = _communityModel.IsUserFollowing(userId, id);
        
        
        // Pass the CommunityModel to the view
        return View(_communityModel);
    }

    [HttpPost]
    public IActionResult FollowCommunityFromButton(int id)
    {
        string userId = _userManager.GetUserId(this.User);
        _communityModel.FollowCommunity(userId, id);

        return RedirectToAction("CommunityMainPage", new { id });
    }

}