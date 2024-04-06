using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoliceCrimeViewer;

namespace CrimeSummaryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrimeSummaryController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        //private readonly HttpMessageHandler _httpMessageHandler;

        public CrimeSummaryController(HttpClient httpClient)
        {
            //_httpMessageHandler = httpMessageHandler;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<List<CrimeSummary>> GetCrimeSummary(double lat, double lng, int month)
        {
            try
            {
                using (_httpClient)//(var httpClient = new HttpClient(_httpMessageHandler))
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime requestedDate = new DateTime(currentDate.Year - (month <= currentDate.Month ? 0 : 1), month, 1);

                    string crimeDataUrl = $"https://data.police.uk/api/crimes-street/all-crime?lat={lat}&lng={lng}&date={requestedDate:yyyy-MM}";
                    var response = await _httpClient.GetAsync(crimeDataUrl);

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(response.StatusCode.ToString());

                    var crimeData = JsonConvert.DeserializeObject<List<Crime>>(await response.Content.ReadAsStringAsync());

                    // Group crimes by category
                    var crimeSummary = crimeData.GroupBy(c => c.Category)
                                                .Select(group => new CrimeSummary(group.Key, group.Count())).ToList();

                    return crimeSummary;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Crime
    {
        public string Category { get; set; }
        // Add other properties as needed
    }
}
