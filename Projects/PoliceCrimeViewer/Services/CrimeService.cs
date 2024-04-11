﻿using PoliceUk;
using PoliceUk.Entities.StreetLevel;

namespace CrimeViewerBackend.Services
{
    public class CrimeService : ICrimeService
{
        private readonly IPoliceUkClient PoliceClient;
        private static DateTime? CrimesLastUpdated = null;

        public CrimeService()
        {
            PoliceClient = new PoliceUkClient();
        }

        /// <inheritdoc/>
        public DateTime GetLastUpdated()
        {
            // Can't get from Interface so cast to class - Raise as issue on PoliceUKClient Github?
            return ((PoliceUkClient)PoliceClient).LastUpdated();
        }

        /// <summary>
        /// Gets a list of crimes in a 1 mile radius of the location specified
        /// </summary>
        /// <param name="lat">The latitude of the location.</param>
        /// <param name="lng">The longitude of the location.</param>
        /// <param name="month">Optional. The month for which to retrieve crimes. If not specified, retrieves crimes for all months.</param>
        /// <returns><see cref="StreetLevelCrimeResults"/> containing a list of crimes.</returns>
        public StreetLevelCrimeResults GetCrimes(double lat, double lng, int? month = null)
        {
            var geoposition = new Geoposition(lat, lng);
            DateTime? requestedDate = month.HasValue ? ConvertMonthToDateTime(month.Value) : null;

            return PoliceClient.StreetLevelCrimes(geoposition, requestedDate);
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
