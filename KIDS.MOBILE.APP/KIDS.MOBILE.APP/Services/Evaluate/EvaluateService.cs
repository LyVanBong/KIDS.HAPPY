using KIDS.MOBILE.APP.Models.Evaluate;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Evaluate
{
    public class EvaluateService : IEvaluateService
    {
        private IRequestProvider _requestProvider;

        public EvaluateService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<int>> UpdateEvaluation(string id, string result)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("Result",result)
                };
                var data = await _requestProvider.PostAsync<int>("Assess/UpdateAssess", parameters);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<StudentAssessmentModel>>> GetStudentAssessment(string assessId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("AssessID",assessId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<StudentAssessmentModel>>("Assess/Select", parameters);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<EvaluationCriteriaModel>>> GetEvaluationCriteria(string assessId, string studentId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("AssessID",assessId),
                    new RequestParameter("StudentID",studentId)
                };
                var data = await _requestProvider.GetAsync<IEnumerable<EvaluationCriteriaModel>>("Assess/Select/Student", parameters);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ResponseModel<IEnumerable<EvaluationBoardModel>>> GetEvaluationBoard(string schoolId, string classId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("SchoolID",schoolId),
                    new RequestParameter("ClassID",classId)
                };
                var data = await _requestProvider.GetAsync<IEnumerable<EvaluationBoardModel>>("Assess/Select/Plan", parameters);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}