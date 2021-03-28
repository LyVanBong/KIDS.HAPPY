using KIDS.MOBILE.APP.Models.Album;
using KIDS.MOBILE.APP.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KIDS.MOBILE.APP.Services.Album
{
    public interface IAlbumDetailService
    {
        /// <summary>
        /// Lấy chi tiết một album
        /// </summary>
        /// <param name="idAlbum"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        Task<ResponseModel<IEnumerable<AlbumDetailModel>>> GetAlbumDetail(string idAlbum, string idUser);

        /// <summary>
        /// Thêm hình ảnh vào album
        /// </summary>
        /// <param name="insert"></param>
        /// <returns></returns>
        Task<ResponseModel<int>> InsertImage(InsertImageModel insert);
    }
}