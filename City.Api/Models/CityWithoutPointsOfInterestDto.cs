namespace CityInfo.Api.Models
{
    /// <summary>
    /// Simple list of Cities without the points of interest collections.
    /// This is the default result.
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Description { get; set; }

    }
}
