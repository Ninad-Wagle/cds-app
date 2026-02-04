using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CdsFrontend.Services;

public class BackendApiService
{
    private readonly HttpClient _http;

    public class CdsStep
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public async Task<List<CdsStep>> GetStepsAsync()
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await _http.GetFromJsonAsync<List<CdsStep>>("/api/cds/steps");
#pragma warning restore CS8603 // Possible null reference return.
    }

    public BackendApiService(HttpClient http)   
    {
        _http = http;
    }

    public async Task<string> PingAsync()
    {
        return await _http.GetStringAsync("/ping");
    }
}
