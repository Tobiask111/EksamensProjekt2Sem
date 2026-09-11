using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.User;

namespace ServerApp.Controllers
{
    [ApiController] // Fortæller at denne klasse er en Web API controller
    [Route("api/user")] // Base-URL for alle endpoints i denne controller jcsxbkvjsbdvkjbs
    public class UserController : ControllerBase
    {

        private ICreateUserRepo userRepo; // Interface til repository (controlleren afhænger kun af interfacet)

        public UserController(ICreateUserRepo userRepo)
        {
            this.userRepo = userRepo; // Dependency Injection: Systemet giver controlleren et repository
        }

        [HttpGet] // Endpoint: GET api/user
        public IEnumerable<Users> Get()
        {
            return userRepo.GetAll(); // Henter alle brugere via repository
        }

        [HttpPost] // Endpoint: POST api/user
        public void Add(Users user)
        {
            userRepo.Add(user); // Kalder repo for at tilføje en ny bruger
        }

        [HttpPost("login")] // Endpoint: POST api/user/login
        public ActionResult<Users> Login([FromBody] Login dto)
        {
            // Validerer brugernavn og password gennem repo
            var user = userRepo.ValidateUser(dto.UserName, dto.Password);

            if (user == null)
                return Unauthorized(); // Returnerer HTTP 401 hvis login mislykkes

            return Ok(user); // Returnerer HTTP 200 + brugerdata hvis login lykkes
        }

        [HttpDelete]  // Endpoint: DELETE api/user/delete/{id}
        [Route("delete/{id:int}")] // Route med variabel id, kun int
        public void Delete(int id)
        {
            userRepo.Delete(id); // Sletter bruger via repository
        }
    }
}
