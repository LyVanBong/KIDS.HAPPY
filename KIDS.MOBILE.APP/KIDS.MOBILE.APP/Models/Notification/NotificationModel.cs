using KIDS.MOBILE.APP.Configurations;
using Newtonsoft.Json;
using System;

namespace KIDS.MOBILE.APP.Models.Notification
{
    public class NotificationModel
    {
        [JsonProperty("Picture")]
        public string Picture { get; set; }

        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("Views")]
        public object Views { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        public string DateTmp => DateTime.Parse(Date).ToString("dd/MM/yyyy");
        public string ImageNotification => AppConstants.UriBaseWebForm + Picture;
    }
}