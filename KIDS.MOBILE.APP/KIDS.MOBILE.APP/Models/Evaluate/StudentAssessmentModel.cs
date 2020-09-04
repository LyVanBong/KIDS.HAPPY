using System;
using System.Globalization;
using KIDS.MOBILE.APP.Configurations;
using Prism.Mvvm;

namespace KIDS.MOBILE.APP.Models.Evaluate
{
    public class StudentAssessmentModel : BindableBase
    {
        private string _picture;
        private double _tyLe;
        public string StudentID { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }

        public string Picture
        {
            get
            {
                return AppConstants.UriBaseWebForm + _picture;
            }
            set => SetProperty(ref _picture, value);
        }

        public string ClassName { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }

        public double TyLe
        {
            get => Math.Round(_tyLe, 0);
            set => SetProperty(ref _tyLe, value);
        }

    }
}