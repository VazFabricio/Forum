using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Data;
using ForumUniversitario.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumUniversitario.Controllers;

public class CommunityController : Controller
{
    private readonly ForumUniversitarioContext db;
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ForumUniversitarioUser> _userManager;

    public CommunityController(ForumUniversitarioContext context, UserManager<ForumUniversitarioUser> userManager)
    {
        db = context;
        this._userManager = userManager;
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
        db.COMMUNITY.Add(collection);
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}