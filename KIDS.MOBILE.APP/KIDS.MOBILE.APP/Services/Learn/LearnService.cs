using KIDS.MOBILE.APP.Models.Learn;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Learn
{
    public class LearnService : ILearnService
    {
        private IRequestProvider _requestProvider;

        public LearnService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<int>> UpdatePlanStudyMorning(string id, string userCreate, string studyCommentAM)
        {
            try
            {
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("UserCreate",userCreate),
                    new RequestParameter("StudyCommentAM",studyCommentAM),
                };
                var data = await _requestProvider.PostAsync<int>("Daily/UpdateStudyMorning", parameters);
                return data;
            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }

        public async Task<ResponseModel<int>> UpdatePlanStudyAfternoon(string id, string userCreate, string studyCommentPM)
        {
            try
            {
                var parameters = new List<RequestParameter>()
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("UserCreate",userCreate),
                    new RequestParameter("StudyCommentPM",studyCommentPM),
                };
                var data = await _requestProvider.PostAsync<int>("Daily/UpdateHygiene", parameters);
                return data;
            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<LearnModel>>> PlanStudyAfternoon(string date, string classId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("date",date),
                    new RequestParameter("classId",classId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<LearnModel>>("StudyPlan/Afternoon", parameters);
                return data;
            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<LearnModel>>> PlanStudyMorning(string date, string classId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("date",date),
                    new RequestParameter("classId",classId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<LearnModel>>("StudyPlan/Morning", parameters);
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