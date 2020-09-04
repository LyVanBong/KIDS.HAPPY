using KIDS.MOBILE.APP.Models.News;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.News
{
    public class NewsService : INewsService
    {
        private IRequestProvider _requestProvider;

        public NewsService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<IEnumerable<NewsModel>>> GetAllNews(string classId, string schoolId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("SchoolId",schoolId),
                    new RequestParameter("ClassId",classId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<NewsModel>>("News/Select/All", parameters);
                return data;
            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }

        public async Task<ResponseModel<int>> InsertNews(InsertNewsModel insert)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ClassID",insert.ClassID),
                    new RequestParameter("Title",insert.Title),
                    new RequestParameter("Content",insert.Content),
                    new RequestParameter("ImageUrl",insert.ImageUrl),
                    new RequestParameter("DateCreate",insert.DateCreate),
                    new RequestParameter("UserCreate",insert.UserCreate),
                };
                var data = await _requestProvider.PostAsync<int>("News/Insert", parameters);
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