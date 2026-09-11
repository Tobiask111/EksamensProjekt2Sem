using Npgsql;
using Server.PW1;

namespace Server.Repositories;

// Baseklasse som andre repositories arver fra
// Abstract class fungere som en skabelon (giver funktionalitet)
public abstract class BaseRepository
{
    // Connection string til online PostgreSQL-database (Neon)
    protected string ConnectionString =>
        // 1. Online database-server og port (Server)
        // 2. Brugernavn til databasen (UserId)
        // 3. Password hentes fra PASSWORD-klassen
        // 4. Databasens navn
        // 5. Kræver krypteret forbindelse (nødvendigt for online database)
        @"Server=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech:5432;
          User Id=neondb_owner;
          Password=" + PASSWORD.PW1 + @";
          Database=LarsenInstallation;
          Ssl Mode=Require;";

    // Opretter en ny databaseforbindelse
    // Forbindelsen åbnes først, når den bruges
    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(ConnectionString);
    }
}
