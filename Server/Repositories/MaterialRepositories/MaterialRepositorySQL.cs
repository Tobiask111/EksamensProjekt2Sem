using Core;

namespace Server.Repositories.MaterialRepositories;

// Repository til håndtering af projectmaterials i databasen
public class MaterialRepositorySQL : BaseRepository, IMaterialRepository
{
    // Tilføjer et nyt ProjectMaterial til databasen
    public void Add(ProjectMaterial m)
    {
        // Opretter databaseforbindelse, lukkes automatisk når using-blokken afsluttes
        using var conn = GetConnection();
        conn.Open(); // Åbner forbindelsen

        // Opretter en SQL-kommando, lukkes automatisk med using
        using var command = conn.CreateCommand();
        command.CommandText = @"
            INSERT INTO projectmaterials
                (projectid, beskrivelse, kostpris, antal, leverandør, total, avance, dækningsgrad) 
            VALUES 
                (@pid, @besk, @kost, @antal, @lev, @total, @avance, @dg)";

        // Binder værdier fra ProjectMaterial-objektet til SQL-parametre
        // Brug af parametre forhindrer SQL-injection og sikrer korrekt datatype
        command.Parameters.AddWithValue("pid", m.ProjectId);
        // @pid i SQL får værdien af ProjectId fra objektet

        command.Parameters.AddWithValue("besk", m.Beskrivelse ?? (object)DBNull.Value);
        // @besk får værdien af Beskrivelse, hvis null sendes DBNull.Value til databasen

        //osv
        command.Parameters.AddWithValue("kost", m.Kostpris);
        command.Parameters.AddWithValue("antal", m.Antal);
        command.Parameters.AddWithValue("lev", m.Leverandør ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("total", m.Total);
        command.Parameters.AddWithValue("avance", m.Avance);
        command.Parameters.AddWithValue("dg", m.Dækningsgrad);

        // Køre INSERT-kommandoen
        command.ExecuteNonQuery();
    }

    // Henter alle ProjectMaterial til et specifikt projekt
    public List<ProjectMaterial> GetByProjectId(int projectId)
    {
        var list = new List<ProjectMaterial>(); // Liste til resultater

        // Opretter databaseforbindelse
        using var conn = GetConnection();
        conn.Open(); // Åbner forbindelsen

        // Opretter SQL-kommando
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projectmaterials WHERE projectid = @id";
        command.Parameters.AddWithValue("id", projectId); // Parameter for sikkerhed

        // Læser resultater fra databasen
        using var reader = command.ExecuteReader();
        while (reader.Read()) // Læser én række ad gangen
        {
            // Mapper databasekolonner til et ProjectMaterial-objekt
            // Hver række i databasen bliver til et objekt i C#-listen
            list.Add(new ProjectMaterial
            {
                // Henter 'materialsid' kolonnen fra databasen
                // Hvis værdien er NULL, bruges 0 som default
                MaterialsId = Convert.ToInt32(reader["materialsid"] is DBNull ? 0 : reader["materialsid"]),

                // Henter 'projectid' kolonnen, forventes altid at have en værdi
                ProjectId = Convert.ToInt32(reader["projectid"]),

                // Henter 'beskrivelse' kolonnen
                // Hvis NULL, bruges tom streng, ellers konverteres til string
                Beskrivelse = reader["beskrivelse"] == DBNull.Value ? "" : reader["beskrivelse"].ToString(),

                // Henter 'kostpris' kolonnen og konverterer til decimal
                Kostpris = Convert.ToDecimal(reader["kostpris"]),

                //Osv
                Antal = Convert.ToDecimal(reader["antal"]),
                Total = Convert.ToDecimal(reader["total"]),
                Leverandør = reader["leverandør"] == DBNull.Value ? "" : reader["leverandør"].ToString(),
                Dækningsgrad = Convert.ToDecimal(reader["dækningsgrad"]),
            });

        }

        return list; // Returnerer listen med materialer
    }
}
