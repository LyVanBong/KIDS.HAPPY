using Newtonsoft.Json;

namespace KIDS.MOBILE.APP.Models.Prescription
{
    public class PrescriptionDetailModel
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("PrescriptionID")]
        public string PrescriptionID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Sort")]
        public int Sort { get; set; }
    }
}