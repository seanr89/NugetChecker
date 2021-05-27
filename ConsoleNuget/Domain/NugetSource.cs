
namespace Domain
{
    /// <summary>
    /// Breakdown of a singular nuget source that could be selected by the user
    /// converted to a record to reduce complexity! (class is not needed here!)
    /// </summary>
    public record NugetSource
    {
        public string Name { get; set; }
        public string URL { get; set; }

    }
}