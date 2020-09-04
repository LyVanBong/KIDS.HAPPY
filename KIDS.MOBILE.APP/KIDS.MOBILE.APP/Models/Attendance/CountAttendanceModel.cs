using Newtonsoft.Json;

namespace KIDS.MOBILE.APP.Models.Attendance
{
    public class CountAttendanceModel
    {
        [JsonProperty("STT")]
        public int STT { get; set; }

        [JsonProperty("CoMat")]
        public int CoMat { get; set; }

        [JsonProperty("NghiCoPhep")]
        public int NghiCoPhep { get; set; }

        [JsonProperty("NghiKhongPhep")]
        public int NghiKhongPhep { get; set; }

        [JsonProperty("DiMuon")]
        public int DiMuon { get; set; }

        public string SiSoHocSinh => $"{CoMat + DiMuon}/{STT}";
    }
}