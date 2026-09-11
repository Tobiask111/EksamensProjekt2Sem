using Core;
namespace Server.Repositories.ProjectRepositories;

// Interface som beskriver hvilke metoder et Project-repository skal have
public interface IProjectRepository
{
    int Create(Project p); // Skal kunne oprette et projekt og returnere projektets ID
    void Update(Project p); // Skal kunne opdatere et eksisterende projekt
    void Delete(int id); // Skal kunne slette et projekt ud fra ID
    Project? GetById(int id); // Skal kunne hente et projekt ud fra ID (eller null)
    IEnumerable<Project> GetAll(); // Skal kunne hente alle projekter
}