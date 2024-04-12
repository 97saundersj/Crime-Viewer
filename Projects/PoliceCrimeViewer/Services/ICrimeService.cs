using PoliceUk.Entities.StreetLevel;

namespace CrimeViewerBackend.Services
{
    public interface ICrimeService
{
        /// <summary>
        /// Get the Date that the crime data was last updated at.
        /// </summary>
        /// <returns><see cref="DateTime"/> representing the last updated date.</returns>
        public DateTime GetLastUpdated();

        /// <summary>
        /// Gets a list of crimes in a 1 mile radius of the location specified
        /// </summary>
        /// <param name="lat">The latitude of the location.</param>
        /// <param name="lng">The longitude of the location.</param>
        /// <param name="month">Optional. The month for which to retrieve crimes. If not specified, retrieves crimes for all months.</param>
        /// <returns><see cref="StreetLevelCrimeResults"/> containing a list of crimes.</returns>
        public StreetLevelCrimeResults GetCrimes(double lat, double lng, int? month = null);

    }
}
