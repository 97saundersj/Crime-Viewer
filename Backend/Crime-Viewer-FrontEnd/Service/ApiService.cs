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
		string baseUrl = null;
		try
		{
			baseUrl = _configuration["ApiBaseUrl"];

			var response = await _httpClient.GetAsync($"{baseUrl}crime/test");
			response.EnsureSuccessStatusCode();
			responseBody = await response.Content.ReadAsStringAsync();
		}
		catch (Exception ex)
		{
			responseBody = ex.Message;
			responseBody += "\n" + $"{baseUrl}crime/test";
			responseBody += ex.InnerException;
		}

		return responseBody;//JsonSerializer.Deserialize<string>(responseBody);
	}
}