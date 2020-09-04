using KIDS.MOBILE.APP.Configurations;
using Newtonsoft.Json;
using System;

namespace KIDS.MOBILE.APP.Models.Attendance
{
    public class AttendanceComeModel
    {
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("StudentID")]
        public string StudentID { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("CoMat")]
        public bool CoMat { get; set; }

        [JsonProperty("NghiCoPhep")]
        public bool NghiCoPhep { get; set; }

        [JsonProperty("NghiKhongPhep")]
        public bool NghiKhongPhep { get; set; }

        [JsonProperty("DiMuon")]
        public bool DiMuon { get; set; }

        [JsonProperty("Hygiene")]
        public string Hygiene { get; set; }

        [JsonProperty("Leave")]
        public string Leave { get; set; }

        public string TmpLeave => DateTime.Parse(Leave).ToString("hh:mm");

        [JsonProperty("LeaveLate")]
        public bool LeaveLate { get; set; }

        [JsonProperty("NguoiDon")]
        public string NguoiDon { get; set; }

        [JsonProperty("SleepFrom")]
        public string SleepFrom { get; set; }

        [JsonProperty("SleepTo")]
        public string SleepTo { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("UserCreate")]
        public string UserCreate { get; set; }

        [JsonProperty("MealComment0")]
        public string MealComment0 { get; set; }

        [JsonProperty("MealComment1")]
        public string MealComment1 { get; set; }

        [JsonProperty("MealComment2")]
        public string MealComment2 { get; set; }

        [JsonProperty("MealComment3")]
        public string MealComment3 { get; set; }

        [JsonProperty("MealComment4")]
        public string MealComment4 { get; set; }

        [JsonProperty("MealComment5")]
        public string MealComment5 { get; set; }

        [JsonProperty("ArriveComment")]
        public string ArriveComment { get; set; }

        [JsonProperty("LeaveComment")]
        public string LeaveComment { get; set; }

        [JsonProperty("StudyCommentAM")]
        public string StudyCommentAM { get; set; }

        [JsonProperty("StudyCommentPM")]
        public string StudyCommentPM { get; set; }

        [JsonProperty("SleepComment")]
        public string SleepComment { get; set; }

        [JsonProperty("HygieneComment")]
        public string HygieneComment { get; set; }

        [JsonProperty("OverallComment")]
        public string OverallComment { get; set; }

        [JsonProperty("DayComment")]
        public string DayComment { get; set; }

        [JsonProperty("PhieuBeNgoan")]
        public string PhieuBeNgoan { get; set; }

        [JsonProperty("WeekComment")]
        public string WeekComment { get; set; }

        [JsonProperty("WeekPhieuBeNgoan")]
        public string WeekPhieuBeNgoan { get; set; }

        [JsonProperty("WeekCommentCate")]
        public string WeekCommentCate { get; set; }

        [JsonProperty("ClassID")]
        public string ClassID { get; set; }

        [JsonProperty("DiemKiemTraID")]
        public string DiemKiemTraID { get; set; }

        [JsonProperty("Diem1")]
        public string Diem1 { get; set; }

        [JsonProperty("Diem2")]
        public string Diem2 { get; set; }

        [JsonProperty("Diem3")]
        public string Diem3 { get; set; }

        [JsonProperty("Diem4")]
        public string Diem4 { get; set; }

        [JsonProperty("DiemTB")]
        public string DiemTB { get; set; }

        [JsonProperty("NhanXetKiemTra")]
        public string NhanXetKiemTra { get; set; }

        [JsonProperty("NickName")]
        public string NickName { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Picture")]
        public string Picture { get; set; }

        public string TmpPicture => AppConstants.UriBaseWebForm + Picture;

        [JsonProperty("ClassName")]
        public string ClassName { get; set; }

        public string CommentCome { get; set; }
    }
}