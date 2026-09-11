using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.MaterialRepositories;
using Server.Service;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/projectmaterials")]
    public class ProjectMaterialsController : ControllerBase
    {
        // Repository som gemmer materialer i databasen
        private readonly IMaterialRepository _repo;

        // Repository som gemmer materialer i databasen
        public ProjectMaterialsController(IMaterialRepository repo)
        {
            _repo = repo;
        }


        // Endpoint: bruges når man uploader en fil

        [HttpPost("upload")]
        public IActionResult Upload(IFormFile? file, [FromQuery] int projectId)
        {
            // Hvis der ikke er valgt en fil, fejlbesked
            if (file == null || file.Length == 0) return BadRequest("No file");

            try
            {
                // Opretter en midlertidig hukommelse (stream)
                using Stream s = new MemoryStream();
                file.CopyTo(s); // Kopierer filens indhold ind i hukommelsen
                s.Position = 0; // starter læsningen fra begyndelsen af filen 

                // Konverterer excel filen til material objekter 
                var materials = MaterialConverter.Convert(s);

                foreach (var m in materials)
                {
                    // Sætter materialer til projekter
                    m.ProjectId = projectId;
                    // Gemmer materialet til db 
                    _repo.Add(m);
                }
                return Ok($"Uploaded {materials.Count} materials."); // Succes besked 
            }
            catch (Exception ex)
            {
                return BadRequest("Error parsing file: " + ex.Message); // Fejlbesked 
            }
        }
    }
}