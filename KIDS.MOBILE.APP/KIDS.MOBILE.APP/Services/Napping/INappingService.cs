using KIDS.MOBILE.APP.Models.Response;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Napping
{
    public interface INappingService
    {
        /// <summary>
        /// Cập nhật học sinh ngủ trưa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userCreate"></param>
        /// <param name="sleepFrom"></param>
        /// <param name="sleepTo"></param>
        /// <param name="sleepComment"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> UpdateNapping(string id, string userCreate, string sleepFrom, string sleepTo, string sleepComment);
    }
}