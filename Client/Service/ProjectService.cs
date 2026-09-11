using Core;
using System.Net.Http.Json;

namespace Client.Service
{
    public class ProjectService
    {
        private readonly HttpClient _http;

        
        public async Task<Calculation?> GetProjectDetails(int id)
        {
            try
            {
                // Kalder endpointet: api/project/5 (hvis id er 5)
                return await _http.GetFromJsonAsync<Calculation>($"api/project/{id}");
            }
            catch (Exception)
            {
                // Hvis projektet ikke findes eller der er fejl, returner null
                return null;
            }
        }
    }
}