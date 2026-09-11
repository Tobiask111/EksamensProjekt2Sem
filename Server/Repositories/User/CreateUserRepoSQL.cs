using System.Data.Common;
using Core;                   

namespace Server.Repositories.User
{
    public class CreateUserRepoSQL : BaseRepository, ICreateUserRepo
    {
    public List<Users> GetAll()
        {

            var result = new List<Users>();    // Tom liste til brugere der hentes

            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection(); //Bruger using for at sikre at forbindelsen bliver lukket(Using = sørger for at det bliver lukket)
            mConnection.Open(); // Åbner forbindelsen
            {
                var command = mConnection.CreateCommand();   // Opretter SQL-kommando
                command.CommandText = @"SELECT * FROM Users"; // SQL der henter alle brugere
                
                using (var reader = command.ExecuteReader())  // Kører SELECT og får en "reader" (Læser kolloner) 
                {
                    while (reader.Read()) // Læser én række ad gangen
                    {
                        // Henter værdier fra kolonnerne i rækken
                        var userid = reader.GetInt32(0);
                        var username = reader.GetString(1);
                        var password = reader.GetString(2);
                        var role = reader.GetString(3);

                        // Mapper database-værdier til et Users-objekt (lægger data ind i C# objekt)
                        Users u = new Users
                        {
                            UserId = userid,
                            UserName = username,
                            Password = password,
                            Role = role
                        };
                        result.Add(u); // Tilføjer bruger til listen
                    }
                }
            }

            return result; // Returnerer hele listen
        }

        public void Add(Users user)
        {
            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection();
            mConnection.Open(); // Åbner forbindelsen
            {

                var command = mConnection.CreateCommand(); // Ny SQL-kommando

                // SQL med parametre
                command.CommandText =
                    "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";

                Console.WriteLine(command.CommandText);

                // Opretter parameter for @username
                var paramName = command.CreateParameter();
                paramName.ParameterName = "username"; // Navn der matcher @username
                command.Parameters.Add(paramName); // Tilføjer til SQL-kommandos parameterliste
                paramName.Value = user.UserName; // Sætter værdien

                // Opretter parameter for @password
                var paramPassword = command.CreateParameter();
                paramPassword.ParameterName = "password";
                command.Parameters.Add(paramPassword);
                paramPassword.Value = user.Password;

                // Opretter parameter for @role
                var paramRole = command.CreateParameter();
                paramRole.ParameterName = "role";
                command.Parameters.Add(paramRole);
                paramRole.Value = user.Role;

                command.ExecuteNonQuery();   // Eksekverer INSERT (kører SQL-kommandoen)
            }
        }

        public Users? ValidateUser(string username, string password)
        {
            // Finder første bruger der matcher username og password (efter GetAll har hentet alle)
            return GetAll().FirstOrDefault(u => u.UserName == username && u.Password == password);
        }

        public void Delete(int id)
        {
            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection();
            mConnection.Open(); // Åbner forbindelsen
            {
                var command = mConnection.CreateCommand(); // Ny SQL-kommando

                command.CommandText = $"DELETE FROM users WHERE userid={id}"; // SQL der sletter ud fra id

                command.ExecuteNonQuery();   // Kører DELETE-kommandoen
            }
        }
    }
}
