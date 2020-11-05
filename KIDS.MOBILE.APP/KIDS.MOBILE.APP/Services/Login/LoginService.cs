using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Login
{
    public class LoginService : ILoginService
    {
        private IRequestProvider _requestProvider;

        public LoginService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<UserModel>> LogiAppByUserPwd(string user, string pass)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("UserName",user),
                    new RequestParameter("PassWord",pass),
                };
                var data = await _requestProvider.PostAsync<UserModel>("login", parameters);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}