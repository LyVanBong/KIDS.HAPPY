using KIDS.MOBILE.APP.Models.Response;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Hygiene
{
    public interface IHygieneService
    {
        /// <summary>
        /// cập nhật vệ sinh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userCreate"></param>
        /// <param name="hygiene"></param>
        /// <param name="hygieneComment"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> UpdateHygiene(string id, string userCreate, string hygiene, string hygieneComment);
    }
}