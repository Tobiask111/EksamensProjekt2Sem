using Core;
using Npgsql;
namespace Server.Repositories.ProjectRepositories;


// Repository der håndterer projekter i databasen
public class ProjectRepositorySQL : BaseRepository, IProjectRepository
{
    // Opretter et nyt projekt i databasen og returnerer det nye projectid
    public int Create(Project pro)
    {
        using var conn = GetConnection(); // Henter databaseforbindelse
        conn.Open(); // Åbner forbindelsen

        using var command = conn.CreateCommand(); // Opretter SQL-kommando
        command.CommandText = @"
            INSERT INTO projects
                (name, billedeurl, datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbejdsmand_timepris) 
            VALUES 
                (@name, @billedeurl, @datecreated, @svend, @lærling, @konsulent, @arbejdsmand)          
            RETURNING projectid"; // Returnerer ID på det nye projekt

        // Parametre sender værdier sikkert til SQL
        command.Parameters.AddWithValue("name", pro.Name);
        command.Parameters.AddWithValue("billedeurl", pro.ImageUrl);
        command.Parameters.AddWithValue("datecreated", pro.DateCreated);
        command.Parameters.AddWithValue("svend", pro.SvendTimePris);
        command.Parameters.AddWithValue("lærling", pro.LærlingTimePris);
        command.Parameters.AddWithValue("konsulent", pro.KonsulentTimePris);
        command.Parameters.AddWithValue("arbejdsmand", pro.ArbejdsmandTimePris);

        return (int)command.ExecuteScalar()!; // Kører INSERT og får projectid tilbage
    }

    // Opdaterer et eksisterende projekt
    public void Update(Project p)
    {
        using var conn = GetConnection(); // Henter databaseforbindelse
        conn.Open();

        using var command = conn.CreateCommand();
        command.CommandText = @"
            UPDATE projects SET 
                name = @name, 
                billedeurl = @billedeurl,
                svend_timepris = @svend, 
                lærling_timepris = @lærling, 
                konsulent_timepris = @konsulent, 
                arbejdsmand_timepris = @arbejdsmand
            WHERE projectid = @id"; // Opdaterer projekt ud fra ID

        // Parametre med nye værdier
        command.Parameters.AddWithValue("id", p.ProjectId);
        command.Parameters.AddWithValue("name", p.Name ?? "");
        command.Parameters.AddWithValue("billedeurl", p.ImageUrl ?? "");
        command.Parameters.AddWithValue("svend", p.SvendTimePris);
        command.Parameters.AddWithValue("lærling", p.LærlingTimePris);
        command.Parameters.AddWithValue("konsulent", p.KonsulentTimePris);
        command.Parameters.AddWithValue("arbejdsmand", p.ArbejdsmandTimePris);

        command.ExecuteNonQuery();            // Kører UPDATE-kommandoen
    }

    // Sletter et projekt ud fra ID
    public void Delete(int id)
    {
        using var conn = GetConnection();    // Henter databaseforbindelse
        conn.Open();

        using var command = conn.CreateCommand();
        command.CommandText = "DELETE FROM projects WHERE projectid = @id"; // SQL delete
        command.Parameters.AddWithValue("id", id);

        command.ExecuteNonQuery(); // Kører DELETE-kommandoen
    }

    // Henter ét projekt ud fra ID
    public Project? GetById(int id)
    {
        using var conn = GetConnection(); // Henter databaseforbindelse
        conn.Open();

        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projects WHERE projectid = @id";
        command.Parameters.AddWithValue("id", id);

        using var reader = command.ExecuteReader(); // Kører SELECT og læser resultatet
        if (reader.Read()) // Hvis der findes en række
        {
            return MapProject(reader); // Mapper DB-data til Project-objekt
        }
        return null; // Returnerer null hvis intet projekt findes
    }

    // Henter alle projekter
    public IEnumerable<Project> GetAll()
    {
        var list = new List<Project>();

        using var conn = GetConnection();
        conn.Open();

        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projects ORDER BY datecreated DESC";

        using var reader = command.ExecuteReader(); // Kører SELECT
        while (reader.Read())  // Læser én række ad gangen
        {
            list.Add(MapProject(reader)); // Mapper og tilføjer til listen
        }
        return list; // Returnerer alle projekter
    }

    // Hjælpemetode: laver et Project-objekt ud fra en database-række
    // Denne metode tager én række fra databasen (reader)
    // og laver den om til et Project-objekt i C#
    private Project MapProject(NpgsqlDataReader reader)
    {
        return new Project
        {
            // Henter projectid fra databasen og laver det om til int
            ProjectId = Convert.ToInt32(reader["projectid"]),

            // Henter navnet på projektet
            // Hvis værdien i databasen er NULL, bruges "Ukendt" i stedet
            Name = reader["name"] == DBNull.Value
                ? "Ukendt"
                : reader["name"].ToString()!,

            //Ligsom før men med dato
            DateCreated = reader["datecreated"] == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(reader["datecreated"]),

            ImageUrl = reader["billedeurl"] == DBNull.Value
                ? string.Empty
                : reader["billedeurl"].ToString()!,

            SvendTimePris = reader["svend_timepris"] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader["svend_timepris"]),

            LærlingTimePris = reader["lærling_timepris"] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader["lærling_timepris"]),

            KonsulentTimePris = reader["konsulent_timepris"] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader["konsulent_timepris"]),

            ArbejdsmandTimePris = reader["arbejdsmand_timepris"] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader["arbejdsmand_timepris"])
        };
    }

}
