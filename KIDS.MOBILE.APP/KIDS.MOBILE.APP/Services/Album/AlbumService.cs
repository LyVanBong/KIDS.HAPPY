using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Album
{
    public class AlbumService : IAlbumService
    {
        private IRequestProvider _requestProvider;

        public AlbumService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<IEnumerable<AlbumModel>>> GetAllAlbum(string classId, string schoolId)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("SchoolId",schoolId),
                    new RequestParameter("ClassId",classId),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<AlbumModel>>("Album/Select/All", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        public async Task<ResponseModel<int>> InsertAlbum(InsertAlbumModel insert)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ClassID",insert.ClassID),
                    new RequestParameter("Thumbnail",insert.Thumbnail),
                    new RequestParameter("Description",insert.Description),
                    new RequestParameter("DateCreate",insert.DateCreate),
                    new RequestParameter("UserCreate",insert.UserCreate)
                };
                var data = await _requestProvider.PostAsync<int>("Album/InsertAlbum", parameters);
                return data;
            }
            catch (Exception)
            {
                
                return null;
            }
        }
    }
}