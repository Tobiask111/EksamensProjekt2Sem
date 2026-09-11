using Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Service
{
    public class UserRepository
    {
        private readonly HttpClient _http;

        public UserRepository(HttpClient http)
        {
            _http = http;
        }
        
        public async Task<Users?> ValidLoginAsync(string name, string password)
        {
            // Pakker brugernavn og password ind i et objekt
            var login = new { UserName = name, Password = password };
            HttpResponseMessage response;

            try
            {
                // Dataene bliver sendt til "api/user/login"
                response = await _http.PostAsJsonAsync("api/user/login", login);
            }
            catch (Exception)
            {
                // Hvis der sker fejl returneres null, så hele programmet ikke går ned
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                // Hvis serveren godkender, laves det til et User-objekt.
                var user = await response.Content.ReadFromJsonAsync<Users>();
                return user;
            }
            
            return null;
        }
    }
}