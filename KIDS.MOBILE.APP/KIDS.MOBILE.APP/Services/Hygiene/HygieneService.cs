using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Hygiene
{
    public class HygieneService : IHygieneService
    {
        private IRequestProvider _requestProvider;

        public HygieneService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<int>> UpdateHygiene(string id, string userCreate, string hygiene, string hygieneComment)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ID",id),
                    new RequestParameter("UserCreate",userCreate),
                    new RequestParameter("Hygiene",hygiene),
                    new RequestParameter("HygieneComment",hygieneComment),
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
    }
}