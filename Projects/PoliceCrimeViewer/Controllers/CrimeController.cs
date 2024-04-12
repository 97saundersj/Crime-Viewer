using Microsoft.AspNetCore.Mvc;
using PoliceUk.Entities.StreetLevel;
using CrimeViewerBackend.Services;

namespace CrimeSummaryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrimeController : ControllerBase
    {
        private readonly ICrimeService CrimeService;

        public CrimeController(ICrimeService crimeService)
        {
            CrimeService = crimeService;
        }

        [HttpGet("lastUpdated")]
        public DateTime GetLastUpdated()
        {
            try
            {
                return CrimeService.GetLastUpdated();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve crime data from Police UK API.", ex);
            }
        }

        [HttpGet("crimes")]
        public StreetLevelCrimeResults GetCrimes(double lat, double lng, int? month = null)
        {
            try
            {
                return CrimeService.GetCrimes(lat, lng, month);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve crime data from Police UK API.", ex);
            }
        }
    }
}
