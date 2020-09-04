using KIDS.MOBILE.APP.Configurations;
using Prism.Mvvm;
using System;

namespace KIDS.MOBILE.APP.Models.User
{
    public class StudentModel : BindableBase
    {
        private string _comment;
        private string _startTime;
        private string _endTime;
        public string StudentID { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public bool? Sex { get; set; }
        public string DOB { get; set; }
        public string TmpDOB => DateTime.Parse(DOB).ToString("dd/MM/yyyy");
        public string ClassID { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public string FatherPhone { get; set; }
        public string MotherPhone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string HKTT { get; set; }
        public string Picture { get; set; }
        public string TmpPicture => AppConstants.UriBaseWebForm + Picture;
        public string DonVi { get; set; }
        public string Nation { get; set; }
        public string Nationality { get; set; }
        public string Description { get; set; }
        public string DoiTuongChinhSach { get; set; }
        public string DoiTuongHoTro { get; set; }
        public string DateCreate { get; set; }
        public string UserCreate { get; set; }
        public string Status { get; set; }
        public string DateStatus { get; set; }
        public string Nguon { get; set; }
        public string NguoiGioiThieu { get; set; }
        public string TruongHoc { get; set; }
        public string NganhNghe { get; set; }
        public string TrinhDoChuyenMon { get; set; }
        public string NhanVien { get; set; }
        public string Type { get; set; }
        public string ClassName { get; set; }
        public string SchoolName { get; set; }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        #region An uong

        public string StartTime
        {
            get => _startTime == null ? "12:00" : _startTime;
            set => SetProperty(ref _startTime, value);
        }

        public string EndTime
        {
            get => _endTime == null ? "14:15" : _endTime;
            set => SetProperty(ref _endTime, value);
        }

        #endregion An uong
    }
}