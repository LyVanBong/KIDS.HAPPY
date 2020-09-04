using KIDS.MOBILE.APP.Models.Learn;
using KIDS.MOBILE.APP.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Learn
{
    public interface ILearnService
    {
        /// <summary>
        /// cap nhat hoc sang
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userCreate"></param>
        /// <param name="studyCommentAM"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> UpdatePlanStudyMorning(string id, string userCreate, string studyCommentAM);

        /// <summary>
        /// cap nhat hoc chieu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userCreate"></param>
        /// <param name="studyCommentPM"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> UpdatePlanStudyAfternoon(string id, string userCreate, string studyCommentPM);

        /// <summary>
        /// danh sach học buổi chiều
        /// </summary>
        /// <param name="date"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<LearnModel>>> PlanStudyAfternoon(string date, string classId);

        /// <summary>
        /// danh sách học buôi sáng
        /// </summary>
        /// <param name="date"></param>
        /// <param name="classId"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<LearnModel>>> PlanStudyMorning(string date, string classId);
    }
}