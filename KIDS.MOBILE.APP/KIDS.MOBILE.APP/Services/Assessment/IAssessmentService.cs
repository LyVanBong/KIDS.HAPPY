using KIDS.MOBILE.APP.Models.Assessment;
using KIDS.MOBILE.APP.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Assessment
{
    public interface IAssessmentService
    {
        /// <summary>
        /// Thêm nhận xét ngày
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<int>> DailyAssessmentAdd(AssessmentModel obj);
        /// <summary>
        /// Thêm nhận xét tuần
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel<int>> WeaklyAssessmentAdd(AssessmentModel obj);
    }
}
