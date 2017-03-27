using Newtonsoft.Json;

namespace TechnicalInterviewHelper.WebApi.Model
{
    public class CompetencyViewModel
    {
        [JsonProperty("id")]
        public int CompetencyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}