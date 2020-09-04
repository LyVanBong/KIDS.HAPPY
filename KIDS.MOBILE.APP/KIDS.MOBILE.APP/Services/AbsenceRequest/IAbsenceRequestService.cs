using KIDS.MOBILE.APP.Models.AbsenceRequest;
using KIDS.MOBILE.APP.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.AbsenceRequest
{
    public interface IAbsenceRequestService
    {
        /// <summary>
        /// xac nhan don xin nghi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approve"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<ResponseModel<string>> DetailAbsenceRepuest(string id, string approve, string description);

        /// <summary>
        /// lay tat ca danh sach don xin nghi hoc
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<AbsenceRequestModel>>> GetAllAbsenceRequest(string teacherId, string classId);
    }
}