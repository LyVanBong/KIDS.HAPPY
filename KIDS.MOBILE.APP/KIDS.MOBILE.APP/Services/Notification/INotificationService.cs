using KIDS.MOBILE.APP.Models.Notification;
using KIDS.MOBILE.APP.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Notification
{
    public interface INotificationService
    {
        /// <summary>
        /// lấy số thông báo chưa xem
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<int>> GetCountNotification();

        /// <summary>
        /// lấy danh sách thông báo
        /// </summary>
        Task<ResponseModel<IEnumerable<NotificationModel>>> GetNotification(string classId, string schoolId);
    }
}