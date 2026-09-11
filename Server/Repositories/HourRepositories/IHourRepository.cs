using Core;

namespace Server.Repositories.HourRepositories;

// Interface for HourRepositorySQL
// Definerer hvilke metoder en repository til ProjectHour skal implementere
public interface IHourRepository
{
    // Metode til at tilføje en ProjectHour til databasen
    // Implementeringen skal oprette en SQL-INSERT og eksekvere den
    void Add(ProjectHour hour);

    // Metode til at hente alle ProjectHour for et bestemt projekt
    // Returnerer en liste med ProjectHour-objekter
    List<ProjectHour> GetByProjectId(int projectId);
}
