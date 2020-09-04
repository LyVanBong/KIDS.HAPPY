using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Models.RequestProvider;
using KIDS.MOBILE.APP.Models.Response;
using KIDS.MOBILE.APP.Services.RequestProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Album
{
    public class AlbumDetailService : IAlbumDetailService
    {
        private IRequestProvider _requestProvider;

        public AlbumDetailService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ResponseModel<IEnumerable<AlbumDetailModel>>> GetAlbumDetail(string AlbumID)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("AlbumID",AlbumID),
                };
                var data = await _requestProvider.GetAsync<IEnumerable<AlbumDetailModel>>("Album/Detail/", parameters);
                return data;
            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }

        public async Task<ResponseModel<int>> InsertImage(InsertImageModel insert)
        {
            try
            {
                var parameters = new List<RequestParameter>
                {
                    new RequestParameter("ClassID",insert.AlbumID),
                    new RequestParameter("Thumbnail",insert.ImageURL),
                    new RequestParameter("Description",insert.Description),
                    new RequestParameter("DateCreate",insert.Sort)
                };
                var data = await _requestProvider.PostAsync<int>("Album/InsertAlbumImage", parameters);
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