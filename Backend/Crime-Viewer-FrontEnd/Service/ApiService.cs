using System.Text.Json;

public class ApiService
{
	private readonly IConfiguration _configuration;
	private readonly HttpClient _httpClient;

	public ApiService(IConfiguration configuration, HttpClient httpClient)
	{
		_configuration = configuration;
		_httpClient = httpClient;
	}

	public async Task<string> GetTest()
	{
		string responseBody = null;
		try
		{
			string baseUrl = _configuration["ApiBaseUrl"];

			var response = await _httpClient.GetAsync($"{baseUrl}crime/test");
			response.EnsureSuccessStatusCode();
			responseBody = await response.Content.ReadAsStringAsync();
		}
		catch (Exception ex)
		{
			responseBody = ex.Message;
		}

		return responseBody;//JsonSerializer.Deserialize<string>(responseBody);
	}
}