using PoliceUk.Entities.StreetLevel;
using System.Text.Json;

public class ApiService
{
	private readonly IConfiguration _configuration;
	private readonly HttpClient _httpClient;
	private readonly string? _baseUrl;

	public ApiService(IConfiguration configuration, HttpClient httpClient)
	{
		_configuration = configuration;
		_httpClient = httpClient;

		_baseUrl = _configuration["ApiBaseUrl"];
	}

	public async Task<DateTime> GetLastUpdated()
	{
		try
		{
			var response = await _httpClient.GetAsync($"{_baseUrl}crime/lastupdated");
			response.EnsureSuccessStatusCode();
			var responseBody = await response.Content.ReadAsStringAsync();

			// Deserialize the response into StreetLevelCrimeResults object
			// TODO: Move PropertyNameCaseInsensitive into Program?
			var summary = JsonSerializer.Deserialize<DateTime>(responseBody, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});
			return summary;
		}
		catch (Exception ex)
		{
			throw new Exception("Failed to retrieve crime summary data from the API.", ex);
		}
	}

	public async Task<StreetLevelCrimeResults> GetCrimeSummary(double latitude, double longitude, int? month = null)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}crime/crimes?lat={latitude}&lng={longitude}&month={month}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

			// Deserialize the response into StreetLevelCrimeResults object
			// TODO: Move PropertyNameCaseInsensitive into Program?
			var summary = JsonSerializer.Deserialize<StreetLevelCrimeResults>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return summary;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to retrieve crime summary data from the API.", ex);
        }
    }
}