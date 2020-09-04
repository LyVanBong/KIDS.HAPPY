using KIDS.MOBILE.APP.Configurations;

namespace KIDS.MOBILE.APP.Models.User
{
    public class ParentModel
    {
        public string ID { get; set; }
        public int Type { get; set; }
        public string StudentID { get; set; }
        public string Name { get; set; }
        public bool Sex { get; set; }
        public string DOB { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Relationship { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string TmpPicture => AppConstants.UriBaseWebForm + Picture;
    }
}