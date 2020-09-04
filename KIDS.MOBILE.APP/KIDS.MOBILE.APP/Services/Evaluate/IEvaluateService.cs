using System.Collections.Generic;
using System.Threading.Tasks;
using KIDS.MOBILE.APP.Models.Evaluate;
using KIDS.MOBILE.APP.Models.Response;

namespace KIDS.MOBILE.APP.Services.Evaluate
{
    public interface IEvaluateService
    {
        /// <summary>
        /// Cập nhật kết quả đánh giá của mỗi học sinh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> UpdateEvaluation(string id, string result);
        /// <summary>
        /// Danh sách  học sinh trong bảng đánh giá
        /// </summary>
        /// <param name="assessId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<StudentAssessmentModel>>> GetStudentAssessment(string assessId);
        /// <summary>
        /// Danh sách chỉ tiêu đánh giá của mỗi học sinh
        /// </summary>
        /// <param name="assessId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<EvaluationCriteriaModel>>> GetEvaluationCriteria(string assessId, string studentId);
        /// <summary>
        /// Danh mục Bảng kế hoạch đánh giá
        /// </summary>
        /// <param name="schoolId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<EvaluationBoardModel>>> GetEvaluationBoard(string schoolId, string classId);
    }
}