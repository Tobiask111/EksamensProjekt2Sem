using Core;

namespace Server.Repositories.User
{
    // Interface som beskriver hvilke metoder et "User Repository" skal have
    // Formål er at være en mellem mand imellem repo og controller
    public interface ICreateUserRepo
    {
        List<Users> GetAll();   // Skal kunne hente alle brugere
        void Add(Users user);   // Skal kunne tilføje en ny bruger
        void Delete(int id);    // Skal kunne slette en bruger ud fra ID
        Users? ValidateUser(string username, string password);  // Skal kunne validere login
    }
}
