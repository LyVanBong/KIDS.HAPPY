using KIDS.MOBILE.APP.Configurations;
using Newtonsoft.Json;

namespace KIDS.MOBILE.APP.Models.News
{
    public class NewsModel
    {
        [JsonProperty("NewsID")]
        public string NewsID { get; set; }

        [JsonProperty("ClassID")]
        public string ClassID { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("ImageURL")]
        public string ImageURL { get; set; }

        [JsonProperty("URL")]
        public string URL { get; set; }

        [JsonProperty("DateCreate")]
        public string DateCreate { get; set; }

        [JsonProperty("UserCreate")]
        public string UserCreate { get; set; }

        [JsonProperty("Approve")]
        public string Approve { get; set; }

        [JsonProperty("NgayTao")]
        public string NgayTao { get; set; }

        [JsonProperty("TenNguoiTao")]
        public string TenNguoiTao { get; set; }

        [JsonProperty("TenLop")]
        public string TenLop { get; set; }

        /// <summary>
        /// đường dẫn ảnh
        /// </summary>
        public string ImgTmp => AppConstants.UriBaseWebForm + ImageURL;
    }
}