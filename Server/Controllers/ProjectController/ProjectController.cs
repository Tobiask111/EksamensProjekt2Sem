using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.ProjectRepositories;
using Server.Service;

namespace Server.Controllers.ProjectController
{
    [ApiController] // Gør klassen til en API-controller
    [Route("api/project")] // Base endpoint
    public class ProjectController : ControllerBase 
    {
        private readonly IProjectRepository _projectRepo;  // Adgang til projekter
        private readonly ProjectCalculationsService _calcService; // Projektberegninger

        public ProjectController(IProjectRepository projectRepo, ProjectCalculationsService calcService) 
        {
            _projectRepo = projectRepo;
            _calcService = calcService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetAll()  // Henter alle projekter
        {
            return Ok(_projectRepo.GetAll()); // Returnerer projektliste
        }

        [HttpGet("{id}")] // Henter detaljer for et projekt
        public ActionResult<Calculation> GetDetails(int id) // Beregner projekt

        {
            var result = _calcService.CalculateProject(id);
            if (result == null) return NotFound("Project not found"); // Hvis projekt ikke findes
            return Ok(result);  // Returnerer beregning
        }

        [HttpPost]// Opretter nyt projekt
        public IActionResult Create(Project pro)
        {
            int newId = _projectRepo.Create(pro); // Gemmer projekt
            return Ok(newId);  // Returnerer nyt id
        }

        [HttpPut("{id}")] // Opdaterer eksisterende projekt
        public IActionResult Update(int id, [FromBody] Project pro)
        {
            if (id != pro.ProjectId) return BadRequest("ID mismatch"); // Tjekker id matcher
            _projectRepo.Update(pro);  // Opdaterer projekt
            return Ok();
        }

        [HttpDelete("{id}")] // Sletter projekt
        public void Delete(int id)
        {
            _projectRepo.Delete(id);   // Fjerner projektet med givne id 
        }
    }
}