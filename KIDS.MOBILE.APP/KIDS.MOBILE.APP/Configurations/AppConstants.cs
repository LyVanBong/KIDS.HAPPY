using KIDS.MOBILE.APP.Models.Login;
using System;

namespace KIDS.MOBILE.APP.Configurations
{
    public static class AppConstants
    {
        /// <summary>
        /// chon ngay
        /// </summary>
        internal static string ChoosedDate = "ChoosedDate";
        /// <summary>
        /// xoa tin nhan
        /// </summary>
        internal static string DeleteMessage = "DeleteMessage";
        /// <summary>
        /// binh luan tin nhan
        /// </summary>
        internal static string CommentMessage = "Comment";
        /// <summary>
        /// sua xoa tin nhan
        /// </summary>
        internal static string EditAndDeleteMessage = "EditAndDelete";
        /// <summary>
        /// danh sach chi tieu danh gia cua hoc sinh
        /// </summary>
        internal static string EvaluationCriteria = "EvaluationCriteria";
        /// <summary>
        /// check pop mang co dang mo hay khong
        /// </summary>
        internal static bool IsOpenPopupNetwork;

        /// <summary>
        /// kiểm tra kết nỗi Internet
        /// </summary>
        internal static bool HasInternet;

        /// <summary>
        /// ID của thông báo đang chọn
        /// </summary>
        internal static string NotificationID = "NotificationID";

        /// <summary>
        /// chon thoi gian
        /// </summary>
        internal static string TimeSelect = "TimePicker";

        /// <summary>
        /// chi tiết student
        /// </summary>
        internal static string StudentDetail = "StudentDetail";

        /// <summary>
        /// Chi tiết hình ảnh
        /// </summary>
        internal static string ImageDetail = "ImageDetail";

        /// <summary>
        /// chi tiet don thuoc
        /// </summary>
        internal static string DetailMedicine = "DetailMedicine";

        /// <summary>
        /// chi tiet tin tuc
        /// </summary>
        internal static string DetailAlbums = "DetailAlbums";

        /// <summary>
        /// Chi tiet album
        /// </summary>
        internal static string DetailNews = "DetailNews";

        /// <summary>
        /// tin tuc
        /// </summary>
        internal static string DataNews = "NewsPost";

        /// <summary>
        /// lưu thông tin user khi login thành công.
        /// </summary>
        internal static UserModel User = new UserModel();

        /// <summary>
        /// uri web form
        /// </summary>
        internal static string UriBaseWebForm = "http://school.hkids.edu.vn";

        /// <summary>
        /// config db realm
        /// </summary>
        internal static string RealmConfiguration = "KIDS.MOBILE.APP.BSOFTGROUP.realm";

        /// <summary>
        /// kiêm tra người dung có lưu tài khoản không
        /// </summary>
        internal static string SaveAccountUser = "SaveAccountUser";

        /// <summary>
        /// Dữ liệu khi user login thành công
        /// </summary>
        internal static string UserLogin = "UserLogin";

        /// <summary>
        /// base url api in application
        /// </summary>
        internal static string UrlApiApp = "http://api.hkids.edu.vn/api/v1/";
    }
}