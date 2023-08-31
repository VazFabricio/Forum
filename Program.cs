using ForumUniversitario.Areas.Identity.Data;
using ForumUniversitario.Data;
using ForumUniversitario.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ForumUniversitarioContextConnection") ?? throw new InvalidOperationException("Connection string 'ForumUniversitarioContextConnection' not found.");

builder.Services.AddDbContext<ForumUniversitarioContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddDefaultIdentity<ForumUniversitarioUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ForumUniversitarioContext>();

builder.Services.AddScoped<PublicationModel>();
builder.Services.AddScoped<CommunityModel>();
builder.Services.AddScoped<CommentModel>();
builder.Services.AddScoped<LikeModel>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); ;
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "community",
        pattern: "community/{id}",
        defaults: new { controller = "Community", action = "CommunityMainPage" });

    endpoints.MapControllerRoute(
        name: "createComment",
        pattern: "Comment/Create/{publicationId}",
        defaults: new { controller = "Comment", action = "Create" });

    endpoints.MapControllerRoute(
        name: "api",
        pattern: "api/{controller}/{action}/{id?}");

    endpoints.MapControllerRoute(
        name: "teste",
        pattern: "Publication/LikePublication/{id}",
        defaults: new { controller = "Publication", action = "LikePublication" });
    
    // endpoints.MapControllerRoute(
    //     name: "isLiked",
    //     pattern: "Publication/{id}/isLike",
    //     defaults: new { controller = "Publication", action = "IsPublicationLiked" });

    endpoints.MapDefaultControllerRoute();
});


app.Run();
