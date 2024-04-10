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

        private static DateTime? CrimesLastUpdated = null;

        public CrimeController(IPoliceUkClient policeClient)
        {
            PoliceClient = policeClient;
        }

        [HttpGet("lastUpdated")]
        public DateTime GetLastUpdated()
        {
            try
            {
                // Can't get from Interface so cast to class - Raise as issue on PoliceUKClient Github?
                CrimesLastUpdated = ((PoliceUkClient)PoliceClient).LastUpdated();
                return CrimesLastUpdated.Value;

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve crime data from Police UK API.", ex);
            }
        }

        [HttpGet("summary")]
        public StreetLevelCrimeResults GetCrimeSummary(double lat, double lng, int? month = null)
        {
            try
            {
                var geoposition = new Geoposition(lat, lng);
                DateTime? requestedDate = month.HasValue ? ConvertMonthToDateTime(month.Value) : null;

                var streetLevelCrimes = PoliceClient.StreetLevelCrimes(geoposition, requestedDate);

                return streetLevelCrimes;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve crime data from Police UK API.", ex);
            }
        }

        private DateTime UpdateLastUpdated()
        {
            try
            {
                // Can't get from Interface so cast to class - Raise as issue on PoliceUKClient Github?
                CrimesLastUpdated = ((PoliceUkClient)PoliceClient).LastUpdated();
                return CrimesLastUpdated.Value;

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve crime data from Police UK API.", ex);
            }
        }

        private DateTime ConvertMonthToDateTime(int monthNumber)
        {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            if (!CrimesLastUpdated.HasValue)
            {
                UpdateLastUpdated();

            }
            if (monthNumber > CrimesLastUpdated.Value.Month)
            {
                // The requested month is in the past, use the previous year
                return new DateTime(currentYear - 1, monthNumber, 1);
            }
            else
            {
                // The requested month is in the current year
                return new DateTime(currentYear, monthNumber, 1);
            }
        }
    }
}
