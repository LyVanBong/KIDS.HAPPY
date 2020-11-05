using KIDS.MOBILE.APP.Configurations;
using KIDS.MOBILE.APP.Models.Notification;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private IRequestProvider _requestProvider;

        public NotificationService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<int>> GetCountNotification()
        {
            try
            {
                var user = AppConstants.User;
                var parameter = new List<RequestParameter>
                {
                    new RequestParameter("classId",user.ClassID),
                    new RequestParameter("shoolId",user.DonVi),
                };
                var data = await _requestProvider.GetAsync<int>("Notification/Count", parameter);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<NotificationModel>>> GetNotification(string classId, string schoolId)
        {
            try
            {
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("ClassId",classId),
                    new RequestParameter("SchoolId",schoolId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<NotificationModel>>("/Notification/All", parameters);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}