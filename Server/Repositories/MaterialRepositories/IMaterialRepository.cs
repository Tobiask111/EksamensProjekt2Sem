using Core;

namespace Server.Repositories.MaterialRepositories;

// Interface for MaterialRepositorySQL
// Definerer hvilke metoder en MaterialRepository skal implementere
public interface IMaterialRepository
{
    // Metode til at tilføje et ProjectMaterial til databasen
    // Implementeringen vil normalt oprette en SQL-INSERT og eksekvere den
    void Add(ProjectMaterial material);

    // Metode til at hente alle ProjectMaterialer for et specifikt projekt
    // Returnerer en liste med ProjectMaterial-objekter
    List<ProjectMaterial> GetByProjectId(int projectId);
}
