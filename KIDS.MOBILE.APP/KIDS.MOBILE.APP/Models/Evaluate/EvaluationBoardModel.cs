using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KIDS.MOBILE.APP.Models.Evaluate
{
    public class EvaluationBoardModel
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}