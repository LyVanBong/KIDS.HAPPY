using KIDS.MOBILE.APP.Configurations;
using Newtonsoft.Json;
using Realms;
using System;

namespace KIDS.MOBILE.APP.Models.User
{
    public class TeacherModel
    {
        [PrimaryKey]
        [JsonProperty("TeacherID")]
        public string TeacherID { get; set; }

        [JsonProperty("Type")]
        public int Type { get; set; }

        [JsonProperty("DoiTuong")]
        public string DoiTuong { get; set; }

        [JsonProperty("ClassID")]
        public string ClassID { get; set; }

        [JsonProperty("PositionID")]
        public object PositionID { get; set; }

        [JsonProperty("NickName")]
        public string NickName { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Sex")]
        public byte Sex { get; set; }

        [JsonProperty("DOB")]
        public DateTime DOB { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("HKTT")]
        public string HKTT { get; set; }

        [JsonProperty("Nation")]
        public string Nation { get; set; }

        [JsonProperty("DonVi")]
        public string DonVi { get; set; }

        [JsonProperty("ChucVu")]
        public string ChucVu { get; set; }

        [JsonProperty("VaiTro")]
        public string VaiTro { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("Tel")]
        public string Tel { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("LaDangVien")]
        public bool LaDangVien { get; set; }

        [JsonProperty("NgayVaoDang")]
        public string NgayVaoDang { get; set; }

        [JsonProperty("Picture")]
        public string Picture { get; set; }

        [JsonProperty("UserCreate")]
        public string UserCreate { get; set; }

        [JsonProperty("DateIn")]
        public string DateIn { get; set; }

        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonProperty("DateStatus")]
        public object DateStatus { get; set; }

        [JsonProperty("NickUser")]
        public string NickUser { get; set; }

        [JsonProperty("ClassName")]
        public string ClassName { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

        [JsonProperty("SchoolName")]
        public string SchoolName { get; set; }

        public string AvatarTeacher => string.Concat(AppConstants.UriBaseWebForm, Picture);
    }
}