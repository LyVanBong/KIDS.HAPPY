using KIDS.MOBILE.APP.Configurations;
using Newtonsoft.Json;

namespace KIDS.MOBILE.APP.Models.Album
{
    public class AlbumDetailModel
    {
        [JsonProperty("ImageID")]
        public string ImageID { get; set; }

        [JsonProperty("AlbumID")]
        public string AlbumID { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("ImageURL")]
        public string ImageURL { get; set; }

        [JsonProperty("DateCreate")]
        public string DateCreate { get; set; }

        [JsonProperty("Sort")]
        public int Sort { get; set; }

        public string UriImage => AppConstants.UriBaseWebForm + ImageURL;
    }
}