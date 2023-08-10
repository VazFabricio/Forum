using ForumUniversitario.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumUniversitario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIsController : ControllerBase
    {

        private readonly ForumUniversitarioContext _db;

        public APIsController(ForumUniversitarioContext db)
        {
            _db = db;
        }

        // Retorna o número total de publicações
        [HttpGet("total")]
        public IActionResult GetTotalPublications()
        {
            int totalPublications = _db.PUBLICATION.Count();
            return Ok(totalPublications);
        }


    }
}
