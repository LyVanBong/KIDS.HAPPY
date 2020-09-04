using KIDS.MOBILE.APP.Models.AbsenceRequest;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.AbsenceRequest
{
    public class AbsenceRequestService : IAbsenceRequestService
    {
        private IRequestProvider _requestProvider;

        public AbsenceRequestService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<string>> DetailAbsenceRepuest(string id, string approve, string description)
        {
            try
            {
                var parameter = new List<RequestParameter>
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("Status","true"),
                    new RequestParameter("Approver",approve),
                    new RequestParameter("Description",description),
                };
                var data = await _requestProvider.PostAsync<string>("Application/Approve", parameter);
                return data;
            }
            catch (Exception e)
            {
                throw;
                return null;
            }
        }

        public Task<ResponseModel<IEnumerable<AbsenceRequestModel>>> GetAllAbsenceRequest(string teacherId, string classId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("TeacherId",teacherId),
                    new RequestParameter("ClassId",classId)
                };
                var data = _requestProvider.GetAsync<IEnumerable<AbsenceRequestModel>>("Leave", parameters);
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