using System;

namespace KIDS.MOBILE.APP.Models.Dining
{
    public class ListOfDishesModel
    {
        public string ID { get; set; }
        public string ThucDon { get; set; }
        public string Khoi { get; set; }
        public int NgayThu { get; set; }
        public DateTime NgayThangTheoThu { get; set; }
        public object ThucDonTuan { get; set; }
        public string BuaAn { get; set; }
        public string MonAn { get; set; }
        public string SchoolID { get; set; }
        public string TenMonAn { get; set; }
        public string TenBuaAn { get; set; }
    }
}