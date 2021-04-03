using Xamarin.Essentials;

namespace KIDS.MOBILE.APP.Models.User
{
    public class UpdateTeacherModel
    {
        public string TeacherId { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public FileResult Picture { get; set; }
    }
}