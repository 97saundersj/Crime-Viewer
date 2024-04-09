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

        [HttpGet("test")]
        public string GetTest()
        {
            return "Working!";
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

		public static DateTime ConvertMonthToDateTime(int monthNumber)
		{
			int currentYear = DateTime.Now.Year;
			int currentMonth = DateTime.Now.Month;

			if (monthNumber > currentMonth - 2)
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
