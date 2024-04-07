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

	public async Task<string> GetTest()
	{
		string responseBody;
		try
		{
			var response = await _httpClient.GetAsync($"{_baseUrl}crime/test");
			response.EnsureSuccessStatusCode();
			responseBody = await response.Content.ReadAsStringAsync();
		}
		catch (Exception ex)
		{
			responseBody = "Error retreving data";
		}

		return responseBody;//JsonSerializer.Deserialize<string>(responseBody);
	}
}