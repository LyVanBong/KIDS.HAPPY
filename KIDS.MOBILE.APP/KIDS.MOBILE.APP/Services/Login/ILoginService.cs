using KIDS.MOBILE.APP.Models.Login;
using KIDS.MOBILE.APP.Models.Response;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Login
{
    public interface ILoginService
    {
        Task<ResponseModel<UserModel>> LogiAppByUserPwd(string user, string pass);
    }
}