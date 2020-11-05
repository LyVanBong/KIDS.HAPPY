using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Napping
{
    public class NappingService : INappingService
    {
        private IRequestProvider _requestProvider;

        public NappingService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<int>> UpdateNapping(string id, string userCreate, string sleepFrom, string sleepTo, string sleepComment)
        {
            try
            {
                var parameter = new List<RequestParameter>()
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("UserCreate",userCreate),
                    new RequestParameter("SleepFrom",sleepFrom),
                    new RequestParameter("SleepTo",sleepTo),
                    new RequestParameter("SleepComment",sleepComment),
                };
                var data = await _requestProvider.PostAsync<int>("Daily/UpdateSleep", parameter);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}