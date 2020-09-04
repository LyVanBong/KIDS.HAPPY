using KIDS.MOBILE.APP.Models.Assessment;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Assessment
{
    public class AssessmentService : IAssessmentService
    {
        private IRequestProvider _requestProvider;
        public AssessmentService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<ResponseModel<int>> DailyAssessmentAdd(AssessmentModel obj)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                new RequestParameter("ID",obj.ID),
                new RequestParameter("UserCreate",obj.UserCreate),
                new RequestParameter("PhieuBeNgoan",obj.PhieuBeNgoan),
                new RequestParameter("DayComment",obj.Comment)
                };
                var data = await _requestProvider.PostAsync<int>("Daily/UpdateAssessment_Day", parameters);
                return data;

            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }
        public async Task<ResponseModel<int>> WeaklyAssessmentAdd(AssessmentModel obj)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                new RequestParameter("ID",obj.ID),
                new RequestParameter("UserCreate",obj.UserCreate),
                new RequestParameter("WeekPhieuBeNgoan",obj.PhieuBeNgoan),
                new RequestParameter("WeekComment",obj.Comment)
                };
                var data = await _requestProvider.PostAsync<int>("Daily/UpdateAssessment_Week", parameters);
                return data;

            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }
    }
}
