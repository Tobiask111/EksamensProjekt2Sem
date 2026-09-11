using Core;

namespace Server.Repositories.HourRepositories;

// Repository til håndtering af timer (ProjectHour) i databasen
public class HourRepositorySQL : BaseRepository, IHourRepository
{
    // Tilføjer en ny ProjectHour til databasen
    public void Add(ProjectHour h)
    {
        // Opretter databaseforbindelse, lukker automatisk når using-blokken afsluttes
        using var conn = GetConnection();
        conn.Open(); // Åbner forbindelsen

        // Opretter en SQL-kommando, lukker automatisk med using
        using var command = conn.CreateCommand();
        command.CommandText = @"
            INSERT INTO projecthours
                (projectid, medarbejder, dato, stoptid, timer, type, kostpris) 
            VALUES 
                (@pid, @med, @dato, @stop, @timer, @type, @kost)";

        // Binder værdier fra ProjectHour-objektet til SQL-parametre
        // Parametre beskytter mod SQL-injection og sikrer korrekt datatype
        command.Parameters.AddWithValue("pid", h.ProjectId);
        // pid for værdien af projectid

        command.Parameters.AddWithValue("med", h.Medarbejder ?? (object)DBNull.Value); // Hvis null så for den null som værdi
        command.Parameters.AddWithValue("dato", h.Dato ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("stop", h.Stoptid ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("timer", h.Timer);
        command.Parameters.AddWithValue("type", h.Type ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("kost", h.Kostpris);

        // Eksekverer INSERT-kommandoen
        command.ExecuteNonQuery();
    }

    // Henter alle ProjectHour for et bestemt projekt
    public List<ProjectHour> GetByProjectId(int projectId)
    {
        var list = new List<ProjectHour>(); // Liste til at gemme resultater

        // Opretter databaseforbindelse
        using var conn = GetConnection();
        conn.Open(); // Åbner forbindelsen

        // Opretter SQL-kommando
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projecthours WHERE projectid = @id";
        command.Parameters.AddWithValue("id", projectId); // Parameter for sikkerhed

        // Læser resultater fra databasen
        using var reader = command.ExecuteReader();
        while (reader.Read()) // Læser én række ad gangen
        {
            // Mapper databasekolonner til ProjectHour-objekt
            list.Add(new ProjectHour
            {
                // Håndterer NULL eller manglende ID-kolonne med default 0
                HourId = Convert.ToInt32(reader["hourid"] is DBNull ? 0 : reader["hourid"]),

                ProjectId = Convert.ToInt32(reader["projectid"]),

                // Hvis medarbejder er NULL, bruges standard "Ukendt"
                Medarbejder = reader["medarbejder"] == DBNull.Value ? "Ukendt" : reader["medarbejder"].ToString(),

                // Dato og stoptid håndteres som nullable DateTime
                Dato = reader["dato"] == DBNull.Value ? null : Convert.ToDateTime(reader["dato"]),
                Stoptid = reader["stoptid"] == DBNull.Value ? null : Convert.ToDateTime(reader["stoptid"]),

                Timer = Convert.ToDecimal(reader["timer"]), // decimal fra databasen
                Type = reader["type"] == DBNull.Value ? "" : reader["type"].ToString(), // tom streng hvis NULL
                Kostpris = Convert.ToDecimal(reader["kostpris"]),
            });
        }

        // Returnerer listen med ProjectHour-objekter
        return list;
    }
}
