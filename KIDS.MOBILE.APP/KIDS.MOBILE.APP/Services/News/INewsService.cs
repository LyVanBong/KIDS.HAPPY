using KIDS.MOBILE.APP.Models.News;
using KIDS.MOBILE.APP.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.News
{
    public interface INewsService
    {
        /// <summary>
        /// Get all news
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="schoolId"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<NewsModel>>> GetAllNews(string classId, string schoolId);

        /// <summary>
        /// insert new News
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> InsertNews(InsertNewsModel insert);
    }
}