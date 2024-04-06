using Microsoft.AspNetCore.Mvc;
using PoliceUk.Entities.StreetLevel;
using PoliceUk;

namespace CrimeSummaryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrimeController : ControllerBase
    {
        private readonly IPoliceUkClient PoliceClient;

        public CrimeController(IPoliceUkClient policeClient)
        {
            PoliceClient = policeClient;
        }

        [HttpGet]
        public StreetLevelCrimeResults GetCrimeSummary(double lat, double lng, int? month = null)
        {
            try
            {
                var geoposition = new Geoposition(lat, lng);
                DateTime? requestedDate = month.HasValue ? DateTime.Now.AddMonths(-month.Value) : null;

                var streetLevelCrimes = PoliceClient.StreetLevelCrimes(geoposition, requestedDate);

                return streetLevelCrimes;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve crime data from Police UK API.", ex);
            }
        }
    }
}
