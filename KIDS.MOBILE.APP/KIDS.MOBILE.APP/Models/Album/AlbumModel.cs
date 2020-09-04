using KIDS.MOBILE.APP.Configurations;
using Newtonsoft.Json;
using System;

namespace KIDS.MOBILE.APP.Models.Album
{
    public class AlbumModel
    {
        [JsonProperty("AlbumID")]
        public string AlbumID { get; set; }

        [JsonProperty("ClassID")]
        public string ClassID { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("DateCreate")]
        public DateTime DateCreate { get; set; }

        [JsonProperty("UserCreate")]
        public string UserCreate { get; set; }

        [JsonProperty("NgayTao")]
        public string NgayTao { get; set; }

        [JsonProperty("TenNguoiTao")]
        public string TenNguoiTao { get; set; }

        [JsonProperty("TenLop")]
        public string TenLop { get; set; }

        public string UriImage => AppConstants.UriBaseWebForm + Thumbnail;
        public string AlbumDescription => TenLop + " - " + NgayTao;
    }
}