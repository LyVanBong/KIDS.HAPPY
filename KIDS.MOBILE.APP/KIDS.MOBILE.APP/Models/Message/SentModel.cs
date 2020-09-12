using KIDS.MOBILE.APP.Configurations;
using Prism.Mvvm;
using System;

namespace KIDS.MOBILE.APP.Models.Message
{
    public class SentModel : BindableBase
    {
        private string _picture;
        private string _dateCreate;
        private bool _isConfirmed;
        public string CommunicationID { get; set; }
        public string StudentID { get; set; }
        public string TeacherID { get; set; }

        public string DateCreate
        {
            get => _dateCreate;
            set
            {
                if (SetProperty(ref _dateCreate, value))
                {
                    RaisePropertyChanged(TmpDateCreate);
                }
            }
        }

        public string TmpDateCreate => DateTime.Parse(DateCreate).ToString("dd/MM/yyyy");
        public int Type { get; set; }
        public string Content { get; set; }

        public bool IsConfirmed
        {
            get => _isConfirmed;
            set => SetProperty(ref _isConfirmed, value);
        }

        public string NguoiGui { get; set; }

        public string Picture
        {
            get => AppConstants.UriBaseWebForm + _picture;
            set => SetProperty(ref _picture, value);
        }

        public string Approver { get; set; }
        public string ApproverDate { get; set; }
        public string ClassID { get; set; }
    }
}