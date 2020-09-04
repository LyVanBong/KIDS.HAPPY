using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Resources;
using Newtonsoft.Json;
using Realms;

namespace KIDS.MOBILE.APP.Models.Login
{
    public class UserModel : RealmObject
    {
        [PrimaryKey]
        [JsonProperty("NguoiSuDung")]
        public string NguoiSuDung { get; set; }

        [JsonProperty("NickName")]
        public string NickName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("USERID")]
        public string UserId { get; set; }

        [JsonProperty("isTeacher")]
        public string IsTeacher { get; set; }

        [JsonProperty("LoaiGiaoVien")]
        public string LoaiGiaoVien { get; set; }

        [JsonProperty("TenGV")]
        public string TenGV { get; set; }

        [JsonProperty("Picture")]
        public string Picture { get; set; }

        [JsonProperty("DonVi")]
        public string DonVi { get; set; }

        [JsonProperty("ClassID")]
        public string ClassID { get; set; }

        [JsonProperty("ClassName")]
        public string ClassName { get; set; }

        [JsonProperty("GradeID")]
        public string GradeID { get; set; }

        [JsonProperty("GradeName")]
        public string GradeName { get; set; }

        /// <summary>
        /// anh dai dien cua giao vien
        /// </summary>
        [Ignored]
        public string AvatarTeacher => string.Concat(AppConstants.UriBaseWebForm, Picture);

        /// <summary>
        /// lời chào giao viên
        /// </summary>
        [Ignored]
        public string WebcomeToTeacher => string.Format(AppResources._00025, TenGV.ToUpper());

        /// <summary>
        /// tên lớp đầy đủ
        /// </summary>
        [Ignored]
        public string ClassNameFull => $"{AppResources._00026} {ClassName}";

        /// <summary>
        /// Châm ngôn của lớp học
        /// </summary>
        [Ignored]
        public string ClassNote => "Coi học trò như con, để dậy bảo !";
    }
}