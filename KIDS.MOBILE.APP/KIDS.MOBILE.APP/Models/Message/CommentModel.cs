using KIDS.MOBILE.APP.Configurations;
using System;

namespace KIDS.MOBILE.APP.Models.Message
{
    public class CommentModel
    {
        private string _picture;
        public string CommunicationID { get; set; }
        public string Parent { get; set; }
        public string ClassID { get; set; }
        public string StudentID { get; set; }
        public string TeacherID { get; set; }
        public DateTime DateCreate { get; set; }
        public int Type { get; set; }
        public bool IsConfirmed { get; set; }
        public string Approver { get; set; }
        public string ApproverDate { get; set; }
        public string Content { get; set; }
        public int Views { get; set; }
        public string NguoiGui { get; set; }

        public string Picture
        {
            get => AppConstants.UriBaseWebForm + _picture;
            set => _picture = value;
        }
    }
}