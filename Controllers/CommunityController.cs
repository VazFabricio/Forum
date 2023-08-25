using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Entidades;
using ForumUniversitario.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumUniversitario.Controllers;

public class CommunityController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ForumUniversitarioUser> _userManager;
    private readonly CommunityModel _communityModel;

    public CommunityController(UserManager<ForumUniversitarioUser> userManager, CommunityModel communityModel)
    {
        this._userManager = userManager;
        _communityModel = communityModel;
    }

    // GET
    public IActionResult Index()
    {
        var communities = _communityModel.GetAllCommunities();
        return View(communities);
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

        string communityName = collection.Name;

        // Verificar se o nome da comunidade já existe
        bool isCommunityNameUnique = _communityModel.IsCommunityNameUnique(communityName);

        if (!isCommunityNameUnique)
        {
            ModelState.AddModelError("Name", "Já existe uma comunidade com este nome.");
            return View(collection);
        }

        _communityModel.SaveCommunity(collection, collection.UserId);

        return RedirectToAction("Index");
    }


    public IActionResult CommunityMainPage(int id)
    {
        var community = _communityModel.GetCommunityById(id);

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

    [HttpPost]
    public IActionResult UnfollowCommunityFromButton(int id)
    {
        string userId = _userManager.GetUserId(this.User);
        _communityModel.UnfollowCommunity(userId, id);

        return RedirectToAction("CommunityMainPage", new { id });
    }

}