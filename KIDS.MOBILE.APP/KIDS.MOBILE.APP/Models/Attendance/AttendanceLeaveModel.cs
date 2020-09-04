using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Resources;
using Prism.Mvvm;
using System;

namespace KIDS.MOBILE.APP.Models.Attendance
{
    public class AttendanceLeaveModel : BindableBase
    {
        private string _parentName;
        private string _startTime;
        private string _sleepFrom;
        private string _sleepTo;
        private string _hygiene;
        private bool _isSelected;
        public string ID { get; set; }
        public string StudentID { get; set; }
        public string Date { get; set; }
        public bool CoMat { get; set; }
        public bool NghiCoPhep { get; set; }
        public bool NghiKhongPhep { get; set; }
        public bool DiMuon { get; set; }

        public string Hygiene
        {
            get => _hygiene == null ? "0" : _hygiene;
            set => SetProperty(ref _hygiene, value);
        }

        public string Leave { get; set; }
        public string TmpLeave => DateTime.Parse(Leave).ToString("hh:mm");
        public bool LeaveLate { get; set; }

        public string NguoiDon { get; set; }

        public string SleepFrom
        {
            get => DateTime.Parse(_sleepFrom).ToString("HH:mm");
            set => SetProperty(ref _sleepFrom, value);
        }

        public string SleepTo
        {
            get => DateTime.Parse(_sleepTo).ToString("HH:mm");
            set => SetProperty(ref _sleepTo, value);
        }

        public string Description { get; set; }
        public string UserCreate { get; set; }
        public string MealComment0 { get; set; }
        public string MealComment1 { get; set; }
        public string MealComment2 { get; set; }
        public string MealComment3 { get; set; }
        public string MealComment4 { get; set; }
        public string MealComment5 { get; set; }
        public string ArriveComment { get; set; }
        public string LeaveComment { get; set; }
        public string StudyCommentAM { get; set; }
        public string StudyCommentPM { get; set; }
        public string SleepComment { get; set; }
        public string HygieneComment { get; set; }
        public string OverallComment { get; set; }
        public string DayComment { get; set; }
        public string PhieuBeNgoan { get; set; }
        public string WeekComment { get; set; }
        public string WeekPhieuBeNgoan { get; set; }
        public string WeekCommentCate { get; set; }
        public string ClassID { get; set; }
        public string DiemKiemTraID { get; set; }
        public string Diem1 { get; set; }
        public string Diem2 { get; set; }
        public string Diem3 { get; set; }
        public string Diem4 { get; set; }
        public string DiemTB { get; set; }
        public string NhanXetKiemTra { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string TmpPicture => AppConstants.UriBaseWebForm + Picture;
        public string ClassName { get; set; }
        public string CommentLeave { get; set; }

        public string ParentName
        {
            get => _parentName = _parentName == null ? AppResources._00078 : _parentName;
            set => SetProperty(ref _parentName, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}