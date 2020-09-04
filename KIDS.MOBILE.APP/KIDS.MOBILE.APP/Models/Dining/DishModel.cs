using Prism.Mvvm;

namespace KIDS.MOBILE.APP.Models.Dining
{
    public class DishModel : BindableBase
    {
        private string _ten;
        public string ID { get; set; }

        public string Ten
        {
            get => _ten;
            set => SetProperty(ref _ten, value);
        }

        public string Nhom { get; set; }
        public string DonViTinh { get; set; }
        public string DonViTinh2 { get; set; }
        public string GiaTriQuyDoi { get; set; }
        public string DonGia { get; set; }
        public string TyLeThaiBo { get; set; }
        public string DongVat { get; set; }
        public string ThucVat { get; set; }
        public string Calo { get; set; }
        public string Dam { get; set; }
        public string Duong { get; set; }
        public string Beo { get; set; }
        public string BuaAnApDung { get; set; }
        public string Khoi { get; set; }
        public string LaNVL { get; set; }
        public string LaBanHang { get; set; }
        public string MoTa { get; set; }
        public string Ma { get; set; }
        public string Ten1 { get; set; }
        public string DonGiaNhap { get; set; }
        public string DonGiaNhap2 { get; set; }
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Status { get; set; }
        public string Sort { get; set; }
        public string DonGia2 { get; set; }
        public string HeSoPhu { get; set; }
        public string DinhMucTonKhoNhoNhat { get; set; }
        public string DinhMucTonKhoLonNhat { get; set; }
        public string QuanLyTheoSoLoVaHanSuDung { get; set; }
        public string NgungBan { get; set; }
        public string Fix { get; set; }
        public string TonKho { get; set; }
        public string Serial { get; set; }
    }
}